using System;
using Telonics;

namespace TelonicsTest
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var a = new ArgosFile("/Users/regan/Projects/AnimalMovement/Telonics/SampleFiles/Gen4bou121203 - multiple email");
			//var o = a.ToTelonicsCsv();
			var o = a.GetTransmissions();
			foreach (var s in o)
				Console.WriteLine (s);
		}
	}
}
