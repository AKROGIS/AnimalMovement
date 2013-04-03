using System;
using System.Linq;
using DataModel;
using Telonics;

namespace FileLibrary
{
    public static class FileDownloader
    {
        /// <summary>
        /// Download programs and/or platforms
        /// </summary>
        /// <param name="handler">An exception handler to allow processing additional items despite an exception.
        /// If the handler is null, processing will stop on first exception.
        /// The handler can throw it's own exception to halt further processing</param>
        /// <param name="pi">A project investigator.
        /// If provided, only the programs/platforms for that PI will be downloaded,
        /// otherwise all programs/platforms will be downloaded</param>
        /// <param name="daysToRetrieve">The number of days to download.  If it is null, then the database is consulted
        /// to get the number of days since the last successful download.  If the number is less than MinDays then nothing is
        /// downloaded (the server is not contacted at all).  If the number is truncated to MaxDays.
        /// MinDays and MaxDays are set in the config file of the calling application.</param>
        public static void DownloadAll(Action<Exception, ArgosProgram, ArgosPlatform> handler = null, ProjectInvestigator pi = null, int? daysToRetrieve = null)
        {
            var db = new AnimalMovementDataContext();
            foreach (var program in db.ArgosPrograms.Where(p => (pi == null || pi == p.ProjectInvestigator) && p.Active.HasValue && p.Active.Value))
            {
                try
                {
                    DownloadArgosProgram(program, daysToRetrieve);
                }
                catch (Exception ex)
                {
                    if (handler == null)
                        throw;
                    handler(ex, program, null);
                }
            }
            foreach (var platform in db.ArgosPlatforms.Where(p => (pi == null || pi == p.ArgosProgram.ProjectInvestigator) && p.Active && !p.ArgosProgram.Active.HasValue))
            {
                try
                {
                    DownloadArgosPlatform(platform, daysToRetrieve);
                }
                catch (Exception ex)
                {
                    if (handler == null)
                        throw;
                    handler(ex, null, platform);
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
            var days = Math.Min(Properties.Settings.Default.ArgosServerMaxDownloadDays, daysSinceLastDownload);
            if (days < Properties.Settings.Default.ArgosServerMinDownloadDays)
                return;
            string errors;
            var results = ArgosWebSite.GetProgram(program.UserName, program.Password, program.ProgramId, days,
                                                 out errors);
            CollarFile file = FileLoader.LoadProgram(program, days, results, errors);
            if (file == null)
                return;
            new FileProcessor().ProcessFile(file);
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
            var days = Math.Min(Properties.Settings.Default.ArgosServerMaxDownloadDays, daysSinceLastDownload);
            if (days < Properties.Settings.Default.ArgosServerMinDownloadDays)
                return;
            var program = platform.ArgosProgram;
            string errors;
            var results = ArgosWebSite.GetCollar(program.UserName, program.Password, platform.PlatformId, days,
                                                 out errors);
            CollarFile file = FileLoader.LoadPlatfrom(platform, days, results, errors);
            if (file == null)
                return;
            new FileProcessor().ProcessFile(file);
        }
    }
}

