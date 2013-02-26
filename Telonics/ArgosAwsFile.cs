using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Telonics
{
    public class ArgosAwsFile : ArgosFile
    {
        public ArgosAwsFile(string path) : base(path) {}

        public ArgosAwsFile(byte[] bytes) : base(bytes) {}

        public ArgosAwsFile(Stream stream) : base(stream) {}

        internal override string Header
        {
            get
            {
                return "\"programNumber\";\"platformId\";\"platformType\";\"platformModel\";\"platformName\";\"platformHexId\";\"satellite\";\"bestMsgDate\";\"duration\";\"nbMessage\";\"message120\";\"bestLevel\";\"frequency\";\"locationDate\";\"latitude\";\"longitude\";\"altitude\";\"locationClass\";\"gpsSpeed\";\"gpsHeading\";\"latitude2\";\"longitude2\";\"altitude2\";\"index\";\"nopc\";\"errorRadius\";\"semiMajor\";\"semiMinor\";\"orientation\";\"hdop\";\"bestDate\";\"compression\";\"type\";\"alarm\";\"concatenated\";\"date\";\"level\";\"doppler\";\"rawData\"\n";
            }
        }

        protected override IEnumerable<ArgosTransmission> GetTransmissions(IEnumerable<string> lines)
        {
            //Each line looks like \"abc\";\"def\";\"pdq\";\"xyz\";
            foreach (var line in lines.Skip(1))
            {
                var tokens = line.Substring(1, line.Length - 3).Split(new[] {"\";\""}, StringSplitOptions.None);
                var transmission = new ArgosTransmission
                    {
                        ProgramId = tokens[0],
                        PlatformId = tokens[1],
                        DateTime = DateTime.Parse(tokens[7], CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
                        Location = String.IsNullOrEmpty(tokens[13]) ? null : new ArgosTransmission.ArgosLocation
                        {
                            DateTime = DateTime.Parse(tokens[13]),
                            Latitude = Double.Parse(tokens[14]),
                            Longitude = Double.Parse(tokens[15]),
                            Class = tokens[17][0]
                        }
                    };
                transmission.AddHexString(tokens[38]);
                transmission.AddLine(line);
                yield return transmission;
            }
        }
    }
}
