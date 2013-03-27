using System;
using DataModel;
using Telonics;

namespace FileLibrary
{
    static class FileLoader
    {
        public static void LoadPath(string path, Project project = null, ProjectInvestigator manager = null)
        {
            throw new NotImplementedException();
        }

        internal static CollarFile LoadProgram(ArgosProgram program, int days,
                                               ArgosWebSite.ArgosWebResult results, string errors)
        {
            //if results is null, then errors should be non-null (database rule, insert will fail if false)
            CollarFile file = null;
            var database = new AnimalMovementDataContext();
            //Linq to SQL wraps the changes in a transaction so file will not be created if log cannot be written
            if (results != null)
            {
                file = new CollarFile
                {
                    Owner = program.Manager,
                    FileName =
                        "program_" + program.ProgramId + "_" + DateTime.Now.ToString("yyyyMMdd") + ".aws",
                    Format = 'F',
                    Status = 'A',
                    Contents = results.ToBytes()
                };
                database.CollarFiles.InsertOnSubmit(file);
            }
            var log = new ArgosDownload
                {
                ProgramId = program.ProgramId,
                CollarFile = file,
                Days = days,
                ErrorMessage = errors
            };
            database.ArgosDownloads.InsertOnSubmit(log);
            database.SubmitChanges();
            return file;
        }

        internal static CollarFile LoadPlatfrom(ArgosPlatform platform, int days,
                                                ArgosWebSite.ArgosWebResult results, string errors)
        {
            //if results is null, then errors should be non-null (database rule, insert will fail if false)
            CollarFile file = null;
            var database = new AnimalMovementDataContext();
            //Linq to SQL wraps the changes in a transaction so file will not be created if log cannot be written
            if (results != null)
            {
                file = new CollarFile
                    {
                        Owner = platform.ArgosProgram.Manager,
                        FileName =
                            "platform_" + platform.PlatformId + "_" + DateTime.Now.ToString("yyyyMMdd") + ".aws",
                        Format = 'F',
                        Status = 'A',
                        Contents = results.ToBytes()
                    };
                database.CollarFiles.InsertOnSubmit(file);
            }
            var log = new ArgosDownload
                {
                    PlatformId = platform.PlatformId,
                    CollarFile = file,
                    Days = days,
                    ErrorMessage = errors
                };
            database.ArgosDownloads.InsertOnSubmit(log);
            database.SubmitChanges();
            return file;
        }

    }
}
