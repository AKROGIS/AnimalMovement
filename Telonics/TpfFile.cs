using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Telonics
{
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
            get { return Path.GetFileNameWithoutExtension(FilePath); }
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
            string[] iridiumIds = GetIridiumIds();
            string[] platformIds = argosIds.Length == 0 ? iridiumIds : argosIds;
            string platformType = argosIds.Length == 0 ? "Iridium" : "Argos";
            if (ids.Length != frequencies.Length ||
                ids.Length != platformIds.Length ||
                frequencies.Length != platformIds.Length)
                throw new InvalidOperationException("Indeterminant number of collars in file " + Name);
            return ids.Select((id, index) => new TpfCollar
            {
                Ctn = id,
                Platform = platformType,
                PlatformId = platformIds[index],
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
            var data = GetTpfData("sections.units.parameters.decimalArgosIdList");
            return data == null ? new string[0] : data.Split();
        }

        private string[] GetIridiumIds()
        {
            var data = GetTpfData("sections.units.parameters.iridiumImeiList");
            return data == null ? new string[0] : data.Split();
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
                //throw new FormatException("No value found for Key: " + key + " not found in file " + Name);
                return null;
            if (data[0] == '{')
                //should read until next '}', but we stop at the newline, and assume the '}' is at the end o fthe line
                return data.Replace("{", "").Replace("}", "").Trim();
            return data.Trim();
        }

        private Double GetFrequency(string s)
        {
            if (Int32.TryParse(s, out int f))
                return f / 1000000.0;
            throw new FormatException("Frequency (" + s + ") in " + Name + " is not in the expected format");
        }

        private DateTime GetTimeStamp(string s)
        {
            if (DateTime.TryParse(s, out DateTime t))
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
