using System.Data;
using Microsoft.SqlServer.Server;

namespace SqlServerExtensions
{
    public class AnimalMovementProcedures
    {
        [SqlProcedure]
        public static void GetPeople()
        {
            var name = new SqlMetaData("Name", SqlDbType.VarChar, 30);
            var age = new SqlMetaData("Age", SqlDbType.Int);

            SqlContext.Pipe.SendResultsStart(new SqlDataRecord(name, age));

            var record = new SqlDataRecord(name, age);
            record.SetString(0, "John Doe");
            record.SetInt32(1, 26);
            SqlContext.Pipe.SendResultsRow(record);

            record = new SqlDataRecord(name, age);
            record.SetString(0, "Alfred Smith");
            record.SetInt32(1, 43);
            SqlContext.Pipe.SendResultsRow(record);

            record = new SqlDataRecord(name, age);
            record.SetString(0, "Jennifer Eason");
            record.SetInt32(1, 36);
            SqlContext.Pipe.SendResultsRow(record);

            SqlContext.Pipe.SendResultsEnd();
        }
    };
}
