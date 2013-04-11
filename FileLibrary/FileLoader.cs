using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using DataModel;
using Telonics;

namespace FileLibrary
{
    public class FileLoader
    {

        #region Public API

        #region Static Methods

        #region AWS loaders

        /// <summary>
        /// Loads and logs the download data for an Argos Program
        /// </summary>
        /// <param name="program">The Argos ProgramId that this data is for</param>
        /// <param name="days">The number of days that are included in this data set</param>
        /// <param name="results">The data returned from the Argos Web Server (may be null)</param>
        /// <param name="errors">Any errors returned by the Argos Web Server (may be null)</param>
        /// <returns>Returns the newly created collar file, complete with database calculated fields</returns>
        /// <remarks>Only one of results and errors can be non-null.</remarks>
        internal static CollarFile LoadProgram(ArgosProgram program, int days,
                                               ArgosWebSite.ArgosWebResult results, string errors)
        {
            //if results is null, then errors should be non-null (database rule, insert will fail if false)
            CollarFile file = null;
            var database = new AnimalMovementDataContext();
            //Linq to SQL wraps the changes in a transaction so file will not be created if log cannot be written
            if (results != null)
            {
                file = new CollarFile
                    {
                        Owner = program.Manager,
                        FileName = "program_" + program.ProgramId + "_" + DateTime.Now.ToString("yyyyMMdd") + ".aws",
                        Status = 'A',
                        Contents = results.ToBytes()
                    };
                database.CollarFiles.InsertOnSubmit(file);
            }
            var log = new ArgosDownload
                {
                    ProgramId = program.ProgramId,
                    CollarFile = file,
                    Days = days,
                    ErrorMessage = errors
                };
            database.ArgosDownloads.InsertOnSubmit(log);
            database.SubmitChanges();
            //Linq TO SQL Insert with SPROC dos not set associations, and provides not partial methods to expand
            if (file != null)
                file.LookupCollarFileFormat = database.LookupCollarFileFormats.First(l => l.Code == file.Format);
            return file;
        }

        /// <summary>
        /// Loads and logs the download data for an Argos Platform
        /// </summary>
        /// <param name="platform">The Argos PlatformId that this data is for</param>
        /// <param name="days">The number of days that are included in this data set</param>
        /// <param name="results">The data returned from the Argos Web Server (may be null)</param>
        /// <param name="errors">Any errors returned by the Argos Web Server (may be null)</param>
        /// <returns>Returns the newly created collar file, complete with database calculated fields</returns>
        /// <remarks>Only one of results and errors can be non-null.</remarks>
        internal static CollarFile LoadPlatfrom(ArgosPlatform platform, int days,
                                                ArgosWebSite.ArgosWebResult results, string errors)
        {
            //if results is null, then errors should be non-null (database rule, insert will fail if false)
            CollarFile file = null;
            var database = new AnimalMovementDataContext();
            //Linq to SQL wraps the changes in a transaction so file will not be created if log cannot be written
            if (results != null)
            {
                file = new CollarFile
                    {
                        Owner = platform.ArgosProgram.Manager,
                        FileName = "platform_" + platform.PlatformId + "_" + DateTime.Now.ToString("yyyyMMdd") + ".aws",
                        Status = 'A',
                        Contents = results.ToBytes()
                    };
                database.CollarFiles.InsertOnSubmit(file);
            }
            var log = new ArgosDownload
                {
                    PlatformId = platform.PlatformId,
                    CollarFile = file,
                    Days = days,
                    ErrorMessage = errors
                };
            database.ArgosDownloads.InsertOnSubmit(log);
            database.SubmitChanges();
            //Linq TO SQL Insert with SPROC dos not set associations, and provides not partial methods to expand
            if (file != null)
                file.LookupCollarFileFormat = database.LookupCollarFileFormats.First(l => l.Code == file.Format);
            return file;
        }

        #endregion

        /// <summary>
        /// Attempts to load the file (or all the files in a folder) into the database
        /// </summary>
        /// <param name="path">A complete file or folder path to data that will be loaded</param>
        /// <param name="handler">Delegate to handle exceptions on each file, so that processing can continue.
        /// If the handler is null, processing will stop on first exception.
        /// The handler can throw it's own exception to halt further processing</param>
        /// <param name="project">Associate the new file with this Project (optional)</param>
        /// <param name="manager">Associate the new file with this Project Investigator (optional)</param>
        /// <param name="collar">This file contains data for this collar.  Not required for files with Argos data, or
        /// if the collar can be determined from the file contents</param>
        /// <param name="status">The new file should be (A)ctive (generate fixes, default) or (I)nactive (no fixes)</param>
        /// <param name="allowDups">File will not be rejected if the contents match an existing file</param>
        /// <remarks>Only one of project and manager can be non-null.</remarks>
        public static void LoadPath(string path, Action<Exception, string, Project, ProjectInvestigator> handler = null,
                                    Project project = null, ProjectInvestigator manager = null, Collar collar = null,
                                    char status = 'A', bool allowDups = false)
        {
            if (path == null)
                throw new ArgumentNullException("path", "A path must be provided");
            if (project != null && manager != null)
                throw new InvalidOperationException(
                    String.Format("Project: {0} and Manager: {1} cannot both be non-null.", project.ProjectId,
                                  manager.Login));

            if (File.Exists(path))
            {
                LoadFilePath(path, project, manager, collar, status, allowDups);
            }
            else
            {
                if (Directory.Exists(path))
                {
                    foreach (var file in Directory.EnumerateFiles(path))
                        try
                        {
                            LoadFilePath(file, project, manager, collar, status, allowDups);
                        }
                        catch (Exception ex)
                        {
                            if (handler == null)
                                throw;
                            handler(ex, file, project, manager);
                        }
                }
                else
                {
                    throw new InvalidOperationException(path + " is not a folder or file");
                }
            }
        }


        #endregion

        #region Public Properties

        /// <summary>
        /// Associate the new file with this Project
        /// </summary>
        public Project Project { get; set; }

        /// <summary>
        /// Associate the new file with this Project Investigator
        /// </summary>
        public ProjectInvestigator Owner { get; set; }

        /// <summary>
        /// This file contains data for this collar.  This is the user provide collar, it may be null.
        /// If this is null and the format of the file requres a collar, and it can be determined from the contents
        /// then this property will contain the derived collar after the insert.
        /// </summary>
        public Collar Collar { get; set; }

        /// <summary>
        /// The new file should be (A)ctive (generate fixes) or (I)nactive (no fixes)
        /// </summary>
        public char Status { get; set; }

        /// <summary>
        /// File will not be rejected if the contents match an existing file
        /// </summary>
        public bool AllowDuplicates { get; set; }

        /// <summary>
        /// Used by the FileProcessor to link a results file with it source data
        /// </summary>
        internal int? ParentFileId { get; set; }
        /// <summary>
        /// Used by the FileProcessor to link a results file with Argos deployment used
        /// </summary>
        internal int? ArgosDeploymentId { get; set; }
        /// <summary>
        /// Used by the FileProcessor to link a results file with the Collar parameters used
        /// </summary>
        internal int? CollarParameterId { get; set; }

        /// <summary>
        /// The data context in which the CollarFile is created
        /// </summary>
        public AnimalMovementDataContext Database { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// The path to the data of this collar file
        /// </summary>
        public Byte[] Contents { get; private set; }

        /// <summary>
        /// The binary contents of the collar file
        /// </summary>
        public Lazy<char?> Format { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new fileprocessor with the name and contents of the file at the file path
        /// </summary>
        /// <param name="filePath">The complete operating system path to the file contents</param> 
        public FileLoader(string filePath)
            : this(filePath, File.ReadAllBytes(filePath))
        { }

        /// <summary>
        /// Create a new fileprocessor with the contents provided, and the name provided
        /// </summary>
        /// <param name="filePath">The file name. If a complete path is provided, only the file name is used</param>
        /// <param name="contents">The binary contents of the file</param>
        /// <remarks>This is provided for internal methods like the file processor that create the contents dynamically</remarks>
        internal FileLoader(string filePath, Byte[] contents)
        {
            if (contents == null || contents.Length == 0)
                throw new ArgumentException("File contents is empty or unavailable", "contents");
            Contents = contents;
            Database = new AnimalMovementDataContext();
            FilePath = filePath;
            Status = 'A';
            Format = new Lazy<char?>(() => GetFormat(Contents));
        }

        #endregion

        #region Public Instance Methods

        /// <summary>
        /// Validates and loads the file to the database
        /// </summary>
        /// <returns>Returns the newly created collar file, complete with database calculated fields</returns>
        public CollarFile Load()
        {
            Validate();
            // The entity objects I got from the callers (i.e. Project, Owner, Collar)
            // came from a foreign DataContext, so they cannot be used to create a new
            // entity in this datacontext.
            var file = new CollarFile
            {
                ProjectId = Project == null ? null : Project.ProjectId,
                FileName = Path.GetFileName(FilePath),
                CollarManufacturer = Collar == null ? null : Collar.CollarManufacturer,
                CollarId = Collar == null ? null : Collar.CollarId,
                Owner = Owner == null ? null : Owner.Login,
                Status = Status,
                Contents = Contents,
                ParentFileId = ParentFileId,
                ArgosDeploymentId = ArgosDeploymentId,
                CollarParameterId = CollarParameterId
            };
            Database.CollarFiles.InsertOnSubmit(file);
            Database.SubmitChanges();
            //Linq TO SQL Insert with SPROC dos not set associations, and provides not partial methods to expand
            file.LookupCollarFileFormat = Database.LookupCollarFileFormats.First(l => l.Code == file.Format);
            return file;
        }

        #endregion

        #endregion

        #region Private methods

        // This is a template for client side usage
        private static void LoadFilePath(string filePath, Project project, ProjectInvestigator owner, Collar collar,
                                         char status, bool allowDups)
        {
            var fileLoader = new FileLoader(filePath)
                {
                    Project = project,
                    Owner = owner,
                    Collar = collar,
                    Status = status,
                    AllowDuplicates = allowDups
                };
            var file = fileLoader.Load();
            if (file.LookupCollarFileFormat.ArgosData == 'Y')
                FileProcessor.ProcessFile(file);
        }


        private void Validate()
        {
            //Do client side validation to save a round trip to the database if we know the insert will fail
            //This logic should be consistent with the database rules

            // one and only one of Project and Owner must be specified
            if (Owner == null && Project == null)
                throw new InvalidOperationException("One of project or owner must be specified.");
            if (Owner != null && Project != null)
                throw new InvalidOperationException("Both project and owner cannot be specified simultaneously.");

            //Check Status
            if (Status != 'A' && Status != 'I')
                throw new InvalidOperationException(
                    String.Format("A status of '{0}' is not acceptable.  Acceptable values are 'A' and 'I'.", Status));

            //Deny duplicates
            if (!AllowDuplicates)
            {
                var duplicate = GetDuplicate();
                if (duplicate != null)
                    throw new InvalidOperationException(
                        String.Format("The contents have already been loaded as file '{0}' {1} '{2}'.",
                                      duplicate.FileName, duplicate.Project == null ? "for manager" : "in project",
                                      duplicate.Project == null ? duplicate.Owner : duplicate.Project.ProjectName));
            }

            //Unknown format
            if (Format.Value == null)
                throw new InvalidOperationException("The contents are not in a recognizable format.");

            //Try and guess the collar if one is required and not provided 
            if (CollarIsRequired)
            {
                if (Collar == null)
                    Collar = GetCollarFromFile();
                if (Collar == null)
                    throw new InvalidOperationException(
                            "The format requires a valid collar but none was provided " +
                            "nor could it be determined from the filename or contents.");
            }
        }


        private CollarFile GetDuplicate()
        {
            var fileHash = (new SHA1CryptoServiceProvider()).ComputeHash(Contents);
            return Database.CollarFiles.FirstOrDefault(f => f.Sha1Hash == fileHash);
        }


        #region Collar from file

        private Collar GetCollarFromFile()
        {
            if (!Format.Value.HasValue)
                return null;

            string argosId = null;
            if (Format.Value.Value == 'B')
                argosId = GetArgosFromFormatB();
            if (Format.Value.Value == 'D')
                argosId = GetArgosFromFormatD();

            //If we have an ArgosId and it maps to one and only one collar, then use it.
            if (argosId != null)
            {
                try
                {
                    return Database.ArgosDeployments.Single(d => d.PlatformId == argosId).Collar;
                }
                catch (Exception)
                {
                   return null;
                }
            }

            string ctn = null;
            if (Format.Value.Value == 'C')
                ctn = GetCtnFromFormatC();
            if (Format.Value.Value == 'H')
                ctn = GetCtnFromFormatH();
            if (ctn == null)
                return null;
            var collar = Database.Collars.FirstOrDefault(c => c.CollarManufacturer == "Telonics" && c.CollarId == ctn);
            if (collar != null)
                return collar;
            //Try without the Alpha suffix
            if (ctn.Length != 7 && !Char.IsUpper(ctn[6]))
                return null;
            ctn = ctn.Substring(0, 6);
            return Database.Collars.FirstOrDefault(c => c.CollarManufacturer == "Telonics" && c.CollarId == ctn);
        }


        private string GetArgosFromFormatB()
        {
            //the first column is the Argos ID, if more than one, then return none
            try
            {
                var checkForHeader = true;
                var db = new SettingsDataContext();
                string argosId = null;
                var header = db.LookupCollarFileHeaders.First(h => h.FileFormat == 'B').Header;
                foreach (var line in ReadLines(Contents, Encoding.UTF8))
                {
                    //skip the first line if it looks like the header
                    if (checkForHeader && line.Normalize().StartsWith(header, StringComparison.OrdinalIgnoreCase))
                    {
                        checkForHeader = false;
                        continue;
                    }
                    //skip empty/blank lines
                    if (string.IsNullOrEmpty(line.Replace(',', ' ').Trim()))
                        continue;
                    var newArgosId = line.Substring(0, line.IndexOf(",", StringComparison.OrdinalIgnoreCase));
                    if (argosId == null)
                    {
                        argosId = newArgosId;
                        checkForHeader = false;
                    }
                    if (newArgosId != argosId)
                        return null;
                }
                return argosId;
            }
            catch (Exception)
            {
                return null;
            }
        }


        private string GetArgosFromFormatD()
        {
            //the third column is the Argos ID, if more than one, then return none
            try
            {
                var checkForHeader = true;
                var db = new SettingsDataContext();
                string argosId = null;
                var header = db.LookupCollarFileHeaders.First(h => h.FileFormat == 'D').Header;
                foreach (var line in ReadLines(Contents, Encoding.UTF8))
                {
                    if (checkForHeader && line.Normalize().StartsWith(header, StringComparison.OrdinalIgnoreCase))
                    {
                        checkForHeader = false;
                        continue;
                    }
                    //skip empty/blank lines
                    if (string.IsNullOrEmpty(line.Replace(',', ' ').Trim()))
                        continue;
                    var newArgosId = line.Split(new[] { '\t', ',' })[2];
                    if (argosId == null)
                    {
                        argosId = newArgosId;
                        checkForHeader = false;
                    }
                    if (newArgosId != argosId)
                        return null;
                }
                return argosId;
            }
            catch (Exception)
            {
                return null;
            }
        }


        private string GetCtnFromFormatC()
        {
            //In the C format, the 7th or 8th line is 'CTN,649024A'
            var line = ReadLines(Contents, Encoding.UTF8)
                .Skip(5).Take(4).FirstOrDefault(l => l.Normalize().StartsWith("CTN,", StringComparison.OrdinalIgnoreCase));
            return line == null ? null : line.Substring(4);
        }


        private string GetCtnFromFormatH()
        {
            //the H format has 4th line begins with 'unitRecord,621500A,'
            var line = ReadLines(Contents, Encoding.UTF8).Skip(3).FirstOrDefault();
            if (line == null)
            return null;
            var tokens = line.Split(',');
            if (tokens.Length < 2)
                return null;
            return tokens[1];
        }


        private static IEnumerable<string> ReadLines(Byte[] bytes, Encoding enc)
        {
            using (var stream = new MemoryStream(bytes, 0, bytes.Length))
            using (var reader = new StreamReader(stream, enc))
                while(reader.Peek() >= 0)
                    yield return reader.ReadLine();
        }


        private bool CollarIsRequired
        {
            get { return Database.LookupCollarFileFormats.Any(f => f.Code == Format.Value && f.ArgosData == 'N'); }
        }

        #endregion

        #region File format

        //This should be kept in sync with CollarInfo.cs in the SqlServer_Files project
        private static char? GetFormat(Byte[] data)
        {
            try
            {
                //get the first line of the file
                var fileHeader = ReadHeader(data, Encoding.UTF8, 500).Trim().Normalize();
                //database for header is only 450 char
                char code = '?';
                var db = new SettingsDataContext();
                foreach (var format in db.LookupCollarFileHeaders)
                {
                    var header = format.Header.Normalize();
                    var regex = format.Regex;
                    if (fileHeader.StartsWith(header, StringComparison.OrdinalIgnoreCase) ||
                        (regex != null && new Regex(regex).IsMatch(fileHeader)))
                    {
                        code = format.FileFormat;
                        break;
                    }
                }
                if (code == '?' && (new ArgosEmailFile(data)).GetPrograms().Any())
                    // We already checked for ArgosAwsFile with the header
                    code = 'E';
                return code == '?' ? (char?)null : code;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static string ReadHeader(Byte[] bytes, Encoding enc, int maxLength)
        {
            var length = Math.Min(bytes.Length, maxLength);
            using (var stream = new MemoryStream(bytes, 0, length))
            using (var reader = new StreamReader(stream, enc))
                return reader.ReadLine();
        }

        #endregion

        #endregion
    }
}
