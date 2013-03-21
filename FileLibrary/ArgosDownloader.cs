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
            foreach (var program in db.ArgosPrograms.Where(p => p.Manager == user && p.Active.HasValue && p.Active.Value))
            {
                DownloadArgosProgram (program);
            }
            foreach (var platform in db.ArgosPlatforms.Where(p => p.ArgosProgram.Manager == user && p.Active && !p.ArgosProgram.Active.HasValue))
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
                //I can't use LINQ to SQL here.  I get .'Cannot add an entity with a key that is already in use'
                //I guess because my PK is a Timestamp set to a getdate() on insert, and the object isn't getting it
                var db = new AnimalMovementDataContext();
                db.ArgosDownloads_Insert(program.ProgramId, null, days, firstFileId, errors);
            }
            catch (Exception ex)
            {
                errors = Environment.NewLine + "Error logging download to database: " + ex.Message +
                    Environment.NewLine + "Errors: '" + errors + "'" + Environment.NewLine +
                        "ProgramId = " + program.ProgramId + "FileId = " + firstFileId;
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
                //I can't use LINQ to SQL here.  I get .'Cannot add an entity with a key that is already in use'
                //I guess because my PK is a Timestamp set to a getdate() on insert, and the object isn't getting it
                var db = new AnimalMovementDataContext();
                db.ArgosDownloads_Insert(null,platform.PlatformId, days, firstFileId, errors);
            }
            catch (Exception ex)
            {
                errors = Environment.NewLine + "Error logging download to database: " + ex.Message +
                    Environment.NewLine + "Errors: '" + errors + "'" + Environment.NewLine +
                        "PlatformId = " + platform.PlatformId + "FileId = " + firstFileId;
            }
        }
    }
}

