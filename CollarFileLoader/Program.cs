using System;
using System.IO;
using System.Linq;
using DataModel;
using FileLibrary;

namespace CollarFileLoader
{
    static class Program
    {
        //TODO - cleanup this documentation!
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
        static void Main(string[] args)
        {
            try
            {
                string caller = Environment.UserDomainName + @"\" + Environment.UserName;
                ProjectInvestigator owner = GetOwner(caller);
                if (args.Length == 0)
                    if (owner != null)
                        FileLoader.LoadPath(Environment.CurrentDirectory, HandleException, null, owner);
                    else
                        Console.WriteLine("Since you ({0}) are not a project investigator, you must specify a project or an owner to load files", caller);
                else
                {
                    Project project = null;
                    foreach (var arg in args)
                    {
                        var projectArg = GetProject(arg);
                        if (projectArg != null)
                        {
                            project = projectArg;
                            owner = null;
                            continue;
                        }
                        var ownerArg = GetOwner(arg);
                        if (ownerArg != null)
                        {
                            owner = ownerArg;
                            project = null;
                            continue;
                        }
                        var path = GetFullPath(arg);
                        if (path != null)
                        {
                            if (project == null && owner == null)
                                Console.WriteLine("Since you ({0}) are not a project investigator, you must specify a project or an owner before a path", caller);
                            else
                                FileLoader.LoadPath(path, HandleException, project, owner);
                            continue;
                        }
                        Console.WriteLine("argument: {0} is not a known project, project investigator or path",arg);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unhandled Exception: {0}", ex.Message);
            }
        }

        private static void HandleException(Exception ex, string path, Project project, ProjectInvestigator manager)
        {
            Console.WriteLine("Unable to load file: {0} for project: {1} or manager: {2} reason: {3}", path, project.ProjectId, manager.Login, ex.Message);
            return;
        }

        private static Project GetProject(string projectId)
        {
            projectId = projectId.Normalize();
            if (projectId.StartsWith("/p:", StringComparison.OrdinalIgnoreCase))
                projectId = projectId.Substring(3);
            if (projectId.StartsWith("/project:", StringComparison.OrdinalIgnoreCase))
                projectId = projectId.Substring(9);
            var database = new AnimalMovementDataContext();
            return database.Projects.FirstOrDefault(p => p.ProjectId == projectId);
        }

        private static ProjectInvestigator GetOwner(string owner)
        {
            owner = owner.Normalize();
            if (owner.StartsWith("/m:", StringComparison.OrdinalIgnoreCase))
                owner = owner.Substring(3);
            if (owner.StartsWith("/manager:", StringComparison.OrdinalIgnoreCase))
                owner = owner.Substring(9);
            if (owner.StartsWith("/o:", StringComparison.OrdinalIgnoreCase))
                owner = owner.Substring(3);
            if (owner.StartsWith("/owner:", StringComparison.OrdinalIgnoreCase))
                owner = owner.Substring(7);
            var database = new AnimalMovementDataContext();
            return database.ProjectInvestigators.FirstOrDefault(p => p.Login == owner);
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
