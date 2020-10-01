using Microsoft.SqlServer.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SqlServer_Parsers
{

    // See http://msdn.microsoft.com/en-us/library/ms131103.aspx
    // for more information on creating CLR Table-Valued Functions

    public class VectronicParsers
    {
        #region SQL Server Table Value Functions
        //Code - Format
        //   A-N - See Parsers.cs
        //   O - Vectronic location data downloaded via the HTTP Wildlife API v2
        //   P - Vectronic activity data downloaded via the HTTP Wildlife API v2
        //   Q - Vectronic mortality data downloaded via the HTTP Wildlife API v2


        // O - Vectronic location data downloaded via the HTTP Wildlife API v2

        [SqlFunction(
            DataAccess = DataAccessKind.Read,
            FillRowMethodName = "FormatO_FillRow",
            TableDefinition =
                @"[LineNumber] [int],
	            [idPosition] [nvarchar](50),
	            [idCollar] [nvarchar](50),
	            [acquisitionTime] [nvarchar](50),
	            [scts] [nvarchar](50),
	            [originCode] [nvarchar](50),
	            [ecefX] [nvarchar](50),
	            [ecefY] [nvarchar](50),
	            [ecefZ] [nvarchar](50),
	            [latitude] [nvarchar](50),
	            [longitude] [nvarchar](50),
	            [height] [nvarchar](50),
	            [dop] [nvarchar](50),
	            [idFixType] [nvarchar](50),
	            [positionError] [nvarchar](50),
	            [satCount] [nvarchar](50),
	            [ch01SatId] [nvarchar](50),
	            [ch01SatCnr] [nvarchar](50),
	            [ch02SatId] [nvarchar](50),
	            [ch02SatCnr] [nvarchar](50),
	            [ch03SatId] [nvarchar](50),
	            [ch03SatCnr] [nvarchar](50),
	            [ch04SatId] [nvarchar](50),
	            [ch04SatCnr] [nvarchar](50),
	            [ch05SatId] [nvarchar](50),
	            [ch05SatCnr] [nvarchar](50),
	            [ch06SatId] [nvarchar](50),
	            [ch06SatCnr] [nvarchar](50),
	            [ch07SatId] [nvarchar](50),
	            [ch07SatCnr] [nvarchar](50),
	            [ch08SatId] [nvarchar](50),
	            [ch08SatCnr] [nvarchar](50),
	            [ch09SatId] [nvarchar](50),
	            [ch09SatCnr] [nvarchar](50),
	            [ch10SatId] [nvarchar](50),
	            [ch10SatCnr] [nvarchar](50),
	            [ch11SatId] [nvarchar](50),
	            [ch11SatCnr] [nvarchar](50),
	            [ch12SatId] [nvarchar](50),
	            [ch12SatCnr] [nvarchar](50),
	            [idMortalityStatus] [nvarchar](50),
	            [activity] [nvarchar](50),
	            [mainVoltage] [nvarchar](50),
	            [backupVoltage] [nvarchar](50),
	            [temperature] [nvarchar](50),
	            [transformedX] [nvarchar](50),
	            [transformedY] [nvarchar](50)")]
        public static IEnumerable ParseFormatO(SqlInt32 fileId)
        {
            return GetJsonRecords(fileId, 'O');
        }


        //   P - Vectronic activity data downloaded via the HTTP Wildlife API v2

        [SqlFunction(
            DataAccess = DataAccessKind.Read,
            FillRowMethodName = "FormatP_FillRow",
            TableDefinition =
                @"[LineNumber] [int],
	            [idMortality] [nvarchar](50),
	            [idCollar] [nvarchar](50),
	            [acquisitionTime] [nvarchar](50),
	            [scts] [nvarchar](50),
	            [originCode] [nvarchar](50),
	            [activityModeCode] [nvarchar](50),
	            [activityModeDt] [nvarchar](50),
	            [activity1] [nvarchar](50),
	            [activity2] [nvarchar](50),
	            [temperature] [nvarchar](50),
	            [activity3] [nvarchar](50)")]
        public static IEnumerable ParseFormatP(SqlInt32 fileId)
        {
            return GetJsonRecords(fileId, 'P');
        }


        //   Q - Vectronic mortality data downloaded via the HTTP Wildlife API v2

        [SqlFunction(
            DataAccess = DataAccessKind.Read,
            FillRowMethodName = "FormatQ_FillRow",
            TableDefinition =
                @"[LineNumber] [int],
                [idMortality] [nvarchar](50),
                [idCollar] [nvarchar](50),
                [acquisitionTime] [nvarchar](50),
                [scts] [nvarchar](50),
                [originCode] [nvarchar](50),
                [idKind] [nvarchar](50)")]
        public static IEnumerable ParseFormatQ(SqlInt32 fileId)
        {
            return GetJsonRecords(fileId, 'Q');
        }

        #endregion

        #region FillRow Functions

        // O - Vectronic location data downloaded via the HTTP Wildlife API v2
        public static void FormatO_FillRow(
            object inputObject,
            out SqlInt32 lineNumber,
            out SqlString idPosition,
            out SqlString idCollar,
            out SqlString acquisitionTime,
            out SqlString scts,
            out SqlString originCode,
            out SqlString ecefX,
            out SqlString ecefY,
            out SqlString ecefZ,
            out SqlString latitude,
            out SqlString longitude,
            out SqlString height,
            out SqlString dop,
            out SqlString idFixType,
            out SqlString positionError,
            out SqlString satCount,
            out SqlString ch01SatId,
            out SqlString ch01SatCnr,
            out SqlString ch02SatId,
            out SqlString ch02SatCnr,
            out SqlString ch03SatId,
            out SqlString ch03SatCnr,
            out SqlString ch04SatId,
            out SqlString ch04SatCnr,
            out SqlString ch05SatId,
            out SqlString ch05SatCnr,
            out SqlString ch06SatId,
            out SqlString ch06SatCnr,
            out SqlString ch07SatId,
            out SqlString ch07SatCnr,
            out SqlString ch08SatId,
            out SqlString ch08SatCnr,
            out SqlString ch09SatId,
            out SqlString ch09SatCnr,
            out SqlString ch10SatId,
            out SqlString ch10SatCnr,
            out SqlString ch11SatId,
            out SqlString ch11SatCnr,
            out SqlString ch12SatId,
            out SqlString ch12SatCnr,
            out SqlString idMortalityStatus,
            out SqlString activity,
            out SqlString mainVoltage,
            out SqlString backupVoltage,
            out SqlString temperature,
            out SqlString transformedX,
            out SqlString transformedY)
        {
            var record = (Dictionary<String, String>)inputObject;
            lineNumber = Int32.TryParse(record.GetValueOrDefault("lineNumber"), out int num) ? num : 0;
            idPosition = record.GetValueOrDefault("idPosition");
            idCollar = record.GetValueOrDefault("idCollar");
            acquisitionTime = record.GetValueOrDefault("acquisitionTime");
            scts = record.GetValueOrDefault("scts");
            originCode = record.GetValueOrDefault("originCode");
            ecefX = record.GetValueOrDefault("ecefX");
            ecefY = record.GetValueOrDefault("ecefY");
            ecefZ = record.GetValueOrDefault("ecefZ");
            latitude = record.GetValueOrDefault("latitude");
            longitude = record.GetValueOrDefault("longitude");
            height = record.GetValueOrDefault("height");
            dop = record.GetValueOrDefault("dop");
            idFixType = record.GetValueOrDefault("idFixType");
            positionError = record.GetValueOrDefault("positionError");
            satCount = record.GetValueOrDefault("satCount");
            ch01SatId = record.GetValueOrDefault("ch01SatId");
            ch01SatCnr = record.GetValueOrDefault("ch01SatCnr");
            ch02SatId = record.GetValueOrDefault("ch02SatId");
            ch02SatCnr = record.GetValueOrDefault("ch02SatCnr");
            ch03SatId = record.GetValueOrDefault("ch03SatId");
            ch03SatCnr = record.GetValueOrDefault("ch03SatCnr");
            ch04SatId = record.GetValueOrDefault("ch04SatId");
            ch04SatCnr = record.GetValueOrDefault("ch04SatCnr");
            ch05SatId = record.GetValueOrDefault("ch05SatId");
            ch05SatCnr = record.GetValueOrDefault("ch05SatCnr");
            ch06SatId = record.GetValueOrDefault("ch06SatId");
            ch06SatCnr = record.GetValueOrDefault("ch06SatCnr");
            ch07SatId = record.GetValueOrDefault("ch07SatId");
            ch07SatCnr = record.GetValueOrDefault("ch07SatCnr");
            ch08SatId = record.GetValueOrDefault("ch08SatId");
            ch08SatCnr = record.GetValueOrDefault("ch08SatCnr");
            ch09SatId = record.GetValueOrDefault("ch09SatId");
            ch09SatCnr = record.GetValueOrDefault("ch09SatCnr");
            ch10SatId = record.GetValueOrDefault("ch10SatId");
            ch10SatCnr = record.GetValueOrDefault("ch10SatCnr");
            ch11SatId = record.GetValueOrDefault("ch11SatId");
            ch11SatCnr = record.GetValueOrDefault("ch11SatCnr");
            ch12SatId = record.GetValueOrDefault("ch12SatId");
            ch12SatCnr = record.GetValueOrDefault("ch12SatCnr");
            idMortalityStatus = record.GetValueOrDefault("idMortalityStatus");
            activity = record.GetValueOrDefault("activity");
            mainVoltage = record.GetValueOrDefault("mainVoltage");
            backupVoltage = record.GetValueOrDefault("backupVoltage");
            temperature = record.GetValueOrDefault("temperature");
            transformedX = record.GetValueOrDefault("transformedX");
            transformedY = record.GetValueOrDefault("transformedY");
        }


        //   P - Vectronic activity data downloaded via the HTTP Wildlife API v2
        public static void FormatP_FillRow(
            object inputObject,
            out SqlInt32 lineNumber,
            out SqlString idActivity,
            out SqlString idCollar,
            out SqlString acquisitionTime,
            out SqlString scts,
            out SqlString originCode,
            out SqlString activityModeCode,
            out SqlString activityModeDt,
            out SqlString activity1,
            out SqlString activity2,
            out SqlString temperature,
            out SqlString activity3)
        {
            var record = (Dictionary<String, String>)inputObject;
            lineNumber = Int32.TryParse(record.GetValueOrDefault("lineNumber"), out int num) ? num : 0;
            idActivity = record.GetValueOrDefault("idActivity");
            idCollar = record.GetValueOrDefault("idCollar");
            acquisitionTime = record.GetValueOrDefault("acquisitionTime");
            scts = record.GetValueOrDefault("scts");
            originCode = record.GetValueOrDefault("originCode");
            activityModeCode = record.GetValueOrDefault("activityModeCode");
            activityModeDt = record.GetValueOrDefault("activityModeDt");
            activity1 = record.GetValueOrDefault("activity1");
            activity2 = record.GetValueOrDefault("activity2");
            temperature = record.GetValueOrDefault("temperature");
            activity3 = record.GetValueOrDefault("activity3");
        }

        //   Q - Vectronic mortality data downloaded via the HTTP Wildlife API v2
        public static void FormatQ_FillRow(
            object inputObject,
            out SqlInt32 lineNumber,
            out SqlString idMortality,
            out SqlString idCollar,
            out SqlString acquisitionTime,
            out SqlString scts,
            out SqlString originCode,
            out SqlString idKind)
        {
            var record = (Dictionary<String,String>)inputObject;
            lineNumber = Int32.TryParse(record.GetValueOrDefault("lineNumber"), out int num) ? num : 0;
            idMortality = record.GetValueOrDefault("idMortality");
            idCollar = record.GetValueOrDefault("idCollar");
            acquisitionTime = record.GetValueOrDefault("acquisitionTime");
            scts = record.GetValueOrDefault("scts");
            originCode = record.GetValueOrDefault("originCode");
            idKind = record.GetValueOrDefault("idKind");
        }

        #endregion

        private static IEnumerable GetJsonRecords(SqlInt32 fileId, char format)
        {
            Byte[] bytes = Parsers.GetFileContents("CollarFiles", fileId, format);
            var data = new List<IDictionary<String, String>>();
            using (var memoryStream = new MemoryStream(bytes))
            using (var reader = new StreamReader(memoryStream))
            {
                string jsonText = reader.ReadToEnd();
                data = SimpleJsonObjectListParser.Parse(jsonText);
                SimpleJsonObjectListParser.AddLineNumbers(data);
            }
            return data;
        }

    }


    // Extension to IDictionary return Default on key not found
    public static class MyExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
            => dictionary.TryGetValue(key, out var ret) ? ret : default;
    }


    class SimpleJsonObjectListParser
    {
        internal static List<IDictionary<String, String>> Parse(String text)
        {
            /*
             * This simple JSON parser assumes that the input is a single list of simple JSON objects.
             * Each object can only contains simple values (string, number, null, boolean)
             * String objects are simple (i.e no unicode, no internal quotes, no delimiters)
             * It returns all values, except null as strings without quotes
             * 
             * I am using this simple solution instead of a proper JSON processor, because:
             * 1) It works or the simple case ahead of me, and there is no reason to supect that it will not work for the files I am expecting
             * 2) The current vesion of SQL server does not support .Net frameworks that provide a JSON parser
             * 3) Third party and first party (via NuGet) parsers are available, but they come with several dependencies,
             *    and the number of assemblies that would need to be loaded into SQL Server makes this appear problematic.
             * 4) I will soon be upgrading to SQL Server 16+ which supports JSON columns, so all this could be handled in the DB.
             */

        var result = new List<IDictionary<String, String>>();
            if (String.IsNullOrEmpty(text))
            {
                return result;
            }
            var objectSeparators = new string[] { "},{\"" };
            var propertySeparators = new string[] { ",\"" };
            var keyValueSeparators = new string[] { "\":" };
            var compactText = text.Replace(" ", "").Replace("\n", "").Replace("[{\"", "").Replace("}]", "");
            foreach (var row in compactText.Split(objectSeparators, StringSplitOptions.None))
            {
                var record = new Dictionary<String, String>();
                foreach (var pair in row.Split(propertySeparators, StringSplitOptions.None))
                {
                    var keyValue = pair.Split(keyValueSeparators, StringSplitOptions.None);
                    if (keyValue.Count() == 2)
                    {
                        var key = keyValue[0];
                        var value = keyValue[1];
                        value = value.Replace("\"", "");
                        if (String.IsNullOrWhiteSpace(value) || value == "null")
                        {
                            value = null;
                        }
                        record.Add(key, value);
                    }
                }
                result.Add(record);
            }
            return result;
        }

        internal static void AddLineNumbers(List<IDictionary<String, String>> data)
        {
            var lineNumber = 1;
            foreach (var record in data)
            {
                record["lineNumber"] = lineNumber.ToString();
                lineNumber += 1;
            }
        }
    }
}
