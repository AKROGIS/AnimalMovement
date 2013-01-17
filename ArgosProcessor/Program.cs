using System;
using System.Linq;
using System.Text;
using DataModel;
using Telonics;

namespace ArgosProcessor
{
    class Program
    {
        /// <summary>
        /// This program obtains an email file from the database, processes all the data in the file, and then loads the results into the database.
        /// This program will be run by the database when the user loads and email file into the database.
        /// The database looks to the standard output from this program for results.  This program will write
        /// nothing to the console on success.  Any warnings or errors will be written to the console for the database.
        /// The database will report non-empty results (errors and warnings) to the user.
        /// It is assumed that all the data in the email file is ASCII text, and that it the concatination of one or more Argos emails.
        /// It is also assumed that the message is in a Telonics data format we know how to process (currently only gen3 or 4)
        /// It is assumed that all the collars are in the database, and that Argos Platform Id is stored in the Alternative Id of the collar
        /// The database should have any necessary information (i.e. deployment dates) necessary to differentiate collars with the same Argos Id
        /// The database should have any necessary parameter files/data needed to process all the data.
        /// All assumptions will be checked, and any failed assumptions will be reported as errors or warnings.
        ///
        /// NOTE: an email file could actually be a composite email file, containing data over a broad range of time,
        ///  such that it contains transmission from a single Argos id when it was a gen3 collar, and also after 
        /// refurbishment into a gen4 (or greater) collar.  We cannot assume that all of the transmissions for a single
        /// Argos Id will map to the same Telonics CTN number (unique collar id)
        /// </summary>
        /// <param name="args">
        /// This program takes a single command line argument, which is an integer id of a
        /// Argos email file in the database.
        /// </param>
        static void Main(string[] args)
        {
            var error = new StringBuilder();
            try
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
                if (args.Length != 1 || !Int32.TryParse(args[0], out id) || id < 1)
                {
                    error.AppendLine("ERROR: One and only one positive integer argument is expected.");
                    return;
                }
                var file = db.CollarFiles.FirstOrDefault(f => f.FileId == id && f.Format == 'E');
                if (file == null)
                {
                    error.AppendLine("ERROR: id provided is not an Argos email file in the database.");
                    return;
                }
                var argos = new ArgosFile(file.Contents.ToArray());
                var analyzer = new ArgosCollarAnalyzer(argos, db);

                foreach (var platform in analyzer.UnknownPlatforms)
                {
                    string message = String.Format(
                        "WARNING: Platform Id {0} was NOT found in the database. All fixes for this platform will be ignored.",
                        platform);
                    error.AppendLine(message);
                }

                foreach (var platform in analyzer.AmbiguousPlatforms)
                {
                    string message = String.Format(
                        "WARNING: Platform Id {0} is ambiguous in the database. Ambiguous fixes for this platform will be ignored.",
                        platform);
                    error.AppendLine(message);
                }

                foreach (var problem in analyzer.CollarsWithProblems)
                {
                    string message = String.Format(
                        "WARNING: Collar {0} {1} cannot be processed.  Reason: {2}",
                        problem.Key.CollarManufacturer, problem.Key.CollarId, problem.Value);
                    error.AppendLine(message);
                }

                argos.Processor = analyzer.ProcessorSelector;
                argos.CollarFinder = analyzer.CollarSelector;
                foreach (var collar in analyzer.ValidCollars)
                {
                    try
                    {
                        var data = argos.ToTelonicsData(collar.CollarId);
                        var collarFile = new CollarFile
                        {
                            Project = file.Project,
                            FileName = file.FileName + "_" + collar.CollarId,
                            Format = analyzer.GetFileFormatForCollar(collar),
                            CollarManufacturer = "Telonics",
                            CollarId = collar.CollarId,
                            Status = 'A',
                            ParentFileId = id,
                            Contents = Encoding.UTF8.GetBytes(String.Join("",data)),

                        };
                        db.CollarFiles.InsertOnSubmit(collarFile);
                        db.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        string message = String.Format(
                            "ERROR: Collar {0} {1} (Argos Id {2}) encountered a problem: {3}",
                            collar.CollarManufacturer, collar.CollarId, collar.AlternativeId, ex.Message);
                        error.AppendLine(message);
                    }
                }
            }
            catch (Exception ex)
            {
                error.AppendLine("\nERROR: Unhandled exception: " + ex.Message);
            }
            finally
            {
                Console.WriteLine(error);
            }
        }
    }
}
