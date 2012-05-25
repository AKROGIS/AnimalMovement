using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace AnimalMovementLibrary
{
    public class Editor
    {
        public static DataTable EditorsForProject(string projectId)
        {
            DataTable data;
            string connectionString = AML.GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand("SELECT [Editor] FROM [dbo].[ProjectEditors] WHERE [ProjectId] = @ProjectParam;", connection);
                command.Parameters.Add(new SqlParameter("@ProjectParam", projectId));
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                data = new DataTable("Editors");
                data.Load(reader);
            }
            return data;
        }
    }
}
