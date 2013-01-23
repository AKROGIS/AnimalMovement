using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Telonics
{
    public class ArgosFile
    {

        #region Private Fields

        private List<string> _lines;
        private List<ArgosTransmission> _transmissions;
        private Dictionary<String, List<ArgosTransmission>> _transmissionsByCtn;  //CTN is the Telonics Id

        #endregion

        #region Public API

        #region Constructors

        public ArgosFile(string path)
        {
            if (String.IsNullOrEmpty(path))
                throw new ArgumentNullException("path", "path must not be null or empty");
            ReadLines(path);
            if (_lines.Count == 0)
                throw new InvalidDataException("File at path has no lines");
        }

        public ArgosFile(Byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                throw new ArgumentNullException("bytes", "byte array must not be null or empty");
            ReadLines(bytes);
            if (_lines.Count == 0)
                throw new InvalidDataException("Byte array has no lines");
        }

        public ArgosFile(Stream stream)
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
        #endregion

        #endregion

        #region Private Methods

        private IEnumerable<ArgosTransmission> GetTransmissions(IEnumerable<string> lines)
        {
            /* Argos files may be contained in numerous concatenated ASCII email files
             * Unidentified lines are ignored.  Transmissions contain a variable number
             * of lines.  The start and end lines will fit a known pattern.
             *
             * 09691 095838   9 31 M 3 2012-12-02 20:00:07  65.726  198.504  0.284 401629897
             *	     2012-12-02 20:03:40  4         12           19          127          112
             *	                                    76           55           86          
             *	     2012-12-02 21:34:42  1         12           19          127          112
             *	                                    76           55           86          110
             *	                                    26          111          162
             *
             * Each group of transmissions starts with the program and platfrom id numbers
             * in this case the program is 09691, and the platfrom is 095838 the rest of the line
             * is ignored it is Argos location data and may be empty.
             * Each transmission starts with the date and time, then a small int, then 3 or 4
             * bytes as a positive integer from 0 to 255.  Typical lines will have 4 bytes. The
             * last line will have only 3 bytes (the first line may also be the last).
             */

            var transmissions = new List<ArgosTransmission>();

            var platformPattern = new Regex(@"^([0-9]{5}) ([0-9]{5,6}) ", RegexOptions.Compiled);
            var transmissionPattern = new Regex(@"^( {5,6})([0-9]{4})-([0-9]{2})-([0-9]{2})", RegexOptions.Compiled);
            var dataPattern = new Regex(@"^( {35,36})", RegexOptions.Compiled);

            string programId = null;
            string platformId = null;
            string platformheader = null;
            ArgosTransmission transmission = null;
            foreach (var line in lines)
            {
                if (platformPattern.IsMatch(line))
                {
                    programId = line.Substring(0, 5).Trim().TrimStart('0');
                    platformId = line.Substring(6, 6).Trim().TrimStart('0');
                    transmission = null;
                    platformheader = line;
                }

                else if (platformId != null && transmissionPattern.IsMatch(line))
                {
                    var tokens = Regex.Split(line.Trim(), @"\s+");
                    if (tokens.Length == 6 || tokens.Length == 7)
                    {
                        var transmissionDateTime = DateTime.Parse(tokens[0] + " " + tokens[1]);

                        transmission = new ArgosTransmission
                        {
                            ProgramId = programId,
                            PlatformId = platformId,
                            DateTime = transmissionDateTime,
                        };
                        if (platformheader != null)
                        {
                            //only the first transmission in a group gets the header
                            transmission.AddLine(platformheader);
                            platformheader = null;
                        }
                        transmission.AddLine(line);
                        transmission.AddRawBytes(tokens.Skip(3));
                        transmissions.Add(transmission);
                    }
                }
                else if (transmission != null && dataPattern.IsMatch(line))
                {
                    transmission.AddLine(line);
                    var tokens = Regex.Split(line.Trim(), @"\s+");
                    if (tokens.Length == 3 || tokens.Length == 4)
                        transmission.AddRawBytes(tokens);
                }
                else
                {
                    programId = null;
                    platformId = null;
                    transmission = null;
                }
            }
            return transmissions;
        }


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
