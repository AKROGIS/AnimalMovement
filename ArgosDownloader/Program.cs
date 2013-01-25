using System;
using Telonics;
using DataModel;
using System.Linq;

namespace ArgosDownloader
{
    internal class Program
    {
        //This program is meant to be run periodically by a scheduler
        //It takes no command line arguments
        private static void Main()
        {
            //TODO add try/catch loops
            //TODO email syaadmin on problems; get sysadmin email from db.
            const int maxDays = 10;
            const int minDays = 1;
            var db = new AnimalMovementDataContext();
            var views = new AnimalMovementViewsDataContext();
            foreach (var collar in views.DownloadableCollars.Where(c => c.Days == null || c.Days >= minDays))
            {
                string username = collar.UserName;
                string password = collar.Password;
                string argosId = collar.PlatformId;
                //TODO - message about Days == null or Days > 10
                //string userEmail = collar.Email;
                int days = collar.Days ?? maxDays;
                days = Math.Min(maxDays, days);
                string errors;
                int? fileId = null;
                var results = ArgosWebSite.GetCollar(username, password, argosId, days, out errors);
                if (results != null)
                {
                    var collarFile = new CollarFile
                    {
                        Project = collar.ProjectId,
                        FileName = collar.PlatformId + "_" + DateTime.Now + ".aws",  //TODO simplify date
                        Format = 'F',
                        CollarManufacturer = collar.CollarManufacturer,
                        CollarId = collar.CollarId,
                        Status = 'A',
                        Contents = results.ToBytes()
                    };
                    // FIXME - Add SQL processor for new AWS (F) file type
                    db.CollarFiles.InsertOnSubmit(collarFile);
                    db.SubmitChanges();
                    fileId = collarFile.FileId;
                }
                // write log
                var log = new ArgosDownload
                    {
                        CollarManufacturer = collar.CollarManufacturer,
                        CollarId = collar.CollarId,
                        FileId = fileId,
                        ErrorMessage = errors
                    };
                db.ArgosDownloads.InsertOnSubmit(log);
                db.SubmitChanges();
            }
        }
    }
}
