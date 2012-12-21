using System;
using System.Data.SqlTypes;
using System.IO;

namespace ArgosProcessor
{
    class Program
    {

        static void Main(string[] args)
        {
#if DEBUG
            // Args[0] is a path to an email file
            string fileName = Path.GetFileName(args[0]);
            byte[] content = File.ReadAllBytes(args[0]);
            SqlInt32 id = Utility.UploadFileToDatabase(fileName, "test", "Telonics", null, 'E', 'A', content, SqlInt32.Null);
            string error = Utility.ProcessArgosDataForTelonicsGen4(id);
            Console.WriteLine(error);
#else
            int id;
            if (args.Length != 1 || !Int32.TryParse(args[0], out id))
            {
                Console.WriteLine("One and only one integer argument is expected.");
                return;
            }
            string error = Utility.ProcessArgosDataForTelonicsGen4(id);
            Console.WriteLine(error);
#endif
        }
    }
}
