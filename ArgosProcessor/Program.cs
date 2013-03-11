using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using DataModel;
using Telonics;

namespace ArgosProcessor
{
    static class Program
    {
        private static AnimalMovementDataContext _database;
        private static AnimalMovementViewsDataContext _views;
        private static String _project;

        /// <summary>
        /// This program obtains an email file from the database, processes all the data in the file, and then loads the results into the database.
        /// This program will be run by the database when the user loads and email file into the database.
        /// The database looks to the standard output from this program for results.  This program will write
        /// nothing to the console on success.  Any warnings or errors will be written to the console for the database.
        /// The database will report non-empty results (errors and warnings) to the user.
        /// It is assumed that all the data in the email file is ASCII text, and that it the concatination of one or more Argos emails.
        /// It is also assumed that the message is in a Telonics data format we know how to process (currently only gen3 or 4)
        /// It is assumed that all the collars are in the database, and that Argos Platform Id is stored in the Alternative Id of the collar
        /// The database should have any necessary information (i.e. deployment dates) necessary to differentiate collars with the same Argos Id
        /// The database should have any necessary parameter files/data needed to process all the data.
        /// All assumptions will be checked, and any failed assumptions will be reported as errors or warnings.
        ///
        /// NOTE: an email file could actually be a composite email file, containing data over a broad range of time,
        ///  such that it contains transmission from a single Argos id when it was a gen3 collar, and also after 
        /// refurbishment into a gen4 (or greater) collar.  We cannot assume that all of the transmissions for a single
        /// Argos Id will map to the same Telonics CTN number (unique collar id)
        /// </summary>
        /// <param name="args">
        /// This program takes zero or more arguments.
        /// If there are no arguments, then the database is queried to get all files that need processing.
        /// for each arg that is an int, the int is assumed to be a FileId in the collarFiles table
        /// for each arg that is a file path, that file path is added to the database, and then processed
        /// for each arg that is a folder path, then all the files in that folder are processed
        /// any other args are ignored with a warning.
        /// </param>
        /// <remarks>
        /// The database can only process one file at a time, and it only makes sense for this tool to process a single file
        /// To process multiple files, the database will call this program multiple times.
        /// </remarks>
        static void Main(string[] args)
        {
            try
            {
                _database = new AnimalMovementDataContext();
                _views = new AnimalMovementViewsDataContext();
                if (args.Length == 0)
                    ProcessAll();
                else
                {
                    foreach (var arg in args)
                    {
                        int id;
                        if (Int32.TryParse(arg, out id) && 0 < id)
                            ProcessId(id);
                        else if (arg.StartsWith("/p:"))
                            _project = arg.Substring(3);
                        else if (System.IO.File.Exists(arg))
                            ProcessFile(arg);
                        else if (System.IO.Directory.Exists(arg))
                            ProcessFolder(arg);
                        else
                            LogGeneralWarning("ignoring argument '" + arg + "'.  Each argument must be an int, file, folder or /p:project");
                    }
                }
            }
            catch (ProcessingException)
            {
                //this is just a signal to exit main, logging has already occured.
            }
            catch (Exception ex)
            {
                LogGeneralError("Unhandled exception: " + ex.Message);
            }
        }

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
                ProcessId(file.FileId);
        }

        static void ProcessFolder(string folderPath)
        {
            foreach (var file in System.IO.Directory.EnumerateFiles(folderPath))
                ProcessFile(System.IO.Path.Combine(folderPath, file));
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
                LogGeneralWarning("Skipping file '"+filePath+"' is not a known Argos file type.");
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

            //TODO - provide a database option where the database only processes a file when asked.
            // currently argos files are not processed, but emails are - get consistent
            if (format == 'F')
                ProcessFile(file, argos);
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
                    throw new InvalidOperationException("Unrecognized File Format: " + file.Format);
            }
            ProcessFile(file, argos);
        }

        static void ProcessFile(CollarFile file, ArgosFile argos)
        {
            var analyzer = new ArgosCollarAnalyzer(argos, _database);

            foreach (var platform in analyzer.UnknownPlatforms)
            {
                string message = String.Format(
                    "WARNING: Platform {0} will be skipped.  It was NOT found in the database.",
                    platform);
                LogFileMessage(file.FileId, message);
            }

            foreach (var platform in analyzer.AmbiguousPlatforms)
            {
                var argosId = platform;  // Use local (not foreach) variable in closure (for compiler version stability)
                var collars = from collar in _database.Collars
                              where collar.ArgosId == argosId
                              select
                              String.Format("Collar: {0} DisposalDate: {1}", collar,
                                            collar.DisposalDate.HasValue
                                            ? collar.DisposalDate.ToString()
                                            : "<NULL> (Active)");
                string message = String.Format(
                    "WARNING: Platform {0} will be skipped because it is ambiguous.\n" +
                    "  Fix this problem by using distinct disposal dates for these collars:\n    {1}",
                    platform, String.Join("\n    ", collars));
                LogFileMessage(file.FileId, message);
            }

            foreach (var problem in analyzer.CollarsWithProblems)
            {
                string message = String.Format(
                    "WARNING: Collar {0} cannot be processed.\n" +
                    "   Reason: {1}",
                    problem.Key, problem.Value);
                LogFileMessage(file.FileId, message);
            }

            argos.Processor = analyzer.ProcessorSelector;
            argos.CollarFinder = analyzer.CollarSelector;
            foreach (var collar in analyzer.ValidCollars)
            {
                try
                {
                    var data = argos.ToTelonicsData(collar.CollarId);
                    var collarFile = new CollarFile
                    {
                        Project = file.Project,
                        FileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName) + "_" + collar.CollarId + ".csv",
                        Format = analyzer.GetFileFormatForCollar(collar),
                        CollarManufacturer = "Telonics",
                        CollarId = collar.CollarId,
                        Status = 'A',
                        ParentFileId = file.FileId,
                        Contents = Encoding.UTF8.GetBytes(String.Join("\n", data)),
                    };
                    _database.CollarFiles.InsertOnSubmit(collarFile);
                    _database.SubmitChanges();
                    LogGeneralMessage(String.Format("Success: Added collar {0} for file {1}", collar, file.FileId));
                }
                catch (Exception ex)
                {
                    string message;
                    if (ex is NoMessagesException && analyzer.UnambiguousSharedCollars.Contains(collar))
                        message = String.Format(
                            "Notice: Collar {0} (for shared argos Id {1}) had no messages.\n" +
                            "  This is common for all but one of the collars that share an Argos Id.",
                            collar, collar.ArgosId);
                    else
                        message = String.Format(
                            "ERROR: Collar {0} (Argos Id {1}) encountered a problem: {2}",
                            collar, collar.ArgosId, ex.Message);
                    LogFileMessage(file.FileId, message, collar.ArgosId, collar.CollarManufacturer, collar.CollarId);
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

        public ProcessingException(string message) : base(message)
        {
        }

        public ProcessingException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ProcessingException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
