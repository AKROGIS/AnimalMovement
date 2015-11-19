using System;

namespace Telonics
{
    public class TpfCollar
    {
        public string Ctn { get; set; }
        public string Platform { get; set; }
        public string PlatformId { get; set; }
        public double Frequency { get; set; }
        public string Owner { get; set; }
        public DateTime TimeStamp { get; set; }
        public TpfFile TpfFile { get; set; }

        public override string ToString()
        {
            return String.Format("File: {5}, CTN: {0}, Platform: {1}, Platform ID: {6}, Frequency: {2}, Owner: {3}, Time Stamp:{4}",
                                 Ctn, Platform, Frequency, Owner, TimeStamp, TpfFile, PlatformId);
        }
    }
}
