using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using DataModel;
using Telonics;

namespace ArgosDownloader
{
    public static class Program
    {
        private const int MinDays = 1;
        private const int MaxDays = 10;
        private const string EmailKey = "sa_email";
        private const string PasswordKey = "sa_email_password";
        private const string MailServer = "smtp.gmail.com";
        private const string LogFile = "ArgosDownloader.log";

        static Dictionary<string, string> _emails;
        static private string _admin;
        static private string _password;

        private static void Main()
        {
            string errors = "";
            try
            {
                var db = new AnimalMovementDataContext();
                var views = new AnimalMovementViewsDataContext();
                _emails = new Dictionary<string, string>();
                _admin = Settings.GetSystemDefault(EmailKey);
                int _daysSinceLastDownload = views.DaysSinceLastDownload() ?? Int32.MaxValue;
                foreach (var collar in views.DownloadableAndAnalyzableCollars.Where(c => c.Days == null || c.Days >= MinDays))
                {
                    try
                    {
                        var days = Math.Min(MaxDays, collar.Days ?? MaxDays);
                        errors = "";
                        int? firstFileId = null;
                        int? secondFileId = null;
                        var results = ArgosWebSite.GetCollar(collar.UserName, collar.Password, collar.PlatformId, days,
                                                             out errors);
                        if (results != null)
                        {
                            var collarFile = new CollarFile
                                {
                                    Project = collar.ProjectId,
                                    FileName = collar.PlatformId + "_" + DateTime.Now.ToString("yyyyMMdd") + ".aws",
                                    Format = 'F',
                                    CollarManufacturer = collar.CollarManufacturer,
                                    CollarId = collar.CollarId,
                                    Status = 'A',
                                    Contents = results.ToBytes()
                                };
                            db.CollarFiles.InsertOnSubmit(collarFile);
                            try
                            {
                                db.SubmitChanges();
                                firstFileId = collarFile.FileId;
                            }
                            catch (Exception ex)
                            {
                                errors = "Error writing raw AWS download to database: " + ex.Message;
                            }
                            if (firstFileId != null)
                            {
                                try
                                {
                                    var collarFile2 = ProcessAws(collar, firstFileId.Value, results);
                                    try
                                    {
                                        db.CollarFiles.InsertOnSubmit(collarFile2);
                                        db.SubmitChanges();
                                        secondFileId = collarFile2.FileId;
                                    }
                                    catch (Exception ex)
                                    {
                                        errors = "Error writing Gen3/4 output to database: " + ex.Message;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    errors = "Error processing AWS download to Gen3/4: " + ex.Message;
                                }
                                if (secondFileId == null)
                                {
                                    try
                                    {
                                        db.CollarFiles.DeleteOnSubmit(collarFile);
                                        db.SubmitChanges();
                                    }
                                    catch (Exception ex)
                                    {
                                        errors += Environment.NewLine + "Error trying to rollback changes: " +
                                                  ex.Message + Environment.NewLine + "The AWS file (id = " + firstFileId +
                                                  ") added to the database could not be removed after a subsequent error.";
                                    }
                                }
                            }
                        }
                        try
                        {
                            //log this activity in the database
                            //if results is null, then errors should be non-null
                            var log = new ArgosDownload
                            {
                                PlatformId = collar.PlatformId,
                                Days = collar.Days,
                                FileId = firstFileId,
                                ErrorMessage = errors
                            };
                            db.ArgosDownloads.InsertOnSubmit(log);
                            db.SubmitChanges();
                        }
                        catch (Exception ex)
                        {
                            errors = Environment.NewLine + "Error logging download to database: " + ex.Message +
                                     Environment.NewLine + "Errors: '" + errors + "'" + Environment.NewLine +
                                     "CollarId = " + collar.CollarId + "FileId = " + firstFileId;
                        }
                        if (collar.SendNoEmails.HasValue && collar.SendNoEmails.Value)
                            WarnAboutCollarDays(collar.Email, collar.UserName, collar.PlatformId, collar.Days,
                                                results != null, _daysSinceLastDownload);
                    }
                    catch (Exception ex)
                    {
                        errors = "Unexpected Platform Exception: " + ex.Message + Environment.NewLine + "Errors: '" + errors + "'";
                    }
                    finally
                    {
                        if (collar.SendNoEmails.HasValue && collar.SendNoEmails.Value)
                            EmailErrors(collar.Email, collar.UserName, collar.PlatformId, errors);
                        EmailErrors(_admin, collar.UserName, collar.PlatformId, errors);
                    }
                } //End foreach
                SendEmails();
            }
            catch (Exception ex)
            {
                ReportException("Setup or Email Exception: '" + ex.Message + "'  Errors: '" + errors+ "'.");
            }
        }


        private static void ReportException(string error)
        {
            //try to email the sys admin, otherwise log to a file, otherwise write to the console
            try
            {
                SendGmail(_admin,
                          "Unexpected Error Running Animal Movements Argos Downloader. ", error);
            }
            catch (Exception innerEx)
            {
                var message =
                    String.Format(Environment.NewLine + Environment.NewLine +
                                  "{0} Argos Downloader unable to email admin with error" + Environment.NewLine +
                                  "  Exception when sending email:" + Environment.NewLine + "{1}" + Environment.NewLine +
                                  "  Original error trying to email to admin:" + Environment.NewLine + "{2}",
                                  DateTime.Now, innerEx, error);
                try
                {
                    File.AppendAllText(LogFile, message);
                }
                catch (Exception)
                {
                    Console.WriteLine("Could not write to append to " + LogFile);
                    Console.WriteLine(message);
                }
            }
        }

        private static void WarnAboutCollarDays(string email, string userName, string argosId, int? days, bool gotData, int daysSinceLastDownload)
        {
            if (gotData && !days.HasValue)
            {
                var msg =
                    String.Format(
                        "Successfully finished first downloaded of collar {0} for {1}.  " +
                        "Only the last {2} days were available on the server.  " +
                        "Be sure to upload previous fixes from some other source.", argosId, userName,
                        MaxDays);
                AddEmail(email, msg);
            }
            if (gotData && daysSinceLastDownload > MaxDays)
            {
                var msg =
                    String.Format(
                        "Successfully downloaded collar {0} for {1} for the first time in {2} days.  " +
                        "Only the last {3} days were available on the server.  " +
                        "Be sure to upload missed fixes from some other source.", argosId, userName,
                        daysSinceLastDownload, MaxDays);
                AddEmail(email, msg);
            }
        }
        
        private static void EmailErrors(string email, string userName, string argosId, string errors)
        {
            if (string.IsNullOrEmpty(errors))
                return;
            if (errors.StartsWith("No data"))
                return;

            var msg =
                String.Format(
                    "Problem downloading or processing collar {0} for {1}." + Environment.NewLine + 
                    "    Message: {2}", argosId, userName, errors);
            AddEmail(email, msg);
        }
        
        static void AddEmail(string email, string message)
        {
            if (String.IsNullOrEmpty(email))
                email = _admin;
            if (String.IsNullOrEmpty(email))
                throw new InvalidOperationException(
                    "Unable to obtain an email address for the system administrator when trying to send " +
                    "a message to a user without an email.  Original message is: " +
                    message);
            if (!_emails.ContainsKey(email))
                _emails[email] = "";
            _emails[email] += message + Environment.NewLine;
        }

        static void SendEmails()
        {
            foreach (var item in _emails)
            {
                SendGmail(item.Key,"Warning from the Animal Movements Argos Downloader", item.Value);
            }
        }

        static void SendGmail(string email, string subject, string body)
        {
            if (_password == null)
                _password = Settings.GetSystemDefault(PasswordKey);
            var fromAddress = new MailAddress(_admin, "Animal Movements Admin");
            var toAddress = new MailAddress(email, "Animal Movements User");
            var smtp = new SmtpClient
            {
                Host = MailServer,
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, _password),
                Timeout = 20000 //milliseconds
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }

        private static CollarFile ProcessAws(DownloadableAndAnalyzableCollar collar, int parentFileId,
                                             ArgosWebSite.ArgosWebResult results)
        {
            //TODO - Can I simplify this? Maybe use the ArgosCollarAnalyzer from ArgosProcessor?

            CollarFile collarFile;
            switch (collar.CollarModel)
            {
                case "Gen3":
                    if (!collar.Gen3Period.HasValue)
                        throw new InvalidOperationException("Gen3 collar cannot be processed without a period");
                    var g3processor = new Gen3Processor(TimeSpan.FromMinutes(collar.Gen3Period.Value));

                    ArgosFile aws3 = new ArgosAwsFile(results.ToBytes()) {Processor = id => g3processor};

                    byte[] g3data =
                        Encoding.UTF8.GetBytes(String.Join(Environment.NewLine, aws3.ToTelonicsData()));
                    collarFile = new CollarFile
                        {
                            Project = collar.ProjectId,
                            FileName =
                                collar.PlatformId + "_gen3_" + DateTime.Now.ToString("yyyyMMdd") +
                                ".csv",
                            Format = 'D',
                            CollarManufacturer = collar.CollarManufacturer,
                            CollarId = collar.CollarId,
                            Status = 'A',
                            Contents = g3data,
                            ParentFileId = parentFileId
                        };
                    break;
                case "Gen4":
                    var g4processor = new Gen4Processor(collar.TpfFile.ToArray());
                    string tdcExe = Settings.GetSystemDefault("tdc_exe");
                    string batchFile = Settings.GetSystemDefault("tdc_batch_file_format");
                    int timeout;
                    Int32.TryParse(Settings.GetSystemDefault("tdc_timeout"), out timeout);
                    if (!String.IsNullOrEmpty(tdcExe))
                        g4processor.TdcExecutable = tdcExe;
                    if (!String.IsNullOrEmpty(batchFile))
                        g4processor.BatchFileTemplate = batchFile;
                    if (timeout != 0)
                        g4processor.TdcTimeout = timeout;

                    ArgosFile aws4 = new ArgosAwsFile(results.ToBytes()) {Processor = id => g4processor};

                    byte[] g4data =
                        Encoding.UTF8.GetBytes(String.Join(Environment.NewLine, aws4.ToTelonicsData()));
                    collarFile = new CollarFile
                        {
                            Project = collar.ProjectId,
                            FileName =
                                collar.PlatformId + "_gen4_" + DateTime.Now.ToString("yyyyMMdd") +
                                ".csv",
                            Format = 'C',
                            CollarManufacturer = collar.CollarManufacturer,
                            CollarId = collar.CollarId,
                            Status = 'A',
                            Contents = g4data,
                            ParentFileId = parentFileId
                        };
                    break;
                default:
                    var msg = String.Format("Unsupported model ({0} for collar {1}", collar.CollarModel, collar.CollarId);
                    throw new ArgumentOutOfRangeException(msg);
            }
            return collarFile;
        }
    }
}
