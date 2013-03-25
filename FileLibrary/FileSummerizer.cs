using System;
using System.Linq;
using DataModel;
using Telonics;

namespace FileLibrary
{
    public static class FileSummerizer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="handler">An exception handler to allow processing additional items despite an exception.
        /// If the handler is null, processing will stop on first exception.
        /// The handler can throw it's own exception to halt further processing</param>
        public static void SummerizeAll(Action<Exception, CollarFile> handler = null)
        {
            var database = new AnimalMovementDataContext();
            var query = from file in database.CollarFiles select file;
            foreach (var item in query)
            {
                try
                {
                    SummerizeFile(item);
                }
                catch (Exception ex)
                {
                    if (handler == null)
                        throw;
                    handler(ex, item);
                }
            }
        }

        public static void SummerizeFile(CollarFile file)
        {
            ArgosFile argos;
            switch (file.Format)
            {
                case 'E':
                    argos = new ArgosEmailFile(file.Contents.ToArray());
                    break;
                case 'F':
                    argos = new ArgosAwsFile(file.Contents.ToArray());
                    break;
                default:
                    throw new InvalidOperationException("Unrecognized File Format: " + file.Format);
            }
            SummarizeArgosFile(file.FileId, argos);
        }

        private static void SummarizeArgosFile(int fileId, ArgosFile file)
        {
            var database = new AnimalMovementDataContext();
            foreach (var program in file.GetPrograms())
            {
                foreach (var platform in file.GetPlatforms(program))
                {
                    var minDate = file.FirstTransmission(platform);
                    var maxDate = file.LastTransmission(platform);
                    var summary = new ArgosFilePlatformDate
                    {
                        FileId = fileId,
                        ProgramId = program,
                        PlatformId = platform,
                        FirstTransmission = minDate,
                        LastTransmission = maxDate
                    };
                    database.ArgosFilePlatformDates.InsertOnSubmit(summary);
                }
            }
            database.SubmitChanges();
        }
    }
}
