using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace Telonics
{
    //ArgosTransmission represent the "raw" data obtained from the input file
    //Once all the raw bytes (AddRawBytes()) have been added, GetMessage() will
    //return the "processed" transmission.
    public class ArgosTransmission
    {
        private List<Byte> _message;
        private readonly List<string> _lines = new List<string>();

        public string ProgramId { get; set; }
        public string PlatformId { get; set; }
        public DateTime DateTime { get; set; }

        public Byte[] Message
        {
            get { return _message.ToArray(); }
        }

        public void AddRawBytes(IEnumerable<string> byteStrings)
        {
            if (_message == null)
                _message = new List<byte>();
            foreach (var item in byteStrings)
                _message.Add(Byte.Parse(item));
        }

        public void AddLine(string line)
        {
            _lines.Add(line);
        }



        public string ToFormatedString()
        {
            var msg = String.Join(" ", _message.Select(b => b.ToString(CultureInfo.InvariantCulture).PadLeft(8, ' ')));
            var msgb = String.Join(" ", _message.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
            return string.Format("[ArgosTransmission: ProgramId={0}, PlatformId={1}, DateTime={2}\n  Message={3}\n  Message={4}]", ProgramId, PlatformId, DateTime, msg, msgb);
        }

        public override string ToString()
        {
            return string.Join("\n", _lines);
        }
    }

}
