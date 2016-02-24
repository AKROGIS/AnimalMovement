using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using DataModel;
using FileLibrary;

namespace IridiumDownloader
{
    internal class Program
    {
        private static void Main()
        {
            try
            {
                GetAndLoadAndProcess();
            }
            catch (Exception ex)
            {
                ReportException("Unexpected Exception in IridiumDownloader: " + ex.Message);
            }
        }

        private static void GetAndLoadAndProcess()
        {
            string downloadFolder = GetDownloadFolder();
            DownloadAllEmailFilesTo(downloadFolder);
            var defaultPi = GetDefaultProjectInvestagator();
            foreach (var path in Directory.GetFiles(downloadFolder))
            {
                try
                {
                    FileLoader.LoadPath(path, manager: defaultPi);
                    // File was successfully Loaded, so delete it.
                    // remove the readonly flag put on files created by TDC
                    File.SetAttributes(path, FileAttributes.Normal);
                    File.Delete(path);
                }
                catch (Exception ex)
                {
                    ReportException(
                        String.Format("Unexpected Exception ({0}) when loading file: {1}", ex.Message, path));
                }
            }
            FileProcessor.ProcessAll(exceptionHandler, defaultPi);
        }

        private static void DownloadAllEmailFilesTo(string downloadFolder)
        {
            var tdcExecutable = Properties.Settings.Default.TdcPathToExecutable;

            if (!File.Exists(tdcExecutable))
                throw new InvalidOperationException("TDC Execution error - TDC not found at " + tdcExecutable);

            var batchFileTemplate = Properties.Settings.Default.TdcBatchFileTemplate;
            var tdcTimeout = Properties.Settings.Default.TdcMillisecondTimeout;

            string batchFilePath = null;
            string logFilePath = null;

            try  //Execute in try block to do cleanup if there is a failure
            {
                batchFilePath = Path.GetTempFileName();
                logFilePath = Path.GetTempFileName();
                var emailUsername = Settings.GetSystemEmail();
                var emailPassword = Settings.GetSystemEmailPassword();

                //Create the batch file and save it to the filesystem
                var batchCommands = String.Format(batchFileTemplate, downloadFolder, logFilePath, emailUsername, emailPassword); 
                File.WriteAllText(batchFilePath, batchCommands);
                
                //  Run TDC with the batch file
                var p = Process.Start(new ProcessStartInfo
                                      {
                                          FileName = tdcExecutable,
                    Arguments = "/emailDownload:\"" + batchFilePath + "\"",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true
                });
                if (p == null)
                    throw new InvalidOperationException("TDC process already running, Please wait and try again.");
                string errors = p.StandardError.ReadToEnd();
                bool exitedNormally = p.WaitForExit(tdcTimeout);
                if (!exitedNormally)
                    throw new InvalidOperationException(
                        String.Format("TDC process did not respond after {0} seconds.\n" +
                                      "Check the path in the Settings table, and be sure you have authorized TDC.",
                                      tdcTimeout / 1000));
                if (!String.IsNullOrEmpty(errors))
                    throw new InvalidOperationException("TDC Execution error " + errors);

                errors = CheckLogFileForErrors(logFilePath, downloadFolder);
                if (!String.IsNullOrEmpty(errors))
                    throw new InvalidOperationException("TDC Execution error " + errors);
            }
            finally
            {
                //cleanup temp files/folders
                if (batchFilePath != null && File.Exists(batchFilePath))
                    File.Delete(batchFilePath);
                if (logFilePath != null && File.Exists(logFilePath))
                    File.Delete(logFilePath);
            }
        }

        private static string CheckLogFileForErrors(string logFilePath, string downloadFolder)
        {
            /*
            *** Here is an example log file with an error:
             *
            Email download started at: 2016.02.18 17:14:03
            Download error.
            Please log in via your web browser: https://support.google.com/mail/accounts/answer/78754 (Failure)
            Your username or password might be invalid.
            Email download completed at: 2016.02.18 17:14:04

            *** Here is an example log file with NO error:

            Email download started at: 2016.02.18 17:31:00
            Successfully downloaded email and created the following file:
            C:\tmp\AM\Iridium\IDF\300234062584150 20160218_173100.idf
            Email download completed at: 2016.02.18 17:31:02

            Email download started at: 2016.02.18 17:32:13
            Download successful, but no new data available.
            Email download completed at: 2016.02.18 17:32:15
            */
            var logLines = File.ReadAllLines(logFilePath);
            return String.Join(Environment.NewLine,
                                 logLines.Where(line => !String.IsNullOrWhiteSpace(line) &&
                                                        !line.StartsWith("Email download started at:") &&
                                                        !line.StartsWith("Successfully downloaded") &&
                                                        !line.StartsWith(downloadFolder) &&
                                                        !line.StartsWith("Download successful") &&
                                                        !line.StartsWith("Email download completed at:")));
        }

        private static string GetDownloadFolder()
        {
            // We use the same directory on each invocation.
            // This will allow us to keep a email that was downloaded (and will not be downloaded again)
            // until it can be successfully Loaded.
            var appDirectory = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]) ??
                               Directory.GetCurrentDirectory();
            var newDirectory = Path.Combine(appDirectory, "IridiumEmailFiles");
            if (!Directory.Exists(newDirectory))
            {
                Directory.CreateDirectory(newDirectory);
            }
            return newDirectory;
        }

        private static ProjectInvestigator GetDefaultProjectInvestagator()
        {
            var piLogin = Properties.Settings.Default.DefaultProjectInvestigator;
            var database = new AnimalMovementDataContext();
            var projectInvestigator = database.ProjectInvestigators.First(p => p.Login == piLogin);
            return projectInvestigator;
        }

        private static void ReportException(string error)
        {
            //try to email the sys admin, otherwise log to a file, otherwise write to the console
            try
            {
                SendEmail(Settings.GetSystemEmail(),
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

        internal static void exceptionHandler(Exception ex, CollarFile file, ArgosPlatform platform)
        {
            if (file == null)
            {
                ReportException(
                    String.Format("Unexpected Exception ({0}) when Loading an unknown collar file",
                    ex.Message));
            }
            else
            {
                ReportException(
                    String.Format("Unexpected Exception ({0}) when Loading collar file (id: {1}, name:{2}", 
                    ex.Message, file.FileId, file.FileName));
            }
        }

        static void SendEmail(string email, string subject, string body)
        {
            var admin = Settings.GetSystemEmail();
            var password = Settings.GetSystemEmailPassword();
            var fromAddress = new MailAddress(admin, "Animal Movements Admin");
            var toAddress = new MailAddress(email, "Animal Movements User");
            var smtp = new SmtpClient
            {
                Host = Properties.Settings.Default.MailServer,
                Port = Properties.Settings.Default.MailServerPort,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, password),
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
