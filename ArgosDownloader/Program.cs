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
            try
            {
                var db = new AnimalMovementDataContext();
                var views = new AnimalMovementViewsDataContext();
                _emails = new Dictionary<string, string>();
                _admin = Settings.GetSystemDefault(EmailKey);
                foreach (var collar in views.DownloadableAndAnalyzableCollars.Where(c => c.Days == null || c.Days >= MinDays))
                {
                    var days = Math.Min(MaxDays, collar.Days ?? MaxDays);
                    string errors;
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
                                db.CollarFiles.InsertOnSubmit(collarFile2);
                                try
                                {
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
                                db.CollarFiles.DeleteOnSubmit(collarFile);
                                try
                                {
                                    db.SubmitChanges();
                                }
                                catch (Exception ex)
                                {
                                    errors += Environment.NewLine + "Error trying to rollback changes: " + ex.Message +
                                              Environment.NewLine + "The AWS file (id = " + firstFileId +
                                              ") added to the database could not be removed after a subsequent error.";
                                }
                            }
                        }
                    }
                    //log this activity in the database
                    //if results is null, then errors should be non-null
                    var log = new ArgosDownload
                        {
                            CollarManufacturer = collar.CollarManufacturer,
                            CollarId = collar.CollarId,
                            FileId = firstFileId,
                            ErrorMessage = errors
                        };
                    db.ArgosDownloads.InsertOnSubmit(log);
                    db.SubmitChanges();
                    WarnAboutCollarDays(collar.Email, collar.UserName, collar.PlatformId, collar.Days, results != null);
                }
                SendEmails();
            }
            catch (Exception ex)
            {
                ReportException(ex);
            }
        }


        private static void ReportException(Exception ex)
        {
            //try to email the sys admin, otherwise log to a file, otherwise write to the console
            try
            {
                SendGmail(_admin,
                          "Unexpected Error Running Animal Movements Argos Downloader", ex.ToString());
            }
            catch (Exception innerEx)
            {
                var message =
                    String.Format(Environment.NewLine + Environment.NewLine +
                                  "{0} Argos Downloader unable to email admin with error" + Environment.NewLine +
                                  "  Exception when sending email:" + Environment.NewLine + "{1}" + Environment.NewLine +
                                  "  Original error trying to email to admin:" + Environment.NewLine + "{2}",
                                  DateTime.Now, innerEx, ex);
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

        private static void WarnAboutCollarDays(string email, string userName, string argosId, int? days, bool gotData)
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
            if (gotData && days > MaxDays)
            {
                var msg =
                    String.Format(
                        "Successfully downloaded collar {0} for {1} for the first time in {2} days.  " +
                        "Only the last {2} days were available on the server.  " +
                        "Be sure to upload missed fixes from some other source.", argosId, userName,
                        MaxDays);
                AddEmail(email, msg);
            }
            if (!gotData && days < MaxDays && MaxDays / 2 < days)
            {
                var msg = String.Format(
                    "Unable to successfully download collar {0} for {1}.  " +
                    "It has been {2} days since the data has been successfully downloaded.  " +
                    "You have {3} days to resolve any issues that may be causing problems.  " +
                    "After that, previous fixes will need to be loaded from some other source.",
                    argosId, userName, days, MaxDays);
                AddEmail(email, msg);
            }
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
                    if (!String.IsNullOrEmpty(tdcExe))
                        g4processor.TdcExecutable = tdcExe;
                    if (!String.IsNullOrEmpty(batchFile))
                        g4processor.BatchFileTemplate = batchFile;

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
