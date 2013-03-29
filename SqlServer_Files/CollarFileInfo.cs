using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Server;

namespace SqlServer_Files
{
    public class CollarFileInfo
    {
        [SqlFunction(IsDeterministic = true, IsPrecise = true, DataAccess = DataAccessKind.Read)]
        public static char FileFormat(SqlBytes data)
        {
            //get the first line of the file
            var fileHeader = ReadHeader(data.Buffer, Encoding.UTF8, 500).Trim().Normalize();  //database for header is only 450 char
            char code = '?';
            using (var connection = new SqlConnection("context connection=true"))
            {
                connection.Open();
                const string sql = "SELECT [Header], [FileFormat], [Regex] FROM [dbo].[LookupCollarFileHeaders]";
                using (var command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader results = command.ExecuteReader())
                    {
                        while (results.Read())
                        {
                            var header = results.GetString(0).Normalize();
                            var format = results.GetString(1)[0]; //.GetChar() is not implemented
                            var regex = results.IsDBNull(2) ? null : results.GetString(2);
                            if (fileHeader.StartsWith(header, StringComparison.OrdinalIgnoreCase) ||
                                (regex != null && new Regex(regex).IsMatch(fileHeader)))
                            {
                                code = format;
                                break;
                            }
                        }
                    }
                }
            }
            if (code == '?' && (new ArgosEmailFile(data.Buffer)).GetPrograms().Any())
                // We already checked for ArgosAwsFile with the header
                code = 'E';
            return code;
        }

        private static string ReadHeader(Byte[] bytes, Encoding enc, int maxLength)
        {
            var length = Math.Min(bytes.Length, maxLength);
            using (var stream = new MemoryStream(bytes, 0, length))
            using (var reader = new StreamReader(stream, enc))
                return reader.ReadLine();
        }

        [SqlProcedure]
        public static void Summerize(SqlInt32 fileId)
        {
            Byte[] bytes = null;
            char format = '?';
            using (var connection = new SqlConnection("context connection=true"))
            {
                connection.Open();
                const string sql = "SELECT [Contents], [Format] FROM [dbo].[CollarFiles] WHERE [FileId] = @fileId";
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add(new SqlParameter("@fileId", SqlDbType.Int) { Value = fileId });
                    using (SqlDataReader results = command.ExecuteReader())
                    {
                        while (results.Read())
                        {
                            bytes = results.GetSqlBytes(0).Buffer;
                            format = results.GetString(1)[0]; //.GetChar() is not implemented
                        }
                    }
                }
            }
            if (bytes == null)
                throw new InvalidOperationException("File not found: " + fileId);
            SummerizeFile(fileId, bytes, format);
        }

        private static void SummerizeFile(SqlInt32 fileId, Byte[] contents, char format)
        {
            ArgosFile argos;
            switch (format)
            {
                case 'E':
                    argos = new ArgosEmailFile(contents);
                    break;
                case 'F':
                    argos = new ArgosAwsFile(contents);
                    break;
                case 'G':
                    argos = new DebevekFile(contents);
                    break;
                default:
                    throw new InvalidOperationException("Unsupported File Format: " + format);
            }
            SummarizeArgosFile(fileId, argos);
        }

        private static void SummarizeArgosFile(SqlInt32 fileId, ArgosFile file)
        {
            using (var connection = new SqlConnection("context connection=true"))
            {
                connection.Open();

                foreach (var program in file.GetPrograms())
                {
                    foreach (var platform in file.GetPlatforms(program))
                    {
                        var minDate = file.FirstTransmission(platform);
                        var maxDate = file.LastTransmission(platform);
                        const string sql = "INSERT INTO [dbo].[ArgosFilePlatformDates] (FileId, PlatformId, ProgramId, FirstTransmission, LastTransmission)" +
                                           " VALUES (@FileId, @PlatformId, @ProgramId, @FirstTransmission, @LastTransmission)";
                        using (var command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.Add(new SqlParameter("@fileId", SqlDbType.Int) {Value = fileId});
                            command.Parameters.Add(new SqlParameter("@PlatformId", SqlDbType.NVarChar) {Value = platform});
                            command.Parameters.Add(new SqlParameter("@ProgramId", SqlDbType.NVarChar) {Value = program});
                            command.Parameters.Add(new SqlParameter("@FirstTransmission", SqlDbType.DateTime2) {Value = minDate});
                            command.Parameters.Add(new SqlParameter("@LastTransmission", SqlDbType.DateTime2) {Value = maxDate});
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }
}
