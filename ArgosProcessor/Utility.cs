using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using DataModel;

namespace ArgosProcessor
{
    sealed class Utility
    {


        internal static string TpfFolder
        {
            get { return Path.Combine(Directory.GetCurrentDirectory(), "tpf"); }
        }


        internal static string PpfFolder
        {
            get { return Path.Combine(Directory.GetCurrentDirectory(), "ppf"); }
        }


        internal static string TpfExtension
        {
            get { return "tpf"; }
        }


        internal static string PpfExtension
        {
            get { return "ppf"; }
        }


        private static Byte[] GetContentsOfCollarFile(int fileId)
        {
            var data = new AnimalMovementDataContext();
            return data.CollarFiles.First(f => f.FileId == fileId).Contents.ToArray();
        }


        private static Byte[] GetContentsOfCollarParameterFile(int fileId)
        {
            var data = new AnimalMovementDataContext();
            return data.CollarParameterFiles.First(f => f.FileId == fileId).Contents.ToArray();
        }


        private static void SyncParameterFiles()
        {
            //tpfFile
            SyncFilesToFolder('A', TpfFolder, TpfExtension);
            //ppfFiles
            SyncFilesToFolder('B', PpfFolder, PpfExtension);
        }


        private static void SyncFilesToFolder(char format, string folder, string ext)
        {
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var dbIds = GetAllCollarParameterFileIdsFromDBForFormat(format).ToArray();
            var fileIds = GetAllFileIdsFromFolder(folder).ToArray();
            var missingIds = dbIds.Except(fileIds);
            var extraFileIds = fileIds.Except(dbIds);

            //Get missing files
            foreach (var id in missingIds)
            {
                string filepath = Path.Combine(folder, id + "." + ext);
                File.WriteAllBytes(filepath, GetContentsOfCollarParameterFile(id));
            }

            //Remove extraneous files
            foreach (var id in extraFileIds)
            {
                string filepath = Path.Combine(folder, id + "." + ext);
                File.Delete(filepath);
            }
        }


        internal static string ProcessArgosDataForTelonicsGen4(int fileId)
        {
            // TDC runs with a batch file, which we can create dynamically.
            //    the batch file can have multiple argos input files, but only one tpf file
            //    However, we have one argos file that is processed multiple times for each tpf file
            //    so we need to create and process multiple batch files for this argos file.

            //Ensure the parameter files are up to date
            SyncParameterFiles();

            // Get the argos file to process from the database and save it to the filesystem
            var dataFilePath = Path.GetTempFileName();
            File.WriteAllBytes(dataFilePath, GetContentsOfCollarFile(fileId));

            //Set some settings
            //tdcExecutable is the full path to the TDC executable file
            //argosLine is a format string for XML content with the following parameters:
            // {0} the full path of input file to process
            //batchTemplate is a format string for XML content with the following parameters:
            // {0} is one or more argosLine
            // {1} is the full path of the TPF file
            // {2} is the full path of a folder for the output files
            // {3} is the full path of a file of the log file - can be the empty string if no log is needed
            var tdcExecutable = Properties.Settings.Default.tdc_commandline;
            var argosLine = Properties.Settings.Default.tpf_batchfile_argosline_template;
            var argosLines = String.Format(argosLine, dataFilePath);
            var batchTemplate = Properties.Settings.Default.tpf_batchfile_template;
            var outputFolder = GetNewTempDirectory();
            var batchFilePath = Path.GetTempFileName();
            var logFilePath = Path.GetTempFileName();
            var projectId = GetProjectIdFromFileId(fileId);
            var errors = new StringBuilder();

            foreach (var tpfFileId in GetPotentialCollarParameterFiles('A', fileId))
            {
                //Get the TPF file from the filesystem
                string tpfFilePath = Path.Combine(TpfFolder, tpfFileId + "." + TpfExtension);
                string batchCommands = String.Format(batchTemplate, argosLines, tpfFilePath, outputFolder,
                                              logFilePath);
                File.WriteAllText(batchFilePath, batchCommands);

                //  Run TDC
                var p = Process.Start(new ProcessStartInfo
                {
                    FileName = tdcExecutable,
                    Arguments = "/batch:" + batchFilePath,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true
                });
                errors.AppendLine(p.StandardError.ReadToEnd());
                p.WaitForExit();

                //TODO - check the log file for errors - What does an error look like???
                    //Batch started at: 2012.12.17 22:04:31
                    //Processing file: C:\Users\resarwas\AppData\Local\Temp\tmpB158.tmp
                    //Unable to load the the parameter file: "C:\Users\resarwas\Documents\Visual Studio 2010\Projects\AnimalMovement\ArgosProcessor\bin\Debug\tpf\23.tpf".This file may require a newer version of TDC.
                    //Batch completed at: 2012.12.17 22:32:27

            }

            // for each output file created by TDC, send the file to the database
            foreach (var path in Directory.GetFiles(outputFolder))
            {
                var collarId = GetCollarIdFromTelonicsGen4File(path);
                UploadSubFileToDatabase(path, projectId, collarId, 'C', fileId);
                File.SetAttributes(path, FileAttributes.Normal);  // remove the readonly flag put on files created by TDC.
                File.Delete(path);
            }

            //cleanup temp files/folders
            File.Delete(logFilePath);
            File.Delete(dataFilePath);
            File.Delete(batchFilePath);
            Directory.Delete(outputFolder);
            return errors.ToString();
        }


        private static string GetCollarIdFromTelonicsGen4File(string path)
        {
            //Telonics TDC always names the report files as 'CTN_xxx.csv'
            //While it is valid to use a collarID in the DB that is not the complete Telonics CTN,
            //  this will cause a problem uploading the TPF files and this module, so we will make it a necessary precondition.

            if (String.IsNullOrEmpty(path))
                throw new ArgumentException("Invalid (null or empty) path", path);
            string fileName = Path.GetFileName(path);
            if (fileName == null)
                throw new ArgumentException("File does not exist or is inaccessible", path);
            //Debug.Assert(fileName.Contains('_'),"File name is not properly formated; missing underscore.");  //always true since this is only called on files output from TDC
            var collarId = fileName.Split('_')[0];
            //Debug.Assert(CollarIsInDatabase(collarId);"Collar is not in the database"); //Always true since this is a precondition of a valid tpf parameter file
            return collarId;
        }


        private static void UploadSubFileToDatabase(string path, string projectId, string collarId, char format, SqlInt32 parentFileId)
        {
            //Debug.Assert(FileIsInDatabase(ParentFileId), "ParentFile is not in the database"); //Always true since invalid fileid would have failed in caller
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException("Invalid (null or empty) path", path);
            string fileName = Path.GetFileName(path);
            byte[] content = File.ReadAllBytes(path);
            if (fileName == null || content == null)
                throw new ArgumentException("File does not exist, is inaccessible, or empty", path);
            UploadFileToDatabase(fileName, projectId, "Telonics", collarId, format, 'A', content, parentFileId);
        }

        //FIXME - replace by call to DataContext
        private static void UploadFileToDatabase(string fileName, string projectId, string collarMfgr, string collarId,
                                                      char format, char status, byte[] content, SqlInt32 parentFileId)
        {
            using (var connection = new SqlConnection(Properties.Settings.Default.Animal_MovementConnectionString))
            {
                connection.Open();
                const string sql = "[dbo].[CollarFile_Insert]";
                using (var command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@FileName", SqlDbType.NVarChar) {Value = fileName});
                    command.Parameters.Add(new SqlParameter("@ProjectId", SqlDbType.NVarChar) {Value = projectId});
                    command.Parameters.Add(new SqlParameter("@CollarManufacturer", SqlDbType.NVarChar) {Value = collarMfgr});
                    command.Parameters.Add(new SqlParameter("@CollarId", SqlDbType.NVarChar) {Value = collarId ?? SqlString.Null, IsNullable = true});
                    command.Parameters.Add(new SqlParameter("@Format", SqlDbType.Char) {Value = format});
                    command.Parameters.Add(new SqlParameter("@Status", SqlDbType.Char) { Value = status });
                    command.Parameters.Add(new SqlParameter("@Contents", SqlDbType.VarBinary) {Value = content});
                    command.Parameters.Add(new SqlParameter("@ParentFileId", SqlDbType.Int) {Value = parentFileId});
                    command.Parameters.Add(new SqlParameter("@FileId", SqlDbType.Int) { Direction = ParameterDirection.Output});
                    command.ExecuteNonQuery();
                }
            }
        }


        //FIXME - replace by call to DataContext
        private static IEnumerable<int> GetAllCollarParameterFileIdsFromDBForFormat(char format)
        {
            var files = new List<int>();

            using (var connection = new SqlConnection(Properties.Settings.Default.Animal_MovementConnectionString))
            {
                connection.Open();
                const string sql = "SELECT FileId FROM [dbo].[CollarParameterFiles] Where [Format] = @Format";
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add(new SqlParameter("@Format", SqlDbType.Char) { Value = format });
                    using (SqlDataReader results = command.ExecuteReader())
                    {
                        while (results.Read())
                        {
                            files.Add(results.GetInt32(0));
                        }
                    }
                }
            }
            return files;
        }

        //FIXME - replace by call to DataContext
        private static IEnumerable<SqlInt32> GetPotentialCollarParameterFiles(char format, SqlInt32 fileId)
        {
            var files = new List<SqlInt32>();

            using (var connection = new SqlConnection(Properties.Settings.Default.Animal_MovementConnectionString))
            {
                connection.Open();
                const string sql = "SELECT * FROM [dbo].[PotentialCollarParameterFilesForFormatAndCollarFile](@Format, @FileId)";
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add(new SqlParameter("@Format", SqlDbType.Char) { Value = format });
                    command.Parameters.Add(new SqlParameter("@FileId", SqlDbType.Int) { Value = fileId });
                    using (SqlDataReader results = command.ExecuteReader())
                    {
                        while (results.Read())
                        {
                            files.Add(results.GetSqlInt32(0));
                        }
                    }
                }
            }
            return files;
        }

        //FIXME - replace by call to DataContext
        private static string GetProjectIdFromFileId(SqlInt32 fileId)
        {
            string projectId = null;
            using (var connection = new SqlConnection(Properties.Settings.Default.Animal_MovementConnectionString))
            {
                connection.Open();
                const string sql = "SELECT [Project] FROM [dbo].[CollarFiles] WHERE [FileId] = @FileId";
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add(new SqlParameter("@FileId", SqlDbType.Int) { Value = fileId });
                    using (SqlDataReader results = command.ExecuteReader())
                    {
                        while (results.Read())
                        {
                            projectId = results.GetString(0);
                        }
                    }
                }
            }
            return projectId;
        }


        private static string GetNewTempDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }


        private static IEnumerable<int> GetAllFileIdsFromFolder(string folder)
        {
            int id = 0;
            return from file in GetAllFileNamesFromFolder(folder)
                   where Int32.TryParse(file, out id)
                   select id;
        }


        private static IEnumerable<string> GetAllFileNamesFromFolder(string path)
        {
            return Directory.EnumerateFiles(path).Select(Path.GetFileNameWithoutExtension);
        }
    }
}
