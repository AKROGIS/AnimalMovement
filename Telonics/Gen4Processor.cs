using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;


namespace Telonics
{
    public class Gen4Processor : IProcessor
    {
        #region Public API

        public Gen4Processor(Byte[] tpfFile)
        {
            TpfFile = tpfFile;
            TdcExecutable = Properties.Settings.Default.TdcPathToExecutable;
            TdcTimeout = Properties.Settings.Default.TdcMillisecondTimeout;
        }

        public Byte[] TpfFile { get; private set; }
        public string TdcExecutable { get; set; }
        public string BatchFileTemplate { get; set; }
        public int TdcTimeout { get; set; }
        public Gen4Format Format { get; set; }

        public IEnumerable<string> ProcessTransmissions(IEnumerable<ArgosTransmission> transmissions, ArgosFile file)
        {
            string text = file.Header ?? "";
            text += String.Join(Environment.NewLine, transmissions.Select(t => t.ToString()));
            return ProcessFile(text);
        }

        public void ProcessDataLog(Byte[] contents)
        {
            Format = Gen4Format.Datalog;
            ProcessFile(Encoding.UTF8.GetString(contents));
        }

        #endregion

        #region Private Methods

        private IEnumerable<string> ProcessFile(string fileContents)
        {
            string tpfPath = null;
            string dataFilePath = null;
            string batchFilePath = null;
            string logFilePath = null;
            string outputFolder = null;
            string[] results;

            try
            {
                if (!File.Exists(TdcExecutable))
                    throw new InvalidOperationException("TDC Execution error - TDC not found at " + TdcExecutable);

                //The datalog format has the tpf file embeded
                if (Format != Gen4Format.Datalog)
                {
                    // save the tpf file to the file system
                    tpfPath = Path.GetTempFileName();
                    File.WriteAllBytes(tpfPath, TpfFile);
                }

                // write the argos file transmission to the filesystem
                dataFilePath = Path.GetTempFileName();
                //TDC batch mode uses the aws extension to determine file type 
                if (Format == Gen4Format.WebService || fileContents.StartsWith("\"programNumber\";\"platformId\";"))
                    dataFilePath = dataFilePath + ".aws";
                File.WriteAllText(dataFilePath, fileContents);
                
                batchFilePath = Path.GetTempFileName();
                logFilePath = Path.GetTempFileName();
                outputFolder = GetNewTempDirectory();

                //Create the batch file and save it to the filesystem
                string batchCommands;
                switch (Format)
                {
                    case Gen4Format.Datalog:
                        if (BatchFileTemplate == null)
                            BatchFileTemplate = Properties.Settings.Default.TdcDatalogBatchFileFormat;
                        batchCommands = String.Format(BatchFileTemplate, dataFilePath, outputFolder, logFilePath);
                        break;
                    case Gen4Format.Email:
                    case Gen4Format.WebService:
                        if (BatchFileTemplate == null)
                            BatchFileTemplate = Properties.Settings.Default.TdcArgosBatchFileFormat;
                        batchCommands = String.Format(BatchFileTemplate, dataFilePath, tpfPath, outputFolder,
                                                      logFilePath);
                        break;
                    default:
                        throw new InvalidOperationException("Unsupported Telonics data format");
                }
                File.WriteAllText(batchFilePath, batchCommands);
                
                //  Run TDC with the batch file
                var p = Process.Start(new ProcessStartInfo
                                      {
                    FileName = TdcExecutable,
                    Arguments = "/batch:\"" + batchFilePath +"\"",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true
                });
                string errors = p.StandardError.ReadToEnd();
                bool exitedNormally = p.WaitForExit(TdcTimeout);
                if (!exitedNormally)
                    throw new InvalidOperationException(
                        String.Format("TDC process did not respond after {0} seconds.\n" +
                                      "Check the path in the Settings table, and be sure you have authorized TDC.",
                                      TdcTimeout/1000));
                if (!String.IsNullOrEmpty(errors))
                    throw new InvalidOperationException("TDC Execution error " + errors);
                
                //Check the log file for errors
/*
Here is an example log file with NO error:
                
Batch started at: 2013.03.02 00:35:17
Processing file: C:\...\37470.aws
Report: "C:\...\650937A_1 Condensed.csv" created successfully.
Batch completed at: 2013.03.02 00:35:18

Here is an example log file with an error:

Batch started at: 2012.12.17 22:04:31
Processing file: C:\...\tmpB158.tmp
Unable to load the the parameter file: "C:\..\23.tpf".This file may require a newer version of TDC.
Batch completed at: 2012.12.17 22:32:27
*/
                var logLines = File.ReadAllLines(logFilePath);
                errors = String.Join("\n", logLines.Where(line => !String.IsNullOrWhiteSpace(line) &&
                                                                  !line.StartsWith("Batch started at:") &&
                                                                  !line.StartsWith("Processing file:") &&
                                                                  !line.EndsWith("successfully.") &&
                                                                  !line.EndsWith("Using default settings instead.") &&  //warning when run as Sql server impersonated proxy account
                                                                  !line.StartsWith("Batch completed at:")));
                if (!String.IsNullOrEmpty(errors))
                    throw new InvalidOperationException("TDC Execution error " + errors);

                // for each output file created by TDC, send the file to the database
                string[] paths = Directory.GetFiles(outputFolder);
                if (paths.Length < 1)
                    throw new InvalidOperationException("TDC Execution error - No output file");
                if (paths.Length > 1)
                    throw new InvalidOperationException("TDC Execution error - multiple output files");
                
                results = File.ReadAllLines(paths[0]);

            }
            finally
            {
                //cleanup temp files/folders
                if (tpfPath != null && File.Exists(tpfPath))
                    File.Delete(tpfPath);
                if (dataFilePath != null && File.Exists(dataFilePath))
                    File.Delete(dataFilePath);
                if (batchFilePath != null && File.Exists(batchFilePath))
                    File.Delete(batchFilePath);
                if (logFilePath != null && File.Exists(logFilePath))
                    File.Delete(logFilePath);
                if (outputFolder != null && Directory.Exists(outputFolder))
                {
                    foreach (var path in Directory.GetFiles(outputFolder))
                    {
                        File.SetAttributes(path, FileAttributes.Normal);
                            // remove the readonly flag put on files created by TDC.
                        File.Delete(path);
                    }
                    Directory.Delete(outputFolder);
                }
            }

            return results;
        }

        private static string GetNewTempDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }

        #endregion
    }
}

