using System;
using System.Linq;
using System.Security.Cryptography;
using DataModel;
using Telonics;

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

        public static void LoadPath(string path, Project project = null, ProjectInvestigator manager = null)
        {
            if (System.IO.File.Exists(path))
                LoadFilePath(path, project);
            else if (System.IO.Directory.Exists(path))
                LoadFolder(path, project);
            throw new InvalidOperationException(path + "is not a folder or file");
        }

        public static void LoadFolder(string folderPath, Project project = null)
        {
            foreach (var file in System.IO.Directory.EnumerateFiles(folderPath))
                try
                {
                    LoadFilePath(System.IO.Path.Combine(folderPath, file), project);
                }
                catch (Exception ex)
                {
                    LogGeneralError("ERROR: " + ex.Message + " processing file " + file + " in " + folderPath);
                }
        }


        static Byte[] _fileContents;
        static Byte[] _fileHash;

        public static void LoadFilePath(string filePath, Project project = null)
        {
            var database = new AnimalMovementDataContext();
            if (project == null)
            {
                LogGeneralError("You must provide a project a project before the file or folder.");
                return;
            }

            LoadAndHashFile(filePath);
            if (_fileContents == null)
                return;
            if (AbortBecauseDuplicate(database, filePath))
                return;
            ArgosFile argos;
            GuessFileFormat(out argos);
            if (argos == null)
            {
                LogGeneralWarning("Skipping file '" + filePath + "' is not a known Argos file type.");
                return;
            }

            var file = new CollarFile
            {
                ProjectId = project.ProjectId,
                FileName = System.IO.Path.GetFileName(filePath),
                CollarManufacturer = "Telonics",
                Status = 'A',
                Contents = _fileContents,
            };
            database.CollarFiles.InsertOnSubmit(file);
            database.SubmitChanges();
            LogGeneralMessage(String.Format("Loaded file {0}, {1} for processing.", file.FileId, file.FileName));

            FileProcessor.ProcessFile(file);

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

        static bool AbortBecauseDuplicate(AnimalMovementDataContext database, string path)
        {
            var duplicate = database.CollarFiles.FirstOrDefault(f => f.Sha1Hash == _fileHash);
            if (duplicate == null)
                return false;
            var msg = String.Format("Skipping {2}, the contents have already been loaded as file '{0}' in project '{1}'.", path,
                                    duplicate.FileName, duplicate.Project.ProjectName);
            LogGeneralWarning(msg);
            return true;
        }

        static char GuessFileFormat(out ArgosFile argos)
        {
            //FIXME - These will throw an exception if the contents is not the correct format
            argos = new ArgosEmailFile(_fileContents);
            if (argos.GetPrograms().Any())
                return 'E';
            argos = new ArgosAwsFile(_fileContents);
            if (argos.GetPrograms().Any())
                return 'F';
            argos = null;
            return '?';
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


    }
}
