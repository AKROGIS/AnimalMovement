using System;
using System.Linq;
using Telonics;

namespace TpfFilesSummerizer
{
    static class Program
    {
        static void Main(string[] args)
        {
            var collars = args.SelectMany(arg => new TpfFile(arg).GetCollars());
            Console.WriteLine("Ctn, Platform, PlatformId, Frequency, TimeStamp, TpfFile");
            foreach (var collar in collars.OrderBy(c => c.PlatformId))
            {
                //Console.WriteLine(collar);
                Console.WriteLine("{0},{5},{1},{2:f5},{3},{4}", collar.Ctn, collar.PlatformId, collar.Frequency,
                                  collar.TimeStamp.ToLocalTime(), collar.TpfFile, collar.Platform);
            }
        }
    }
}
