using System;
using System.Linq;
using System.IO;
using DataModel;
using FileLibrary;

namespace ArgosProcessor
{
    internal static class Program
    {
        /// <summary>
        /// This program may be run by a user from the command line to load multiple files/folder of files.
        /// The program may also be used by a user to process all unprocessed files.
        /// This program may be called by the database with a single integer argument which will be a
        /// FileId for a record in the CollarFiles table.
        /// If the file(s) is an Argos file (email or downloads), then the file is summerized and processed
        /// locally or on the server depending on the availablity of the TDC.exe for processing.
        /// The processing determines which collars were using the argos platform during the period of the
        /// transmissions, and extracts the GPS locations for each transmission based on the active parameters
        /// for the collar during each range of transmissions.
        /// Any errors will be written to the Console.  The database obtains the console output from this program.
        /// Nothing is written to the console on success.
        /// The processor will write any file specific issues, i.e. unknown platform, missing parameters, etc
        /// to the Argos to the ArgosFileProcessingIssues table in the database
        /// </summary>
        /// <param name="args">
        /// This program takes zero or more arguments.
        /// If there are no arguments, then the database is queried to get all files that need processing.
        /// for each arg that is an int, the int is assumed to be a FileId in the CollarFiles table
        /// for each arg that is a path (file or folder), the file (or all files in the folder) is
        ///   loaded into the CollarFiles table and then processed.
        /// an the arg in the form /p:XXXX, defines the project to which subsequent files are loaded
        /// any other args are ignored with a warning.
        /// </param>
        /// <remarks>
        /// If a copy of TDC.exe is available from this program, then the FileProcessor will be
        /// make use of it to process the files locally and send the results to the database.
        /// If TDC.exe is not available, then the FileProcessor will request that the database
        /// invoke the file processor on the server.
        /// </remarks>
        private static void Main(string[] args)
        {
            try
            {
                //TODO check availability of TDC.exe (and options like TDC timeout) to set FileProcessor options
                var processor = new FileProcessor { ProcessLocally = true };

                if (args.Length == 0)
                    processor.ProcessAll();
                else
                {
                    Project project = null;
                    foreach (var arg in args)
                    {
                        try
                        {
                            int id;
                            string path;
                            if (Int32.TryParse(arg, out id) && 0 < id)
                                processor.ProcessId(id);
                            else if (arg.StartsWith("/p:"))
                                project = GetProject(arg.Substring(3));
                            else if ((path = GetFullPath(arg)) != null)
                                processor.ProcessPath(path, project);
                            else
                                Console.WriteLine("Ignoring argument '{0}'.  Each argument must be an integer, file, folder or /p:project", arg);
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
