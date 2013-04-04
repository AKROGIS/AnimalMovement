using System;
using System.IO;
using System.Linq;
using DataModel;
using FileLibrary;

namespace CollarFileLoader
{
    static class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                    FileLoader.LoadFolder(Environment.CurrentDirectory);
                else
                {
                    Project project = null;
                    foreach (var arg in args)
                    {
                        try
                        {
                            string path;
                            if (arg.StartsWith("/p:"))
                                project = GetProject(arg.Substring(3));
                            else if ((path = GetFullPath(arg)) != null)
                                FileLoader.LoadPath(path, project);
                            else
                                Console.WriteLine("Ignoring argument '{0}'.  Each argument must be a file, folder or /p:ProjectId", arg);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Exception {0} on argument '{1}'.", ex.Message, arg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unhandled Exception: {0}", ex.Message);
            }
        }

        private static Project GetProject(string projectId)
        {
            var database = new AnimalMovementDataContext();
            return database.Projects.FirstOrDefault(p => p.ProjectId == projectId);
        }

        private static string GetFullPath(string potentialPath)
        {
            if (File.Exists(potentialPath) || Directory.Exists(potentialPath))
                return potentialPath;
            try
            {
                if (Path.IsPathRooted(potentialPath))
                    return null;
                var fullPath = Path.Combine(Environment.CurrentDirectory, potentialPath);
                if (File.Exists(fullPath) || Directory.Exists(fullPath))
                    return potentialPath;
            }
            catch (ArgumentException)
            {
                //Thrown by Path.IsPathRooted, or Path.Combine if the provided path contains invalid characters
            }
            return null;
        }
    }
}
