using System;
using System.Linq;
using Telonics;

namespace TpfFilesSummerizer
{
    class Program
    {
        static void Main(string[] args)
        {
            var collars = args.SelectMany(arg => TpfFile.LoadFromPath(arg).ParseForCollars());
            Console.WriteLine("Ctn, ArgosId, Frequency, TimeStamp, TpfFile");
            foreach (var collar in collars.OrderBy(c => c.ArgosId))
                //Console.WriteLine(collar);
                Console.WriteLine("{0},{1},{2:f5},{3},{4}", collar.Ctn, collar.ArgosId, collar.Frequency,
                                  collar.TimeStamp.ToLocalTime(), collar.TpfFile);
        }
    }
}
