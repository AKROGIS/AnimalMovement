using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using DataModel;
using Telonics;


//TODO - When loading an Argos file, we need to populate the ArgosDownloadSummary Table.
//TODO - Error handling/logging need to be rethunk to support a library.
//TODO - Need method to Process(CollarFile,ArgosPlatform)
//TODO - ProcessAll need to check for partial files

namespace FileLibrary
{
    public class FileProcessor
    {
        public bool ProcessLocally { get; set; }

        public void ProcessAll(Action<Exception, CollarFile, ArgosPlatform> handler = null, ProjectInvestigator pi = null)
        {
            var database = new AnimalMovementDataContext();
            var views = new AnimalMovementViewsDataContext();
            foreach (var file in views.NeverProcessedArgosFiles)
                try
                {
                    if (ProcessLocally)
                        ProcessId(file.FileId);
                    else
                        database.ArgosFile_Process(file.FileId);
                }
                catch (Exception ex)
                {
                    LogFileMessage(file.FileId, "ERROR: " + ex.Message + "processing " + (ProcessLocally ? "locally" : "in database"));
                }
        }


        public void ProcessId(int fileId)
        {
            var database = new AnimalMovementDataContext();
            if (ProcessLocally)
            {
                var file = database.CollarFiles.FirstOrDefault(f => f.FileId == fileId && (f.Format == 'E' || f.Format == 'F' || f.Format == 'G'));
                if (file == null)
                {
                    var msg = String.Format("{0} is not an Id for an Argos file in the database.", fileId);
                    LogGeneralError(msg);
                    return;
                }
                ProcessFile(file);
            }
            else
            {
                database.ArgosFile_Process(fileId);
            }
        }

        public void ProcessFile(CollarFile file)
        {
            try
            {
                ArgosFile argos;
                switch (file.Format)
                {
                    case 'E':
                        argos = new ArgosEmailFile(file.Contents.ToArray());
                        break;
                    case 'F':
                        argos = new ArgosAwsFile(file.Contents.ToArray());
                        break;
                    case 'G':
                        argos = new DebevekFile(file.Contents.ToArray());
                        break;
                    default:
                        LogGeneralError("Unrecognized File Format: " + file.Format);
                        return;
                }
                ProcessFile(file, argos);
            }
            catch (Exception ex)
            {
                LogFileMessage(file.FileId, "ERROR: " + ex.Message);
            }
        }

        internal void ProcessFile(CollarFile file, ArgosFile argos)
        {
            var database = new AnimalMovementDataContext();
            var views = new AnimalMovementViewsDataContext();

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
                    LogFileMessage(file.FileId, msg);
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
                    LogFileMessage(file.FileId, msg, item.Platform);
                    continue;
                }
                if (parameterSets[0].StartDate != null && item.First < parameterSets[0].StartDate)
                {
                    var msg = String.Format("No Collar or TelonicsParameters for ArgosId {0} from {1:g} to {2:g}", item.Platform, item.First, parameterSets[0].StartDate);
                    LogFileMessage(file.FileId, msg, item.Platform);
                }
                int lastIndex = parameterSets.Count - 1;
                if (parameterSets[lastIndex].EndDate != null && parameterSets[lastIndex].EndDate < item.Last)
                {
                    var msg = String.Format("No Collar or TelonicsParameters for ArgosId {0} from {1:g} to {2:g}", item.Platform, parameterSets[lastIndex].EndDate, item.Last);
                    LogFileMessage(file.FileId, msg, item.Platform);
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
                        char format = '?';
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
                                        format = 'D';
                                        break;
                                    case "Gen4":
                                        processor = GetGen4Processor(parameterSet);
                                        format = 'C';
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
                                format = 'B';
                                break;
                            default:
                                //This is programming error, as we should have already checked for this condition
                                throw new InvalidOperationException("Unsupported CollarFile Format '" + format +"'.");
                        }
                        if (processor == null)
                            issue =
                                String.Format(
                                    "Oh No! processor is null. Skipping parameter set for collar {3}, ArgosId:{0} from {1:g} to {2:g}",
                                    parameterSet.PlatformId, start, end,
                                    parameterSet.CollarId);
                        if (issue != null)
                        {
                            LogFileMessage(file.FileId, issue, parameterSet.PlatformId, parameterSet.CollarManufacturer, parameterSet.CollarId);
                            continue;
                        }
                        var transmissions = item.Transmissions.Where(t => start <= t.DateTime && t.DateTime <= end);
                        var lines = processor.ProcessTransmissions(transmissions, argos);
                        var data = Encoding.UTF8.GetBytes(String.Join("\n", lines));
                        var collarFile = new CollarFile
                        {
                            ProjectId = file.Project.ProjectId,
                            FileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName) + "_" + parameterSet.CollarId + ".csv",
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
                        LogFileMessage(file.FileId, message, parameterSet.PlatformId, parameterSet.CollarManufacturer, parameterSet.CollarId);
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
            var processor = new Gen4Processor(parameterSet.Contents.ToArray());
            string tdcExe = Settings.GetSystemDefault("tdc_exe");
            string batchFile = Settings.GetSystemDefault("tdc_batch_file_format");
            int timeout;
            Int32.TryParse(Settings.GetSystemDefault("tdc_timeout"), out timeout);
            if (!String.IsNullOrEmpty(tdcExe))
                processor.TdcExecutable = tdcExe;
            if (!String.IsNullOrEmpty(batchFile))
                processor.BatchFileTemplate = batchFile;
            if (timeout != 0)
                processor.TdcTimeout = timeout;
            return processor;
        }

        #region Logging

        static void LogFileMessage(int fileid, string message, string platform = null, string collarMfgr = null, string collarId = null)
        {
            try
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
            catch (Exception ex)
            {
                LogFatalError("Exception Logging Message in Database: " + ex.Message);
            }
        }

        static void LogFatalError(string error)
        {
            LogGeneralError(error);
            throw new ProcessingException();
        }

        static void LogGeneralWarning(string warning)
        {
            LogGeneralMessage("Warning: " + warning);
        }

        static void LogGeneralError(string error)
        {
            LogGeneralMessage("ERROR: " + error);
        }

        static void LogGeneralMessage(string message)
        {
            Console.WriteLine(message);
            System.IO.File.AppendAllText("ArgosProcessor.log", message);
        }

        #endregion


        public void ProcessPartialFile(CollarFile file, ArgosPlatform platform)
        {
            throw new NotImplementedException();
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
