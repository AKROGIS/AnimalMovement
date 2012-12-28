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
		private List<ArgosRecord> _records;

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

			public void AddRawBytes(IEnumerable<string> byteStrings)
			{
				if (message == null)
					message = new List<byte>();
				foreach (var item in byteStrings)
					message.Add(Byte.Parse(item));
			}

			public IEnumerable<ArgosRecord> GetRecords()
			{
				if (message == null)
					return new ArgosRecord[0];
				// parse the byte array
				throw new NotImplementedException();
			}

		}

		private class ArgosRecord
		{
			/* Example:
			 * 2008.03.24,00:57:14,77271,1,Good,2008.03.23,16:00,-150.7335,67.3263
			 * 2008.03.24,00:57:14,77271,2,Good,2008.03.22,16:00,-150.7079,67.3266
			 * 2008.03.24,00:57:14,77271,3,Bad,2008.03.21,16:36,-150.7069,67.1897
			 * 2008.03.24,00:57:14,77271,4,Bad,2008.03.20,16:11,-150.5969,67.1896
			 * 2008.03.24,00:57:14,77271,5,Bad,2008.03.19,16:25,-150.8698,67.5159
			 * 2008.03.24,01:04:02,77271,1,Good,2008.03.23,16:00,-150.7335,67.3263
			 * 2008.03.24,01:04:02,77271,2,Good,2008.03.22,16:00,-150.7079,67.3266
			 */

			public string PlatformId { get; set;}
			public DateTime TransmissionDateTime { get; set;}
			public DateTime FixDateTime { get; set;}
			public int FixNumber {get;set;}
			public bool IsBad {get; set;}
			public double Latitude {get;set;}
			public double Longitude {get;set;}

			public string ToTelonicsCsv()
			{
				var format = "{0},{1},{2},{3},{4},{5},{6},{7:F4},{8:F4}";
				var line = String.Format (format,
				                          TransmissionDateTime.ToString ("yyyy.MM.dd"),
				                          TransmissionDateTime.ToString ("HH:mm.mm"),
				                          PlatformId,
				                          FixNumber,
				                          (IsBad ? "Bad" : "Good"),
				                          FixDateTime.ToString ("yyyy.MM.dd"),
				                          FixDateTime.ToString ("HH:mm"),
				                          Longitude,
				                          Latitude);
				return line;
			}
		}
		
		private class ArgosHeader
		{
		}
		
		private class ArgosFix
		{
		}

		#endregion

		#region public API

		public IEnumerable<string> ToTelonicsCsv()
		{
			if (_records == null)
				Parse ();
			foreach (var record in _records)
				yield return record.ToTelonicsCsv();
		}

		#endregion

		private void Parse()
		{
			_records = new List<ArgosRecord>();
			foreach(var transmission in GetTransmissions(_lines))
			{
				foreach(var record in transmission.GetRecords())
				{
					_records.Add(record);
				}
			}
			if (_records.Count == 0)
			{
				throw new ApplicationException("File has no Transmissions.");
			}
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
					var tokens = line.Split();
					if (tokens.Length == 6 || tokens.Length == 7)
					{
						var transmissionDateTime = DateTime.Parse(line);
						if (PlatformCheckWithDate != null && 
						    !PlatformCheckWithDate(platformId, transmissionDateTime))
						{
							platformId = null;
							break;
						}

						transmission = new ArgosTransmission{
							ProgramId = programId, 
							PlatformId = platformId,
							DateTime = transmissionDateTime};
						transmission.AddRawBytes(tokens.Skip(3));
						transmissions.Add(transmission);
					}
				}
				else if (transmission != null && dataPattern.IsMatch(line))
				{
					var tokens = line.Split();
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
