using System;
using System.IO;
using Telonics;

namespace ArgosDownloader
{
    internal class Program
    {
        //This program is meant to be run periodically by a scheduler
        //It takes no command line arguments
        private static void Main()
        {
            // FIXME - get database connection from config file
            // FIXME - Get input from database
            const string username = "BURCH";
            const string password = "LOUGAROU";
            const string argosId = "37470";
            const int days = 10;
            string errors;
            var results = ArgosWebSite.GetCollar(username, password, argosId, days, out errors);
            if (results == null)
            {
                Console.WriteLine(errors);
            }
            else
            {
                string path = Path.Combine(@"C:\tmp", argosId + ".aws"); //path for the output file
                File.WriteAllText(path, results.ToString());
            }
            // FIXME - upload this file to database as type AWS
            // FIXME - update database with success/failure
        }
    }
}
