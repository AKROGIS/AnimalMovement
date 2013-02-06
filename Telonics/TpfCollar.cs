using System;

namespace Telonics
{
    public class TpfCollar
    {
        public string Ctn { get; set; }
        public string ArgosId { get; set; }
        public double Frequency { get; set; }
        public string Owner { get; set; }
        public DateTime TimeStamp { get; set; }
        public TpfFile TpfFile { get; set; }

        public override string ToString()
        {
            return String.Format("File: {5}, CTN: {0}, Argos ID: {1}, Frequency: {2}, Owner: {3}, Time Stamp:{4}",
                                 Ctn, ArgosId, Frequency, Owner, TimeStamp, TpfFile);
        }
    }
}
