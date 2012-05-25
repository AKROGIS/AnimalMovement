using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace AnimalMovementLibrary
{
    public class Investigator
    {
        public static Investigator FromName(string name)
        {
            if (_investigators == null)
                _investigators = BuildInvestigatorsFromDatabase();
            //FIXME case of name may not match database case.
            string key = name.ToLower();
            try
            {
                return _investigators[key];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        public static List<Investigator> Investigators
        {
            get
            {
                if (_investigators == null)
                    _investigators = BuildInvestigatorsFromDatabase();
                return _investigators.Values.ToList();
            }
        }
        private static Dictionary<string, Investigator> _investigators = null;

        private static Dictionary<string, Investigator> BuildInvestigatorsFromDatabase()
        {
            var investigators = new Dictionary<string, Investigator>();
            string connectionString = AML.GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand("SELECT [Login], [Name], [Email], [Phone] " +
                                               "FROM [dbo].[ProjectInvestigators];", connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string login = (string)reader["Login"];
                    string name = (string)reader["Name"];
                    string email = (string)reader["Email"];
                    string phone = (string)reader["Phone"];

                    investigators.Add(login.ToLower(), new Investigator(login, name, email, phone));
                }
            }
            return investigators;
        }

        public static bool IsInvestigator(string user)
        {
            //string connectionString = AML.GetConnectionString();
            //using (var connection = new SqlConnection(connectionString))
            //{
            //    connection.Open();

            //    var command = new SqlCommand("SELECT * FROM dbo.ProjectInvestigators WHERE UserName = @Parameter;", connection);
            //    command.Parameters.Add(new SqlParameter("@Parameter", user));
            //    SqlDataReader reader = command.ExecuteReader();
            //    return reader.HasRows;
            //}
            if (_investigators == null)
                _investigators = BuildInvestigatorsFromDatabase();
            return _investigators.ContainsKey(user.ToLower());
            //return _investigators.Keys.Any(k => string.Equals(k, user, StringComparison.OrdinalIgnoreCase));
        }

        public static void Refresh()
        {
            _investigators = BuildInvestigatorsFromDatabase();
        }

        private Investigator(string login, string name, string email, string phone)
        {
            Login = login;
            Name = name;
            Email = email;
            Phone = phone;
        }

        public string Login { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }

        //private static void BuildInvestigatorFromDatabase()
        //{
        //    string connectionString = AML.GetConnectionString();
        //    using (var connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();

        //        var command = new SqlCommand("SELECT [Login], [Name], [Email], [Phone] " +
        //                                       "FROM [dbo].[ProjectInvestigators] " +
        //                                      "WHERE [Login] = @LoginParam;", connection);
        //        command.Parameters.Add(new SqlParameter("@LoginParam", Login));
        //        SqlDataReader reader = command.ExecuteReader();
        //        if (!reader.HasRows)
        //            return;
        //        reader.Read();
        //        Name = (string)reader["Name"];
        //        Email = (string)reader["Email"];
        //        Phone = (string)reader["Phone"];
        //    }
        //}

        public override string ToString()
        {
            return Name;
        }
    }
}
