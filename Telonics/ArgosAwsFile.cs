using System;
using System.Collections.Generic;
using System.IO;

namespace Telonics
{
    public class ArgosAwsFile : ArgosFile
    {
        public ArgosAwsFile(string path) : base(path) {}

        public ArgosAwsFile(byte[] bytes) : base(bytes) {}

        public ArgosAwsFile(Stream stream) : base(stream) {}

        protected override IEnumerable<ArgosTransmission> GetTransmissions(IEnumerable<string> lines)
        {
            throw new NotImplementedException();
        }
    }
}
