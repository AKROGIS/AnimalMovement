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

// See http://msdn.microsoft.com/en-us/library/ms131103.aspx
// for more information on creating CLR Table-Valued Functions

namespace SqlServer_Parsers
{
    //This attribute allows us to write to the static field in a CLR plugin for SQL Server
    // "Storing to a static field is not allowed in safe assemblies" otherwise
    [CompilerGenerated]
    public class Parsers
    {
        private static int _formatCHeaderLines = 23;

        #region SQL Server Table Value Functions
        //Code - Format
        //   A - Telonics Gen3 Store On Board Format
        //   B - Ed Debevek's Child File Format - for a single deployment of a platform
        //   C - Telonics Gen4 File Format (from Argos or direct download)
        //   D - Telonics Gen3 File Format
        //   E - Telonics Email File Format (parsed with the SqlServer_Files project)
        //   F - Argos WebService CSV File Format (aws)
        //   G - Ed Debevek's Parent File Format, not parsed (children are parsed as 'B')
        //   H - Telonics Gen4 Store On Board, not parsed (children are parsed as 'C')
        //   I - Iridium Email Download Format; (contains info needed to select parameter file for processing)
        //   M - Televilt Format
        //   N - Lotek/Iridium GPS Location data downloaded as CSV from https://webservice.lotek.com  (No Parser)
        //   O-Q - See VectronicParsers.cs


        // A - Telonics Store On Board Format

        [SqlFunction(
            DataAccess = DataAccessKind.Read,
            FillRowMethodName = "FormatA_FillRow",
            TableDefinition =
                @"[LineNumber] [int],
	            [Fix #] [nvarchar](50),
	            [Date] [nvarchar](50),
	            [Time] [nvarchar](50),
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
	            [Satellite Data] [nvarchar](150)")]
        public static IEnumerable ParseFormatA(SqlInt32 fileId)
        {
            return GetLines(fileId, 'A', Default_LineSelector, null);
        }


        // B - Ed Debevek's File Format

        [SqlFunction(
            DataAccess = DataAccessKind.Read,
            FillRowMethodName = "FormatB_FillRow",
            TableDefinition =
                @"[LineNumber] [int],
	                [PlatformId] [nvarchar](255) NULL,
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
            return GetLines(fileId, 'B', Default_LineSelector, FormatB_ColumnSelector);
        }


        // C - Telonics Gen4 File Format

        [SqlFunction(
            DataAccess = DataAccessKind.Read,
            FillRowMethodName = "FormatC_FillRow",
            TableDefinition =
                @"[LineNumber] [int],
	                [AcquisitionTime] [nvarchar](50) NULL,
	                [AcquisitionStartTime] [nvarchar](50) NULL,
	                [Ctn] [nvarchar](50) NULL,
	                [ArgosId] [nvarchar](50) NULL,
	                [ArgosLocationClass] [nvarchar](50) NULL,
	                [ArgosLatitude] [nvarchar](50) NULL,
	                [ArgosLongitude] [nvarchar](50) NULL,
	                [ArgosAltitude] [nvarchar](50) NULL,
	                [GpsFixTime] [nvarchar](50) NULL,
	                [GpsFixAttempt] [nvarchar](50) NULL,
	                [GpsLatitude] [nvarchar](50) NULL,
	                [GpsLongitude] [nvarchar](50) NULL,
	                [GpsUtmZone] [nvarchar](50) NULL,
	                [GpsUtmNorthing] [nvarchar](50) NULL,
	                [GpsUtmEasting] [nvarchar](50) NULL,
	                [GpsAltitude] [nvarchar](50) NULL,
	                [GpsSpeed] [nvarchar](50) NULL,
	                [GpsHeading] [nvarchar](50) NULL,
	                [GpsHorizontalError] [nvarchar](50) NULL,
	                [GpsPositionalDilution] [nvarchar](50) NULL,
	                [GpsHorizontalDilution] [nvarchar](50) NULL,
	                [GpsSatelliteBitmap] [nvarchar](50) NULL,
	                [GpsSatelliteCount] [nvarchar](50) NULL,
	                [GpsNavigationTime] [nvarchar](50) NULL,
	                [UnderwaterPercentage] [nvarchar](50) NULL,
	                [DiveCount] [nvarchar](50) NULL,
	                [AverageDiveDuration] [nvarchar](50) NULL,
	                [MaximumDiveDuration] [nvarchar](50) NULL,
	                [LayerPercentage] [nvarchar](50) NULL,
	                [MaximumDiveDepth] [nvarchar](50) NULL,
	                [DiveStartTime] [nvarchar](50) NULL,
	                [DiveDuration] [nvarchar](50) NULL,
	                [DiveDepth] [nvarchar](50) NULL,
	                [DiveProfile] [nvarchar](50) NULL,
	                [ActivityCount] [nvarchar](50) NULL,
	                [Temperature] [nvarchar](50) NULL,
	                [RemoteAnalog] [nvarchar](50) NULL,
	                [SatelliteUplink] [nvarchar](50) NULL,
	                [ReceiveTime] [nvarchar](50) NULL,
	                [SatelliteName] [nvarchar](50) NULL,
	                [RepetitionCount] [nvarchar](50) NULL,
	                [LowVoltage] [nvarchar](50) NULL,
	                [Mortality] [nvarchar](50) NULL,
	                [SaltwaterFailsafe] [nvarchar](50) NULL,
	                [HaulOut] [nvarchar](50) NULL,
	                [DigitalInput] [nvarchar](50) NULL,
	                [MotionDetected] [nvarchar](50) NULL,
	                [TrapTriggerTime] [nvarchar](50) NULL,
	                [ReleaseTime] [nvarchar](50) NULL,
	                [PredeploymentData] [nvarchar](50) NULL,
	                [Error] [nvarchar](250) NULL")]
        public static IEnumerable ParseFormatC(SqlInt32 fileId)
        {
            return GetLines(fileId, 'C', FormatC_LineSelector, FormatC_ColumnSelector);
        }


        // D - Telonics Gen3 File Format

        [SqlFunction(
            DataAccess = DataAccessKind.Read,
            FillRowMethodName = "FormatD_FillRow",
            TableDefinition =
                @"[LineNumber] [int],
	                [TXDate] [nvarchar](50) NULL,
	                [TXTime] [nvarchar](50) NULL,
	                [PTTID] [nvarchar](50) NULL,
	                [FixNum] [nvarchar](50) NULL,
	                [FixQual] [nvarchar](50) NULL,
	                [FixDate] [nvarchar](50) NULL,
	                [FixTime] [nvarchar](50) NULL,
	                [Longitude] [nvarchar](50) NULL,
	                [Latitude] [nvarchar](50) NULL")]
        public static IEnumerable ParseFormatD(SqlInt32 fileId)
        {
            return GetLines(fileId, 'D', FormatD_LineSelector, null);
        }


        // F - Argos WebService CSV File Format (aws)

        [SqlFunction(
    DataAccess = DataAccessKind.Read,
    FillRowMethodName = "FormatF_FillRow",
    TableDefinition =
        @"[LineNumber] [int],
            [programNumber] [nvarchar](50) NULL,
            [platformId] [nvarchar](50) NULL,
            [platformType] [nvarchar](50) NULL,
            [platformModel] [nvarchar](50) NULL,
            [platformName] [nvarchar](50) NULL,
            [platformHexId] [nvarchar](50) NULL,
            [satellite] [nvarchar](50) NULL,
            [bestMsgDate] [nvarchar](50) NULL,
            [duration] [nvarchar](50) NULL,
            [nbMessage] [nvarchar](50) NULL,
            [message120] [nvarchar](50) NULL,
            [bestLevel] [nvarchar](50) NULL,
            [frequency] [nvarchar](50) NULL,
            [locationDate] [nvarchar](50) NULL,
            [latitude] [nvarchar](50) NULL,
            [longitude] [nvarchar](50) NULL,
            [altitude] [nvarchar](50) NULL,
            [locationClass] [nvarchar](50) NULL,
            [gpsSpeed] [nvarchar](50) NULL,
            [gpsHeading] [nvarchar](50) NULL,
            [latitude2] [nvarchar](50) NULL,
            [longitude2] [nvarchar](50) NULL,
            [altitude2] [nvarchar](50) NULL,
            [index] [nvarchar](50) NULL,
            [nopc] [nvarchar](50) NULL,
            [errorRadius] [nvarchar](50) NULL,
            [semiMajor] [nvarchar](50) NULL,
            [semiMinor] [nvarchar](50) NULL,
            [orientation] [nvarchar](50) NULL,
            [hdop] [nvarchar](50) NULL,
            [bestDate] [nvarchar](50) NULL,
            [compression] [nvarchar](50) NULL,
            [type] [nvarchar](50) NULL,
            [alarm] [nvarchar](50) NULL,
            [concatenated] [nvarchar](50) NULL,
            [date] [nvarchar](50) NULL,
            [level] [nvarchar](50) NULL,
            [doppler] [nvarchar](50) NULL,
            [rawData] [nvarchar](500) NULL")]
        public static IEnumerable ParseFormatF(SqlInt32 fileId)
        {
            return GetLines(fileId, 'F', FormatF_LineSelector, null);
        }

        // I - Iridium Email Download Format

        [SqlFunction(
            DataAccess = DataAccessKind.Read,
            FillRowMethodName = "FormatI_FillRow",
            TableDefinition =
                @"[LineNumber] [int],
	                [EmailAddress] [varchar](500) NULL,
	                [EmailUID] [varchar](50) NULL,
	                [Imei] [varchar](50) NULL,
	                [MessageTime] [varchar](50) NULL,
	                [StatusCode] [varchar](50) NULL,
	                [StatusString] [varchar](50) NULL,
	                [Latitude] [varchar](50) NULL,
	                [Longitude] [varchar](50) NULL,
	                [CEPRadius] [varchar](50) NULL,
	                [MessageLength] [varchar](50) NULL,
	                [MessageBytes] [varchar](4000) NULL")]
        public static IEnumerable ParseFormatI(SqlInt32 fileId)
        {
            return GetLines(fileId, 'I', Default_LineSelector, null);
        }

        // M - Televilt Format

        [SqlFunction(
            DataAccess = DataAccessKind.Read,
            FillRowMethodName = "FormatM_FillRow",
            TableDefinition =
                @"[LineNumber] [int],
	            [FixDate] [nvarchar](50),
	            [FixTime] [nvarchar](50),
	            [Lat] [nvarchar](50),
	            [Lon] [nvarchar](50),
	            [PDOP] [nvarchar](50),
	            [Fix] [nvarchar](50)")]
        public static IEnumerable ParseFormatM(SqlInt32 fileId)
        {
            return GetLines(fileId, 'M', Default_LineSelector, null);
        }

        #endregion

        private struct Line
        {
            internal readonly SqlInt32 LineNumber;
            internal readonly string LineText;
            internal readonly UInt64 ColumnMask;
            public Line(SqlInt32 lineNumber, string lineText, UInt64 mask)
            {
                LineNumber = lineNumber;
                LineText = lineText;
                ColumnMask = mask;
            }
        }

        private static IEnumerable GetLines(SqlInt32 fileId, char format, Func<int, string, bool> lineSelector, Func<StreamReader, UInt64> columnSelector)
        {
            Byte[] bytes = GetFileContents("CollarFiles", fileId, format);
            var resultCollection = new ArrayList();
            UInt64 columnMask = UInt64.MaxValue; //Select all
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
                    {
                        resultCollection.Add(new Line(lineNumber, line, columnMask));
                    }

                    lineNumber++;
                }
            }
            return resultCollection;
        }

        internal static Byte[] GetFileContents(string table, SqlInt32 fileId, char format)
        {
            Byte[] bytes = null;

            using (var connection = new SqlConnection("context connection=true"))
            {
                connection.Open();

                string sql = "SELECT [Contents] FROM [dbo].[" + table + "] WHERE [FileId] = @fileId AND [Format] = @format";
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add(new SqlParameter("@fileId", SqlDbType.Int) { Value = fileId });
                    command.Parameters.Add(new SqlParameter("@format", SqlDbType.Char) { Value = format });

                    using (SqlDataReader results = command.ExecuteReader())
                    {
                        while (results.Read())
                        {
                            bytes = results.GetSqlBytes(0).Buffer;
                        }
                    }
                }
            }
            return bytes;
        }

        #region Column Selectors

        internal static UInt64 FormatB_ColumnSelector(StreamReader stream)
        {
            UInt64 mask = UInt64.MinValue;
            if (stream == null)
            {
                return mask;
            }

            string header = stream.ReadLine();
            if (header == null)
            {
                return mask;
            }

            string[] columns = header.Split(new[] { '\t', ',' }, 14);
            // Column 14+ is not well known and is for any and all other data that may be included
            // See Section 2.5 of Argos/Telonics Online Data Conversion, April 2010, Edward M. Debevec
            var wellKnownColumns = new[]
                                       {
                                           "PlatformId", "varies", "Species", "varies", "Park", "FixDate", "FixTime", "FixMonth", "FixDay",
                                           "FixYear", "LatWGS84", "LonWGS84", "Temperature"
                                       };
            var variableColumns = new Dictionary<int, List<string>>(2)
            {
                [1] = new List<string> { "MooseNo", "WolfNo", "CaribouNo", "MuskoxNo" },
                [3] = new List<string> { "Location", "Pack" }
            };
            int fileColumn = 0;
            for (int i = 0; i < wellKnownColumns.Length; i++)
            {
                if (columns.Length <= fileColumn)
                {
                    break;
                }

                if (columns[fileColumn] != wellKnownColumns[i] &&
                    (wellKnownColumns[i] != "varies" ||
                     !variableColumns[i].Contains(columns[fileColumn])))
                {
                    continue;
                }

                mask |= 1u << i;
                fileColumn++;
            }
            return mask;
        }

        internal static UInt64 FormatC_ColumnSelector(StreamReader stream)
        {
            UInt64 mask = UInt64.MinValue;
            if (stream == null)
            {
                return mask;
            }

            // Read lines from header until the last header row
            // The last header row has the column names for the data in the file
            // There is a line in the header (i.e. "Headers Row,24") that specifies how many lines are in the header
            for (int i = 0; i < _formatCHeaderLines - 1; i++)
            {
                // Look for the line that defines where the header actually is
                var s = stream.ReadLine();
                if (s != null && s.StartsWith("Headers Row", StringComparison.OrdinalIgnoreCase))
                {
                    _formatCHeaderLines = Int32.Parse(s.Split(new[] { '\t', ',' }, 3)[1]);
                }
            }
            string header = stream.ReadLine();
            if (header == null)
            {
                return mask;
            }

            string[] columns = header.Split(new[] { '\t', ',' }, 65);
            //Fix Iridium Column Header
            columns = columns.Select(x =>
            {
                if (x == "Iridium CEP Radius")
                {
                    return "Argos Location Class";
                }

                if (x == "Iridium Latitude")
                {
                    return "Argos Latitude";
                }

                if (x == "Iridium Longitude")
                {
                    return "Argos Longitude";
                }

                return x;
            }).ToArray();
            var wellKnownColumns = new[]
                                       {
                                //"Acquisition Time",
                                //our mask is only 64 bits, and there are 65 possible columns, luckily, the first is guaranteed, so we skip it.
                                "Acquisition Start Time", "Ctn",
                                "Argos ID", "Argos Location Class", "Argos Latitude","Argos Longitude", "Argos Altitude",
                                "GPS Fix Time","GPS Fix Attempt", "GPS Latitude", "GPS Longitude",
                                "GPS UTM Zone", "GPS UTM Northing", "GPS UTM Easting", "GPS Altitude", "GPS Speed", "GPS Heading",
                                "GPS Horizontal Error", "GPS Positional Dilution", "GPS Horizontal Dilution", "GPS Satellite Bitmap",
                                "GPS Satellite Count", "GPS Navigation Time",
                                "Underwater Percentage", "Dive Count", "Average Dive Duration", "Maximum Dive Duration", "Layer Percentage",
                                "Maximum Dive Depth", "Dive Start Time", "Dive Duration", "Dive Depth", "Dive Profile",
                                "Activity Count", "Temperature", "Remote Analog", "Satellite Uplink",
                                "Receive Time", "Satellite Name", "Repetition Count", "Low Voltage", "Mortality",
                                "Saltwater Failsafe", "Haul out", "Digital Input", "Motion Detected", "Trap Trigger Time", "Release Time",
                                "Start Unix Time", "Start Year", "Start Month", "Start Day", "Start Hour", "Start Minute", "Start Second",
                                "Stop Unix Time", "Stop Year", "Stop Month", "Stop Day", "Stop Hour", "Stop Minute", "Stop Second",
                                "Predeployment Data", "Error"
                                       };
            int fileColumn = 1; //Skip the first (0th) column in the header 
            for (int i = 0; i < wellKnownColumns.Length; i++)
            {
                if (columns.Length <= fileColumn)
                {
                    break;
                }
                //Assumes that all file columns are in well known columns, and in the same order
                //If that is not true, then all remaining columns are skipped
                if (columns[fileColumn] != wellKnownColumns[i])
                {
                    continue;
                }

                mask |= 1ul << i;
                fileColumn++;
            }
            return mask;
        }

        #endregion

        #region Line Selectors

        internal static bool Default_LineSelector(int lineNumber, string lineText)
        {
            if (lineNumber == 1)
            {
                return false;
            }

            if (string.IsNullOrEmpty(lineText.Replace(',', ' ').Trim()))
            {
                return false;
            }

            return true;
        }

        internal static bool FormatC_LineSelector(int lineNumber, string lineText)
        {
            if (lineNumber <= _formatCHeaderLines)
            {
                return false;
            }

            if (string.IsNullOrEmpty(lineText.Trim()))
            {
                return false;
            }

            return true;
        }

        internal static bool FormatD_LineSelector(int lineNumber, string lineText)
        {
            //The header in this format is optional 
            if (lineNumber == 1 && lineText.Trim().StartsWith("TXDate,", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (string.IsNullOrEmpty(lineText.Trim()))
            {
                return false;
            }

            return true;
        }

        internal static bool FormatF_LineSelector(int lineNumber, string lineText)
        {
            //The header in this format is optional 
            if (lineNumber == 1 && lineText.Trim().StartsWith("\"programNumber\";", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (lineText.Trim().Equals("MAX_RESPONSE_REACHED", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (string.IsNullOrEmpty(lineText.Trim()))
            {
                return false;
            }

            return true;
        }

        #endregion

        #region FillRow Functions

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
            string[] parts = line.LineText.Split(new[] { '\t', ',' }, 18);
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
            string[] parts = line.LineText.Split(new[] { '\t', ',' }, 14);
            UInt64 columnMask = line.ColumnMask;
            lineNumber = line.LineNumber;
            int dbColumn = 0;
            int fileColumn = 0;
            collarId = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            animalId = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            species = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            group = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            park = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            fixDate = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            fixTime = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            fixMonth = Include(columnMask, dbColumn++) ? NullableInt(parts[fileColumn++]) : SqlInt32.Null;
            fixDay = Include(columnMask, dbColumn++) ? NullableInt(parts[fileColumn++]) : SqlInt32.Null;
            fixYear = Include(columnMask, dbColumn++) ? NullableInt(parts[fileColumn++]) : SqlInt32.Null;
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
            out SqlString ctn,
            out SqlString argosId,
            out SqlString argosLocationClass,
            out SqlString argosLatitude,
            out SqlString argosLongitude,
            out SqlString argosAltitude,
            out SqlString gpsFixTime,
            out SqlString gpsFixAttempt,
            out SqlString gpsLatitude,
            out SqlString gpsLongitude,
            out SqlString gpsUtmZone,
            out SqlString gpsUtmNorthing,
            out SqlString gpsUtmEasting,
            out SqlString gpsAltitude,
            out SqlString gpsSpeed,
            out SqlString gpsHeading,
            out SqlString gpsHorizontalError,
            out SqlString gpsPositionalDilution,
            out SqlString gpsHorizontalDilution,
            out SqlString gpsSatelliteBitmap,
            out SqlString gpsSatelliteCount,
            out SqlString gpsNavigationTime,
            out SqlString underwaterPercentage,
            out SqlString diveCount,
            out SqlString averageDiveDuration,
            out SqlString maximumDiveDuration,
            out SqlString layerPercentage,
            out SqlString maximumDiveDepth,
            out SqlString diveStartTime,
            out SqlString diveDuration,
            out SqlString diveDepth,
            out SqlString diveProfile,
            out SqlString activityCount,
            out SqlString temperature,
            out SqlString remoteAnalog,
            out SqlString satelliteUplink,
            out SqlString receiveTime,
            out SqlString satelliteName,
            out SqlString repetitionCount,
            out SqlString lowVoltage,
            out SqlString mortality,
            out SqlString saltwaterFailsafe,
            out SqlString haulOut,
            out SqlString digitalInput,
            out SqlString motionDetected,
            out SqlString trapTriggerTime,
            out SqlString releaseTime,
            out SqlString predeploymentData,
            out SqlString error)
        {
            var line = (Line)inputObject;
            string[] parts = line.LineText.Split(new[] { '\t', ',' }, 65);
            UInt64 columnMask = line.ColumnMask;
            lineNumber = line.LineNumber;
            int dbColumn = 0;
            int fileColumn = 0;
            acquisitionTime = NullableString(parts[fileColumn++]);
            acquisitionStartTime = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            ctn = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            argosId = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            argosLocationClass = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            argosLatitude = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            argosLongitude = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            argosAltitude = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            gpsFixTime = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            gpsFixAttempt = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            gpsLatitude = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            gpsLongitude = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            gpsUtmZone = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            gpsUtmNorthing = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            gpsUtmEasting = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            gpsAltitude = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            gpsSpeed = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            gpsHeading = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            gpsHorizontalError = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            gpsPositionalDilution = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            gpsHorizontalDilution = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            gpsSatelliteBitmap = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            gpsSatelliteCount = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            gpsNavigationTime = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            underwaterPercentage = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            diveCount = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            averageDiveDuration = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            maximumDiveDuration = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            layerPercentage = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            maximumDiveDepth = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            diveStartTime = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            diveDuration = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            diveDepth = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            diveProfile = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            activityCount = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            temperature = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            remoteAnalog = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            satelliteUplink = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            receiveTime = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            satelliteName = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            repetitionCount = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            lowVoltage = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            mortality = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            saltwaterFailsafe = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            haulOut = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            digitalInput = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            motionDetected = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            trapTriggerTime = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            releaseTime = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            for (int i = 0; i < 14; i++)
            {
                if (Include(columnMask, dbColumn++))
                {
                    fileColumn++;
                }
            }
            /*
startUnixTime = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
startYear = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
startMonth = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
startDay = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
startHour = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
startMinute = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
startSecond = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
stopUnixTime = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
stopYear = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
stopMonth = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
stopDay = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
stopHour = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
stopMinute = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
stopSecond = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
*/
            predeploymentData = Include(columnMask, dbColumn++) ? NullableString(parts[fileColumn++]) : SqlString.Null;
            error = Include(columnMask, dbColumn) ? NullableString(parts[fileColumn]) : SqlString.Null;
        }


        public static void FormatD_FillRow(
            object inputObject,
            out SqlInt32 lineNumber,
            out SqlString txDate,
            out SqlString txTime,
            out SqlString pttid,
            out SqlString fixNum,
            out SqlString fixQual,
            out SqlString fixDate,
            out SqlString fixTime,
            out SqlString longitude,
            out SqlString latitude)
        {
            var line = (Line)inputObject;
            string[] parts = line.LineText.Split('\t', ',');
            lineNumber = line.LineNumber;
            int fileColumn = 0;
            txDate = NullableString(parts[fileColumn++]);
            txTime = NullableString(parts[fileColumn++]);
            pttid = NullableString(parts[fileColumn++]);
            fixNum = NullableString(parts[fileColumn++]);
            fixQual = NullableString(parts[fileColumn++]);
            fixDate = NullableString(parts[fileColumn++]);
            fixTime = NullableString(parts[fileColumn++]);
            // Lat/Long are either signed, or have alpha (N,S,E,W) suffix - user options
            // Convert to signed, so database can assume they are a number.
            string lon = parts[fileColumn++];
            string lat = parts[fileColumn];
            if (String.IsNullOrEmpty(lon))
            {
                longitude = SqlString.Null;
            }
            else
            {
                switch (lon.Last())
                {
                    case 'W':
                        longitude = "-" + lon.Substring(0, lon.Length - 1);
                        break;
                    case 'E':
                        longitude = lon.Substring(0, lon.Length - 1);
                        break;
                    default:
                        longitude = lon;
                        break;
                }
            }
            if (String.IsNullOrEmpty(lat))
            {
                latitude = SqlString.Null;
            }
            else
            {
                switch (lat.Last())
                {
                    case 'S':
                        latitude = "-" + lat.Substring(0, lat.Length - 1);
                        break;
                    case 'N':
                        latitude = lat.Substring(0, lat.Length - 1);
                        break;
                    default:
                        latitude = lat;
                        break;
                }
            }
        }


        public static void FormatF_FillRow(
            object inputObject,
            out SqlInt32 lineNumber,
            out SqlString programNumber,
            out SqlString platformId,
            out SqlString platformType,
            out SqlString platformModel,
            out SqlString platformName,
            out SqlString platformHexId,
            out SqlString satellite,
            out SqlString bestMsgDate,
            out SqlString duration,
            out SqlString nbMessage,
            out SqlString message120,
            out SqlString bestLevel,
            out SqlString frequency,
            out SqlString locationDate,
            out SqlString latitude,
            out SqlString longitude,
            out SqlString altitude,
            out SqlString locationClass,
            out SqlString gpsSpeed,
            out SqlString gpsHeading,
            out SqlString latitude2,
            out SqlString longitude2,
            out SqlString altitude2,
            out SqlString index,
            out SqlString nopc,
            out SqlString errorRadius,
            out SqlString semiMajor,
            out SqlString semiMinor,
            out SqlString orientation,
            out SqlString hdop,
            out SqlString bestDate,
            out SqlString compression,
            out SqlString type,
            out SqlString alarm,
            out SqlString concatenated,
            out SqlString date,
            out SqlString level,
            out SqlString doppler,
            out SqlString rawData)
        {
            //format: "aaa";"bbb";"ccc";
            var line = (Line)inputObject;
            string[] parts = line.LineText.Split(new[] { "\";\"" }, StringSplitOptions.None);
            lineNumber = line.LineNumber;
            int column = 0;
            programNumber = NullableString(parts[column++].TrimStart('"'));
            platformId = NullableString(parts[column++]);
            platformType = NullableString(parts[column++]);
            platformModel = NullableString(parts[column++]);
            platformName = NullableString(parts[column++]);
            platformHexId = NullableString(parts[column++]);
            satellite = NullableString(parts[column++]);
            bestMsgDate = NullableString(parts[column++]);
            duration = NullableString(parts[column++]);
            nbMessage = NullableString(parts[column++]);
            message120 = NullableString(parts[column++]);
            bestLevel = NullableString(parts[column++]);
            frequency = NullableString(parts[column++]);
            locationDate = NullableString(parts[column++]);
            latitude = NullableString(parts[column++]);
            longitude = NullableString(parts[column++]);
            altitude = NullableString(parts[column++]);
            locationClass = NullableString(parts[column++]);
            gpsSpeed = NullableString(parts[column++]);
            gpsHeading = NullableString(parts[column++]);
            latitude2 = NullableString(parts[column++]);
            longitude2 = NullableString(parts[column++]);
            altitude2 = NullableString(parts[column++]);
            index = NullableString(parts[column++]);
            nopc = NullableString(parts[column++]);
            errorRadius = NullableString(parts[column++]);
            semiMajor = NullableString(parts[column++]);
            semiMinor = NullableString(parts[column++]);
            orientation = NullableString(parts[column++]);
            hdop = NullableString(parts[column++]);
            bestDate = NullableString(parts[column++]);
            compression = NullableString(parts[column++]);
            type = NullableString(parts[column++]);
            alarm = NullableString(parts[column++]);
            concatenated = NullableString(parts[column++]);
            date = NullableString(parts[column++]);
            level = NullableString(parts[column++]);
            doppler = NullableString(parts[column++]);
            rawData = NullableString(parts[column].TrimEnd(';').TrimEnd('"'));
        }


        public static void FormatI_FillRow(
            object inputObject,
            out SqlInt32 lineNumber,
            out SqlString emailAddress,
            out SqlString emailUid,
            out SqlString imei,
            out SqlString messageTime,
            out SqlString statusCode,
            out SqlString statusText,
            out SqlString latitude,
            out SqlString longitude,
            out SqlString cepRadius,
            out SqlString messageLength,
            out SqlString messageBytes)
        {
            var line = (Line)inputObject;
            string[] parts = line.LineText.Split(',');
            lineNumber = line.LineNumber;
            emailAddress = parts[0];
            emailUid = parts[1];
            imei = parts[2];
            messageTime = parts[3];
            statusCode = parts[4];
            statusText = parts[5];
            latitude = parts[6];
            longitude = parts[7];
            cepRadius = parts[8];
            messageLength = parts[9];
            messageBytes = parts[10];
        }


        public static void FormatM_FillRow(
            object inputObject,
            out SqlInt32 lineNumber,
            out SqlString date,
            out SqlString time,
            out SqlString lat,
            out SqlString lon,
            out SqlString pdop,
            out SqlString fix)
        {
            var line = (Line)inputObject;
            string[] parts = line.LineText.Split(',');
            lineNumber = line.LineNumber;
            date = parts[0];
            time = parts[1];
            lat = parts[2];
            lon = parts[3];
            pdop = parts[4];
            fix = parts[5];
        }


        #endregion

        private static SqlString NullableString(string s)
        {
            var t = s.Trim();
            return String.IsNullOrEmpty(t) ? SqlString.Null : t;
        }

        internal static SqlInt32 NullableInt(string s)
        {
            return String.IsNullOrEmpty(s.Trim()) ? SqlInt32.Null : Int32.Parse(s);
        }

        internal static SqlDouble NullableDouble(string s)
        {
            return String.IsNullOrEmpty(s.Trim()) ? SqlDouble.Null : Double.Parse(s);
        }

        internal static bool Include(UInt64 mask, int column)
        {
            return (mask & (1ul << column)) == 1ul << column;
        }

    };
}

