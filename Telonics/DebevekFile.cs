using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Telonics
{
    public class DebevekFile : ArgosFile
    {
        public DebevekFile(string path) : base(path) { }

        public DebevekFile(byte[] bytes) : base(bytes) { }

        public DebevekFile(Stream stream) : base(stream) { }

        private string _header = String.Empty;

        internal override string Header
        {
            get { return _header; }
        }

        protected override IEnumerable<ArgosTransmission> GetTransmissions(IEnumerable<string> lines)
        {
            //The debevek file contains fixes not transmissions,
            //but since they all contain an Argos Id we can fake it.
            var platformIndex = -1;
            var dateIndex = -1;
            var timeIndex = -1;
            var latIndex = -1;
            var lonIndex = -1;

            foreach (var line in lines)
            {
                var cleanLine = line.Trim();
                if (cleanLine == String.Empty)
                    continue;
                if (_header == String.Empty)
                {
                    _header = cleanLine;
                    var columns = _header.Split(new[] { '\t', ',' });
                    for (int i = 0; i < columns.Length; i++)
                    {
                        var column = columns[i];
                        if (String.Equals(column, "CollarID", StringComparison.InvariantCultureIgnoreCase))
                            platformIndex = i;
                        if (String.Equals(column, "FixDate", StringComparison.InvariantCultureIgnoreCase))
                            dateIndex = i;
                        if (String.Equals(column, "FixTime", StringComparison.InvariantCultureIgnoreCase))
                            timeIndex = i;
                        if (String.Equals(column, "LatWGS84", StringComparison.InvariantCultureIgnoreCase))
                            latIndex = i;
                        if (String.Equals(column, "LonWGS84", StringComparison.InvariantCultureIgnoreCase))
                            lonIndex = i;
                    }
                    if (platformIndex == -1 || dateIndex == -1 || timeIndex == -1 || latIndex == -1 || lonIndex == -1)
                        yield break;
                    continue;
                }

                var tokens = cleanLine.Split(new[] { '\t', ',' });
                var dateTime = DateTime.Parse(tokens[dateIndex] + " " + tokens[dateIndex], CultureInfo.InvariantCulture,
                                              DateTimeStyles.RoundtripKind);
                var transmission = new ArgosTransmission
                {
                    ProgramId = null,
                    PlatformId = tokens[platformIndex],
                    DateTime = dateTime,
                    Location = String.IsNullOrEmpty(tokens[latIndex]) ? null : new ArgosTransmission.ArgosLocation
                    {
                        DateTime = dateTime,
                        Latitude = Double.Parse(tokens[latIndex]),
                        Longitude = Double.Parse(tokens[lonIndex])
                    }
                };
                transmission.AddLine(line);
                yield return transmission;
            }
        }
    }
}
