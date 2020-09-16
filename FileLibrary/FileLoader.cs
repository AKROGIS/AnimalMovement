using DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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
            //Linq to SQL does not return the PK (timestamp) of the new ArgosDownload object, so don't use it in this data context
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
            {
                file.LookupCollarFileFormat = database.LookupCollarFileFormats.First(l => l.Code == file.Format);
            }

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
            //Linq to SQL does not return the PK (timestamp) of the new ArgosDownload object, so don't use it in this data context
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
            {
                file.LookupCollarFileFormat = database.LookupCollarFileFormats.First(l => l.Code == file.Format);
            }

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
            {
                throw new ArgumentNullException("path", "A path must be provided");
            }

            if (project != null && manager != null)
            {
                throw new InvalidOperationException(
                    String.Format("Project: {0} and Manager: {1} cannot both be non-null.", project.ProjectId,
                                  manager.Login));
            }

            if (File.Exists(path))
            {
                try
                {
                    LoadAndProcessFilePath(path, project, manager, collar, status, allowDups);
                }
                catch (Exception ex)
                {
                    if (handler == null)
                    {
                        throw;
                    }

                    handler(ex, path, project, manager);
                }
            }
            else
            {
                if (Directory.Exists(path))
                {
                    foreach (var file in Directory.EnumerateFiles(path))
                    {
                        try
                        {
                            LoadAndProcessFilePath(file, project, manager, collar, status, allowDups);
                        }
                        catch (Exception ex)
                        {
                            if (handler == null)
                            {
                                throw;
                            }

                            handler(ex, file, project, manager);
                        }
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
        /// The path to the data of this collar file
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// The binary contents of the collar file
        /// </summary>
        public Byte[] Contents { get; private set; }

        /// <summary>
        /// The Format that the file thinks it is.
        /// No exceptions are thrown.  Null is returned if unknown or undeterminable.
        /// </summary>
        public LookupCollarFileFormat Format => LazyFormat.Value;

        /// <summary>
        /// The collar that the file thinks it belongs to.
        /// The file's collar must be in the database or null is returned.
        /// No exceptions are thrown.  Null is returned instead.
        /// </summary>
        public Collar FileCollar => LazyFileCollar.Value;

        /// <summary>
        /// True if the client is required to provide a collar specification for this file.
        /// No exceptions are thrown. If Format is null (unknown or undeterminable), then return false.
        /// </summary>
        public bool CollarIsRequired => FileFormatRequiresCollar && Collar == null && FileCollar == null;

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
            {
                throw new ArgumentException("File contents is empty or unavailable", "contents");
            }

            Contents = contents;
            Database = new AnimalMovementDataContext();
            FilePath = filePath;
            Status = 'A';
            LazyFormat = new Lazy<LookupCollarFileFormat>(GetFormat);
            LazyFileCollar = new Lazy<Collar>(GetFileCollar);
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
            if (FileFormatRequiresCollar && Collar == null)
            {
                Collar = FileCollar;
            }
            // The entity objects I got from the callers (i.e. Project, Owner, Collar)
            // came from a foreign DataContext, so they cannot be used as associated
            // entities with a new entity in this datacontext.
            var file = new CollarFile
            {
                ProjectId = Project?.ProjectId,
                FileName = Path.GetFileName(FilePath),
                CollarManufacturer = Collar?.CollarManufacturer,
                CollarId = Collar?.CollarId,
                Owner = Owner?.Login,
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
        private static void LoadAndProcessFilePath(string filePath, Project project, ProjectInvestigator owner, Collar collar,
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
            if (file.LookupCollarFileFormat.ArgosData == 'Y' || file.Format == 'H')
            {
                FileProcessor.ProcessFile(file);
            }
        }


        private void Validate()
        {
            //Do client side validation to save a round trip to the database if we know the insert will fail
            //This logic should be consistent with the database rules

            // one and only one of Project and Owner must be specified
            if (Owner == null && Project == null)
            {
                throw new InvalidOperationException("One of project or owner must be specified.");
            }

            if (Owner != null && Project != null)
            {
                throw new InvalidOperationException("Both project and owner cannot be specified simultaneously.");
            }

            //Check Status
            if (Status != 'A' && Status != 'I')
            {
                throw new InvalidOperationException(
                    String.Format("A status of '{0}' is not acceptable.  Acceptable values are 'A' and 'I'.", Status));
            }

            //Deny duplicates
            if (!AllowDuplicates)
            {
                var duplicate = GetDuplicate();
                if (duplicate != null)
                {
                    throw new InvalidOperationException(
                        String.Format("The contents have already been loaded as file '{0}' {1} '{2}'.",
                                      duplicate.FileName, duplicate.Project == null ? "for manager" : "in project",
                                      duplicate.Project == null ? duplicate.Owner : duplicate.Project.ProjectName));
                }
            }

            //Unknown format
            if (Format == null)
            {
                throw new InvalidOperationException("The contents are not in a recognizable format.");
            }

            //Are we missing a collar when one is required
            if (CollarIsRequired)
            {
                throw new InvalidOperationException(
                    "The format requires a valid collar but none was provided " +
                    "nor could it be determined from the filename or contents.");
            }
        }


        private CollarFile GetDuplicate()
        {
            var fileHash = new SHA1CryptoServiceProvider().ComputeHash(Contents);
            return Database.CollarFiles.FirstOrDefault(f => f.Sha1Hash == fileHash);
        }


        #region Collar from file

        private bool FileFormatRequiresCollar => Format != null && Format.RequiresCollar == 'Y';

        private Lazy<Collar> LazyFileCollar { get; set; }

        private Collar GetFileCollar()
        {
            if (Format == null)
            {
                return null;
            }

            try
            {
                string argosId = null;
                if (Format.Code == 'B')
                {
                    argosId = GetArgosFromFormatB();
                }

                if (Format.Code == 'D')
                {
                    argosId = GetArgosFromFormatD();
                }

                //If we have an ArgosId and it maps to one and only one collar, then use it.
                if (argosId != null)
                {
                    return Database.ArgosDeployments.Single(d => d.PlatformId == argosId).Collar;
                }

                string ctn = null;
                if (Format.Code == 'C')
                {
                    ctn = GetCtnFromFormatC();
                }

                if (Format.Code == 'H')
                {
                    ctn = GetCtnFromFormatH();
                }

                if (ctn == null)
                {
                    return null;
                }

                var collar =
                    Database.Collars.FirstOrDefault(c => c.CollarManufacturer == "Telonics" && c.CollarId == ctn);
                if (collar != null)
                {
                    return collar;
                }
                //Try without the Alpha suffix
                if (ctn.Length != 7 && !Char.IsUpper(ctn[6]))
                {
                    return null;
                }

                ctn = ctn.Substring(0, 6);
                return Database.Collars.FirstOrDefault(c => c.CollarManufacturer == "Telonics" && c.CollarId == ctn);
            }
            catch (Exception)
            {
                return null;
            }
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
                    {
                        continue;
                    }

                    var newArgosId = line.Substring(0, line.IndexOf(",", StringComparison.OrdinalIgnoreCase));
                    if (argosId == null)
                    {
                        argosId = newArgosId;
                        checkForHeader = false;
                    }
                    if (newArgosId != argosId)
                    {
                        return null;
                    }
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
                    {
                        continue;
                    }

                    var newArgosId = line.Split('\t', ',')[2];
                    if (argosId == null)
                    {
                        argosId = newArgosId;
                        checkForHeader = false;
                    }
                    if (newArgosId != argosId)
                    {
                        return null;
                    }
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
            return line?.Substring(4);
        }


        private string GetCtnFromFormatH()
        {
            //the H format has multiple header lines that alternate with field names  (starts with #)
            //and data lines.  items are comma separated.  Looking for the data in a field called 'ctn'
            var endOfHeader = "packedDatalogRecord";
            var searchFieldName = "ctn";

            var lines = ReadLines(Contents, Encoding.UTF8).TakeWhile(l => !l.StartsWith(endOfHeader));
            int indexOfData = -1;
            foreach (var line in lines)
            {
                if (0 <= indexOfData)
                {
                    var tokens = line.Split(',');
                    return tokens[indexOfData];
                }
                if (line.StartsWith("#") && line.Contains(',' + searchFieldName + ','))
                {
                    var tokens = line.Split(',');
                    indexOfData = new List<string>(tokens).IndexOf(searchFieldName);
                }
            }
            return null;
        }


        private static IEnumerable<string> ReadLines(Byte[] bytes, Encoding enc)
        {
            using (var stream = new MemoryStream(bytes, 0, bytes.Length))
            using (var reader = new StreamReader(stream, enc))
            {
                while (reader.Peek() >= 0)
                {
                    yield return reader.ReadLine();
                }
            }
        }

        #endregion

        #region File format

        private Lazy<LookupCollarFileFormat> LazyFormat { get; set; }

        //This should be kept in sync with CollarInfo.cs in the SqlServer_Files project
        private LookupCollarFileFormat GetFormat()
        {
            try
            {
                char code = '?';
                //get the first line of the file
                var fileHeader = ReadHeader(Contents, Encoding.UTF8, 500).Trim().Normalize();
                //database for header is only 450 char
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
                if (code == '?' && new ArgosEmailFile(Contents).GetPrograms().Any())
                {
                    // We already checked for ArgosAwsFile with the header
                    code = 'E';
                }

                return Database.LookupCollarFileFormats.FirstOrDefault(f => f.Code == code);
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
            {
                return reader.ReadLine();
            }
        }

        #endregion

        #endregion
    }
}
