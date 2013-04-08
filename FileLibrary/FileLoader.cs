using System;
using System.Linq;
using System.Security.Cryptography;
using DataModel;
using Telonics;

//FIXME - this file needs massive cleanup!

namespace FileLibrary
{
    static public class FileLoader
    {
        #region AWS loaders

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
            return file;
        }

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
            return file;
        }

        #endregion

        public static void LoadPath(string path, Action<Exception, string, Project, ProjectInvestigator> handler = null, Project project = null, ProjectInvestigator manager = null, Collar collar = null, char status = 'A', bool allowDups = false)
        {
            if (path == null)
                throw new ArgumentNullException("path", "A path must be provided");
            if (project != null && manager != null)
                throw new InvalidOperationException(String.Format("Project: {0} and Manager: {1} cannot both be non-null.", project.ProjectId, manager.Login));

            if (System.IO.File.Exists(path))
            {
                LoadFilePath(path, project, manager, collar, status, allowDups);
            }
            else
            {
                if (System.IO.Directory.Exists(path))
                {
                    foreach (var file in System.IO.Directory.EnumerateFiles(path))
                        try
                        {
                            LoadFilePath(file, project, manager, collar, status, allowDups);
                        }
                        catch (Exception ex)
                        {
                            if (handler == null)
                                throw;
                            handler(ex, file, project, manager)
                        }
                }
                else
                {
                    throw new InvalidOperationException(path + " is not a folder or file");
                }
            }
        }



        private static void LoadFilePath(string filePath, Project project, ProjectInvestigator owner, Collar collar, char status, bool allowDups)
        {
            /*
             * var file = new file(...);
             * if file.IsDup & !allowDups
             * throw exception
             * 
             */

            var database = new AnimalMovementDataContext();

            var fileContents = System.IO.File.ReadAllBytes(path);
            var fileHash = (new SHA1CryptoServiceProvider()).ComputeHash(fileContents);
            var duplicate = database.CollarFiles.FirstOrDefault(f => f.Sha1Hash == fileHash);
            if (duplicate != null && !allowDups)
                throw new InvalidOperationException(String.Format("Skipping {2}, the contents have already been loaded as file '{0}' {1} '{2}'.", path,
                                    duplicate.FileName, duplicate.Project == null ? "for manager" : "in project", 
                                                        duplicate.Project == null ? duplicate.Owner : duplicate.Project.ProjectName));

            //FIXME can I use objects from different contexts like this?
            var file = new CollarFile
            {
                Project = project,
                FileName = System.IO.Path.GetFileName(filePath),
                Collar = collar,
                ProjectInvestigator = owner,
                Status = status,
                Contents = fileContents,
            };
            database.CollarFiles.InsertOnSubmit(file);
            database.SubmitChanges();

            if (file.LookupCollarFileFormat.ArgosData = 'Y')
                FileProcessor.ProcessFile(file);
        }

        static void xxLoadAndHashFile(string path)
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

        static bool xxAbortBecauseDuplicate(AnimalMovementDataContext database, string path)
        {
            var duplicate = database.CollarFiles.FirstOrDefault(f => f.Sha1Hash == _fileHash);
            if (duplicate == null)
                return false;
            var msg = String.Format("Skipping {2}, the contents have already been loaded as file '{0}' in project '{1}'.", path,
                                    duplicate.FileName, duplicate.Project.ProjectName);
            LogGeneralWarning(msg);
            return true;
        }

        public static bool IsKnownFileFormat(string path)
        {
            try
            {
                return FileFormat (System.IO.File.ReadAllBytes(path)) != '?';
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static char? GuessFileFormat()
        {
            try {
                //FIXME - These will throw an exception if the contents is not the correct format
                argos = new ArgosEmailFile(_fileContents);
                if (argos.GetPrograms().Any())
                    return 'E';
                argos = new ArgosAwsFile(_fileContents);
                if (argos.GetPrograms().Any())
                    return 'F';
                argos = null;
                return null;
            } catch (Exception ex) {
                return null;
            }
        }

        //This should be kept in sync with CollarInfo.cs in the SqlServer_Files project
        private static char FileFormat(Byte[] data)
        {
            //get the first line of the file
            var fileHeader = ReadHeader(data, Encoding.UTF8, 500).Trim().Normalize();  //database for header is only 450 char
            char code = '?';
            var db = new SettingsDataContext();
            foreach (var format in db.LookupCollarFileHeaders)
            {
                var header = format.Header.Normalize();
                var fileFormat = format.FileFormat; //.GetChar() is not implemented
                var regex = format.Regex;
                if (fileHeader.StartsWith(header, StringComparison.OrdinalIgnoreCase) ||
                    (regex != null && new Regex(regex).IsMatch(fileHeader)))
                {
                    code = format;
                    break;
                }
            }
            if (code == '?' && (new ArgosEmailFile(data)).GetPrograms().Any())
                // We already checked for ArgosAwsFile with the header
                code = 'E';
            return code;
        }
        
        private static string ReadHeader(Byte[] bytes, Encoding enc, int maxLength)
        {
            var length = Math.Min(bytes.Length, maxLength);
            using (var stream = new MemoryStream(bytes, 0, length))
                using (var reader = new StreamReader(stream, enc))
                    return reader.ReadLine();
        }


    }
}
