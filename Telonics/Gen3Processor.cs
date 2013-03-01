using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Telonics
{
    public class Gen3Processor: IProcessor
    {
        #region Public API

        public Gen3Processor(TimeSpan period)
        {
            if (period <= new TimeSpan())
                throw new ArgumentException("Period for a Gen3 Processor must be greater than zero");
            Period = period;
        }

        public TimeSpan Period { get; private set; }

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
                                                   : (fix.Longitude < -180 || fix.Longitude > 180) ? "Error" : fix.Longitude.ToString("F4"),
                                               fix.ConditionCode == ArgosConditionCode.Unavailable
                                                   ? ""
                                                   : (fix.Latitude < -90 || fix.Latitude > 90) ? "Error" : fix.Latitude.ToString("F4"));
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

        #endregion

        #region Private Methods

        private ArgosGen3Message GetMessage(ArgosTransmission transmission)
        {
            return new ArgosGen3Message
            {
                PlatformId = transmission.PlatformId,
                TransmissionDateTime = transmission.DateTime,
                Fixes = GetFixes(transmission.DateTime, transmission.Message)
            };
        }

        private ArgosFix[] GetFixes(DateTime transmissionDateTime, ICollection<byte> _message)
        {
            if (_message == null)
                return new ArgosFix[0];

            //Get the message header
            bool messageHasSensorData = _message.BooleanAt(0);
            //ignore sensor or error messages
            if (messageHasSensorData)
                return new ArgosFix[0];
            if (_message.Count < 9) //72 bits (9 bytes) required for a full absolute fix
                return new[] { new ArgosFix { ConditionCode = ArgosConditionCode.Invalid } };

            //Get the absolute Fix
            byte reportedCrc = _message.ByteAt(1, 6);
            byte fixBufferType = _message.ByteAt(7, 2);
            uint longitudeBits = _message.UInt32At(9, 22);
            double longitude = longitudeBits.TwosComplement(22, 4);
            uint latitudeBits = _message.UInt32At(31, 21);
            double latitude = latitudeBits.TwosComplement(21, 4);
            ushort julian = _message.UInt16At(52, 9);
            byte hour = _message.ByteAt(61, 5);
            byte minute = _message.ByteAt(66, 6);
            DateTime fixDate = CalculateFixDate(transmissionDateTime, julian, hour, minute);

            // Cyclical Redundancy Check
            var crc = new CRC();
            crc.Update(fixBufferType, 2);
            crc.Update((int)longitudeBits, 22);
            crc.Update((int)latitudeBits, 21);
            crc.Update(julian, 9);
            crc.Update(hour, 5);
            crc.Update(minute, 6);
            ArgosConditionCode cCode = (crc.Value == reportedCrc) ? ArgosConditionCode.Good : ArgosConditionCode.Bad;


            var fixes = new List<ArgosFix>
                    {
                        new ArgosFix
                            {
                                ConditionCode = cCode,
                                Longitude = longitude,
                                Latitude = latitude,
                                DateTime = fixDate
                            }
                    };

            //Setup for the relative fixes
            if (fixBufferType > 3)
                throw new InvalidDataException("Argos Message has invalid Fix Buffer Type.");
            int numberOfRelativeFixes = (new[] { 0, 3, 4, 5 })[fixBufferType];
            int doubleLength = (new[] { 0, 17, 12, 9 })[fixBufferType];
            int relativeFixLength = (new[] { 0, 46, 36, 30 })[fixBufferType];

            //Get the relative fixes
            for (var i = 0; i < numberOfRelativeFixes; i++)
            {
                int firstBit = 72 + i * relativeFixLength;
                int bytesRequired = (firstBit + relativeFixLength + 7) / 8; //+7 to round up
                if (_message.Count < bytesRequired)
                    break;
                reportedCrc = _message.ByteAt(firstBit, 6);
                firstBit += 6;
                longitudeBits = _message.UInt32At(firstBit, doubleLength);
                double longitudeDelta = longitudeBits.TwosComplement(doubleLength, 4);
                firstBit += doubleLength;
                latitudeBits = _message.UInt32At(firstBit, doubleLength);
                double latitudeDelta = latitudeBits.TwosComplement(doubleLength, 4);
                firstBit += doubleLength;
                //Get the time of the relative fixes
                byte delay = _message.ByteAt(firstBit, 6);

                TimeSpan timeOffset = TimeSpan.FromMinutes((i + 1) * Period.TotalMinutes);
                fixDate = fixDate.AddMinutes(-fixDate.Minute); //Round down to the hour

                // Cyclical Redundancy Check
                crc = new CRC();
                crc.Update((int)longitudeBits, doubleLength);
                crc.Update((int)latitudeBits, doubleLength);
                crc.Update(delay, 6);
                cCode = (crc.Value == reportedCrc) ? ArgosConditionCode.Good : ArgosConditionCode.Bad;

                //If the CRC is good we still need to check for values out of range
                if (cCode == ArgosConditionCode.Good)
                {
                    //if the 6 bits of delay are all ones the fix could not be acquired
                    if ((delay & 0x3F) == 0x3F)
                        cCode = ArgosConditionCode.Unavailable;
                }
                if (delay > 59) //59 min is max delay
                    delay = 0;
                //NOTE: In some cases Unavailable is reported when CRC was bad, but usually not.

                DateTime relFixDate;
                if (fixDate == default(DateTime))
                    relFixDate = new DateTime(); // use default value
                else
                    relFixDate = fixDate - timeOffset + TimeSpan.FromMinutes(delay);

                fixes.Add(
                    new ArgosFix
                    {
                        ConditionCode = cCode,
                        Longitude = longitude + longitudeDelta,
                        Latitude = latitude + latitudeDelta,
                        DateTime = relFixDate
                    }
                );
            }

            return fixes.ToArray();
        }

        private static DateTime CalculateFixDate(DateTime transmissionDateTime, ushort dayOfYear, byte hour, byte minute)
        {
            //The fix message reports how much time has past since the begining of the year,
            //but it does not report what year the fix occured in.
            //The transmission and the fix maybe in different years
            //for example, a fix taken on the 364th day of 2010, but not transmitted until Jan 2, 2011

            //Check for values out of range
            //We do not know what year this is in, so we are conservative and us 366
            if (dayOfYear > 366 || hour > 23 || minute > 59)
                return default(DateTime); //use default to indicate an error

            //Timespans are from the first day of the year, so we subtract one from the dayOfYear
            var fixTimeSpan = new TimeSpan(dayOfYear - 1, hour, minute, 0, 0);
            int transYear = transmissionDateTime.Year;
            TimeSpan transmissionTimeSpan = transmissionDateTime - new DateTime(transYear, 1, 1, 0, 0, 0);
            int fixYear;
            //The fix must occur before the transmission
            if (fixTimeSpan < transmissionTimeSpan)
                fixYear = transYear;
            else
                fixYear = transYear - 1;
            DateTime fixDateTime = new DateTime(fixYear, 1, 1, 0, 0, 0) + fixTimeSpan;
            return fixDateTime;
        }

        #endregion
    }
}
