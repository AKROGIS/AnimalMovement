using System;
using System.Linq;
using System.Collections.Generic;
using DataModel;
using Telonics;

namespace FileLibrary
{
    public static class ArgosDownloader
    {
        private const int MinDays = 1;
        private const int MaxDays = 10;

        //FIXME - remove these when the datamodel is updated
        //public static IEnumerable<ArgosDownloadable> DownloadableArgosPrograms { get; set; }
        //public static IEnumerable<ArgosDownloadable> DownloadableArgosPlatforms { get; set; }
        
        public static void DownloadAll(string user)
        {
            var db = new AnimalMovementDataContext();
            foreach (var program in db.ArgosPrograms.Where(p => p.Investigator == user && p.Active.HasValue && p.Active.Value))
            {
                DownloadArgosProgram (program);
            }
            foreach (var platform in db.ArgosPlatforms.Where(p => p.ArgosProgram.Investigator == user && p.Active && !p.ArgosProgram.Active.HasValue))
            {
                DownloadArgosPlatform (platform);
            }
        }
        public static void DownloadArgosProgram(ArgosProgram program, int? requestedDays = null)
        {
            //FIXME - if requestedDays is null, then query the database.
            //FIXME - get rid of requestedDays parameter, and always get days from db
            int days = Math.Min(MaxDays, requestedDays ?? MaxDays);
            var errors = "";
            int? firstFileId = null;
            var results = ArgosWebSite.GetProgram(program.UserName, program.Password, program.ProgramId, days,
                                                 out errors);
            if (results != null)
            {
                try
                {
                    var processor = new FileProcessor();
                    firstFileId = processor.LoadAws(results.ToBytes());
                    if (firstFileId.HasValue)
                        processor.ProcessId(firstFileId.Value);
                }
                //FIXME - distinguish between download and processing errors for the sake of the download log
                catch (Exception ex)
                {
                    errors = "Error loading/processing AWS download: " + ex.Message;
                }
            }
            try
            {
                //log this activity in the database
                //if results is null, then errors should be non-null
                var log = new ArgosDownload
                {
                    //FIXME - update datamodel to support new ArgosDownload table
                    Program = program.ProgramId,
                    Username = program.UserName,
                    Days = days,
                    FileId = firstFileId,
                    ErrorMessage = errors
                };
                var db = new AnimalMovementDataContext();
                db.ArgosDownloads.InsertOnSubmit(log);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                errors = Environment.NewLine + "Error logging download to database: " + ex.Message +
                    Environment.NewLine + "Errors: '" + errors + "'" + Environment.NewLine +
                        "CollarId = " + collar.CollarId + "FileId = " + firstFileId;
            }
        }
        
        public static void DownloadArgosPlatform(ArgosPlatform platform, int? requestedDays = null)
        {
            //FIXME - if requestedDays is null, then query the database.
            //FIXME - get rid of requestedDays parameter, and always get days from db
            int days = Math.Min(MaxDays, requestedDays ?? MaxDays);
            var program = platform.ArgosProgram;
            var errors = "";
            int? firstFileId = null;
            var results = ArgosWebSite.GetCollar(program.UserName, program.Password, platform.PlatformId, days,
                                                 out errors);
            if (results != null)
            {
                try
                {
                    var processor = new FileProcessor();
                    firstFileId = processor.LoadAws(results.ToBytes());
                    if (firstFileId.HasValue)
                        processor.ProcessId(firstFileId.Value);
                }
                //FIXME - distinguish between download and processing errors for the sake of the download log
                catch (Exception ex)
                {
                    errors = "Error loading/processing AWS download: " + ex.Message;
                }
            }
            try
            {
                //log this activity in the database
                //if results is null, then errors should be non-null
                var log = new ArgosDownload
                {
                    //FIXME - update datamodel to support new ArgosDownload table
                    Platform = platform.PlatformId,
                    Username = program.UserName,
                    Days = days,
                    FileId = firstFileId,
                    ErrorMessage = errors
                };
                var db = new AnimalMovementDataContext();
                db.ArgosDownloads.InsertOnSubmit(log);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                errors = Environment.NewLine + "Error logging download to database: " + ex.Message +
                    Environment.NewLine + "Errors: '" + errors + "'" + Environment.NewLine +
                        "CollarId = " + collar.CollarId + "FileId = " + firstFileId;
            }
        }
    }
}

