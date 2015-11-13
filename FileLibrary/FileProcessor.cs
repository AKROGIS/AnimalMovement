using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using DataModel;
using Telonics;

namespace FileLibrary
{
    public static class FileProcessor
    {
        #region Public API

        /// <summary>
        /// Find and process all of the Argos or DataLog files that need full or partial processing
        /// </summary>
        /// <param name="handler">Delegate to handle exceptions on each file, so that processing can continue.
        /// If the handler is null, processing will stop on first exception.
        /// The handler can throw it's own exception to halt further processing</param>
        /// <param name="pi">A project investigator.
        /// If provided, only files owned by or in projects managed by the given PI will be processed
        /// If null, then all files needing processing will be processed</param>
        public static void ProcessAll(Action<Exception, CollarFile, ArgosPlatform> handler = null,
                                      ProjectInvestigator pi = null)
        {
            var database = new AnimalMovementDataContext();
            var views = new AnimalMovementViews();
            foreach (var fileId in
                views.NeverProcessedArgosFiles.Select(x => x.FileId)
                     .Concat(views.NeverProcessedDataLogFiles.Select(x => x.FileId)))
            {
                var file = database.CollarFiles.First(f => f.FileId == fileId);
                if (pi != null && file.ProjectInvestigator != pi && (file.Project == null ||
                                                                     file.Project.ProjectInvestigator1 != pi))
                    continue;
                try
                {
                    ProcessFile(file);
                }
                catch (Exception ex)
                {
                    if (handler == null)
                        throw;
                    handler(ex, file, null);
                }
            }
            foreach (var item in views.PartiallyProcessedArgosFiles)
            {
                var file = database.CollarFiles.First(f => f.FileId == item.FileId);
                var platform = database.ArgosPlatforms.First(p => p.PlatformId == item.PlatformId);
                if (pi != null && file.ProjectInvestigator != pi && (file.Project == null ||
                                                                     file.Project.ProjectInvestigator1 != pi))
                    continue;
                try
                {
                    ProcessFile(file, platform);
                }
                catch (Exception ex)
                {
                    if (handler == null)
                        throw;
                    handler(ex, file, platform);
                }
            }
        }


        /// <summary>
        /// Full or partial processing of Telonics data in an Argos file
        /// </summary>
        /// <param name="file">The collar file with Argos data to processes.  Must not be null</param>
        /// <param name="platform">The Argos platform in the file to process.
        /// If this is null, then all the platforms will be processed.</param>
        public static void ProcessFile(CollarFile file, ArgosPlatform platform = null)
        {
            if (file == null)
                throw new ArgumentNullException("file", "No collar file was provided to process.");
            if (!ProcessOnServer(file, platform))
                if (file.Format == 'H')
                    ProcessDataLogFile(file);
                else
                    ProcessFile(file, GetArgosFile(file), platform);
        }

        #endregion

        #region private methods

        private static void ProcessDataLogFile(CollarFile file)
        {
            LogGeneralMessage(String.Format("Start local processing of file {0}", file.FileId));
            var databaseFunctions = new AnimalMovementFunctions();
            databaseFunctions.ArgosFile_ClearProcessingResults(file.FileId);

            var processor = new Gen4Processor(null);
            var lines = processor.ProcessDataLog(file.Contents.ToArray());
            //Add a newline, so that it matches the exactly matches the direct output from TDC
            var data = Encoding.UTF8.GetBytes(String.Join(Environment.NewLine, lines) + Environment.NewLine);
            var filename = Path.GetFileNameWithoutExtension(file.FileName) + "_" + DateTime.Now.ToString("yyyyMMdd") + ".csv";
            var fileLoader = new FileLoader(filename, data)
            {
                Project = file.Project,
                Owner = file.ProjectInvestigator,
                Collar = new Collar
                {
                    CollarManufacturer = file.CollarManufacturer,
                    CollarId = file.CollarId
                },
                Status = file.Status,
                ParentFileId = file.FileId,
                AllowDuplicates = true
            };
            fileLoader.Load();
            LogGeneralMessage("Finished local processing of file");
        }

        private static void ProcessFile(CollarFile file, ArgosFile argos, ArgosPlatform platform)
        {
            LogGeneralMessage(String.Format("Start local processing of file {0}", file.FileId));
            var databaseFunctions = new AnimalMovementFunctions();
            if (platform == null)
            {
                databaseFunctions.ArgosFile_ClearProcessingResults(file.FileId);
                if (IsArgosAwsFileIncomplete(argos as ArgosAwsFile) ?? false)
                    LogIssueForFile(file.FileId,
                                    "The Argos server could not return all the data requested and this file is incomplete.");
            }
            else
            {
                databaseFunctions.ArgosFile_UnProcessPlatform(file.FileId, platform.PlatformId);
            }

            var transmissionsByPlatform = from transmission in argos.GetTransmissions()
                                          group transmission by transmission.PlatformId
                                          into transmissions
                                          where platform == null || transmissions.Key == platform.PlatformId
                                          select transmissions;

            foreach (var transmissionSet in transmissionsByPlatform)
                try
                {
                    ProcessTransmissions(file, argos, transmissionSet);
                }
                catch (Exception ex)
                {
                    var message = String.Format("ERROR {0} adding Argos {1} transmissions",
                                                ex.Message, transmissionSet.Key);
                    LogIssueForFile(file.FileId, message, transmissionSet.Key);
                }
            LogGeneralMessage("Finished local processing of file");
        }


        private static void ProcessTransmissions(CollarFile file, ArgosFile argos,
                                                 IGrouping<string, ArgosTransmission> group)
        {
            LogGeneralMessage(String.Format("  Start processing transmissions for Argos Id {0}", group.Key));
            var platformId = group.Key;
            var transmissions = group.ToList();
            var first = transmissions.Min(t => t.DateTime);
            var last = transmissions.Max(t => t.DateTime);
            var databaseViews = new AnimalMovementViews();
            var parameterSets =
                databaseViews.GetTelonicsParametersForArgosDates(platformId, first, last)
                             .OrderBy(c => c.StartDate)
                             .ToList();
            if (parameterSets.Count == 0)
            {
                var msg = String.Format("No Collar for ArgosId {0} from {1:g} to {2:g}",
                                        platformId, first, last);
                LogIssueForFile(file.FileId, msg, platformId, null, null, first, last);
                return;
            }
            if (parameterSets[0].StartDate != null && first < parameterSets[0].StartDate)
            {
                var msg = String.Format("No Collar for ArgosId {0} from {1:g} to {2:g}",
                                        platformId, first, parameterSets[0].StartDate);
                LogIssueForFile(file.FileId, msg, platformId, null, null, first, parameterSets[0].StartDate);
            }
            int lastIndex = parameterSets.Count - 1;
            if (parameterSets[lastIndex].EndDate != null && parameterSets[lastIndex].EndDate < last)
            {
                var msg = String.Format("No Collar for ArgosId {0} from {1:g} to {2:g}",
                                        platformId, parameterSets[lastIndex].EndDate, last);
                LogIssueForFile(file.FileId, msg, platformId, null, null, parameterSets[lastIndex].EndDate, last);
            }
            foreach (var parameterSet in parameterSets)
            {
                if (parameterSet.ParameterId == null ||
                    (parameterSet.CollarModel == "Gen3" && parameterSet.Gen3Period == null) ||
                    (parameterSet.CollarModel == "Gen4" && parameterSet.Format == null))
                {
                    var start = parameterSet.StartDate ?? first;
                    var end = parameterSet.EndDate ?? last;
                    var msg = String.Format("No Telonics Parameters for Collar {0}/{3} from {1:g} to {2:g}",
                                            parameterSet.CollarManufacturer, start, end, parameterSet.CollarId);
                    LogIssueForFile(file.FileId, msg, platformId, parameterSet.CollarManufacturer, parameterSet.CollarId, start, end);
                    continue;
                }
                try
                {
                    ProcessParameterSet(file, argos, first, last, transmissions, parameterSet);
                }
                catch (Exception ex)
                {
                    var message = String.Format(
                        "ERROR {0} adding Argos {1} transmissions from {2:g} to {3:g} to Collar {4}/{5}",
                        ex.Message, parameterSet.PlatformId, first, last,
                        parameterSet.CollarManufacturer, parameterSet.CollarId);
                    LogIssueForFile(file.FileId, message, parameterSet.PlatformId, parameterSet.CollarManufacturer,
                                    parameterSet.CollarId, first, last);
                }
            }
            LogGeneralMessage("  Finished processing transmissions");
        }


        private static void ProcessParameterSet(CollarFile file, ArgosFile argos, DateTime first, DateTime last,
                                                IEnumerable<ArgosTransmission> transmissions,
                                                GetTelonicsParametersForArgosDatesResult parameters)
        {
            LogGeneralMessage(String.Format("    Start processing collar {0}/{1}", parameters.CollarManufacturer,
                                            parameters.CollarId));
            var start = parameters.StartDate ?? DateTime.MinValue;
            if (start < first)
                start = first;
            var end = parameters.EndDate ?? DateTime.MaxValue;
            if (last < end)
                end = last;
            var processor = GetProcessor(file, parameters);
            var transmissionSubset = transmissions.Where(t => start <= t.DateTime && t.DateTime <= end);
            var lines = processor.ProcessTransmissions(transmissionSubset, argos);
            var data = Encoding.UTF8.GetBytes(String.Join(Environment.NewLine, lines) + Environment.NewLine);
            var filename = Path.GetFileNameWithoutExtension(file.FileName) + "_" + parameters.CollarId + ".csv";
            var fileLoader = new FileLoader(filename, data)
                {
                    Project = file.Project,
                    Owner = file.ProjectInvestigator,
                    Collar = new Collar {CollarManufacturer = parameters.CollarManufacturer,
                                         CollarId = parameters.CollarId},
                    Status = file.Status,
                    ParentFileId = file.FileId,
                    ArgosDeploymentId = parameters.DeploymentId,
                    CollarParameterId = parameters.ParameterId,
                    AllowDuplicates = true
                };
            fileLoader.Load();
            var message =
                String.Format(
                    "    Successfully added Argos {0} transmissions from {1:g} to {2:g} to Collar {3}/{4}",
                    parameters.PlatformId, start, end,
                    parameters.CollarManufacturer, parameters.CollarId);
            LogGeneralMessage(message);
        }


        #region Get Processor

        private static IProcessor GetProcessor(CollarFile file, GetTelonicsParametersForArgosDatesResult parameters)
        {
            switch (file.Format)
            {
                case 'E':
                case 'F':
                    switch (parameters.CollarModel)
                    {
                        case "Gen3":
                            return GetGen3Processor(parameters);
                        case "Gen4":
                            return GetGen4Processor(parameters);
                        case "GPS8000":
                            return GetGps8000Processor(parameters);
                        default:
                            throw new InvalidOperationException("Unsupported collar model '" + parameters.CollarModel +
                                                                "'. (supported models are Gen3, and Gen4)");
                    }
                case 'G':
                    return new DebevekProcessor();
                default:
                    throw new InvalidOperationException("Unsupported CollarFile Format '" + file.Format +
                                                        "'. (supported formats are E,F,G)");
            }
        }

        private static IProcessor GetGen3Processor(GetTelonicsParametersForArgosDatesResult parameters)
        {
            if (parameters.Gen3Period == null)
                throw new InvalidOperationException("Unknown period for Gen3 collar");
            if (parameters.Format == 'B')
                throw new InvalidOperationException(
                    "GPS Fixes for Gen3 collars with PTT parameter files (*.ppf) must be processed with Telonics ADC-T03.");
            return new Gen3Processor(TimeSpan.FromMinutes(parameters.Gen3Period.Value));
        }

        private static IProcessor GetGen4Processor(GetTelonicsParametersForArgosDatesResult parameters)
        {
            if (parameters.Format != 'A' || parameters.Contents == null)
                throw new InvalidOperationException("Invalid parameter file format or contents for Gen4 processor");
            return new Gen4Processor(parameters.Contents.ToArray());
        }

        private static IProcessor GetGps8000Processor(GetTelonicsParametersForArgosDatesResult parameters)
        {
            //if (parameters.Format != 'A' || parameters.Contents == null)
            //    throw new InvalidOperationException("Invalid parameter file format or contents for Gen4 processor");
            return new Gps8000Processor();
        }

        #endregion


        #region Server Check

        private static bool ProcessOnServer(CollarFile file, ArgosPlatform platform = null)
        {
            if (NeedTelonicsSoftware(file) && !HaveAccessToTelonicsSoftware())
            {
                if (OnDatabaseServer())
                    throw new InvalidOperationException("No access to Telonics software to process files.");
                LogGeneralMessage(String.Format("Start processing file {0} on database", file.FileId));
                var database = new AnimalMovementFunctions();
                if (platform == null)
                    database.ArgosFile_Process(file.FileId);
                else
                    database.ArgosFile_ProcessPlatform(file.FileId, platform.PlatformId);
                LogGeneralMessage("Finished processing file on database");
                return true;
            }
            return false;
        }

        private static bool OnDatabaseServer()
        {
            var database = new AnimalMovementViews();
            return database.Connection.DataSource == Environment.MachineName;
        }

        private static bool HaveAccessToTelonicsSoftware()
        {
            var pathToTdc = new Gen4Processor(null).TdcExecutable;
            return File.Exists(pathToTdc);
        }

        private static bool NeedTelonicsSoftware(CollarFile file)
        {
            var database = new AnimalMovementViews();
            return database.FileHasGen4Data(file.FileId) ?? false;
        }

        #endregion


        #region Argos Files

        private static ArgosFile GetArgosFile(CollarFile file)
        {
            switch (file.Format)
            {
                case 'E':
                    return new ArgosEmailFile(file.Contents.ToArray());
                case 'F':
                    return new ArgosAwsFile(file.Contents.ToArray());
                case 'G':
                    return new DebevekFile(file.Contents.ToArray());
                default:
                    throw new InvalidOperationException("Unsupported file format: " + file.Format +
                                                        " (supported formats are E,F,G)");
            }
        }


        private static bool? IsArgosAwsFileIncomplete(ArgosAwsFile awsFile)
        {
            if (awsFile == null)
                return null;
            if (!awsFile.MaxResponseReached.HasValue)
                awsFile.GetTransmissions();
            return awsFile.MaxResponseReached;
        }

        #endregion


        #region Logging

        private static void LogIssueForFile(int fileid, string message, string platform = null, string collarMfgr = null,
                                            string collarId = null, DateTime? firstTransmission = null,
                                            DateTime? lastTransmission = null)
        {
            if (Properties.Settings.Default.LogErrorsToConsole)
                Console.WriteLine(message);
            if (Properties.Settings.Default.LogErrorsToLogFile)
                try
                {
                    File.AppendAllText(Properties.Settings.Default.FileProcessorLogFilePath,
                                       String.Format("{0}: {1}" + Environment.NewLine, DateTime.Now, message));
                }
                catch (Exception ex)
                {
                    Debug.Print("Unable to log to file " + ex.Message);
                }
            var issue = new ArgosFileProcessingIssue
                {
                    FileId = fileid,
                    Issue = message,
                    PlatformId = platform,
                    CollarManufacturer = collarMfgr,
                    CollarId = collarId,
                    FirstTransmission = firstTransmission,
                    LastTransmission = lastTransmission
                };
            var database = new AnimalMovementDataContext();
            database.ArgosFileProcessingIssues.InsertOnSubmit(issue);
            database.SubmitChanges();
        }

        private static void LogGeneralMessage(string message)
        {
            if (Properties.Settings.Default.LogMessagesToConsole)
                Console.WriteLine(message);
            if (Properties.Settings.Default.LogMessagesToLogFile)
                try
                {
                    File.AppendAllText(Properties.Settings.Default.FileProcessorLogFilePath,
                                       String.Format("{0}: {1}" + Environment.NewLine, DateTime.Now, message));
                }
                catch (Exception ex)
                {
                    Debug.Print("Unable to log to file " + ex.Message);
                }
        }

        #endregion

        #endregion
    }
}
