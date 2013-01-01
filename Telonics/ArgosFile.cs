using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Telonics
{
    public class ArgosFile
    {
		private List<string> _lines;
		private List<ArgosMessage> _messages;

		public Dictionary<string, int> PeriodForPlatform { get; set;}

		public Func<string, Boolean> PlatformCheck {get; set;}
		public Func<string, DateTime, Boolean> PlatformCheckWithDate {get; set;}

		#region Constructors and line readers

		public ArgosFile(string path)
		{
			if (String.IsNullOrEmpty(path))
				throw new ArgumentNullException("path", "path must not be null or empty");
			ReadLines(path);
			if (_lines.Count == 0)
				throw new InvalidDataException("File at path has no lines");			
		}
		
		public ArgosFile(Byte[] bytes)
		{
			if (bytes == null || bytes.Length == 0)
				throw new ArgumentNullException("bytes", "byte array must not be null or empty");
			ReadLines(bytes);
			if (_lines.Count == 0)
				throw new InvalidDataException("Byte array has no lines");			
		}
		
		public ArgosFile(Stream stream)
		{
			if (stream == null)
				throw new ArgumentNullException("stream", "stream must not be null");
			ReadLines(stream);
			if (_lines.Count == 0)
				throw new InvalidDataException("stream has no lines");			
		}


		private void ReadLines(string path)
		{
			_lines = File.ReadAllLines(path).ToList();
		}
		
		private void ReadLines(Byte[] bytes)
		{
			using (var stream = new MemoryStream(bytes,0,bytes.Length))
				_lines = ReadLines(stream, Encoding.UTF8).ToList();
		}
		
		private void ReadLines(Stream stream)
		{
			_lines = ReadLines(stream, Encoding.UTF8).ToList();
		}
		
		static IEnumerable<string> ReadLines(Stream stream, Encoding enc) 
		{
			using (var reader = new StreamReader(stream, enc))
				while (!reader.EndOfStream)
					yield return reader.ReadLine();     
		}

		#endregion

		#region Private classes

		private class ArgosTransmission
		{
			private List<Byte> message;
			
			public string ProgramId { get; set;}
			public string PlatformId { get; set;}
			public DateTime DateTime { get; set;}
			public int Period {get;set;}

			public void AddRawBytes(IEnumerable<string> byteStrings)
			{
				if (message == null)
					message = new List<byte>();
				foreach (var item in byteStrings)
					message.Add(Byte.Parse(item));
			}

			public ArgosMessage GetMessage()
			{
				return new ArgosMessage{
					PlatformId = PlatformId,
					TransmissionDateTime = DateTime,
					Fixes = GetFixes()
				};
			}

			private ArgosFix[] GetFixes()
			{
				if (message == null)
					return new ArgosFix[0];
				
				//Get the message header
				bool messageHasGpsData = message.BooleanAt(0);
				//ignore sensor or error messages
				if (!messageHasGpsData)
					return new ArgosFix[0];
				
				//Get the absolute Fix
				byte reportedCrc = message.ByteAt(1,6);
				byte fixBufferType = message.ByteAt(7,2);
				uint longitudeBits = message.UInt32At(9,22);
				double longitude = longitudeBits.ToSignedBinary(22,4);
				uint latitudeBits = message.UInt32At(31,21);
				double latitude = latitudeBits.ToSignedBinary(21,4);
				ushort julian = message.UInt16At(52,9);
				byte hour = message.ByteAt(61,5);
				byte minute = message.ByteAt(66,6);
				DateTime fixDate = CalculateFixDate(DateTime, julian, hour, minute);

				// Cyclical Redundancy Check
				var crc = new CRC();
				crc.Update(fixBufferType, 2);
				crc.Update((int)longitudeBits, 22);
				crc.Update((int)latitudeBits, 21);
				crc.Update(julian, 9);
				crc.Update(hour, 5);
				crc.Update(minute, 6);
				bool isBad = crc.Value != reportedCrc;

				var fixes = new List<ArgosFix>();
				fixes.Add (
					new ArgosFix {
						IsBad = isBad,
						Longitude = longitude,
						Latitude = latitude,
						DateTime = fixDate
					}
				);

				//Setup for the relative fixes
				if (fixBufferType < 0 || fixBufferType > 3)
					throw new InvalidDataException("Argos Message has invalid Fix Buffer Type.");
				int numberOfRelativeFixes = (new []{0,4,5,6})[fixBufferType];
				int doubleLength = (new []{0,17,12,9})[fixBufferType];
				int relativeFixLength = (new []{0,46,36,30})[fixBufferType];

				//Get the relative fixes
				for (var i = 0; i < numberOfRelativeFixes; i++)
				{
					int firstBit = 72 + i * relativeFixLength;
					reportedCrc = message.ByteAt(firstBit,6);
					firstBit += 6;
					longitudeBits = message.UInt32At(firstBit,doubleLength);
					double longitudeDelta = longitudeBits.ToSignedBinary(doubleLength,4);
					firstBit += doubleLength;
					latitudeBits = message.UInt32At(firstBit,doubleLength);
					double latitudeDelta = latitudeBits.ToSignedBinary(doubleLength,4);
					firstBit += doubleLength;
					byte delay = message.ByteAt(firstBit,6);
					TimeSpan offsetMinutes = TimeSpan.FromMinutes((i+1) * Period - delay);

					// Cyclical Redundancy Check
					crc = new CRC();
					crc.Update((int)longitudeBits, doubleLength);
					crc.Update((int)latitudeBits, doubleLength);
					crc.Update(delay, 6);
					isBad = crc.Value != reportedCrc;
					fixes.Add (
						new ArgosFix {
							IsBad = isBad,
							Longitude = longitude + longitudeDelta,
							Latitude = latitude + latitudeDelta,
							DateTime = fixDate - offsetMinutes
						}
					);
				}

				return fixes.ToArray();
			}

			private DateTime CalculateFixDate (DateTime transmissionDateTime, ushort dayOfYear, byte hour, byte minute)
			{
				//The fix message reports how much time has past since the begining of the year,
				//but it does not report what year the fix occured in.
				//The transmission and the fix maybe in different years
				//for example, a fix taken on the 364th day of 2010, but not transmitted until Jan 2, 2011

				//Timespans are from the first day of the year, so we subtract one from the dayOfYear
				TimeSpan fixTimeSpan = new TimeSpan(dayOfYear - 1, hour, minute, 0, 0);
				int transYear = transmissionDateTime.Year;
				TimeSpan transmissionTimeSpan = transmissionDateTime - new DateTime(transYear,1,1,0,0,0);
				int fixYear; 
				//The fix must occur before the transmission
				if (fixTimeSpan < transmissionTimeSpan)
					fixYear = transYear;
				else
					fixYear = transYear - 1;
				DateTime fixDateTime = new DateTime(fixYear,1,1,0,0,0) + fixTimeSpan;
				return fixDateTime;
			}

			public override string ToString ()
			{
				return string.Format ("[ArgosTransmission: ProgramId={0}, PlatformId={1}, DateTime={2}, Message={3}]", ProgramId, PlatformId, DateTime, message);
			}
		}

		private class ArgosMessage
		{
			/* Examples:
			 * 2008.03.24,00:57:14,77271,1,Good,2008.03.23,16:00,-150.7335,67.3263
			 * 2008.03.24,00:57:14,77271,2,Good,2008.03.22,16:00,-150.7079,67.3266
			 * 2008.03.24,00:57:14,77271,3,Bad,2008.03.21,16:36,-150.7069,67.1897
			 * 2008.03.24,00:57:14,77271,4,Bad,2008.03.20,16:11,-150.5969,67.1896
			 * 2008.03.24,00:57:14,77271,5,Bad,2008.03.19,16:25,-150.8698,67.5159
			 * 
			 * 2008.03.24,01:04:02,77271,1,Good,2008.03.23,16:00,-150.7335,67.3263
			 * 2008.03.24,01:04:02,77271,2,Good,2008.03.22,16:00,-150.7079,67.3266
			 */

			public string PlatformId { get; set;}
			public DateTime TransmissionDateTime { get; set;}
			public ArgosFix[] Fixes {get; set;}

			public string ToTelonicsCsv()
			{
				var sb = new StringBuilder();
				var format = "{0},{1},{2},{3},{4},{5},{6},{7:F4},{8:F4}";
				for (var fixNumber = 1; fixNumber <= Fixes.Length; fixNumber++)
				{
					var fix = Fixes[fixNumber];
					var line = String.Format (format,
					                          TransmissionDateTime.ToString ("yyyy.MM.dd"),
					                          TransmissionDateTime.ToString ("HH:mm.mm"),
					                          PlatformId,
					                          fixNumber,
					                          (fix.IsBad ? "Bad" : "Good"),
					                          fix.DateTime.ToString ("yyyy.MM.dd"),
					                          fix.DateTime.ToString ("HH:mm"),
					                          fix.Longitude,
					                          fix.Latitude);
					sb.AppendLine(line);
				}
				return sb.ToString();
			}

			public override string ToString ()
			{
				return string.Format ("[ArgosRecord: PlatformId={0}, TransmissionDateTime={1}, NumberOfFixes={2}]", PlatformId, TransmissionDateTime, Fixes.Length);
			}
		}
		
		private class ArgosFix
		{
			public DateTime DateTime { get; set;}
			public double Latitude {get;set;}
			public double Longitude {get;set;}
			public bool IsBad {get; set;}
		}

		#endregion

		#region public API

		public IEnumerable<string> ToTelonicsCsv()
		{
			if (_messages == null)
				Parse ();
			foreach (var record in _messages)
				yield return record.ToTelonicsCsv();
		}

		#endregion

		private void Parse()
		{
			_messages = new List<ArgosMessage>();
			foreach(var transmission in GetTransmissions(_lines))
			{
				_messages.Add(transmission.GetMessage());
			}
			if (_messages.Count == 0)
			{
				throw new ApplicationException("File has no Transmissions.");
			}
		}

		public IEnumerable<string> GetTransmissions()
		{
			return GetTransmissions(_lines).Select(t => t.ToString());
		}

		private IEnumerable<ArgosTransmission> GetTransmissions(List<string> lines)
		{
			/* Argos files may be contained in numerous concatenated ASCII email files
			 * Unidentified lines are ignored.  Transmissions contain a variable number
			 * of lines.  The start and end lines will fit a known pattern.
			 *
			 * 09691 095838   9 31 M 3 2012-12-02 20:00:07  65.726  198.504  0.284 401629897
			 *	     2012-12-02 20:03:40  4         12           19          127          112
			 *	                                    76           55           86          
			 *	     2012-12-02 21:34:42  1         12           19          127          112
			 *	                                    76           55           86          110
			 *	                                    26          111          162
			 *
			 * Each group of transmissions starts with the program and platfrom id numbers
			 * in this case the program is 09691, and the platfrom is 095838 the rest of the line
			 * is ignored it is Argos location data and may be empty.
			 * Each transmission starts with the date and time, then a small int, then 3 or 4
			 * bytes as a positive integer from 0 to 255.  Typical lines will have 4 bytes. The
			 * last line will have only 3 bytes (the first line may also be the last).
			 */

			var transmissions = new List<ArgosTransmission>();

			var platformPattern = new Regex(@"^([0-9]{5}) ([0-9]{5,6}) ");
			var transmissionPattern = new Regex(@"^( {5,6})([0-9]{4})-([0-9]{2})-([0-9]{2})");
			var dataPattern = new Regex(@"^( {35,36})");

			string programId = null;
			string platformId = null;
			ArgosTransmission transmission = null;
			foreach (var line in lines)
			{
				if (platformPattern.IsMatch(line))
				{
					programId = line.Substring(0, 5).Trim().TrimStart('0');
					platformId = line.Substring(6, 6).Trim().TrimStart('0');
					transmission = null;
					if (PlatformCheck != null && !PlatformCheck(platformId))
						platformId = null;
				}

				else if (platformId != null && transmissionPattern.IsMatch(line))
				{
					var tokens = Regex.Split(line.Trim(), @"\s+");
					if (tokens.Length == 6 || tokens.Length == 7)
					{
						var transmissionDateTime = DateTime.Parse(tokens[0] + " " + tokens[1]);
						if (PlatformCheckWithDate != null && 
						    !PlatformCheckWithDate(platformId, transmissionDateTime))
						{
							platformId = null;
							break;
						}

						transmission = new ArgosTransmission{
							ProgramId = programId, 
							PlatformId = platformId,
							DateTime = transmissionDateTime,
							Period = PeriodForPlatform[platformId]
						};
						transmission.AddRawBytes(tokens.Skip(3));
						transmissions.Add(transmission);
					}
				}
				else if (transmission != null && dataPattern.IsMatch(line))
				{
					var tokens = Regex.Split(line.Trim(), @"\s+");
					if (tokens.Length == 3 || tokens.Length == 4)
						transmission.AddRawBytes(tokens);
				}
				else
				{
					programId = null;
					platformId = null;
					transmission = null;
				}
			}
			return transmissions;
		}
    }
}