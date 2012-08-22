using System;

namespace TpfFilesSummerizer
{
    class Collar
    {
        public string Ctn { get; set; }
        public string ArgosId { get; set; }
        public double Frequency { get; set; }
        public string Owner { get; set; }
        public TpfFile TpfFile { get; set; }

        public override string ToString()
        {
            return String.Format("File: {4}, Collar: {0}, Argos: {1}, Frequency: {2}, Owner: {3}",
                Ctn, ArgosId, Frequency, Owner, TpfFile);
        }
    }
}
