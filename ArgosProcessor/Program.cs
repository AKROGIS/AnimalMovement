using System;
using System.Linq;
using System.Text;
using DataModel;
using Telonics;

namespace ArgosProcessor
{
    static class Program
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
        /// <remarks>
        /// The database can only process one file at a time, and it only makes sense for this tool to process a single file
        /// To process multiple files, the database will call this program multiple times.
        /// </remarks>
        static void Main(string[] args)
        {
            var error = new StringBuilder();
            try
            {
                var db = new AnimalMovementDataContext();
#if DODIR
                // args[0] is a path to a folder of email files
                var folder = String.Copy(args[0]);
                foreach (var email in System.IO.Directory.EnumerateFiles(folder))
                {
                    args[0] = System.IO.Path.Combine(folder, email);
#endif
#if DOFILE || DODIR
            // args[0] is a path to an email file
            string fileName = System.IO.Path.GetFileName(args[0]);
            byte[] content = System.IO.File.ReadAllBytes(args[0]);
            var testFile = new CollarFile
                {
                    Project = "ARCNVSID022",
                    FileName = fileName,
                    Format = 'E',
                    CollarManufacturer = "Telonics",
                    Status = 'A',
                    Contents = content
                };
            db.CollarFiles.InsertOnSubmit(testFile);
            db.SubmitChanges();
            args = new[] { testFile.FileId.ToString( System.Globalization.CultureInfo.InvariantCulture) };
            error.AppendLine(String.Format("Loaded file {0}, {1}", testFile.FileId, fileName));
#endif
                int id;
                if (args.Length != 1 || !Int32.TryParse(args[0], out id) || id < 1)
                {
                    error.AppendLine("ERROR: One and only one positive integer argument is expected.");
                    return;
                }
                var file = db.CollarFiles.FirstOrDefault(f => f.FileId == id && (f.Format == 'E' || f.Format == 'F'));
                if (file == null)
                {
                    var msg = String.Format("ERROR: id {0} is not an Argos email or AWS file in the database.", id);
                    error.AppendLine(msg);
                    return;
                }
                ArgosFile argos;
                switch (file.Format)
                {
                    case 'E':
                        argos = new ArgosEmailFile(file.Contents.ToArray());
                        break;
                    case 'F':
                        argos = new ArgosAwsFile(file.Contents.ToArray());
                        break;
                    default:
                        throw new InvalidOperationException("Unrecognized File Format: " + file.Format);
                }
                var analyzer = new ArgosCollarAnalyzer(argos, db);

                foreach (var platform in analyzer.UnknownPlatforms)
                {
                    string message = String.Format(
                        "WARNING: Platform {0} will be skipped.  It was NOT found in the database.",
                        platform);
                    error.AppendLine(message);
                }

                foreach (var platform in analyzer.AmbiguousPlatforms)
                {
                    var argosId = platform;  // Use local (not foreach) variable in closure (for compiler version stability)
                    var collars = from collar in db.Collars
                                  where collar.ArgosId == argosId
                                  select
                                      String.Format("Collar: {0} DisposalDate: {1}", collar,
                                                    collar.DisposalDate.HasValue
                                                        ? collar.DisposalDate.ToString()
                                                        : "<NULL> (Active)");
                    string message = String.Format(
                        "WARNING: Platform {0} will be skipped because it is ambiguous.\n" +
                        "  Fix this problem by using distinct disposal dates for these collars:\n    {1}",
                        platform, String.Join("\n    ", collars));
                    error.AppendLine(message);
                }

                foreach (var problem in analyzer.CollarsWithProblems)
                {
                    string message = String.Format(
                        "WARNING: Collar {0} cannot be processed.\n" +
                        "   Reason: {1}",
                        problem.Key, problem.Value);
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
                                FileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName) + "_" + collar.CollarId + ".csv",
                                Format = analyzer.GetFileFormatForCollar(collar),
                                CollarManufacturer = "Telonics",
                                CollarId = collar.CollarId,
                                Status = 'A',
                                ParentFileId = id,
                                Contents = Encoding.UTF8.GetBytes(String.Join("\n", data)),
                            };
                        db.CollarFiles.InsertOnSubmit(collarFile);
                        db.SubmitChanges();
#if VERBOSE
                        error.AppendLine(String.Format("Success: Added collar {0}", collar));
#endif
                    }
                    catch (Exception ex)
                    {
                        string message;
                        if (ex is NoMessagesException && analyzer.UnambiguousSharedCollars.Contains(collar))
                            message = String.Format(
                                "Notice: Collar {0} (for shared argos Id {1}) had no messages.\n" +
                                "  This is common for all but one of the collars that share an Argos Id.",
                                collar, collar.ArgosId);
                        else
                            message = String.Format(
                                "ERROR: Collar {0} (Argos Id {1}) encountered a problem: {2}",
                                collar, collar.ArgosId, ex.Message);
                        error.AppendLine(message);
                    }
                }
#if DODIR
                }
#endif
            }
            catch (Exception ex)
            {
                error.AppendLine("\nERROR: Unhandled exception: " + ex.Message);
            }
            finally
            {
                Console.WriteLine(error);
#if DOFILE || DODIR
                System.IO.File.AppendAllText("ArgosDownloader.log", error.ToString());
#endif
            }
        }
    }
}
