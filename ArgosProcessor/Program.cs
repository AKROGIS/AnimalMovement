using System;
using System.Linq;
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
                    processor.ProcessAll(handleException);
                else
                {
                    ArgosPlatform platform = null;
                    CollarFile file = null;
                    ProjectInvestigator pi = null;
                    foreach (var arg in args)
                    {
                        if (ClearPlatform(arg))
                            platform = null;
                        else
                        {
                            if (ClearCollarFile(arg))
                                file = null;
                            else
                            {
                                var fileArg = GetCollarFile(arg);
                                if (fileArg != null)
                                    file = fileArg;
                                else
                                {
                                    var platformArg = GetPlatform(arg);
                                    if (platformArg != null)
                                        platform = platformArg;
                                    else
                                    {
                                        var piArg = GetProjectInvestigator(arg);
                                        if (piArg != null)
                                            pi = piArg;
                                        else
                                            Console.WriteLine("Unhandled argument: {0}", arg);
                                    }
                                }
                            }
                        }
                        if (args.Length == 1 && pi != null)
                            processor.ProcessAll(handleException, pi);
                        if (file != null && platform != null)
                            try
                            {
                                processor.ProcessPartialFile(file, platform);
                            }
                            catch (Exception ex)
                            {
                                handleException(ex, file, platform);
                            }
                        if (file != null && platform == null)
                            try
                            {
                                processor.ProcessFile(file);
                            }
                            catch (Exception ex)
                            {
                                handleException(ex, file, null);
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unhandled Exception: {0}", ex.Message);
            }
        }

        private static void handleException(Exception ex, CollarFile file, ArgosPlatform platform)
        {
            throw new NotImplementedException();
        }

        private static bool ClearCollarFile(string arg)
        {
            arg = arg.Normalize();
            return (arg.Equals("/nf", StringComparison.OrdinalIgnoreCase) ||
                    arg.Equals("/nofile", StringComparison.OrdinalIgnoreCase));
        }

        private static bool ClearPlatform(string arg)
        {
            arg = arg.Normalize();
            return (arg.Equals("/np", StringComparison.OrdinalIgnoreCase) ||
                    arg.Equals("/noplatform", StringComparison.OrdinalIgnoreCase) ||
                    arg.Equals("/na", StringComparison.OrdinalIgnoreCase) ||
                    arg.Equals("/noargos", StringComparison.OrdinalIgnoreCase));
        }

        private static CollarFile GetCollarFile(string fileId)
        {
            fileId = fileId.Normalize();
            if (fileId.StartsWith("/f:", StringComparison.OrdinalIgnoreCase))
                fileId = fileId.Substring(3);
            if (fileId.StartsWith("/file:", StringComparison.OrdinalIgnoreCase))
                fileId = fileId.Substring(6);
            int id;
            if (!Int32.TryParse(fileId, out id) || id < 1)
                return null;
            var database = new AnimalMovementDataContext();
            //FIXME only get files with Argos Data
            return database.CollarFiles.FirstOrDefault(f => f.FileId == id);
        }

        private static ArgosPlatform GetPlatform(string platformId)
        {
            platformId = platformId.Normalize();
            if (platformId.StartsWith("/a:", StringComparison.OrdinalIgnoreCase))
                platformId = platformId.Substring(3);
            if (platformId.StartsWith("/argos:", StringComparison.OrdinalIgnoreCase))
                platformId = platformId.Substring(7);
            if (platformId.StartsWith("/p:", StringComparison.OrdinalIgnoreCase))
                platformId = platformId.Substring(3);
            if (platformId.StartsWith("/platform:", StringComparison.OrdinalIgnoreCase))
                platformId = platformId.Substring(10);
            var database = new AnimalMovementDataContext();
            return database.ArgosPlatforms.FirstOrDefault(p => p.PlatformId == platformId);
        }

        private static ProjectInvestigator GetProjectInvestigator(string pi)
        {
            var database = new AnimalMovementDataContext();
            return database.ProjectInvestigators.FirstOrDefault(p => p.Login == pi);
        }
    }
}
