using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper disable PossibleMultipleEnumeration

// This is actually a Lotek (not a Telonics) Collar
// This will decode the Lotek Encoded Argos Message in an Argos File
// It will be returned in the Telonics Gen3 output format.
// There is a lot of Lotek info (activity//mortality) that is not reported in the Gen3 format
// The Lotek tools provide the infomration in several different files with no standard formats.
// TODO: develop a standard file format for all Lotek data

namespace Telonics
{
    public class Gps8000Processor: IProcessor
    {
        #region Public API

        public IEnumerable<string> ProcessTransmissions(IEnumerable<ArgosTransmission> transmissions, ArgosFile file)
        {
            return transmissions.SelectMany(transmission => GetMessage(transmission).FixesAsCsv());
        }

        #endregion

        #region Private Classes

        //ArgosMessages hold the list of fixes in a transmission, and
        //can be represented as a comma separated values for output.
        //They are obtained from ArgosTransmission.GetMessage()
        private class ArgosGen3Message
        {
            /* Example CSV Output  from Telonics Gen3 tools:
             * 2008.03.24,00:57:14,77271,1,Good,2008.03.23,16:00,-150.7335,67.3263
             * 2008.03.24,00:57:14,77271,2,Good,2008.03.22,16:00,-150.7079,67.3266
             * 2008.03.24,00:57:14,77271,3,Bad,2008.03.21,16:36,-150.7069,67.1897
             * 2008.03.24,00:57:14,77271,4,Bad,2008.03.20,16:11,-150.5969,67.1896
             * 2008.03.24,00:57:14,77271,5,Bad,2008.03.19,16:25,-150.8698,67.5159
             * 
             * 2008.03.24,01:04:02,77271,1,Good,2008.03.23,16:00,-150.7335,67.3263
             * 2008.03.24,01:04:02,77271,2,Good,2008.03.22,16:00,-150.7079,67.3266
             */

            public string PlatformId { private get; set; }
            public DateTime TransmissionDateTime { private get; set; }
            public ArgosFix[] Fixes { private get; set; }

            public IEnumerable<string> FixesAsCsv()
            {
                const string format = "{0},{1},{2},{3},{4},{5},{6},{7},{8}";
                int fixNumber = 0;
                if (Fixes.Length == 0)
                    yield return String.Format("{0},{1},{2},{3},{4},,,,",
                                                TransmissionDateTime.ToString("yyyy.MM.dd"),
                                                TransmissionDateTime.ToString("HH:mm:ss"),
                                                PlatformId, fixNumber, "Invalid");
                foreach (var fix in Fixes)
                {
                    fixNumber++;
                    if (fix.ConditionCode == ArgosConditionCode.Invalid)
                        continue;
                    yield return String.Format(format,
                                               TransmissionDateTime.ToString("yyyy.MM.dd"),
                                               TransmissionDateTime.ToString("HH:mm:ss"),
                                               PlatformId,
                                               fixNumber,
                                               (fix.ConditionCode),
                                               (fix.DateTime == default(DateTime)) ? "Error" : fix.DateTime.ToString("yyyy.MM.dd"),
                                               (fix.DateTime == default(DateTime)) ? "Error" : fix.DateTime.ToString("HH:mm"),
                                               fix.ConditionCode == ArgosConditionCode.Unavailable
                                                   ? ""
                                                   : (fix.Longitude < -180 || fix.Longitude > 180) ? "Error" : fix.Longitude.ToString("F7"),
                                               fix.ConditionCode == ArgosConditionCode.Unavailable
                                                   ? ""
                                                   : (fix.Latitude < -90 || fix.Latitude > 90) ? "Error" : fix.Latitude.ToString("F7"));
                }
            }

            public override string ToString()
            {
                return string.Format("[ArgosRecord: PlatformId={0}, TransmissionDateTime={1}, NumberOfFixes={2}]", PlatformId, TransmissionDateTime, Fixes.Length);
            }
        }

        //ArgosFix are created by ArgosTransmission and obtained from the ArgosMessage
        private class ArgosFix
        {
            public DateTime DateTime { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public ArgosConditionCode ConditionCode { get; set; }
        }

        private struct LotekData
        {
            public LotekData(LotekGpsData gpsData)
                : this()
            {
                GpsData = gpsData;
            }
            public LotekData(DateTime mortalityTime)
                : this()
            {
                MortalityTime = mortalityTime;
            }
            public LotekData(LotekGpsData gpsData, LotekActivityData activityData)
                : this()
            {
                GpsData = gpsData;
                ActivityData = activityData;
            }
            public LotekData(LotekGpsData gpsData, DateTime mortalityTime, LotekActivityData activityData)
                : this()
            {
                GpsData = gpsData;
                ActivityData = activityData;
                MortalityTime = mortalityTime;
            }
            public LotekGpsData? GpsData { get; private set; }
            public DateTime? MortalityTime { get; private set; }
            public LotekActivityData? ActivityData { get; private set; }
        }

        private struct LotekGpsData
        {
            public LotekGpsData(double lat, double lon, DateTime timestamp, bool is3D)
                : this()
            {
                Lat = lat;
                Lon = lon;
                Timestamp = timestamp;
                Is3D = is3D;
            }
            public double Lat { get; private set; }
            public double Lon { get; private set; }
            public DateTime Timestamp { get; private set; }
            public bool Is3D { get; private set; }

            public string Csv1
            {
                get
                {
                    return String.Format("{0:d},{1:c},{2:f7},{3:f7},0.00,N/A,{4},No,0.00,0.00,00,", Timestamp.Date,
                        Timestamp.TimeOfDay, Lat, Lon, Is3D ? "3D" : "2D");
                }
            }
        }

        private struct LotekActivityData
        {
            public LotekActivityData(DateTime timestamp, byte? activity1, byte? activity2, byte? activity3, byte? activity4)
                : this()
            {
                RefTime = timestamp;
                Activity1 = activity1;
                Activity2 = activity2;
                Activity3 = activity3;
                Activity4 = activity4;
            }
            public DateTime RefTime { get; private set; }
            public byte? Activity1 { get; private set; }
            public byte? Activity2 { get; private set; }
            public byte? Activity3 { get; private set; }
            public byte? Activity4 { get; private set; }
        }

        #endregion

        #region Private Methods

        private ArgosGen3Message GetMessage(ArgosTransmission transmission)
        {
            return new ArgosGen3Message
            {
                PlatformId = transmission.PlatformId,
                TransmissionDateTime = transmission.DateTime,
                Fixes = GetFixes(transmission.Message)
            };
        }

        private ArgosFix[] GetFixes(ICollection<byte> message)
        {
            if (message == null)
                return new ArgosFix[0];

            var bytes = message.ToArray();
            List<LotekData> data = new List<LotekData>();
            if (bytes.Length == 31)
            {
                switch (bytes[0])
                {
                    case 0x11:
                        data = PinPoint(bytes);
                        break;
                    case 0x10:
                        data = NoActivity_NoMortality(bytes);
                        break;
                    case 0x12:
                        data = NoActivity_Mortality(bytes);
                        break;
                    case 0x14:
                        data = Activity_NoMortality(bytes);
                        break;
                    case 0x16:
                        data = Activity_Mortality(bytes);
                        break;
                    default:
                        Console.WriteLine("Unrecognized header in Argos Message '{0}'.  Skipping", BitConverter.ToString(bytes));
                        break;
                }
            }
            else
            {
                Console.WriteLine("Incorrect message length in Argos Message '{0}'.  Skipping", BitConverter.ToString(bytes));
            }

            return (from item in data
                where item.GpsData != null
                select new ArgosFix
                {
                    ConditionCode = ArgosConditionCode.Good,
                    Longitude = item.GpsData.Value.Lon,
                    Latitude = item.GpsData.Value.Lat,
                    DateTime = item.GpsData.Value.Timestamp
                }).ToArray();
        }

        private static LotekData MortalityOrLocationReader(IEnumerable<byte> data)
        {
            //Console.WriteLine("MortalityReader");
            var crc = CalcCrc8(data.Take(data.Count() - 1));
            if (crc == data.Last())
            {
                var bytes = data.ToArray();
                if (bytes[1] == 0xff && bytes[2] == 0xff && bytes[3] == 0xff &&
                    bytes[7] == 0xff && bytes[8] == 0xff && bytes[9] == 0xff)
                {
                    var mortTimestamp = Time(data.Skip(4).Take(3));
                    return new LotekData(mortTimestamp);
                }
                return new LotekData(GpsReader(data.Skip(1)));
            }
            return new LotekData();
        }

        private static LotekData LocationReader(IEnumerable<byte> data, bool firstByteIsHeader = false)
        {
            //Console.WriteLine("LocationReader");
            var crc = CalcCrc8(data.Take(data.Count() - 1));
            if (crc == data.Last())
            {
                var offset = firstByteIsHeader ? 1 : 0;
                return new LotekData(GpsReader(data.Skip(offset)));
            }
            return new LotekData();
        }

        private static LotekData ActivityReader(IEnumerable<byte> data, bool firstByteIsHeader = false)
        {
            //Console.WriteLine("ActivityReader");
            var crc = CalcCrc8(data.Take(data.Count() - 1));
            //var crc2 = CalcCrc8(data.Skip(1).Take(data.Count() - 1));
            //Console.WriteLine(" crc1: {0:x2}, crc2: {1:x2}, crc3: {2:x2}", crc, crc2, data.Last());
            if (crc == data.Last())
            {
                var offset = firstByteIsHeader ? 1 : 0;
                var gpsData = GpsReader(data.Skip(offset));
                var refTime = gpsData.Timestamp.Round(TimeSpan.FromMinutes(5));
                var activityBits = data.Skip(9 + offset).First();
                var activityBytes = data.Skip(10 + offset).Take(4).ToArray();
                var isMortality = (activityBits & 1) == 1;
                var activity1 = ((activityBits & 2) == 2) ? activityBytes[0] : (byte?)null;
                if (isMortality)
                {
                    var mortTime = Time(activityBytes.Skip(1));
                    var activityData1 = new LotekActivityData(refTime, activity1, null, null, null);
                    return new LotekData(gpsData, mortTime, activityData1);
                }
                var activity2 = ((activityBits & 4) == 4) ? activityBytes[1] : (byte?)null;
                var activity3 = ((activityBits & 8) == 8) ? activityBytes[2] : (byte?)null;
                var activity4 = ((activityBits & 16) == 16) ? activityBytes[3] : (byte?)null;
                var activityData2 = new LotekActivityData(refTime, activity1, activity2, activity3, activity4);
                return new LotekData(gpsData, activityData2);
            }
            return new LotekData();
        }

        private static LotekGpsData GpsReader(IEnumerable<byte> data)
        {
            var is3D = Is3D(data.First());
            var gpsTime = Time(data.Take(3));
            var lon = LatOrLong(data.Skip(3).Take(3));
            var lat = LatOrLong(data.Skip(6).Take(3));
            return new LotekGpsData(lat, lon, gpsTime, is3D);
        }

        private static List<LotekData> PinPoint(byte[] data)
        {
            return new List<LotekData>
            {
                LocationReader(new ArraySegment<byte>(data, 1, 10).ArrayFragment()),
                LocationReader(new ArraySegment<byte>(data, 11, 10).ArrayFragment()),
                LocationReader(new ArraySegment<byte>(data, 21, 10).ArrayFragment())
            };
        }

        private static List<LotekData> NoActivity_NoMortality(byte[] data)
        {
            return new List<LotekData>
            {
                MortalityOrLocationReader(new ArraySegment<byte>(data, 0, 11).ArrayFragment()),
                LocationReader(new ArraySegment<byte>(data, 11, 10).ArrayFragment()),
                LocationReader(new ArraySegment<byte>(data, 21, 10).ArrayFragment())
            };
        }

        private static List<LotekData> NoActivity_Mortality(byte[] data)
        {
            return new List<LotekData>
            {
                MortalityOrLocationReader(new ArraySegment<byte>(data, 0, 11).ArrayFragment()),
                LocationReader(new ArraySegment<byte>(data, 11, 10).ArrayFragment()),
                LocationReader(new ArraySegment<byte>(data, 21, 10).ArrayFragment())
            };
        }

        private static List<LotekData> Activity_NoMortality(byte[] data)
        {
            return new List<LotekData>
            {
                ActivityReader(new ArraySegment<byte>(data, 0, 16).ArrayFragment(), firstByteIsHeader: true),
                ActivityReader(new ArraySegment<byte>(data, 16, 15).ArrayFragment())
            };
        }

        private static List<LotekData> Activity_Mortality(byte[] data)
        {
            return new List<LotekData>
            {
                ActivityReader(new ArraySegment<byte>(data, 0, 16).ArrayFragment(), firstByteIsHeader: true),
                ActivityReader(new ArraySegment<byte>(data, 16, 15).ArrayFragment())
            };
        }

        private static bool Is3D(Byte data)
        {
            const byte firstBit = 1 << 7;
            return (data & firstBit) == firstBit;
        }

        private static DateTime Time(IEnumerable<byte> data)
        {
            var bytes = data.ToArray();
            var number = (bytes[0] << 24) + (bytes[1] << 16) + (bytes[2] << 8) + 0x80;
            number = (number << 1) >> 1;
            var timespan = TimeSpan.FromSeconds(number);
            var basedate = new DateTime(2000, 1, 1);
            return basedate + timespan;
        }

        private static float LatOrLong(IEnumerable<byte> data)
        {
            // float is being used to match the Lotek results/specs
            var bytes = data.ToArray();
            int number = (bytes[0] << 24) + (bytes[1] << 16) + (bytes[2] << 8) + 0x80;
            return number / (float)10000000.0;
        }

        private static readonly byte[] CrcTable = {
                                            0x00,0x07,0x0e,0x09,0x1c,0x1b,0x12,0x15,0x38,0x3f,0x36,0x31,0x24,0x23,0x2a,0x2d,
                                            0x70,0x77,0x7e,0x79,0x6c,0x6b,0x62,0x65,0x48,0x4f,0x46,0x41,0x54,0x53,0x5a,0x5d,
                                            0xe0,0xe7,0xee,0xe9,0xfc,0xfb,0xf2,0xf5,0xd8,0xdf,0xd6,0xd1,0xc4,0xc3,0xca,0xcd,
                                            0x90,0x97,0x9e,0x99,0x8c,0x8b,0x82,0x85,0xa8,0xaf,0xa6,0xa1,0xb4,0xb3,0xba,0xbd,
                                            0xc7,0xc0,0xc9,0xce,0xdb,0xdc,0xd5,0xd2,0xff,0xf8,0xf1,0xf6,0xe3,0xe4,0xed,0xea,
                                            0xb7,0xb0,0xb9,0xbe,0xab,0xac,0xa5,0xa2,0x8f,0x88,0x81,0x86,0x93,0x94,0x9d,0x9a,
                                            0x27,0x20,0x29,0x2e,0x3b,0x3c,0x35,0x32,0x1f,0x18,0x11,0x16,0x03,0x04,0x0d,0x0a,
                                            0x57,0x50,0x59,0x5e,0x4b,0x4c,0x45,0x42,0x6f,0x68,0x61,0x66,0x73,0x74,0x7d,0x7a,
                                            0x89,0x8e,0x87,0x80,0x95,0x92,0x9b,0x9c,0xb1,0xb6,0xbf,0xb8,0xad,0xaa,0xa3,0xa4,
                                            0xf9,0xfe,0xf7,0xf0,0xe5,0xe2,0xeb,0xec,0xc1,0xc6,0xcf,0xc8,0xdd,0xda,0xd3,0xd4,
                                            0x69,0x6e,0x67,0x60,0x75,0x72,0x7b,0x7c,0x51,0x56,0x5f,0x58,0x4d,0x4a,0x43,0x44,
                                            0x19,0x1e,0x17,0x10,0x05,0x02,0x0b,0x0c,0x21,0x26,0x2f,0x28,0x3d,0x3a,0x33,0x34,
                                            0x4e,0x49,0x40,0x47,0x52,0x55,0x5c,0x5b,0x76,0x71,0x78,0x7f,0x6a,0x6d,0x64,0x63,
                                            0x3e,0x39,0x30,0x37,0x22,0x25,0x2c,0x2b,0x06,0x01,0x08,0x0f,0x1a,0x1d,0x14,0x13,
                                            0xae,0xa9,0xa0,0xa7,0xb2,0xb5,0xbc,0xbb,0x96,0x91,0x98,0x9f,0x8a,0x8d,0x84,0x83,
                                            0xde,0xd9,0xd0,0xd7,0xc2,0xc5,0xcc,0xcb,0xe6,0xe1,0xe8,0xef,0xfa,0xfd,0xf4,0xf3
                                        };
        
        private static byte CalcCrc8(IEnumerable<byte> data)
        {
            byte crc = 0;
    	    foreach (byte ele in data) 
	        {
                crc ^= ele;
                crc = CrcTable[crc];
	        }
            return crc;
        }
        
        #endregion
    }

    public static class DateExtensions
    {
        public static DateTime Round(this DateTime date, TimeSpan span)
        {
            long ticks = (date.Ticks + (span.Ticks / 2) + 1) / span.Ticks;
            return new DateTime(ticks * span.Ticks);
        }
    }

    //Work around for lack of IEnumerable<T> interface on ArraySegment
    //FIXME: remove when .net 4.5 is implemented
    static class Extensions
    {
        public static T[] ArrayFragment<T>(this ArraySegment<T> segment)
        {
            var arr = new T[segment.Count];
            Array.Copy(segment.Array, segment.Offset, arr, 0, segment.Count);
            return arr;
        }
    }
}
