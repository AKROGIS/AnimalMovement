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
            foreach (var collar in collars.OrderBy(c => c.ArgosId))
                //Console.WriteLine(collar);
                Console.WriteLine("{0},{1},{2:f3},{3}", collar.Ctn, collar.ArgosId, collar.Frequency, collar.TpfFile);
        }
    }
}
