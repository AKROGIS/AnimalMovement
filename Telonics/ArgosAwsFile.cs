using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Telonics
{
    public class ArgosAwsFile : ArgosFile
    {
        public ArgosAwsFile(string path) : base(path) {}

        public ArgosAwsFile(byte[] bytes) : base(bytes) {}

        public ArgosAwsFile(Stream stream) : base(stream) {}

        protected override IEnumerable<ArgosTransmission> GetTransmissions(IEnumerable<string> lines)
        {
            //Each line looks like "abc";"def";"pdq";"xyz"
            return
                lines.Select(line => line.Substring(1, line.Length - 2).Split(new[] {"\";\""}, StringSplitOptions.None))
                     .Select(tokens =>
                         {
                             var transmission = new ArgosTransmission
                                 {
                                     ProgramId = tokens[0],
                                     PlatformId = tokens[1],
                                     DateTime = DateTime.Parse(tokens[7]),
                                     LocationDateTime = DateTime.Parse(tokens[13]),
                                     Latitude = Double.Parse(tokens[14]),
                                     Longitude = Double.Parse(tokens[15])
                                 };
                             transmission.AddHexString(tokens[38]);
                             return transmission;
                         });
        }
    }
}
