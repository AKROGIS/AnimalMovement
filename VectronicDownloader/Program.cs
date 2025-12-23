using DataModel;
using FileLibrary;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;


namespace VectronicDownloader
{
    internal class Program
    {
        private static void Main()
        {
            try
            {
                FetchLoadAndProcess();
            }
            catch (Exception ex)
            {
                ReportException("Unexpected Exception in VectronicDownloader: " + ex.Message);
            }
        }

        private static void FetchLoadAndProcess()
        {
            string downloadFolder = GetDownloadFolder();
            var db = new AnimalMovementViews();
            var date = DateTime.Now.ToString("o").Substring(0, 10);
            foreach (var sensor in db.VectronicSensorsToRetrieves)
            {
                // Fetch

                string path = null;
                try
                {
                    path = DownloadFile(sensor, downloadFolder);
                }
                catch (Exception ex)
                {
                    ReportException(
                        String.Format("Unexpected Exception ({0}) when downloading file: {1}", ex.Message, path ?? "Path Unknown"));
                    continue;
                }
                if (path == null)
                {
                    continue;
                }

                // Load

                var pi = GetProjectInvestagator(sensor.Manager);
                var collar = GetCollar(sensor.CollarId);
                try
                {
                    FileLoader.LoadPath(path, manager: pi, collar: collar);
                    // File was successfully Loaded, so delete it.
                    File.Delete(path);
                }
                catch (Exception ex)
                {
                    ReportException(
                        String.Format("Unexpected Exception ({0}) when loading file: {1}", ex.Message, path));
                    continue;
                }
            }

            // Process

            try
            {
                FileProcessor.ProcessAll(ExceptionHandler);
            }
            catch (Exception ex)
            {
                ReportException(
                    String.Format("Unexpected Exception ({0}) when processing Vectronic Files", ex.Message));
            }

        }

        private static string DownloadFile(VectronicSensorsToRetrieve sensor, string downloadFolder)
        {
            string data;
            try
            {
                data = GetSensorData(sensor.CollarId, sensor.SensorCode, sensor.CollarKey, sensor.LastId);
            }
            catch
            {
                // Do nothing; we will try again later
                return null;
            }
            // The data is JSON (without spaces), put each object on a separate line for easier preview
            if (data.StartsWith("[]"))
            {
                // No data available; check the next sensor
                return null;
            }
            data = data.Replace("},{", "},\n{");
            var date = DateTime.Now.ToString("o").Substring(0, 10);
            var filename = $"Vectronic{sensor.CollarId}{sensor.SensorCode}_{date}.json";
            var path = Path.Combine(downloadFolder, filename);
            File.WriteAllText(path, data);
            return path;
        }

        private static HttpClient client = null;

        private static string GetSensorData(string collarId, string sensorCode, string collarKey, int? lastId)
        {
            if (client == null)
            {
                HttpClientHandler handler = new HttpClientHandler()
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                };
                client = new HttpClient(handler)
                {
                    BaseAddress = new Uri(Properties.Settings.Default.ApiBaseUrl)
                };
            }
            var uri = $"{collarId}/{sensorCode}?collarkey={collarKey}";
            if (lastId != null)
            {
                uri += $"&gt-id={lastId}";
            }
            HttpResponseMessage response = client.GetAsync(uri).Result;
            response.EnsureSuccessStatusCode();
            string result = response.Content.ReadAsStringAsync().Result;
            return result;
        }

        private static string GetDownloadFolder()
        {
            // We use the same directory on each invocation.
            // This will allow us to keep a file until it can be successfully Loaded.
            var appDirectory = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]) ??
                               Directory.GetCurrentDirectory();
            var newDirectory = Path.Combine(appDirectory, "VectronicDownloads");
            if (!Directory.Exists(newDirectory))
            {
                Directory.CreateDirectory(newDirectory);
            }
            return newDirectory;
        }

        private static ProjectInvestigator GetProjectInvestagator(string login)
        {
            var database = new AnimalMovementDataContext();
            var projectInvestigator = database.ProjectInvestigators.First(p => p.Login == login);
            return projectInvestigator;
        }

        private static Collar GetCollar(string vectronicId)
        {
            var database = new AnimalMovementDataContext();
            var collar = database.Collars.First(c => c.CollarManufacturer == "Vectronic" && c.CollarId == vectronicId);
            return collar;
        }

        private static void ReportException(string error)
        {
            //try to email the sys admin, otherwise log to a file, otherwise write to the console
            try
            {
                SendEmail(Properties.Settings.Default.MailTo,
                          "Unexpected Error Running Animal Movements Vectronic Downloader. ", error);
            }
            catch (Exception innerEx)
            {
                var message =
                    String.Format(Environment.NewLine + Environment.NewLine +
                                  "{0} Vectronic Downloader unable to email admin with error" + Environment.NewLine +
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

        internal static void ExceptionHandler(Exception ex, CollarFile file, ArgosPlatform platform)
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
            var fromAddress = new MailAddress(email, "Animal Movements Admin");
            var toAddress = new MailAddress(email, "Animal Movements Admin");
            var smtp = new SmtpClient(Properties.Settings.Default.MailHost);
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
