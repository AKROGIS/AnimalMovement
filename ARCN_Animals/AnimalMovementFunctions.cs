using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using Microsoft.SqlServer.Server;

// See http://msdn.microsoft.com/en-us/library/ms131103.aspx
// for more information on creating CLR Table-Valued Functions

public partial class AnimalMovementFunctions
{
    [SqlFunction]
    public static SqlDateTime LocalTime(SqlDateTime utcDateTime)
    {
        return new SqlDateTime(utcDateTime.Value.ToLocalTime());
    }

    [SqlFunction(
               DataAccess = DataAccessKind.Read,
        FillRowMethodName = "FormatA_FillRow",
          TableDefinition = 
              @"[LineNumber] [int],
	            [Fix #] [nvarchar](50),
	            [Date] [nchar](10),
	            [Time] [nchar](8),
	            [Fix Status] [nvarchar](50),
	            [Status Text] [nvarchar](150),
	            [Velocity East(m s)] [nvarchar](50),
	            [Velocity North(m s)] [nvarchar](50),
	            [Velocity Up(m s)] [nvarchar](50),
	            [Latitude] [nvarchar](50),
	            [Longitude] [nvarchar](50),
	            [Altitude(m)] [nvarchar](50),
	            [PDOP] [nvarchar](50),
	            [HDOP] [nvarchar](50),
	            [VDOP] [nvarchar](50),
	            [TDOP] [nvarchar](50),
	            [Temperature Sensor(deg )] [nvarchar](50),
	            [Activity Sensor] [nvarchar](50),
	            [Satellite Data] [nvarchar](150)" )]
    public static IEnumerable ParseFormatA(SqlInt32 fileId)
    {
        return GetLines(fileId, 'A', FormatA_LineSelector, null);
    }

    [SqlFunction(
               DataAccess = DataAccessKind.Read,
        FillRowMethodName = "FormatB_FillRow",
          TableDefinition =
                  @"[LineNumber] [int],
	                [CollarID] [nvarchar](255) NULL,
	                [AnimalId] [nvarchar](255) NULL,
	                [Species] [nvarchar](255) NULL,
	                [Group] [nvarchar](255) NULL,
	                [Park] [nvarchar](255) NULL,
	                [FixDate] [nvarchar](255) NULL,
	                [FixTime] [nvarchar](255) NULL,
	                [FixMonth] [int] NULL,
	                [FixDay] [int] NULL,
	                [FixYear] [int] NULL,
	                [LatWGS84] [float] NULL,
	                [LonWGS84] [float] NULL,
	                [Temperature] [float] NULL,
	                [Other] [nvarchar](255) NULL")]
    public static IEnumerable ParseFormatB(SqlInt32 fileId)
    {
        return GetLines(fileId, 'B', FormatB_LineSelector, FormatB_ColumnSelector);
    }

    [SqlFunction(
               DataAccess = DataAccessKind.Read,
        FillRowMethodName = "FormatC_FillRow",
          TableDefinition =
                  @"[LineNumber] [int],
	                [AcquisitionTime] [nvarchar](50) NULL,
	                [AcquisitionStartTime] [nvarchar](50) NULL,
	                [ArgosLocationClass] [nvarchar](50) NULL,
	                [ArgosLatitude] [nvarchar](50) NULL,
	                [ArgosLongitude] [nvarchar](50) NULL,
	                [ArgosAltitude] [nvarchar](50) NULL,
	                [GpsFixAttempt] [nvarchar](50) NULL,
	                [GpsLatitude] [nvarchar](50) NULL,
	                [GpsLongitude] [nvarchar](50) NULL,
	                [GpsUtmZone] [nvarchar](50) NULL,
	                [GpsUtmNorthing] [nvarchar](50) NULL,
	                [GpsUtmEasting] [nvarchar](50) NULL,
	                [Temperature] [nvarchar](50) NULL,
	                [SatelliteUplink] [nvarchar](50) NULL,
	                [ReceiveTime] [nvarchar](50) NULL,
	                [SatelliteName] [nvarchar](50) NULL,
	                [RepetitionCount] [nvarchar](50) NULL,
	                [LowVoltage] [nvarchar](50) NULL,
	                [Mortality] [nvarchar](50) NULL,
	                [PredeploymentData] [nvarchar](50) NULL,
	                [Error] [nvarchar](250) NULL")]
    public static IEnumerable ParseFormatC(SqlInt32 fileId)
    {
        return GetLines(fileId, 'C', FormatC_LineSelector, null);
    }

    private struct Line
    {
        internal readonly SqlInt32 LineNumber;
        internal readonly string LineText;
        internal readonly uint ColumnMask;
        public Line(SqlInt32 lineNumber, string lineText, uint mask)
        {
            LineNumber = lineNumber;
            LineText = lineText;
            ColumnMask = mask;
        }
    }

    private static IEnumerable GetLines(SqlInt32 fileId, char format, Func<int, string, bool> lineSelector, Func<StreamReader, UInt32> columnSelector)
    {
        var resultCollection = new ArrayList();

        using (var connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            const string sql = "SELECT [Contents] FROM [dbo].[CollarFiles] WHERE [FileId] = @fileId AND [Format] = @format";
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add(new SqlParameter("@fileId", SqlDbType.Int) { Value = fileId });
                command.Parameters.Add(new SqlParameter("@format", SqlDbType.Char) { Value = format });

                using (SqlDataReader results = command.ExecuteReader())
                {
                    while (results.Read())
                    {
                        Byte[] bytes = results.GetSqlBytes(0).Buffer;
                        uint columnMask = UInt32.MaxValue; //Select all
                        if (columnSelector != null)
                        {
                            using (var memoryStream = new MemoryStream(bytes))
                            using (var reader = new StreamReader(memoryStream))
                            {
                                columnMask = columnSelector(reader);
                            }
                        }
                        int lineNumber = 1;
                        using (var memoryStream = new MemoryStream(bytes))
                        using (var reader = new StreamReader(memoryStream))
                        {
                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                if (lineSelector(lineNumber, line))
                                    resultCollection.Add(new Line(lineNumber, line, columnMask));
                                lineNumber++;
                            }
                        }

                    }
                }
            }
        }

        return resultCollection;
    }

    internal static uint FormatB_ColumnSelector(StreamReader stream)
    {
        uint mask = UInt32.MinValue;
        if (stream == null)
            return mask;
        string header = stream.ReadLine();
        if (header == null)
            return mask;
        string[] columns = header.Split(new[] { '\t', ',' }, 14);
        // See Section 2.5 of Argos/Telonics Online Data Conversion, April 2010, Edward M. Debevec
        var wellKnownColumns = new[]
                                {
                                    "CollarID", "varies", "Species", "varies", "Park", "FixDate", "FixTime", "FixMonth", "FixDay",
                                    "FixYear", "LatWGS84", "LonWGS84", "Temperature"
                                };
        var variableColumns = new Dictionary<int, List<string>>(2);
        variableColumns[1] = new List<string> { "MooseNo", "WolfNo", "CaribouNo", "MuskOx" };
        variableColumns[3] = new List<string> { "Location", "Pack" };
        int fileColumn = 0;
        for (int i = 0; i < wellKnownColumns.Length; i++)
        {
            if (columns.Length <= fileColumn)
                break;
            if (columns[fileColumn] == wellKnownColumns[i] ||
                (wellKnownColumns[i] == "varies" && variableColumns[i].Contains(columns[fileColumn])))
            {
                mask = mask | 1u << i;
                fileColumn++;
            }
        }
        return mask;
    }

    internal static bool FormatA_LineSelector(int lineNumber, string lineText)
    {
        if (lineNumber == 1)
            return false;
        if (string.IsNullOrEmpty(lineText.Trim()))
            return false;
        return true;
    }

    internal static bool FormatB_LineSelector(int lineNumber, string lineText)
    {
        if (lineNumber == 1)
            return false;
        if (string.IsNullOrEmpty(lineText.Replace(',', ' ').Trim()))
            return false;
        return true;
    }

    internal static bool FormatC_LineSelector(int lineNumber, string lineText)
    {
        if (lineNumber <= 23)
            return false;
        if (string.IsNullOrEmpty(lineText.Trim()))
            return false;
        return true;
    }

    public static void FormatA_FillRow(
        object inputObject,
        out SqlInt32 lineNumber,
        out SqlString fix,
        out SqlString date,
        out SqlString time,
        out SqlString fixStatus,
        out SqlString statusText,
        out SqlString velocityE,
        out SqlString velocityN,
        out SqlString velocityUp,
        out SqlString latitude,
        out SqlString longitude,
        out SqlString altitude,
        out SqlString pdop,
        out SqlString hdop,
        out SqlString vdop,
        out SqlString tdop,
        out SqlString temperature,
        out SqlString activity,
        out SqlString data)
    {
        var line = (Line)inputObject;
        string[] parts = line.LineText.Split(new[] {'\t', ','}, 18);
        lineNumber = line.LineNumber;
        fix = parts[0];
        date = parts[1];
        time = parts[2];
        fixStatus = parts[3];
        statusText = parts[4];
        velocityE = parts[5];
        velocityN = parts[6];
        velocityUp = parts[7];
        latitude = parts[8];
        longitude = parts[9];
        altitude = parts[10];
        pdop = parts[11];
        hdop = parts[12];
        vdop = parts[13];
        tdop = parts[14];
        temperature = parts[15];
        activity = parts[16];
        data = parts[17];
    }



    public static void FormatB_FillRow(
        object inputObject,
        out SqlInt32 lineNumber,
        out SqlString collarId,
        out SqlString animalId,
        out SqlString species,
        out SqlString group,
        out SqlString park,
        out SqlString fixDate,
        out SqlString fixTime,
        out SqlInt32 fixMonth,
        out SqlInt32 fixDay,
        out SqlInt32 fixYear,
        out SqlDouble latWgs84,
        out SqlDouble lonWgs84,
        out SqlDouble temperature,
        out SqlString data)
    {
        var line = (Line)inputObject;
        string[] parts = line.LineText.Split(new[] {'\t', ','}, 14);
        uint columnMask = line.ColumnMask;
        lineNumber = line.LineNumber;
        int dbColumn = 0;
        int fileColumn = 0;
        collarId = Include(columnMask, dbColumn++) ? parts[fileColumn++] : SqlString.Null;
        animalId = Include(columnMask, dbColumn++) ? parts[fileColumn++] : SqlString.Null;
        species = Include(columnMask, dbColumn++) ? parts[fileColumn++] : SqlString.Null;
        group = Include(columnMask, dbColumn++) ? parts[fileColumn++] : SqlString.Null;
        park = Include(columnMask, dbColumn++) ? parts[fileColumn++] : SqlString.Null;
        fixDate = Include(columnMask, dbColumn++) ? parts[fileColumn++] : SqlString.Null;
        fixTime = Include(columnMask, dbColumn++) ? parts[fileColumn++] : SqlString.Null;
        fixMonth = Include(columnMask, dbColumn++) ? Int32.Parse(parts[fileColumn++]) : SqlInt32.Null;
        fixDay = Include(columnMask, dbColumn++) ? Int32.Parse(parts[fileColumn++]) : SqlInt32.Null;
        fixYear = Include(columnMask, dbColumn++) ? Int32.Parse(parts[fileColumn++]) : SqlInt32.Null;
        latWgs84 = Include(columnMask, dbColumn++) ? NullableDouble(parts[fileColumn++]) : SqlDouble.Null;
        lonWgs84 = Include(columnMask, dbColumn++) ? NullableDouble(parts[fileColumn++]) : SqlDouble.Null;
        temperature = Include(columnMask, dbColumn) ? NullableDouble(parts[fileColumn++]) : SqlDouble.Null;
        //Data is everything left over on the line
        data = parts.Length <= fileColumn ? SqlString.Null : String.Join(",", parts.Skip(fileColumn).ToArray());
    }

    public static void FormatC_FillRow(
        object inputObject,
        out SqlInt32 lineNumber,
        out SqlString acquisitionTime,
        out SqlString acquisitionStartTime,
        out SqlString argosLocationClass,
        out SqlString argosLatitude,
        out SqlString argosLongitude,
        out SqlString argosAltitude,
        out SqlString gpsFixAttempt,
        out SqlString gpsLatitude,
        out SqlString gpsLongitude,
        out SqlString gpsUtmZone,
        out SqlString gpsUtmNorthing,
        out SqlString gpsUtmEasting,
        out SqlString temperature,
        out SqlString satelliteUplink,
        out SqlString receiveTime,
        out SqlString satelliteName,
        out SqlString repetitionCount,
        out SqlString lowVoltage,
        out SqlString mortality,
        out SqlString predeploymentData,
        out SqlString error)
    {
        var line = (Line)inputObject;
        string[] parts = line.LineText.Split(new[] { '\t', ',' }, 21);
        lineNumber = line.LineNumber;
        acquisitionTime = String.IsNullOrEmpty(parts[0]) ? SqlString.Null : parts[0];
        acquisitionStartTime = String.IsNullOrEmpty(parts[1]) ? SqlString.Null : parts[1];
        argosLocationClass = String.IsNullOrEmpty(parts[2]) ? SqlString.Null : parts[2];
        argosLatitude = String.IsNullOrEmpty(parts[3]) ? SqlString.Null : parts[3];
        argosLongitude = String.IsNullOrEmpty(parts[4]) ? SqlString.Null : parts[4];
        argosAltitude = String.IsNullOrEmpty(parts[5]) ? SqlString.Null : parts[5];
        gpsFixAttempt = String.IsNullOrEmpty(parts[6]) ? SqlString.Null : parts[6];
        gpsLatitude = String.IsNullOrEmpty(parts[7]) ? SqlString.Null : parts[7];
        gpsLongitude = String.IsNullOrEmpty(parts[8]) ? SqlString.Null : parts[8];
        gpsUtmZone = String.IsNullOrEmpty(parts[9]) ? SqlString.Null : parts[9];
        gpsUtmNorthing = String.IsNullOrEmpty(parts[10]) ? SqlString.Null : parts[10];
        gpsUtmEasting = String.IsNullOrEmpty(parts[11]) ? SqlString.Null : parts[11];
        temperature = String.IsNullOrEmpty(parts[12]) ? SqlString.Null : parts[12];
        satelliteUplink = String.IsNullOrEmpty(parts[13]) ? SqlString.Null : parts[13];
        receiveTime = String.IsNullOrEmpty(parts[14]) ? SqlString.Null : parts[14];
        satelliteName = String.IsNullOrEmpty(parts[15]) ? SqlString.Null : parts[15];
        repetitionCount = String.IsNullOrEmpty(parts[16]) ? SqlString.Null : parts[16];
        lowVoltage = String.IsNullOrEmpty(parts[17]) ? SqlString.Null : parts[17];
        mortality = String.IsNullOrEmpty(parts[18]) ? SqlString.Null : parts[18];
        predeploymentData = String.IsNullOrEmpty(parts[19]) ? SqlString.Null : parts[19];
        error = String.IsNullOrEmpty(parts[20]) ? SqlString.Null : parts[20];
    }


    internal static SqlDouble NullableDouble(string s)
    {
        return String.IsNullOrEmpty(s.Trim()) ? SqlDouble.Null : Double.Parse(s);
    }

    internal static bool Include(uint mask, int column)
    {
        return (mask & (1 << column)) == (1 << column);
    }

};

