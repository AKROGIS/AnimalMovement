using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using DataModel;
using Telonics;

namespace FileLibrary
{
    public static class FileProcessor
    {
        public static void ProcessAll(Action<Exception, CollarFile, ArgosPlatform> handler = null, ProjectInvestigator pi = null)
        {
            var database = new AnimalMovementDataContext();
            var views = new AnimalMovementViewsDataContext();
            foreach (var item in views.NeverProcessedArgosFiles)
            {
                var file = database.CollarFiles.First(f => f.FileId == item.FileId);
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
                    ProcessPartialFile(file, platform);
                }
                catch (Exception ex)
                {
                    if (handler == null)
                        throw;
                    handler(ex, file, platform);
                }
            }
        }


        public static void ProcessFile(CollarFile file)
        {
            ArgosFile argos = GetArgosFile(file);
            ProcessFile(file, argos);
        }


        public static void ProcessPartialFile(CollarFile file, ArgosPlatform platform)
        {
            ArgosFile argos = GetArgosFile(file);
            ProcessPartialFile(file, argos, platform);
        }


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


        private static void ProcessPartialFile(CollarFile file, ArgosFile argos, ArgosPlatform platform)
        {
            throw new NotImplementedException();
        }


        private static void ProcessFile(CollarFile file, ArgosFile argos)
        {
            var database = new AnimalMovementDataContext();
            var views = new AnimalMovementViewsDataContext();

            if (NeedTelonicsSoftware(file) && !HaveAccessToTelonicsSoftware())
            {
                if (OnDatabaseServer())
                    throw new InvalidOperationException("No access to Telonics software to process files.");
                database.ArgosFile_Process(file.FileId);
                return;
            }
            
            database.ArgosFile_ClearProcessingResults(file.FileId);

            var transmissionGroups = from transmission in argos.GetTransmissions()
                                     group transmission by transmission.PlatformId
                                         into transmissions
                                         select new
                                         {
                                             Platform = transmissions.Key,
                                             First = transmissions.Min(t => t.DateTime),
                                             Last = transmissions.Max(t => t.DateTime),
                                             Transmissions = transmissions
                                         };

            var awsFile = argos as ArgosAwsFile;
            if (awsFile != null)
            {
                string msg = null;
                if (!awsFile.MaxResponseReached.HasValue)
                    msg = String.Format("Programming Error, unable to determine if file is truncated.");
                else if (awsFile.MaxResponseReached.Value)
                    msg = String.Format("This file is truncated.  The Argos server could not return all the data requested.");
                if (msg != null)
                    LogIssueForFile(file.FileId, msg);
            }

            foreach (var item in transmissionGroups)
            {
                var parameterSets =
                    views.GetTelonicsParametersForArgosDates(item.Platform, item.First, item.Last)
                          .OrderBy(c => c.StartDate)
                          .ToList();
                if (parameterSets.Count == 0)
                {
                    var msg = String.Format("No Collar or TelonicsParameters for ArgosId {0} from {1:g} to {2:g}", item.Platform, item.First, item.Last);
                    LogIssueForFile(file.FileId, msg, item.Platform);
                    continue;
                }
                if (parameterSets[0].StartDate != null && item.First < parameterSets[0].StartDate)
                {
                    var msg = String.Format("No Collar or TelonicsParameters for ArgosId {0} from {1:g} to {2:g}", item.Platform, item.First, parameterSets[0].StartDate);
                    LogIssueForFile(file.FileId, msg, item.Platform);
                }
                int lastIndex = parameterSets.Count - 1;
                if (parameterSets[lastIndex].EndDate != null && parameterSets[lastIndex].EndDate < item.Last)
                {
                    var msg = String.Format("No Collar or TelonicsParameters for ArgosId {0} from {1:g} to {2:g}", item.Platform, parameterSets[lastIndex].EndDate, item.Last);
                    LogIssueForFile(file.FileId, msg, item.Platform);
                }
                foreach (var parameterSet in parameterSets)
                {
                    DateTime start = ((parameterSet.StartDate ?? DateTime.MinValue) < item.First
                                          ? item.First
                                          : parameterSet.StartDate.Value);
                    DateTime end = (item.Last < (parameterSet.EndDate ?? DateTime.MaxValue)
                                          ? item.Last
                                          : parameterSet.EndDate.Value);
                    try
                    {
                        IProcessor processor = null;
                        string issue = null;
                        switch (file.Format)
                        {
                            case 'E':
                            case 'F':
                                switch (parameterSet.CollarModel)
                                {
                                    case "Gen3":
                                        try
                                        {
                                            processor = GetGen3Processor(parameterSet);
                                        }
                                        catch (Exception ex)
                                        {
                                            issue =
                                                String.Format(
                                                    "{4}. Skipping parameter set for collar {3}, ArgosId:{0} from {1:g} to {2:g}",
                                                    parameterSet.PlatformId, start, end,
                                                    parameterSet.CollarId, ex.Message);
                                        }
                                        break;
                                    case "Gen4":
                                        processor = GetGen4Processor(parameterSet);
                                        break;
                                    default:
                                        issue =
                                            String.Format(
                                                "Unknown CollarModel '{4}' encountered. Skipping parameter set for collar {3}, ArgosId:{0} from {1:g} to {2:g}",
                                                parameterSet.PlatformId, start, end,
                                                parameterSet.CollarId, parameterSet.CollarModel);
                                        break;
                                }
                                break;
                            case 'G':
                                processor = new DebevekProcessor();
                                break;
                            default:
                                issue = String.Format("Unsupported CollarFile Format '" + file.Format +
                                                      "'. (supported formats are E,F,G)");
                                break;
                        }
                        if (processor == null)
                            issue =
                                String.Format(
                                    "Oh No! processor is null. Skipping parameter set for collar {3}, ArgosId:{0} from {1:g} to {2:g}",
                                    parameterSet.PlatformId, start, end,
                                    parameterSet.CollarId);
                        if (issue != null)
                        {
                            LogIssueForFile(file.FileId, issue, parameterSet.PlatformId, parameterSet.CollarManufacturer, parameterSet.CollarId);
                            continue;
                        }
                        var transmissions = item.Transmissions.Where(t => start <= t.DateTime && t.DateTime <= end);
                        var lines = processor.ProcessTransmissions(transmissions, argos);
                        var data = Encoding.UTF8.GetBytes(String.Join("\n", lines));
                        var collarFile = new CollarFile
                        {
                            Project = file.Project,
                            FileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + parameterSet.CollarId + ".csv",
                            CollarManufacturer = parameterSet.CollarManufacturer,
                            CollarId = parameterSet.CollarId,
                            Status = file.Status,
                            ParentFileId = file.FileId,
                            Contents = data,
                            Owner = file.Owner
                        };
                        database.CollarFiles.InsertOnSubmit(collarFile);
                        database.SubmitChanges();
                        var message =
                            String.Format(
                                "Successfully added Argos {0} transmissions from {1:g} to {2:g} to Collar {3}/{4}",
                                parameterSet.PlatformId, start, end,
                                parameterSet.CollarManufacturer, parameterSet.CollarId);
                        LogGeneralMessage(message);
                        //LogFileMessage(file.FileId, message, parameterSet.PlatformId, parameterSet.CollarManufacturer, parameterSet.CollarId);
                    }
                    catch (Exception ex)
                    {
                        var message =
                            String.Format(
                                "ERROR {5} adding Argos {0} transmissions from {1:g} to {2:g} to Collar {3}/{4}",
                                parameterSet.PlatformId, start, end,
                                parameterSet.CollarManufacturer, parameterSet.CollarId, ex.Message);
                        LogIssueForFile(file.FileId, message, parameterSet.PlatformId, parameterSet.CollarManufacturer, parameterSet.CollarId);
                    }
                }
            }
        }


        private static IProcessor GetGen3Processor(GetTelonicsParametersForArgosDatesResult parameterSet)
        {
            if (parameterSet == null || parameterSet.Gen3Period == null)
                throw new ProcessingException("Unknown period for Gen3 collar");
            if (parameterSet.Format == 'B')
                throw new ProcessingException("GPS Fixes for Gen3 collars with PTT parameter files (*.ppf) must be processed with Telonics ADC-T03.");
            return new Gen3Processor(TimeSpan.FromMinutes(parameterSet.Gen3Period.Value));
        }


        private static IProcessor GetGen4Processor(GetTelonicsParametersForArgosDatesResult parameterSet)
        {
            var processor = new Gen4Processor(parameterSet.Contents.ToArray())
                {
                    BatchFileTemplate = Properties.Settings.Default.TdcBatchFileFormat,
                    TdcExecutable = Properties.Settings.Default.TdcPathToExecutable,
                    TdcTimeout = Properties.Settings.Default.TdcMillisecondTimeout
                };
            return processor;
        }


        #region Logging

        static void LogIssueForFile(int fileid, string message, string platform = null, string collarMfgr = null, string collarId = null)
        {
            var issue = new ArgosFileProcessingIssue
            {
                FileId = fileid,
                Issue = message,
                PlatformId = platform,
                CollarManufacturer = collarMfgr,
                CollarId = collarId
            };
            var database = new AnimalMovementDataContext();
            database.ArgosFileProcessingIssues.InsertOnSubmit(issue);
            database.SubmitChanges();
        }

        static void LogGeneralMessage(string message)
        {
            Console.WriteLine(message);
            File.AppendAllText("ArgosProcessor.log", message);
        }

        #endregion

        static bool OnDatabaseServer()
        {
            var database = new AnimalMovementViewsDataContext();
            return database.Connection.DataSource == Environment.MachineName;
        }

        static bool HaveAccessToTelonicsSoftware()
        {
            return File.Exists(Properties.Settings.Default.TdcPathToExecutable);
        }

        static bool NeedTelonicsSoftware(CollarFile file)
        {
            var database = new AnimalMovementViewsDataContext();
            var hasGen4 = database.FileHasGen4Data(file.FileId);
            //I should never get null; limitation of Linq to SQL
            return hasGen4.HasValue && hasGen4.Value;
        }



    }

    [Serializable]
    public class ProcessingException : Exception
    {
        public ProcessingException()
        {
        }

        public ProcessingException(string message)
            : base(message)
        {
        }

        public ProcessingException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected ProcessingException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}
