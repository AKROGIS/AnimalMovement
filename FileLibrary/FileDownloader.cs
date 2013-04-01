using System;
using System.Linq;
using DataModel;
using Telonics;

namespace FileLibrary
{
    public static class FileDownloader
    {
        private const int MinDays = 1;
        private const int MaxDays = 10;

        /// <summary>
        /// Download programs and platforms
        /// </summary>
        /// <param name="handler">An exception handler to allow processing additional items despite an exception.
        /// If the handler is null, processing will stop on first exception.
        /// The handler can throw it's own exception to halt further processing</param>
        /// <param name="user">The name (loginId) of a project investigator.
        /// If provided, only the programs/platforms for that PI will be downloaded,
        /// otherwise all programs/platforms will be downloaded</param>
        public static void DownloadAll(Action<Exception, string, string> handler = null, string user = null)
        {
            var db = new AnimalMovementDataContext();
            foreach (var program in db.ArgosPrograms.Where(p => (user == null || user == p.Manager) && p.Active.HasValue && p.Active.Value))
            {
                try
                {
                    DownloadArgosProgram(program);
                }
                catch (Exception ex)
                {
                    if (handler == null)
                        throw;
                    handler(ex, program.ProgramId, null);
                }
            }
            foreach (var platform in db.ArgosPlatforms.Where(p => (user == null || user == p.ArgosProgram.Manager) && p.Active && !p.ArgosProgram.Active.HasValue))
            {
                try
                {
                    DownloadArgosPlatform(platform);
                }
                catch (Exception ex)
                {
                    if (handler == null)
                        throw;
                    handler(ex, null, platform.ProgramId);
                }
            }
        }


        public static void DownloadArgosProgram(ArgosProgram program, int? daysToRetrieve = null)
        {
            int daysSinceLastDownload;
            if (daysToRetrieve.HasValue)
                daysSinceLastDownload = daysToRetrieve.Value;
            else
            {
                var database = new AnimalMovementDataContext();
                var dateOfLastDownload = (from log in database.ArgosDownloads
                                          where log.ProgramId == program.ProgramId && log.FileId != null
                                          orderby log.TimeStamp descending
                                          select log.TimeStamp).FirstOrDefault();
                daysSinceLastDownload = (DateTime.Now - dateOfLastDownload).Days;
            }
            var days = Math.Min(MaxDays, daysSinceLastDownload);
            if (days < MinDays)
                return;
            string errors;
            var results = ArgosWebSite.GetProgram(program.UserName, program.Password, program.ProgramId, days,
                                                 out errors);
            CollarFile file = FileLoader.LoadProgram(program, days, results, errors);
            if (file != null)
            {
                //FIXME- is this a local or server process?? who sets the TDC path?? 
                var processor = new FileProcessor();
                processor.ProcessFile(file);
            }
        }


        public static void DownloadArgosPlatform(ArgosPlatform platform, int? daysToRetrieve = null)
        {
            int daysSinceLastDownload;
            if (daysToRetrieve.HasValue)
                daysSinceLastDownload = daysToRetrieve.Value;
            else
            {
                var database = new AnimalMovementDataContext();
                var dateOfLastDownload = (from log in database.ArgosDownloads
                                          where log.PlatformId == platform.PlatformId && log.FileId != null
                                          orderby log.TimeStamp descending
                                          select log.TimeStamp).FirstOrDefault();
                daysSinceLastDownload = (DateTime.Now - dateOfLastDownload).Days;
            }
            var days = Math.Min(MaxDays, daysSinceLastDownload);
            if (days < MinDays)
                return;
            var program = platform.ArgosProgram;
            string errors;
            var results = ArgosWebSite.GetCollar(program.UserName, program.Password, platform.PlatformId, days,
                                                 out errors);
            CollarFile file = FileLoader.LoadPlatfrom(platform, days, results, errors);
            if (file != null)
            {
                //FIXME- is this a local or server process?? who sets the TDC path?? 
                var processor = new FileProcessor();
                processor.ProcessFile(file);
            }
        }
    }
}

