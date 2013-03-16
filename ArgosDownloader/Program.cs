using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using DataModel;
using Telonics;
using FileLibrary;

namespace ArgosDownloader
{
    public static class Program
    {
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

                foreach (var download in FileLibrary.ArgosDownloader.DownloadableArgosPrograms)
                {
                    DownloadProgram(download);
                }
                foreach (var download in FileLibrary.ArgosDownloader.DownloadableArgosPlatforms)
                {
                    DownloadPlatform(download);
                }
                SendEmails();
            }
            catch (Exception ex)
            {
                ReportException("Unhandled Exception: '" + ex.Message);
            }
        }

        private static void DownloadProgram(ArgosDownloadable download)
        {
            var program = FileLibrary.ArgosDownloader.ArgosPrograms.FirstOrDefault (p => p.ProgramId == download.ProgramId);
            try {
                FileLibrary.ArgosDownloader.DownloadArgosProgram (program, download.Days);
            } catch (Exception ex) {
                var errors = "Download error " + ex.Message + " for program " + download.ProgramId;
                if (download.SendNoEmails.HasValue && download.SendNoEmails.Value)
                    AddErrorToEmail (program.Email, program.UserName, program.ProgramId, errors);
                AddErrorToEmail (_admin, program.UserName, program.ProgramId, errors);
            }
        }
        
        private static void DownloadPlatform(ArgosDownloadable download)
        {
            var platform = FileLibrary.ArgosDownloader.ArgosPlatforms.First(p => p.PlatformId == download.PlatformId);
            try {
                FileLibrary.ArgosDownloader.DownloadArgosPlatform (platform, download.Days);
            } catch (Exception ex) {
                var program = platform.Program;
                var errors = "Download error " + ex.Message + " for platform " + download.PlatformId;
                if (download.SendNoEmails.HasValue && download.SendNoEmails.Value)
                    AddErrorToEmail (program.Email, program.UserName, program.ProgramId, errors);
                AddErrorToEmail (_admin, program.UserName, program.ProgramId, errors);
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

        private static void AddErrorToEmail(string email, string userName, string argosId, string errors)
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
    }
}
