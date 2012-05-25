using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace AnimalMovementLibrary
{
    public class CollarFile
    {
        public static DataTable FilesForProject(string projectId)
        {
            DataTable data;
            string connectionString = AML.GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                //var command = new SqlCommand("SELECT [FileId],[FileName],[UploadDate],[UserName],[CollarManufacturer],[CollarId],[Format],[Status] FROM [dbo].[CollarFiles] WHERE [ProjectId] = @ProjectParam;", connection);
                var command = new SqlCommand("SELECT [FileId],[FileName],[Status] FROM [dbo].[CollarFiles] WHERE [Project] = @ProjectParam;", connection);
                command.Parameters.Add(new SqlParameter("@ProjectParam", projectId));
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                data = new DataTable("CollarFiles");
                data.Load(reader);
            }
            return data;
        }
    }
}
