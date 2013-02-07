using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;  //in mscorlib - For Hex to Byte[]

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

        internal void AddRawBytes(IEnumerable<string> byteStrings)
        {
            if (_message == null)
                _message = new List<byte>();
            foreach (var item in byteStrings)
                _message.Add(Byte.Parse(item));
        }

        internal void AddHexString(string hexString)
        {
            if (_message == null)
                _message = new List<byte>();
            _message.AddRange(SoapHexBinary.Parse(hexString).Value); 
        }

        public void AddLine(string line)
        {
            _lines.Add(line);
        }

        #endregion

        #region Public API

        public string ProgramId { get; internal set; }
        public string PlatformId { get; internal set; }
        public DateTime DateTime { get; internal set; }
        public double Latitude { get; internal set; }
        public double Longitude { get; internal set; }

        public IEnumerable<string> Lines
        {
            get { return _lines.ToArray(); }
        }

        internal Byte[] Message
        {
            get { return _message.ToArray(); }
        }

        public string ToFormatedString()
        {
            return
                string.Format(
                    "[ArgosTransmission: Platform {0}/{1} at ({2},{3}) at {4}.  Has{5}Telonics GPS message\n", ProgramId,
                    PlatformId, Longitude, Latitude, DateTime, _message != null ? " " : " no ");
        }

        public override string ToString()
        {
            return string.Join("\n", _lines);
        }

        #endregion

    }

}
