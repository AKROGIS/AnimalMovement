using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataModel;

namespace FileLibrary
{
    public class FileProcessor
    {
        public bool ProcessLocally { get; set; }
        
        public int? LoadAws(Byte[] contents,  Project project = null, Collar collar = null)
        {
            throw new NotImplementedException();
        }
        
        public void LoadPath(string path,  Project project = null, Collar collar = null)
        {
            throw new NotImplementedException();
        }
        
        public void ProcessAll()
        {
            throw new NotImplementedException();
        }

        public void ProcessPath(string path, Project project = null)
        {
            if (System.IO.File.Exists(path))
                throw new NotImplementedException();
            else if (System.IO.Directory.Exists(path))
                throw new NotImplementedException();
            throw new NotImplementedException();
        }

        public void ProcessId(int fileId)
        {
            if (ProcessLocally)
            {
                
            }
            else
            {
                var database = new AnimalMovementDataContext(); 
                database.ProcessAllCollarsForArgosFile(fileId);
            }
        }

        public void ProcessFile(CollarFile file)
        {
            throw new NotImplementedException();
        }

        public void SummerizeFile(CollarFile file)
        {
            throw new NotImplementedException();
        }

        public void SummerizeAll()
        {
            throw new NotImplementedException();
        }
    }
}

//From ArgosDownloader
/*
        public int? LoadAws(Byte[] contents, Project project = null)
        {
            throw new NotImplementedException();
        }


                        var days = Math.Min(MaxDays, collar.Days ?? MaxDays);
                        errors = "";
                        int? firstFileId = null;
                        var results = ArgosWebSite.GetCollar(collar.UserName, collar.Password, collar.PlatformId, days,
                                                             out errors);
                        if (results != null)
                        {
                            try
                            {
                                var processor = new FileProcessor();
                                firstFileId = processor.LoadAws(results.ToBytes());
                            }
                            catch (Exception ex)
                            {
                                errors = "Error loading/processing AWS download: " + ex.Message;
                            }
                        }
                        try
                        {
                            //log this activity in the database
                            //if results is null, then errors should be non-null
                            var log = new ArgosDownload
                            {
                                CollarManufacturer = collar.CollarManufacturer,
                                CollarId = collar.CollarId,
                                FileId = firstFileId,
                                ErrorMessage = errors
                            };
                            db.ArgosDownloads.InsertOnSubmit(log);
                            db.SubmitChanges();
                        }
                        catch (Exception ex)
                        {
                            errors = Environment.NewLine + "Error logging download to database: " + ex.Message +
                                     Environment.NewLine + "Errors: '" + errors + "'" + Environment.NewLine +
                                     "CollarId = " + collar.CollarId + "FileId = " + firstFileId;
                        }


                        int? secondFileId = null;
                             var collarFile = new CollarFile
                                {
                                    Project = collar.ProjectId,
                                    FileName = collar.PlatformId + "_" + DateTime.Now.ToString("yyyyMMdd") + ".aws",
                                    Format = 'F',
                                    CollarManufacturer = collar.CollarManufacturer,
                                    CollarId = collar.CollarId,
                                    Status = 'A',
                                    Contents = results.ToBytes()
                                };
                            db.CollarFiles.InsertOnSubmit(collarFile);
                            try
                            {
                                db.SubmitChanges();
                                firstFileId = collarFile.FileId;
                            }
                            catch (Exception ex)
                            {
                                errors = "Error writing raw AWS download to database: " + ex.Message;
                            }
                            if (firstFileId != null)
                            {
                                try
                                {
                                    var collarFile2 = ProcessAws(collar, firstFileId.Value, results);
                                    try
                                    {
                                        db.CollarFiles.InsertOnSubmit(collarFile2);
                                        db.SubmitChanges();
                                        secondFileId = collarFile2.FileId;
                                    }
                                    catch (Exception ex)
                                    {
                                        errors = "Error writing Gen3/4 output to database: " + ex.Message;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    errors = "Error processing AWS download to Gen3/4: " + ex.Message;
                                }
                                if (secondFileId == null)
                                {
                                    try
                                    {
                                        db.CollarFiles.DeleteOnSubmit(collarFile);
                                        db.SubmitChanges();
                                    }
                                    catch (Exception ex)
                                    {
                                        errors += Environment.NewLine + "Error trying to rollback changes: " +
                                                  ex.Message + Environment.NewLine + "The AWS file (id = " + firstFileId +
                                                  ") added to the database could not be removed after a subsequent error.";
                                    }
                                }
                            }


        private static CollarFile ProcessAws(DownloadableAndAnalyzableCollar collar, int parentFileId,
                                             ArgosWebSite.ArgosWebResult results)
        {
            //TODO - Can I simplify this? Maybe use the ArgosCollarAnalyzer from ArgosProcessor?

            CollarFile collarFile;
            switch (collar.CollarModel)
            {
                case "Gen3":
                    if (!collar.Gen3Period.HasValue)
                        throw new InvalidOperationException("Gen3 collar cannot be processed without a period");
                    var g3processor = new Gen3Processor(TimeSpan.FromMinutes(collar.Gen3Period.Value));

                    ArgosFile aws3 = new ArgosAwsFile(results.ToBytes()) {Processor = id => g3processor};

                    byte[] g3data =
                        Encoding.UTF8.GetBytes(String.Join(Environment.NewLine, aws3.ToTelonicsData()));
                    collarFile = new CollarFile
                        {
                            Project = collar.ProjectId,
                            FileName =
                                collar.PlatformId + "_gen3_" + DateTime.Now.ToString("yyyyMMdd") +
                                ".csv",
                            Format = 'D',
                            CollarManufacturer = collar.CollarManufacturer,
                            CollarId = collar.CollarId,
                            Status = 'A',
                            Contents = g3data,
                            ParentFileId = parentFileId
                        };
                    break;
                case "Gen4":
                    var g4processor = new Gen4Processor(collar.TpfFile.ToArray());
                    string tdcExe = Settings.GetSystemDefault("tdc_exe");
                    string batchFile = Settings.GetSystemDefault("tdc_batch_file_format");
                    int timeout;
                    Int32.TryParse(Settings.GetSystemDefault("tdc_timeout"), out timeout);
                    if (!String.IsNullOrEmpty(tdcExe))
                        g4processor.TdcExecutable = tdcExe;
                    if (!String.IsNullOrEmpty(batchFile))
                        g4processor.BatchFileTemplate = batchFile;
                    if (timeout != 0)
                        g4processor.TdcTimeout = timeout;

                    ArgosFile aws4 = new ArgosAwsFile(results.ToBytes()) {Processor = id => g4processor};

                    byte[] g4data =
                        Encoding.UTF8.GetBytes(String.Join(Environment.NewLine, aws4.ToTelonicsData()));
                    collarFile = new CollarFile
                        {
                            Project = collar.ProjectId,
                            FileName =
                                collar.PlatformId + "_gen4_" + DateTime.Now.ToString("yyyyMMdd") +
                                ".csv",
                            Format = 'C',
                            CollarManufacturer = collar.CollarManufacturer,
                            CollarId = collar.CollarId,
                            Status = 'A',
                            Contents = g4data,
                            ParentFileId = parentFileId
                        };
                    break;
                default:
                    var msg = String.Format("Unsupported model ({0} for collar {1}", collar.CollarModel, collar.CollarId);
                    throw new ArgumentOutOfRangeException(msg);
            }
            return collarFile;
        }
*/

//From ArgosProcessor
//TODO - When loading an Argos file, we need to populate the ArgosDownloadSummary Table.
//TODO - error handeling/logging need to be rethunk to support a library.

/*
        #region Logging

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
                _database.ArgosFileProcessingIssues.InsertOnSubmit(issue);
                _database.SubmitChanges();
            }
            catch (Exception ex)
            {
                LogFatalError("Exception Logging Message in Database: " + ex.Message);
            }
        }

        #endregion


        static void ProcessAll()
        {
            foreach (var file in _views.UnprocessedArgosFiles)
                try
                {
                    if (_processLocally)
                        ProcessId(file.FileId);
                    else
                        _database.ProcessAllCollarsForArgosFile(file.FileId);
                }
                catch (Exception ex)
                {
                    LogFileMessage(file.FileId, "ERROR: " + ex.Message + "processing " + (_processLocally ? "locally" : "in database"));
                }
        }

        static void ProcessFolder(string folderPath)
        {
            foreach (var file in System.IO.Directory.EnumerateFiles(folderPath))
                try
                {
                    ProcessFile(System.IO.Path.Combine(folderPath, file));
                }
                catch (Exception ex)
                {
                    LogGeneralError("ERROR: " + ex.Message + " processing file " + file + " in " + folderPath);
                }
        }

        #region process filepath

        static Byte[] _fileContents;
        static Byte[] _fileHash;

        static void ProcessFile(string filePath)
        {
            if (_project == null)
            {
                LogGeneralError("You must provide a project a project before the file or folder.");
                return;
            }

            LoadAndHashFile(filePath);
            if (_fileContents == null)
                return;
            if (AbortBecauseDuplicate(filePath))
                return;
            ArgosFile argos;
            char format = GuessFileFormat(out argos);
            if (argos == null)
            {
                LogGeneralWarning("Skipping file '" + filePath + "' is not a known Argos file type.");
                return;
            }

            var file = new CollarFile
            {
                Project = _project,
                FileName = System.IO.Path.GetFileName(filePath),
                Format = format,
                CollarManufacturer = "Telonics",
                Status = 'A',
                Contents = _fileContents,
                Sha1Hash = _fileHash
            };
            _database.CollarFiles.InsertOnSubmit(file);
            _database.SubmitChanges();
            LogGeneralMessage(String.Format("Loaded file {0}, {1} for processing.", file.FileId, file.FileName));
            SummarizeArgosFile(file.FileId, argos);

            if (_processLocally)
                ProcessFile(file, argos);
            else
                _database.ProcessAllCollarsForArgosFile(file.FileId);
        }

        static void LoadAndHashFile(string path)
        {
            try
            {
                _fileContents = System.IO.File.ReadAllBytes(path);
            }
            catch (Exception ex)
            {
                LogGeneralError("The file cannot be read: " + ex.Message);
                return;
            }
            _fileHash = (new SHA1CryptoServiceProvider()).ComputeHash(_fileContents);
        }

        static bool AbortBecauseDuplicate(string path)
        {
            var duplicate = _database.CollarFiles.FirstOrDefault(f => f.Sha1Hash == _fileHash);
            if (duplicate == null)
                return false;
            var msg = String.Format("Skipping {2}, the contents have already been loaded as file '{0}' in project '{1}'.", path,
                                    duplicate.FileName, duplicate.Project1.ProjectName);
            LogGeneralWarning(msg);
            return true;
        }

        static char GuessFileFormat(out ArgosFile argos)
        {
            argos = new ArgosEmailFile(_fileContents);
            if (argos.GetPrograms().Any())
                return 'E';
            argos = new ArgosAwsFile(_fileContents);
            if (argos.GetPrograms().Any())
                return 'F';
            argos = null;
            return '?';
        }

        #endregion

        static void ProcessId(int id)
        {
            var file = _database.CollarFiles.FirstOrDefault(f => f.FileId == id && (f.Format == 'E' || f.Format == 'F'));
            if (file == null)
            {
                var msg = String.Format("{0} is not an Argos email or AWS file Id in the database.", id);
                LogGeneralError(msg);
                return;
            }
            ProcessFile(file);
        }

        private static void ProcessFile(CollarFile file)
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

        static void ProcessFile(CollarFile file, ArgosFile argos)
        {
            ClearPreviousProcessedFiles(file);
            ClearPreviousProcessingIssues(file);

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
            foreach (var item in transmissionGroups)
            {
                var parameterSets =
                    _views.GetTelonicsParametersForArgosDates(item.Platform, item.First, item.Last)
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
                            Project = file.Project,
                            FileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName) + "_" + parameterSet.CollarId + ".csv",
                            Format = format,
                            CollarManufacturer = "Telonics",
                            CollarId = parameterSet.CollarId,
                            Status = 'A',
                            ParentFileId = file.FileId,
                            Contents = data,
                            Sha1Hash = (new SHA1CryptoServiceProvider()).ComputeHash(data)
                        };
                        _database.CollarFiles.InsertOnSubmit(collarFile);
                        _database.SubmitChanges();
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

        private static void ClearPreviousProcessedFiles(CollarFile parentFile)
        {
            foreach (var file in _database.CollarFiles.Where(f => f.ParentFileId == parentFile.FileId))
                _database.CollarFiles.DeleteOnSubmit(file);
            _database.SubmitChanges();
        }

        private static void ClearPreviousProcessingIssues(CollarFile file)
        {
            foreach (var issue in _database.ArgosFileProcessingIssues.Where(i => i.CollarFile == file))
                _database.ArgosFileProcessingIssues.DeleteOnSubmit(issue);
            _database.SubmitChanges();
        }

        private static void SummarizeArgosFile(int fileId, ArgosFile file)
        {
            foreach (var program in file.GetPrograms())
            {
                foreach (var platform in file.GetPlatforms(program))
                {
                    var minDate = file.FirstTransmission(platform);
                    var maxDate = file.LastTransmission(platform);
                    var summary = new ArgosFilePlatformDate
                        {
                            FileId = fileId,
                            ProgramId = program,
                            PlatformId = platform,
                            FirstTransmission = minDate,
                            LastTransmission = maxDate
                        };
                    _database.ArgosFilePlatformDates.InsertOnSubmit(summary);
                }
            }
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
*/