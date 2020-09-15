using Microsoft.SqlServer.Server;
using System.Data.SqlTypes;
using System.Security.Cryptography;

namespace SqlServer_Functions
{
    public class SimpleFunctions
    {
        [SqlFunction]
        public static SqlDateTime LocalTime(SqlDateTime utcDateTime)
        {
            if (utcDateTime.IsNull)
            {
                return SqlDateTime.Null;
            }

            return new SqlDateTime(utcDateTime.Value.ToLocalTime());
        }

        [SqlFunction]
        public static SqlDateTime UtcTime(SqlDateTime localDateTime)
        {
            if (localDateTime.IsNull)
            {
                return SqlDateTime.Null;
            }

            return new SqlDateTime(localDateTime.Value.ToUniversalTime());
        }

        [SqlFunction(IsDeterministic = true, IsPrecise = true)]
        public static SqlBinary Sha1Hash(SqlBytes data)
        {
            return new SHA1CryptoServiceProvider().ComputeHash(data.Stream);
        }
    }
}
