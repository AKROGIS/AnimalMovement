using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace ArgosProcessor
{
    class Utility
    {

        private static Byte[] GetContentsOfCollarFile(SqlInt32 fileId)
        {
            return GetFileContents("CollarFiles", fileId);
        }


        private static Byte[] GetContentsOfCollarParameterFile(SqlInt32 fileId)
        {
            return GetFileContents("CollarParameterFiles", fileId);
        }


        private static Byte[] GetFileContents(string table, SqlInt32 fileId)
        {
            Byte[] bytes = null;

            using (var connection = new SqlConnection(Properties.Settings.Default.Animal_MovementConnectionString))
            {
                connection.Open();
                string sql = "SELECT [Contents] FROM [" + table + "] WHERE [FileId] = @fileId";
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add(new SqlParameter("@fileId", SqlDbType.Int) { Value = fileId });
                    using (SqlDataReader results = command.ExecuteReader())
                    {
                        while (results.Read())
                        {
                            bytes = results.GetSqlBytes(0).Buffer;
                        }
                    }
                }
            }
            return bytes;
        }


        private static void SyncParameterFiles()
        {
            //tpfFile
            SyncFilesToFolder('A', "tpf", "tpf");
            //ppfFiles
            SyncFilesToFolder('B', "ppf", "ppf");
        }


        private static void SyncFilesToFolder(char format, string folder, string ext)
        {
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var dbIds = GetAllCollarParameterFileIdsFromDBForFormat(format);
            var fileIds = GetAllFileIdsFromFolder(folder);
            var missingIds = dbIds.Except(fileIds);

            //Get missing files
            foreach (var id in missingIds)
            {
                string filepath = Path.Combine(folder, id + "." + ext);
                File.WriteAllBytes(filepath, GetContentsOfCollarParameterFile(id));
            }

            //Remove extra files
            //TODO
        }


        internal static void ProcessArgosDataForTelonicsGen3(SqlInt32 fileId)
        {
            //FIXME - implement this code
            //Save file to disk with temp name
            //Ensure PPT files are avaialble to ADC-T03
            //Setup ADC-T03 (input/output folders)
            //Run ADC-T03
            //For each file in output folder
            //  Add the file to the database
        }


        internal static string ProcessArgosDataForTelonicsGen4(SqlInt32 fileId)
        {
            // Get the file to process from the database and save it to the filesystem
            string dataFilePath = Path.GetTempFileName();
            File.WriteAllBytes(dataFilePath, GetContentsOfCollarFile(fileId));

            //Ensure the parameter files are up to date
            SyncParameterFiles();

            //get the settings
            //commandLine is a format string for the contents of a Windows command.  It has one parameter
            //  {0} the full path of the XML batch settings
            var commandLine = Properties.Settings.Default.tdc_commandline;
            //argosLine is a format string with 1 parameter for full path of input file;  there can be multiple argosLines in the batch settings
            var argosLine = Properties.Settings.Default.tpf_batchfile_argosline_template;
            //batchSettings - is a format string for an XML file with the following parameters
            // {0} is one or more argosLines
            // {1} is the full path of the TPF file
            // {2} is the full path of a folder for the output files
            // {3} is the full path of a file of the log file - can be the empty string if no log is needed
            var batchTemplate = Properties.Settings.Default.tpf_batchfile_template;

            //string tpfFilePath = Path.GetTempFileName();
            string batchFilePath = Path.GetTempFileName();
            string logFilePath = Path.GetTempFileName();

            //create a temp folder for the output files
            string outputFolder = GetNewTempDirectory();

            string projectId = GetProjectIdFromFileId(fileId);

            var errors = new StringBuilder();
            foreach (var tpfFileId in GetPotentialCollarParameterFiles('A', fileId))
            {
                //Get the TPF file from the filesystem
                string tpfFilePath = Path.Combine(Directory.GetCurrentDirectory(), "tpf", tpfFileId + "." + "tpf");

                //  Create the input batch file
                //    the batch file can have multiple argos input files, but only one tfp file
                //    we only process one file at a time
                string batchCommands = String.Format(batchTemplate, String.Format(argosLine, dataFilePath), tpfFilePath, outputFolder,
                                              logFilePath);
                File.WriteAllText(batchFilePath, batchCommands);

                //  Run TDC
                var p = Process.Start(new ProcessStartInfo
                {
                    FileName = commandLine,
                    Arguments = "/batch:" + batchFilePath,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true
                });
                errors.AppendLine(p.StandardError.ReadToEnd());
                p.WaitForExit();

                //TODO - check the log file for errors ???
            }

            // for each output file created by TDC, send the file to the database
            foreach (var path in Directory.GetFiles(outputFolder))
            {
                UploadSubFileToDatabase(path, projectId, 'C', fileId);
                errors.AppendLine("Inserted " + path);
                //File.Delete(file);
            }

            //cleanup temp files/folders
            //File.Delete(logFilePath);
            //File.Delete(tpfFilePath);
            File.Delete(dataFilePath);
            File.Delete(batchFilePath);
            //Directory.Delete(outputFolder);
            return errors.ToString();
        }


        private static void UploadSubFileToDatabase(string path, string projectId, char format, SqlInt32 parentFileId)
        {
            if (path == null)
                throw new NullReferenceException();
            string fileName = Path.GetFileName(path);
            byte[] content = File.ReadAllBytes(path);
            if (fileName == null || content == null)
                throw new ArgumentException("File does not exist, is inaccessible, or empty", path);
            //Assert - Parent File exists in database (true since invalid fileid would have failed in caller)

            //FIXME - Improve CollarId extraction.  user id '634123' does not equal Telonics Id '634123A'
            //TODO - Any benefit to using the collar id in the column in the file?
            //Telonics TDC always names the report files as 'CTN_xxx.csv', since we know this is a TDC output, we cab use that information
            //However the user may have a different id for this collar.
            Debug.Assert(fileName.Contains('_'),"File name is not properly formated; missing underscore.");
            string collarId = fileName.Split('_')[0];
            //Assert CollarId is in the database - precondition of the valid tpf parameter file
            UploadFileToDatabase(fileName, projectId, "Telonics", collarId, format, 'A', content, parentFileId);
        }


        internal static SqlInt32 UploadFileToDatabase(string fileName, string projectId, string collarMfgr, string collarId,
                                                      char format, char status, byte[] content, SqlInt32 parentFileId)
        {
            SqlInt32 fileId;
            using (var connection = new SqlConnection(Properties.Settings.Default.Animal_MovementConnectionString))
            {
                connection.Open();
                const string sql =
                    "EXEC CollarFile_Insert @FileName, @ProjectId, @CollarManufacturer, @CollarId, @Format, @Status, @Contents, @FileId";
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add(new SqlParameter("@FileName", SqlDbType.NVarChar) {Value = fileName});
                    command.Parameters.Add(new SqlParameter("@ProjectId", SqlDbType.NVarChar) {Value = projectId});
                    command.Parameters.Add(new SqlParameter("@CollarManufacturer", SqlDbType.NVarChar) {Value = collarMfgr});
                    command.Parameters.Add(new SqlParameter("@CollarId", SqlDbType.NVarChar) {Value = collarId ?? SqlString.Null, IsNullable = true});
                    command.Parameters.Add(new SqlParameter("@Format", SqlDbType.Char) {Value = format});
                    command.Parameters.Add(new SqlParameter("@Status", SqlDbType.Char) { Value = status });
                    command.Parameters.Add(new SqlParameter("@Contents", SqlDbType.VarBinary) {Value = content});
                    //command.Parameters.Add(new SqlParameter("@ParentFileId", SqlDbType.Int) {Value = parentFileId});
                    var fileIdParameter = new SqlParameter("@FileId", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                    command.Parameters.Add(fileIdParameter);
                    command.ExecuteNonQuery();
                    fileId = (SqlInt32) fileIdParameter.SqlValue;
                }
            }
            return fileId;
        }


        private static IEnumerable<SqlInt32> GetAllFileIdsFromFolder(string folder)
        {
            int id = 0;
            return from file in GetAllFileNamesFromFolder(folder)
                   where Int32.TryParse(file, out id)
                   select (SqlInt32) id;
        }


        private static IEnumerable<string> GetAllFileNamesFromFolder(string path)
        {
            return Directory.EnumerateFiles(path).Select(Path.GetFileNameWithoutExtension);
        }

        private static IEnumerable<SqlInt32> GetAllCollarParameterFileIdsFromDBForFormat(char format)
        {
            var files = new List<SqlInt32>();

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
                            files.Add(results.GetSqlInt32(0));
                        }
                    }
                }
            }
            return files;
        }


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


        public static string GetNewTempDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }

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


        //private static string GetSystemSetting(SqlString key)
        //{
        //    string setting = null;
        //    using (var connection = new SqlConnection(Properties.Settings.Default.Animal_MovementConnectionString))
        //    {
        //        connection.Open();
        //        const string sql = "SELECT [Value] FROM [dbo].[Settings] WHERE [Username] = 'system' AND [Key] =  @key";
        //        using (var command = new SqlCommand(sql, connection))
        //        {
        //            command.Parameters.Add(new SqlParameter("@key", SqlDbType.NVarChar) { Value = key });
        //            using (SqlDataReader results = command.ExecuteReader())
        //            {
        //                while (results.Read())
        //                {
        //                    setting = results.GetString(0);
        //                }
        //            }
        //        }
        //    }
        //    return setting;
        //}
    }
}
