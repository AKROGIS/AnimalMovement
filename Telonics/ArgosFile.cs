using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Telonics
{
    public class ArgosFile
    {
        private List<string> _lines;
        private List<ArgosTransmission> _transmissions;
        private List<ArgosGen3Message> _gen3messages;

        public Func<string, TimeSpan> PlatformPeriod { get; set; }
        public Func<string, Boolean> IsGen3Platform { get; set; }
        public Func<string, DateTime, Boolean> IsGen3PlatformOnDate { get; set; }

        #region Constructors and line readers

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

        static IEnumerable<string> ReadLines(Stream stream, Encoding enc)
        {
            using (var reader = new StreamReader(stream, enc))
                while (!reader.EndOfStream)
                    yield return reader.ReadLine();
        }

        #endregion

        #region Private classes

        //ArgosTransmission represent the "raw" data obtained from the input file
        //Once all the raw bytes (AddRawBytes()) have been added, GetMessage() will
        //return the "processed" transmission.
        private class ArgosTransmission
        {
            private List<Byte> message;

            public string ProgramId { get; set; }
            public string PlatformId { get; set; }
            public DateTime DateTime { get; set; }
            public Func<string, TimeSpan> PlatformPeriod { private get; set; }

            public void AddRawBytes(IEnumerable<string> byteStrings)
            {
                if (message == null)
                    message = new List<byte>();
                foreach (var item in byteStrings)
                    message.Add(Byte.Parse(item));
            }

            public ArgosGen3Message GetMessage()
            {
                return new ArgosGen3Message
                {
                    PlatformId = PlatformId,
                    TransmissionDateTime = DateTime,
                    Fixes = GetFixes()
                };
            }

            private ArgosFix[] GetFixes()
            {
                if (message == null)
                    return new ArgosFix[0];

                //Get the message header
                bool messageHasSensorData = message.BooleanAt(0);
                //ignore sensor or error messages
                if (messageHasSensorData)
                    return new ArgosFix[0];
                if (message.Count < 9) //72 bits (9 bytes) required for a full absolute fix
                    return new [] { new ArgosFix { ConditionCode = ArgosConditionCode.Invalid } };

                //Get the absolute Fix
                byte reportedCrc = message.ByteAt(1, 6);
                byte fixBufferType = message.ByteAt(7, 2);
                uint longitudeBits = message.UInt32At(9, 22);
                double longitude = longitudeBits.TwosComplement(22, 4);
                uint latitudeBits = message.UInt32At(31, 21);
                double latitude = latitudeBits.TwosComplement(21, 4);
                ushort julian = message.UInt16At(52, 9);
                byte hour = message.ByteAt(61, 5);
                byte minute = message.ByteAt(66, 6);
                DateTime fixDate = CalculateFixDate(DateTime, julian, hour, minute);

                // Cyclical Redundancy Check
                var crc = new CRC();
                crc.Update(fixBufferType, 2);
                crc.Update((int)longitudeBits, 22);
                crc.Update((int)latitudeBits, 21);
                crc.Update(julian, 9);
                crc.Update(hour, 5);
                crc.Update(minute, 6);
                ArgosConditionCode cCode = (crc.Value == reportedCrc) ? ArgosConditionCode.Good : ArgosConditionCode.Bad;

                //If the CRC was good we need to check for values out of range
                if (cCode == ArgosConditionCode.Good && (julian > 366 || hour > 24 || minute > 60))
                    fixDate = new DateTime(); //use default to indicate an error

                var fixes = new List<ArgosFix>
                    {
                        new ArgosFix
                            {
                                ConditionCode = cCode,
                                Longitude = longitude,
                                Latitude = latitude,
                                DateTime = fixDate
                            }
                    };

                //Setup for the relative fixes
                if (fixBufferType > 3)
                    throw new InvalidDataException("Argos Message has invalid Fix Buffer Type.");
                int numberOfRelativeFixes = (new[] { 0, 3, 4, 5 })[fixBufferType];
                int doubleLength = (new[] { 0, 17, 12, 9 })[fixBufferType];
                int relativeFixLength = (new[] { 0, 46, 36, 30 })[fixBufferType];

                //Get the relative fixes
                for (var i = 0; i < numberOfRelativeFixes; i++)
                {
                    int firstBit = 72 + i * relativeFixLength;
                    int bytesRequired = (firstBit + relativeFixLength + 7) / 8; //+7 to round up
                    if (message.Count < bytesRequired)
                        break;
                    reportedCrc = message.ByteAt(firstBit, 6);
                    firstBit += 6;
                    longitudeBits = message.UInt32At(firstBit, doubleLength);
                    double longitudeDelta = longitudeBits.TwosComplement(doubleLength, 4);
                    firstBit += doubleLength;
                    latitudeBits = message.UInt32At(firstBit, doubleLength);
                    double latitudeDelta = latitudeBits.TwosComplement(doubleLength, 4);
                    firstBit += doubleLength;
                    //Get the time of the relative fixes
                    byte delay = message.ByteAt(firstBit, 6);
                    if (PlatformPeriod == null)
                        throw new InvalidDataException("Function to calculate the platform period was not provided.");
                    TimeSpan platformPeriod = PlatformPeriod(PlatformId);
                    TimeSpan timeOffset = TimeSpan.FromMinutes((i + 1) * platformPeriod.TotalMinutes);
                    fixDate = fixDate.AddMinutes(-fixDate.Minute); //Round down to the hour

                    // Cyclical Redundancy Check
                    crc = new CRC();
                    crc.Update((int)longitudeBits, doubleLength);
                    crc.Update((int)latitudeBits, doubleLength);
                    crc.Update(delay, 6);
                    cCode = (crc.Value == reportedCrc) ? ArgosConditionCode.Good : ArgosConditionCode.Bad;

                    //If the CRC is good we still need to check for values out of range
                    if (cCode == ArgosConditionCode.Good)
                    {
                        //if the 6 bits of delay are all ones the fix could not be acquired
                        if ((delay & 0x3F) == 0x3F)
                            cCode = ArgosConditionCode.Unavailable;
                        if (delay > 60) //60 min is max delay
                            delay = 0;
                    }
                    //NOTE: In some cases Unavailable is reported when CRC was bad, but usually not.

                    DateTime relFixDate;
                    if (fixDate == default(DateTime))
                        relFixDate = new DateTime(); // use default value
                    else
                        relFixDate = fixDate - timeOffset + TimeSpan.FromMinutes(delay);
                    fixes.Add(
                        new ArgosFix
                        {
                            ConditionCode = cCode,
                            Longitude = longitude + longitudeDelta,
                            Latitude = latitude + latitudeDelta,
                            DateTime = relFixDate
                        }
                    );
                }

                return fixes.ToArray();
            }

            private DateTime CalculateFixDate(DateTime transmissionDateTime, ushort dayOfYear, byte hour, byte minute)
            {
                //The fix message reports how much time has past since the begining of the year,
                //but it does not report what year the fix occured in.
                //The transmission and the fix maybe in different years
                //for example, a fix taken on the 364th day of 2010, but not transmitted until Jan 2, 2011

                //Timespans are from the first day of the year, so we subtract one from the dayOfYear
                var fixTimeSpan = new TimeSpan(dayOfYear - 1, hour, minute, 0, 0);
                int transYear = transmissionDateTime.Year;
                TimeSpan transmissionTimeSpan = transmissionDateTime - new DateTime(transYear, 1, 1, 0, 0, 0);
                int fixYear;
                //The fix must occur before the transmission
                if (fixTimeSpan < transmissionTimeSpan)
                    fixYear = transYear;
                else
                    fixYear = transYear - 1;
                DateTime fixDateTime = new DateTime(fixYear, 1, 1, 0, 0, 0) + fixTimeSpan;
                return fixDateTime;
            }

            public override string ToString()
            {
                var msg = String.Join(" ", message.Select(b => b.ToString(CultureInfo.InvariantCulture).PadLeft(8, ' ')));
                var msgb = String.Join(" ", message.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
                return string.Format("[ArgosTransmission: ProgramId={0}, PlatformId={1}, DateTime={2}\n  Message={3}\n  Message={4}]", ProgramId, PlatformId, DateTime, msg, msgb);
            }
        }

        //ArgosMessages hold the list of fixes in a transmission, and
        //can be represented as a comma separated values for output.
        //They are obtained from ArgosTransmission.GetMessage()
        private class ArgosGen3Message
        {
            /* Example CSV Output  from Telonics Gen3 tools:
             * 2008.03.24,00:57:14,77271,1,Good,2008.03.23,16:00,-150.7335,67.3263
             * 2008.03.24,00:57:14,77271,2,Good,2008.03.22,16:00,-150.7079,67.3266
             * 2008.03.24,00:57:14,77271,3,Bad,2008.03.21,16:36,-150.7069,67.1897
             * 2008.03.24,00:57:14,77271,4,Bad,2008.03.20,16:11,-150.5969,67.1896
             * 2008.03.24,00:57:14,77271,5,Bad,2008.03.19,16:25,-150.8698,67.5159
             * 
             * 2008.03.24,01:04:02,77271,1,Good,2008.03.23,16:00,-150.7335,67.3263
             * 2008.03.24,01:04:02,77271,2,Good,2008.03.22,16:00,-150.7079,67.3266
             */

            public string PlatformId { get; set; }
            public DateTime TransmissionDateTime { private get; set; }
            public ArgosFix[] Fixes { private get; set; }

            public IEnumerable<string> FixesAsCsv()
            {
                const string format = "{0},{1},{2},{3},{4},{5},{6},{7},{8}";
                int fixNumber = 0;
                if (Fixes.Length == 0)
                    yield return String.Format("{0},{1},{2},{3},{4},,,,",
                                                TransmissionDateTime.ToString("yyyy.MM.dd"),
                                                TransmissionDateTime.ToString("HH:mm:ss"),
                                                PlatformId, fixNumber, "Invalid");
                foreach (var fix in Fixes)
                {
                    fixNumber++;
                    if (fix.ConditionCode == ArgosConditionCode.Invalid)
                        continue;
                    yield return String.Format(format,
                                               TransmissionDateTime.ToString("yyyy.MM.dd"),
                                               TransmissionDateTime.ToString("HH:mm:ss"),
                                               PlatformId,
                                               fixNumber,
                                               (fix.ConditionCode),
                                               (fix.DateTime == default(DateTime)) ? "Error" : fix.DateTime.ToString("yyyy.MM.dd"),
                                               (fix.DateTime == default(DateTime)) ? "Error" : fix.DateTime.ToString("HH:mm"),
                                               fix.ConditionCode == ArgosConditionCode.Unavailable
                                                   ? ""
                                                   : fix.Longitude.ToString("F4"),
                                               fix.ConditionCode == ArgosConditionCode.Unavailable
                                                   ? ""
                                                   : fix.Latitude.ToString("F4"));
                }
            }

            public override string ToString()
            {
                return string.Format("[ArgosRecord: PlatformId={0}, TransmissionDateTime={1}, NumberOfFixes={2}]", PlatformId, TransmissionDateTime, Fixes.Length);
            }
        }

        //ArgosFix are created by ArgosTransmission and obtained from the ArgosMessage
        private class ArgosFix
        {
            public DateTime DateTime { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public ArgosConditionCode ConditionCode { get; set; }
        }

        #endregion

        #region public API

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

        public IEnumerable<string> GetGen3Messages()
        {
            if (_transmissions == null)
                _transmissions = GetTransmissions(_lines).ToList();
            if (_gen3messages == null)
                _gen3messages = _transmissions.Select(t => t.GetMessage()).ToList();
            return _gen3messages.Select(m => m.ToString());
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

        public IEnumerable<string> ToGen3TelonicsCsv()
        {
            if (_transmissions == null)
                _transmissions = GetTransmissions(_lines).ToList();
            if (_gen3messages == null)
                _gen3messages = _transmissions.Select(t => t.GetMessage()).ToList();
            return _gen3messages.SelectMany(m => m.FixesAsCsv());
        }

        public IEnumerable<string> ToGen3TelonicsCsv(string platform)
        {
            if (platform == null)
                return ToGen3TelonicsCsv();

            if (_transmissions == null)
                _transmissions = GetTransmissions(_lines).ToList();
            if (_gen3messages == null)
                _gen3messages = _transmissions.Select(t => t.GetMessage()).ToList();
            return _gen3messages.Where(m => m.PlatformId == platform).SelectMany(m => m.FixesAsCsv());
        }

        #endregion

        #region private methods

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
            ArgosTransmission transmission = null;
            foreach (var line in lines)
            {
                if (platformPattern.IsMatch(line))
                {
                    programId = line.Substring(0, 5).Trim().TrimStart('0');
                    platformId = line.Substring(6, 6).Trim().TrimStart('0');
                    transmission = null;
                    if (IsGen3Platform != null && !IsGen3Platform(platformId))
                        platformId = null;
                }

                else if (platformId != null && transmissionPattern.IsMatch(line))
                {
                    var tokens = Regex.Split(line.Trim(), @"\s+");
                    if (tokens.Length == 6 || tokens.Length == 7)
                    {
                        var transmissionDateTime = DateTime.Parse(tokens[0] + " " + tokens[1]);
                        if (IsGen3PlatformOnDate != null &&
                            !IsGen3PlatformOnDate(platformId, transmissionDateTime))
                        {
                            platformId = null;
                            continue;
                        }

                        transmission = new ArgosTransmission
                        {
                            ProgramId = programId,
                            PlatformId = platformId,
                            DateTime = transmissionDateTime,
                            PlatformPeriod = PlatformPeriod
                        };
                        transmission.AddRawBytes(tokens.Skip(3));
                        transmissions.Add(transmission);
                    }
                }
                else if (transmission != null && dataPattern.IsMatch(line))
                {
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

        #endregion
    }
}
