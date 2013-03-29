using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;  //in mscorlib - For Hex to Byte[]

//This is a duplicate of a file in the SqlServer_Files project (SQL CLR assemblies need to be independent)
//changes to one copy should be manually replicated in the other.
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

        public int LineNumber { get; internal set; }
        public string ProgramId { get; internal set; }
        public string PlatformId { get; internal set; }
        public DateTime DateTime { get; internal set; }
        public ArgosLocation Location { get; internal set; }


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
                    "ArgosTransmission for platform {0}/{1} at {2} {3}, and has{4}Telonics GPS message.", ProgramId,
                    PlatformId, DateTime,
                    Location == null
                        ? "has no location"
                        : String.Format("at ({0},{1})", Location.Longitude, Location.Latitude),
                    _message != null ? " " : " NO ");
        }

        public override string ToString()
        {
            //.net4.0 required for Join(string, IEnumerable<string>)
            return String.Join("\n", _lines.ToArray());
        }

        public class ArgosLocation
        {
            public DateTime DateTime { get; internal set; }
            public float Latitude { get; internal set; }
            public float Longitude { get; internal set; }
            public float Altitude { get; internal set; }
            public char Class { get; internal set; }
        }

        #endregion

    }

}
