using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;


namespace Telonics
{
    public class Gen4Processor : IProcessor
    {

        #region Private Constants (Default values for public properties)

        //tdcExecutable is the full path to the TDC executable file
        //batchTemplate is a format string for XML content with the following parameters:
        // {0} is the full path of the argos input file (only one is allowed)
        // {1} is the full path of the TPF file
        // {2} is the full path of a folder for the output files
        // {3} is the full path of a file of the log file - can be the empty string if no log is needed
        private const string DefaultTdcExecutable = @"C:\Program Files (x86)\Telonics\Data Converter\TDC.exe";
        private const string DefaultBatchFileTemplate = @"
<BatchSettings>
  <ArgosFile>{0}</ArgosFile>
  <ParameterFile>{1}</ParameterFile>
  <OutputFolder>{2}</OutputFolder>
  <BatchLog>{3}</BatchLog>
  <MoveFiles>false</MoveFiles>
  <GoogleEarth>false</GoogleEarth>
</BatchSettings>";

        #endregion

        #region Public API

        public Gen4Processor(Byte[] tpfFile)
        {
            TpfFile = tpfFile;
            TdcExecutable = DefaultTdcExecutable;
            BatchFileTemplate = DefaultBatchFileTemplate;
        }

        public Byte[] TpfFile { get; private set; }
        public string TdcExecutable { get; set; }
        public string BatchFileTemplate { get; set; }

        public IEnumerable<string> ProcessTransmissions(IEnumerable<ArgosTransmission> transmissions, ArgosFile file)
        {
            string text = file.Header ?? "";
            text += String.Join(Environment.NewLine, transmissions.Select(t => t.ToString()));
            return ProcessFile(text);
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
            string[] paths = null;
            string[] results;

            try
            {
                if (!File.Exists(TdcExecutable))
                    throw new InvalidOperationException("TDC Execution error - TDC not found at " + TdcExecutable);
                
                // save the tpf file to the file system
                tpfPath = Path.GetTempFileName();
                File.WriteAllBytes(tpfPath, TpfFile);
                
                // write the argos file transmission to the filesystem
                dataFilePath = Path.GetTempFileName();
                //TDC batch mode uses the aws extension to determine file type 
                if (fileContents.StartsWith("\"programNumber\";\"platformId\";"))
                    dataFilePath = dataFilePath + ".aws";
                File.WriteAllText(dataFilePath, fileContents);
                
                batchFilePath = Path.GetTempFileName();
                logFilePath = Path.GetTempFileName();
                outputFolder = GetNewTempDirectory();

                //Create the batch file and save it to the filesystem
                string batchCommands = String.Format(BatchFileTemplate, dataFilePath, tpfPath, outputFolder, logFilePath);
                File.WriteAllText(batchFilePath, batchCommands);
                
                //  Run TDC with the batch file
                //FIXME - put a timeout on this command
                var p = Process.Start(new ProcessStartInfo
                                      {
                    FileName = TdcExecutable,
                    Arguments = "/batch:\"" + batchFilePath +"\"",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true
                });
                string errors = p.StandardError.ReadToEnd();
                p.WaitForExit();
                
                if (!String.IsNullOrEmpty(errors))
                    throw new InvalidOperationException("TDC Execution error " + errors);
                
                //FIXME - check and report errors in the log file
                //Here is an example log file with an error:
                /*
                Batch started at: 2012.12.17 22:04:31
                Processing file: C:\Users\resarwas\AppData\Local\Temp\tmpB158.tmp
                Unable to load the the parameter file: "C:\Users\resarwas\Documents\Visual Studio 2010\Projects\AnimalMovement\ArgosProcessor\bin\Debug\tpf\23.tpf".This file may require a newer version of TDC.
                Batch completed at: 2012.12.17 22:32:27
                */
                
                // for each output file created by TDC, send the file to the database
                paths = Directory.GetFiles(outputFolder);
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
                if (paths != null && paths[0] != null && File.Exists(paths[0]))
                {
                    File.SetAttributes(paths[0], FileAttributes.Normal);  // remove the readonly flag put on files created by TDC.
                    File.Delete(paths[0]);
                }
                if (outputFolder != null && Directory.Exists(outputFolder))
                    Directory.Delete(outputFolder);
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

