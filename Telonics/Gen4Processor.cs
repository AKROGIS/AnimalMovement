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

        public IEnumerable<string> Process(IEnumerable<ArgosTransmission> transmissions)
        {
            if (!File.Exists(TdcExecutable))
                throw new InvalidOperationException("TDC Execution error - TDC not found at " + TdcExecutable);

            // save the tpf file to the file system
            string tpfPath = Path.GetTempFileName();
            File.WriteAllBytes(tpfPath, TpfFile);

            // write the argos file transmission to the filesystem
            var dataFilePath = Path.GetTempFileName();
            File.WriteAllLines(dataFilePath, transmissions.Select(t => t.ToString()));

            var outputFolder = GetNewTempDirectory();
            var batchFilePath = Path.GetTempFileName();
            var logFilePath = Path.GetTempFileName();

            //Create the batch file and save it to the filesystem
            string batchCommands = String.Format(BatchFileTemplate, dataFilePath, tpfPath, outputFolder, logFilePath);
            File.WriteAllText(batchFilePath, batchCommands);

            //  Run TDC with the batch file
            var p = System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = TdcExecutable,
                Arguments = "/batch:" + batchFilePath,
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
            var paths = Directory.GetFiles(outputFolder);
            if (paths.Length < 1)
                throw new InvalidOperationException("TDC Execution error - No output file");
            if (paths.Length > 1)
                throw new InvalidOperationException("TDC Execution error - multiple output files");

            var results = File.ReadAllLines(paths[0]);

            //cleanup temp files/folders
            //FIXME - put this in a finally block, so cleanup is always done.
            //in a finally block, need to figure out what has been created and what has not, and need to protect against exceptions
            File.SetAttributes(paths[0], FileAttributes.Normal);  // remove the readonly flag put on files created by TDC.
            File.Delete(paths[0]);
            Directory.Delete(outputFolder);
            File.Delete(logFilePath);
            File.Delete(batchFilePath);
            File.Delete(dataFilePath);
            File.Delete(tpfPath);

            return results;
        }

        #endregion

        #region Private Methods

        private static string GetNewTempDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }

        #endregion
    }
}

