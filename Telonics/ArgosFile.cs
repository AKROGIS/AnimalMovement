using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

//This is a duplicate of a file in the SqlServer_Files project (SQL CLR assemblies need to be independent)
//changes to one copy should be manually replicated in the other.
namespace Telonics
{
    public abstract class ArgosFile
    {

        #region Private Fields

        private List<string> _lines;
        private List<ArgosTransmission> _transmissions;

        #endregion

        #region Public API

        #region Constructors

        protected ArgosFile(string path)
        {
            if (String.IsNullOrEmpty(path))
                throw new ArgumentNullException("path", "path must not be null or empty");
            ReadLines(path);
            if (_lines.Count == 0)
                throw new InvalidDataException("File at path has no lines");
        }

        protected ArgosFile(Byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                throw new ArgumentNullException("bytes", "byte array must not be null or empty");
            ReadLines(bytes);
            if (_lines.Count == 0)
                throw new InvalidDataException("Byte array has no lines");
        }

        protected ArgosFile(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream", "stream must not be null");
            ReadLines(stream);
            if (_lines.Count == 0)
                throw new InvalidDataException("stream has no lines");
        }

        #endregion

        #region Public Methods

        public IEnumerable<string> GetPrograms()
        {
            if (_transmissions == null)
                _transmissions = GetTransmissions(_lines).ToList();
            return _transmissions.Select(t => t.ProgramId).Distinct();
        }

        public IEnumerable<string> GetPlatforms()
        {
            if (_transmissions == null)
                _transmissions = GetTransmissions(_lines).ToList();
            return _transmissions.Select(t => t.PlatformId).Distinct();
        }

        public IEnumerable<string> GetPlatforms(string program)
        {
            if (_transmissions == null)
                _transmissions = GetTransmissions(_lines).ToList();
            return _transmissions.Where(t => t.ProgramId == program).Select(t => t.PlatformId).Distinct();
        }

        public IEnumerable<ArgosTransmission> GetTransmissions()
        {
            if (_transmissions == null)
                _transmissions = GetTransmissions(_lines).ToList();
            return _transmissions.ToArray();
        }

        public DateTime FirstTransmission(string platform)
        {
            if (_transmissions == null)
                _transmissions = GetTransmissions(_lines).ToList();
            return (from t in _transmissions
                    where t.PlatformId == platform
                    orderby t.DateTime
                    select t.DateTime).First();
        }

        public DateTime LastTransmission(string platform)
        {
            if (_transmissions == null)
                _transmissions = GetTransmissions(_lines).ToList();
            return (from t in _transmissions
                    where t.PlatformId == platform
                    orderby t.DateTime descending
                    select t.DateTime).First();
        }

        #endregion

        #endregion

        #region Private Methods

        internal abstract string Header { get; }

        protected abstract IEnumerable<ArgosTransmission> GetTransmissions(IEnumerable<string> lines);

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
