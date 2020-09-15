using System;
using System.Linq;
using DataModel;
using FileLibrary;

namespace ArgosProcessor
{
    internal static class Program
    {
        /// <summary>
        /// Full or partially processes telonics data in argos files.  This program is designed to run regularly
        /// as a scheduled task (with no arguments) as well as being called by the database with a single fileid
        /// argument (to fully process that file), or with two arguments, platformid and fileid (to partially
        /// process that file for just the single platform).  When a file is fully or partially processed, it
        /// clears any prior processing results and issues, to prevent duplicates.
        /// Any processing errors are written to the database, any other errors are written to the console.
        /// </summary>
        /// <param name="args">
        /// If there are no args then ask the database for a list of files that need processing and process them all.
        /// If there is only one argument and that argument is the login (domain\username) of a project
        ///   investigator in the database, then process all the files that belong to the PI or his projects and
        ///   need processing.
        /// If the argument matches /np[latform], or '/na[rgos]', then the remaining files will be fully processed
        /// If the argument matches /nf[ile], then the next argos platform specified will not be used to
        ///   partially process the previously specified file
        /// If the argument matches /a[rgos]:xxx or /p[latform]:xxx or xxx and xxx is a platform id in the
        ///   database, then the previously specified file (or the next file if no has been specified) will be
        ///   partially processed for only this platform.
        /// if the argument matches /f[ile]:xxx or xxx, and xxx is a fileid in the database, then the file will
        ///   be partially processed if an argos platform has been provided as a prior argument, otherwise it will
        ///   be fully processed.
        /// In the unlikely event that an argument matches a valid platform and a valid file, then the tie will go
        ///   to the file.
        /// Any other argument is ignored with a warning
        /// Note:
        ///   A project investigator login will be accepted as an argument in any position, but is only
        ///     meaningful as the first and only argument
        ///   If you want to partially process a file, you must provide the Argos platform first
        /// Examples:
        ///   To fully process multiple files
        ///     ArgosProcessor,exe f1 f2 f3 ...
        ///   To partially process multiple files with for one Argos platform
        ///     ArgosProcessor,exe p1 f1 f2 f3 ...
        ///   To partially process one file for multiple Argos platforms
        ///     ArgosProcessor,exe p1 f1 p2 p3 ...
        ///   To partially process multiple files with multiple Argos platforms
        ///     ArgosProcessor,exe p1 f1 p2 p3 /nf p4 f2 p5 p6 ...
        ///   To fully process files f1 and f2 and partially process files f3 and f4
        ///     ArgosProcessor,exe f1 f2 p1 f3 p2 /nf p3 f4 p4 ...
        ///     or ArgosProcessor,exe p1 f3 p2 /nf p3 f4 p4 /np f1 f2 ...
        /// </param>
        /// <remarks>
        /// If a copy of TDC.exe is available from this program (as specified in the config file),
        /// then the processor will be make use of it to process the files locally and send the
        /// results to the database, otherwise the processor will request that the database
        /// invoke the ArgosProcessor on the server.
        /// </remarks>
        private static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                    FileProcessor.ProcessAll(HandleException);
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
                            FileProcessor.ProcessAll(HandleException, pi);
                        if (file != null)
                            try
                            {
                                FileProcessor.ProcessFile(file, platform);
                            }
                            catch (Exception ex)
                            {
                                HandleException(ex, file, platform);
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unhandled Exception: {0}", ex.Message);
            }
        }

        private static void HandleException(Exception ex, CollarFile file, ArgosPlatform platform)
        {
            if (file == null)
            {
                Console.WriteLine("Processor exception handler called without a file: {0}", ex.Message);
                return;
            }
            if (file.LookupCollarFileFormat.ArgosData != 'Y')
            {
                Console.WriteLine("Processor exception handler called with a Non-Argos Data file({1}): {0}", ex.Message, file.FileId);
                return;
            }
            var db = new AnimalMovementDataContext();
            db.ArgosFileProcessingIssues_Insert(file.FileId, ex.Message, platform?.PlatformId,
                                                null, null, null, null);
        }

        private static bool ClearCollarFile(string arg)
        {
            arg = arg.Normalize();
            return arg.Equals("/nf", StringComparison.OrdinalIgnoreCase) ||
                   arg.Equals("/nofile", StringComparison.OrdinalIgnoreCase);
        }

        private static bool ClearPlatform(string arg)
        {
            arg = arg.Normalize();
            return arg.Equals("/np", StringComparison.OrdinalIgnoreCase) ||
                   arg.Equals("/noplatform", StringComparison.OrdinalIgnoreCase) ||
                   arg.Equals("/na", StringComparison.OrdinalIgnoreCase) ||
                   arg.Equals("/noargos", StringComparison.OrdinalIgnoreCase);
        }

        private static CollarFile GetCollarFile(string fileId)
        {
            fileId = fileId.Normalize();
            if (fileId.StartsWith("/f:", StringComparison.OrdinalIgnoreCase))
                fileId = fileId.Substring(3);
            if (fileId.StartsWith("/file:", StringComparison.OrdinalIgnoreCase))
                fileId = fileId.Substring(6);
            if (!Int32.TryParse(fileId, out int id) || id < 1)
                return null;
            var database = new AnimalMovementDataContext();
            return (from collar in database.CollarFiles
                    where (collar.LookupCollarFileFormat.Code == 'H' || collar.LookupCollarFileFormat.ArgosData == 'Y') && collar.FileId == id
                    select collar).FirstOrDefault();
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
