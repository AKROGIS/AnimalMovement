using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Server;

namespace SqlServer_Files
{
    public class CollarFileInfo
    {
        [SqlFunction(IsDeterministic = true, IsPrecise = true)]
        public static char FileFormat(SqlBytes data)
        {
            return 'X';
        }

        [SqlProcedure]
        public static void Summerize(SqlInt32 fileId)
        {
            return;
        }
    }
}
