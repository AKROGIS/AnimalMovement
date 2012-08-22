using System;
using System.Linq;

namespace TpfFilesSummerizer
{
    class Program
    {
        static void Main(string[] args)
        {
            var collars = args.SelectMany(arg => TpfFile.LoadFromPath(arg).ParseForCollars());
            foreach (var collar in collars.OrderBy(c => c.ArgosId))
                //Console.WriteLine(collar);
                Console.WriteLine("{0},{1},{2:f2},{3}", collar.Ctn, collar.ArgosId, collar.Frequency, collar.TpfFile);
        }
    }
}


