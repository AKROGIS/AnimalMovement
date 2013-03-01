using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Server;

// See http://msdn.microsoft.com/en-us/library/ms131103.aspx
// for more information on creating CLR Table-Valued Functions

namespace SqlServerExtensions
{
    public class TfpSummerizer
    {
        [SqlFunction(DataAccess = DataAccessKind.Read,
                     FillRowMethodName = "FillRow",
                     TableDefinition = @"[FileId] [int],
                                         [CTN] [nvarchar](16),
                                         [Platform] [nvarchar](8),
                                         [Frequency] [float],
                                         [TimeStamp] [DateTime2](7)")]
        public static IEnumerable SummarizeTpfFile(SqlInt32 fileId)
        {
            Byte[] bytes = GetFileContents("CollarParameterFiles", fileId, 'A');
            var tpf = new TpfFile(bytes);
            var resultCollection = new ArrayList();
            foreach (var tpfCollar in tpf.GetCollars())
                resultCollection.Add(new Row(fileId, tpfCollar));
            return resultCollection;
        }


        private static Byte[] GetFileContents(string table, SqlInt32 fileId, char format)
        {
            Byte[] bytes = null;

            using (var connection = new SqlConnection("context connection=true"))
            {
                connection.Open();

                string sql = "SELECT [Contents] FROM [dbo].[" + table + "] WHERE [FileId] = @fileId AND [Format] = @format";
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add(new SqlParameter("@fileId", SqlDbType.Int) { Value = fileId });
                    command.Parameters.Add(new SqlParameter("@format", SqlDbType.Char) { Value = format });

                    using (SqlDataReader results = command.ExecuteReader())
                    {
                        while (results.Read())
                        {
                            bytes = results.GetSqlBytes(0).Buffer;
                        }
                    }
                }
            }
            return bytes;
        }


        private struct Row
        {
            internal readonly SqlInt32 FileId;
            internal readonly TpfCollar Collar;
            public Row(SqlInt32 fileId, TpfCollar collar)
            {
                FileId = fileId;
                Collar = collar;
            }
        }

        public static void FillRow(object inputObject,
                                   out SqlInt32 fileId,
                                   out SqlString ctn,
                                   out SqlString platform,
                                   out SqlDouble frequency,
                                   out DateTime timeStamp)
        {
            var row = (Row) inputObject;
            fileId = row.FileId;
            ctn = row.Collar.Ctn;
            platform = row.Collar.ArgosId;
            frequency = row.Collar.Frequency;
            timeStamp = row.Collar.TimeStamp;
        }


        //The following two private classes are taken verbatim from the Telonics Library in this solution
        //I cannot references them from a SQL Server Project
        public class TpfCollar
        {
            public string Ctn { get; set; }
            public string ArgosId { get; set; }
            public double Frequency { get; set; }
            public string Owner { get; set; }
            public DateTime TimeStamp { get; set; }
            public TpfFile TpfFile { get; set; }

            public override string ToString()
            {
                return String.Format("File: {5}, CTN: {0}, Argos ID: {1}, Frequency: {2}, Owner: {3}, Time Stamp:{4}",
                                     Ctn, ArgosId, Frequency, Owner, TimeStamp, TpfFile);
            }
        }

        public class TpfFile
        {
            #region Private Fields

            private List<string> _lines;

            #endregion

            #region Public API

            #region Constructors

            public TpfFile(string path)
            {
                if (String.IsNullOrEmpty(path))
                    throw new ArgumentNullException("path", "path must not be null or empty");
                ReadLines(path);
                if (_lines.Count == 0)
                    throw new InvalidDataException("File at path has no lines");
                FilePath = path;
            }

            public TpfFile(Byte[] bytes)
            {
                if (bytes == null || bytes.Length == 0)
                    throw new ArgumentNullException("bytes", "byte array must not be null or empty");
                ReadLines(bytes);
                if (_lines.Count == 0)
                    throw new InvalidDataException("Byte array has no lines");
            }

            public TpfFile(Stream stream)
            {
                if (stream == null)
                    throw new ArgumentNullException("stream", "stream must not be null");
                ReadLines(stream);
                if (_lines.Count == 0)
                    throw new InvalidDataException("stream has no lines");
            }

            #endregion

            #region Public Properties

            public string FilePath { get; private set; }

            public string Name
            {
                get { return FilePath == null ? null : Path.GetFileNameWithoutExtension(FilePath); }
            }

            #endregion

            #region Public Methods

            public IEnumerable<TpfCollar> GetCollars()
            {
                if (_lines == null)
                    throw new InvalidOperationException("You must Load() the Tpf file before extracting properties");

                string owner = GetOwner();
                string[] ids = GetIds();
                string[] frequencies = GetFrequencies();
                string[] timeStamps = GetTimeStamps();
                string[] argosIds = GetArgosIds();
                if (ids.Length != frequencies.Length ||
                    ids.Length != argosIds.Length ||
                    frequencies.Length != argosIds.Length)
                    throw new InvalidOperationException("Indeterminant number of collars in file " + Name);
                return ids.Select((id, index) => new TpfCollar
                {
                    Ctn = id,
                    ArgosId = argosIds[index],
                    Frequency = GetFrequency(frequencies[index]),
                    TimeStamp = GetTimeStamp(timeStamps[3 * index] + " " + timeStamps[3 * index + 1]),
                    Owner = owner,
                    TpfFile = this
                });
            }

            public override string ToString()
            {
                return Name ?? "TPF File";
            }

            #endregion

            #endregion

            #region Private Methods

            public string GetOwner()
            {
                return GetTpfData("sections.customer.parameters.customer");
            }

            private string[] GetFrequencies()
            {
                return GetTpfData("sections.units.parameters.vhfFrequencyList").Split();
            }

            private string[] GetArgosIds()
            {
                return GetTpfData("sections.units.parameters.decimalArgosIdList").Split();
            }

            private string[] GetIds()
            {
                return GetTpfData("sections.units.parameters.ctnList").Split();
            }

            private string[] GetTimeStamps()
            {
                return GetTpfData("sections.units.parameters.parameterTimestampList").Split();
            }

            private string GetTpfData(string key)
            {
                var value = from line in _lines
                            where line.StartsWith(key + " ")
                            select line.Replace(key + " ", "");
                var data = value.FirstOrDefault();
                if (String.IsNullOrEmpty(data))
                    throw new FormatException("No value found for Key: " + key + " not found in file " + Name);
                if (data[0] == '{')
                    //should read until next '}', but we stop at the newline, and assume the '}' is at the end o fthe line
                    return data.Replace("{", "").Replace("}", "").Trim();
                return data.Trim();
            }

            private Double GetFrequency(string s)
            {
                int f;
                if (Int32.TryParse(s, out f))
                    return f / 1000000.0;
                throw new FormatException("Frequency (" + s + ") in " + Name + " is not in the expected format");
            }

            private DateTime GetTimeStamp(string s)
            {
                DateTime t;
                if (DateTime.TryParse(s, out t))
                    return new DateTime(t.Ticks, DateTimeKind.Utc);
                throw new FormatException("TimeStamp (" + s + ") in " + Name + " is not in the expected format");
            }

            #region Line Readers

            private void ReadLines(string path)
            {
                _lines = File.ReadAllLines(path).ToList();
            }

            private void ReadLines(Byte[] bytes)
            {
                using (var stream = new MemoryStream(bytes, 0, bytes.Length))
                    _lines = ReadLines(stream, Encoding.UTF8).ToList();
            }

            private void ReadLines(Stream stream)
            {
                _lines = ReadLines(stream, Encoding.UTF8).ToList();
            }

            private static IEnumerable<string> ReadLines(Stream stream, Encoding enc)
            {
                using (var reader = new StreamReader(stream, enc))
                    while (!reader.EndOfStream)
                        yield return reader.ReadLine();
            }

            #endregion

            #endregion

        }

    }
}
