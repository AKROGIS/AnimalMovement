USE [Animal_Movement]
GO

DROP FUNCTION [dbo].[ParseFormatA]
DROP FUNCTION [dbo].[ParseFormatB]
DROP FUNCTION [dbo].[ParseFormatC]
DROP FUNCTION [dbo].[ParseFormatD]
DROP FUNCTION [dbo].[ParseFormatF]
DROP FUNCTION [dbo].[ParseFormatI]
DROP FUNCTION [dbo].[ParseFormatM]
GO

DROP ASSEMBLY [SqlServer_Parsers]

GO

CREATE ASSEMBLY [SqlServer_Parsers]
--FROM 'C:\Users\resarwas\Documents\GitHub\AnimalMovement\SqlServer_Parsers\bin\Release\SqlServer_Parsers.dll'
FROM 'E:\sqlsde\repldata\SqlServer_Parsers.dll'
WITH PERMISSION_SET = SAFE

GO

SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE FUNCTION [dbo].[ParseFormatA](@fileId [int])
RETURNS  TABLE (
	[LineNumber] [int] NULL,
	[Fix #] [nvarchar](50) NULL,
	[Date] [nvarchar](50) NULL,
	[Time] [nvarchar](50) NULL,
	[Fix Status] [nvarchar](50) NULL,
	[Status Text] [nvarchar](150) NULL,
	[Velocity East(m s)] [nvarchar](50) NULL,
	[Velocity North(m s)] [nvarchar](50) NULL,
	[Velocity Up(m s)] [nvarchar](50) NULL,
	[Latitude] [nvarchar](50) NULL,
	[Longitude] [nvarchar](50) NULL,
	[Altitude(m)] [nvarchar](50) NULL,
	[PDOP] [nvarchar](50) NULL,
	[HDOP] [nvarchar](50) NULL,
	[VDOP] [nvarchar](50) NULL,
	[TDOP] [nvarchar](50) NULL,
	[Temperature Sensor(deg )] [nvarchar](50) NULL,
	[Activity Sensor] [nvarchar](50) NULL,
	[Satellite Data] [nvarchar](150) NULL
) WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [SqlServer_Parsers].[SqlServer_Parsers.Parsers].[ParseFormatA]
GO


CREATE FUNCTION [dbo].[ParseFormatB](@fileId [int])
RETURNS  TABLE (
	[LineNumber] [int] NULL,
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
	[Other] [nvarchar](255) NULL
) WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [SqlServer_Parsers].[SqlServer_Parsers.Parsers].[ParseFormatB]
GO


CREATE FUNCTION [dbo].[ParseFormatC](@fileId [int])
RETURNS  TABLE (
	[LineNumber] [int] NULL,
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
	[Error] [nvarchar](250) NULL
) WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [SqlServer_Parsers].[SqlServer_Parsers.Parsers].[ParseFormatC]
GO


CREATE FUNCTION [dbo].[ParseFormatD](@fileId [int])
RETURNS  TABLE (
	[LineNumber] [int] NULL,
	[TXDate] [nvarchar](50) NULL,
	[TXTime] [nvarchar](50) NULL,
	[PTTID] [nvarchar](50) NULL,
	[FixNum] [nvarchar](50) NULL,
	[FixQual] [nvarchar](50) NULL,
	[FixDate] [nvarchar](50) NULL,
	[FixTime] [nvarchar](50) NULL,
	[Longitude] [nvarchar](50) NULL,
	[Latitude] [nvarchar](50) NULL
) WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [SqlServer_Parsers].[SqlServer_Parsers.Parsers].[ParseFormatD]
GO


CREATE FUNCTION [dbo].[ParseFormatF](@fileId [int])
RETURNS  TABLE (
	[LineNumber] [int] NULL,
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
	[rawData] [nvarchar](500) NULL
) WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [SqlServer_Parsers].[SqlServer_Parsers.Parsers].[ParseFormatF]
GO


CREATE FUNCTION [dbo].[ParseFormatI](@fileId [int])
RETURNS  TABLE (
	[LineNumber] [int] NULL,
	[EmailAddress] [nvarchar](500) NULL,
	[EmailUID] [nvarchar](50) NULL,
	[Imei] [nvarchar](50) NULL,
	[MessageTime] [nvarchar](50) NULL,
	[StatusCode] [nvarchar](50) NULL,
	[StatusString] [nvarchar](50) NULL,
	[Latitude] [nvarchar](50) NULL,
	[Longitude] [nvarchar](50) NULL,
	[CEPRadius] [nvarchar](50) NULL,
	[MessageLength] [nvarchar](50) NULL,
	[MessageBytes] [nvarchar](4000) NULL
) WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [SqlServer_Parsers].[SqlServer_Parsers.Parsers].[ParseFormatI]
GO


CREATE FUNCTION [dbo].[ParseFormatM](@fileId [int])
RETURNS  TABLE (
	[LineNumber] [int] NULL,
	[FixDate] [nvarchar](50) NULL,
	[FixTime] [nvarchar](50) NULL,
	[Lat] [nvarchar](50) NULL,
	[Lon] [nvarchar](50) NULL,
	[PDOP] [nvarchar](50) NULL,
	[Fix] [nvarchar](50) NULL
) WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [SqlServer_Parsers].[SqlServer_Parsers.Parsers].[ParseFormatM]
GO
