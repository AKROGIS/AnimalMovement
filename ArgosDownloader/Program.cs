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

        static private Dictionary<string, string> _emails;
        static private string _admin;

        /// <summary>
        /// Downloads Argos Programs and Platforms from the Argos Server, then uploads and processes the files.
        /// Any processing errors are written to the database, any other errors are emailed to the system admin,
        /// and optionally the PI of the platform/program.
        /// </summary>
        /// <param name="args">
        /// If there are no args, then process all active programs and platforms for all users. (An active platform
        ///   is one where the platform is active, and the parent program is neither active or inactive, i.e null)
        /// All args are checked to see if they are a program identifier, and if so the program is downloaded.
        /// If an arg is not a program it is checked to see if it is a platform identifier, and if so the platform
        ///   is downloaded.
        ///  as a program, if it is not a validor as a platform if it the integer is not a program.
        /// If there is only one arg and that arg is a project investigator, then download all programs and
        ///   platforms for that PI
        /// If there is only one arg and that arg in the form /d:XX, where XX is an integer, then download
        ///   that many days for all programs and platforms for all users
        /// If there is only two arguments, and one  arg is a project investigator and the other arg is in the
        ///   form /d:XX, where XX is an integer, then download that many days for all programs and platforms for that PI
        /// An integer arg between 0 and 100, that is not a valid program or platform or an arg in the form /d:XX or
        ///   /days:XX, where XX is an integer defines the number of days (subject to Min/Max Days stipulated by the
        ///   Argos Server) to download for the remaining args.
        /// An arg in the form /nodays, clears a previous setting of days, so the remaining arguments will download
        ///   the number of days recommended by the database (this is the default behavior).
        /// Finally an argument is checked to see if it is a project investigator's login (with domain - as stored in
        ///   Login column of the ProjectINvestigators table.  If a valid PI is provided as the only argument, or optionally
        ///   with a days argument (before or after), then all active programs and platforms (as defined above) for that PI
        ///   are downloaded (for the last X days if the optional days argument was provided)
        /// If the only argument provided was the days argument, then all programs and platforms (as defined above) for all
        ///   users for the last X days will be downloaded.
        /// Any argument in the form /program:XXXX where XXX is a valid ProgramId will be processed as a program.
        /// Any argument in the form /platform:XXXX where XXX is a valid PlatformId will be processed as a platform.
        /// All other args are ignored with an error message to the console.
        /// </param>
        private static void Main(string[] args)
        {
            try
            {
                _emails = new Dictionary<string, string>();
                _admin = Settings.GetSystemEmail();
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
                            days = (daysArg == Int32.MinValue ? null : daysArg);
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
            if (arg == "/nodays")
                return Int32.MinValue;
            if (arg.StartsWith("/d:") && Int32.TryParse(arg.Substring(3), out days))
                return days;
            if (arg.StartsWith("/days:") && Int32.TryParse(arg.Substring(6), out days))
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
                SendEmail(_admin,
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
                    File.AppendAllText(Properties.Settings.Default.LogFile, message);
                }
                catch (Exception)
                {
                    Console.WriteLine("Could not write to append to " + Properties.Settings.Default.LogFile);
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
                    SendEmail(address, subject, message);
            }
        }

        static private string _password;

        static void SendEmail(string email, string subject, string body)
        {
            if (_password == null)
                _password = Settings.GetSystemEmailPassword();
            var fromAddress = new MailAddress(_admin, "Animal Movements Admin");
            var toAddress = new MailAddress(email, "Animal Movements User");
            var smtp = new SmtpClient
            {
                Host = Properties.Settings.Default.MailServer,
                Port = Properties.Settings.Default.MailServerPort,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, _password),
                Timeout = Properties.Settings.Default.MailServerMilliSecondTimeout
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
