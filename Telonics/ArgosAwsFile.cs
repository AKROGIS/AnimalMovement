using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

//This is a duplicate of a file in the SqlServer_Files project (SQL CLR assemblies need to be independent)
//changes to one copy should be manually replicated in the other.
namespace Telonics
{
    public class ArgosAwsFile : ArgosFile
    {
        public ArgosAwsFile(string path) : base(path) { }

        public ArgosAwsFile(byte[] bytes) : base(bytes) { }

        public ArgosAwsFile(Stream stream) : base(stream) { }

        internal override string Header => "\"programNumber\";\"platformId\";\"platformType\";\"platformModel\";\"platformName\";\"platformHexId\";\"satellite\";\"bestMsgDate\";\"duration\";\"nbMessage\";\"message120\";\"bestLevel\";\"frequency\";\"locationDate\";\"latitude\";\"longitude\";\"altitude\";\"locationClass\";\"gpsSpeed\";\"gpsHeading\";\"latitude2\";\"longitude2\";\"altitude2\";\"index\";\"nopc\";\"errorRadius\";\"semiMajor\";\"semiMinor\";\"orientation\";\"hdop\";\"bestDate\";\"compression\";\"type\";\"alarm\";\"concatenated\";\"date\";\"level\";\"doppler\";\"rawData\"\n";

        /// <summary>
        /// AWS files may be too large for the server to return all records.
        /// </summary>
        public bool? MaxResponseReached { get; private set; }

        //We Fail completely if there is a parse or index error anywhere in the file
        //We could skip lines with parse errors, but we have no way to alert the user
        protected override IEnumerable<ArgosTransmission> GetTransmissions(IEnumerable<string> lines)
        {
            //Each line looks like \"abc\";\"def\";\"pdq\";\"xyz\";
            int lineNumber = 1;
            foreach (var line in lines.Skip(1))
            {
                lineNumber++;
                if (String.Equals(line.Trim(), "MAX_RESPONSE_REACHED", StringComparison.InvariantCultureIgnoreCase))
                {
                    MaxResponseReached = true;
                    yield break;
                }
                var tokens = line.Substring(1, line.Length - 3).Split(new[] { "\";\"" }, StringSplitOptions.None);
                var transmission = new ArgosTransmission
                {
                    LineNumber = lineNumber,
                    ProgramId = tokens[0],
                    PlatformId = tokens[1],
                    DateTime = DateTime.Parse(tokens[7], CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
                    Location = String.IsNullOrEmpty(tokens[13]) ? null : new ArgosTransmission.ArgosLocation
                    {
                        DateTime = DateTime.Parse(tokens[13], CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
                        Latitude = Single.Parse(tokens[14]),
                        Longitude = Single.Parse(tokens[15]),
                        Altitude = Single.Parse(tokens[16]),
                        Class = tokens[17][0]
                    }
                };
                transmission.AddHexString(tokens[38]);
                transmission.AddLine(line);
                yield return transmission;
            }
            MaxResponseReached = false;
        }
    }
}
