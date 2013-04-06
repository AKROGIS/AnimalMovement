using System;
using System.Linq;
using DataModel;
using FileLibrary;

namespace ArgosProcessor
{
    internal static class Program
    {
        /// <summary>
        /// Full or partially processes telonics data in argos files
        /// </summary>
        /// <param name="args">
        /// FIXME - document arguments
        /// !!! must provide platform first to do partial processing
        /// </param>
        private static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                    FileProcessor.ProcessAll(handleException);
                else
                {
                    //TODO build a dictionary of platforms for collars
                    // c1 p1 p2 c2 c3 c4 p2 p3 p4
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
                            FileProcessor.ProcessAll(handleException, pi);
                        if (file != null)
                            try
                            {
                                FileProcessor.ProcessFile(file, platform);
                            }
                            catch (Exception ex)
                            {
                                handleException(ex, file, platform);
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
            if (file == null)
            {
                Console.WriteLine("Processor exception handler called without a file: {0}", ex.Message);
                return;
            }
            var db = new AnimalMovementDataContext();
            db.ArgosFileProcessingIssues_Insert(file.FileId, ex.Message, platform == null ? null : platform.PlatformId,
                                                null, null);
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
            return (from collar in database.CollarFiles
                    where collar.LookupCollarFileFormat.ArgosData == 'Y' && collar.FileId == id
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
