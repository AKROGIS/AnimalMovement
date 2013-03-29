using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

//This is a duplicate of a file in the SqlServer_Files project (SQL CLR assemblies need to be independent)
//changes to one copy should be manually replicated in the other.
namespace Telonics
{
    public class ArgosEmailFile : ArgosFile
    {
        public ArgosEmailFile(string path) : base(path) { }
        public ArgosEmailFile(Byte[] bytes) : base(bytes) { }
        public ArgosEmailFile(Stream stream) : base(stream) { }

        internal override string Header
        {
            get { return String.Empty; }
        }

        //We Fail completely if there is a parse or index error anywhere in the file
        //We could skip lines with parse errors, but we have no way to alert the user
        protected override IEnumerable<ArgosTransmission> GetTransmissions(IEnumerable<string> lines)
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
            ArgosTransmission.ArgosLocation location = null;
            ArgosTransmission transmission = null;
            int lineNumber = 0;
            foreach (var line in lines)
            {
                lineNumber++;
                if (platformPattern.IsMatch(line))
                {
                    programId = line.Substring(0, 5).Trim().TrimStart('0');
                    platformId = line.Substring(6, 6).Trim().TrimStart('0');
                    location = line.Length < 61
                                   ? null
                                   : new ArgosTransmission.ArgosLocation
                                       {
                                           DateTime = DateTime.Parse(line.Substring(24, 19)),
                                           Latitude = Double.Parse(line.Substring(44, 7)),
                                           Longitude = Double.Parse(line.Substring(52, 8)),
                                           Class = line[22]
                                       };
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
                                LineNumber = lineNumber,
                                ProgramId = programId,
                                PlatformId = platformId,
                                DateTime = transmissionDateTime,
                                Location = location,
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
    }
}
