using System;
using System.Collections.Generic;
using System.Linq;

namespace Telonics
{
    public static class ByteArrayExtensions
    {
        public static bool BooleanAt(this IEnumerable<byte> data, int bit)
        {
            //bit is a zero-based counter from left to right
            if (bit < 0)
                throw new IndexOutOfRangeException("Bit index must be greater than or equal to 0");
            int indexOfByte = bit / 8;
            int bitInByte = bit % 8;
            byte mask = 1;
            mask <<= 7 - bitInByte;
            byte b = data.Skip(indexOfByte).First();
            return (b & mask) != 0;
        }

        public static byte ByteAt(this IEnumerable<byte> data, int startBit, int bitCount)
        {
            if (bitCount > 8)
                throw new IndexOutOfRangeException("Bit count for a byte must be less than 9");
            string s = GetBitString(data, startBit, bitCount);
            return Convert.ToByte(s, 2);
        }

        public static UInt16 UInt16At(this IEnumerable<byte> data, int startBit, int bitCount)
        {
            if (bitCount > 16)
                throw new IndexOutOfRangeException("Bit count for a 16 bit integer must be less than 17");
            string s = GetBitString(data, startBit, bitCount);
            return Convert.ToUInt16(s, 2);
        }

        public static UInt32 UInt32At(this IEnumerable<byte> data, int startBit, int bitCount)
        {
            if (bitCount > 32)
                throw new IndexOutOfRangeException("Bit count for a 32 bit integer must be less than 33");
            string s = GetBitString(data, startBit, bitCount);
            return Convert.ToUInt32(s, 2);
        }

        private static string GetBitString(IEnumerable<byte> data, int startBit, int bitCount)
        {
            //startBit is a zero-based counter from left to right
            if (bitCount < 1)
                throw new IndexOutOfRangeException("Bit count must be greater than 0");
            if (startBit < 0)
                throw new IndexOutOfRangeException("starting bit index must be greater than or equal to 0");
            int indexOfFirstByte = startBit / 8;
            int indexOfLastByte = (startBit + bitCount) / 8;
            int byteCount = 1 + indexOfLastByte - indexOfFirstByte;
            string s = String.Join("", data.Skip(indexOfFirstByte).Take(byteCount).Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
            int bitInByte = startBit % 8;
            s = s.Substring(bitInByte, bitCount);
            return s;
        }

        public static double ToSignedBinary(this UInt32 number, int signBit, byte decimals)
        {
            //signBit is the number of right aligned bits used in the number, the left most of those is the sign bit
            if (signBit < 2)
                throw new IndexOutOfRangeException("Sign bit must be greater than 0");
            if (signBit > 32)
                throw new IndexOutOfRangeException("Sign bit count for a 32bit integer must be less than 33");
            int signMask = 1 << signBit - 1;
            int sign = (number & signMask) == 0 ? 1 : -1;
            uint numberMask = (uint)Math.Pow(2, signBit - 1) - 1;
            uint numberWithoutSign = number & numberMask;
            double decimalShift = Math.Pow(10, decimals);
            return sign * numberWithoutSign / decimalShift;
        }

        public static double TwosComplement(this UInt32 number, int length, byte decimals)
        {
            //signBit is the number of right aligned bits used in the number, the left most of those is the sign bit
            if (length < 1)
                throw new IndexOutOfRangeException("Length must be greater than 0");
            if (32 < length)
                throw new IndexOutOfRangeException("Length for a 32bit integer must be less than 33");
            int max = 1 << length - 1;
            int offset = 1 << length;
            double decimalShift = Math.Pow(10, decimals);
            if (number >= max)
                return (number - offset) / decimalShift;
            return number / decimalShift;
        }
    }
}
