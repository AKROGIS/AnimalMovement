using System;
using System.IO;
using System.Linq;
using DataModel;
using FileLibrary;

namespace CollarFileLoader
{
    internal static class Program
    {
        /// <summary>
        /// Load multiple files or folder of files into the database.
        /// The collar is determined from the contents if possible and necessary.  It is not possible
        /// to explicitely specify the collar to use, because 1) in most cases it is not necessary,
        /// especially in the case of bulk loading a folder (the primary purpose of this command),
        /// and 2) it would be too easy to make errors that cannot be checked for.  Files will be
        /// loaded as active, and duplicates will not be allowed.  If you need more flexibility,
        /// then you should use the interface in the Animal Movements application.
        /// If a file contains Argos data, then it is also processed.  Any processing errors are
        /// written to the database, any other errors are written to the console.
        /// </summary>
        /// <param name="args">
        /// If no arguments are provided, then the program will attempt to load all files in current directory
        ///   and associated them with the user.  It is an error if the user is not a project investigator.
        /// If the argument matches /o[wner]:xxx or /m[anager]:xxx or xxx, and xxx is a login (domain\username) of a
        ///   valid project investigator, then the subsequent files will be associated with this project investigator.
        ///   Specifying a valid project investigator will clear a previously set project.  The person running this 
        ///   program must have the necessary database permissions (typically this means being the project investigator
        ///   or and assistant to the project investigator).
        /// If the argument matches /p[roject]:xxx or xxx, and xxx is a project id in the database then the remaining
        ///   files will be associated with this project. Specifying a valid project will clear a previously set project
        ///   investigator. The person running this program must have the necessary database permissions (typically this
        ///   means being an editor on the project).
        /// In the highly unlikely case that an argument is both a project id and a project investigator login, then
        ///   the tie will go to the project.
        /// If the argument is a file (relative or absolute) then that file is read and loaded if possible
        /// If the argument is a folder (relative or absolute) then all the files in the folder are read and loaded
        ///   if possible.
        /// All other args are ignored with a warning.
        /// </param>
        /// <remarks>
        /// If a file contains raw Gen4 data and if a copy of TDC.exe is NOT available (the path is specified
        /// in the config file) then this program will request that the database invoke the file processor on the server,
        /// otherwise, this program will process the file locally and send the results to the database.
        /// </remarks>
        private static void Main(string[] args)
        {
            try
            {
                string caller = Environment.UserDomainName + @"\" + Environment.UserName;
                ProjectInvestigator owner = GetOwner(caller);
                if (args.Length == 0)
                    if (owner != null)
                        FileLoader.LoadPath(Environment.CurrentDirectory, HandleException, null, owner);
                    else
                        Console.WriteLine(
                            "Since you ({0}) are not a project investigator, you must specify a project or an owner to load files",
                            caller);
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
                                Console.WriteLine(
                                    "Since you ({0}) are not a project investigator, you must specify a project or an owner before a path",
                                    caller);
                            else
                                FileLoader.LoadPath(path, HandleException, project, owner);
                            continue;
                        }
                        Console.WriteLine("argument: {0} is not a known project, project investigator or path", arg);
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
            Console.WriteLine("Unable to load file: {0} for project: {1} or manager: {2} reason: {3}", path,
                project == null ? "<null>" : project.ProjectId, manager == null ? "<null>" : manager.Login, ex.Message);
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
