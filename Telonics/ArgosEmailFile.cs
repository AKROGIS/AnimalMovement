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
             * 
             * In some email files, leading white space is removed, and remaining white space is
             * normalized to a single space per whitespace group; i.e. \s+ => ' '
             */

            var transmissions = new List<ArgosTransmission>();

            var platformPattern = new Regex(@"^\s*([0-9]{5})\s+([0-9]{5,6})\s+", RegexOptions.Compiled);  //Be more liberal with whitespace
            var transmissionPattern = new Regex(@"^\s*([0-9]{4})-([0-9]{2})-([0-9]{2})", RegexOptions.Compiled); //Ignore leading space
            var dataPattern = new Regex(@"^[\s0-9]*$", RegexOptions.Compiled);  //Just white space and decimal digits

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
                    var tokens = Regex.Split(line.Trim(), @"\s+");
                    //do not use String.Split() - It splits on each whitespace character, not groups of whitespace
                    //since we passed the Regex Match, we know there are at least two tokens
                    programId = tokens[0].TrimStart('0');
                    platformId = tokens[1].TrimStart('0');
                    location = tokens.Length < 11
                                   ? null
                                   : new ArgosTransmission.ArgosLocation
                                   {
                                       DateTime = DateTime.Parse(tokens[6] + " " + tokens[7]),
                                       Latitude = Single.Parse(tokens[8]),
                                       Longitude = Single.Parse(tokens[9]),
                                       Altitude = Single.Parse(tokens[10]),
                                       Class = tokens[5][0]
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
