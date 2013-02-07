using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Telonics
{
    public abstract class ArgosFile
    {

        #region Private Fields

        private List<string> _lines;
        private List<ArgosTransmission> _transmissions;
        private Dictionary<String, List<ArgosTransmission>> _transmissionsByCtn;  //CTN is the Telonics Id

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

        #region Public Properties

        /// <summary>
        /// Function that returns the Telonics ID (CTN Number) for a platformId (ArgosId) and Transmission date/time
        /// </summary>
        public Func<String, DateTime, String> CollarFinder { get; set; }
        /// <summary>
        /// Function that returns the object that knows how to process Argos transmissions for the given Telonics ID
        /// </summary>
        public Func<String, IProcessor> Processor { get; set; }

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

        public IEnumerable<string> GetTransmissions()
        {
            if (_transmissions == null)
                _transmissions = GetTransmissions(_lines).ToList();
            return _transmissions.Select(t => t.ToString());
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

        public IEnumerable<string> ToTelonicsData(string telonicsId)
        {
            if (_transmissionsByCtn == null)
            {
                if (CollarFinder == null)
                    throw new InvalidOperationException("The TelonicsId function must be defined to call ToTelonicsData()");
                if (_transmissions == null)
                    _transmissions = GetTransmissions(_lines).ToList();
                _transmissionsByCtn = SortTransmissions(_transmissions);
            }
            if (Processor == null)
                throw new InvalidOperationException("The Processor function must be defined to call ToTelonicsData()");
            var processor = Processor(telonicsId);
            if (processor == null)
                throw new InvalidOperationException("There is no processor defined for Telonics Id " + telonicsId);
            if (!_transmissionsByCtn.ContainsKey(telonicsId) || _transmissionsByCtn[telonicsId].Count < 1)
                throw new NoMessagesException("There are no messages for Telonics Id " + telonicsId);
            return Processor(telonicsId).Process(_transmissionsByCtn[telonicsId]);
        }

        /// <summary>
        /// Processes all the transmissions,  caller must ensure the file has only one platform,
        /// and that the default processor is appropriate for this platform
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> ToTelonicsData()
        {
            if (_transmissions == null)
                _transmissions = GetTransmissions(_lines).ToList(); 
            if (Processor == null)
                throw new InvalidOperationException("The Processor function must be defined to call ToTelonicsData()");
            var processor = Processor("default");
            if (processor == null)
                throw new InvalidOperationException("There is no default processor defined");
            return processor.Process(_transmissions);
        }

        #endregion

        #endregion

        #region Private Methods

        abstract protected IEnumerable<ArgosTransmission> GetTransmissions(IEnumerable<string> lines);

        private Dictionary<string, List<ArgosTransmission>> SortTransmissions(IEnumerable<ArgosTransmission> transmissions)
        {
            var results = new Dictionary<string, List<ArgosTransmission>>();
            foreach (var transmission in transmissions)
            {
                var ctn = CollarFinder(transmission.PlatformId, transmission.DateTime);
                if (ctn == null)
                    continue;
                if (!results.ContainsKey(ctn))
                    results[ctn] = new List<ArgosTransmission>();
                results[ctn].Add(transmission);
            }
            return results;
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

        private IEnumerable<string> ReadLines(Stream stream, Encoding enc)
        {
            using (var reader = new StreamReader(stream, enc))
                while (!reader.EndOfStream)
                    yield return reader.ReadLine();
        }

        #endregion

        #endregion

    }
}
