using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace AnimalMovementLibrary
{
    public class Project
    {
        public static Project FromName(string name)
        {
            if (_projects == null)
                _projects = BuildProjectsFromDatabase();
            return _projects[name];
        }

        public static List<Project> Projects
        {
            get
            {
                if (_projects == null)
                    _projects = BuildProjectsFromDatabase();
                return _projects.Values.ToList();
            }
        }

        private static Dictionary<string, Project> _projects = null;

        private static Dictionary<string, Project> BuildProjectsFromDatabase()
        {
            var projects = new Dictionary<string, Project>();
            string connectionString = AML.GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand("SELECT [ProjectId], [ProjectName], [ProjectInvestigator], [UnitCode], [Description] " +
                                               "FROM [dbo].[Projects];", connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string id = (string)reader["ProjectId"];
                    string name = (string)reader["ProjectName"];
                    string investigator = (string)reader["ProjectInvestigator"];
                    string unit = null;
                    if (!reader.IsDBNull(3))
                        unit = (string)reader["UnitCode"];
                    string description = null;
                    if (!reader.IsDBNull(4))
                        description = (string)reader["Description"];

                    projects.Add(id, new Project(id, name, investigator, unit, description));
                }
            }
            return projects;
        }


        private Project(string id, string name, string investigator, string unit, string description)
        {
            Id = id;
            Name = name;
            InvestigatorId = investigator;
            Investigator = Investigator.FromName(InvestigatorId);
            Unit = unit;
            Description = description;
        }

        //public Project(string id)
        //{
        //    Id = id;
        //    BuildProjectFromDatabase();
        //}

        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string InvestigatorId { get; private set; }
        public Investigator Investigator { get; private set; }
        public string Unit { get; private set; }

        public DataTable Animals { get; private set; }
        public DataTable Editors { get; private set; }
        public DataTable Files { get; private set; }

        public static void Refresh()
        {
            _projects = BuildProjectsFromDatabase();
        }

        //private void BuildProjectFromDatabase()
        //{
        //    string connectionString = AML.GetConnectionString();
        //    using (var connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();

        //        //var command = new SqlCommand("SELECT [ProjectName],[UnitCode],[ProjectInvestigator],[Name],[Description] " +
        //        //                               "FROM [dbo].[Projects] AS [P] " +
        //        //                         "INNER JOIN [dbo].[ProjectInvestigators] AS [I] " +
        //        //                                 "ON [P].[ProjectInvestigator] = [I].[Login] " +
        //        //                              "WHERE [ProjectId] = @ProjectParam;", connection);
        //        var command = new SqlCommand("SELECT [ProjectName],[UnitCode],[ProjectInvestigator],[Description] " +
        //                                       "FROM [dbo].[Projects] " +
        //                                      "WHERE [ProjectId] = @ProjectParam;", connection);
        //        command.Parameters.Add(new SqlParameter("@ProjectParam", Id));
        //        SqlDataReader reader = command.ExecuteReader();
        //        if (!reader.HasRows)
        //            return;
        //        reader.Read();
        //        Name = (string)reader["ProjectName"];
        //        Description = (string)reader["Description"];
        //        InvestigatorId = (string)reader["ProjectInvestigator"];
        //        Investigator = Investigator.FromName(InvestigatorId);
        //        Unit = (string)reader["UnitCode"];
        //    }
        //    Animals = Animal.AnimalsForProject(Id);
        //    Editors = Editor.EditorsForProject(Id);
        //    Files = CollarFile.FilesForProject(Id);
        //}

        public void UpdateProject(string name, string investigator, string unit, string description)
        {
            Name = name;
            Description = description;
            InvestigatorId = investigator;
            Investigator = Investigator.FromName(InvestigatorId);
            Unit = unit;

            string connectionString = AML.GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand("[dbo].[Project_Delete]", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ProjectId", Id));
                command.Parameters.Add(new SqlParameter("@ProjectName", name));
                command.Parameters.Add(new SqlParameter("@ProjectInvestigator", investigator));
                command.Parameters.Add(new SqlParameter("@UnitCode", unit));
                command.Parameters.Add(new SqlParameter("@Description", description));
                command.ExecuteNonQuery();
            }
            Refresh();
        }

        public void RefreshAnimals()
        {
            Animals = Animal.AnimalsForProject(Id);
          
        }

        public void RefreshEditors()
        {
            Editors = Editor.EditorsForProject(Id);            
        }

        public void RefreshFiles()
        {
            Files = CollarFile.FilesForProject(Id);            
        }

        public static bool IsInvestigator(string user, string projectId)
        {
            //string connectionString = AML.GetConnectionString();
            //using (var connection = new SqlConnection(connectionString))
            //{
            //    connection.Open();

            //    var command = new SqlCommand("SELECT * FROM dbo.Projects WHERE ProjectId = @ProjectParam AND ProjectInvestigator = @UserParam;", connection);
            //    command.Parameters.Add(new SqlParameter("@ProjectParam", project));
            //    command.Parameters.Add(new SqlParameter("@UserParam", user));
            //    SqlDataReader reader = command.ExecuteReader();
            //    return reader.HasRows;
            //}
            return string.Equals(FromName(projectId).InvestigatorId, user, StringComparison.OrdinalIgnoreCase);
        }

    }
}
