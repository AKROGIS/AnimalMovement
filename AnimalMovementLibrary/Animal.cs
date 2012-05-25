using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace AnimalMovementLibrary
{
    public class Animal
    {
        public Animal(string projectId, string animalId)
        {
            ProjectId = projectId;
            Id = animalId;
            BuildAnimalFromDatabase();
        }

        public string Id { get; private set; }
        public string ProjectId { get; private set; }
        public string Species { get; private set; }
        public string Gender { get; private set; }
        public string Group { get; private set; }
        public string Description { get; private set; }

        public void Refresh()
        {
            BuildAnimalFromDatabase();
        }

        private void BuildAnimalFromDatabase()
        {
            string connectionString = AML.GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand("SELECT [Species],[Gender],[GroupName],[Description]  FROM [dbo].[Animals] WHERE [ProjectId] = @ProjectParam AND [AnimalId] = @AnimalParam;", connection);
                command.Parameters.Add(new SqlParameter("@ProjectParam", ProjectId));
                command.Parameters.Add(new SqlParameter("@AnimalParam", Id));
                SqlDataReader reader = command.ExecuteReader();
                if (!reader.HasRows)
                    return;
                reader.Read();
                Species = (string)reader["Species"];
                Gender = (string)reader["Gender"];
                Group = (string)reader["GroupName"];
                Description = (string)reader["Description"];
            }
        }

        public static DataTable AnimalsForProject(string projectId)
        {
            DataTable data;
            string connectionString = AML.GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand("SELECT [AnimalId],[Species],[Gender],[GroupName],[Description] FROM [dbo].[Animals] WHERE [ProjectId] = @ProjectParam;", connection);
                command.Parameters.Add(new SqlParameter("@ProjectParam", projectId));
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                data = new DataTable("Animals");
                data.Load(reader);
            }
            return data;
        }
    }
}
