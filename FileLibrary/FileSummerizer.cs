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
            var query = from file in database.CollarFiles
                        join item in database.ArgosFilePlatformDates on file.FileId equals item.FileId into fileItems
                        from item in fileItems.DefaultIfEmpty()
                        where item == null && (file.Format == 'B' || file.Format == 'E' || file.Format == 'F')
                        select file;
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


        public static void SummerizeFile(int fileId)
        {
            var database = new AnimalMovementDataContext();
            var file = database.CollarFiles.FirstOrDefault(
                    f => f.FileId == fileId && (f.Format == 'B' || f.Format == 'E' || f.Format == 'F'));
            if (file == null)
                throw new InvalidOperationException("Collar File not found (or the wrong format)");
            SummerizeFile(file);
        }


        public static void SummerizeFile(CollarFile file)
        {
            ArgosFile argos;
            switch (file.Format)
            {
                case 'B':
                    argos = new DebevekFile(file.Contents.ToArray());
                    break;
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
