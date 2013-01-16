using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace Telonics
{

    /// <summary>
    /// ArgosTransmission represent a single "raw" Argos transmission
    /// An ArgosFile contains zero or more Argos Transmissions which are created by the ArgosFile
    /// </summary>
    public class ArgosTransmission
    {

        #region Private Fields
        
        private List<Byte> _message;
        private readonly List<string> _lines = new List<string>();

        #endregion

        #region Internal Properties & Methods

        internal ArgosTransmission()
        {
        }

        internal Byte[] Message
        {
            get { return _message.ToArray(); }
        }

        internal void AddRawBytes(IEnumerable<string> byteStrings)
        {
            if (_message == null)
                _message = new List<byte>();
            foreach (var item in byteStrings)
                _message.Add(Byte.Parse(item));
        }

        internal void AddLine(string line)
        {
            _lines.Add(line);
        }

        #endregion

        #region Public API

        public string ProgramId { get; internal set; }
        public string PlatformId { get; internal set; }
        public DateTime DateTime { get; internal set; }


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

        #endregion

    }

}
