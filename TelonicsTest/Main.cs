using System;
using Telonics;

namespace TelonicsTest
{
	class MainClass
	{

		public static void Main (string[] args)
		{
			//TestCrc();
			TestArgosFile();
			//TestBits();
		}

		public static void TestArgosFile ()
		{
			string path = "/Users/regan/Projects/AnimalMovement/Telonics/SampleFiles/Gen4bou121203 - multiple email";
			Console.WriteLine("File {0}", path);
			var a = new ArgosFile(path);
			a.PlatformPeriod = (p => TimeSpan.FromMinutes(360));
			//Console.WriteLine("Transmissions in File");
			//foreach (var s in a.GetTransmissions())
			//	Console.WriteLine (s);
			Console.WriteLine("Messages in File");
			foreach (var s in a.GetMessages())
				Console.WriteLine (s);
			Console.WriteLine("Programs in File");
			foreach (var p in a.GetPrograms())
				Console.WriteLine("  {0}",p);
			Console.WriteLine("Collars in File");
			foreach (var p in a.GetPlatforms())
				Console.WriteLine("  {0} Start {1} End {2}",p, a.FirstTransmission(p), a.LastTransmission(p));
			Console.WriteLine("CSV Output");
			foreach (var l in a.ToTelonicsCsv())
				Console.WriteLine(l);
		}

		/* Simple Test Program for 6-bit CRC generation algorithm for T03 Format. */
		/****************************** Definitions ********************************/ 
		struct Field
		{
			public Field(int value, int length):this()
			{
				Value = value;
				Length =length;
			}
			public int Value  { get; private set;}
			public int Length  { get; private set;}
		}

		public static void TestCrc ()
		{
			Field[] testCase =
			{
				new Field(4,3),
				new Field(10,4),
				new Field(30,5),
				new Field(55,6),
				new Field(111,7),
				new Field(222,8),
				new Field(950,10),
				new Field(3999,12),
			};

			// Display test values
			Console.WriteLine("\nTest case input values in decimal and binary:");
			for (int i = 0; i < 8; i++ )
			{
				string binaryString = Convert.ToString(testCase[i].Value, 2);
				Console.WriteLine("{0} {1,4} {2,2} {3,12}",
				                  i+1, testCase[i].Value, testCase[i].Length, binaryString); 
			}

			// Calculate CRC value
			var crc = new CRC();
			for (int i = 0; i < 8; i++ )
				crc.Update(testCase[i].Value, testCase[i].Length );

			// Display CRC value
			int a = crc.Value; 	// Get final CRC value
			string binary_str = Convert.ToString(a, 2);
			Console.WriteLine("\n6-bit CRC of all fields = {0:X} (hex) = {1} (binary)", a, binary_str);
			Console.WriteLine("Done.");
		}

		static void TestBits()
		{
			Byte[] bytes = {0,255,123,23,210,3};
			string s = "";
			foreach (var b in bytes)
			{
				s = Convert.ToString(b,2).PadLeft(8,'0');
				Console.WriteLine(s);
			}
			Console.WriteLine("Bit at {0} is {1}",8,bytes.BooleanAt(8));
			Console.WriteLine("Bit at {0} is {1}",9,bytes.BooleanAt(9));
			Console.WriteLine("Bit at {0} is {1}",17,bytes.BooleanAt(17));
			Console.WriteLine("Bit at {0} is {1}",18,bytes.BooleanAt(18));

			Console.WriteLine("Byte at {0},{1} is {2}",7, 3, bytes.ByteAt(7,3));
			Console.WriteLine("Byte at {0},{1} is {2}",8, 3, bytes.ByteAt(8,3));
			Console.WriteLine("Byte at {0},{1} is {2}",9, 3, bytes.ByteAt(9,3));
			Console.WriteLine("Byte at {0},{1} is {2}",5, 8, bytes.ByteAt(5,8));
			Console.WriteLine("UInt16 at {0},{1} is {2}",5, 8, bytes.UInt16At(5,8));
			Console.WriteLine("Uint32 at {0},{1} is {2}",5, 8, bytes.UInt32At(5,8));

			Console.WriteLine("SignedBinary at {0},{1}/{2} is {3}",5, 8, 0, bytes.UInt32At(5,8).ToSignedBinary(8,0));
			Console.WriteLine("SignedBinary at {0},{1}/{2} is {3}",5, 8, 2, bytes.UInt32At(5,8).ToSignedBinary(8,2));
			Console.WriteLine("SignedBinary at {0},{1}/{2} is {3}",9, 6, 0, bytes.UInt32At(9,6).ToSignedBinary(6,0));
			Console.WriteLine("SignedBinary at {0},{1}/{2} is {3}",9, 6, 2, bytes.UInt32At(9,6).ToSignedBinary(6,2));
		}
	}
}
