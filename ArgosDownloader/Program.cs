using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using DataModel;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args">
        /// If there are no args, then process all programs and platforms for all users (subject to Active settings)
        /// If there is only one arg and that arg is a project investigator, then download all programs and
        ///   platforms for that PI
        /// If there is only one arg and that arg in the form /d:XX, where XX is an integer, then download
        ///   that many days for all programs and platforms for all users
        /// If there is only two arguments, and one  arg is a project investigator and the other arg is in the
        ///   form /d:XX, where XX is an integer, then download that many days for all programs and platforms for that PI
        /// An the arg in the form /d:XX, where XX is an integer defines the number of days (subject to Min/Max Days
        ///   stipulated by the Argos Server) to download for the remaining args. If this argument is not provided,
        ///   then the database is consulted to get the number of days since the last successful download.  Once this
        ///   is used, there is no way to reset to use the databases number.
        /// All other integer args are processed as a program, or as a platform if it the integer is not a program.
        /// Any argument in the form /platform:XXXX where XXX is a valid PlatformId will be processed as a platform.
        /// Any argument in the form /program:XXXX where XXX is a valid ProgramId will be processed as a program.
        /// All other args are ignored with an error message to the console.
        /// </param>
        private static void Main(string[] args)
        {
            try
            {
                _emails = new Dictionary<string, string>();
                _admin = Settings.GetSystemDefault(EmailKey);
                if (args.Length == 0)
                    FileDownloader.DownloadAll(exceptionHandler);
                else
                {
                    int? days = null;
                    ProjectInvestigator pi = null;
                    foreach (var arg in args)
                    {
                        var program = GetProgram(arg);
                        if (program != null)
                        {
                            try
                            {
                                FileDownloader.DownloadArgosProgram(program, days);
                            }
                            catch (Exception ex)
                            {
                                exceptionHandler(ex, program, null);
                            }
                            continue;
                        }
                        var platform = GetPlatform(arg);
                        if (platform != null)
                        {
                            try
                            {
                                FileDownloader.DownloadArgosPlatform(platform, days);
                            }
                            catch (Exception ex)
                            {
                                exceptionHandler(ex, null, platform);
                            }
                            continue;
                        }
                        var daysArg = GetDays(arg);
                        if (daysArg != null)
                        {
                            days = daysArg;
                            continue;
                        }
                        var piArg = GetProjectInvestigator(arg);
                        if (piArg != null)
                        {
                            pi = piArg;
                            continue;
                        }
                        Console.WriteLine("Unhandled argument: {0}", arg);
                    }
                    if ((args.Length == 1 && (days != null || pi != null)) ||
                        (args.Length == 2 && days != null && pi != null))
                        FileDownloader.DownloadAll(exceptionHandler, pi, days);
                }
                SendEmails();
            }
            catch (Exception ex)
            {
                ReportException("Unhandled Exception: '" + ex.Message);
            }
        }

        private static ArgosProgram GetProgram(string programId)
        {
            if (programId.StartsWith("/program:"))
                programId = programId.Substring(9);
            var database = new AnimalMovementDataContext();
            return database.ArgosPrograms.FirstOrDefault(p => p.ProgramId == programId);
        }

        private static ArgosPlatform GetPlatform(string platformId)
        {
            if (platformId.StartsWith("/platform:"))
                platformId = platformId.Substring(10);
            var database = new AnimalMovementDataContext();
            return database.ArgosPlatforms.FirstOrDefault(p => p.PlatformId == platformId);
        }

        private static int? GetDays(string arg)
        {
            int days;
            if (arg.StartsWith("/d:") && Int32.TryParse(arg.Substring(3), out days))
                return days;
            if (Int32.TryParse(arg, out days) && 0 < days && days < 100)
                return days;
            return null;
        }

        private static ProjectInvestigator GetProjectInvestigator(string pi)
        {
            var database = new AnimalMovementDataContext();
            return database.ProjectInvestigators.FirstOrDefault(p => p.Login == pi);
        }

        internal static void exceptionHandler(Exception ex, ArgosProgram program, ArgosPlatform platform)
        {
            if (program == null && platform == null)
            {
                AddErrorToEmail(_admin, null, null, "Downloader exception handler called without a program or platform.");
                return;
            }
            string errors = null;
            if (program != null)
            {
                errors = "Download error '" + ex.Message + "' for program " + program.ProgramId;
            }
            //If program and platform are both non-null (unanticipated), then program is ignored.
            if (platform != null)
            {
                errors = "Download error '" + ex.Message + "' for platform " + platform.ProgramId;
                program = platform.ArgosProgram;
            }
            AddErrorToEmail(program.ProjectInvestigator.Email, program.UserName, program.ProgramId, errors);
            AddErrorToEmail(_admin, program.UserName, program.ProgramId, errors);
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

        private static void AddErrorToEmail(string address, string userName, string argosId, string errors)
        {
            if (string.IsNullOrEmpty(errors))
                return;
            if (errors.StartsWith("No data"))
                return;

            var msg =
                String.Format(
                    "Problem downloading or processing collar {0} for {1}." + Environment.NewLine +
                    "    Message: {2}", argosId, userName, errors);
            AddEmail(address, msg);
        }

        static void AddEmail(string address, string message)
        {
            if (String.IsNullOrEmpty(address))
                address = _admin;
            if (String.IsNullOrEmpty(address))
                throw new InvalidOperationException(
                    "Unable to obtain an email address for the system administrator when trying to send " +
                    "a message to a user without an email.  Original message is: " +
                    message);
            if (!_emails.ContainsKey(address))
                _emails[address] = "";
            _emails[address] += message + Environment.NewLine;
        }

        static void SendEmails()
        {
            const string subject = "Warning from the Animal Movements Argos Downloader";
            foreach (var item in _emails)
            {
                var address = item.Key;
                var message = item.Value;
                if (address == _admin || Settings.PiWantsEmails(address))
                    SendGmail(address, subject, message);
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
