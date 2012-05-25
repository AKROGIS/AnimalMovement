using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;

namespace AnimalMovementLibrary
{
    public sealed class AML
    {

        public static void Connect()
        {
            //GenerateSampleData();
            string connectionString = GetConnectionString();
            KnownProjectCodes = new List<string>();
            Investigators = null;
            ConnectToData(connectionString);
        }

        private static void ConnectToData(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand("SELECT * FROM dbo.LookupCollarFileFormats;", connection);
                SqlDataReader reader = command.ExecuteReader();
                var data = new DataTable("LookupCollarFileFormats");
                data.Load(reader);
                reader.Close();
                Formats = data;

                command = new SqlCommand("SELECT * FROM dbo.LookupCollarManufacturers;", connection);
                reader = command.ExecuteReader();
                data = new DataTable("CollarManufacturers");
                data.Load(reader);
                reader.Close();
                CollarMfgrs = data;

                command = new SqlCommand("SELECT ProjectId FROM dbo.Projects;", connection);
                reader = command.ExecuteReader();
                KnownProjectCodes.Clear();
                while (reader.Read())
                    KnownProjectCodes.Add(reader.GetString(0));
                reader.Close();

                //FIXME - Close the READER

                Investigators = Investigator.Investigators;

                //command = new SqlCommand("SELECT UserName FROM dbo.ProjectInvestigators;", connection);
                //reader = command.ExecuteReader();
                //Investigators.Clear();
                //while (reader.Read())
                //    Investigators.Add(new Investigator(reader.GetString(0)));
                //reader.Close();
            }
        }

        public static List<Investigator> Investigators { get; private set; }


        static public string GetConnectionString()
        {
            // To avoid storing the connection string in your code, you can retrieve it from a configuration file.
            //return "Data Source=(local);Initial Catalog=Animal_Movement;Integrated Security=SSPI";
            return "Data Source=INPAKRO39088\\SQL2008R2;Initial Catalog=Animal_Movement;Integrated Security=SSPI";
        }

        private static void GenerateSampleData()
        {
            var data = new DataTable();
            DataColumn column;
            DataRow row;
            column = new DataColumn {ColumnName = "Code", DataType = typeof (string)};
            data.Columns.Add(column);
            column = new DataColumn {ColumnName = "CollarManufacturer", DataType = typeof (string)};
            data.Columns.Add(column);
            column = new DataColumn {ColumnName = "HasCollarIdColumn", DataType = typeof (string)};
            data.Columns.Add(column);
            column = new DataColumn {ColumnName = "Name", DataType = typeof (string)};
            data.Columns.Add(column);
            row = data.NewRow();
            row["Code"] = "A";
            row["Name"] = "Format A";
            row["CollarManufacturer"] = "telonics";
            row["HasCollarIdColumn"] = "Y";
            data.Rows.Add(row);
            row = data.NewRow();
            row["Code"] = "B";
            row["Name"] = "Format B";
            row["CollarManufacturer"] = "telonics";
            row["HasCollarIdColumn"] = "N";
            data.Rows.Add(row);
            Formats = data;

            data = new DataTable();
            column = new DataColumn {ColumnName = "CollarManufacturer", DataType = typeof (string)};
            data.Columns.Add(column);
            column = new DataColumn {ColumnName = "Name", DataType = typeof (string)};
            data.Columns.Add(column);
            row = data.NewRow();
            row["CollarManufacturer"] = "telonics";
            row["Name"] = "Telonics Argos Gen 4";
            data.Rows.Add(row);
            row = data.NewRow();
            row["CollarManufacturer"] = "unknown";
            row["Name"] = "Unknown Mfgr";
            data.Rows.Add(row);
            CollarMfgrs = data;
        }

        public static void UploadFile(string path, string format, string collarMfgr, string collarId, string project, string status)
        {
            Debug.Print("path = {0}, format = {1}, collarMfgr = {2}, collarId = {3}, project = {4}, status = {5}", path,
                        format, collarMfgr, collarId, project, status);

            //Upload file to server - wait if file exists on server
            int fileId;
            byte[] data = File.ReadAllBytes(path);
            string connectionString = GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                //begining file upload
                //This is done in two steps, so we can report on the progress.
                //FIXME - Send message to user
                var command = new SqlCommand("[dbo].[InsertCollarFile]", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 0;
                command.Parameters.Add(new SqlParameter("@FileID", SqlDbType.Int) { Direction = ParameterDirection.Output });
                command.Parameters.Add(new SqlParameter("@FileName", Path.GetFileNameWithoutExtension(path)));
                command.Parameters.Add(new SqlParameter("@Project", project));
                command.Parameters.Add(new SqlParameter("@CollarManufacturer", collarMfgr));
                command.Parameters.Add(new SqlParameter("@CollarId", collarId));
                command.Parameters.Add(new SqlParameter("@Status", "I"));
                command.Parameters.Add(new SqlParameter("@Format", format));
                command.Parameters.Add(new SqlParameter("@Contents", data));
                command.ExecuteNonQuery();
                fileId = (int)command.Parameters["@FileId"].Value;

                //Activate the file (The server can process locations from 200 fixes per second)
                if (status == "A")
                {
                    //FIXME - Message the user (update progress)
                    command = new SqlCommand("[dbo].[UpdateCollarFileStatus]", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 0;
                    command.Parameters.Add(new SqlParameter("@FileID", fileId));
                    command.Parameters.Add(new SqlParameter("@Status", status));
                    command.ExecuteNonQuery();
                }

            }
            Debug.Print("fileId = {0}", fileId);
        }

        public static string ValidateInput(string path, string format, string collarMfgr, string collarId, string project, string status)
        {
            //check if file has already been uploaded
            //Check if I have editing permissions
            //Check if project is null - i.e. I am not an editor on any projects
            Debug.Print("path = {0}, format = {1}, collarMfgr = {2}, collarId = {3}, project = {4}, status = {5}", path,
                        format, collarMfgr, collarId, project, status);
            return null;
        }

        public static List<string> KnownProjectCodes { get; private set; }

        public static DataTable Formats { get; private set; }

        public static DataTable CollarMfgrs { get; private set; }

        public static DataTable Projects(string user)
        {
            DataTable data;
            string connectionString = GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand("SELECT * FROM dbo.Projects " +
                                             "LEFT JOIN dbo.ProjectEditors " +
                                             "ON Projects.ProjectId = ProjectEditors.ProjectId " +
                                             "WHERE Editor = @Parameter " +
                                             "OR ProjectInvestigator = @Parameter;", connection);
                command.Parameters.Add(new SqlParameter("@Parameter", user));
                SqlDataReader reader = command.ExecuteReader();
                data = new DataTable("Projects");
                data.Load(reader);
            }
            return data;
        }

        public static DataTable Collars(string mfgr)
        {
            DataTable data;
            string connectionString = GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand("SELECT * FROM dbo.Collars WHERE CollarManufacturer = @Parameter;", connection);
                command.Parameters.Add(new SqlParameter("@Parameter", mfgr));
                SqlDataReader reader = command.ExecuteReader();
                data = new DataTable("Collars");
                data.Load(reader);

                connection.Close();
            }
            return data;
        }


        public static string GuessFileFormat(string path)
        {
            //FIXME - implement
            return "A";
        }

        public static string GuessDefaultProject(string user)
        {
            string project;
            string connectionString = GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand("SELECT [Value] FROM dbo.Settings WHERE [Key] = 'project' AND [Username] = @Parameter;", connection);
                command.Parameters.Add(new SqlParameter("@Parameter", user));
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    project = reader.GetString(0);
                }
                else
                    project = null;

            }
            return project;
        }

        public static bool DoesFormatHaveCollarId(string formatCode)
        {
            DataRow[] rows = Formats.Select("Code = '" + formatCode + "'");
            if (rows.Length != 1)
                throw new Exception("Bad code");
            var hasCollarIdColumn = (string)rows[0]["HasCollarIdColumn"];
            return hasCollarIdColumn == "Y";
        }

        public static string CollarManufacturer(string formatCode)
        {
            DataRow[] rows = Formats.Select("Code = '" + formatCode + "'");
            if (rows.Length != 1)
                throw new Exception("Bad code");
            return (string)rows[0]["CollarManufacturer"];
        }

        public static void UpdateSetting(string key, string value)
        {
            string connectionString = GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand("[dbo].[UpdateSettings]", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Key", key));
                command.Parameters.Add(new SqlParameter("@Value", value));
                command.ExecuteNonQuery();
            }
        }

        public static bool IsEditor(string user, string project)
        {
            string connectionString = GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand("SELECT dbo.IsEditor(@ProjectParam, @UserParam)", connection);
                command.Parameters.Add(new SqlParameter("@UserParam", user));
                command.Parameters.Add(new SqlParameter("@ProjectParam", project));
                return (bool)command.ExecuteScalar();
            }
        }

        public static bool IsInvestigator(string user)
        {
            return Investigator.IsInvestigator(user);
        }

        public static bool IsInvestigator(string user, string projectId)
        {
            return Project.IsInvestigator(user, projectId);
        }

        public static void DeleteFile(int fileId)
        {
            string connectionString = GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand("[dbo].[CollarFile_Delete]", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@FileId", fileId));
                command.ExecuteNonQuery();
            }
        }

        public static void DeleteEditor(string project, string editor)
        {
            string connectionString = GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand("[dbo].[Editor_Delete]", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ProjectId", project));
                command.Parameters.Add(new SqlParameter("@Editor", editor));
                command.ExecuteNonQuery();
            }
        }

        public static void AddEditor(string currentUser, string project, string editor)
        {
            string connectionString = GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand("[dbo].[Editor_Insert]", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ProjectId", project));
                command.Parameters.Add(new SqlParameter("@Editor", editor));
                command.ExecuteNonQuery();
            }
        }

        public static void DeleteAnimal(string project, string animalId)
        {
            string connectionString = GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand("[dbo].[Animal_Delete]", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ProjectId", project));
                command.Parameters.Add(new SqlParameter("@AnimalId", animalId));
                command.ExecuteNonQuery();
            }
        }

        public static void AddAnimal(string project, string animal, string species, string gender, string group, string description)
        {
            string connectionString = GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand("[dbo].[Animal_Insert]", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ProjectId", project));
                command.Parameters.Add(new SqlParameter("@AnimalId", animal));
                command.Parameters.Add(new SqlParameter("@Species", species));
                command.Parameters.Add(new SqlParameter("@Gender", gender[0]));
                command.Parameters.Add(new SqlParameter("@GroupName", group));
                command.Parameters.Add(new SqlParameter("@Description", description));
                command.ExecuteNonQuery();
            }
        }

        public static string GetNextAnimalId(string project)
        {
            string connectionString = GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand("SELECT dbo.NextAnimalId(@ProjectParam)", connection);
                command.Parameters.Add(new SqlParameter("@ProjectParam", project));
                return (string)command.ExecuteScalar();
            }
        }


        public static void DeleteProject(string projectCode)
        {
            string connectionString = GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand("[dbo].[Project_Delete]", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ProjectId", projectCode));
                command.ExecuteNonQuery();
            }
            KnownProjectCodes.Remove(projectCode);
        }

        public static void AddProject(string code, string name, string investigator, string unit, string description)
        {
            string connectionString = GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand("[dbo].[Project_Insert]", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ProjectId", code));
                command.Parameters.Add(new SqlParameter("@ProjectName", name));
                //While the database will default to the current user, the case may not match. so we are explicit
                command.Parameters.Add(new SqlParameter("@ProjectInvestigator", investigator));
                command.Parameters.Add(new SqlParameter("@UnitCode", unit));
                command.Parameters.Add(new SqlParameter("@Description", description));
                command.ExecuteNonQuery();
            }
        }
    }
}
