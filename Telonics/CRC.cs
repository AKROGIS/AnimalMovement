namespace Telonics
{

    /***************************************************************************
     * These are the T03 Format 6-bit CRC routines
     **************************************************************************/
    internal class CRC
    {
        private int _crc;

        public void Update(int value, int numberOfBits)
        {
            const byte FEEDBACK = 0x43;
            const byte MAXTAP = 0x40;

            // Left justify the input value
            for (int i = numberOfBits; i < 32; i++)
                value <<= 1;

            // Update CRC
            for (int i = 0; i < numberOfBits; i++, value <<= 1)
            {
                bool bit1 = (_crc & MAXTAP) != 0; // ? 1: 0;			// Get bit from CRC
                bool bit2 = (value & 0x80000000L) != 0; //? 1: 0;	// Get bit from input value
                if (bit1 ^ bit2)
                    _crc ^= FEEDBACK;
                _crc <<= 1;
            }
        }

        //Returns the calculated CRC value, right justified and limited to 6 bits.
        public int Value
        {
            get
            {
                int crc = _crc >> 1; //Right justify to eliminate extra zero bit
                crc &= 0x3F; //Limit to 6 bits 
                return crc;
            }
        }

    }
}
