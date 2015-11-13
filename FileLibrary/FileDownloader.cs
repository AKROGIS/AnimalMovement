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


        /// <summary>
        /// Retrieves data for an Argos Program from the Argos Web Server
        /// </summary>
        /// <param name="program">The Argos Program (contains Platforms) to retrieve from the server</param>
        /// <param name="daysToRetrieve">The number of most recent days to retrieve from the server</param>
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
                //Problem: using Days always truncates.
                // If I download at 1am on day 1, and then 11pm on day 2, (1.9 days -> 1day),
                //   I will miss any data created between 1am and 11pm on day 1.
                // However, rounding up may cause a lot of duplication,
                // (i.e. 6:01am on day1 and then 6:02am on day2, will need to download 2 days to get the extra minute)
                // by default we should round up to make sure that all data is obtained.  However, since this is
                // typically called on a scheduled task at the same time (+/- download/processing time) everyday.
                // I will check if we are close to an even day, and then round tword that day
                var timespan = DateTime.Now - dateOfLastDownload;
                int extraDay = timespan.Hours == 0 && timespan.Minutes < 5 ? 0 : 1;
                daysSinceLastDownload = timespan.Days + extraDay;
            }
            var days = Math.Min(ArgosWebSite.MaxDays, daysSinceLastDownload);
            if (days < ArgosWebSite.MinDays)
                return;
            string errors;
            var results = ArgosWebSite.GetProgram(program.UserName, program.Password, program.ProgramId, days,
                                                 out errors);
            CollarFile file = FileLoader.LoadProgram(program, days, results, errors);
            if (file == null)
                return;
            FileProcessor.ProcessFile(file);
        }


        /// <summary>
        /// Retrieves data for an Argos Platform from the Argos Web Server
        /// </summary>
        /// <param name="platform">The Argos Platform to retrieve from the server</param>
        /// <param name="daysToRetrieve">The number of most recent days to retrieve from the server</param>
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
            var days = Math.Min(ArgosWebSite.MaxDays, daysSinceLastDownload);
            if (days < ArgosWebSite.MinDays)
                return;
            var program = platform.ArgosProgram;
            string errors;
            var results = ArgosWebSite.GetCollar(program.UserName, program.Password, platform.PlatformId, days,
                                                 out errors);
            CollarFile file = FileLoader.LoadPlatfrom(platform, days, results, errors);
            if (file == null)
                return;
            FileProcessor.ProcessFile(file);
        }
    }
}

