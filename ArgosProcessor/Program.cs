using System;
using System.Collections.Generic;
using System.Linq;
using DataModel;
using Telonics;

namespace ArgosProcessor
{
    class Program
    {

        static void Main(string[] args)
        {
            var db = new AnimalMovementDataContext();
#if DEBUGx
            // Args[0] is a path to an email file
            string fileName = System.IO.Path.GetFileName(args[0]);
            byte[] content = System.IO.File.ReadAllBytes(args[0]);
            var collarFile = new CollarFile
                {
                    Project = "test",
                    FileName = fileName,
                    Format = 'E',
                    CollarManufacturer = "Telonics",
                    Status = 'A',
                    Contents = content
                };
            db.CollarFiles.InsertOnSubmit(collarFile);
            db.SubmitChanges();
            args = new[] { collarFile.FileId.ToString( System.Globalization.CultureInfo.InvariantCulture) };
#endif
            int id;
            if (args.Length != 1 || !Int32.TryParse(args[0], out id))
            {
                Console.WriteLine("ERROR: One and only one integer argument is expected.");
                return;
            }
            var file = db.CollarFiles.FirstOrDefault(f => f.FileId == id && f.Format == 'E');
            if (file == null)
            {
                Console.WriteLine("ERROR: id provided is not an Argos email file in the database.");
                return;
            }
            var argos = new ArgosFile(file.Contents.ToArray());
            var allids = argos.GetPlatforms().ToArray();
            var gen3ids = allids.Intersect(from collar in db.Collars
                                           where collar.CollarModel == "TelonicsGen3"
                                           select collar.AlternativeId).ToArray();
            var gen4ids = allids.Intersect(from collar in db.Collars
                                           where collar.CollarModel == "TelonicsGen4"
                                           select collar.AlternativeId).ToArray();
            if (gen3ids.Length > 0)
            {
                var gen34ids = gen3ids.Intersect(gen4ids).ToArray();
                argos.IsGen3Platform = gen3ids.Contains;
                //FIXME get platform period from the database
                //FIXME argos.PlatformPeriod = ??
                if (gen34ids.Length > 0)
                {
                    var startDates = new Dictionary<string, DateTime>();
                    var endDates = new Dictionary<string, DateTime>();
                    //FIXME set dates by querying database deployments
                    argos.IsGen3PlatformOnDate = (i, d) => startDates.ContainsKey(i) && startDates[i] < d && d < endDates[i];
                }
                var results = argos.ToGen3TelonicsCsv();
                //FIXME save to database
                //FIXME collect any errors
                string error = results.First();
                Console.WriteLine(error);
                //FIXME can we write one CTN independent file, or do we need to break this apart?
            }
            // need collar id, model, and possibly date range
            // need a function to identify the Gen3 collars
            if (gen4ids.Length > 0)
            {
                // process Gen4 data in email.  This will automatically obtain the CTN number,
                // and deal with any ambiguities in an acceptable manner.
                string error = Utility.ProcessArgosDataForTelonicsGen4(id);
                Console.WriteLine(error);
            }
            //FIXME - Check for ids not processed



        }
    }
}
