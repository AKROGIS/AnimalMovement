USE [Animal_Movement]
GO
CREATE USER [BLM\Domain Users] FOR LOGIN [BLM\Domain Users]
GO
CREATE USER [INPAKROMS53AIS\repl_distribution] WITH DEFAULT_SCHEMA=[dbo]
GO
CREATE USER [INPAKROMS53AIS\repl_logreader] WITH DEFAULT_SCHEMA=[dbo]
GO
CREATE USER [INPAKROMS53AIS\repl_merge] WITH DEFAULT_SCHEMA=[dbo]
GO
CREATE USER [INPAKROMS53AIS\repl_snapshot] WITH DEFAULT_SCHEMA=[dbo]
GO
CREATE USER [INPAKROVMAIS\sql_proxy] FOR LOGIN [INPAKROVMAIS\sql_proxy] WITH DEFAULT_SCHEMA=[dbo]
GO
CREATE USER [link] FOR LOGIN [link] WITH DEFAULT_SCHEMA=[dbo]
GO
CREATE USER [NPS\BAMangipane] FOR LOGIN [NPS\BAMangipane] WITH DEFAULT_SCHEMA=[dbo]
GO
CREATE USER [NPS\BBorg] FOR LOGIN [NPS\BBorg] WITH DEFAULT_SCHEMA=[dbo]
GO
CREATE USER [NPS\Domain Users] FOR LOGIN [NPS\Domain Users]
GO
CREATE USER [NPS\DSchertz] FOR LOGIN [NPS\DSchertz] WITH DEFAULT_SCHEMA=[dbo]
GO
CREATE USER [NPS\JAPutera] FOR LOGIN [NPS\JAPutera] WITH DEFAULT_SCHEMA=[dbo]
GO
CREATE USER [NPS\JHSchmidt] FOR LOGIN [NPS\JHSchmidt] WITH DEFAULT_SCHEMA=[dbo]
GO
CREATE USER [NPS\JPLawler] FOR LOGIN [NPS\JPLawler] WITH DEFAULT_SCHEMA=[dbo]
GO
CREATE USER [NPS\KCJoly] FOR LOGIN [NPS\KCJoly] WITH DEFAULT_SCHEMA=[dbo]
GO
CREATE USER [NPS\kklauder] FOR LOGIN [NPS\kklauder] WITH DEFAULT_SCHEMA=[dbo]
GO
CREATE USER [NPS\mdcameron] FOR LOGIN [NPS\mdcameron] WITH DEFAULT_SCHEMA=[dbo]
GO
CREATE USER [NPS\MLJohnson] WITH DEFAULT_SCHEMA=[dbo]
GO
CREATE USER [NPS\msorum] FOR LOGIN [NPS\msorum] WITH DEFAULT_SCHEMA=[dbo]
GO
CREATE USER [NPS\PAOwen] FOR LOGIN [NPS\PAOwen] WITH DEFAULT_SCHEMA=[dbo]
GO
CREATE USER [NPS\RESarwas] FOR LOGIN [NPS\RESarwas] WITH DEFAULT_SCHEMA=[dbo]
GO
CREATE USER [NPS\SDMiller] FOR LOGIN [NPS\sdmiller] WITH DEFAULT_SCHEMA=[dbo]
GO
CREATE ROLE [ArgosProcessor]
GO
CREATE ROLE [dog_house]
GO
CREATE ROLE [Editor]
GO
CREATE ROLE [Investigator]
GO
CREATE ROLE [Viewer]
GO
ALTER ROLE [Viewer] ADD MEMBER [BLM\Domain Users]
GO
ALTER ROLE [db_owner] ADD MEMBER [INPAKROMS53AIS\repl_logreader]
GO
ALTER ROLE [db_owner] ADD MEMBER [INPAKROMS53AIS\repl_snapshot]
GO
ALTER ROLE [Viewer] ADD MEMBER [INPAKROVMAIS\sql_proxy]
GO
ALTER ROLE [ArgosProcessor] ADD MEMBER [INPAKROVMAIS\sql_proxy]
GO
ALTER ROLE [Viewer] ADD MEMBER [link]
GO
ALTER ROLE [Editor] ADD MEMBER [NPS\BAMangipane]
GO
ALTER ROLE [Investigator] ADD MEMBER [NPS\BAMangipane]
GO
ALTER ROLE [Viewer] ADD MEMBER [NPS\BAMangipane]
GO
ALTER ROLE [Editor] ADD MEMBER [NPS\BBorg]
GO
ALTER ROLE [Investigator] ADD MEMBER [NPS\BBorg]
GO
ALTER ROLE [Viewer] ADD MEMBER [NPS\BBorg]
GO
ALTER ROLE [Viewer] ADD MEMBER [NPS\Domain Users]
GO
ALTER ROLE [Editor] ADD MEMBER [NPS\DSchertz]
GO
ALTER ROLE [Viewer] ADD MEMBER [NPS\DSchertz]
GO
ALTER ROLE [Editor] ADD MEMBER [NPS\JAPutera]
GO
ALTER ROLE [Investigator] ADD MEMBER [NPS\JAPutera]
GO
ALTER ROLE [Viewer] ADD MEMBER [NPS\JAPutera]
GO
ALTER ROLE [Editor] ADD MEMBER [NPS\JHSchmidt]
GO
ALTER ROLE [Viewer] ADD MEMBER [NPS\JHSchmidt]
GO
ALTER ROLE [Editor] ADD MEMBER [NPS\JPLawler]
GO
ALTER ROLE [Investigator] ADD MEMBER [NPS\JPLawler]
GO
ALTER ROLE [Viewer] ADD MEMBER [NPS\JPLawler]
GO
ALTER ROLE [Editor] ADD MEMBER [NPS\KCJoly]
GO
ALTER ROLE [Investigator] ADD MEMBER [NPS\KCJoly]
GO
ALTER ROLE [Viewer] ADD MEMBER [NPS\KCJoly]
GO
ALTER ROLE [Editor] ADD MEMBER [NPS\kklauder]
GO
ALTER ROLE [Viewer] ADD MEMBER [NPS\kklauder]
GO
ALTER ROLE [Editor] ADD MEMBER [NPS\mdcameron]
GO
ALTER ROLE [Investigator] ADD MEMBER [NPS\mdcameron]
GO
ALTER ROLE [Viewer] ADD MEMBER [NPS\mdcameron]
GO
ALTER ROLE [Editor] ADD MEMBER [NPS\MLJohnson]
GO
ALTER ROLE [Viewer] ADD MEMBER [NPS\MLJohnson]
GO
ALTER ROLE [Editor] ADD MEMBER [NPS\msorum]
GO
ALTER ROLE [Investigator] ADD MEMBER [NPS\msorum]
GO
ALTER ROLE [Viewer] ADD MEMBER [NPS\msorum]
GO
ALTER ROLE [Editor] ADD MEMBER [NPS\PAOwen]
GO
ALTER ROLE [Viewer] ADD MEMBER [NPS\PAOwen]
GO
ALTER ROLE [Editor] ADD MEMBER [NPS\RESarwas]
GO
ALTER ROLE [Investigator] ADD MEMBER [NPS\RESarwas]
GO
ALTER ROLE [Viewer] ADD MEMBER [NPS\RESarwas]
GO
ALTER ROLE [Editor] ADD MEMBER [NPS\SDMiller]
GO
ALTER ROLE [Investigator] ADD MEMBER [NPS\SDMiller]
GO
ALTER ROLE [Viewer] ADD MEMBER [NPS\SDMiller]
GO
ALTER ROLE [Viewer] ADD MEMBER [ArgosProcessor]
GO
ALTER ROLE [Viewer] ADD MEMBER [Editor]
GO
ALTER ROLE [Editor] ADD MEMBER [Investigator]
GO
GRANT ALTER ON ROLE::[Editor] TO [Investigator] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Regan Sarwas
-- Create date: May 30, 2012
-- Description:	Returns a row of fix information for a specific collar.
-- Example:     SELECT * FROM CollarFixSummary('Telonics', '96007')
-- Modified:    Aug 16, 2012, added Unique field
-- =============================================
CREATE FUNCTION [dbo].[CollarFixSummary] 
(
	@CollarManufacturer NVARCHAR(255), 
	@CollarId           NVARCHAR(255)
)
RETURNS @summary TABLE
(
	[Count] int,
	[Unique] int,
	[First] datetime2,
	[Last] datetime2
) 
AS
BEGIN
	DECLARE @temp int;
	
	SELECT @temp = 1 
	FROM [dbo].[CollarFixes]
	WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId
	GROUP BY FixDate
	
	INSERT @summary
	SELECT  COUNT(*) AS [Count],
			@@ROWCOUNT AS [Unique],
			dbo.LocalTime(MIN(FixDate)) AS [First],
			dbo.LocalTime(MAX(FixDate)) AS [Last]
	FROM [dbo].[CollarFixes]
	WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId
	GROUP BY CollarManufacturer, CollarId
	
	RETURN
END


GO
GRANT SELECT ON [dbo].[CollarFixSummary] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE FUNCTION [dbo].[DaysSinceLastDownload] 
(
)
RETURNS INT
AS
BEGIN
    DECLARE @Result INT;

	SELECT @Result = DATEDIFF(day, MAX(TimeStamp), GETDATE())
	  FROM ArgosDownloads
	 WHERE ErrorMessage IS NULL
	
	RETURN @Result
END




GO
GRANT EXECUTE ON [dbo].[DaysSinceLastDownload] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE FUNCTION [dbo].[DoDateRangesOverlap] 
(
	@StartDate1		DATETIME2 = NULL, 
	@EndDate1		DATETIME2 = NULL,
	@StartDate2		DATETIME2 = NULL, 
	@EndDate2		DATETIME2 = NULL
)
RETURNS BIT
AS
BEGIN
	-- A StartDate of NULL means the begining of time
	-- A EndDate of NULL means the ending of time
	
	-- Touching i.e. EndDate = StartDate WAS considered overlapping
	-- At that instant in time, a collar for example would be on two animals
	-- simultaneously, which violates reality and creates some ambiguity.
	-- In reality, it is highly unlikely that we will get a fix at this exact instant, and
	-- even if we did, then that fix would be applied to both animals.  No real problem.
	-- Since user's are likely to be confused if they get an overlap error when they
	-- stop and start different deployment on the same date (time = 00:00) I have
	-- relaxed this requirement:
	--      EndDate = StartDate IS NOT overlapping
	
	-- There are nine cases: (NULL2DATE, DATE2DATE, DATE2NULL)^2
	
	IF @StartDate1 IS NULL and @EndDate1 IS NOT NULL
	BEGIN
		IF @StartDate2 IS NULL and @EndDate2 IS NOT NULL
		BEGIN
			RETURN 1
		END
		IF @StartDate2 IS NOT NULL and @EndDate2 IS NOT NULL
		BEGIN
			IF @EndDate1 <= @StartDate2 RETURN 0 ELSE RETURN 1
		END	
		IF @StartDate2 IS NOT NULL and @EndDate2 IS NULL
		BEGIN
			IF @EndDate1 <= @StartDate2 RETURN 0 ELSE RETURN 1
		END	
	END
	
	IF @StartDate1 IS NOT NULL and @EndDate1 IS NOT NULL
	BEGIN
		IF @StartDate2 IS NULL and @EndDate2 IS NOT NULL
		BEGIN
			IF @EndDate2 <= @StartDate1 RETURN 0 ELSE RETURN 1
		END
		IF @StartDate2 IS NOT NULL and @EndDate2 IS NOT NULL
		BEGIN
			IF @EndDate2 <= @StartDate1 OR @EndDate1 <= @StartDate2  RETURN 0 ELSE RETURN 1
		END	
		IF @StartDate2 IS NOT NULL and @EndDate2 IS NULL
		BEGIN
			IF @EndDate1 <= @StartDate2 RETURN 0 ELSE RETURN 1
		END	
	END	

	IF @StartDate1 IS NOT NULL and @EndDate1 IS NULL
	BEGIN
		IF @StartDate2 IS NULL and @EndDate2 IS NOT NULL
		BEGIN
			IF @EndDate2 <= @StartDate1 RETURN 0 ELSE RETURN 1
		END
		IF @StartDate2 IS NOT NULL and @EndDate2 IS NOT NULL
		BEGIN
			IF @EndDate2 <= @StartDate1 RETURN 0 ELSE RETURN 1
		END	
		IF @StartDate2 IS NOT NULL and @EndDate2 IS NULL
		BEGIN
			RETURN 1
		END	
	END
	-- @StartDate1 IS NULL and @EndDate1 IS NULL, so we are guaranteed to overlap 
	RETURN 1
END


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE FUNCTION [dbo].[EndOfFollowingConnectedMovement] 
(
	@Project VARCHAR(32)   = NULL, 
	@Animal  VARCHAR(32)   = NULL, 
	@Time    DATETIME2 = NULL
)
RETURNS DATETIME2
AS
BEGIN
	DECLARE @Result DATETIME2
	SET @Result = (SELECT [EndDate] 
					 FROM [dbo].[Movements]
					WHERE [ProjectId] = @Project
					  AND [AnimalId] = @Animal
					  AND [StartDate] = @Time);
	RETURN @Result
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE FUNCTION [dbo].[EndOfMovement]
(
	@Project VARCHAR(32)   = NULL, 
	@Animal  VARCHAR(32)   = NULL, 
	@Time    DATETIME2 = NULL
)
RETURNS DATETIME2
AS
BEGIN
	DECLARE @Result DATETIME2
	SET @Result = (SELECT [EndDate] 
	                 FROM [dbo].[Movements]
					WHERE [ProjectId] = @Project
					  AND [AnimalId] = @Animal
					  AND [StartDate] = @Time);
	RETURN @Result
END

GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE FUNCTION [dbo].[FileFormat](@data [varbinary](max))
RETURNS [nchar](1) WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [SqlServer_Files].[SqlServer_Files.CollarFileInfo].[FileFormat]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Regan Sarwas
-- Create date: April 4, 2013
-- Description:	Returns a bit (boolean) if we need to use TDC.exe on this file
--              This is true for files of Gen4 datalog files (format 'H')
--              Email and AWS files ('E', 'F'), may have only Gen3 collars in them
--              Check if the file has transmissions that map to a Gen4 collar.
-- =============================================

CREATE FUNCTION [dbo].[FileHasGen4Data] 
(
	@FileId INT
)
RETURNS BIT
AS
BEGIN
	IF EXISTS(   SELECT 1
				   FROM ArgosFilePlatformDates AS D
			CROSS APPLY GetTelonicsParametersForArgosDates(D.PlatformId, D.FirstTransmission, D.LastTransmission) AS P
				  WHERE D.FileId = @FileId AND P.CollarModel = 'Gen4'
			 )
		RETURN 1
	IF EXISTS(   SELECT 1
	               FROM CollarFiles
	              WHERE FileId = @FileId AND Format = 'H'
	         )
	    RETURN 1
	RETURN 0
END

GO
GRANT EXECUTE ON [dbo].[FileHasGen4Data] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE FUNCTION [dbo].[GetLocationGeography]
(
	@Project VARCHAR(32)   = NULL, 
	@Animal  VARCHAR(32)   = NULL, 
	@Time    DATETIME2 = NULL
)
RETURNS GEOGRAPHY
AS
BEGIN
	DECLARE @Result GEOGRAPHY
	SET @Result = (SELECT [Location] 
	                 FROM [dbo].[Locations]
					WHERE [ProjectId] = @Project
					  AND [AnimalId] = @Animal
	                  AND [FixDate] = @Time);
	RETURN @Result
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE FUNCTION [dbo].[GetMovementEndGeography]
(
	@Project VARCHAR(32)   = NULL, 
	@Animal  VARCHAR(32)   = NULL, 
	@Time    DATETIME2 = NULL
)
RETURNS GEOGRAPHY
AS
BEGIN
	DECLARE @Result GEOGRAPHY
	SET @Result = (SELECT [Shape].STEndPoint() 
	                 FROM [dbo].[Movements]
					WHERE [ProjectId] = @Project
					  AND [AnimalId] = @Animal
	                  AND [EndDate] = @Time);
	RETURN @Result
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE FUNCTION [dbo].[GetMovementStartGeography]
(
	@Project VARCHAR(32)   = NULL, 
	@Animal  VARCHAR(32)   = NULL, 
	@Time    DATETIME2 = NULL
)
RETURNS GEOGRAPHY
AS
BEGIN
	DECLARE @Result GEOGRAPHY
	SET @Result = (SELECT [Shape].STStartPoint() 
	                 FROM [dbo].[Movements]
					WHERE [ProjectId] = @Project
					  AND [AnimalId] = @Animal
	                  AND [StartDate] = @Time);
	RETURN @Result
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE FUNCTION [dbo].[IsFixEditor] 
(
	@FixId    INT = NULL, 
	@User sysname = NULL
)
RETURNS BIT
AS
BEGIN
	DECLARE @Result BIT
	
		SELECT @Result = [dbo].[IsProjectEditor]([f].[ProjectId], @User)
		  FROM [dbo].[CollarFixes] AS [x]
	INNER JOIN [dbo].[CollarFiles] AS [f]
			ON [x].[FileId] = [f].[FileId]
		 WHERE [x].[FixId] = @FixId;
		 
	RETURN @Result
END






GO
GRANT EXECUTE ON [dbo].[IsFixEditor] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE FUNCTION [dbo].[IsInvestigatorEditor] 
(
    @ProjectInvestigator sysname, 
    @User                sysname
)
RETURNS BIT
AS
BEGIN
    -- Check that @User is in the Investigator or the Editor Role
    IF NOT EXISTS( SELECT 1 FROM sys.sysmembers WHERE (USER_NAME(groupuid) = 'Editor' AND USER_NAME(memberuid) = @user)
                                                   OR (USER_NAME(groupuid) = 'Investigator' AND USER_NAME(memberuid) = @user))
        RETURN 0
    
    --Check that @User is either an Assistant or the PI
    IF EXISTS(     SELECT 1 FROM dbo.ProjectInvestigators AS P 
                LEFT JOIN [dbo].[ProjectInvestigatorAssistants] AS A 
                       ON A.ProjectInvestigator = P.[Login] 
                    WHERE (@User = @ProjectInvestigator AND P.[Login] = @User) 
                       OR (A.ProjectInvestigator = @ProjectInvestigator AND @User = A.Assistant)
             )
        RETURN 1
    RETURN 0
END




GO
GRANT EXECUTE ON [dbo].[IsInvestigatorEditor] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE FUNCTION [dbo].[IsProjectEditor] 
(
    @Project VARCHAR(32)   = NULL, 
    @User sysname = NULL
)
RETURNS BIT
AS
BEGIN
    -- Check that @User is in the Investigator or the Editor Role
    IF NOT EXISTS( SELECT 1 FROM sys.sysmembers WHERE (USER_NAME(groupuid) = 'Editor' AND USER_NAME(memberuid) = @user)
                                                   OR (USER_NAME(groupuid) = 'Investigator' AND USER_NAME(memberuid) = @user))
        RETURN 0
    
    --Check that the users is either an editor or the PI on the project
    IF EXISTS(     SELECT 1 FROM dbo.Projects AS P 
                LEFT JOIN dbo.ProjectEditors AS E ON P.ProjectId = E.ProjectId 
                    WHERE P.ProjectId = @Project
                      AND (E.Editor = @User OR P.ProjectInvestigator = @User))
        RETURN 1
    
    -- Check if user is an assistant to the PI of the project
    IF EXISTS( SELECT 1 FROM dbo.Projects AS P
                 JOIN [dbo].[ProjectInvestigatorAssistants] AS A 
                   ON A.ProjectInvestigator = P.ProjectInvestigator 
                WHERE @Project = P.ProjectId AND @User = A.Assistant)
        RETURN 1	
    RETURN 0
END



GO
GRANT EXECUTE ON [dbo].[IsProjectEditor] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE FUNCTION [dbo].[LocalTime](@utcDateTime [datetime])
RETURNS [datetime] WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [SqlServer_Functions].[SqlServer_Functions.SimpleFunctions].[LocalTime]
GO
GRANT EXECUTE ON [dbo].[LocalTime] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE FUNCTION [dbo].[NextAnimalId] 
(
	@ProjectId VARCHAR(32)   = NULL 
)
RETURNS VARCHAR(16)
AS
BEGIN
	-- Check that project exists
	IF NOT EXISTS( SELECT 1 FROM Projects WHERE ProjectId = @ProjectId)
	BEGIN
			-- You can't raise errors in a function, so you can either do an illegal operation to
			-- stop the process, or in may case I will just return a safe result
			RETURN NULL
	END
	
	DECLARE @Year CHAR(2)
	DECLARE @Count int
	DECLARE @Id VARCHAR(16)

	--Get the prefix for the current year (i.e. 2012 -> '12')
	SELECT @Year  = SUBSTRING(CONVERT(CHAR(4),YEAR(GETDATE())),3,2)
	SELECT @Count =	convert(varchar(10),(max(id) + 1))
	  from (select case WHEN ISNUMERIC(SUBSTRING(AnimalId, 3, 9)) = 1
	                    then convert(int,SUBSTRING(AnimalId, 3, 9))
	                    ELSE 0 END as id
	          from Animals
	         where ProjectId = @ProjectId AND AnimalId LIKE @Year+'%') as t
	if (@Count < 100)
		SELECT @Id = @Year + REPLACE(STR(@Count, 2, 0), ' ', '0')
	else
		SELECT @Id = @Year + REPLACE(STR(@Count, 6, 0), ' ', '')
	If @Id is null
		SET @Id = @Year + '01'
	RETURN @id
END


GO
GRANT EXECUTE ON [dbo].[NextAnimalId] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE FUNCTION [dbo].[NextLocationTime] 
(
	@Project VARCHAR(16)   = NULL, 
	@Animal  VARCHAR(16)   = NULL, 
	@Time    DATETIME2 = NULL
)
RETURNS DATETIME2
AS
BEGIN
	DECLARE @Result DATETIME2
	SET @Result = (SELECT TOP 1 [FixDate] 
					 FROM [dbo].[Locations]
					WHERE [ProjectId] = @Project
					  AND [AnimalId] = @Animal
					  AND [Status] IS NULL
					  AND [FixDate] > @Time
			     ORDER BY [FixDate]);
	RETURN @Result
END

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
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
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
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
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
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
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
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE FUNCTION [dbo].[ParseFormatE](@fileId [int])
RETURNS  TABLE (
	[LineNumber] [int] NULL,
	[ProgramId] [nvarchar](50) NULL,
	[PlatformId] [nvarchar](50) NULL,
	[TransmissionDate] [datetime2](7) NULL,
	[LocationDate] [datetime2](7) NULL,
	[Latitude] [real] NULL,
	[Longitude] [real] NULL,
	[Altitude] [real] NULL,
	[LocationClass] [nchar](1) NULL,
	[Message] [varbinary](50) NULL
) WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [SqlServer_Files].[SqlServer_Files.CollarFileInfo].[ParseFormatE]
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
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
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
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
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
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
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE FUNCTION [dbo].[ParseFormatO](@fileId [int])
RETURNS  TABLE (
	[LineNumber] [int] NULL,
	[idPosition] [nvarchar](50) NULL,
	[idCollar] [nvarchar](50) NULL,
	[acquisitionTime] [nvarchar](50) NULL,
	[scts] [nvarchar](50) NULL,
	[originCode] [nvarchar](50) NULL,
	[ecefX] [nvarchar](50) NULL,
	[ecefY] [nvarchar](50) NULL,
	[ecefZ] [nvarchar](50) NULL,
	[Latitude] [nvarchar](50) NULL,
	[Longitude] [nvarchar](50) NULL,
	[height] [nvarchar](50) NULL,
	[dop] [nvarchar](50) NULL,
	[idFixType] [nvarchar](50) NULL,
	[positionError] [nvarchar](50) NULL,
	[satCount] [nvarchar](50) NULL,
	[ch01SatId] [nvarchar](50) NULL,
	[ch01SatCnr] [nvarchar](50) NULL,
	[ch02SatId] [nvarchar](50) NULL,
	[ch02SatCnr] [nvarchar](50) NULL,
	[ch03SatId] [nvarchar](50) NULL,
	[ch03SatCnr] [nvarchar](50) NULL,
	[ch04SatId] [nvarchar](50) NULL,
	[ch04SatCnr] [nvarchar](50) NULL,
	[ch05SatId] [nvarchar](50) NULL,
	[ch05SatCnr] [nvarchar](50) NULL,
	[ch06SatId] [nvarchar](50) NULL,
	[ch06SatCnr] [nvarchar](50) NULL,
	[ch07SatId] [nvarchar](50) NULL,
	[ch07SatCnr] [nvarchar](50) NULL,
	[ch08SatId] [nvarchar](50) NULL,
	[ch08SatCnr] [nvarchar](50) NULL,
	[ch09SatId] [nvarchar](50) NULL,
	[ch09SatCnr] [nvarchar](50) NULL,
	[ch10SatId] [nvarchar](50) NULL,
	[ch10SatCnr] [nvarchar](50) NULL,
	[ch11SatId] [nvarchar](50) NULL,
	[ch11SatCnr] [nvarchar](50) NULL,
	[ch12SatId] [nvarchar](50) NULL,
	[ch12SatCnr] [nvarchar](50) NULL,
	[idMortalityStatus] [nvarchar](50) NULL,
	[activity] [nvarchar](50) NULL,
	[mainVoltage] [nvarchar](50) NULL,
	[backupVoltage] [nvarchar](50) NULL,
	[temperature] [nvarchar](50) NULL,
	[transformedX] [nvarchar](50) NULL,
	[transformedY] [nvarchar](50) NULL
) WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [SqlServer_Parsers].[SqlServer_Parsers.VectronicParsers].[ParseFormatO]
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE FUNCTION [dbo].[ParseFormatP](@fileId [int])
RETURNS  TABLE (
	[LineNumber] [int] NULL,
	[idMortality] [nvarchar](50) NULL,
	[idCollar] [nvarchar](50) NULL,
	[acquisitionTime] [nvarchar](50) NULL,
	[scts] [nvarchar](50) NULL,
	[originCode] [nvarchar](50) NULL,
	[activityModeCode] [nvarchar](50) NULL,
	[activityModeDt] [nvarchar](50) NULL,
	[activity1] [nvarchar](50) NULL,
	[activity2] [nvarchar](50) NULL,
	[temperature] [nvarchar](50) NULL,
	[activity3] [nvarchar](50) NULL
) WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [SqlServer_Parsers].[SqlServer_Parsers.VectronicParsers].[ParseFormatP]
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE FUNCTION [dbo].[ParseFormatQ](@fileId [int])
RETURNS  TABLE (
	[LineNumber] [int] NULL,
	[idMortality] [nvarchar](50) NULL,
	[idCollar] [nvarchar](50) NULL,
	[acquisitionTime] [nvarchar](50) NULL,
	[scts] [nvarchar](50) NULL,
	[originCode] [nvarchar](50) NULL,
	[idKind] [nvarchar](50) NULL
) WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [SqlServer_Parsers].[SqlServer_Parsers.VectronicParsers].[ParseFormatQ]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[PreviousLocationTime] 
(
	@Project VARCHAR(16)   = NULL, 
	@Animal  VARCHAR(16)   = NULL, 
	@Time    DATETIME2 = NULL
)
RETURNS DATETIME2
AS
BEGIN
	DECLARE @Result DATETIME2
	SET @Result = (SELECT TOP 1 [FixDate] 
					 FROM [dbo].[Locations]
					WHERE [ProjectId] = @Project
					  AND [AnimalId] = @Animal
					  AND [Status] IS NULL
					  AND [FixDate] < @Time
			     ORDER BY [FixDate] DESC);
	RETURN @Result
END

GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE FUNCTION [dbo].[Sha1Hash](@data [varbinary](max))
RETURNS [varbinary](8000) WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [SqlServer_Functions].[SqlServer_Functions.SimpleFunctions].[Sha1Hash]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE FUNCTION [dbo].[StartOfOverlappingMovement] 
(
	@Project VARCHAR(16)   = NULL, 
	@Animal  VARCHAR(16)   = NULL, 
	@Time    DATETIME2 = NULL
)
RETURNS DATETIME2
AS
BEGIN
	DECLARE @Result DATETIME2
	SET @Result = (SELECT TOP 1 [StartDate]
	                 FROM [dbo].[Movements]
					WHERE [ProjectId] = @Project
					  AND [AnimalId] = @Animal
	                  AND [StartDate] < @Time
	                  AND [EndDate] > @Time
	             ORDER BY [StartDate] DESC);
	RETURN @Result
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE FUNCTION [dbo].[StartOfPriorConnectedMovement] 
(
	@Project VARCHAR(16)   = NULL, 
	@Animal  VARCHAR(16)   = NULL, 
	@Time    DATETIME2 = NULL
)
RETURNS DATETIME2
AS
BEGIN
	DECLARE @Result DATETIME2
	SET @Result = (SELECT [StartDate] 
					 FROM [dbo].[Movements]
					WHERE [ProjectId] = @Project
					  AND [AnimalId] = @Animal
					  AND [EndDate] = @Time);
	RETURN @Result
END

GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE FUNCTION [dbo].[SummarizeTpfFile](@fileId [int])
RETURNS  TABLE (
	[FileId] [int] NULL,
	[CTN] [nvarchar](16) NULL,
	[Platform] [nvarchar](16) NULL,
	[PlatformId] [nvarchar](16) NULL,
	[Frequency] [float] NULL,
	[TimeStamp] [datetime2](7) NULL
) WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [SqlServer_TpfSummerizer].[SqlServer_TpfSummerizer.TfpSummerizer].[SummarizeTpfFile]
GO
GRANT SELECT ON [dbo].[SummarizeTpfFile] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE FUNCTION [dbo].[UtcTime](@localDateTime [datetime])
RETURNS [datetime] WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [SqlServer_Functions].[SqlServer_Functions.SimpleFunctions].[UtcTime]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CollarParameterFiles](
	[FileId] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Owner] [sysname] NOT NULL,
	[FileName] [nvarchar](255) NOT NULL,
	[Format] [char](1) NOT NULL,
	[UploadDate] [datetime2](7) NOT NULL,
	[UploadUser] [sysname] NOT NULL,
	[Contents] [varbinary](max) NOT NULL,
	[Status] [char](1) NOT NULL,
	[Sha1Hash]  AS ([dbo].[Sha1Hash]([Contents])) PERSISTED,
 CONSTRAINT [PK_CollarParameterFiles] PRIMARY KEY CLUSTERED 
(
	[FileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[CollarParameterFiles] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO


CREATE VIEW [dbo].[AllTpfFileData]
AS

-- All TPF Data
     SELECT T.*, P.[FileName], P.[Status]
       FROM CollarParameterFiles AS P
CROSS APPLY (SELECT * FROM SummarizeTpfFile(P.FileId)) AS T
      WHERE P.Format = 'A' AND P.[Status] = 'A'
GO
DENY SELECT ON [dbo].[AllTpfFileData] TO [dog_house] AS [dbo]
GO
GRANT SELECT ON [dbo].[AllTpfFileData] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CollarFixes](
	[FixId] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[HiddenBy] [int] NULL,
	[FileId] [int] NOT NULL,
	[LineNumber] [int] NOT NULL,
	[CollarManufacturer] [varchar](16) NOT NULL,
	[CollarId] [varchar](16) NOT NULL,
	[FixDate] [datetime2](7) NOT NULL,
	[Lat] [float] NOT NULL,
	[Lon] [float] NOT NULL,
 CONSTRAINT [PK_CollarFixes] PRIMARY KEY CLUSTERED 
(
	[FixId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[CollarFixes] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Regan Sarwas
-- Create date: May 30, 2012
-- Description:	Returns a table of conflicting fixes for a specific collar.
-- Example:     SELECT * FROM ConflictingFixes('Telonics', '96007', DEFAULT)
-- Modified:    Aug 15, 2012, filtered conflicts only include series that have different locations
-- Modified:    Feb 22, 2013, added an optional currency filter to return only conflicts in the last X days 
-- =============================================
CREATE FUNCTION [dbo].[ConflictingFixes] 
(
	@CollarManufacturer NVARCHAR(255), 
	@CollarId           NVARCHAR(255),
	@LastXdays          INTEGER = 36500
)
RETURNS TABLE 
AS
	RETURN
		SELECT FixId, HiddenBy, FileId, LineNumber, dbo.LocalTime(C.FixDate) AS LocalFixTime, Lat, Lon
		FROM CollarFixes AS C
		INNER JOIN 
			(SELECT   CollarManufacturer, CollarId, FixDate 
			 FROM
				(SELECT DISTINCT CollarManufacturer, CollarId, FixDate, Lat, Lon
				 FROM CollarFixes
			     WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId) AS T
			 GROUP BY CollarManufacturer, CollarId, FixDate
			 HAVING   COUNT(FixDate) > 1) AS D
		ON  C.CollarManufacturer = D.CollarManufacturer
		AND C.CollarId = D.CollarId
		AND C.FixDate = D.FixDate
        AND C.FixDate > DATEADD(DAY, -@LastXdays, GETDATE())

GO
GRANT SELECT ON [dbo].[ConflictingFixes] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArgosFileProcessingIssues](
	[IssueId] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[FileId] [int] NOT NULL,
	[Issue] [nvarchar](max) NOT NULL,
	[PlatformId] [varchar](8) NULL,
	[CollarManufacturer] [varchar](16) NULL,
	[CollarId] [varchar](16) NULL,
	[FirstTransmission] [datetime2](7) NULL,
	[LastTransmission] [datetime2](7) NULL,
 CONSTRAINT [PK_ArgosFileProcessingIssues] PRIMARY KEY CLUSTERED 
(
	[IssueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[ArgosFileProcessingIssues] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CollarFiles](
	[FileId] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[FileName] [nvarchar](255) NOT NULL,
	[UploadDate] [datetime2](7) NOT NULL,
	[ProjectId] [varchar](16) NULL,
	[UserName] [sysname] NOT NULL,
	[CollarManufacturer] [varchar](16) NULL,
	[CollarId] [varchar](16) NULL,
	[Format] [char](1) NOT NULL,
	[Status] [char](1) NOT NULL,
	[Contents] [varbinary](max) NOT NULL,
	[ParentFileId] [int] NULL,
	[Owner] [nvarchar](128) NULL,
	[Sha1Hash]  AS ([dbo].[Sha1Hash]([Contents])) PERSISTED,
	[ArgosDeploymentId] [int] NULL,
	[CollarParameterId] [int] NULL,
 CONSTRAINT [PK_CollarFiles] PRIMARY KEY CLUSTERED 
(
	[FileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[CollarFiles] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[DataLog_NeverProcessed]
AS
----------- DataLog_NeverProcessed
-----------   Is a file of format 'H' (It has no Argos transmissions),
-----------   but no child files, and no processing issues
     SELECT P.FileId
       FROM CollarFiles AS P      
  LEFT JOIN CollarFiles AS C
         ON P.FileId = C.ParentFileId
  LEFT JOIN ArgosFileProcessingIssues AS I
         ON I.FileId = P.FileId
      WHERE P.Format = 'H'
        AND C.FileId IS NULL
        AND I.FileId IS NULL

GO
GRANT SELECT ON [dbo].[DataLog_NeverProcessed] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Locations](
	[ProjectId] [varchar](16) NOT NULL,
	[AnimalId] [varchar](16) NOT NULL,
	[FixDate] [datetime2](7) NOT NULL,
	[Location] [geography] NOT NULL,
	[FixId] [int] NOT NULL,
	[Status] [char](1) NULL,
 CONSTRAINT [PK_Locations] PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC,
	[AnimalId] ASC,
	[FixDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[Locations] TO [Editor] AS [dbo]
GO
GRANT SELECT ON [dbo].[Locations] TO [INPAKROVMAIS\sql_proxy] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[MostRecentLocations]
AS
SELECT L.ProjectId, L.AnimalId, L.FixDate, dbo.LocalTime(L.[FixDate]) as [LocalDateTime], L.Location, L.FixId
FROM   dbo.Locations AS L
INNER JOIN
	   (SELECT   ProjectId, AnimalId, MAX(FixDate) AS FixDate
		FROM     dbo.Locations
		WHERE    [Status] IS NULL
		GROUP BY ProjectId, AnimalId) AS F
ON F.ProjectId = L.ProjectId AND F.AnimalId = L.AnimalId AND F.FixDate = L.FixDate

GO
DENY SELECT ON [dbo].[MostRecentLocations] TO [dog_house] AS [dbo]
GO
GRANT SELECT ON [dbo].[MostRecentLocations] TO [Editor] AS [dbo]
GO
GRANT SELECT ON [dbo].[MostRecentLocations] TO [INPAKROVMAIS\sql_proxy] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[FirstKnownLocations]
AS
SELECT L.ProjectId, L.AnimalId, L.FixDate, L.Location, L.FixId
FROM   dbo.Locations AS L
INNER JOIN
	   (SELECT   ProjectId, AnimalId, MIN(FixDate) AS FixDate
		FROM     dbo.Locations
		WHERE    [Status] IS NULL
		GROUP BY ProjectId, AnimalId) AS F
ON F.ProjectId = L.ProjectId AND F.AnimalId = L.AnimalId AND F.FixDate = L.FixDate


GO
DENY SELECT ON [dbo].[FirstKnownLocations] TO [dog_house] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Animals](
	[ProjectId] [varchar](16) NOT NULL,
	[AnimalId] [varchar](16) NOT NULL,
	[Species] [varchar](32) NULL,
	[Gender] [varchar](7) NOT NULL,
	[MortalityDate] [datetime2](7) NULL,
	[GroupName] [nvarchar](500) NULL,
	[Description] [nvarchar](2000) NULL,
 CONSTRAINT [PK_Animals] PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC,
	[AnimalId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[Animals] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[LastLocationOfKnownMortalities]
AS
SELECT L.ProjectId, L.AnimalId, L.FixDate, L.Location, L.FixId
FROM Locations AS L
INNER JOIN
   (SELECT A.ProjectId, A.AnimalId, MAX(FixDate) AS FixDate
	FROM dbo.Locations AS L
	INNER JOIN dbo.Animals AS A
	ON A.AnimalId = L.AnimalId AND A.ProjectId = L.ProjectId AND L.FixDate < A.MortalityDate
	WHERE [Status] IS NULL
	GROUP BY A.ProjectId, A.AnimalId) AS F
ON F.ProjectId = L.ProjectId AND F.AnimalId = L.AnimalId and F.FixDate = L.FixDate
GO
DENY SELECT ON [dbo].[LastLocationOfKnownMortalities] TO [dog_house] AS [dbo]
GO
GRANT SELECT ON [dbo].[LastLocationOfKnownMortalities] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/* Converts a datetime to the ordinal date (day of the year)
 * commonly confused with the Julian date (days sine 1/1/4713BC)
 */
CREATE FUNCTION [dbo].[DateTimeToOrdinal] (
    @Date DATETIME2  
)   
	RETURNS INT
    WITH SCHEMABINDING -- This is a deterministic function.

AS
BEGIN
    RETURN 1 + DATEDIFF (day,
                         CONVERT(DATETIME2,
                                 CAST(YEAR(@Date) AS CHAR(4))+'0101',
                                 112), -- format 112 (yyyymmdd) is deterministic
                         @Date)
END

GO
GRANT EXECUTE ON [dbo].[DateTimeToOrdinal] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Movements](
	[ProjectId] [varchar](16) NOT NULL,
	[AnimalId] [varchar](16) NOT NULL,
	[StartDate] [datetime2](7) NOT NULL,
	[EndDate] [datetime2](7) NOT NULL,
	[Duration] [float] NOT NULL,
	[Distance] [float] NOT NULL,
	[Speed] [float] NOT NULL,
	[Shape] [geography] NOT NULL,
 CONSTRAINT [PK_Movement] PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC,
	[AnimalId] ASC,
	[StartDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[Movements] TO [Editor] AS [dbo]
GO
GRANT SELECT ON [dbo].[Movements] TO [INPAKROVMAIS\sql_proxy] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Projects](
	[ProjectId] [varchar](16) NOT NULL,
	[ProjectName] [nvarchar](150) NOT NULL,
	[ProjectInvestigator] [sysname] NOT NULL,
	[UnitCode] [char](4) NULL,
	[Description] [nvarchar](2000) NULL,
 CONSTRAINT [PK_Projects] PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[Projects] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE VIEW [dbo].[VelocityVectors]
AS
    SELECT M.[ProjectId]
          ,M.[AnimalId]
          ,M.[StartDate]
          ,M.[EndDate]
          ,dbo.LocalTime(M.[StartDate]) as [LocalDateTime]
          ,dbo.LocalTime(M.[EndDate]) as [EndLocalDateTime]
          ,YEAR(dbo.LocalTime(M.[StartDate])) as [Year]
          ,dbo.DateTimeToOrdinal(dbo.LocalTime(M.[StartDate])) as [OrdinalDate]
          ,M.[Duration]
          ,M.[Distance]
          ,M.[Speed]
          ,P.[UnitCode]
          ,A.[Species]
          ,A.[Gender]
          ,A.[GroupName]
          ,M.[Shape]
      FROM dbo.Movements AS M
INNER JOIN dbo.Animals   AS A  ON M.ProjectId = A.ProjectId
							  AND M.AnimalId  = A.AnimalId
INNER JOIN dbo.Projects  AS P  ON A.ProjectId = P.ProjectId
     WHERE M.Distance <> 0




GO
DENY SELECT ON [dbo].[VelocityVectors] TO [dog_house] AS [dbo]
GO
GRANT SELECT ON [dbo].[VelocityVectors] TO [Editor] AS [dbo]
GO
GRANT SELECT ON [dbo].[VelocityVectors] TO [INPAKROVMAIS\sql_proxy] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[PotentialMortalities]
AS
SELECT V.ProjectId, V.AnimalId, max(F.FixDate) as last_date, 14 as days, count(*) as num_fixes, 
       min(duration) as min_dur, 14 * 24 / min(v.duration) as expected_fix_count,
	   avg(v.speed) as avg_speed
	   --, geography::EnvelopeAggregate(v.Shape) AS Shape
FROM dbo.VelocityVectors AS V
INNER JOIN
	   (SELECT   ProjectId, AnimalId, MAX(FixDate) AS FixDate
		FROM     dbo.Locations
		WHERE    [Status] IS NULL
		GROUP BY ProjectId, AnimalId) AS F
ON F.ProjectId = V.ProjectId AND F.AnimalId = V.AnimalId AND DATEDIFF(day, V.EndDate, F.FixDate) < 14
INNER JOIN dbo.Animals as A on A.ProjectId = V.ProjectId AND A.AnimalId = V.AnimalId
where a.MortalityDate is null  -- not already identified as a mortality
group by V.ProjectId, V.AnimalId
-- We are looking at the last 14 days worth of fixes for each animal
having avg(v.speed) < 5  -- stationary; average speed is less than 3 meters per hour
-- The following criteria help remove false positives, but they can also hide true positives.  Remove for now, to let PI assess
--and count(*) > 7 -- we have sufficient fixes (at least one every other day) to make any conclusions (removes problems with large min duration)
--and count(*) / (14 * 24 / min(v.duration))  > .5 -- we have at least 50% of the fixes expected (assuming min duration is valid)

GO
DENY SELECT ON [dbo].[PotentialMortalities] TO [dog_house] AS [dbo]
GO
GRANT SELECT ON [dbo].[PotentialMortalities] TO [Editor] AS [dbo]
GO
GRANT SELECT ON [dbo].[PotentialMortalities] TO [INPAKROVMAIS\sql_proxy] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE VIEW [dbo].[VelocityVectors_NPS]
AS
    SELECT M.[ProjectId]
          ,M.[AnimalId]
          ,M.[StartDate]
          ,M.[EndDate]
          ,dbo.LocalTime(M.[StartDate]) as [LocalDateTime]
          ,dbo.LocalTime(M.[EndDate]) as [EndLocalDateTime]
          ,YEAR(dbo.LocalTime(M.[StartDate])) as [Year]
          ,dbo.DateTimeToOrdinal(dbo.LocalTime(M.[StartDate])) as [OrdinalDate]
          ,M.[Duration]
          ,M.[Distance]
          ,M.[Speed]
          ,P.[UnitCode]
          ,A.[Species]
          ,A.[Gender]
          ,A.[GroupName]
          ,M.[Shape]
      FROM dbo.Movements AS M
INNER JOIN dbo.Animals   AS A  ON M.ProjectId = A.ProjectId
							  AND M.AnimalId  = A.AnimalId
INNER JOIN dbo.Projects  AS P  ON A.ProjectId = P.ProjectId
     WHERE M.Distance <> 0
       AND M.StartDate < DATEADD(DAY,-30,GETDATE())




GO
DENY SELECT ON [dbo].[VelocityVectors_NPS] TO [dog_house] AS [dbo]
GO
GRANT SELECT ON [dbo].[VelocityVectors_NPS] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Regan Sarwas
-- Create date: May 30, 2012
-- Description:	Returns a row of location information for a specific animal.
-- Example:     SELECT * FROM AnimalLocationSummary('LACL_Sheep', '0501')
-- =============================================
CREATE FUNCTION [dbo].[AnimalLocationSummary] 
(
	@ProjectId	NVARCHAR(255), 
	@AnimalId	NVARCHAR(255)
)
RETURNS TABLE 
AS
	RETURN
		SELECT	COUNT(*) as [Count],
				MIN(Location.Long) as [Left], MAX(Location.Long) as [Right],
				MIN(Location.Lat) as [Bottom], MAX(Location.Lat) as [Top],
				dbo.LocalTime(MIN(FixDate)) AS [First], dbo.LocalTime(MAX(FixDate)) as [Last]
		 FROM     [dbo].[Locations]
		 WHERE    ProjectId = @ProjectId AND AnimalId = @AnimalId AND Status IS NULL
		 GROUP BY ProjectId, AnimalId


GO
GRANT SELECT ON [dbo].[AnimalLocationSummary] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE VIEW [dbo].[NoMovement]
AS
    SELECT M.[ProjectId]
          ,M.[AnimalId]
          --,M.[StartDate]
          --,M.[EndDate]
          ,dbo.LocalTime(M.[StartDate]) as [LocalDateTime]
          ,dbo.LocalTime(M.[EndDate]) as [EndLocalDateTime]
          ,YEAR(dbo.LocalTime(M.[StartDate])) as [Year]
          ,dbo.DateTimeToOrdinal(dbo.LocalTime(M.[StartDate])) as [OrdinalDate]
          ,M.[Duration]
          --,M.[Distance]
          ,M.[Speed]
          ,P.[UnitCode]
          ,A.[Species]
          ,A.[Gender]
          ,A.[GroupName]
          ,M.[Shape]
      FROM dbo.Movements AS M
INNER JOIN dbo.Animals   AS A  ON M.ProjectId = A.ProjectId
							  AND M.AnimalId  = A.AnimalId
INNER JOIN dbo.Projects  AS P  ON A.ProjectId = P.ProjectId
     WHERE M.Distance = 0



GO
DENY SELECT ON [dbo].[NoMovement] TO [dog_house] AS [dbo]
GO
GRANT SELECT ON [dbo].[NoMovement] TO [Editor] AS [dbo]
GO
GRANT SELECT ON [dbo].[NoMovement] TO [INPAKROVMAIS\sql_proxy] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE VIEW [dbo].[ValidLocations_NPS]
AS
    SELECT L.FixId
          ,L.ProjectId
          ,L.AnimalId
          ,L.[FixDate]
          ,dbo.LocalTime(L.[FixDate]) as [LocalDateTime]
          ,YEAR(dbo.LocalTime(L.[FixDate])) as [Year]
          ,dbo.DateTimeToOrdinal(dbo.LocalTime(L.[FixDate])) as [OrdinalDate]
          ,P.[UnitCode]
          ,A.[Species]
          ,A.[Gender]
          ,A.[GroupName]
          --,L.[Status]
          ,L.[Location] AS [Shape]
      FROM dbo.Locations AS L
INNER JOIN dbo.Animals   AS A  ON L.ProjectId = A.ProjectId
							  AND L.AnimalId  = A.AnimalId
INNER JOIN dbo.Projects  AS P  ON A.ProjectId = P.ProjectId
     WHERE L.Status IS NULL
       AND L.[FixDate] < DATEADD(DAY,-30,GETDATE())





GO
DENY SELECT ON [dbo].[ValidLocations_NPS] TO [dog_house] AS [dbo]
GO
GRANT SELECT ON [dbo].[ValidLocations_NPS] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LookupFileStatus](
	[Code] [char](1) NOT NULL,
	[Name] [nvarchar](32) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [PK_CollarFileStatus] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[LookupFileStatus] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Regan Sarwas
-- Create date: May 31, 2012
-- Description:	Returns a table of files with fixes for a specific collar.
-- Example:     SELECT * FROM CollarFixesByFile('Telonics', '96007')
-- =============================================
CREATE FUNCTION [dbo].[CollarFixesByFile] 
(
	@CollarManufacturer NVARCHAR(255), 
	@CollarId           NVARCHAR(255)
)
RETURNS TABLE 
AS
	RETURN
		SELECT  F.[FileId],
				F.[FileName] AS [File],
				S.[Name] AS [Status],
				COUNT(FixDate) AS [FixCount],
				dbo.LocalTime(MIN(FixDate)) AS [First],
				dbo.LocalTime(MAX(FixDate)) AS [Last]
		FROM [dbo].[CollarFiles] AS F
		INNER JOIN [dbo].[LookupFileStatus] AS S
		ON F.[Status] = S.[Code]
		LEFT JOIN [dbo].[CollarFixes] AS X
		ON X.FileId = F.FileId
		WHERE (F.CollarManufacturer = @CollarManufacturer AND F.CollarId = @CollarId)
		   OR (X.CollarManufacturer = @CollarManufacturer AND X.CollarId = @CollarId)
		GROUP BY F.[FileId], F.[FileName], S.[Name]




GO
GRANT SELECT ON [dbo].[CollarFixesByFile] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[InvalidLocations]
AS
    SELECT L.FixId
          ,L.ProjectId
          ,L.AnimalId
          ,L.[FixDate]
          ,dbo.LocalTime(L.[FixDate]) as [LocalDateTime]
          ,YEAR(dbo.LocalTime(L.[FixDate])) as [Year]
          ,dbo.DateTimeToOrdinal(dbo.LocalTime(L.[FixDate])) as [OrdinalDate]
          ,P.[UnitCode]
          ,A.[Species]
          ,A.[Gender]
          ,A.[GroupName]
          ,L.[Status]
          ,L.[Location] AS [Shape]
      FROM dbo.Locations AS L
INNER JOIN dbo.Animals   AS A  ON L.ProjectId = A.ProjectId
							  AND L.AnimalId  = A.AnimalId
INNER JOIN dbo.Projects  AS P  ON A.ProjectId = P.ProjectId
     WHERE L.Status IS NOT NULL




GO
DENY SELECT ON [dbo].[InvalidLocations] TO [dog_house] AS [dbo]
GO
GRANT SELECT ON [dbo].[InvalidLocations] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArgosDeployments](
	[DeploymentId] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[PlatformId] [varchar](8) NOT NULL,
	[CollarManufacturer] [varchar](16) NOT NULL,
	[CollarId] [varchar](16) NOT NULL,
	[StartDate] [datetime2](7) NULL,
	[EndDate] [datetime2](7) NULL,
 CONSTRAINT [PK_ArgosDeployments] PRIMARY KEY CLUSTERED 
(
	[DeploymentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[ArgosDeployments] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CollarParameters](
	[ParameterId] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[CollarManufacturer] [varchar](16) NOT NULL,
	[CollarId] [varchar](16) NOT NULL,
	[FileId] [int] NULL,
	[Gen3Period] [int] NULL,
	[StartDate] [datetime2](7) NULL,
	[EndDate] [datetime2](7) NULL,
 CONSTRAINT [PK_CollarParameters] PRIMARY KEY CLUSTERED 
(
	[ParameterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[CollarParameters] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Collars](
	[CollarManufacturer] [varchar](16) NOT NULL,
	[CollarId] [varchar](16) NOT NULL,
	[CollarModel] [varchar](24) NOT NULL,
	[Manager] [sysname] NOT NULL,
	[Owner] [nvarchar](100) NULL,
	[SerialNumber] [varchar](100) NULL,
	[Frequency] [float] NULL,
	[HasGps] [bit] NOT NULL,
	[Notes] [nvarchar](max) NULL,
	[DisposalDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Collars] PRIMARY KEY CLUSTERED 
(
	[CollarManufacturer] ASC,
	[CollarId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[Collars] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		Regan Sarwas
-- Create date: March 8, 2013
-- Description:	Returns a table of Telonics processing parameters
--              for the Argos Id in the date range. 
-- Example:     SELECT * FROM GetTelonicsParametersForArgosDates('37470', '2008-03-01', '2014-03-01') ORDER BY StartDate
-- Caller need to check that the results cover the entire requested date range
-- Caller should order the results by startdate, then check the following:
--   if @StartDate < firstrecord.startDate(and not null) then Missing parameters
--   if @EndDate > lastrecord.EndDate(and not null) more missing parameters

-- Update May 8, 2013
--   Removed valid parameter check.  The processor will now be able to check
--   if there is a valid Argos-collar deployment, but no collar-parameter
--   assignment, and therefore write a better error message.
--   The client must now check for invalid parameters.
-- =============================================
CREATE FUNCTION [dbo].[GetTelonicsParametersForArgosDates] 
(
    @PlatformID VARCHAR(8),
    @StartDate DATETIME2(7),
    @EndDate DATETIME2(7)
)
RETURNS TABLE 
AS
    RETURN
     SELECT A.DeploymentId, P.ParameterId, A.PlatformId, A.CollarManufacturer, A.CollarId, C.CollarModel,
            P.Gen3Period, F.Format, F.[FileName], F.Contents,
            CASE WHEN (A.StartDate IS NOT NULL AND P.StartDate IS NOT NULL)
                 THEN (CASE WHEN A.StartDate < P.StartDate THEN P.StartDate ELSE A.StartDate END)
                 ELSE (CASE WHEN A.StartDate IS NULL THEN P.StartDate ELSE A.StartDate END) END AS StartDate,
            CASE WHEN (A.EndDate IS NOT NULL AND P.EndDate IS NOT NULL)
                 THEN (CASE WHEN A.EndDate < P.EndDate THEN A.EndDate ELSE P.EndDate END)
                 ELSE (CASE WHEN A.EndDate IS NULL THEN P.EndDate ELSE A.EndDate END) END AS EndDate
       FROM ArgosDeployments AS A
 INNER JOIN Collars AS C
         ON A.CollarManufacturer = C.CollarManufacturer AND A.CollarId = C.CollarId
  LEFT JOIN CollarParameters AS P
         ON C.CollarManufacturer = P.CollarManufacturer AND C.CollarId = P.CollarId
  LEFT JOIN CollarParameterFiles AS F
         ON P.FileId = F.FileId
      WHERE A.PlatformId = @PlatformID
        AND (A.StartDate IS NULL OR A.StartDate < @EndDate)
        AND (P.StartDate IS NULL OR P.StartDate < @EndDate)
        AND (A.EndDate IS NULL OR @StartDate < A.EndDate)
        AND (P.EndDate IS NULL OR @StartDate < P.EndDate)



GO
GRANT SELECT ON [dbo].[GetTelonicsParametersForArgosDates] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE VIEW [dbo].[ValidLocationsWithLatLong]
AS
    SELECT L.FixId
          ,L.ProjectId
          ,L.AnimalId
          ,L.[FixDate]
          ,dbo.LocalTime(L.[FixDate]) as [LocalDateTime]
          ,YEAR(dbo.LocalTime(L.[FixDate])) as [Year]
          ,dbo.DateTimeToOrdinal(dbo.LocalTime(L.[FixDate])) as [OrdinalDate]
          ,P.[UnitCode]
          ,A.[Species]
          ,A.[Gender]
          ,A.[GroupName]
          ,L.[Location] AS [Shape]
          ,L.[Location].Lat AS [Lat_WGS84]
          ,L.[Location].Long AS [Lon_WGS84]
      FROM dbo.Locations AS L
INNER JOIN dbo.Animals   AS A  ON L.ProjectId = A.ProjectId
							  AND L.AnimalId  = A.AnimalId
INNER JOIN dbo.Projects  AS P  ON A.ProjectId = P.ProjectId
     WHERE L.Status IS NULL





GO
DENY SELECT ON [dbo].[ValidLocationsWithLatLong] TO [dog_house] AS [dbo]
GO
GRANT SELECT ON [dbo].[ValidLocationsWithLatLong] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[AnimalLocations]
AS
    SELECT L.ProjectId
          ,L.AnimalId
          ,dbo.LocalTime(L.[FixDate]) as [Local DateTime]
          ,A.[Species]
          ,A.[Gender]
          ,A.[GroupName]
          ,L.[Location].Lat AS [Lat WGS84]
          ,L.[Location].Long AS [Lon WGS84]
          ,L.[Status]
      FROM dbo.Locations AS L
INNER JOIN dbo.Animals   AS A  ON L.ProjectId = A.ProjectId
                              AND L.AnimalId  = A.AnimalId

GO
DENY SELECT ON [dbo].[AnimalLocations] TO [dog_house] AS [dbo]
GO
GRANT SELECT ON [dbo].[AnimalLocations] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE VIEW [dbo].[ValidLocations]
AS
    SELECT L.FixId
          ,L.ProjectId
          ,L.AnimalId
          ,L.[FixDate]
          ,dbo.LocalTime(L.[FixDate]) as [LocalDateTime]
          ,YEAR(dbo.LocalTime(L.[FixDate])) as [Year]
          ,dbo.DateTimeToOrdinal(dbo.LocalTime(L.[FixDate])) as [OrdinalDate]
          ,P.[UnitCode]
          ,A.[Species]
          ,A.[Gender]
          ,A.[GroupName]
          --,L.[Status]
          ,L.[Location] AS [Shape]
      FROM dbo.Locations AS L
INNER JOIN dbo.Animals   AS A  ON L.ProjectId = A.ProjectId
							  AND L.AnimalId  = A.AnimalId
INNER JOIN dbo.Projects  AS P  ON A.ProjectId = P.ProjectId
     WHERE L.Status IS NULL





GO
DENY SELECT ON [dbo].[ValidLocations] TO [dog_house] AS [dbo]
GO
GRANT SELECT ON [dbo].[ValidLocations] TO [Editor] AS [dbo]
GO
GRANT SELECT ON [dbo].[ValidLocations] TO [INPAKROVMAIS\sql_proxy] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE VIEW [dbo].[NoMovement_NPS]
AS
    SELECT M.[ProjectId]
          ,M.[AnimalId]
          --,M.[StartDate]
          --,M.[EndDate]
          ,dbo.LocalTime(M.[StartDate]) as [LocalDateTime]
          ,dbo.LocalTime(M.[EndDate]) as [EndLocalDateTime]
          ,YEAR(dbo.LocalTime(M.[StartDate])) as [Year]
          ,dbo.DateTimeToOrdinal(dbo.LocalTime(M.[StartDate])) as [OrdinalDate]
          ,M.[Duration]
          --,M.[Distance]
          ,M.[Speed]
          ,P.[UnitCode]
          ,A.[Species]
          ,A.[Gender]
          ,A.[GroupName]
          ,M.[Shape]
      FROM dbo.Movements AS M
INNER JOIN dbo.Animals   AS A  ON M.ProjectId = A.ProjectId
							  AND M.AnimalId  = A.AnimalId
INNER JOIN dbo.Projects  AS P  ON A.ProjectId = P.ProjectId
     WHERE M.Distance = 0
       AND M.StartDate < DATEADD(DAY,-30,GETDATE())



GO
DENY SELECT ON [dbo].[NoMovement_NPS] TO [dog_house] AS [dbo]
GO
GRANT SELECT ON [dbo].[NoMovement_NPS] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CollarDataIridiumMail](
	[FileId] [int] NOT NULL,
	[LineNumber] [int] NOT NULL,
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
	[MessageBytes] [varchar](5000) NULL,
 CONSTRAINT [PK_CollarDataIridiumMail] PRIMARY KEY CLUSTERED 
(
	[FileId] ASC,
	[LineNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[CollarDataIridiumMail] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





-- =============================================
-- Author:		Regan Sarwas
-- Create date: Feb 22, 2016
-- Description:	Returns a table with zero or one row containing the collar id and parameter file id
--              associated with an Iridium download file.
--              ASSUMES only one IMEI (Iridium ID - or collar) per 'CollarDataIridiumMail' File.
--              This matches the download data recieved so far.
-- Example:     SELECT * FROM CollarParametersForIridiumDownload(40025)
-- =============================================
CREATE FUNCTION [dbo].[CollarParametersForIridiumDownload] 
(
	@FileId  INT
)
RETURNS TABLE 
AS
	RETURN
	  SELECT TOP 1
		T.fileid AS [ParameterFileId],
		'Telonics' AS [CollarManufacturer],
		CTN AS [CollarId]
	  FROM [dbo].[CollarDataIridiumMail] AS D
	  JOIN [dbo].[AllTpfFileData] AS T
	  ON T.[PlatformId] = D.[Imei] AND T.[TimeStamp] < D.[MessageTime]
	  WHERE T.[Platform] = 'Iridium'
	  AND D.[FileId] = @FileId
	  ORDER BY T.[TimeStamp] DESC



GO
GRANT SELECT ON [dbo].[CollarParametersForIridiumDownload] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[IDF_NeverProcessed]
AS
----------- IDF_NeverProcessed
-----------   Is a Active file of format 'I' (It has no Argos transmissions),
-----------   but no child files, and no processing issues
     SELECT P.FileId
       FROM CollarFiles AS P      
  LEFT JOIN CollarFiles AS C
         ON P.FileId = C.ParentFileId
  LEFT JOIN ArgosFileProcessingIssues AS I
         ON I.FileId = P.FileId
      WHERE P.[Format] = 'I'
	    AND P.[Status] = 'A'
        AND C.FileId IS NULL
        AND I.FileId IS NULL



GO
GRANT SELECT ON [dbo].[IDF_NeverProcessed] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArgosFilePlatformDates](
	[ItemId] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[FileId] [int] NOT NULL,
	[PlatformId] [varchar](8) NOT NULL,
	[ProgramId] [varchar](8) NULL,
	[FirstTransmission] [datetime2](7) NOT NULL,
	[LastTransmission] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_ArgosFilePlatformDates] PRIMARY KEY CLUSTERED 
(
	[ItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[ArgosFilePlatformDates] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[ArgosFile_NeverProcessed]
AS
----------- ArgosFile_NeverProcessed
-----------   Is a file which has transmissions (ensures file is correct format),
-----------   but no child files, and no processing issues
-----------   (relies on files of correct format having been summerized) 
     SELECT T.FileId
       FROM ArgosFilePlatformDates AS T      
  LEFT JOIN CollarFiles AS C
         ON T.FileId = C.ParentFileId
  LEFT JOIN ArgosFileProcessingIssues AS I
         ON I.FileId = T.FileId
      WHERE C.FileId IS NULL
        AND I.FileId IS NULL
   GROUP BY T.FileId
GO
GRANT SELECT ON [dbo].[ArgosFile_NeverProcessed] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE VIEW [dbo].[ArgosFile_NeedsPartialProcessing]
AS
----------- ArgosFile_NeedsPartialProcessing
----------- Files/platforms that can and should be processed (do it as soon as possible)
-----------   Is a (file,platform) where the file has transmissions
-----------   that can be processed (overlapping deployment/parameters)
-----------   but there is no results file, and no processing issues.
	 SELECT A.FileId, A.PlatformId
	   FROM ArgosFilePlatformDates AS A
  LEFT JOIN 
         ( CollarFiles AS C
		   LEFT JOIN ArgosDeployments AS D
				  ON D.DeploymentId = C.ArgosDeploymentId
				 AND D.CollarManufacturer = C.CollarManufacturer AND D.CollarId = C.CollarId
		   LEFT JOIN CollarParameters AS P
				  ON P.ParameterId = C.CollarParameterId
				 AND P.CollarManufacturer = C.CollarManufacturer AND P.CollarId = C.CollarId
		) ON C.ParentFileId = A.FileId AND D.PlatformId = A.PlatformId
  LEFT JOIN ArgosFileProcessingIssues AS I
		 ON I.PlatformId = A.PlatformId AND I.FileId = A.FileId
	  WHERE A.FileId IS NOT NULL AND A.PlatformId IS NOT NULL
        AND (D.StartDate IS NULL OR D.StartDate < A.FirstTransmission)
        AND (P.StartDate IS NULL OR P.StartDate < A.FirstTransmission)
        AND (D.EndDate IS NULL OR A.LastTransmission < D.EndDate)
        AND (P.EndDate IS NULL OR A.LastTransmission < P.EndDate)
	    AND C.FileId IS NULL  AND I.IssueId IS NULL
		AND A.FileId NOT IN (SELECT FileId FROM ArgosFile_NeverProcessed)



GO
GRANT SELECT ON [dbo].[ArgosFile_NeedsPartialProcessing] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CollarDeployments](
	[DeploymentId] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[ProjectId] [varchar](16) NOT NULL,
	[AnimalId] [varchar](16) NOT NULL,
	[CollarManufacturer] [varchar](16) NOT NULL,
	[CollarId] [varchar](16) NOT NULL,
	[DeploymentDate] [datetime2](7) NOT NULL,
	[RetrievalDate] [datetime2](7) NULL,
 CONSTRAINT [PK_CollarDeployments] PRIMARY KEY CLUSTERED 
(
	[DeploymentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[CollarDeployments] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LookupCollarManufacturers](
	[CollarManufacturer] [varchar](16) NOT NULL,
	[Name] [nvarchar](200) NULL,
	[Website] [nvarchar](200) NULL,
	[Description] [nvarchar](2000) NULL,
 CONSTRAINT [PK_CollarManufacturers] PRIMARY KEY CLUSTERED 
(
	[CollarManufacturer] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[LookupCollarManufacturers] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[AnimalFixesByFile]
AS

----------- Animal Fixes by File
     SELECT F.FileId, M.Name AS Manufacturer, F.CollarId, P.ProjectName AS Project, CD.AnimalId,
		    dbo.LocalTime(MIN(F.FixDate)) AS [First Fix],
			dbo.LocalTime(MAX(F.FixDate)) AS [Last Fix],
            COUNT(F.FixDate) AS [Number of Fixes]
	   FROM dbo.CollarFixes AS F
 INNER JOIN dbo.CollarDeployments AS CD
	     ON F.CollarManufacturer = CD.CollarManufacturer AND F.CollarId = CD.CollarId
 INNER JOIN dbo.LookupCollarManufacturers AS M
	     ON F.CollarManufacturer = M.CollarManufacturer
  LEFT JOIN dbo.Projects AS P
         ON CD.ProjectId = P.ProjectId
	  WHERE F.FixDate > CD.DeploymentDate
        AND (F.FixDate < CD.RetrievalDate OR CD.RetrievalDate IS NULL)
   GROUP BY P.ProjectName, CD.AnimalId, F.FileId, M.Name, F.CollarId



GO
DENY SELECT ON [dbo].[AnimalFixesByFile] TO [dog_house] AS [dbo]
GO
GRANT SELECT ON [dbo].[AnimalFixesByFile] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CollarKeys](
	[CollarManufacturer] [varchar](16) NOT NULL,
	[CollarId] [varchar](16) NOT NULL,
	[CollarKey] [varchar](512) NULL,
 CONSTRAINT [PK_CollarKeys] PRIMARY KEY CLUSTERED 
(
	[CollarManufacturer] ASC,
	[CollarId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[CollarKeys] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CollarSensors](
	[CollarManufacturer] [varchar](16) NOT NULL,
	[CollarId] [varchar](16) NOT NULL,
	[SensorCode] [varchar](8) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_CollarSensors] PRIMARY KEY CLUSTERED 
(
	[CollarManufacturer] ASC,
	[CollarId] ASC,
	[SensorCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[CollarSensors] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CollarDataVectronicActivity](
	[FileId] [int] NOT NULL,
	[LineNumber] [int] NOT NULL,
	[idActivity] [varchar](50) NULL,
	[idCollar] [varchar](50) NULL,
	[acquisitionTime] [varchar](50) NULL,
	[scts] [varchar](50) NULL,
	[originCode] [varchar](50) NULL,
	[activityModeCode] [varchar](50) NULL,
	[activityModeDt] [varchar](50) NULL,
	[activity1] [varchar](50) NULL,
	[activity2] [varchar](50) NULL,
	[temperature] [varchar](50) NULL,
	[activity3] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[FileId] ASC,
	[LineNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[CollarDataVectronicActivity] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CollarDataVectronicMortality](
	[FileId] [int] NOT NULL,
	[LineNumber] [int] NOT NULL,
	[idMortality] [varchar](50) NULL,
	[idCollar] [varchar](50) NULL,
	[acquisitionTime] [varchar](50) NULL,
	[scts] [varchar](50) NULL,
	[originCode] [varchar](50) NULL,
	[idKind] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[FileId] ASC,
	[LineNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[CollarDataVectronicMortality] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CollarDataVectronicPosition](
	[FileId] [int] NOT NULL,
	[LineNumber] [int] NOT NULL,
	[idPosition] [varchar](50) NULL,
	[idCollar] [varchar](50) NULL,
	[acquisitionTime] [varchar](50) NULL,
	[scts] [varchar](50) NULL,
	[originCode] [varchar](10) NULL,
	[ecefX] [varchar](50) NULL,
	[ecefY] [varchar](50) NULL,
	[ecefZ] [varchar](50) NULL,
	[latitude] [varchar](50) NULL,
	[longitude] [varchar](50) NULL,
	[height] [varchar](50) NULL,
	[dop] [varchar](50) NULL,
	[idFixType] [varchar](50) NULL,
	[positionError] [varchar](50) NULL,
	[satCount] [varchar](50) NULL,
	[ch01SatId] [varchar](50) NULL,
	[ch01SatCnr] [varchar](50) NULL,
	[ch02SatId] [varchar](50) NULL,
	[ch02SatCnr] [varchar](50) NULL,
	[ch03SatId] [varchar](50) NULL,
	[ch03SatCnr] [varchar](50) NULL,
	[ch04SatId] [varchar](50) NULL,
	[ch04SatCnr] [varchar](50) NULL,
	[ch05SatId] [varchar](50) NULL,
	[ch05SatCnr] [varchar](50) NULL,
	[ch06SatId] [varchar](50) NULL,
	[ch06SatCnr] [varchar](50) NULL,
	[ch07SatId] [varchar](50) NULL,
	[ch07SatCnr] [varchar](50) NULL,
	[ch08SatId] [varchar](50) NULL,
	[ch08SatCnr] [varchar](50) NULL,
	[ch09SatId] [varchar](50) NULL,
	[ch09SatCnr] [varchar](50) NULL,
	[ch10SatId] [varchar](50) NULL,
	[ch10SatCnr] [varchar](50) NULL,
	[ch11SatId] [varchar](50) NULL,
	[ch11SatCnr] [varchar](50) NULL,
	[ch12SatId] [varchar](50) NULL,
	[ch12SatCnr] [varchar](50) NULL,
	[idMortalityStatus] [varchar](50) NULL,
	[activity] [varchar](50) NULL,
	[mainVoltage] [varchar](50) NULL,
	[backupVoltage] [varchar](50) NULL,
	[temperature] [varchar](50) NULL,
	[transformedX] [varchar](50) NULL,
	[transformedY] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[FileId] ASC,
	[LineNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[CollarDataVectronicPosition] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[LatestVectronicSensorRecordIds]
AS
SELECT idCollar AS CollarId, 'act' AS SensorCode, MAX(idActivity) AS LastId FROM CollarDataVectronicActivity GROUP BY idCollar
UNION ALL
SELECT idCollar AS CollarId, 'mor' AS SensorCode, MAX(idMortality) AS LastId FROM CollarDataVectronicMortality GROUP BY idCollar
UNION ALL
SELECT idCollar AS CollarId, 'gps' AS SensorCode, MAX(idPosition) AS LastId FROM CollarDataVectronicPosition GROUP BY idCollar
GO
GRANT SELECT ON [dbo].[LatestVectronicSensorRecordIds] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VectronicSensorsToRetrieve]
AS
     SELECT C.Manager, C.CollarId, K.CollarKey, S.SensorCode, I.LastId
	   FROM Collars as C
  LEFT JOIN CollarSensors AS S
         ON C.CollarManufacturer = S.CollarManufacturer AND C.CollarId = S.CollarId
  LEFT JOIN CollarKeys AS K
         ON C.CollarManufacturer = K.CollarManufacturer AND C.collarId = K.CollarId
  LEFT JOIN LatestVectronicSensorRecordIds AS I
         ON C.collarId = I.CollarId AND S.SensorCode = I.SensorCode
      WHERE C.CollarManufacturer = 'Vectronic' AND S.IsActive = 1

GO
GRANT SELECT ON [dbo].[VectronicSensorsToRetrieve] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CollarDataTelonicsGen3StoreOnBoard](
	[FileId] [int] NOT NULL,
	[LineNumber] [int] NOT NULL,
	[Fix #] [varchar](50) NULL,
	[Date] [nvarchar](50) NULL,
	[Time] [nvarchar](50) NULL,
	[Fix Status] [varchar](50) NULL,
	[Status Text] [varchar](150) NULL,
	[Velocity East(m s)] [varchar](50) NULL,
	[Velocity North(m s)] [varchar](50) NULL,
	[Velocity Up(m s)] [varchar](50) NULL,
	[Latitude] [varchar](50) NULL,
	[Longitude] [varchar](50) NULL,
	[Altitude(m)] [varchar](50) NULL,
	[PDOP] [varchar](50) NULL,
	[HDOP] [varchar](50) NULL,
	[VDOP] [varchar](50) NULL,
	[TDOP] [varchar](50) NULL,
	[Temperature Sensor(deg )] [varchar](50) NULL,
	[Activity Sensor] [varchar](50) NULL,
	[Satellite Data] [varchar](150) NULL,
 CONSTRAINT [PK_CollarDataTelonicsGen3StoreOnBoard] PRIMARY KEY CLUSTERED 
(
	[FileId] ASC,
	[LineNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[CollarDataTelonicsGen3StoreOnBoard] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[Gen3StoreOnBoardLocations]
AS
SELECT     dbo.CollarDataTelonicsGen3StoreOnBoard.*, dbo.Animals.*, dbo.Locations.Location, dbo.CollarFiles.FileName, dbo.CollarFiles.UserName, 
                      dbo.CollarFiles.UploadDate
FROM         dbo.CollarDataTelonicsGen3StoreOnBoard INNER JOIN
                      dbo.CollarFixes ON dbo.CollarDataTelonicsGen3StoreOnBoard.FileId = dbo.CollarFixes.FileId AND 
                      dbo.CollarDataTelonicsGen3StoreOnBoard.LineNumber = dbo.CollarFixes.LineNumber INNER JOIN
                      dbo.Locations ON dbo.CollarFixes.FixId = dbo.Locations.FixId INNER JOIN
                      dbo.Animals ON dbo.Locations.ProjectId = dbo.Animals.ProjectId AND dbo.Locations.AnimalId = dbo.Animals.AnimalId INNER JOIN
                      dbo.CollarFiles ON dbo.CollarDataTelonicsGen3StoreOnBoard.FileId = dbo.CollarFiles.FileId AND dbo.CollarFixes.FileId = dbo.CollarFiles.FileId

GO
DENY SELECT ON [dbo].[Gen3StoreOnBoardLocations] TO [dog_house] AS [dbo]
GO
GRANT SELECT ON [dbo].[Gen3StoreOnBoardLocations] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArgosDownloads](
	[TimeStamp] [datetime2](7) NOT NULL,
	[ProgramId] [varchar](8) NULL,
	[PlatformId] [varchar](8) NULL,
	[Days] [int] NULL,
	[FileId] [int] NULL,
	[ErrorMessage] [varchar](max) NULL,
 CONSTRAINT [PK_ArgosDownloads] PRIMARY KEY CLUSTERED 
(
	[TimeStamp] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[ArgosDownloads] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArgosPlatforms](
	[PlatformId] [varchar](8) NOT NULL,
	[ProgramId] [varchar](8) NOT NULL,
	[Notes] [nvarchar](max) NULL,
	[Active] [bit] NOT NULL,
	[DisposalDate] [datetime2](7) NULL,
 CONSTRAINT [PK_ArgosPlatforms] PRIMARY KEY CLUSTERED 
(
	[PlatformId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[ArgosPlatforms] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArgosPrograms](
	[ProgramId] [varchar](8) NOT NULL,
	[ProgramName] [nvarchar](255) NULL,
	[UserName] [sysname] NOT NULL,
	[Password] [nvarchar](128) NOT NULL,
	[Manager] [sysname] NOT NULL,
	[StartDate] [datetime2](7) NULL,
	[EndDate] [datetime2](7) NULL,
	[Notes] [nvarchar](max) NULL,
	[Active] [bit] NULL,
 CONSTRAINT [PK_ArgosPrograms] PRIMARY KEY CLUSTERED 
(
	[ProgramId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[ArgosPrograms] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CollarDataArgosEmail](
	[FileId] [int] NOT NULL,
	[LineNumber] [int] NOT NULL,
	[ProgramId] [varchar](50) NULL,
	[PlatformId] [varchar](50) NULL,
	[TransmissionDate] [datetime2](7) NULL,
	[LocationDate] [datetime2](7) NULL,
	[Latitude] [real] NULL,
	[Longitude] [real] NULL,
	[Altitude] [real] NULL,
	[LocationClass] [char](1) NULL,
	[Message] [varbinary](50) NULL,
 CONSTRAINT [PK_CollarDataArgosEmail] PRIMARY KEY CLUSTERED 
(
	[FileId] ASC,
	[LineNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[CollarDataArgosEmail] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CollarDataArgosWebService](
	[FileId] [int] NOT NULL,
	[LineNumber] [int] NOT NULL,
	[programNumber] [varchar](50) NULL,
	[platformId] [varchar](50) NULL,
	[platformType] [varchar](50) NULL,
	[platformModel] [varchar](50) NULL,
	[platformName] [varchar](50) NULL,
	[platformHexId] [varchar](50) NULL,
	[satellite] [varchar](50) NULL,
	[bestMsgDate] [varchar](50) NULL,
	[duration] [varchar](50) NULL,
	[nbMessage] [varchar](50) NULL,
	[message120] [varchar](50) NULL,
	[bestLevel] [varchar](50) NULL,
	[frequency] [varchar](50) NULL,
	[locationDate] [varchar](50) NULL,
	[latitude] [varchar](50) NULL,
	[longitude] [varchar](50) NULL,
	[altitude] [varchar](50) NULL,
	[locationClass] [varchar](50) NULL,
	[gpsSpeed] [varchar](50) NULL,
	[gpsHeading] [varchar](50) NULL,
	[latitude2] [varchar](50) NULL,
	[longitude2] [varchar](50) NULL,
	[altitude2] [varchar](50) NULL,
	[index] [varchar](50) NULL,
	[nopc] [varchar](50) NULL,
	[errorRadius] [varchar](50) NULL,
	[semiMajor] [varchar](50) NULL,
	[semiMinor] [varchar](50) NULL,
	[orientation] [varchar](50) NULL,
	[hdop] [varchar](50) NULL,
	[bestDate] [varchar](50) NULL,
	[compression] [varchar](50) NULL,
	[type] [varchar](50) NULL,
	[alarm] [varchar](50) NULL,
	[concatenated] [varchar](50) NULL,
	[date] [varchar](50) NULL,
	[level] [varchar](50) NULL,
	[doppler] [varchar](50) NULL,
	[rawData] [varchar](500) NULL,
 CONSTRAINT [PK_CollarDataArgosWebService] PRIMARY KEY CLUSTERED 
(
	[FileId] ASC,
	[LineNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[CollarDataArgosWebService] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CollarDataDebevekFormat](
	[FileID] [int] NOT NULL,
	[LineNumber] [int] NOT NULL,
	[PlatformId] [char](6) NULL,
	[AnimalId] [char](6) NULL,
	[Species] [nvarchar](255) NULL,
	[Group] [nvarchar](255) NULL,
	[Park] [nvarchar](255) NULL,
	[FixDate] [nvarchar](255) NULL,
	[FixTime] [nvarchar](255) NULL,
	[FixMonth] [float] NULL,
	[FixDay] [float] NULL,
	[FixYear] [float] NULL,
	[LatWGS84] [float] NULL,
	[LonWGS84] [float] NULL,
	[Temperature] [float] NULL,
	[Other] [nvarchar](255) NULL,
 CONSTRAINT [PK_CollarDataDebevekFormat] PRIMARY KEY CLUSTERED 
(
	[FileID] ASC,
	[LineNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[CollarDataDebevekFormat] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CollarDataLotekIridium](
	[FileId] [int] NOT NULL,
	[LineNumber] [int] NOT NULL,
	[Device_ID] [varchar](500) NULL,
	[DateTime_GMT] [varchar](500) NULL,
	[DateTime_Local] [varchar](500) NULL,
	[Latitude] [varchar](500) NULL,
	[Longitude] [varchar](500) NULL,
	[Altitude] [varchar](500) NULL,
	[Fix_Status] [varchar](500) NULL,
	[DOP] [varchar](500) NULL,
	[Temp_C] [varchar](500) NULL,
	[Main_V] [varchar](500) NULL,
	[Back_V] [varchar](500) NULL,
 CONSTRAINT [PK_CollarDataLotekIridium] PRIMARY KEY CLUSTERED 
(
	[FileId] ASC,
	[LineNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CollarDataTelevilt](
	[FileId] [int] NOT NULL,
	[LineNumber] [int] NOT NULL,
	[FixDate] [varchar](50) NULL,
	[FixTime] [varchar](50) NULL,
	[Lat] [varchar](50) NULL,
	[Lon] [varchar](50) NULL,
	[PDOP] [varchar](50) NULL,
	[Fix] [varchar](50) NULL,
 CONSTRAINT [PK_CollarDataTelevilt] PRIMARY KEY CLUSTERED 
(
	[FileId] ASC,
	[LineNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[CollarDataTelevilt] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CollarDataTelonicsGen3](
	[FileId] [int] NOT NULL,
	[LineNumber] [int] NOT NULL,
	[TXDate] [varchar](50) NULL,
	[TXTime] [varchar](50) NULL,
	[PTTID] [varchar](50) NULL,
	[FixNum] [varchar](50) NULL,
	[FixQual] [varchar](50) NULL,
	[FixDate] [varchar](50) NULL,
	[FixTime] [varchar](50) NULL,
	[Longitude] [varchar](50) NULL,
	[Latitude] [varchar](50) NULL,
 CONSTRAINT [PK_CollarDataTelonicsGen3] PRIMARY KEY CLUSTERED 
(
	[FileId] ASC,
	[LineNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[CollarDataTelonicsGen3] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CollarDataTelonicsGen4](
	[FileId] [int] NOT NULL,
	[LineNumber] [int] NOT NULL,
	[AcquisitionTime] [varchar](50) NULL,
	[AcquisitionStartTime] [varchar](50) NULL,
	[Ctn] [varchar](50) NULL,
	[ArgosId] [varchar](50) NULL,
	[ArgosLocationClass] [varchar](50) NULL,
	[ArgosLatitude] [varchar](50) NULL,
	[ArgosLongitude] [varchar](50) NULL,
	[ArgosAltitude] [varchar](50) NULL,
	[GpsFixTime] [varchar](50) NULL,
	[GpsFixAttempt] [varchar](50) NULL,
	[GpsLatitude] [varchar](50) NULL,
	[GpsLongitude] [varchar](50) NULL,
	[GpsUtmZone] [varchar](50) NULL,
	[GpsUtmNorthing] [varchar](50) NULL,
	[GpsUtmEasting] [varchar](50) NULL,
	[GpsAltitude] [varchar](50) NULL,
	[GpsSpeed] [varchar](50) NULL,
	[GpsHeading] [varchar](50) NULL,
	[GpsHorizontalError] [varchar](50) NULL,
	[GpsPositionalDilution] [varchar](50) NULL,
	[GpsHorizontalDilution] [varchar](50) NULL,
	[GpsSatelliteBitmap] [varchar](50) NULL,
	[GpsSatelliteCount] [varchar](50) NULL,
	[GpsNavigationTime] [varchar](50) NULL,
	[UnderwaterPercentage] [varchar](50) NULL,
	[DiveCount] [varchar](50) NULL,
	[AverageDiveDuration] [varchar](50) NULL,
	[MaximumDiveDuration] [varchar](50) NULL,
	[LayerPercentage] [varchar](50) NULL,
	[MaximumDiveDepth] [varchar](50) NULL,
	[DiveStartTime] [varchar](50) NULL,
	[DiveDuration] [varchar](50) NULL,
	[DiveDepth] [varchar](50) NULL,
	[DiveProfile] [varchar](50) NULL,
	[ActivityCount] [varchar](50) NULL,
	[Temperature] [varchar](50) NULL,
	[RemoteAnalog] [varchar](50) NULL,
	[SatelliteUplink] [varchar](50) NULL,
	[ReceiveTime] [varchar](50) NULL,
	[SatelliteName] [varchar](50) NULL,
	[RepetitionCount] [varchar](50) NULL,
	[LowVoltage] [varchar](50) NULL,
	[Mortality] [varchar](50) NULL,
	[SaltwaterFailsafe] [varchar](50) NULL,
	[HaulOut] [varchar](50) NULL,
	[DigitalInput] [varchar](50) NULL,
	[MotionDetected] [varchar](50) NULL,
	[TrapTriggerTime] [varchar](50) NULL,
	[ReleaseTime] [varchar](50) NULL,
	[PredeploymentData] [varchar](50) NULL,
	[Error] [varchar](250) NULL,
 CONSTRAINT [PK_CollarDataTelonicsGen4] PRIMARY KEY CLUSTERED 
(
	[FileId] ASC,
	[LineNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[CollarDataTelonicsGen4] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GAAR_Bear_Denning](
	[BearNo] [int] NOT NULL,
	[Sex] [nchar](1) NOT NULL,
	[InYear] [int] NOT NULL,
	[OutYear] [int] NOT NULL,
	[DenIn] [datetime2](7) NOT NULL,
	[DenOut] [datetime2](7) NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HomeRangeExclusions](
	[Creator] [nvarchar](100) NOT NULL,
	[FixId] [int] NOT NULL,
	[ExclusionCode] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Creator] ASC,
	[FixId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[HomeRangeExclusions] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HomeRangeVhfExclusions](
	[Creator] [nvarchar](100) NOT NULL,
	[LocationId] [int] NOT NULL,
	[ExclusionCode] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Creator] ASC,
	[LocationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[HomeRangeVhfExclusions] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Locations_In_CartoDB](
	[fixid] [int] NOT NULL,
 CONSTRAINT [PK_Locations_In_CartoDB] PRIMARY KEY CLUSTERED 
(
	[fixid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[Locations_In_CartoDB] TO [INPAKROVMAIS\sql_proxy] AS [dbo]
GO
GRANT INSERT ON [dbo].[Locations_In_CartoDB] TO [INPAKROVMAIS\sql_proxy] AS [dbo]
GO
GRANT SELECT ON [dbo].[Locations_In_CartoDB] TO [INPAKROVMAIS\sql_proxy] AS [dbo]
GO
GRANT UPDATE ON [dbo].[Locations_In_CartoDB] TO [INPAKROVMAIS\sql_proxy] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LookupCollarFileFormats](
	[Code] [char](1) NOT NULL,
	[CollarManufacturer] [varchar](16) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[ArgosData] [char](1) NOT NULL,
	[RequiresCollar] [char](1) NULL,
 CONSTRAINT [PK_CollarFileFormats] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[LookupCollarFileFormats] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LookupCollarFileHeaders](
	[Header] [nvarchar](450) NOT NULL,
	[FileFormat] [char](1) NOT NULL,
	[Regex] [nvarchar](450) NULL,
 CONSTRAINT [PK_CollarFileHeaders] PRIMARY KEY CLUSTERED 
(
	[Header] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[LookupCollarFileHeaders] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LookupCollarModels](
	[CollarManufacturer] [varchar](16) NOT NULL,
	[CollarModel] [varchar](24) NOT NULL,
 CONSTRAINT [PK_LookupCollarModels] PRIMARY KEY CLUSTERED 
(
	[CollarManufacturer] ASC,
	[CollarModel] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[LookupCollarModels] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LookupCollarParameterFileFormats](
	[Code] [char](1) NOT NULL,
	[CollarManufacturer] [varchar](16) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [PK_LookupCollarParameterFileFormats] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[LookupCollarParameterFileFormats] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LookupCollarSensors](
	[CollarManufacturer] [varchar](16) NOT NULL,
	[Code] [varchar](8) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Description] [nvarchar](500) NULL,
 CONSTRAINT [PK_LookupCollarSensors] PRIMARY KEY CLUSTERED 
(
	[CollarManufacturer] ASC,
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[LookupCollarSensors] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LookupGender](
	[Sex] [varchar](7) NOT NULL,
 CONSTRAINT [PK_LookupSex] PRIMARY KEY CLUSTERED 
(
	[Sex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[LookupGender] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LookupHomeRangeExclusionCodes](
	[Code] [int] NOT NULL,
	[Description] [nvarchar](2000) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[LookupHomeRangeExclusionCodes] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LookupQueryLayerServers](
	[Location] [nvarchar](128) NOT NULL,
	[Connection] [nvarchar](255) NOT NULL,
	[Database] [sysname] NULL,
 CONSTRAINT [PK_QueryLayerServers] PRIMARY KEY CLUSTERED 
(
	[Location] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[LookupQueryLayerServers] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LookupSpecies](
	[Species] [varchar](32) NOT NULL,
 CONSTRAINT [PK_LookupSpecies] PRIMARY KEY CLUSTERED 
(
	[Species] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[LookupSpecies] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Movements_In_CartoDB](
	[ProjectId] [varchar](16) NOT NULL,
	[AnimalId] [varchar](16) NOT NULL,
	[StartDate] [datetime2](7) NOT NULL,
	[EndDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Movements_In_CartoDB] PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC,
	[AnimalId] ASC,
	[StartDate] ASC,
	[EndDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[Movements_In_CartoDB] TO [INPAKROVMAIS\sql_proxy] AS [dbo]
GO
GRANT INSERT ON [dbo].[Movements_In_CartoDB] TO [INPAKROVMAIS\sql_proxy] AS [dbo]
GO
GRANT SELECT ON [dbo].[Movements_In_CartoDB] TO [INPAKROVMAIS\sql_proxy] AS [dbo]
GO
GRANT UPDATE ON [dbo].[Movements_In_CartoDB] TO [INPAKROVMAIS\sql_proxy] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectEditors](
	[ProjectId] [varchar](16) NOT NULL,
	[Editor] [sysname] NOT NULL,
 CONSTRAINT [PK_ProjectEditors] PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC,
	[Editor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[ProjectEditors] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectExportBoundaries](
	[Project] [nvarchar](16) NULL,
	[Shape] [geography] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[ProjectExportBoundaries] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectInvestigatorAssistants](
	[ProjectInvestigator] [sysname] NOT NULL,
	[Assistant] [sysname] NOT NULL,
 CONSTRAINT [PK_ProjectInvestigatorAssistants] PRIMARY KEY CLUSTERED 
(
	[ProjectInvestigator] ASC,
	[Assistant] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[ProjectInvestigatorAssistants] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectInvestigators](
	[Login] [sysname] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](200) NOT NULL,
	[Phone] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_ProjectInvestigators] PRIMARY KEY CLUSTERED 
(
	[Login] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[ProjectInvestigators] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Settings](
	[Username] [sysname] NOT NULL,
	[Key] [nvarchar](30) NOT NULL,
	[Value] [nvarchar](500) NULL,
 CONSTRAINT [PK_Settings] PRIMARY KEY CLUSTERED 
(
	[Username] ASC,
	[Key] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[Settings] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VhfLocations](
	[LocationId] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[FileId] [int] NOT NULL,
	[LineNumber] [int] NOT NULL,
	[ProjectId] [varchar](16) NOT NULL,
	[AnimalId] [varchar](16) NOT NULL,
	[Species] [varchar](32) NULL,
	[Gender] [varchar](7) NULL,
	[GroupName] [nvarchar](500) NULL,
	[Description] [nvarchar](2000) NULL,
	[LocalFixDate] [datetime2](7) NOT NULL,
	[Location] [geography] NOT NULL,
 CONSTRAINT [PK_VhfLocations] PRIMARY KEY CLUSTERED 
(
	[LocationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[VhfLocations] TO [Viewer] AS [dbo]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_ArgosDeployments_Collars] ON [dbo].[ArgosDeployments]
(
	[CollarManufacturer] ASC,
	[CollarId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_ArgosDeployments_PlatformId] ON [dbo].[ArgosDeployments]
(
	[PlatformId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_ArgosDownloads_PlatformId] ON [dbo].[ArgosDownloads]
(
	[PlatformId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_ArgosDownloads_ProgamId] ON [dbo].[ArgosDownloads]
(
	[ProgramId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ArgosFilePlatformDates_FileId] ON [dbo].[ArgosFilePlatformDates]
(
	[FileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_ArgosFilePlatformDates_PlatformId] ON [dbo].[ArgosFilePlatformDates]
(
	[PlatformId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ArgosFileProcessingIssues_CollarFiles] ON [dbo].[ArgosFileProcessingIssues]
(
	[FileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_ArgosFileProcessingIssues_Collars] ON [dbo].[ArgosFileProcessingIssues]
(
	[CollarManufacturer] ASC,
	[CollarId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_ArgosFileProcessingIssues_PlatformId] ON [dbo].[ArgosFileProcessingIssues]
(
	[PlatformId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_CollarDataTelonicsGen4_ActivityCount] ON [dbo].[CollarDataTelonicsGen4]
(
	[ActivityCount] ASC
)
INCLUDE([FileId],[AcquisitionStartTime]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_CollarDataTelonicsGen4_FileId_AcquisitionStartTime_Temperature] ON [dbo].[CollarDataTelonicsGen4]
(
	[FileId] ASC,
	[AcquisitionStartTime] ASC,
	[Temperature] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_CollarDataTelonicsGen4_Temperature] ON [dbo].[CollarDataTelonicsGen4]
(
	[Temperature] ASC
)
INCLUDE([FileId],[AcquisitionStartTime]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_CollarDeployments_Animals] ON [dbo].[CollarDeployments]
(
	[ProjectId] ASC,
	[AnimalId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_CollarDeployments_Collars] ON [dbo].[CollarDeployments]
(
	[CollarManufacturer] ASC,
	[CollarId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_CollarFiles_Format] ON [dbo].[CollarFiles]
(
	[Format] ASC
)
INCLUDE([FileId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CollarFiles_ParentFileId] ON [dbo].[CollarFiles]
(
	[ParentFileId] ASC
)
INCLUDE([FileId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_CollarFiles_Status] ON [dbo].[CollarFiles]
(
	[Status] ASC
)
INCLUDE([FileId],[FileName],[CollarManufacturer],[CollarId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_CollarFixes_Collar] ON [dbo].[CollarFixes]
(
	[CollarManufacturer] ASC,
	[CollarId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CollarFixes_FileId] ON [dbo].[CollarFixes]
(
	[FileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CollarFixes_FixDate] ON [dbo].[CollarFixes]
(
	[FixDate] ASC
)
INCLUDE([CollarManufacturer],[CollarId],[Lat],[Lon]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CollarFixes_HiddenBy] ON [dbo].[CollarFixes]
(
	[HiddenBy] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Locations_FixId] ON [dbo].[Locations]
(
	[FixId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_Locations_Status] ON [dbo].[Locations]
(
	[Status] ASC
)
INCLUDE([ProjectId],[AnimalId],[FixDate]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Movements_EndDate] ON [dbo].[Movements]
(
	[ProjectId] ASC,
	[AnimalId] ASC,
	[EndDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Movements_StartDate_EndDate] ON [dbo].[Movements]
(
	[StartDate] ASC,
	[EndDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ArgosDownloads] ADD  CONSTRAINT [DF_ArgosDownloads_TimeStamp]  DEFAULT (getdate()) FOR [TimeStamp]
GO
ALTER TABLE [dbo].[CollarFiles] ADD  CONSTRAINT [DF_CollarFiles_UploadDate]  DEFAULT (getdate()) FOR [UploadDate]
GO
ALTER TABLE [dbo].[CollarFiles] ADD  CONSTRAINT [DF_CollarFiles_UserName]  DEFAULT (original_login()) FOR [UserName]
GO
ALTER TABLE [dbo].[CollarFiles] ADD  CONSTRAINT [DF_CollarFiles_Status]  DEFAULT ('I') FOR [Status]
GO
ALTER TABLE [dbo].[CollarParameterFiles] ADD  CONSTRAINT [DF_CollarParameterFiles_UploadDate]  DEFAULT (getdate()) FOR [UploadDate]
GO
ALTER TABLE [dbo].[CollarParameterFiles] ADD  CONSTRAINT [DF_CollarParameterFiles_UploadUser]  DEFAULT (original_login()) FOR [UploadUser]
GO
ALTER TABLE [dbo].[Collars] ADD  CONSTRAINT [DF_Collars_Manager]  DEFAULT (original_login()) FOR [Manager]
GO
ALTER TABLE [dbo].[Collars] ADD  CONSTRAINT [DF_Collars_Owner]  DEFAULT ('NPS') FOR [Owner]
GO
ALTER TABLE [dbo].[CollarSensors] ADD  CONSTRAINT [DF_CollarSensors_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Projects] ADD  CONSTRAINT [DF_Projects_PrincipalInvestigator]  DEFAULT (original_login()) FOR [ProjectInvestigator]
GO
ALTER TABLE [dbo].[Animals]  WITH CHECK ADD  CONSTRAINT [FK_Animals_Gender] FOREIGN KEY([Gender])
REFERENCES [dbo].[LookupGender] ([Sex])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Animals] CHECK CONSTRAINT [FK_Animals_Gender]
GO
ALTER TABLE [dbo].[Animals]  WITH CHECK ADD  CONSTRAINT [FK_Animals_Projects] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Projects] ([ProjectId])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Animals] CHECK CONSTRAINT [FK_Animals_Projects]
GO
ALTER TABLE [dbo].[Animals]  WITH CHECK ADD  CONSTRAINT [FK_Animals_Species] FOREIGN KEY([Species])
REFERENCES [dbo].[LookupSpecies] ([Species])
GO
ALTER TABLE [dbo].[Animals] CHECK CONSTRAINT [FK_Animals_Species]
GO
ALTER TABLE [dbo].[ArgosDeployments]  WITH CHECK ADD  CONSTRAINT [FK_ArgosDeployments_ArgosPlatforms] FOREIGN KEY([PlatformId])
REFERENCES [dbo].[ArgosPlatforms] ([PlatformId])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[ArgosDeployments] CHECK CONSTRAINT [FK_ArgosDeployments_ArgosPlatforms]
GO
ALTER TABLE [dbo].[ArgosDeployments]  WITH CHECK ADD  CONSTRAINT [FK_ArgosDeployments_Collars] FOREIGN KEY([CollarManufacturer], [CollarId])
REFERENCES [dbo].[Collars] ([CollarManufacturer], [CollarId])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[ArgosDeployments] CHECK CONSTRAINT [FK_ArgosDeployments_Collars]
GO
ALTER TABLE [dbo].[ArgosDownloads]  WITH CHECK ADD  CONSTRAINT [FK_ArgosDownloads_CollarFiles] FOREIGN KEY([FileId])
REFERENCES [dbo].[CollarFiles] ([FileId])
GO
ALTER TABLE [dbo].[ArgosDownloads] CHECK CONSTRAINT [FK_ArgosDownloads_CollarFiles]
GO
ALTER TABLE [dbo].[ArgosFilePlatformDates]  WITH CHECK ADD  CONSTRAINT [FK_ArgosFilePlatformDates_CollarFiles] FOREIGN KEY([FileId])
REFERENCES [dbo].[CollarFiles] ([FileId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ArgosFilePlatformDates] CHECK CONSTRAINT [FK_ArgosFilePlatformDates_CollarFiles]
GO
ALTER TABLE [dbo].[ArgosFileProcessingIssues]  WITH CHECK ADD  CONSTRAINT [FK_ArgosFileProcessingIssues_CollarFiles] FOREIGN KEY([FileId])
REFERENCES [dbo].[CollarFiles] ([FileId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ArgosFileProcessingIssues] CHECK CONSTRAINT [FK_ArgosFileProcessingIssues_CollarFiles]
GO
ALTER TABLE [dbo].[ArgosFileProcessingIssues]  WITH CHECK ADD  CONSTRAINT [FK_ArgosFileProcessingIssues_Collars] FOREIGN KEY([CollarManufacturer], [CollarId])
REFERENCES [dbo].[Collars] ([CollarManufacturer], [CollarId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ArgosFileProcessingIssues] CHECK CONSTRAINT [FK_ArgosFileProcessingIssues_Collars]
GO
ALTER TABLE [dbo].[ArgosPlatforms]  WITH CHECK ADD  CONSTRAINT [FK_ArgosPlatforms_ArgosPrograms] FOREIGN KEY([ProgramId])
REFERENCES [dbo].[ArgosPrograms] ([ProgramId])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[ArgosPlatforms] CHECK CONSTRAINT [FK_ArgosPlatforms_ArgosPrograms]
GO
ALTER TABLE [dbo].[ArgosPrograms]  WITH CHECK ADD  CONSTRAINT [FK_ArgosPrograms_ProjectInvestigators] FOREIGN KEY([Manager])
REFERENCES [dbo].[ProjectInvestigators] ([Login])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[ArgosPrograms] CHECK CONSTRAINT [FK_ArgosPrograms_ProjectInvestigators]
GO
ALTER TABLE [dbo].[CollarDataArgosEmail]  WITH CHECK ADD  CONSTRAINT [FK_CollarDataArgosEmail_CollarFiles] FOREIGN KEY([FileId])
REFERENCES [dbo].[CollarFiles] ([FileId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CollarDataArgosEmail] CHECK CONSTRAINT [FK_CollarDataArgosEmail_CollarFiles]
GO
ALTER TABLE [dbo].[CollarDataArgosWebService]  WITH CHECK ADD  CONSTRAINT [FK_CollarDataArgosWebService_CollarFiles] FOREIGN KEY([FileId])
REFERENCES [dbo].[CollarFiles] ([FileId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CollarDataArgosWebService] CHECK CONSTRAINT [FK_CollarDataArgosWebService_CollarFiles]
GO
ALTER TABLE [dbo].[CollarDataDebevekFormat]  WITH CHECK ADD  CONSTRAINT [FK_CollarDataDebevekFormat_CollarFiles] FOREIGN KEY([FileID])
REFERENCES [dbo].[CollarFiles] ([FileId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CollarDataDebevekFormat] CHECK CONSTRAINT [FK_CollarDataDebevekFormat_CollarFiles]
GO
ALTER TABLE [dbo].[CollarDataIridiumMail]  WITH CHECK ADD  CONSTRAINT [FK_CollarDataIridiumMail_CollarFiles] FOREIGN KEY([FileId])
REFERENCES [dbo].[CollarFiles] ([FileId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CollarDataIridiumMail] CHECK CONSTRAINT [FK_CollarDataIridiumMail_CollarFiles]
GO
ALTER TABLE [dbo].[CollarDataTelevilt]  WITH CHECK ADD  CONSTRAINT [FK_CollarDataTelevilt_CollarFiles] FOREIGN KEY([FileId])
REFERENCES [dbo].[CollarFiles] ([FileId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CollarDataTelevilt] CHECK CONSTRAINT [FK_CollarDataTelevilt_CollarFiles]
GO
ALTER TABLE [dbo].[CollarDataTelonicsGen3]  WITH CHECK ADD  CONSTRAINT [FK_CollarDataTelonicsGen3_CollarFiles] FOREIGN KEY([FileId])
REFERENCES [dbo].[CollarFiles] ([FileId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CollarDataTelonicsGen3] CHECK CONSTRAINT [FK_CollarDataTelonicsGen3_CollarFiles]
GO
ALTER TABLE [dbo].[CollarDataTelonicsGen3StoreOnBoard]  WITH CHECK ADD  CONSTRAINT [FK_CollarDataTelonicsGen3StoreOnBoard_CollarFiles] FOREIGN KEY([FileId])
REFERENCES [dbo].[CollarFiles] ([FileId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CollarDataTelonicsGen3StoreOnBoard] CHECK CONSTRAINT [FK_CollarDataTelonicsGen3StoreOnBoard_CollarFiles]
GO
ALTER TABLE [dbo].[CollarDataTelonicsGen4]  WITH CHECK ADD  CONSTRAINT [FK_CollarDataTelonicsGen4_CollarFiles] FOREIGN KEY([FileId])
REFERENCES [dbo].[CollarFiles] ([FileId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CollarDataTelonicsGen4] CHECK CONSTRAINT [FK_CollarDataTelonicsGen4_CollarFiles]
GO
ALTER TABLE [dbo].[CollarDataVectronicActivity]  WITH CHECK ADD  CONSTRAINT [FK_CollarDataVectronicActivity_CollarFiles] FOREIGN KEY([FileId])
REFERENCES [dbo].[CollarFiles] ([FileId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CollarDataVectronicActivity] CHECK CONSTRAINT [FK_CollarDataVectronicActivity_CollarFiles]
GO
ALTER TABLE [dbo].[CollarDataVectronicMortality]  WITH CHECK ADD  CONSTRAINT [FK_CollarDataVectronicMortality_CollarFiles] FOREIGN KEY([FileId])
REFERENCES [dbo].[CollarFiles] ([FileId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CollarDataVectronicMortality] CHECK CONSTRAINT [FK_CollarDataVectronicMortality_CollarFiles]
GO
ALTER TABLE [dbo].[CollarDataVectronicPosition]  WITH CHECK ADD  CONSTRAINT [FK_CollarDataVectronicPosition_CollarFiles] FOREIGN KEY([FileId])
REFERENCES [dbo].[CollarFiles] ([FileId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CollarDataVectronicPosition] CHECK CONSTRAINT [FK_CollarDataVectronicPosition_CollarFiles]
GO
ALTER TABLE [dbo].[CollarDeployments]  WITH CHECK ADD  CONSTRAINT [FK_CollarDeployments_Animals] FOREIGN KEY([ProjectId], [AnimalId])
REFERENCES [dbo].[Animals] ([ProjectId], [AnimalId])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[CollarDeployments] CHECK CONSTRAINT [FK_CollarDeployments_Animals]
GO
ALTER TABLE [dbo].[CollarDeployments]  WITH CHECK ADD  CONSTRAINT [FK_CollarDeployments_Collars] FOREIGN KEY([CollarManufacturer], [CollarId])
REFERENCES [dbo].[Collars] ([CollarManufacturer], [CollarId])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[CollarDeployments] CHECK CONSTRAINT [FK_CollarDeployments_Collars]
GO
ALTER TABLE [dbo].[CollarFiles]  WITH CHECK ADD  CONSTRAINT [FK_CollarFiles_ArgosDeployments] FOREIGN KEY([ArgosDeploymentId])
REFERENCES [dbo].[ArgosDeployments] ([DeploymentId])
GO
ALTER TABLE [dbo].[CollarFiles] CHECK CONSTRAINT [FK_CollarFiles_ArgosDeployments]
GO
ALTER TABLE [dbo].[CollarFiles]  WITH CHECK ADD  CONSTRAINT [FK_CollarFiles_CollarFiles] FOREIGN KEY([ParentFileId])
REFERENCES [dbo].[CollarFiles] ([FileId])
GO
ALTER TABLE [dbo].[CollarFiles] CHECK CONSTRAINT [FK_CollarFiles_CollarFiles]
GO
ALTER TABLE [dbo].[CollarFiles]  WITH CHECK ADD  CONSTRAINT [FK_CollarFiles_CollarParameters] FOREIGN KEY([CollarParameterId])
REFERENCES [dbo].[CollarParameters] ([ParameterId])
GO
ALTER TABLE [dbo].[CollarFiles] CHECK CONSTRAINT [FK_CollarFiles_CollarParameters]
GO
ALTER TABLE [dbo].[CollarFiles]  WITH CHECK ADD  CONSTRAINT [FK_CollarFiles_Collars] FOREIGN KEY([CollarManufacturer], [CollarId])
REFERENCES [dbo].[Collars] ([CollarManufacturer], [CollarId])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[CollarFiles] CHECK CONSTRAINT [FK_CollarFiles_Collars]
GO
ALTER TABLE [dbo].[CollarFiles]  WITH CHECK ADD  CONSTRAINT [FK_CollarFiles_LookupCollarFileFormats] FOREIGN KEY([Format])
REFERENCES [dbo].[LookupCollarFileFormats] ([Code])
GO
ALTER TABLE [dbo].[CollarFiles] CHECK CONSTRAINT [FK_CollarFiles_LookupCollarFileFormats]
GO
ALTER TABLE [dbo].[CollarFiles]  WITH CHECK ADD  CONSTRAINT [FK_CollarFiles_LookupFileStatus] FOREIGN KEY([Status])
REFERENCES [dbo].[LookupFileStatus] ([Code])
GO
ALTER TABLE [dbo].[CollarFiles] CHECK CONSTRAINT [FK_CollarFiles_LookupFileStatus]
GO
ALTER TABLE [dbo].[CollarFiles]  WITH CHECK ADD  CONSTRAINT [FK_CollarFiles_ProjectInvestigators] FOREIGN KEY([Owner])
REFERENCES [dbo].[ProjectInvestigators] ([Login])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[CollarFiles] CHECK CONSTRAINT [FK_CollarFiles_ProjectInvestigators]
GO
ALTER TABLE [dbo].[CollarFiles]  WITH CHECK ADD  CONSTRAINT [FK_CollarFiles_Projects] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Projects] ([ProjectId])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[CollarFiles] CHECK CONSTRAINT [FK_CollarFiles_Projects]
GO
ALTER TABLE [dbo].[CollarFixes]  WITH CHECK ADD  CONSTRAINT [FK_CollarFixes_CollarFiles] FOREIGN KEY([FileId])
REFERENCES [dbo].[CollarFiles] ([FileId])
GO
ALTER TABLE [dbo].[CollarFixes] CHECK CONSTRAINT [FK_CollarFixes_CollarFiles]
GO
ALTER TABLE [dbo].[CollarFixes]  WITH CHECK ADD  CONSTRAINT [FK_CollarFixes_Collars] FOREIGN KEY([CollarManufacturer], [CollarId])
REFERENCES [dbo].[Collars] ([CollarManufacturer], [CollarId])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[CollarFixes] CHECK CONSTRAINT [FK_CollarFixes_Collars]
GO
ALTER TABLE [dbo].[CollarKeys]  WITH CHECK ADD  CONSTRAINT [FK_CollarKeys_Collars] FOREIGN KEY([CollarManufacturer], [CollarId])
REFERENCES [dbo].[Collars] ([CollarManufacturer], [CollarId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CollarKeys] CHECK CONSTRAINT [FK_CollarKeys_Collars]
GO
ALTER TABLE [dbo].[CollarParameterFiles]  WITH CHECK ADD  CONSTRAINT [FK_CollarParameterFiles_LookupFileStatus] FOREIGN KEY([Status])
REFERENCES [dbo].[LookupFileStatus] ([Code])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[CollarParameterFiles] CHECK CONSTRAINT [FK_CollarParameterFiles_LookupFileStatus]
GO
ALTER TABLE [dbo].[CollarParameterFiles]  WITH CHECK ADD  CONSTRAINT [FK_CollarParameterFiles_LookupParameterFileFormats] FOREIGN KEY([Format])
REFERENCES [dbo].[LookupCollarParameterFileFormats] ([Code])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[CollarParameterFiles] CHECK CONSTRAINT [FK_CollarParameterFiles_LookupParameterFileFormats]
GO
ALTER TABLE [dbo].[CollarParameterFiles]  WITH CHECK ADD  CONSTRAINT [FK_CollarParameterFiles_ProjectInvestigators] FOREIGN KEY([Owner])
REFERENCES [dbo].[ProjectInvestigators] ([Login])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CollarParameterFiles] CHECK CONSTRAINT [FK_CollarParameterFiles_ProjectInvestigators]
GO
ALTER TABLE [dbo].[CollarParameters]  WITH CHECK ADD  CONSTRAINT [FK_CollarParameters_CollarParameterFiles] FOREIGN KEY([FileId])
REFERENCES [dbo].[CollarParameterFiles] ([FileId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CollarParameters] CHECK CONSTRAINT [FK_CollarParameters_CollarParameterFiles]
GO
ALTER TABLE [dbo].[CollarParameters]  WITH CHECK ADD  CONSTRAINT [FK_CollarParameters_Collars] FOREIGN KEY([CollarManufacturer], [CollarId])
REFERENCES [dbo].[Collars] ([CollarManufacturer], [CollarId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CollarParameters] CHECK CONSTRAINT [FK_CollarParameters_Collars]
GO
ALTER TABLE [dbo].[Collars]  WITH CHECK ADD  CONSTRAINT [FK_Collars_LookupCollarManufacturers] FOREIGN KEY([CollarManufacturer])
REFERENCES [dbo].[LookupCollarManufacturers] ([CollarManufacturer])
GO
ALTER TABLE [dbo].[Collars] CHECK CONSTRAINT [FK_Collars_LookupCollarManufacturers]
GO
ALTER TABLE [dbo].[Collars]  WITH CHECK ADD  CONSTRAINT [FK_Collars_LookupCollarModels] FOREIGN KEY([CollarManufacturer], [CollarModel])
REFERENCES [dbo].[LookupCollarModels] ([CollarManufacturer], [CollarModel])
GO
ALTER TABLE [dbo].[Collars] CHECK CONSTRAINT [FK_Collars_LookupCollarModels]
GO
ALTER TABLE [dbo].[Collars]  WITH CHECK ADD  CONSTRAINT [FK_Collars_Managers] FOREIGN KEY([Manager])
REFERENCES [dbo].[ProjectInvestigators] ([Login])
GO
ALTER TABLE [dbo].[Collars] CHECK CONSTRAINT [FK_Collars_Managers]
GO
ALTER TABLE [dbo].[CollarSensors]  WITH CHECK ADD  CONSTRAINT [FK_CollarSensors_Collars] FOREIGN KEY([CollarManufacturer], [CollarId])
REFERENCES [dbo].[Collars] ([CollarManufacturer], [CollarId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CollarSensors] CHECK CONSTRAINT [FK_CollarSensors_Collars]
GO
ALTER TABLE [dbo].[CollarSensors]  WITH CHECK ADD  CONSTRAINT [FK_CollarSensors_LookupCollarSensors] FOREIGN KEY([CollarManufacturer], [SensorCode])
REFERENCES [dbo].[LookupCollarSensors] ([CollarManufacturer], [Code])
GO
ALTER TABLE [dbo].[CollarSensors] CHECK CONSTRAINT [FK_CollarSensors_LookupCollarSensors]
GO
ALTER TABLE [dbo].[HomeRangeExclusions]  WITH CHECK ADD  CONSTRAINT [FK_HomeRangeExclusions_CollarFixes] FOREIGN KEY([FixId])
REFERENCES [dbo].[CollarFixes] ([FixId])
GO
ALTER TABLE [dbo].[HomeRangeExclusions] CHECK CONSTRAINT [FK_HomeRangeExclusions_CollarFixes]
GO
ALTER TABLE [dbo].[HomeRangeExclusions]  WITH CHECK ADD  CONSTRAINT [FK_HomeRangeExclusions_LookupHomeRangeExclusionCodes] FOREIGN KEY([ExclusionCode])
REFERENCES [dbo].[LookupHomeRangeExclusionCodes] ([Code])
GO
ALTER TABLE [dbo].[HomeRangeExclusions] CHECK CONSTRAINT [FK_HomeRangeExclusions_LookupHomeRangeExclusionCodes]
GO
ALTER TABLE [dbo].[HomeRangeVhfExclusions]  WITH CHECK ADD  CONSTRAINT [FK_HomeRangeVhfExclusions_LookupHomeRangeExclusionCodes] FOREIGN KEY([ExclusionCode])
REFERENCES [dbo].[LookupHomeRangeExclusionCodes] ([Code])
GO
ALTER TABLE [dbo].[HomeRangeVhfExclusions] CHECK CONSTRAINT [FK_HomeRangeVhfExclusions_LookupHomeRangeExclusionCodes]
GO
ALTER TABLE [dbo].[HomeRangeVhfExclusions]  WITH CHECK ADD  CONSTRAINT [FK_HomeRangeVhfExclusions_VhfLocations] FOREIGN KEY([LocationId])
REFERENCES [dbo].[VhfLocations] ([LocationId])
GO
ALTER TABLE [dbo].[HomeRangeVhfExclusions] CHECK CONSTRAINT [FK_HomeRangeVhfExclusions_VhfLocations]
GO
ALTER TABLE [dbo].[Locations]  WITH CHECK ADD  CONSTRAINT [FK_Locations_Animals] FOREIGN KEY([ProjectId], [AnimalId])
REFERENCES [dbo].[Animals] ([ProjectId], [AnimalId])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Locations] CHECK CONSTRAINT [FK_Locations_Animals]
GO
ALTER TABLE [dbo].[Locations]  WITH CHECK ADD  CONSTRAINT [FK_Locations_CollarFixes] FOREIGN KEY([FixId])
REFERENCES [dbo].[CollarFixes] ([FixId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Locations] CHECK CONSTRAINT [FK_Locations_CollarFixes]
GO
ALTER TABLE [dbo].[LookupCollarFileFormats]  WITH CHECK ADD  CONSTRAINT [FK_LookupCollarFileFormats_LookupCollarManufacturer] FOREIGN KEY([CollarManufacturer])
REFERENCES [dbo].[LookupCollarManufacturers] ([CollarManufacturer])
GO
ALTER TABLE [dbo].[LookupCollarFileFormats] CHECK CONSTRAINT [FK_LookupCollarFileFormats_LookupCollarManufacturer]
GO
ALTER TABLE [dbo].[LookupCollarFileHeaders]  WITH CHECK ADD  CONSTRAINT [FK_CollarFileHeaders_LookupCollarFileFormats] FOREIGN KEY([FileFormat])
REFERENCES [dbo].[LookupCollarFileFormats] ([Code])
GO
ALTER TABLE [dbo].[LookupCollarFileHeaders] CHECK CONSTRAINT [FK_CollarFileHeaders_LookupCollarFileFormats]
GO
ALTER TABLE [dbo].[LookupCollarModels]  WITH CHECK ADD  CONSTRAINT [FK_LookupCollarModels_LookupCollarManufacturers] FOREIGN KEY([CollarManufacturer])
REFERENCES [dbo].[LookupCollarManufacturers] ([CollarManufacturer])
GO
ALTER TABLE [dbo].[LookupCollarModels] CHECK CONSTRAINT [FK_LookupCollarModels_LookupCollarManufacturers]
GO
ALTER TABLE [dbo].[LookupCollarParameterFileFormats]  WITH CHECK ADD  CONSTRAINT [FK_LookupCollarParameterFileFormats_LookupCollarManufacturer] FOREIGN KEY([CollarManufacturer])
REFERENCES [dbo].[LookupCollarManufacturers] ([CollarManufacturer])
GO
ALTER TABLE [dbo].[LookupCollarParameterFileFormats] CHECK CONSTRAINT [FK_LookupCollarParameterFileFormats_LookupCollarManufacturer]
GO
ALTER TABLE [dbo].[LookupCollarSensors]  WITH CHECK ADD  CONSTRAINT [FK_LookupCollarSensors_LookupCollarManufacturers] FOREIGN KEY([CollarManufacturer])
REFERENCES [dbo].[LookupCollarManufacturers] ([CollarManufacturer])
GO
ALTER TABLE [dbo].[LookupCollarSensors] CHECK CONSTRAINT [FK_LookupCollarSensors_LookupCollarManufacturers]
GO
ALTER TABLE [dbo].[Movements]  WITH CHECK ADD  CONSTRAINT [FK_Movements_Animals] FOREIGN KEY([ProjectId], [AnimalId])
REFERENCES [dbo].[Animals] ([ProjectId], [AnimalId])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Movements] CHECK CONSTRAINT [FK_Movements_Animals]
GO
ALTER TABLE [dbo].[ProjectEditors]  WITH CHECK ADD  CONSTRAINT [FK_ProjectEditors_Projects] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Projects] ([ProjectId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProjectEditors] CHECK CONSTRAINT [FK_ProjectEditors_Projects]
GO
ALTER TABLE [dbo].[ProjectInvestigatorAssistants]  WITH CHECK ADD  CONSTRAINT [FK_ProjectInvestigatorAssistants_ProjectInvestigators] FOREIGN KEY([ProjectInvestigator])
REFERENCES [dbo].[ProjectInvestigators] ([Login])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProjectInvestigatorAssistants] CHECK CONSTRAINT [FK_ProjectInvestigatorAssistants_ProjectInvestigators]
GO
ALTER TABLE [dbo].[Projects]  WITH CHECK ADD  CONSTRAINT [FK_Projects_ProjectInvestigators] FOREIGN KEY([ProjectInvestigator])
REFERENCES [dbo].[ProjectInvestigators] ([Login])
GO
ALTER TABLE [dbo].[Projects] CHECK CONSTRAINT [FK_Projects_ProjectInvestigators]
GO
ALTER TABLE [dbo].[VhfLocations]  WITH CHECK ADD  CONSTRAINT [FK_VhfLocations_CollarFiles] FOREIGN KEY([FileId])
REFERENCES [dbo].[CollarFiles] ([FileId])
GO
ALTER TABLE [dbo].[VhfLocations] CHECK CONSTRAINT [FK_VhfLocations_CollarFiles]
GO
ALTER TABLE [dbo].[LookupCollarFileFormats]  WITH CHECK ADD  CONSTRAINT [CK_LookupCollarFileFormats] CHECK  (([ArgosData]='Y' OR [ArgosData]='N'))
GO
ALTER TABLE [dbo].[LookupCollarFileFormats] CHECK CONSTRAINT [CK_LookupCollarFileFormats]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Regan Sarwas
-- Create date: March 2, 2012
-- Description:	Deletes an animal
-- =============================================
CREATE PROCEDURE [dbo].[Animal_Delete] 
	@ProjectId nvarchar(255) = NULL,
	@AnimalId nvarchar(255) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	-- Get the name of the caller
	DECLARE @Caller sysname = ORIGINAL_LOGIN();

	-- Validate permission for this operation
	-- The caller must be able to edit this project
	IF NOT EXISTS (SELECT 1 WHERE [dbo].[IsProjectEditor](@ProjectId, @Caller) = 1)
	BEGIN
		DECLARE @message1 nvarchar(200) = 'You ('+@Caller+') must be the principal investigator or editor on this project ('+@ProjectId+') to delete an animal.';
		RAISERROR(@message1, 18, 0)
		RETURN (1)
	END

	-- deleting a non-existing animal will silently succeed.
	-- All other verification is handled by primary/foreign key and column constraints.
	DELETE FROM dbo.Animals WHERE [ProjectId] = @ProjectId AND [AnimalId] = @AnimalId
END





GO
GRANT EXECUTE ON [dbo].[Animal_Delete] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Regan Sarwas
-- Create date: March 28, 2012
-- Description:	Adds a new animal to a project.
-- =============================================
CREATE PROCEDURE [dbo].[Animal_Insert] 
	@ProjectId NVARCHAR(255)= NULL,
	@AnimalId NVARCHAR(255) = NULL, 
	@Species NVARCHAR(255) = NULL, 
	@Gender NVARCHAR(7) = 'Unknown', 
	@MortalityDate DATETIME2(7) = NULL, 
	@GroupName NVARCHAR(255) = NULL, 
	@Description NVARCHAR(4000) = NULL 
AS
BEGIN
	SET NOCOUNT ON;
	
	-- Get the name of the caller
	DECLARE @Caller sysname = ORIGINAL_LOGIN();

	-- Validate permission for this operation
	-- The caller must be authorized to edit the project
	IF dbo.IsProjectEditor(@ProjectId, @Caller) = 0
	BEGIN
		DECLARE @message1 nvarchar(200) = 'You ('+@Caller+') must be the principal investigator, assistant or editor on this project ('+@ProjectId+') to add an animal.';
		RAISERROR(@message1, 18, 0)
		RETURN (1)
	END
	
	IF @Gender IS NULL OR @Gender = ''
		SET @Gender = 'Unknown'
	
	--All other verification is handled by primary/foreign key and column constraints.
	INSERT INTO dbo.Animals ([ProjectId], [AnimalId], [Species], [Gender], [MortalityDate], [GroupName], [Description])
		 VALUES (nullif(@ProjectId,''), nullif(@AnimalId,''), nullif(@Species,''), @Gender,
		         @MortalityDate, nullif(@GroupName,''), nullif(@Description,''));

END





GO
GRANT EXECUTE ON [dbo].[Animal_Insert] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Regan Sarwas
-- Create date: March 28, 2012
-- Description:	Updates an Animal
-- =============================================
CREATE PROCEDURE [dbo].[Animal_Update] 
	@ProjectId NVARCHAR(255)= NULL,
	@AnimalId NVARCHAR(255) = NULL, 
	@Species NVARCHAR(255) = NULL, 
	@Gender NVARCHAR(7) = NULL, 
	@MortalityDate DATETIME2(7),
	@GroupName NVARCHAR(255) = NULL, 
	@Description NVARCHAR(4000) = NULL 
AS
BEGIN
	SET NOCOUNT ON;

	-- Get the name of the caller
	DECLARE @Caller sysname = ORIGINAL_LOGIN();

	-- Validate permission for this operation
	-- The caller must be authorized to edit the project
	IF dbo.IsProjectEditor(@ProjectId, @Caller) = 0
	BEGIN
		DECLARE @message1 nvarchar(200) = 'You ('+@Caller+') must be the principal investigator, assistant or editor on this project ('+@ProjectId+') to update an animal.';
		RAISERROR(@message1, 18, 0)
		RETURN (1)
	END

	-- Verify this is an existing animal
	-- Otherwise, the update will silently succeed, which could be confusing.
	IF NOT EXISTS (SELECT 1 FROM [dbo].[Animals] WHERE [ProjectId] = @ProjectId AND [AnimalId] = @AnimalId)
	BEGIN
		DECLARE @message2 nvarchar(100) = 'There is no such animal (' + @AnimalId + ') in project (' + @ProjectId + ').';
		RAISERROR(@message2, 18, 0)
		RETURN (1)
	END
		
	-- If a parameter is not provided, use the existing value.
	-- (to put null in a field the user will need to pass an empty string)
	IF @Species IS NULL
	BEGIN
		SELECT @Species = [Species] FROM [dbo].[Animals] WHERE [ProjectId] = @ProjectId AND [AnimalId] = @AnimalId;
	END
	
	IF @Gender IS NULL
	BEGIN
		SELECT @Gender = [Gender] FROM [dbo].[Animals] WHERE [ProjectId] = @ProjectId AND [AnimalId] = @AnimalId;
	END
	
	-- There is no default value for mortality date, since we can't pass '' to express null,
	
	IF @GroupName IS NULL
	BEGIN
		SELECT @GroupName = [GroupName] FROM [dbo].[Animals] WHERE [ProjectId] = @ProjectId AND [AnimalId] = @AnimalId;
	END
	
	IF @Description IS NULL
	BEGIN
		SELECT @Description = [Description] FROM [dbo].[Animals] WHERE [ProjectId] = @ProjectId AND [AnimalId] = @AnimalId;
	END
	
	-- Do the update, replacing empty strings with NULLs
	-- All other verification is handled by primary/foreign key and column constraints.

	UPDATE dbo.Animals SET [Species] = nullif(@Species,''),
						   [Gender] = nullif(@Gender,''),
						   [GroupName] = nullif(@GroupName,''),
						   [MortalityDate] = @MortalityDate,
						   [Description] = nullif(@Description,'')
					 WHERE [ProjectId] = @ProjectId AND [AnimalId] = @AnimalId;

END





GO
GRANT EXECUTE ON [dbo].[Animal_Update] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Regan Sarwas
-- Create date: March 21, 2013
-- Description: Deletes an Argos Deployment
-- =============================================
CREATE PROCEDURE [dbo].[ArgosDeployment_Delete] 
    @DeploymentId int 
AS
BEGIN
    SET NOCOUNT ON;

    -- Get the name of the caller
    DECLARE @Caller sysname = ORIGINAL_LOGIN();

    DECLARE @OldCollarOwner sysname
    DECLARE @OldPlatformOwner sysname
    
    -- Check that this is a valid deployment
    --   this is done now to get the old owner and avoid a confusing permission error	
        SELECT @OldCollarOwner = C.Manager, @OldPlatformOwner = P.Manager
          FROM [dbo].[ArgosDeployments] AS D
          JOIN [dbo].[Collars] AS C ON C.CollarManufacturer = D.CollarManufacturer AND C.CollarId = D.CollarId
          JOIN [dbo].[ArgosPlatforms] AS A ON A.PlatformId = D.PlatformId
          JOIN [dbo].[ArgosPrograms] AS P ON P.ProgramId = A.ProgramId
         WHERE [DeploymentId] = @DeploymentId
    IF @OldCollarOwner IS NULL
        RETURN -- quietly, there is nothing to delete, so we have finished our task.


	-- Caller must be the Manager or Assistant of the Collar
    IF (@OldCollarOwner != @Caller
        AND NOT EXISTS (SELECT 1
                          FROM ProjectInvestigatorAssistants
                         WHERE Assistant = @Caller AND ProjectInvestigator = @OldCollarOwner))
    BEGIN
        DECLARE @message1 nvarchar(100) = 'You ('+@Caller+') are not the manager (or assistant) of the Collar'
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END
	
	-- Caller must be the Manager or Assistant of the Platform's Program
    IF @OldPlatformOwner != @Caller
       AND NOT EXISTS (SELECT 1
                         FROM ProjectInvestigatorAssistants
                        WHERE Assistant = @Caller AND ProjectInvestigator = @OldPlatformOwner)
    BEGIN
        DECLARE @message2 nvarchar(100) = 'You ('+@Caller+') are not the manager (or assistant) of Argos Platform'
        RAISERROR(@message2, 18, 0)
        RETURN 2
    END


    DELETE FROM dbo.ArgosDeployments WHERE [DeploymentId] = @DeploymentId

END


GO
GRANT EXECUTE ON [dbo].[ArgosDeployment_Delete] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Regan Sarwas
-- Create date: March 21, 2013
-- Description:	Adds a new Argos Deployment
-- =============================================
CREATE PROCEDURE [dbo].[ArgosDeployment_Insert] 
    @PlatformId NVARCHAR(255),
    @CollarManufacturer NVARCHAR(255), 
    @CollarId NVARCHAR(255), 
    @StartDate DATETIME2(7) = NULL,
    @EndDate DATETIME2(7) = NULL,
    @DeploymentId INT OUTPUT 
AS
BEGIN
    SET NOCOUNT ON;

    -- Get the name of the caller
    DECLARE @Caller sysname = ORIGINAL_LOGIN();

    DECLARE @NewCollarOwner sysname
    DECLARE @NewPlatformOwner sysname

    -- Validate permission for this operation
    
    -- Caller must be an editor, managed by permissions on SP

	-- Caller must be the Manager or Assistant of the Collar
    SELECT @NewCollarOwner = Manager FROM [dbo].[Collars] WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId
    IF (@NewCollarOwner != @Caller
        AND NOT EXISTS (SELECT 1
                          FROM ProjectInvestigatorAssistants
                         WHERE Assistant = @Caller AND ProjectInvestigator = @NewCollarOwner))
    BEGIN
        DECLARE @message1 nvarchar(100) = 'You ('+@Caller+') are not the manager (or assistant) of the Collar ' + @CollarManufacturer + '/' + @CollarId;
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END
	
	-- Caller must be the Manager or Assistant of the Platform's Program
    SELECT @NewPlatformOwner = Manager FROM [dbo].[ArgosPlatforms] AS A
                                       JOIN [dbo].[ArgosPrograms] AS P ON P.ProgramId = A.ProgramId
                                      WHERE A.PlatformId = @NewPlatformOwner
    IF @NewPlatformOwner != @Caller
       AND NOT EXISTS (SELECT 1
                         FROM ProjectInvestigatorAssistants
                        WHERE Assistant = @Caller AND ProjectInvestigator = @NewPlatformOwner)
    BEGIN
        DECLARE @message2 nvarchar(100) = 'You ('+@Caller+') are not the manager (or assistant) of Argos Platform ' + @PlatformId;
        RAISERROR(@message2, 18, 0)
        RETURN 2
    END


    INSERT INTO [dbo].[ArgosDeployments] ([PlatformId], [CollarManufacturer], [CollarId], [StartDate], [EndDate])
                                  VALUES (@PlatformId,  @CollarManufacturer,  @CollarId,  @StartDate,  @EndDate)
    SET @DeploymentId = SCOPE_IDENTITY()
END

GO
GRANT EXECUTE ON [dbo].[ArgosDeployment_Insert] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Regan Sarwas
-- Create date: March 21, 2013
-- Description: Update an Argos Deployment
-- =============================================
CREATE PROCEDURE [dbo].[ArgosDeployment_Update] 
    @DeploymentId       INT, 
    @PlatformId         NVARCHAR(255) = NULL,
    @CollarManufacturer NVARCHAR(255) = NULL, 
    @CollarId           NVARCHAR(255) = NULL, 
    @StartDate          DATETIME2(7),
    @EndDate            DATETIME2(7) 
AS
BEGIN
    SET NOCOUNT ON;

    -- Get the name of the caller
    DECLARE @Caller sysname = ORIGINAL_LOGIN();

    DECLARE @OldCollarOwner sysname
    DECLARE @OldPlatformOwner sysname
    DECLARE @NewCollarOwner sysname
    DECLARE @NewPlatformOwner sysname
    
    -- Check that this is a valid deployment
    --   this is done now to get the old owner and avoid a confusing permission error	
        SELECT @OldCollarOwner = C.Manager, @OldPlatformOwner = P.Manager
          FROM [dbo].[ArgosDeployments] AS D
          JOIN [dbo].[Collars] AS C ON C.CollarManufacturer = D.CollarManufacturer AND C.CollarId = D.CollarId
          JOIN [dbo].[ArgosPlatforms] AS A ON A.PlatformId = D.PlatformId
          JOIN [dbo].[ArgosPrograms] AS P ON P.ProgramId = A.ProgramId
         WHERE [DeploymentId] = @DeploymentId
    IF @OldCollarOwner IS NULL
    BEGIN
        DECLARE @message1 nvarchar(100) = 'ArgosDeployment ' + CONVERT(nvarchar(10),@DeploymentId) + ' does not exist.';
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END


	-- Caller must be the Manager or Assistant of the old Collar
    IF (@OldCollarOwner != @Caller
        AND NOT EXISTS (SELECT 1
                          FROM ProjectInvestigatorAssistants
                         WHERE Assistant = @Caller AND ProjectInvestigator = @OldCollarOwner))
    BEGIN
        DECLARE @message2 nvarchar(100) = 'You ('+@Caller+') are not the manager (or assistant) of the old Collar'
        RAISERROR(@message2, 18, 0)
        RETURN 2
    END
	
	-- Caller must be the Manager or Assistant of the old Platform's Program
    IF @OldPlatformOwner != @Caller
       AND NOT EXISTS (SELECT 1
                         FROM ProjectInvestigatorAssistants
                        WHERE Assistant = @Caller AND ProjectInvestigator = @OldPlatformOwner)
    BEGIN
        DECLARE @message3 nvarchar(100) = 'You ('+@Caller+') are not the manager (or assistant) of old Argos Platform'
        RAISERROR(@message3, 18, 0)
        RETURN 3
    END

    --Fix Defaults
    -- I use NULL to indicate don't change value only on non-null fields.
    -- The defaults for nullable field should match the insert SP.
    -- If the client does not want to change a nullable field they must provide the original value.
    IF @PlatformId IS NULL
        SELECT @PlatformId = [PlatformId] FROM [dbo].[ArgosDeployments] WHERE [DeploymentId] = @DeploymentId;
    IF @CollarManufacturer IS NULL
        SELECT @CollarManufacturer = [CollarManufacturer] FROM [dbo].[ArgosDeployments] WHERE [DeploymentId] = @DeploymentId;
    IF @CollarId IS NULL
        SELECT @CollarId = [CollarId] FROM [dbo].[ArgosDeployments] WHERE [DeploymentId] = @DeploymentId;


	-- Caller must be the Manager or Assistant of the new Collar
    SELECT @NewCollarOwner = Manager FROM [dbo].[Collars] WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId
    IF (@NewCollarOwner != @Caller
        AND NOT EXISTS (SELECT 1
                          FROM ProjectInvestigatorAssistants
                         WHERE Assistant = @Caller AND ProjectInvestigator = @NewCollarOwner))
    BEGIN
        DECLARE @message4 nvarchar(100) = 'You ('+@Caller+') are not the manager (or assistant) of the new Collar ' + @CollarManufacturer + '/' + @CollarId;
        RAISERROR(@message4, 18, 0)
        RETURN 4
    END
	
	-- Caller must be the Manager or Assistant of the new Platform's Program
    SELECT @NewPlatformOwner = Manager FROM [dbo].[ArgosPlatforms] AS A
                                       JOIN [dbo].[ArgosPrograms] AS P ON P.ProgramId = A.ProgramId
                                      WHERE A.PlatformId = @NewPlatformOwner
    IF @NewPlatformOwner != @Caller
       AND NOT EXISTS (SELECT 1
                         FROM ProjectInvestigatorAssistants
                        WHERE Assistant = @Caller AND ProjectInvestigator = @NewPlatformOwner)
    BEGIN
        DECLARE @message5 nvarchar(100) = 'You ('+@Caller+') are not the manager (or assistant) of new Argos Platform ' + @PlatformId;
        RAISERROR(@message5, 18, 0)
        RETURN 5
    END


    UPDATE [dbo].[ArgosDeployments] SET [PlatformId] = @PlatformId,
                                        [CollarManufacturer] = @CollarManufacturer,
                                        [CollarId] = @CollarId,
                                        [StartDate] = @StartDate,
                                        [EndDate] = @EndDate
                                  WHERE [DeploymentId] = @DeploymentId;
END

GO
GRANT EXECUTE ON [dbo].[ArgosDeployment_Update] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Regan Sarwas
-- Create date: January 25, 2013
-- Description:	Logs a new Argos collar download success/failure
-- =============================================
CREATE PROCEDURE [dbo].[ArgosDownloads_Insert] 
	@ProgramId NVARCHAR(255) = NULL, 
	@PlatformId NVARCHAR(255) = NULL,
	@Days INT = NULL,
	@FileId INT = NULL,
	@ErrorMessage NVARCHAR(255) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	-- Validate permission for this operation
	-- The caller is managed by permissions on the Stored Procedure
	
	-- Check the insert trigger for additional business rule enforcement
			
	INSERT INTO dbo.ArgosDownloads ([ProgramId], [PlatformId], [Days], [FileId], [ErrorMessage])
		 VALUES (@ProgramId, @PlatformId, @Days, @FileId, @ErrorMessage)

END



GO
GRANT EXECUTE ON [dbo].[ArgosDownloads_Insert] TO [ArgosProcessor] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[ArgosDownloads_Insert] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Regan Sarwas
-- Create date: March 16, 2013
-- Description: Clears all the prior results from processing an Argos file.
--              Anyone in the Editor or ArgosProcessor role can run this command
--              since the file can always be reprocessed.
--              It is harmless to call this on a file that has not been processed.
--              It is also harmless to call this on a file of the wrong type.
-- =============================================
CREATE PROCEDURE [dbo].[ArgosFile_ClearProcessingResults] 
    @FileId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Validate permission for this operation
    -- controlled by execute permissions on SP	

    -- wrap the two operations in a transaction
    BEGIN TRY
        BEGIN TRANSACTION

            --Delete any existing sub files; this must be done in a cursor to call the SP
            --We can't call the SPROC, because it will deny deleting a sub file
            --Fortunately, the SPROC doesn't do anything more than a delete
            --(the rest of the work is done in the trigger)
            DELETE FROM [dbo].[CollarFiles] WHERE [FileId] IN (SELECT FileId FROM CollarFiles WHERE ParentFileId = @FileId)

            -- Delete any prior processing issues
            DELETE FROM ArgosFileProcessingIssues WHERE FileId = @FileId

        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
            ROLLBACK TRANSACTION;
        EXEC [dbo].[Utility_RethrowError]
        RETURN 1
    END CATCH

END


GO
GRANT EXECUTE ON [dbo].[ArgosFile_ClearProcessingResults] TO [ArgosProcessor] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[ArgosFile_ClearProcessingResults] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:      Regan Sarwas
-- Create date: March 5, 2013
-- Description: Processes all the collars in an Argos file
-- =============================================
CREATE PROCEDURE [dbo].[ArgosFile_Process] 
	@FileId INT = NULL
	
WITH EXECUTE AS OWNER  -- To run XP_cmdshell (only available to SA)

AS
BEGIN
	SET NOCOUNT ON;

	-- Get the name of the caller
	DECLARE @Caller sysname = ORIGINAL_LOGIN();

	-- Make sure we have a suitable FileId
	DECLARE @ProjectId nvarchar(255);
	DECLARE @Owner sysname;
	DECLARE @Uploader sysname;
	SELECT @ProjectId = ProjectId, @Owner = [Owner], @Uploader = UserName
	  FROM CollarFiles AS C JOIN LookupCollarFileFormats AS F ON C.Format = F.Code
	 WHERE FileId = @FileId AND [Status] = 'A' AND F.ArgosData = 'Y'
	IF @ProjectId IS NULL AND @Owner IS NULL
	BEGIN
		DECLARE @message1 nvarchar(100) = 'Invalid Input: FileId provided is not a valid active file in a suitable format.';
		RAISERROR(@message1, 18, 0)
		RETURN (1)
	END

	-- Validate permission for this operation	
	-- The caller must be the owner or uploader of the file, the PI or editor on the project, or and ArgosProcessor 
	IF @Caller <> @Owner AND @Caller <> @Uploader  -- Not the file owner or uploader
	   AND dbo.IsInvestigatorEditor(@Owner,@Caller) = 0  -- Not an assistant to the owner
	   AND dbo.IsProjectEditor(@ProjectId, @Caller) = 0  -- Not an editor on the project
       AND NOT EXISTS (SELECT 1 
                         FROM sys.database_role_members AS RM 
                         JOIN sys.database_principals AS U 
                           ON RM.member_principal_id = U.principal_id 
                         JOIN sys.database_principals AS R 
                           ON RM.role_principal_id = R.principal_id 
                        WHERE U.name = @Caller AND R.name = 'ArgosProcessor') -- Not in the ArgosProcessor Role
	BEGIN
		DECLARE @message2 nvarchar(200) = 'Invalid Permission: You ('+@Caller+') must be the owner ('+ISNULL(@Owner,'<NULL>')+'), or have uploaded the file, or be an editor on this project ('+ISNULL(@ProjectId,'<NULL>')+') to process the collar file.';
		RAISERROR(@message2, 18, 0)
		RETURN (1)
	END
	
	
	-- Clear any (if any) prior processing results 
	EXEC [dbo].[ArgosFile_ClearProcessingResults] @FileId


	-- Run the External command
	DECLARE @exe nvarchar (255);
	SELECT @exe = Value FROM Settings WHERE Username = 'system' AND [Key] = 'argosProcessor'
	IF @exe IS NULL
		SET @exe = 'C:\Users\sql_proxy\ArgosProcessor.exe'
	DECLARE @cmd nvarchar(200) = '"' + @exe + '" /f:' + CONVERT(NVARCHAR(10),@FileId);
    BEGIN TRY
		EXEC xp_cmdshell @cmd
    END TRY
    BEGIN CATCH
		SELECT 1 -- Do nothing, we tried, we failed, hopefully the ArgosProcesor is running as a scheduled task.
    END CATCH

END





GO
GRANT EXECUTE ON [dbo].[ArgosFile_Process] TO [ArgosProcessor] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[ArgosFile_Process] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Regan Sarwas
-- Create date: March 22, 2013
--              Description: Processes only the transmission for a single platform in a file.
--              This may be called by the Argos Processor library by a client not on the server
--              It will call the Argos Processor library on the server.
--              Ignore any failures.  If this really needs to happen, then it will be caught when
--              the Argos Processor runs as a scheduled on the server.
-- =============================================
CREATE PROCEDURE [dbo].[ArgosFile_ProcessPlatform] 
    @FileId INT,
    @PlatformId VARCHAR(255)

WITH EXECUTE AS OWNER  -- To run XP_cmdshell (only available to SA)

AS
BEGIN
	SET NOCOUNT ON;

	-- Get the name of the caller
	DECLARE @Caller sysname = ORIGINAL_LOGIN();

	-- Make sure we have a suitable FileId
	DECLARE @ProjectId nvarchar(255);
	DECLARE @Owner sysname;
	DECLARE @Uploader sysname;
	SELECT @ProjectId = ProjectId, @Owner = [Owner], @Uploader = UserName
	  FROM CollarFiles AS C
	  JOIN ArgosFilePlatformDates AS D
	    ON D.FileId = C.FileId
	 WHERE C.FileId = @FileId AND D.PlatformId = @PlatformId
	IF @ProjectId IS NULL AND @Owner IS NULL
	BEGIN
		DECLARE @message1 nvarchar(100) = 'Invalid Input: FileId provided is not a valid active file in a suitable format.';
		RAISERROR(@message1, 18, 0)
		RETURN (1)
	END

	-- Validate permission for this operation	
	-- The caller must be the owner or uploader of the file, the PI or editor on the project, or and ArgosProcessor 
	IF @Caller <> @Owner AND @Caller <> @Uploader  -- Not the file owner or uploader
	   AND dbo.IsInvestigatorEditor(@Owner,@Caller) = 0  -- Not an assistant to the owner
	   AND dbo.IsProjectEditor(@ProjectId, @Caller) = 0  -- Not an editor on the project
       AND NOT EXISTS (SELECT 1 
                         FROM sys.database_role_members AS RM 
                         JOIN sys.database_principals AS U 
                           ON RM.member_principal_id = U.principal_id 
                         JOIN sys.database_principals AS R 
                           ON RM.role_principal_id = R.principal_id 
                        WHERE U.name = @Caller AND R.name = 'ArgosProcessor') -- Not in the ArgosProcessor Role
	BEGIN
		DECLARE @message2 nvarchar(200) = 'Invalid Permission: You ('+@Caller+') must be the owner ('+ISNULL(@Owner,'<NULL>')+'), or have uploaded the file, or be an editor on this project ('+ISNULL(@ProjectId,'<NULL>')+') to process the collar file.';
		RAISERROR(@message2, 18, 0)
		RETURN (1)
	END
	

	-- Clear any (if any) prior processing results
	-- This will ensure that the database is setup to respond to a query for all
	-- files needing partial processing, should the xp_cmdshell not work. 
	EXEC dbo.ArgosFile_UnProcessPlatform @FileId, @PlatformId


	-- Run the External command
	-- This will fail if the sql_proxy user is not logged in.
	-- We will rely on a regularly scheduled external OS task.
	DECLARE @exe nvarchar (255);
	SELECT @exe = Value FROM Settings WHERE Username = 'system' AND [Key] = 'argosProcessor'
	IF @exe IS NULL
		SET @exe = 'C:\Users\sql_proxy\ArgosProcessor.exe'
	DECLARE @cmd nvarchar(200) = '"' + @exe + '" /f:' + CONVERT(NVARCHAR(10),@FileId) + ' /p:' + @PlatformId;
    BEGIN TRY
		EXEC xp_cmdshell @cmd
    END TRY
    BEGIN CATCH
		SELECT 1 -- Do nothing, we tried, we failed, hopefully the ArgosProcesor is running as a scheduled task.
    END CATCH
END



GO
GRANT EXECUTE ON [dbo].[ArgosFile_ProcessPlatform] TO [ArgosProcessor] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[ArgosFile_ProcessPlatform] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Regan Sarwas
-- Create date: March 22, 2013
-- Description: Un-processes the transmissions for a single platform in a file.
--              This is called by the
--              Argos Processor in response a to an ArgosFile_ProcessPlatform request.
--              It may be that a client might want to call this (though I doubt it)
--              nevertheless, it won't hurt, since the results can be regenerated
--              (and will be if the Argos Processor is running a scheduled task with no parameters).
--              Usually when the Argos Processor calls this, there are no results/issues
--              (that is how it got included in the query ArgosFile_NeedsPartialProcessing),
--              but it may get called in other situations, besides, it doesn't hurt to try to
--              delete items that do not exits.
-- =============================================
CREATE PROCEDURE [dbo].[ArgosFile_UnProcessPlatform] 
    @FileId INT,
    @PlatformId VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Validate permission for this operation
    -- controlled by execute permissions on SP	

    -- wrap the two operations in a transaction
    BEGIN TRY
        BEGIN TRANSACTION
            -- Find and delete all result files derived from @FileId and @PlatformId
             DELETE F
               FROM CollarFiles AS F
         INNER JOIN ArgosDeployments AS D
                 ON F.ArgosDeploymentId = D.DeploymentId
              WHERE D.PlatformId = @PlatformId  -- result file is based on the platform specified
                AND F.ParentFileId = @FileId  -- parent is the file specified

            -- Find and delete all issues for this @FileId and @PlatformId
             DELETE FROM ArgosFileProcessingIssues WHERE FileId = @FileId AND PlatformId = @PlatformId

        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
            ROLLBACK TRANSACTION;
        EXEC [dbo].[Utility_RethrowError]
        RETURN 1
    END CATCH
 
END



GO
GRANT EXECUTE ON [dbo].[ArgosFile_UnProcessPlatform] TO [ArgosProcessor] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[ArgosFile_UnProcessPlatform] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Regan Sarwas
-- Create date: March 16, 2013
-- Description: inserts a new collar issue to the database.
-- =============================================
CREATE PROCEDURE [dbo].[ArgosFileProcessingIssues_Insert] 
	@FileId				INT,
	@Issue				NVARCHAR(max),
	@PlatformId			NVARCHAR(255) = NULL,
	@CollarManufacturer NVARCHAR(255) = NULL,
	@CollarId			NVARCHAR(255) = NULL,
	@FirstTransmission  DATETIME2(7)  = NULL,
	@LastTransmission   DATETIME2(7)  = NULL
AS
BEGIN
	SET NOCOUNT ON;
    
	-- Validate permission for this operation
	-- The caller must be an editor or ArgosProcessor in the database - handled by execute permissions

	-- The file must be one that has Argos Transmissions
	IF NOT EXISTS (SELECT 1 FROM CollarFiles AS C JOIN LookupCollarFileFormats AS F
	               ON C.Format = F.Code WHERE FileId = @FileId AND F.ArgosData = 'Y')
	BEGIN
		DECLARE @message2 nvarchar(200) = 'The source file is not the correct format.';
		RAISERROR(@message2, 18, 0)
		RETURN (1)
	END

	-- All other verification is handled by primary/foreign key and column constraints.
	INSERT INTO [dbo].[ArgosFileProcessingIssues]
				([FileId], [Issue], [PlatformId], [CollarManufacturer], [CollarId], [FirstTransmission], [LastTransmission])
		 VALUES (@FileId,  @Issue,  @PlatformId,  @CollarManufacturer,  @CollarId,  @FirstTransmission,  @LastTransmission)
END

GO
GRANT EXECUTE ON [dbo].[ArgosFileProcessingIssues_Insert] TO [ArgosProcessor] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[ArgosFileProcessingIssues_Insert] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Regan Sarwas
-- Create date: March 20, 2013
-- Description:	Deletes an Argos Platform
-- =============================================
CREATE PROCEDURE [dbo].[ArgosPlatform_Delete] 
    @PlatformId NVARCHAR(255) = NULL 
AS
BEGIN
    SET NOCOUNT ON;

    -- Get the name of the caller
    DECLARE @Caller sysname = ORIGINAL_LOGIN();
    
    -- Check that this is a valid Platform
    IF NOT EXISTS (SELECT 1 FROM [dbo].[ArgosPlatforms] WHERE [PlatformId] = @PlatformId)
    BEGIN
        RETURN -- quietly, the program is deleted
    END
    
    -- You must be the Manager or an Assistant to delete the Platform
    IF NOT EXISTS (   SELECT 1 FROM [dbo].[ArgosPlatforms] AS P1
                  INNER JOIN [dbo].[ArgosPrograms] AS P2
                          ON P2.[ProgramId] = P1.[ProgramId]
                   LEFT JOIN [dbo].[ProjectInvestigatorAssistants] AS A
                          ON A.ProjectInvestigator = P2.Manager
                       WHERE [PlatformId] = @PlatformId 
                         AND (@Caller = P2.[Manager] OR @Caller = A.Assistant)
                  )
    BEGIN
        DECLARE @message1 nvarchar(100) = 'You are not the manager, or an assistant, of the ArgosPlatform ' + @PlatformId;
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END


    DELETE FROM dbo.ArgosPlatforms WHERE [PlatformId] = @PlatformId
END




GO
GRANT EXECUTE ON [dbo].[ArgosPlatform_Delete] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Regan Sarwas
-- Create date: March 20, 2013
-- Description:	Adds a new Argos Platform
-- =============================================
CREATE PROCEDURE [dbo].[ArgosPlatform_Insert] 
    @PlatformId NVARCHAR(255),
    @ProgramId NVARCHAR(255),
    @DisposalDate DATETIME2(7) = NULL,
    @Notes NVARCHAR(max) = NULL,
    @Active BIT = 1 
AS
BEGIN
    SET NOCOUNT ON;

    -- Get the name of the caller
    DECLARE @Caller sysname = ORIGINAL_LOGIN();

    -- Validate permission for this operation
    -- Caller must be an editor, managed by permissions on SP
    -- Caller must be the Manager, or manager's assistant of the related Program to add a Platform to the Program
    IF NOT EXISTS (   SELECT 1 FROM [dbo].[ArgosPrograms] AS P
                   LEFT JOIN [dbo].[ProjectInvestigatorAssistants] AS A
                          ON A.ProjectInvestigator = P.Manager
                       WHERE [ProgramId] = @ProgramId 
                         AND (@Caller = P.[Manager] OR @Caller = A.Assistant)
                  )
    BEGIN
        DECLARE @message1 nvarchar(100) = 'You are not the manager, or assistant, of the ArgosProgram ' + @ProgramId;
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END

    --Fix Defaults
    SET @Notes = NULLIF(@Notes,'')
    
    --All other verification is handled by primary/foreign key and column constraints.
    INSERT INTO [dbo].[ArgosPlatforms] ([PlatformId], [ProgramId], [DisposalDate], [Notes], [Active])
                                VALUES (@PlatformId, @ProgramId, @DisposalDate, @Notes, @Active)
END

GO
GRANT EXECUTE ON [dbo].[ArgosPlatform_Insert] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		Regan Sarwas
-- Create date: March 20, 2013
-- Description:	Updates an ArgosPlatform
-- =============================================
CREATE PROCEDURE [dbo].[ArgosPlatform_Update] 
    @OldPlatformId NVARCHAR(255),
    @NewPlatformId NVARCHAR(255)= NULL,
    @ProgramId NVARCHAR(255) = NULL, 
    @DisposalDate DATETIME2(7) = NULL,
    @Notes NVARCHAR(max) = NULL,
    @Active BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    -- Get the name of the caller
    DECLARE @Caller sysname = ORIGINAL_LOGIN();

    -- Check that this is a valid platform
    DECLARE @OldManager sysname
    SELECT @OldManager = [Manager] FROM [dbo].[ArgosPlatforms] AS A
                                   JOIN [dbo].[ArgosPrograms] AS P ON P.ProgramId = A.ProgramId
                                  WHERE A.[PlatformId] = @OldPlatformId
    IF @OldManager IS NULL
    BEGIN
        DECLARE @message1 nvarchar(100) = 'ArgosPlatform ' + ISNULL(@OldPlatformId,'<NULL>') + ' does not exist.';
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END
    
    -- I use NULL to indicate don't change value only on non-null fields.
    -- The defaults for nullable field should match the insert SP.
    -- If the client does not want to change a nullable field they must provide the original value.
    IF @NewPlatformId IS NULL
        SET @NewPlatformId = @OldPlatformId
    IF @ProgramId IS NULL
        SELECT @ProgramId = [ProgramId] FROM [dbo].[ArgosPlatforms] WHERE [PlatformId] = @NewPlatformId;

    -- Check that this is a valid new platform
    DECLARE @NewManager sysname
    SELECT @NewManager = [Manager] FROM [dbo].[ArgosPrograms] WHERE ProgramId = @ProgramId
    IF @NewManager IS NULL
    BEGIN
        DECLARE @message2 nvarchar(100) = 'ArgosProgram ' + ISNULL(@ProgramId,'<NULL>') + ' does not exist.';
        RAISERROR(@message2, 18, 0)
        RETURN 1
    END
    
    -- The caller must be the old manager, or an assistant for the old manager
    IF @OldManager != @Caller
       AND NOT EXISTS (SELECT 1
                         FROM ProjectInvestigatorAssistants
                        WHERE Assistant = @Caller AND ProjectInvestigator = @OldManager)
    BEGIN
        DECLARE @message3 nvarchar(200) = 'You ('+@Caller+') must be the manager ('+@OldManager+') or an assistant to the manager to update this platform.';
        RAISERROR(@message3, 18, 0)
        RETURN 1
    END

    -- The caller must be the new manager, or an assistant for the new manager
    IF @NewManager != @Caller
       AND NOT EXISTS (SELECT 1
                         FROM ProjectInvestigatorAssistants
                        WHERE Assistant = @Caller AND ProjectInvestigator = @NewManager)
    BEGIN
        DECLARE @message4 nvarchar(200) = 'You ('+@Caller+') must be the new manager ('+@NewManager+') or an assistant to the new manager to change the platform.';
        RAISERROR(@message4, 18, 0)
        RETURN 2
    END


    --Fix Defaults
    SET @Notes = NULLIF(@Notes,'')
    
    -- Do the update
    -- We rely on the PK not changing in the update trigger, so we need to change the PK in a separate operation
    -- If the PK is changing, we need to do that as a separate operation
    -- All other verification is handled by primary/foreign key and column constraints.
    IF @NewPlatformId <> @OldPlatformId
    BEGIN
        BEGIN TRAN UpdatePlatformTwice
            BEGIN TRY
                UPDATE dbo.ArgosPlatforms SET [PlatformId] = @NewPlatformId
                                        WHERE [PlatformId] = @OldPlatformId;
                UPDATE dbo.ArgosPlatforms SET [ProgramId] = @ProgramId,
                                              [DisposalDate] = @DisposalDate,
                                              [Notes] = @Notes,
                                              [Active] = @Active
                                        WHERE [PlatformId] = @NewPlatformId;
            END TRY
            BEGIN CATCH
                ROLLBACK TRAN UpdatePlatformTwice
                EXEC dbo.Utility_RethrowError
                RETURN 1
            END CATCH
        COMMIT TRAN UpdatePlatformTwice
    END
    ELSE
        UPDATE dbo.ArgosPlatforms SET [ProgramId] = @ProgramId,
                                      [DisposalDate] = @DisposalDate,
                                      [Notes] = @Notes,
                                      [Active] = @Active
                                WHERE [PlatformId] = @NewPlatformId;


END









GO
GRANT EXECUTE ON [dbo].[ArgosPlatform_Update] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		Regan Sarwas
-- Create date: March 20, 2013
-- Description:	Deletes an Argos Program
-- =============================================
CREATE PROCEDURE [dbo].[ArgosProgram_Delete] 
    @ProgramId NVARCHAR(255) = NULL 
AS
BEGIN
    SET NOCOUNT ON;

    -- Get the name of the caller
    DECLARE @Caller sysname = ORIGINAL_LOGIN();
    
    -- Check that this is a valid program
    IF NOT EXISTS (SELECT 1 FROM [dbo].[ArgosPrograms] WHERE [ProgramId] = @ProgramId)
    BEGIN
        RETURN -- quietly, the program is deleted
    END
    
    -- You must be the Manager or an Assistant to delete the program
    IF NOT EXISTS (   SELECT 1 FROM [dbo].[ArgosPrograms] AS P
                   LEFT JOIN [dbo].[ProjectInvestigatorAssistants] AS A
                          ON A.ProjectInvestigator = P.Manager
                       WHERE [ProgramId] = @ProgramId 
                         AND (@Caller = P.[Manager] OR @Caller = A.Assistant)
                  )
    BEGIN
        DECLARE @message1 nvarchar(100) = 'You are not the manager, or the managers assistant, of the ArgosProgram ' + @ProgramId;
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END

    DELETE FROM dbo.ArgosPrograms WHERE [ProgramId] = @ProgramId
END



GO
GRANT EXECUTE ON [dbo].[ArgosProgram_Delete] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Regan Sarwas
-- Create date: March 20, 2013
-- Description:	Adds a new Argos Program
-- =============================================
CREATE PROCEDURE [dbo].[ArgosProgram_Insert] 
    @ProgramId NVARCHAR(255)= NULL,
    @ProgramName NVARCHAR(255) = NULL, 
    @UserName NVARCHAR(255) = NULL, 
    @Password NVARCHAR(255) = NULL, 
    @Manager NVARCHAR(255) = NULL, 
    @StartDate DATETIME2(7) = NULL,
    @EndDate DATETIME2(7) = NULL,
    @Notes NVARCHAR(max) = NULL,
    @Active BIT = 1 
AS
BEGIN
    SET NOCOUNT ON;

    -- Get the name of the caller
    DECLARE @Caller sysname = ORIGINAL_LOGIN();

    -- Validate permission for this operation
    -- Caller must be an investigator, or an investigator's assistant, managed by permissions on SP
    if @Manager IS NULL
        SET @Manager = @Caller
    IF @Manager != @Caller
       AND NOT EXISTS (SELECT 1
                         FROM ProjectInvestigatorAssistants
                        WHERE Assistant = @Caller AND ProjectInvestigator = @Manager)
    BEGIN
        DECLARE @message1 nvarchar(100) = 'You ('+@Caller+') must be the manager ('+@Manager+') or an assistant to the manager to update create this program.';
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END

    --Fix Defaults
    SET @Manager = ISNULL(@Manager,@Caller)
    SET @ProgramName = NULLIF(@ProgramName,'')
    SET @Notes = NULLIF(@Notes,'')
    
    --All other verification is handled by primary/foreign key and column constraints.
    INSERT INTO [dbo].[ArgosPrograms] ([ProgramId], [ProgramName], [UserName], [Password],
                                       [Manager], [StartDate], [EndDate], [Notes], [Active])
                               VALUES (@ProgramId, @ProgramName, @UserName, @Password,
                                       @Manager, @StartDate, @EndDate, @Notes, @Active)
END

GO
GRANT EXECUTE ON [dbo].[ArgosProgram_Insert] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Regan Sarwas
-- Create date: March 20, 2013
-- Description:	Updates an ArgosProgram
-- =============================================
CREATE PROCEDURE [dbo].[ArgosProgram_Update] 
    @OldProgramId NVARCHAR(255),
    @NewProgramId NVARCHAR(255)= NULL,
    @ProgramName NVARCHAR(255) = NULL, 
    @UserName NVARCHAR(255) = NULL, 
    @Password NVARCHAR(255) = NULL, 
    @Manager NVARCHAR(255) = NULL, 
    @StartDate DATETIME2(7) = NULL,
    @EndDate DATETIME2(7) = NULL,
    @Notes NVARCHAR(max) = NULL,
    @Active BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    -- Get the name of the caller
    DECLARE @Caller sysname = ORIGINAL_LOGIN();

    -- Check that this is a valid program
    DECLARE @OldManager sysname
    SELECT @OldManager = [Manager] FROM [dbo].[ArgosPrograms] WHERE [ProgramId] = @OldProgramId
    IF @OldManager IS NULL
    BEGIN
        DECLARE @message1 nvarchar(100) = 'ArgosProgram ' + ISNULL(@OldProgramId,'<NULL>') + ' does not exist.';
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END
    
    -- The caller must be the old manager, or an assistant for the old manager
    IF @OldManager != @Caller
       AND NOT EXISTS (SELECT 1
                         FROM ProjectInvestigatorAssistants
                        WHERE Assistant = @Caller AND ProjectInvestigator = @OldManager)
    BEGIN
        DECLARE @message3 nvarchar(200) = 'You ('+@Caller+') must be the manager ('+@OldManager+') or an assistant to the owner to update this file.';
        RAISERROR(@message3, 18, 0)
        RETURN 1
    END

    -- The caller must be the new manager, or an assistant for the new manager
    IF @Manager IS NULL
        SELECT @Manager = @OldManager

    IF @Manager != @Caller
       AND NOT EXISTS (SELECT 1
                         FROM ProjectInvestigatorAssistants
                        WHERE Assistant = @Caller AND ProjectInvestigator = @Manager)
    BEGIN
        DECLARE @message2 nvarchar(200) = 'You ('+@Caller+') must be the new manager ('+@Manager+') or an assistant to the new owner to change the owner.';
        RAISERROR(@message2, 18, 0)
        RETURN 2
    END


    --Fix Defaults
    SET @Manager = ISNULL(@Manager,@Caller)
    SET @ProgramName = NULLIF(@ProgramName,'')
    SET @Notes = NULLIF(@Notes,'')

    -- I use NULL to indicate don't change value only on non-null fields.
    -- The defaults for nullable field should match the insert SP.
    -- If the client does not want to change a nullable field they must provide the original value.
    IF @NewProgramId IS NULL
        SET @NewProgramId = @OldProgramId
    IF @UserName IS NULL
        SELECT @UserName = [UserName] FROM [dbo].[ArgosPrograms] WHERE [ProgramId] = @OldProgramId;
    
    IF @Password IS NULL
        SELECT @Password = [Password] FROM [dbo].[ArgosPrograms] WHERE [ProgramId] = @OldProgramId;
    
    IF @Manager IS NULL
        SELECT @Manager = [Manager] FROM [dbo].[ArgosPrograms] WHERE [ProgramId] = @OldProgramId;
    
    
    -- Do the update, replacing empty strings with NULLs
    -- All other verification is handled by primary/foreign key and column constraints.
    UPDATE dbo.ArgosPrograms SET [ProgramId] = @NewProgramId,
                                 [ProgramName] = @ProgramName,
                                 [UserName] = @UserName,
                                 [Password] = @Password,
                                 [Manager] = @Manager,
                                 [StartDate] = @StartDate,
                                 [EndDate] = @EndDate,
                                 [Notes] = @Notes,
                                 [Active] = @Active
                           WHERE [ProgramId] = @OldProgramId;

END



GO
GRANT EXECUTE ON [dbo].[ArgosProgram_Update] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Regan Sarwas
-- Create date: April 2, 2012
-- Description: Delete a collar
-- =============================================
CREATE PROCEDURE [dbo].[Collar_Delete] 
    @CollarManufacturer NVARCHAR(255),
    @CollarId           NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    -- Get the name of the caller
    DECLARE @Caller SYSNAME = ORIGINAL_LOGIN();

    DECLARE @Manager SYSNAME
    SELECT @Manager = Manager FROM Collars WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId
    IF @Manager IS NULL
        RETURN -- Collar was not found, so it must have been deleted already

    -- Validate permission for this operation

    -- The caller must be the Manager or assistant of the collar
    IF @Manager != @Caller
        AND NOT EXISTS (SELECT 1 FROM ProjectInvestigatorAssistants
                         WHERE Assistant = @Caller AND ProjectInvestigator = @Manager)
    BEGIN
        DECLARE @message1 nvarchar(200) = 'You ('+@Caller+') must be the manager (or assistant) of this collar ('+@CollarManufacturer+'/'+@CollarId+') to delete it.';
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END


    DELETE FROM dbo.Collars WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId

END

GO
GRANT EXECUTE ON [dbo].[Collar_Delete] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Regan Sarwas
-- Create date: April 2, 2012
-- Description: Adds a new Collar.
--    Modified: 2013-03-01 - Added Manager to parameter list (allows creating a collar for another PI)
-- =============================================
CREATE PROCEDURE [dbo].[Collar_Insert] 
    @CollarManufacturer NVARCHAR(255),
    @CollarId           NVARCHAR(255), 
    @CollarModel        NVARCHAR(255), 
    @Manager            NVARCHAR(255) = NULL, -- null defaults to caller
    @Owner              NVARCHAR(255) = NULL,  
    @SerialNumber       NVARCHAR(255) = NULL, 
    @Frequency          FLOAT = NULL, 
    @HasGps             BIT = 0, 
    @Notes              NVARCHAR(max) = NULL,
    @DisposalDate       DATETIME2(7) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Get the name of the caller
    DECLARE @Caller sysname = ORIGINAL_LOGIN();

    --Fix Text Inputs
    SET @CollarManufacturer = NULLIF(@CollarManufacturer,'')
    SET @CollarId           = NULLIF(@CollarId,'')
    SET @CollarModel        = NULLIF(@CollarModel,'')
    SET @Manager            = NULLIF(@Manager,'')
    SET @Owner              = NULLIF(@Owner,'')
    SET @SerialNumber       = NULLIF(@SerialNumber,'')
    SET @Notes              = NULLIF(@Notes,'')

    --default PI is the calling user
    SET @Manager = ISNULL(@Manager, @Caller) 

    -- Validate permission for this operation
    -- If the caller is not the owner or assistant then error and return
    IF @Manager != @Caller
       AND NOT EXISTS (SELECT 1
                         FROM ProjectInvestigatorAssistants
                        WHERE Assistant = @Caller AND ProjectInvestigator = @Manager)
    BEGIN
        DECLARE @message1 nvarchar(100) = 'You ('+@Caller+') must be the manager (or assistant) of the a collar'
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END


    INSERT INTO dbo.Collars ([CollarManufacturer], [CollarId], [CollarModel], [Manager], [Owner],  
                             [SerialNumber], [Frequency], [HasGps], [Notes])
                     VALUES (@CollarManufacturer, @CollarId, @CollarModel, @Manager, @Owner,
                             @SerialNumber, @Frequency, @HasGps, @Notes)

END

GO
GRANT EXECUTE ON [dbo].[Collar_Insert] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Regan Sarwas
-- Create date: April 2, 2012
-- Description: Updates a Collar
-- =============================================
CREATE PROCEDURE [dbo].[Collar_Update] 
    @CollarManufacturer NVARCHAR(255),
    @CollarId           NVARCHAR(255), 
    @CollarModel        NVARCHAR(255) = NULL, 
    @Manager            SYSNAME       = NULL, 
    @Owner              NVARCHAR(255), 
    @SerialNumber       NVARCHAR(255), 
    @Frequency          FLOAT, 
    @HasGps             BIT           = NULL, 
    @Notes              NVARCHAR(MAX),
    @DisposalDate       DATETIME2(7)
AS
BEGIN
    SET NOCOUNT ON;

    -- Get the name of the caller
    DECLARE @Caller SYSNAME = ORIGINAL_LOGIN();

    -- Fix Text Inputs
    SET @CollarModel        = NULLIF(@CollarModel,'')
    SET @Manager            = NULLIF(@Manager,'')
    SET @Owner              = NULLIF(@Owner,'')
    SET @SerialNumber       = NULLIF(@SerialNumber,'')
    SET @Notes              = NULLIF(@Notes,'')

    -- Verify this is an existing collar by getting the original owner (needed for permissions check)
    DECLARE @OldManager SYSNAME
    SELECT @OldManager = Manager FROM dbo.Collars WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId
    IF @OldManager IS NULL
    BEGIN
        DECLARE @message1 nvarchar(100) = 'There is no such collar ('+@CollarManufacturer+'/'+@CollarId+').';
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END

    --Fix Defaults
    -- I use NULL to indicate "don't change value" only on non-null fields.
    -- The defaults for nullable field should match the insert SP.
    -- If the client does not want to change a nullable field they must provide the original value.
    IF @CollarModel IS NULL
        SELECT @CollarModel = [CollarModel] FROM dbo.Collars WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId;
    IF @Manager IS NULL
        SELECT @Manager = [Manager] FROM dbo.Collars WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId;
    IF @HasGps IS NULL
        SELECT @HasGps = [HasGps] FROM dbo.Collars WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId;


    -- Validate permission for this operation
    -- The caller must be the old manager or assistant of the collar
    IF @OldManager != @Caller
       AND NOT EXISTS (SELECT 1 FROM ProjectInvestigatorAssistants
                        WHERE Assistant = @Caller AND ProjectInvestigator = @OldManager)
    BEGIN
        DECLARE @message2 nvarchar(200) = 'You ('+@Caller+') must be the old manager (or assistant) of this collar.';
        RAISERROR(@message2, 18, 0)
        RETURN 2
    END

    -- The caller must be the manager or assistant of the collar
    IF @Manager != @Caller
       AND NOT EXISTS (SELECT 1 FROM ProjectInvestigatorAssistants
                        WHERE Assistant = @Caller AND ProjectInvestigator = @Manager)
    BEGIN
        DECLARE @message3 nvarchar(200) = 'You ('+@Caller+') must be the new manager (or assistant) of this collar.';
        RAISERROR(@message3, 18, 0)
        RETURN 3
    END


    UPDATE dbo.Collars SET [CollarModel]  = @CollarModel,
                           [Manager]      = @Manager,
                           [Owner]        = @Owner,
                           [SerialNumber] = @SerialNumber,
                           [Frequency]    = @Frequency,
                           [HasGps]       = @HasGps,
                           [Notes]        = @Notes,
                           [DisposalDate] = @DisposalDate
                     WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId;

END

GO
GRANT EXECUTE ON [dbo].[Collar_Update] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Regan Sarwas
-- Create date: March 2, 2012
-- Description:	Parses the data in a Collar File
-- =============================================
CREATE PROCEDURE [dbo].[CollarData_Insert] 
    @FileId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- This is not executed directly, only by CollarFile_Insert 

    DECLARE @Format CHAR = NULL
    SELECT @Format = Format FROM CollarFiles WHERE FileId = @FileId

    IF @Format = 'A'  -- Store on board
    BEGIN
        -- only parse the data if it is not already in the file
        IF NOT EXISTS (SELECT 1 FROM [dbo].[CollarDataTelonicsGen3StoreOnBoard] WHERE [FileId] = @FileId)
        BEGIN
            INSERT INTO [dbo].[CollarDataTelonicsGen3StoreOnBoard] SELECT @FileId as FileId, * FROM [dbo].[ParseFormatA] (@FileId) 
        END
    END
        
    IF @Format = 'B'  -- Debevek SubFile Format
    BEGIN
        -- only parse the data if it is not already in the file
        IF NOT EXISTS (SELECT 1 FROM [dbo].[CollarDataDebevekFormat] WHERE [FileId] = @FileId)
        BEGIN
            INSERT INTO [dbo].[CollarDataDebevekFormat] SELECT @FileId as FileId, * FROM [dbo].[ParseFormatB] (@FileId) 
        END
    END
    
    IF @Format = 'C'  -- Telonics Gen4 Convertor Format
    BEGIN
        -- only parse the data if it is not already in the file
        IF NOT EXISTS (SELECT 1 FROM [dbo].[CollarDataTelonicsGen4] WHERE [FileId] = @FileId)
        BEGIN
            INSERT INTO [dbo].[CollarDataTelonicsGen4] SELECT @FileId as FileId, * FROM [dbo].[ParseFormatC] (@FileId) 
        END
    END
    
    IF @Format = 'D'  -- Telonics Gen3 Convertor Format
    BEGIN
        -- only parse the data if it is not already in the file
        IF NOT EXISTS (SELECT 1 FROM [dbo].[CollarDataTelonicsGen3] WHERE [FileId] = @FileId)
        BEGIN
            INSERT INTO [dbo].[CollarDataTelonicsGen3] SELECT @FileId as FileId, * FROM [dbo].[ParseFormatD] (@FileId) 
        END
    END
    
    IF @Format = 'E' -- Telonics email format
    BEGIN
        -- only parse the non-gps data if it is not already in the file
        IF NOT EXISTS (SELECT 1 FROM [dbo].[CollarDataArgosEmail] WHERE [FileId] = @FileId)
        BEGIN
            INSERT INTO [dbo].[CollarDataArgosEmail] SELECT @FileId as FileId, * FROM [dbo].[ParseFormatE] (@FileId) 
        END
    END
    
    IF @Format = 'F'  -- Argos Web Services Format
    BEGIN
        -- only parse the non-gps data if it is not already in the file
        IF NOT EXISTS (SELECT 1 FROM [dbo].[CollarDataArgosWebService] WHERE [FileId] = @FileId)
        BEGIN
            INSERT INTO [dbo].[CollarDataArgosWebService] SELECT @FileId as FileId, * FROM [dbo].[ParseFormatF] (@FileId) 
        END
    END
    
    --IF @Format = 'G'  -- Debevek Format
    -- do not process the parent, only the children ('B')
    
    --IF @Format = 'H'  -- Gen4 Datalog (store-on-board) Format
    -- do not process the parent, only the children ('C')

    IF @Format = 'I'  -- Iridium Email Download Format
    BEGIN
        -- only parse the data if it is not already in the file
        IF NOT EXISTS (SELECT 1 FROM [dbo].[CollarDataIridiumMail] WHERE [FileId] = @FileId)
        BEGIN
            INSERT INTO [dbo].[CollarDataIridiumMail] SELECT @FileId as FileId, * FROM [dbo].[ParseFormatI] (@FileId) 
        END
    END
    
    --IF @Format = 'J'  -- VHF Format
    -- do not process in the database; uses a one-time external processor.

    --IF @Format = 'K'  -- Old Argos PPT Email Format
    -- do not process in the database; uses a one-time external processor.

    --IF @Format = 'L'  -- Old Argos PPT CSV Format
    -- do not process in the database; uses a one-time external processor.

    IF @Format = 'M'  -- Televilt Format
    BEGIN
        -- only parse the data if it is not already in the file
        IF NOT EXISTS (SELECT 1 FROM [dbo].[CollarDataTelevilt] WHERE [FileId] = @FileId)
        BEGIN
            INSERT INTO [dbo].[CollarDataTelevilt] SELECT @FileId as FileId, * FROM [dbo].[ParseFormatM] (@FileId) 
        END
    END

   -- IF @Format = 'N'  -- Lotek Webservice format
   -- This is likely a one time upload, so loaded the data manually.
   -- BEGIN
        -- only parse the data if it is not already in the file
   --     IF NOT EXISTS (SELECT 1 FROM [dbo].[CollarDataLotekIridium] WHERE [FileId] = @FileId)
   --     BEGIN
   --         INSERT INTO [dbo].[CollarDataLotekIridium] SELECT @FileId as FileId, * FROM [dbo].[ParseFormatN] (@FileId) 
   --     END
   -- END

    IF @Format = 'O'  -- Vectronic location data downloaded via the HTTP Wildlife API v2
    BEGIN
        -- only parse the data if it is not already in the file
        IF NOT EXISTS (SELECT 1 FROM [dbo].[CollarDataVectronicPosition] WHERE [FileId] = @FileId)
        BEGIN
            INSERT INTO [dbo].[CollarDataVectronicPosition] SELECT @FileId as FileId, * FROM [dbo].[ParseFormatO] (@FileId) 
        END
    END

    IF @Format = 'P'  -- Vectronic activity data downloaded via the HTTP Wildlife API v2
    BEGIN
        -- only parse the data if it is not already in the file
        IF NOT EXISTS (SELECT 1 FROM [dbo].[CollarDataVectronicActivity] WHERE [FileId] = @FileId)
        BEGIN
            INSERT INTO [dbo].[CollarDataVectronicActivity] SELECT @FileId as FileId, * FROM [dbo].[ParseFormatP] (@FileId) 
        END
    END

    IF @Format = 'Q'  -- Vectronic mortality data downloaded via the HTTP Wildlife API v2
    BEGIN
        -- only parse the data if it is not already in the file
        IF NOT EXISTS (SELECT 1 FROM [dbo].[CollarDataVectronicMortality] WHERE [FileId] = @FileId)
        BEGIN
            INSERT INTO [dbo].[CollarDataVectronicMortality] SELECT @FileId as FileId, * FROM [dbo].[ParseFormatQ] (@FileId) 
        END
    END

END



GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Regan Sarwas
-- Create date: April 2, 2012
-- Description: Delete a Collar Deployment.
-- =============================================
CREATE PROCEDURE [dbo].[CollarDeployment_Delete] 
    @DeploymentId	INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Get the name of the caller
    DECLARE @Caller SYSNAME = ORIGINAL_LOGIN();

    -- Validate permission for this operation

    -- Validate deployment and get variables for permission checking
    DECLARE @ProjectId NVARCHAR(255);
    SELECT  @ProjectId = ProjectId FROM dbo.CollarDeployments WHERE DeploymentId = @DeploymentId
    IF @ProjectId IS NULL
        RETURN 
    DECLARE @ProjectPI NVARCHAR(255);
    SELECT  @ProjectPI = ProjectInvestigator FROM dbo.Projects WHERE ProjectId = @ProjectId
    DECLARE @CollarPI NVARCHAR(255);
    SELECT  @CollarPI = Manager FROM dbo.Collars AS C JOIN dbo.CollarDeployments AS D
                                 ON D.CollarManufacturer = C.CollarManufacturer AND D.CollarId = C.CollarId
                              WHERE DeploymentId = @DeploymentId
    DECLARE @IsProjectEditor BIT = dbo.IsProjectEditor(@ProjectId, @Caller)
    DECLARE @IsInvestigatorEditor BIT = dbo.IsInvestigatorEditor(@CollarPI, @Caller)

    -- The caller must be the Project PI or editor on the project
    -- or the collar owner or an assistant to the collar owner
    IF @IsProjectEditor = 0 AND @IsInvestigatorEditor = 0 
    BEGIN
        DECLARE @message1 nvarchar(200) = 'You ('+@Caller+') must be the PI or editor on the project OR owner (or assitant) of the collar.';
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END

    -- If the collar does not belong to the project PI, then
    --   The caller must also be the Owner (or assistant) of the collar
    IF @ProjectPI != @CollarPI AND @IsInvestigatorEditor = 0
    BEGIN
            DECLARE @message2 nvarchar(200) = 'You ('+@Caller+') must also be the manager (or assistant) of the collar.';
            RAISERROR(@message2, 18, 0)
            RETURN 2
    END

    -- If the project does not belong to the collar owner, then
    --   The caller must also be an editor on the project
    IF @ProjectPI != @CollarPI AND @IsProjectEditor = 0
    BEGIN
            DECLARE @message3 nvarchar(200) = 'You ('+@Caller+') must also be the PI or editor of the project.';
            RAISERROR(@message3, 18, 0)
            RETURN 3
    END


    DELETE FROM dbo.CollarDeployments WHERE DeploymentId = @DeploymentId

END


GO
GRANT EXECUTE ON [dbo].[CollarDeployment_Delete] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Regan Sarwas
-- Create date: April 2, 2012
-- Description: Adds a new Collar Deployment.
-- =============================================
CREATE PROCEDURE [dbo].[CollarDeployment_Insert] 
    @ProjectId          NVARCHAR(255),
    @AnimalId           NVARCHAR(255), 
    @CollarManufacturer NVARCHAR(255),
    @CollarId           NVARCHAR(255), 
    @DeploymentDate     DATETIME2, 
    @RetrievalDate      DATETIME2 = NULL, 
    @DeploymentId       INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Get the name of the caller
    DECLARE @Caller SYSNAME = ORIGINAL_LOGIN();

    --Fix Text Inputs
    SET @ProjectId          = NULLIF(@ProjectId,'')
    SET @AnimalId           = NULLIF(@AnimalId,'')
    SET @CollarManufacturer = NULLIF(@CollarManufacturer,'')
    SET @CollarId           = NULLIF(@CollarId,'')

    -- Validate permission for this operation

    -- Get variables for permission checking
    DECLARE @ProjectPI NVARCHAR(255);
    SELECT  @ProjectPI = ProjectInvestigator FROM dbo.Projects WHERE ProjectId = @ProjectId
    DECLARE @CollarPI NVARCHAR(255);
    SELECT  @CollarPI = Manager FROM dbo.Collars WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId
    DECLARE @IsProjectEditor BIT = dbo.IsProjectEditor(@ProjectId, @Caller)
    DECLARE @IsInvestigatorEditor BIT = dbo.IsInvestigatorEditor(@CollarPI, @Caller)

    -- The caller must be the Project PI or editor on the project
    -- or the collar owner or an assistant to the collar owner
    IF @IsProjectEditor = 0 AND @IsInvestigatorEditor = 0 
    BEGIN
        DECLARE @message1 nvarchar(200) = 'You ('+@Caller+') must be the PI or editor on the project OR owner (or assitant) of the collar.';
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END

    -- If the collar does not belong to the project PI, then
    --   The caller must also be the Owner (or assistant) of the collar
    IF @ProjectPI != @CollarPI AND @IsInvestigatorEditor = 0
    BEGIN
            DECLARE @message2 nvarchar(200) = 'You ('+@Caller+') must also be the manager (or assistant) of the collar.';
            RAISERROR(@message2, 18, 0)
            RETURN 2
    END

    -- If the project does not belong to the collar owner, then
    --   The caller must also be an editor on the project
    IF @ProjectPI != @CollarPI AND @IsProjectEditor = 0
    BEGIN
            DECLARE @message3 nvarchar(200) = 'You ('+@Caller+') must also be the PI or editor of the project.';
            RAISERROR(@message3, 18, 0)
            RETURN 3
    END

    -- All other verification is handled by triggers, primary/foreign key and column constraints.
    INSERT INTO dbo.CollarDeployments
                ([ProjectId], [AnimalId], [CollarManufacturer], [CollarId], [DeploymentDate], [RetrievalDate])
         VALUES (@ProjectId,  @AnimalId,  @CollarManufacturer,  @CollarId,  @DeploymentDate,  @RetrievalDate)
    SET @DeploymentId = SCOPE_IDENTITY()

END

GO
GRANT EXECUTE ON [dbo].[CollarDeployment_Insert] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Regan Sarwas
-- Create date: April 2, 2012
-- Description: Adds a new Collar Deployment.
-- =============================================
CREATE PROCEDURE [dbo].[CollarDeployment_Update] 
    @DeploymentId       INT,
    @ProjectId          NVARCHAR(255) = NULL,
    @AnimalId           NVARCHAR(255) = NULL, 
    @CollarManufacturer NVARCHAR(255) = NULL,
    @CollarId           NVARCHAR(255) = NULL, 
    @DeploymentDate     DATETIME2 = NULL, 
    @RetrievalDate      DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    -- Get the name of the caller
    DECLARE @Caller sysname = ORIGINAL_LOGIN();

    -- Validate Deployment before we go any further
    DECLARE @OldProjectId          NVARCHAR(255)
    DECLARE @OldCollarManufacturer NVARCHAR(255)
    DECLARE @OldCollarId           NVARCHAR(255) 
    SELECT @OldProjectId = ProjectId, @OldCollarManufacturer = CollarManufacturer, @OldCollarId = CollarId
      FROM [dbo].[CollarDeployments] WHERE [DeploymentId] = @DeploymentId
    IF @OldProjectId IS NULL
    BEGIN
        RAISERROR('The deployment you want to change was not found.', 18, 0)
        RETURN 1
    END
    
    --Fix Text Inputs
    SET @ProjectId          = NULLIF(@ProjectId,'')
    SET @AnimalId           = NULLIF(@AnimalId,'')
    SET @CollarManufacturer = NULLIF(@CollarManufacturer,'')
    SET @CollarId           = NULLIF(@CollarId,'')

    --Fix Defaults
    -- I use NULL to indicate don't change value only on non-null fields.
    -- The defaults for nullable field should match the insert SP.
    -- If the client does not want to change a nullable field they must provide the original value.
    IF @ProjectId IS NULL
        SELECT @ProjectId = [ProjectId] FROM [dbo].[CollarDeployments] WHERE [DeploymentId] = @DeploymentId;
    IF @AnimalId IS NULL
        SELECT @AnimalId = [AnimalId] FROM [dbo].[CollarDeployments] WHERE [DeploymentId] = @DeploymentId;
    IF @CollarManufacturer IS NULL
        SELECT @CollarManufacturer = [CollarManufacturer] FROM [dbo].[CollarDeployments] WHERE [DeploymentId] = @DeploymentId;
    IF @CollarId IS NULL
        SELECT @CollarId = [CollarId] FROM [dbo].[CollarDeployments] WHERE [DeploymentId] = @DeploymentId;
    IF @DeploymentDate IS NULL
        SELECT @DeploymentDate = [DeploymentDate] FROM [dbo].[CollarDeployments] WHERE [DeploymentId] = @DeploymentId;


    -- Get variables for permission checking against OLD deployment
    DECLARE @OldProjectPI NVARCHAR(255);
    SELECT  @OldProjectPI = ProjectInvestigator FROM dbo.Projects WHERE ProjectId = @OldProjectId
    DECLARE @OldCollarPI NVARCHAR(255);
    SELECT  @OldCollarPI = Manager FROM dbo.Collars AS C JOIN dbo.CollarDeployments AS D
                                  ON D.CollarManufacturer = C.CollarManufacturer AND D.CollarId = C.CollarId
                               WHERE DeploymentId = @DeploymentId
    DECLARE @IsOldProjectEditor BIT = dbo.IsProjectEditor(@OldProjectId, @Caller)
    DECLARE @IsOldInvestigatorEditor BIT = dbo.IsInvestigatorEditor(@OldCollarPI, @Caller)

    -- The caller must be the Project PI or editor on the project
    -- or the collar owner or an assistant to the collar owner
    IF @IsOldProjectEditor = 0 AND @IsOldInvestigatorEditor = 0 
    BEGIN
        DECLARE @message1 nvarchar(200) = 'You ('+@Caller+') must be the PI or editor on the old project OR owner (or assitant) of the old collar.';
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END

    -- If the collar does not belong to the project PI, then
    --   The caller must also be the Owner (or assistant) of the collar
    IF @OldProjectPI != @OldCollarPI AND @IsOldInvestigatorEditor = 0
    BEGIN
            DECLARE @message2 nvarchar(200) = 'You ('+@Caller+') must also be the manager (or assistant) of the old collar.';
            RAISERROR(@message2, 18, 0)
            RETURN 2
    END

    -- If the project does not belong to the collar owner, then
    --   The caller must also be an editor on the project
    IF @OldProjectPI != @OldCollarPI AND @IsOldProjectEditor = 0
    BEGIN
            DECLARE @message3 nvarchar(200) = 'You ('+@Caller+') must also be the PI or editor of the old project.';
            RAISERROR(@message3, 18, 0)
            RETURN 3
    END


    -- Get variables for permission checking against NEW deployment
    DECLARE @ProjectPI NVARCHAR(255);
    SELECT  @ProjectPI = ProjectInvestigator FROM dbo.Projects WHERE ProjectId = @ProjectId
    DECLARE @CollarPI NVARCHAR(255);
    SELECT  @CollarPI = Manager FROM dbo.Collars
                               WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId
    DECLARE @IsProjectEditor BIT = dbo.IsProjectEditor(@ProjectId, @Caller)
    DECLARE @IsInvestigatorEditor BIT = dbo.IsInvestigatorEditor(@CollarPI, @Caller)

    -- The caller must be the Project PI or editor on the project
    -- or the collar owner or an assistant to the collar owner
    IF @IsProjectEditor = 0 AND @IsInvestigatorEditor = 0 
    BEGIN
        DECLARE @message4 nvarchar(200) = 'You ('+@Caller+') must be the PI or editor on the new project OR owner (or assitant) of the new collar.';
        RAISERROR(@message4, 18, 0)
        RETURN 4
    END

    -- If the collar does not belong to the project PI, then
    --   The caller must also be the Owner (or assistant) of the collar
    IF @ProjectPI != @CollarPI AND @IsInvestigatorEditor = 0
    BEGIN
            DECLARE @message5 nvarchar(200) = 'You ('+@Caller+') must also be the manager (or assistant) of the new collar.';
            RAISERROR(@message5, 18, 0)
            RETURN 5
    END

    -- If the project does not belong to the collar owner, then
    --   The caller must also be an editor on the project
    IF @ProjectPI != @CollarPI AND @IsProjectEditor = 0
    BEGIN
            DECLARE @message6 nvarchar(200) = 'You ('+@Caller+') must also be the PI or editor of the new project.';
            RAISERROR(@message6, 18, 0)
            RETURN 6
    END


    UPDATE dbo.CollarDeployments
       SET RetrievalDate  = @RetrievalDate,
           DeploymentDate = @DeploymentDate
     WHERE DeploymentId   = @DeploymentId;
END


GO
GRANT EXECUTE ON [dbo].[CollarDeployment_Update] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Regan Sarwas
-- Create date: March 2, 2012
--     revised: March 25, 2013
-- Description: Deletes a CollarFile
--              related records are deleted by FK Cascades, or the INSTEAD OF trigger
--              Ensures that Child files are never delete (the trigger handles it)
-- =============================================
CREATE PROCEDURE [dbo].[CollarFile_Delete] 
    @FileId int
AS
BEGIN
    SET NOCOUNT ON;

    -- Get the name of the caller
    DECLARE @Caller SYSNAME = ORIGINAL_LOGIN();

    -- First check that this is a valid file, if not exit quietly (we are done)
    IF NOT EXISTS (SELECT 1 FROM [dbo].[CollarFiles] WHERE [FileId] = @FileId)
    BEGIN
        RETURN 0
    END

    -- Validate permission for this operation
    -- To delete a file you must be an editor of the project, or the Owner (or assistant) of the file
    -- Do not check the uploader. i.e. Do not allow someone who may have lost their privileges to remove a file.

    -- Get the projectId and Owner
    DECLARE @ProjectId NVARCHAR(255) = NULL
    DECLARE @Owner NVARCHAR(255) = NULL
    SELECT @ProjectID = [ProjectId], @Owner = [Owner] FROM [dbo].[CollarFiles] WHERE [FileId] = @FileId;
    IF [dbo].[IsProjectEditor](@ProjectID,@Caller) = 0 AND [dbo].[IsInvestigatorEditor](@Owner,@Caller) = 0
    BEGIN
        DECLARE @message1 nvarchar(200)
        IF @ProjectId IS NOT NULL
            SET @message1 = 'You ('+@Caller+') are not the principal investigator or an editor of this file''s project ('+@ProjectId+').';
        ELSE
            SET @message1 = 'You ('+@Caller+') are not the owner (' + @Owner + ')of this file or their assistant.';
        RAISERROR(@message1, 18, 0)
        RETURN (1)
    END
 
    -- Check that this is not somebodies child
    -- this was previously denied, but it is required to allow the user to delete collar parameters and argos deployments
    -- In general, client apps should deny deleting client files directly.  The scheduled ArgosProcessor will recreate
    -- them unless an Argos Deployment or Collar Parameter is also deleted.

    -- Delete this file
    DELETE FROM [dbo].[CollarFiles] WHERE [FileId] = @FileId;

END


GO
GRANT EXECUTE ON [dbo].[CollarFile_Delete] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- ===============================================
-- Author:      Regan Sarwas
-- Create date: February 23, 2016
-- Description: Fixes the owner of an IDF (format I) file based on the relevant Collar Parameter file
--              These files are loaded before the owner is known.
--              Once they are loaded (and parsed), the table 'CollarDataIridiumMail' can be used to
--              find the relevant TPF file (if it exists), and then owner of the collar can be
--              set to the owner of the TPF file.
--              It is an error to call this without a format I file ID, or to not have a matching TPF file.
--              Can be run as any user, as it is always safe to make this fix, but we will limit to editors.
-- ===============================================
CREATE PROCEDURE [dbo].[CollarFile_FixOwnerOfIdfFile] 
    @FileId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- FileId must be an Parsed Iridium Email File (i.e. in CollarDataIridiumMail) 
    IF NOT EXISTS (SELECT 1 FROM CollarDataIridiumMail WHERE FileId = @FileId)
    BEGIN
        DECLARE @message1 nvarchar(100) = 'Invalid parameter: FileId ' + CAST(@FileId AS VARCHAR(10)) + ' was not found in the CollarDataIridiumMail table';
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END

    -- File will have one or more (typically one) records in CollarDataIridiumMail for each collar in the email.
	-- Each email will belong to a specific PI (although we can't tell from the email), so we will assume that all
	-- the collars in the email belong to the same PI, and only look at the first matching record in CollarDataIridiumMail
    DECLARE @Owner NVARCHAR(255) = NULL
	 SELECT TOP 1 @Owner = P.Owner
	   FROM [dbo].[CollarDataIridiumMail] AS D
	   JOIN [dbo].[AllTpfFileData] AS T
	     ON T.[PlatformId] = D.[Imei] AND T.[TimeStamp] < D.[MessageTime]
	   JOIN [dbo].[CollarParameterFiles] AS P
	     ON P.FileId = T.FileId
	  WHERE T.[Platform] = 'Iridium'
	    AND D.[FileId] = @FileId
   ORDER BY T.[TimeStamp] DESC
 
    -- Must have found an owner in the TPF file relationship 
    IF @Owner IS NULL
    BEGIN
        DECLARE @message2 nvarchar(100) = 'The Iridium Id in file ' + CAST(@FileId AS VARCHAR(10)) + ' is not referenced in any loaded Collar Parameter Files.';
        RAISERROR(@message2, 18, 0)
        RETURN 2
    END

    -- Do the update
    UPDATE [dbo].[CollarFiles]
       SET [Owner] = @Owner
     WHERE [FileId] = @FileId

END





GO
GRANT EXECUTE ON [dbo].[CollarFile_FixOwnerOfIdfFile] TO [ArgosProcessor] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[CollarFile_FixOwnerOfIdfFile] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Regan Sarwas
-- Create date: March 3, 2012
-- Description:	Adds a new collar file to the database.
-- =============================================
CREATE PROCEDURE [dbo].[CollarFile_Insert] 
    @FileName NVARCHAR(255),
    @Status CHAR = 'I', 
    @Contents VARBINARY(max),
    @ProjectId NVARCHAR(255) = NULL, 
    @Owner NVARCHAR(255) = NULL, 
    @ParentFileId INT = NULL,
    @ArgosDeploymentId INT = NULL,
    @CollarParameterId INT = NULL,
    @CollarManufacturer NVARCHAR(255) = NULL, 
    @CollarId NVARCHAR(255) = NULL, 
    @FileId INT OUTPUT,
    @Format CHAR OUTPUT,
    @UploadDate DATETIME2(7) OUTPUT,
    @UserName NVARCHAR(128) OUTPUT,
    @Sha1Hash VARBINARY(8000) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Get the name of the caller
    DECLARE @Caller SYSNAME = ORIGINAL_LOGIN();

    --Fix Text Inputs
    SET @FileName           = NULLIF(@FileName,'')
    SET @ProjectId          = NULLIF(@ProjectId,'')
    SET @Owner              = NULLIF(@Owner,'')
    SET @CollarManufacturer = NULLIF(@CollarManufacturer,'')
    SET @CollarId           = NULLIF(@CollarId,'')

    -- Fix the defaults
    IF @Owner IS NULL AND @ProjectId IS NULL
        SET @Owner = @Caller

    -- Validate permission for this operation
    -- The caller must be in the ArgosProcessor role
    -- OR if @Owner is non-null the caller must be the owner or the owner's assistant (owner may not be a PI, but that will be caught with ref integrity).
    -- OR if @ProjectID is non-null the caller must be the project PI or editor on the project
    IF     NOT EXISTS (SELECT 1 
                         FROM sys.database_role_members AS RM 
                         JOIN sys.database_principals AS U 
                           ON RM.member_principal_id = U.principal_id 
                         JOIN sys.database_principals AS R 
                           ON RM.role_principal_id = R.principal_id 
                        WHERE U.name = @Caller AND R.name = 'ArgosProcessor') -- Not in the ArgosProcessor Role
       AND @Caller != @Owner  -- Not the file's owner
       AND NOT EXISTS (SELECT 1
                         FROM ProjectInvestigatorAssistants
                        WHERE Assistant = @Caller AND ProjectInvestigator = @Owner) -- Not the file Owner's Assistant
       AND dbo.IsProjectEditor(@ProjectId, @Caller) = 0  -- Not an editor on the project
    BEGIN
        DECLARE @message1 nvarchar(200)
        IF @ProjectId IS NOT NULL
            SET @message1 = 'You ('+@Caller+') are not the principal investigator or an editor of this file''s project ('+@ProjectId+').';
        ELSE
            SET @message1 = 'Either you ('+@Caller+') or the owner (' + @Owner + ') are not a principal investigator.';
        RAISERROR(@message1, 18, 0)
        RETURN (1)
    END

    -- Get the format of the file, and determine if this format has Argos data (to determine processing required)
    SET @Format = [dbo].[FileFormat](@Contents)
    IF NOT EXISTS (SELECT 1 FROM dbo.LookupCollarFileFormats WHERE Code = @Format)
    BEGIN
        DECLARE @message2 nvarchar(100) = 'Invalid parameter: The contents of this file is not in a format the system recognizes.';
        RAISERROR(@message2, 18, 0)
        RETURN (1)
    END

    -- Do the update, the rest of the integrity checks are done in the trigger
    INSERT INTO dbo.CollarFiles ([FileName], [ProjectId], [Owner], [CollarManufacturer], [CollarId], [Format], [Status], [Contents], [ParentFileId], [ArgosDeploymentId], [CollarParameterId])
                         VALUES (@FileName,  @ProjectId,  @Owner,  @CollarManufacturer,  @CollarId,  @Format,  @Status,  @Contents,  @ParentFileId,  @ArgosDeploymentId,  @CollarParameterId)
    SET @FileId = SCOPE_IDENTITY();
    SELECT @UploadDate = [UploadDate], @UserName = [UserName], @Sha1Hash = [Sha1Hash] FROM dbo.CollarFiles WHERE FileId = @FileId
END


GO
GRANT EXECUTE ON [dbo].[CollarFile_Insert] TO [ArgosProcessor] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[CollarFile_Insert] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ===============================================
-- Author:      Regan Sarwas
-- Create date: March 2, 2012
-- Description: Updates the editable fields of a CollarFile
-- ===============================================
CREATE PROCEDURE [dbo].[CollarFile_Update] 
    @FileId INT, 
    @FileName NVARCHAR(255) = NULL,
    @Status CHAR(1),
    @CollarManufacturer NVARCHAR(255),
    @CollarId NVARCHAR(255),
    @ProjectId NVARCHAR(255),
    @Owner NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    -- Get the name of the caller
    DECLARE @Caller sysname = ORIGINAL_LOGIN();

    --Fix Text Inputs
    SET @FileName           = NULLIF(@FileName,'')
    SET @CollarManufacturer = NULLIF(@CollarManufacturer,'')
    SET @CollarId           = NULLIF(@CollarId,'')
    SET @ProjectId          = NULLIF(@ProjectId,'')
    SET @Owner              = NULLIF(@Owner,'')

    -- Validate permission for this operation
    -- Verify we are updating a valid collar
    DECLARE @CurrentOwner NVARCHAR(255) = NULL
    DECLARE @CurrentProjectId NVARCHAR(255) = NULL
    SELECT @CurrentOwner = [Owner], @CurrentProjectId = [ProjectId] FROM [dbo].[CollarFiles] WHERE [FileId] = @FileId;
    IF @CurrentOwner IS NULL AND @CurrentProjectId IS NULL
    BEGIN
        DECLARE @message1 nvarchar(100) = 'Invalid parameter: FileId ' + CAST(@FileId AS VARCHAR(10)) + ' was not found in the CollarFiles table';
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END

    -- caller must be old owner or their assistant 
    IF @CurrentOwner IS NOT NULL
       AND @caller != @CurrentOwner
       AND NOT EXISTS (SELECT 1 FROM ProjectInvestigatorAssistants
                        WHERE Assistant = @Caller AND ProjectInvestigator = @CurrentOwner) -- Not the file Owner's Assistant
    BEGIN
        DECLARE @message2 nvarchar(100) = 'You ('+@Caller+') are not the owner (' + @CurrentOwner + ') (or their assistant) of the file.';
        RAISERROR(@message2, 18, 0)
        RETURN 2
    END

    -- caller must be new owner or their assistant 
    IF @Owner IS NOT NULL
       AND @caller != @Owner
       AND NOT EXISTS (SELECT 1 FROM ProjectInvestigatorAssistants
                        WHERE Assistant = @Caller AND ProjectInvestigator = @Owner) -- Not the file Owner's Assistant
    BEGIN
        DECLARE @message3 nvarchar(100) = 'You ('+@Caller+') are not the new owner (' + @Owner + ') (or their assistant) of the file.';
        RAISERROR(@message3, 18, 0)
        RETURN 3
    END

    -- caller must be old project PI or a project editor 
    IF @CurrentProjectId IS NOT NULL
       AND dbo.IsProjectEditor(@CurrentProjectId, @Caller) = 0  -- Not an editor on the current project
    BEGIN
        DECLARE @message4 nvarchar(100) = 'You ('+@Caller+') are not the principal investigator, assistant or  an editor of this file''s project ('+@CurrentProjectId+').';
        RAISERROR(@message4, 18, 0)
        RETURN 4
    END

    -- caller must be new project PI or a project editor 
    IF @ProjectId IS NOT NULL
       AND dbo.IsProjectEditor(@ProjectId, @Caller) = 0  -- Not an editor on the new project
    BEGIN
        DECLARE @message5 nvarchar(100) = 'You ('+@Caller+') are not the principal investigator, assistant or  an editor of this file''s project ('+@ProjectId+').';
        RAISERROR(@message5, 18, 0)
        RETURN 5
    END

    -- I use NULL to indicate don't change value only on non-null fields.
    -- The defaults for nullable field should match the insert SP.
    -- If the client does not want to change a nullable field they must provide the original value.
    IF @FileName IS NULL
        SELECT @FileName = [FileName] FROM [dbo].[CollarFiles] WHERE [FileId] = @FileId;

    -- Do the update
    UPDATE [dbo].[CollarFiles]
       SET [FileName] = @FileName, [Status] = @Status, [CollarManufacturer] = @CollarManufacturer,
           [CollarId] = @CollarId, [ProjectId] = @ProjectId, [Owner] = @Owner
     WHERE [FileId] = @FileId

END



GO
GRANT EXECUTE ON [dbo].[CollarFile_Update] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Regan Sarwas
-- Create date: March 2, 2012
-- Description:	Deletes the fixes for a CollarFile
--              Useful when changing status.
-- =============================================
CREATE PROCEDURE [dbo].[CollarFixes_Delete] 
	@FileId int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	-- This is not executed directly, only by CollarFile_Delete & CollarFile_UpdateStatus 

	DELETE FROM [dbo].[CollarFixes] WHERE [FileID] = @FileId;
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Regan Sarwas
-- Create date: March 2, 2012
-- Description:	Add the fixes for a CollarFile Data
-- =============================================
CREATE PROCEDURE [dbo].[CollarFixes_Insert] 
    @FileId INT,
    @DeploymentId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- This is not executed directly, only by CollarFile_Insert & CollarFile_Update 

    -- if FileId is not found, format is unknown or file is Inactive, then no fixes will be added.
    DECLARE @Format CHAR = NULL
    SELECT @Format = Format FROM CollarFiles WHERE FileId = @FileId AND [Status] = 'A'

    
    IF @Format = 'A'  -- Store on board
    BEGIN
        INSERT INTO dbo.CollarFixes (FileId, LineNumber, CollarManufacturer, CollarId, FixDate, Lat, Lon)
         SELECT I.FileId, I.LineNumber, F.CollarManufacturer, F.CollarId,
                CONVERT(datetime2, I.[Date]+ ' ' + ISNULL(I.[Time],'')),
                CONVERT(float, I.Latitude),
                CASE WHEN CONVERT(numeric(10,5),I.Longitude) % 360.0 < -180 THEN 360 + (CONVERT(numeric(10,5),I.Longitude) % 360.0)
                     WHEN CONVERT(numeric(10,5),I.Longitude) % 360.0 > 180 THEN (CONVERT(numeric(10,5),I.Longitude) % 360.0) - 360
                     ELSE CONVERT(numeric(10,5),I.Longitude) % 360.0 END
           FROM dbo.CollarDataTelonicsGen3StoreOnBoard as I INNER JOIN CollarFiles as F 
             ON I.FileId = F.FileId
          WHERE F.[Status] = 'A'
            AND I.[Fix Status] = 'Fix Available'
            AND I.FileId = @FileId
    END
    
    IF @Format = 'B'  -- Debevek Sub file, same as a 'G' file, except it is for a single Collar
    BEGIN
        INSERT INTO dbo.CollarFixes (FileId, LineNumber, CollarManufacturer, CollarId, FixDate, Lat, Lon)
         SELECT I.FileId, I.LineNumber, F.CollarManufacturer, F.CollarId,
                CONVERT(datetime2, I.[FixDate]+ ' ' + ISNULL(I.[FixTime],'')),
                I.LatWGS84, I.LonWGS84
           FROM dbo.CollarDataDebevekFormat as I
     INNER JOIN CollarFiles as F 
             ON I.FileId = F.FileId
          WHERE F.[Status] = 'A'
            AND I.FileId = @FileId
            AND I.LatWGS84 IS NOT NULL AND I.LonWGS84 IS NOT NULL
            AND I.[FixDate] IS NOT NULL
    END
    
    /*
    The use of AquisitionTime and not GpsFixTime is confusing, but apperantly correct
    of the 810,428 records collected as of 8/23/2012,
    GpsFixDate is non null in only 9205 records (GpsFixattempt = Succeeded(2D) or Succeeded(3D))
    In those cases, GpsFixDate == AcquisitionTime
    AcquisitionTime is never null when GpsFixAttempt == Succeed (110,090 records),
    but is null when GpsFixAttemp == Failed (71 records)
    
    There are some cases where Acquisition time is bogus (in the future),
    I've manually hidden those locations, but I'm not sure the correct strategy.
    */
    
    IF @Format = 'C'  -- Telonics Gen4 Condensed Convertor Format
    BEGIN
        INSERT INTO dbo.CollarFixes (FileId, LineNumber, CollarManufacturer, CollarId, FixDate, Lat, Lon)
         SELECT I.FileId, I.LineNumber, F.CollarManufacturer, F.CollarId,
                CONVERT(datetime2, I.[AcquisitionTime]),
                CONVERT(float, I.GpsLatitude), CONVERT(float, I.GpsLongitude)
           FROM dbo.CollarDataTelonicsGen4 as I INNER JOIN CollarFiles as F 
             ON I.FileId = F.FileId
          WHERE F.[Status] = 'A'
            AND I.FileId = @FileId
            AND I.GpsLatitude IS NOT NULL AND I.GpsLongitude IS NOT NULL
            AND I.[AcquisitionTime] IS NOT NULL -- Note: GPSFixTime is often NULL while AcquisitionTime is not
            AND I.[AcquisitionTime] < dbo.UtcTime(F.UploadDate)  -- Ignore some bogus (obviously future) fix dates
            AND (I.[ArgosLatitude] IS NULL OR I.[ArgosLatitude] NOT LIKE '%[%]')  -- Some Iridium/Telonics records are not normal location records (they have a percent in this column)
            AND -90 <= CONVERT(float, I.GpsLatitude) AND CONVERT(float, I.GpsLatitude) <= 90
            AND -180 <= CONVERT(float, I.GpsLongitude) AND CONVERT(float, I.GpsLongitude) <= 180
    END
    
    IF @Format = 'D'  -- Telonics Gen3 Format

    -- FixStatus: Bad, Good, Unavailable, Err:CCC-FFF, Invalid.
    -- Only use FixStatus = Good.
    -- Exception:  If Fix #1 has been declared Bad, fixes 2-6  must be considered Bad as well even if their status is listed as Good.
    -- This is necessary since fixes 2-6 are reconstructed using information in Fix #1.
    
    -- Date format is YYYY.MM.DD; Time Format is HH:MM, unless there is GPS error information then HH:MM:SS
    -- Lat/Long are decimal degrees with 4 decimal places, and are either signed, or have alpha (N,S,E,W) suffix - user options
    -- Lat/Long with Alpha suffix in source file are converted to signed prefix by parser during insert to CollarData table 
    
    BEGIN
        -- find valid packages (sequential fixes 1-6 at a specific date/time)
        -- turns out transmission date/time is not a unique identifier - there are multiple packages during a transmission.
        -- need to identify fixes 2-6 that need to be eliminated by location in file; i.e. do not
        -- add lines 2-6 after a bad fix 1.
        -- caution: in a package fixes 2 to 6 are optional. 
        
        -- step 1 find line numbers for valid first fixes
        SELECT LineNumber INTO #ValidFirstFix  
          FROM [Animal_Movement].[dbo].[CollarDataTelonicsGen3]
         WHERE FileId = @FileId and FixNum = 1 and FixQual = 'Good'
          
        -- step 2 find line numbers associated with a good first fixes
        /*
        SELECT C.LineNumber INTO #ValidLines  
          FROM [Animal_Movement].[dbo].[CollarDataTelonicsGen3] AS C 
          INNER JOIN  #ValidFirstFix AS F
            ON (C.LineNumber = F.LineNumber)
            OR (C.LineNumber = F.LineNumber+1 AND C.FixNum = 2)
            OR (C.LineNumber = F.LineNumber+2 AND C.FixNum = 3)
            OR (C.LineNumber = F.LineNumber+3 AND C.FixNum = 4)
            OR (C.LineNumber = F.LineNumber+4 AND C.FixNum = 5)
            OR (C.LineNumber = F.LineNumber+5 AND C.FixNum = 6)
          WHERE C.FileId = @FileId
        */
        
    -- By my observations, if any fix is bad, then all subsequent fixes in that package (1-6) are suspect,
    -- and should be eliminated, hence the new logic below:
    
        SELECT LineNumber INTO #ValidLines FROM #ValidFirstFix  

        SELECT C.LineNumber INTO #ValidSecondFix  
          FROM [Animal_Movement].[dbo].[CollarDataTelonicsGen3] AS C 
    INNER JOIN  #ValidFirstFix AS F
            ON (C.LineNumber = F.LineNumber + 1 AND C.FixNum = 2)
         WHERE FileId = @FileId and FixQual <> 'Bad'
        INSERT INTO #ValidLines (LineNumber) SELECT LineNumber FROM #ValidSecondFix  

        SELECT C.LineNumber INTO #ValidThirdFix  
          FROM [Animal_Movement].[dbo].[CollarDataTelonicsGen3] AS C 
    INNER JOIN  #ValidSecondFix AS F
            ON (C.LineNumber = F.LineNumber + 1 AND C.FixNum = 3)
         WHERE FileId = @FileId and FixQual <> 'Bad'
        INSERT INTO #ValidLines (LineNumber) SELECT LineNumber FROM #ValidThirdFix  

        SELECT C.LineNumber INTO #ValidFourthFix  
          FROM [Animal_Movement].[dbo].[CollarDataTelonicsGen3] AS C 
    INNER JOIN  #ValidThirdFix AS F
            ON (C.LineNumber = F.LineNumber + 1 AND C.FixNum = 4)
         WHERE FileId = @FileId and FixQual <> 'Bad'
        INSERT INTO #ValidLines (LineNumber) SELECT LineNumber FROM #ValidFourthFix  

        SELECT C.LineNumber INTO #ValidFifthFix  
          FROM [Animal_Movement].[dbo].[CollarDataTelonicsGen3] AS C 
    INNER JOIN  #ValidFourthFix AS F
            ON (C.LineNumber = F.LineNumber + 1 AND C.FixNum = 5)
         WHERE FileId = @FileId and FixQual <> 'Bad'
        INSERT INTO #ValidLines (LineNumber) SELECT LineNumber FROM #ValidFifthFix  

        SELECT C.LineNumber INTO #ValidSixthFix  
          FROM [Animal_Movement].[dbo].[CollarDataTelonicsGen3] AS C 
    INNER JOIN  #ValidFifthFix AS F
            ON (C.LineNumber = F.LineNumber + 1 AND C.FixNum = 6)
         WHERE FileId = @FileId and FixQual <> 'Bad'
        INSERT INTO #ValidLines (LineNumber) SELECT LineNumber FROM #ValidSixthFix  
        
        -- step 3 add valid fixes to the fixes table.
        INSERT INTO dbo.CollarFixes (FileId, LineNumber, CollarManufacturer, CollarId, FixDate, Lat, Lon)
        SELECT I.FileId, I.LineNumber, F.CollarManufacturer, F.CollarId,
               CONVERT(datetime2, I.[FixDate]+ ' ' + ISNULL(I.[FixTime],'')),
               CONVERT(float, I.Latitude), CONVERT(float, I.Longitude)
          FROM dbo.CollarDataTelonicsGen3 as I
    INNER JOIN CollarFiles as F 
            ON I.FileId = F.FileId
    INNER JOIN #ValidLines AS V
            ON I.LineNumber = V.LineNumber
         WHERE F.[Status] = 'A'
           AND I.FileId = @FileId
           AND I.[FixQual] = 'Good'
           AND I.[FixDate] <> 'Error'
           AND I.Latitude IS NOT NULL AND I.Longitude IS NOT NULL
           AND I.[Longitude] <> 'Error' AND I.[Latitude] <> 'Error'
    END
        
    IF @Format = 'E' -- Telonics email format
    -- GPS fixes in the email are converted with an external application to formats 'C' or 'D'
    -- This adds the Argos PTT locations to the Fixes table (only for non-GPS collars)
    BEGIN
        INSERT INTO dbo.CollarFixes (FileId, LineNumber, CollarManufacturer, CollarId, FixDate, Lat, Lon)
         SELECT @FileId, MIN(I.LineNumber), C.CollarManufacturer, C.CollarId,
                I.LocationDate, I.Latitude,
                CASE WHEN CONVERT(numeric(10,5),I.Longitude) % 360.0 < -180 THEN 360 + (CONVERT(numeric(10,5),I.Longitude) % 360.0)
                     WHEN CONVERT(numeric(10,5),I.Longitude) % 360.0 > 180 THEN (CONVERT(numeric(10,5),I.Longitude) % 360.0) - 360
                     ELSE CONVERT(numeric(10,5),I.Longitude) % 360.0 END
           FROM dbo.CollarDataArgosEmail AS I
     INNER JOIN CollarFiles AS F 
             ON I.FileId = F.FileId
     INNER JOIN ArgosDeployments AS D
             ON I.platformId = D.PlatformId
            AND (D.StartDate IS NULL OR D.StartDate < I.TransmissionDate)
            AND (D.EndDate IS NULL OR I.TransmissionDate < D.EndDate)
     INNER JOIN Collars AS C
             ON C.CollarManufacturer = D.CollarManufacturer AND C.CollarId = D.CollarId
          WHERE F.[Status] = 'A'
            AND I.FileId = @FileId
            AND I.Latitude BETWEEN -90 AND 90
            AND I.Longitude IS NOT NULL
            AND I.LocationDate < F.UploadDate  -- Ignore some bogus (obviously future) fix dates
            AND C.HasGps = 0
            AND (@DeploymentId IS NULL OR @DeploymentId = D.DeploymentId)
       GROUP BY C.CollarManufacturer, C.CollarId, I.LocationDate, I.Latitude, I.Longitude
    END
    
    IF @Format = 'F'  -- Argos Web Services Format
    -- GPS fixes in the raw data of a AWS file are converted with an external application to formats 'C' or 'D'
    -- This adds the Argos PTT locations to the Fixes table (only for non-GPS collars)
    BEGIN
        INSERT INTO dbo.CollarFixes (FileId, LineNumber, CollarManufacturer, CollarId, FixDate, Lat, Lon)
        SELECT I.FileId, I.LineNumber, C.CollarManufacturer, C.CollarId,
               CONVERT(datetime2, I.[locationDate]),
               CONVERT(float, I.latitude), CONVERT(float, I.longitude)
          FROM dbo.CollarDataArgosWebService as I INNER JOIN CollarFiles as F 
            ON I.FileId = F.FileId
    INNER JOIN ArgosDeployments AS D
            ON I.platformId = D.PlatformId
           AND (D.StartDate IS NULL OR D.StartDate < I.locationDate)
           AND (D.EndDate IS NULL OR I.locationDate < D.EndDate)
    INNER JOIN Collars AS C
            ON C.CollarManufacturer = D.CollarManufacturer AND C.CollarId = D.CollarId
         WHERE F.[Status] = 'A'
           AND I.FileId = @FileId
           AND I.latitude IS NOT NULL AND I.longitude IS NOT NULL
           AND I.[locationDate] IS NOT NULL
           AND I.[locationDate] < F.UploadDate  -- Ignore some bogus (obviously future) fix dates
           AND C.HasGps = 0
           AND (@DeploymentId IS NULL OR @DeploymentId = D.DeploymentId)
    END

    --IF @Format = 'G'  -- Debevek Format
    -- nothing to do for this format
    
    --IF @Format = 'H'  -- Gen4 Datalog (store-on-board) Format
    -- nothing to do for this format
    
    --IF @Format = 'I'  -- Telonics Iridium Email Format
    -- nothing to do for this format
    
    --IF @Format = 'J'  -- VHF Format
    -- nothing to do for this format
    
    --IF @Format = 'K'  -- Old Argos PPT Email Format
    -- nothing to do for this format
    
    --IF @Format = 'L'  -- Old Argos PPT CSV Format
    -- nothing to do for this format

    IF @Format = 'M'  -- Televilt Format
    BEGIN
        INSERT INTO dbo.CollarFixes (FileId, LineNumber, CollarManufacturer, CollarId, FixDate, Lat, Lon)
        SELECT I.FileId, I.LineNumber, C.CollarManufacturer, C.CollarId,
               CONVERT(datetime2, I.[FixDate] + ' ' + ISNULL(I.[FixTime],'')),
               CONVERT(float, I.Lat), CONVERT(float, I.Lon)
          FROM dbo.CollarDataTelevilt as I INNER JOIN CollarFiles as F 
            ON I.FileId = F.FileId
    INNER JOIN Collars AS C
            ON C.CollarManufacturer = F.CollarManufacturer AND C.CollarId = F.CollarId
         WHERE F.[Status] = 'A'
           AND I.FileId = @FileId
           AND I.Lat IS NOT NULL AND I.Lon IS NOT NULL
           AND I.[FixDate] IS NOT NULL
           AND CONVERT(datetime2, I.[FixDate]) < F.UploadDate  -- Ignore some bogus (obviously future) fix dates
    END

    --IF @Format = 'N'  -- Lotek/IridiumFormat
	-- One Time manual processing (Lotek would not release specs on Iridium file format,
	-- so we had to use website download and direct collar downloads and process by hand.
	-- If we use Lotek/Iridium again (I hope not), we will need to revisit this.
    --BEGIN
    --    INSERT INTO dbo.CollarFixes (FileId, LineNumber, CollarManufacturer, CollarId, FixDate, Lat, Lon)
    --    SELECT I.FileId, I.LineNumber, 'Lotek', I.Device_ID,
    --          Convert(Datetime2(0), (CONVERT(datetime, CONVERT(decimal(12,7), I.DateTime_GMT)-2))),
    --           CONVERT(float, I.Latitude), CONVERT(float, I.Longitude)
    --      FROM dbo.CollarDataLotekIridium as I INNER JOIN CollarFiles as F 
    --        ON I.FileId = F.FileId
    --     WHERE F.[Status] = 'A'
    --       AND I.FileId = @FileId
    --       AND I.Latitude IS NOT NULL AND I.Longitude IS NOT NULL
    --       AND I.DateTime_GMT IS NOT NULL
    --END

	IF @Format = 'O'  -- Vectronic location data downloaded via the HTTP Wildlife API v2
    BEGIN
        INSERT INTO dbo.CollarFixes (FileId, LineNumber, CollarManufacturer, CollarId, FixDate, Lat, Lon)
        SELECT I.FileId, I.LineNumber, 'Vectronic', I.[idCollar],
               CONVERT(datetime2, I.[acquisitionTime]),
               CONVERT(float, I.[latitude]), CONVERT(float, I.[longitude])
          FROM dbo.CollarDataVectronicPosition as I INNER JOIN CollarFiles as F 
            ON I.FileId = F.FileId
         WHERE F.[Status] = 'A'
           AND I.FileId = @FileId
           AND I.[latitude] IS NOT NULL AND I.[longitude] IS NOT NULL
           AND I.[acquisitionTime] IS NOT NULL
    END

END



GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Regan Sarwas
-- Create date: March 3, 2012
-- Description:	Adjusts which of several conflicting fixes will be the active one.
--
-- A fix may hide zero or one other fix
-- A fix may be hidden by zero or one other fix
-- A fix cannot hide itself
-- Therefore in a set of fixes that share the same collar and fixdate,
-- there is one and only one fix that is not hidden
-- there is one and only one fix that is not hidding any other fixes
-- =============================================
CREATE PROCEDURE [dbo].[CollarFixes_UpdateUnhideFix] 
    @FixId int = Null
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Get some information about the fix to update
    DECLARE @HiddenBy  INT;
    DECLARE @ProjectId NVARCHAR(255);
     SELECT @HiddenBy  = [HiddenBy],
            @ProjectId = [ProjectId]
       FROM [dbo].[CollarFixes] AS F1
      INNER JOIN [dbo].[CollarFiles] AS F2
         ON F1.FileId = F2.FileId
      WHERE [FixId] = @FixId;

    IF @ProjectId IS NULL
    BEGIN
        DECLARE @message1 nvarchar(100) = 'Invalid parameter: (' + @FixId + ') was not found in the CollarFixes table';
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END
    -- If ProjectId was found, then HiddenBy is guaranteed.

    -- Get the name of the caller
    DECLARE @Caller sysname = ORIGINAL_LOGIN();

    -- Validate permission for this operation
    -- The caller must be authorized to edit the project data
    IF dbo.IsProjectEditor(@ProjectId, @Caller) = 0
    BEGIN
        DECLARE @message2 nvarchar(200) = 'You ('+@Caller+') must be the principal investigator, assistant or editor on this project ('+@ProjectId+') to update this file.';
        RAISERROR(@message2, 18, 0)
        RETURN (1)
    END
    
    IF  @HiddenBy IS NULL
    BEGIN
        RETURN 0  --Nothing to do.
    END
    
    -- Fix is hidden, and we have permissions, lets do it.
    DECLARE @ConflictingFixes TABLE
    (
        FixId int,
        HiddenBy int
    );
    DECLARE @NotHidden int;
    DECLARE @NotHider int;

    BEGIN TRY
        BEGIN TRAN

            INSERT INTO @ConflictingFixes 
                SELECT DISTINCT F1.FixId, F1.HiddenBy FROM dbo.CollarFixes AS F1
                INNER JOIN dbo.CollarFixes AS F2
                   ON F1.CollarManufacturer = F2.CollarManufacturer
                  AND F1.CollarId = F2.CollarId
                  AND F1.FixDate = F2.FixDate
                WHERE F1.FixId != F2.FixId AND (F1.FixId = @FixId OR F2.FixId = @FixId) ORDER BY F1.FixId;

            -- The fix in the set that is not hidden
            SET @NotHidden = (SELECT FixId FROM @ConflictingFixes WHERE HiddenBy IS NULL)
            -- The fix in the set that is not hidding any other fixes
            SET @NotHider = (SELECT FixId FROM @ConflictingFixes WHERE FixId NOT IN (SELECT HiddenBy FROM @ConflictingFixes WHERE HiddenBy IS NOT NULL))
            
            --Hide the currently Un Hidden fix with the fix not hidding any other fixes
            --(creates circlular loop of hidden-ness)
            UPDATE dbo.CollarFixes SET HiddenBy = @NotHider WHERE FixId = @NotHidden
            --Unhide the selected fix 
            UPDATE dbo.CollarFixes SET HiddenBy = NULL WHERE FixId = @FixId

        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
            ROLLBACK TRANSACTION;
        EXEC [dbo].[Utility_RethrowError]
        RETURN 1
    END CATCH
END




GO
GRANT EXECUTE ON [dbo].[CollarFixes_UpdateUnhideFix] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:      Regan Sarwas
-- Create date: Sept 14, 2020
-- Description: Delete a collar Key
-- =============================================
CREATE PROCEDURE [dbo].[CollarKey_Delete] 
    @CollarManufacturer NVARCHAR(255),
    @CollarId           NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    -- Get the name of the caller
    DECLARE @Caller SYSNAME = ORIGINAL_LOGIN();

    DECLARE @Manager SYSNAME
    SELECT @Manager = Manager FROM Collars WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId
    IF @Manager IS NULL
        RETURN -- Collar was not found, so it must have been deleted already

    -- Validate permission for this operation

    -- The caller must be the Manager or assistant of the collar
    IF @Manager != @Caller
        AND NOT EXISTS (SELECT 1 FROM ProjectInvestigatorAssistants
                         WHERE Assistant = @Caller AND ProjectInvestigator = @Manager)
    BEGIN
        DECLARE @message1 nvarchar(200) = 'You ('+@Caller+') must be the manager (or assistant) of this collar ('+@CollarManufacturer+'/'+@CollarId+') to delete it.';
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END


    DELETE FROM dbo.CollarKeys WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId

END

GO
GRANT EXECUTE ON [dbo].[CollarKey_Delete] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:      Regan Sarwas
-- Create date: Sept 14, 2020
-- Description: Adds a new Collar Key.
-- =============================================
CREATE PROCEDURE [dbo].[CollarKey_Insert] 
    @CollarManufacturer NVARCHAR(255),
    @CollarId           NVARCHAR(255), 
    @CollarKey          NVARCHAR(512)
AS
BEGIN
    SET NOCOUNT ON;

    -- Get the name of the caller
    DECLARE @Caller sysname = ORIGINAL_LOGIN();

    DECLARE @Manager SYSNAME
    SELECT @Manager = Manager FROM Collars WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId
    IF @Manager IS NULL
     BEGIN
        DECLARE @message1 nvarchar(200) = 'There is no such collar ('+@CollarManufacturer+'/'+@CollarId+').';
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END
	
    -- Validate permission for this operation
    -- If the caller is not the owner or assistant then error and return
    IF @Manager != @Caller
       AND NOT EXISTS (SELECT 1
                         FROM ProjectInvestigatorAssistants
                        WHERE Assistant = @Caller AND ProjectInvestigator = @Manager)
    BEGIN
        DECLARE @message2 nvarchar(100) = 'You ('+@Caller+') must be the manager (or assistant) of the collar to add this key.'
        RAISERROR(@message2, 18, 0)
        RETURN 1
    END

    INSERT INTO dbo.CollarKeys ([CollarManufacturer], [CollarId], [CollarKey])
                     VALUES (@CollarManufacturer, @CollarId, @CollarKey)

END

GO
GRANT EXECUTE ON [dbo].[CollarKey_Insert] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:      Regan Sarwas
-- Create date: Sept 14, 2020
-- Description: Updates a Collar Key
-- =============================================
CREATE PROCEDURE [dbo].[CollarKey_Update] 
    @CollarManufacturer NVARCHAR(255),
    @CollarId           NVARCHAR(255), 
    @CollarKey          NVARCHAR(512) 
AS
BEGIN
    SET NOCOUNT ON;

    -- Get the name of the caller
    DECLARE @Caller SYSNAME = ORIGINAL_LOGIN();

    -- Verify this is an existing collar by getting the manager (needed for permissions check)
    DECLARE @Manager SYSNAME
    SELECT @Manager = Manager FROM dbo.Collars WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId
    IF @Manager IS NULL
    BEGIN
        DECLARE @message1 nvarchar(100) = 'There is no such collar ('+@CollarManufacturer+'/'+@CollarId+').';
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END

    -- Validate permission for this operation
    -- The caller must be the manager or assistant of the collar
    IF @Manager != @Caller
       AND NOT EXISTS (SELECT 1 FROM ProjectInvestigatorAssistants
                        WHERE Assistant = @Caller AND ProjectInvestigator = @Manager)
    BEGIN
        DECLARE @message2 nvarchar(200) = 'You ('+@Caller+') must be the manager (or assistant) of this collar.';
        RAISERROR(@message2, 18, 0)
        RETURN 2
    END

    UPDATE dbo.CollarKeys SET [CollarKey] = @CollarKey
                     WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId;

END

GO
GRANT EXECUTE ON [dbo].[CollarKey_Update] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Regan Sarwas
-- Create date: November 9, 2012
-- Description:	Deletes a CollarParameter from the database
-- =============================================
CREATE PROCEDURE [dbo].[CollarParameter_Delete] 
    @ParameterId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Get the name of the caller
    DECLARE @Caller sysname = ORIGINAL_LOGIN();

    DECLARE @OldFileOwner sysname
    DECLARE @OldCollarOwner sysname

    -- Validate permission for this operation

    -- The caller must be an editor in the database - handled by execute permissions

    -- Verify that the relationship exists 
    --   this is done now to get the old owner and avoid a confusing permission error	
        SELECT @OldCollarOwner = C.[Manager], @OldFileOwner = F.[Owner]
          FROM [dbo].[CollarParameters] AS P
    INNER JOIN [dbo].[Collars] AS C ON C.CollarManufacturer = P.CollarManufacturer AND C.CollarId = P.CollarId
     LEFT JOIN [dbo].[CollarFiles] AS F ON F.FileId = P.FileId
         WHERE [ParameterId] = @ParameterId
    IF @OldCollarOwner IS NULL
        RETURN -- quietly, there is nothing to delete, so we have finished our task.

    -- To delete the parameter assignment the caller must be 
    --    the owner or assistant of the collar
    --    the owner or assistant of the collar parameter file (if it exists)
    IF (@OldCollarOwner != @Caller
        AND NOT EXISTS (SELECT 1
                          FROM ProjectInvestigatorAssistants
                         WHERE Assistant = @Caller AND ProjectInvestigator = @OldCollarOwner))
       OR
       (@OldFileOwner IS NOT NULL AND @OldFileOwner != @Caller
        AND NOT EXISTS (SELECT 1
                          FROM ProjectInvestigatorAssistants
                         WHERE Assistant = @Caller AND ProjectInvestigator = @OldFileOwner))
    BEGIN
        DECLARE @message1 nvarchar(200) = 'You ('+@Caller+') must be the owner/assistant of the collar, and the owner/assistant of the parameter file to delete it.';
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END

    -- All other verification is handled by primary/foreign key and column constraints.
    DELETE FROM dbo.CollarParameters WHERE [ParameterId] = @ParameterId;

END

GO
GRANT EXECUTE ON [dbo].[CollarParameter_Delete] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Regan Sarwas
-- Create date: November 9, 2012
-- Description:	Adds a new collar parameter to the database.
-- =============================================
CREATE PROCEDURE [dbo].[CollarParameter_Insert] 
    @CollarManufacturer NVARCHAR(255),
    @CollarId			NVARCHAR(255), 
    @FileId				INT = NULL,
    @Gen3Period         INT = NULL,
    @StartDate			DATETIME2 = NULL, 
    @EndDate			DATETIME2 = NULL,
    @ParameterId		INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Get the name of the caller
    DECLARE @Caller sysname = ORIGINAL_LOGIN();

    DECLARE @NewFileOwner sysname
    DECLARE @NewCollarOwner sysname

    -- Validate permission for this operation

    -- The caller must be an editor in the database, handled by execute permissions
    
    -- To insert a parameter assignment the caller must be 
    --    the owner or assistant of the new collar
    --    the owner or assistant of the new collar parameter file (if it exists)
    SELECT @NewCollarOwner = Manager FROM [dbo].[Collars] WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId
    SELECT @NewFileOwner = [Owner] FROM [dbo].[CollarFiles] WHERE FileId = @FileId
    IF (@NewCollarOwner != @Caller
        AND NOT EXISTS (SELECT 1
                          FROM ProjectInvestigatorAssistants
                         WHERE Assistant = @Caller AND ProjectInvestigator = @NewCollarOwner))
       OR
       (@NewFileOwner IS NOT NULL AND @NewFileOwner != @Caller
        AND NOT EXISTS (SELECT 1
                          FROM ProjectInvestigatorAssistants
                         WHERE Assistant = @Caller AND ProjectInvestigator = @NewFileOwner))
    BEGIN
        DECLARE @message1 nvarchar(200) = 'You ('+@Caller+') must be the owner/assistant of the collar and the owner/assistant of the file to add this parameter.';
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END

    -- All other verification is handled by column constraints and a trigger.
    INSERT INTO dbo.CollarParameters ([CollarManufacturer], [CollarId], [FileId], [Gen3Period], [StartDate], [EndDate])
                              VALUES (@CollarManufacturer,  @CollarId,  @FileId,  @Gen3Period,  @StartDate,  @EndDate)
    SET @ParameterId = SCOPE_IDENTITY()
END


GO
GRANT EXECUTE ON [dbo].[CollarParameter_Insert] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Regan Sarwas
-- Create date: November 9, 2012
-- Description:	Updates a CollarParameter Assignment.
-- =============================================
CREATE PROCEDURE [dbo].[CollarParameter_Update] 
    @ParameterId		INT,
    @CollarManufacturer NVARCHAR(255) = NULL,
    @CollarId			NVARCHAR(255) = NULL, 
    @FileId				INT,
    @Gen3Period			INT,
    @StartDate			DATETIME2, 
    @EndDate			DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    -- Get the name of the caller
    DECLARE @Caller sysname = ORIGINAL_LOGIN();
    
    DECLARE @OldFileOwner sysname
    DECLARE @NewFileOwner sysname
    DECLARE @OldCollarOwner sysname
    DECLARE @NewCollarOwner sysname

    -- Validate permission for this operation
    
    -- The caller must be an editor in the database, handled by execute permissions
        
    -- Verify that the relationship exists 
    --   this is done now to get the old owner and avoid a confusing permission error	
        SELECT @OldCollarOwner = C.[Manager], @OldFileOwner = F.[Owner]
          FROM [dbo].[CollarParameters] AS P
    INNER JOIN [dbo].[Collars] AS C ON C.CollarManufacturer = P.CollarManufacturer AND C.CollarId = P.CollarId
     LEFT JOIN [dbo].[CollarFiles] AS F ON F.FileId = P.FileId
         WHERE [ParameterId] = @ParameterId
    IF @OldCollarOwner IS NULL
    BEGIN
        RAISERROR('The collar parameter you want to change was not found.', 18, 0)
        RETURN 1
    END

    -- To update a parameter assignment the caller must be 
    --    the owner or assistant of the old collar
    --    the owner or assistant of the old collar parameter file (if it exists)
    IF (@OldCollarOwner != @Caller
        AND NOT EXISTS (SELECT 1
                          FROM ProjectInvestigatorAssistants
                         WHERE Assistant = @Caller AND ProjectInvestigator = @OldCollarOwner))
       OR
       (@OldFileOwner IS NOT NULL AND @OldFileOwner != @Caller
        AND NOT EXISTS (SELECT 1
                          FROM ProjectInvestigatorAssistants
                         WHERE Assistant = @Caller AND ProjectInvestigator = @OldFileOwner))
    BEGIN
        DECLARE @message1 nvarchar(200) = 'You ('+@Caller+') must be the owner/assistant of the collar and the owner/assistant of the related file to update this parameter.';
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END


    -- Fix Defaults
    IF (@CollarManufacturer IS NULL)
        SELECT @CollarManufacturer = [CollarManufacturer] FROM CollarParameters WHERE [ParameterId] = @ParameterId
    IF (@CollarId IS NULL)
        SELECT @CollarId = [CollarId] FROM CollarParameters WHERE [ParameterId] = @ParameterId
    
        
    -- To update a parameter assignment the caller must be 
    --    the owner or assistant of the new collar
    --    the owner or assistant of the new collar parameter file (if it exists)
    SELECT @NewCollarOwner = Manager FROM [dbo].[Collars] WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId
    SELECT @NewFileOwner = [Owner] FROM [dbo].[CollarFiles] WHERE FileId = @FileId
    IF (@NewCollarOwner != @Caller
        AND NOT EXISTS (SELECT 1
                          FROM ProjectInvestigatorAssistants
                         WHERE Assistant = @Caller AND ProjectInvestigator = @NewCollarOwner))
       OR
       (@NewFileOwner IS NOT NULL AND @NewFileOwner != @Caller
        AND NOT EXISTS (SELECT 1
                          FROM ProjectInvestigatorAssistants
                         WHERE Assistant = @Caller AND ProjectInvestigator = @NewFileOwner))
    BEGIN
        DECLARE @message2 nvarchar(200) = 'You ('+@Caller+') must be the owner/assistant of the collar and the owner/assistant of the related file to update this parameter.';
        RAISERROR(@message2, 18, 0)
        RETURN 2
    END

    -- All other verification is handled by constraints and trigger logic.
    UPDATE [dbo].[CollarParameters] SET [CollarManufacturer] = @CollarManufacturer
                                       ,[CollarId] = @CollarId
                                       ,[FileId] = @FileId
                                       ,[Gen3Period] = @Gen3Period
                                       ,[StartDate] = @StartDate
                                       ,[EndDate] = @EndDate
                                 WHERE  [ParameterId] = @ParameterId

END


GO
GRANT EXECUTE ON [dbo].[CollarParameter_Update] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Regan Sarwas
-- Create date: November 9, 2012
-- Description:	Deletes a CollarParameterFile from the database
-- =============================================
CREATE PROCEDURE [dbo].[CollarParameterFile_Delete] 
    @FileId int
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Get the name of the caller
    DECLARE @Caller sysname = ORIGINAL_LOGIN();

    -- Validate permission for this operation

    -- The caller must be an editor in the database - handled by execute permissions

    -- Verify that the file exists (this is done now to avoid the following check issuing a confusing error)
    IF NOT EXISTS (SELECT 1 FROM [dbo].[CollarParameterFiles] WHERE [FileId] = @FileId)
    BEGIN
        RETURN -- exit quietly, there is nothing to do.
    END


    -- The caller must be the owner or an assistant to the owner of the collar parameter file
    IF NOT EXISTS (   SELECT 1 FROM [dbo].[CollarParameterFiles] AS P
                   LEFT JOIN [dbo].[ProjectInvestigatorAssistants] AS A
                          ON A.ProjectInvestigator = P.[Owner]
                       WHERE [FileId] = @FileId
                         AND ([Owner] = @Caller OR A.[Assistant] = @Caller)
                  )
    BEGIN
        DECLARE @message1 nvarchar(200) = 'You ('+@Caller+') must be the owner or an assistant to the owner to delete this file.';
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END

    -- All other verification is handled by declarative constraints and triggers.
    
    DELETE FROM dbo.CollarParameterFiles WHERE FileId = @FileId;
END


GO
GRANT EXECUTE ON [dbo].[CollarParameterFile_Delete] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Regan Sarwas
-- Create date: October 12, 2012
-- Description:	Adds a new collar parameter file to the database.
-- =============================================
CREATE PROCEDURE [dbo].[CollarParameterFile_Insert] 
    @Owner NVARCHAR(255), 
    @FileName NVARCHAR(255),
    @Format CHAR, 
    @Contents VARBINARY(max),
    @Status CHAR = 'A',
    @FileId INT OUTPUT,
    @UploadDate DATETIME2(7) OUTPUT,
    @UploadUser NVARCHAR(128) OUTPUT,
    @Sha1Hash VARBINARY(8000) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Get the name of the caller
    DECLARE @Caller sysname = ORIGINAL_LOGIN();

    IF @Owner IS NULL
        SET @Owner = @Caller

    -- Validate permission for this operation

    -- The caller must be an editor in the database - handled by execute permissions

    -- The caller must be the owner, or an assistant for the owner
    IF @Owner != @Caller
       AND NOT EXISTS (SELECT 1
                         FROM ProjectInvestigatorAssistants
                        WHERE Assistant = @Caller AND ProjectInvestigator = @Owner)
    BEGIN
        DECLARE @message1 nvarchar(200) = 'You ('+@Caller+') must be the owner ('+@Owner+') or an assistant to the owner to insert this file.';
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END

    -- All other verification is handled by primary/foreign key and column constraints.
    INSERT INTO dbo.CollarParameterFiles ([Owner], [FileName], [Format], [Contents], [Status])
         VALUES (@Owner, @FileName, @Format, @Contents, @Status)

    -- Get the output values.  This is key for Linq to Sql to get the full state of the new object
    SET @FileId = SCOPE_IDENTITY();
    SELECT @UploadDate = [UploadDate], @UploadUser = [UploadUser], @Sha1Hash = [Sha1Hash] FROM dbo.CollarParameterFiles WHERE FileId = @FileId

END

GO
GRANT EXECUTE ON [dbo].[CollarParameterFile_Insert] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Regan Sarwas
-- Create date: November 9, 2012
-- Description:	Updates a collar parameter file in the database.
--              You cannot update the contents, upload date/user,
--              if those are wrong, delete and re-upload the file.
-- =============================================
CREATE PROCEDURE [dbo].[CollarParameterFile_Update] 
    @FileId INT,
    @Owner NVARCHAR(255) = NULL, 
    @FileName NVARCHAR(255) = NULL,
    @Format CHAR = NULL, 
    @Status CHAR = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Get the name of the caller
    DECLARE @Caller sysname = ORIGINAL_LOGIN();

    -- Validate permission for this operation

    -- The caller must be an editor in the database - handled by execute permissions

    -- Verify that the file exists (this is done now to avoid the following check issuing a confusing error)
    DECLARE @OldOwner sysname
    SELECT @OldOwner = [Owner] FROM [dbo].[CollarParameterFiles] WHERE [FileId] = @FileId
    IF @OldOwner IS NULL
    BEGIN
        RAISERROR('The file you want to update was not found.', 18, 0)
        RETURN 1
    END

    -- The caller must be the old owner, or an assistant for the old owner
    IF @OldOwner != @Caller
       AND NOT EXISTS (SELECT 1
                         FROM ProjectInvestigatorAssistants
                        WHERE Assistant = @Caller AND ProjectInvestigator = @OldOwner)
    BEGIN
        DECLARE @message1 nvarchar(200) = 'You ('+@Caller+') must be the owner ('+@OldOwner+') or an assistant to the owner to update this file.';
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END

    -- The caller must be the new owner, or an assistant for the new owner
    IF @Owner IS NULL
        SELECT @Owner = @OldOwner

    IF @Owner != @Caller
       AND NOT EXISTS (SELECT 1
                         FROM ProjectInvestigatorAssistants
                        WHERE Assistant = @Caller AND ProjectInvestigator = @Owner)
    BEGIN
        DECLARE @message2 nvarchar(200) = 'You ('+@Caller+') must be the new owner ('+@Owner+') or an assistant to the new owner to change the owner.';
        RAISERROR(@message2, 18, 0)
        RETURN 2
    END

    --Fix Defaults
    -- I use NULL to indicate don't change value only on non-null fields.
    -- The defaults for nullable field should match the insert SP.
    -- If the client does not want to change a nullable field they must provide the original value.
    IF @FileName IS NULL
        SELECT @FileName = [FileName] FROM dbo.CollarParameterFiles WHERE FileId = @FileId;

    IF @Format IS NULL
        SELECT @Format = [Format] FROM dbo.CollarParameterFiles WHERE FileId = @FileId;

    IF @Status IS NULL
        SELECT @Status = [Status] FROM dbo.CollarParameterFiles WHERE FileId = @FileId;


    -- All other verification is handled by primary/foreign key and column constraints.
    UPDATE dbo.CollarParameterFiles SET [Owner] = @Owner,
                                        [FileName] = @FileName,
                                        [Format] = @Format,
                                        [Status] = @Status
                                  WHERE [FileId] = @FileId

END


GO
GRANT EXECUTE ON [dbo].[CollarParameterFile_Update] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:      Regan Sarwas
-- Create date: Sept 14, 2020
-- Description: Delete a collar Sensor
-- =============================================
CREATE PROCEDURE [dbo].[CollarSensor_Delete] 
    @CollarManufacturer NVARCHAR(255),
    @CollarId           NVARCHAR(255),
    @SensorCode         NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    -- Get the name of the caller
    DECLARE @Caller SYSNAME = ORIGINAL_LOGIN();

    DECLARE @Manager SYSNAME
    SELECT @Manager = Manager FROM Collars WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId
    IF @Manager IS NULL
        RETURN -- Collar was not found, so it must have been deleted already

    -- Validate permission for this operation

    -- The caller must be the Manager or assistant of the collar
    IF @Manager != @Caller
        AND NOT EXISTS (SELECT 1 FROM ProjectInvestigatorAssistants
                         WHERE Assistant = @Caller AND ProjectInvestigator = @Manager)
    BEGIN
        DECLARE @message1 nvarchar(200) = 'You ('+@Caller+') must be the manager (or assistant) of this collar ('+@CollarManufacturer+'/'+@CollarId+') to delete it.';
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END


    DELETE FROM dbo.CollarSensors WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId AND SensorCode = @SensorCode

END

GO
GRANT EXECUTE ON [dbo].[CollarSensor_Delete] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:      Regan Sarwas
-- Create date: Sept 14, 2020
-- Description: Adds a new Collar Sensor.
-- =============================================
CREATE PROCEDURE [dbo].[CollarSensor_Insert] 
    @CollarManufacturer NVARCHAR(255),
    @CollarId           NVARCHAR(255), 
    @SensorCode         NVARCHAR(255),
	@IsActive           BIT = 0
AS
BEGIN
    SET NOCOUNT ON;

    -- Get the name of the caller
    DECLARE @Caller sysname = ORIGINAL_LOGIN();

    DECLARE @Manager SYSNAME
    SELECT @Manager = Manager FROM Collars WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId
    IF @Manager IS NULL
     BEGIN
        DECLARE @message1 nvarchar(200) = 'There is no such collar ('+@CollarManufacturer+'/'+@CollarId+').';
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END
	
    -- Validate permission for this operation
    -- If the caller is not the owner or assistant then error and return
    IF @Manager != @Caller
       AND NOT EXISTS (SELECT 1
                         FROM ProjectInvestigatorAssistants
                        WHERE Assistant = @Caller AND ProjectInvestigator = @Manager)
    BEGIN
        DECLARE @message2 nvarchar(100) = 'You ('+@Caller+') must be the manager (or assistant) of the collar to add this sensor.'
        RAISERROR(@message2, 18, 0)
        RETURN 1
    END

    INSERT INTO dbo.CollarSensors ([CollarManufacturer], [CollarId], [SensorCode], [IsActive])
                     VALUES (@CollarManufacturer, @CollarId, @SensorCode, @IsActive)

END

GO
GRANT EXECUTE ON [dbo].[CollarSensor_Insert] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:      Regan Sarwas
-- Create date: Sept 14, 2020
-- Description: Updates a Collar Sensor
-- =============================================
CREATE PROCEDURE [dbo].[CollarSensor_Update] 
    @CollarManufacturer NVARCHAR(255),
    @CollarId           NVARCHAR(255), 
    @SensorCode         NVARCHAR(255),
	@IsActive           BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Get the name of the caller
    DECLARE @Caller SYSNAME = ORIGINAL_LOGIN();

    -- Verify this is an existing collar by getting the manager (needed for permissions check)
    DECLARE @Manager SYSNAME
    SELECT @Manager = Manager FROM dbo.Collars WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId
    IF @Manager IS NULL
    BEGIN
        DECLARE @message1 nvarchar(100) = 'There is no such collar ('+@CollarManufacturer+'/'+@CollarId+').';
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END

	--Fix Defaults
    -- I use NULL to indicate "don't change value" only on non-null fields.
    -- The defaults for nullable field should match the insert SP.
    -- If the client does not want to change a nullable field they must provide the original value.
    IF @IsActive IS NULL
        SELECT @IsActive = [IsActive] FROM dbo.CollarSensors WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId AND SensorCode = @SensorCode;

    -- Validate permission for this operation
    -- The caller must be the manager or assistant of the collar
    IF @Manager != @Caller
       AND NOT EXISTS (SELECT 1 FROM ProjectInvestigatorAssistants
                        WHERE Assistant = @Caller AND ProjectInvestigator = @Manager)
    BEGIN
        DECLARE @message2 nvarchar(200) = 'You ('+@Caller+') must be the manager (or assistant) of this collar.';
        RAISERROR(@message2, 18, 0)
        RETURN 2
    END

    UPDATE dbo.CollarSensors SET IsActive = @IsActive
                     WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId AND SensorCode = @SensorCode;

END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[Location_Added] 
	@Project NVARCHAR(255), 
	@Animal  NVARCHAR(255), 
	@Time    DATETIME2
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE
		@PrevTime DATETIME2,
		@NextTime DATETIME2;
		
	-- Typically an inserted location (new or updated) does not have any ajoining movements (it's new)
	-- however, if this location is part of a larger operation, then a prior location may connect a movement
	-- to this point.  We can safely delete any connected movements, before begining the standard process.
	EXEC [dbo].[Location_Deleted] @Project, @Animal, @Time
	
	SET @PrevTime = [dbo].[StartOfOverlappingMovement](@Project, @Animal, @Time);
	SET @NextTime = [dbo].[EndOfMovement](@Project, @Animal, @PrevTime);
	
	EXEC [dbo].[Movement_Delete] @Project, @Animal, @PrevTime, @NextTime;
	 
	IF @PrevTime IS NULL
	BEGIN
		SET @PrevTime = [dbo].[PreviousLocationTime](@Project, @Animal, @Time);
		SET @NextTime = [dbo].[NextLocationTime](@Project, @Animal, @Time);
	END
	
	EXEC [dbo].[Movement_Insert] @Project, @Animal, @PrevTime, @Time; 
	EXEC [dbo].[Movement_Insert] @Project, @Animal, @Time, @NextTime; 
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Location_Deleted] 
	@Project NVARCHAR(255), 
	@Animal  NVARCHAR(255), 
	@Time    DATETIME2
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE
		@PrevTime DATETIME2,
		@NextTime DATETIME2;
	
	SET @PrevTime = [dbo].[StartOfPriorConnectedMovement](@Project, @Animal, @Time);
	SET @NextTime = [dbo].[EndOfFollowingConnectedMovement](@Project, @Animal, @Time);
	
	EXEC [dbo].[Movement_Delete] @Project, @Animal, @PrevTime, @Time; 
	EXEC [dbo].[Movement_Delete] @Project, @Animal, @Time, @NextTime; 
	EXEC [dbo].[Movement_Insert] @Project, @Animal, @PrevTime, @NextTime; 
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Regan Sarwas
-- Create date: March 2, 2012
-- Description:	Updates the Status of a Location
-- =============================================
CREATE PROCEDURE [dbo].[Location_UpdateStatus] 
    @ProjectId NVARCHAR(255) = NULL, 
    @AnimalId  NVARCHAR(255) = NULL, 
    @FixDate   DATETIME2     = NULL,
    @Status    CHAR          = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Get the name of the caller
    DECLARE @Caller sysname = ORIGINAL_LOGIN();

    -- Validate permission for this operation
    -- The caller must be authorized to edit the project data
    IF dbo.IsProjectEditor(@ProjectId, @Caller) = 0
    BEGIN
        DECLARE @message1 nvarchar(200) = 'You ('+@Caller+') must be the principal investigator, assistant or editor on this project ('+@ProjectId+') to update this location.';
        RAISERROR(@message1, 18, 0)
        RETURN (1)
    END
    
    UPDATE [Locations]
       SET [Status] = @Status
     WHERE [ProjectId] = @ProjectId
       AND [AnimalId] = @AnimalId
       AND [FixDate] = @FixDate;
END

GO
GRANT EXECUTE ON [dbo].[Location_UpdateStatus] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Movement_Delete] 
	@Project NVARCHAR(255), 
	@Animal  NVARCHAR(255), 
	@PrevTime DATETIME2,
	@NextTime DATETIME2
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE FROM [dbo].[Movements]
	   	  WHERE [ProjectId] = @Project
		    AND [AnimalId] = @Animal
	        AND [StartDate] = @PrevTime
	        AND [EndDate] = @NextTime;
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[Movement_Insert] 
	@Project NVARCHAR(255), 
	@Animal  NVARCHAR(255), 
	@PrevTime DATETIME2,
	@NextTime DATETIME2
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE
		@PrevPoint GEOGRAPHY,
		@NextPoint GEOGRAPHY;
		
	IF @Animal IS NOT NULL AND @PrevTime IS NOT NULL AND @NextTime IS NOT NULL
	BEGIN
		SET @PrevPoint = [dbo].[GetLocationGeography](@Project, @Animal, @PrevTime);
		SET @NextPoint = [dbo].[GetLocationGeography](@Project, @Animal, @NextTime);
		-- In some cases, the location we need may be deleted (Ah, the joys of processing a multirow trigger)
		-- In these cases, we can use the yet to be deleted movement,
		-- if there is still no point, then we do not need to add a movement
		IF @PrevPoint IS NULL
			SET @PrevPoint = [dbo].[GetMovementEndGeography](@Project, @Animal, @PrevTime);
		IF @NextPoint IS NULL
			SET @NextPoint = [dbo].[GetMovementStartGeography](@Project, @Animal, @NextTime);
		IF @PrevPoint IS NOT NULL AND @NextPoint IS NOT NULL
			EXEC [dbo].[Movement_Insert_Sub] @Project, @Animal, @PrevTime, @NextTime, @PrevPoint, @NextPoint;
	END
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Movement_Insert_Sub] 
	@Project    NVARCHAR(255), 
	@Animal     NVARCHAR(255), 
	@StartTime  DATETIME2,
	@EndTime    DATETIME2,
	@StartPoint GEOGRAPHY,
	@EndPoint   GEOGRAPHY
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE
		@Vector   GEOGRAPHY,
		@Duration FLOAT,
		@Distance FLOAT,
		@Speed    FLOAT;

	IF @StartPoint.STEquals(@EndPoint) = 0   -- 0 is False
		BEGIN
			SET @Vector = GEOGRAPHY::STLineFromText('LINESTRING(' +
									 STR(@StartPoint.Long,13,8) + ' ' +
									 STR(@StartPoint.Lat,13,8) + ', ' +
									 STR(@EndPoint.Long,13,8)  + ' ' +
									 STR(@EndPoint.Lat,13,8)  + ')', 4326);
			SET @Distance = @Vector.STLength();
			--Alternative methods - TODO test which is fastest
			--SET @Vector = geography::Parse(geometry::Parse(@StartPoint.STUnion(@EndPoint).ToString()).STConvexHull().ToString())
			--SET @Distance = @StartPoint.STDistance(@EndPoint);
		END
	ELSE
		BEGIN
			SET @Vector = @StartPoint;
			SET @Distance = 0;			
		END;
	
	SET @Duration = DATEDIFF(second, @StartTime, @EndTime)/60.0/60.0;
	-- since datediff returns an int, this may round twords zero
	IF @Duration = 0
		BEGIN
			SET @Duration = DATEDIFF(nanosecond, @StartTime, @EndTime)/60.0/60.0/1000000000.0;
		END
	ELSE
	-- Since times come from locations they must be unique for an animal, therefore @StartTime != @EndTime
	-- The max precision of datetime2 is 7 digits (100 ns), therefore, it is impossible for the delta to be less than 1 nanosecond.
	-- I'm keeping this check in here for extra safety.	
	IF @Duration = 0
		BEGIN
			SET @Speed = -1;
		END
	ELSE
		BEGIN
			SET @Speed = @Distance/@Duration;
		END
	
	INSERT INTO [dbo].[Movements]
				([ProjectId], [AnimalId], [StartDate], [EndDate], [Duration], [Distance], [Speed], [Shape])
		 VALUES (@Project, @Animal, @StartTime, @EndTime, @Duration, @Distance, @Speed, @Vector);
END




GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Regan Sarwas
-- Create date: March 2, 2012
-- Description: Deletes a project
-- =============================================
CREATE PROCEDURE [dbo].[Project_Delete] 
    @ProjectId NVARCHAR(255) = NULL 
AS
BEGIN
    SET NOCOUNT ON;

    -- Get the name of the caller
    DECLARE @Caller sysname = ORIGINAL_LOGIN();

    -- Get the Project Investigator to check permissions
    DECLARE @ProjectInvestigator sysname
    SELECT @ProjectInvestigator = ProjectInvestigator FROM [dbo].[Projects] WHERE [ProjectId] = @ProjectId
    IF @ProjectInvestigator IS NULL
        RETURN 0 --There is no project to delete, we must be done.
    
    -- You must be the ProjectInvestigator or assistant to delete the project
    IF (@ProjectInvestigator != @Caller
        AND NOT EXISTS (SELECT 1
                          FROM ProjectInvestigatorAssistants
                         WHERE Assistant = @Caller AND ProjectInvestigator = @ProjectInvestigator))
    BEGIN
        DECLARE @message1 nvarchar(100) = 'You ('+@Caller+') are not the owner (or assitant) of the ' + @ProjectId + ' project.';
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END


    -- wrap the two operations in a transaction
    BEGIN TRY
        BEGIN TRANSACTION
            DELETE FROM dbo.ProjectEditors WHERE [ProjectId] = @ProjectId
            DELETE FROM dbo.Projects WHERE [ProjectId] = @ProjectId
        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
            ROLLBACK TRANSACTION;
        EXEC [dbo].[Utility_RethrowError]
        RETURN 1
    END CATCH
    
END


GO
GRANT EXECUTE ON [dbo].[Project_Delete] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Regan Sarwas
-- Create date: March 28, 2012
-- Description: Adds a new project to the database.
-- =============================================
CREATE PROCEDURE [dbo].[Project_Insert] 
    @ProjectId           NVARCHAR(255),
    @ProjectName         NVARCHAR(255), 
    @ProjectInvestigator SYSNAME, 
    @UnitCode            NVARCHAR(255) = NULL, 
    @Description         NVARCHAR(4000) = NULL 
AS
BEGIN
    SET NOCOUNT ON;

    -- Get the name of the caller
    DECLARE @Caller SYSNAME = ORIGINAL_LOGIN();

    --Fix Text Inputs
    SET @ProjectId           = NULLIF(@ProjectId,'')
    SET @ProjectName         = NULLIF(@ProjectName,'')
    SET @UnitCode            = NULLIF(@UnitCode,'')
    SET @ProjectInvestigator = NULLIF(@ProjectInvestigator,'')
    SET @Description         = NULLIF(@Description,'')

    --default PI is the calling user
    SET @ProjectInvestigator = ISNULL(@ProjectInvestigator, @Caller) 

    -- If the caller is not a PI or assistant then error and return
    IF @ProjectInvestigator != @Caller
       AND NOT EXISTS (SELECT 1
                         FROM ProjectInvestigatorAssistants
                        WHERE Assistant = @Caller AND ProjectInvestigator = @ProjectInvestigator)
    BEGIN
        DECLARE @message1 NVARCHAR(100) = 'You ('+@Caller+') must be a project investigator (or assitant) to create a project';
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END


    INSERT INTO dbo.Projects ([ProjectId], [ProjectName], [ProjectInvestigator], [UnitCode], [Description])
         VALUES              (@ProjectId,  @ProjectName,  @ProjectInvestigator,  @UnitCode,  @Description)

END


GO
GRANT EXECUTE ON [dbo].[Project_Insert] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* Protection Model
 * ================
 * All PIs and editors need permission to run this SP. - Done.
 * PIs can only modify projects for which they are the PI or an editor. - Done.
 * Editors can only modify projects for which they are an editor. - Done.
 * No one is allowed to modify the Project Id. - Done. (to change the ID you must deleted and recreate)
 * Only the PI (or assitant) is allowed to change the PI column. - Done
 */

-- =============================================
-- Author:      Regan Sarwas
-- Create date: March 28, 2012
-- Description: Updates a project name
-- =============================================
CREATE PROCEDURE [dbo].[Project_Update] 
    @ProjectId           NVARCHAR(255),
    @ProjectName         NVARCHAR(255)  = NULL, 
    @UnitCode            NVARCHAR(255), 
    @ProjectInvestigator NVARCHAR(255) = NULL, 
    @Description         NVARCHAR(4000) 
AS
BEGIN
    SET NOCOUNT ON;

    -- Get the name of the caller
    DECLARE @Caller sysname = ORIGINAL_LOGIN();
        
    -- Check that this is a valid deployment
    --   this is done now to get the old PI and avoid a confusing permission error	
    DECLARE @OldProjectInvestigator sysname
    SELECT @OldProjectInvestigator = ProjectInvestigator FROM [dbo].[Projects] WHERE [ProjectId] = @ProjectId
    IF @OldProjectInvestigator IS NULL
    BEGIN
        DECLARE @message1 nvarchar(100) = 'Project (' + @ProjectId + ') is not an existing project.';
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END

    --Fix Defaults
    -- I use NULL to indicate don't change value only on non-null fields.
    -- The defaults for nullable field should match the insert SP.
    -- If the client does not want to change a nullable field they must provide the original value.
    SET @ProjectName = NULLIF(@ProjectName,'')
    SET @UnitCode    = NULLIF(@UnitCode,'')
    SET @ProjectInvestigator = NULLIF(@ProjectInvestigator,'')
    SET @Description = NULLIF(@Description,'')
    IF @ProjectName IS NULL
        SELECT @ProjectName = [ProjectName] FROM [dbo].[Projects] WHERE [ProjectId] = @ProjectId;
    IF @ProjectInvestigator IS NULL
        SELECT @ProjectInvestigator = [ProjectInvestigator] FROM [dbo].[Projects] WHERE [ProjectId] = @ProjectId;

    
    -- You must be the ProjectInvestigator or and Editor on the project to update it
    IF [dbo].[IsProjectEditor](@ProjectId, @Caller) = 0
    BEGIN
        DECLARE @message2 nvarchar(100) = 'You are not the owner or an editor of the ' + @ProjectId + ' project.';
        RAISERROR(@message2, 18, 0)
        RETURN 2
    END

    -- To change the PI, you must be the old PI or an assistant to the old PI
    --  and an assistant to the new PI or the new PI 
    IF @ProjectInvestigator != @OldProjectInvestigator
    BEGIN
        IF (@OldProjectInvestigator != @Caller
        AND NOT EXISTS (SELECT 1
                          FROM ProjectInvestigatorAssistants
                         WHERE Assistant = @Caller AND ProjectInvestigator = @OldProjectInvestigator))
        BEGIN
            DECLARE @message3 nvarchar(100) = 'You are not the owner or assistant of the ' + @ProjectId + ' project.';
            RAISERROR(@message3, 18, 0)
            RETURN 3
        END
        IF (@ProjectInvestigator != @Caller
        AND NOT EXISTS (SELECT 1
                          FROM ProjectInvestigatorAssistants
                         WHERE Assistant = @Caller AND ProjectInvestigator = @ProjectInvestigator))
        BEGIN
            DECLARE @message4 nvarchar(100) = 'You are not the new owner (or assistant) of the ' + @ProjectId + ' project.';
            RAISERROR(@message4, 18, 0)
            RETURN 4
        END
    END


    UPDATE dbo.Projects SET [ProjectName]         = @ProjectName,
                            [UnitCode]            = @UnitCode,
                            [ProjectInvestigator] = @ProjectInvestigator,
                            [Description]         = @Description
                      WHERE [ProjectId]           = @ProjectId

END

GO
GRANT EXECUTE ON [dbo].[Project_Update] TO [Editor] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Regan Sarwas
-- Create date: March 27, 2012
-- Description:	Removes an editor from a project
-- =============================================
CREATE PROCEDURE [dbo].[ProjectEditor_Delete] 
	@ProjectId nvarchar(255) = Null,
	@Editor sysname  = Null
AS
BEGIN
	SET NOCOUNT ON;

	-- Permissions only allow Investigators to execute this procedure
	-- The Invesitigator role has Alter Any User, and Alter Role 'Editor' permissions

	--If editor is not an editor on the project, then there is nothing to do, so return
	IF NOT EXISTS(SELECT 1 FROM [dbo].[ProjectEditors] WHERE ProjectId = @ProjectId AND Editor = @Editor)
	BEGIN
		RETURN
	END
		
	-- Get the name of the caller
	DECLARE @Caller sysname = ORIGINAL_LOGIN();
	
	-- If the caller is not the PI for the project, then error and return
	IF NOT EXISTS (SELECT 1 FROM dbo.Projects WHERE ProjectId = @ProjectId AND ProjectInvestigator = @Caller)
	BEGIN
		DECLARE @message1 nvarchar(100) = 'You ('+@Caller+') are not the principal investigator for project (' + @ProjectId + ')';
		RAISERROR(@message1, 18, 0)
		RETURN 1
	END

	-- This is the key step from the applications standpoint
	DELETE FROM [dbo].[ProjectEditors] WHERE ProjectId = @ProjectId AND Editor = @Editor
	
END

GO
GRANT EXECUTE ON [dbo].[ProjectEditor_Delete] TO [Investigator] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Regan Sarwas
-- Create date: March 27, 2012
-- Description:	Adds a new editor to a project
-- =============================================
CREATE PROCEDURE [dbo].[ProjectEditor_Insert] 
	@ProjectId nvarchar(255) = Null,
	@Editor sysname = Null
AS
BEGIN
	SET NOCOUNT ON;

	-- Get the name of the caller
	DECLARE @Caller sysname = ORIGINAL_LOGIN();
	
	--If editor is already an editor on the project, then there is nothing to do, so return
	IF EXISTS(SELECT 1 FROM [dbo].[ProjectEditors] WHERE ProjectId = @ProjectId AND Editor = @Editor)
	BEGIN
		RETURN 0
	END
	
	-- If the caller is not the PI for the project, then error and return
	IF NOT EXISTS (SELECT 1 FROM dbo.Projects WHERE ProjectId = @ProjectId AND ProjectInvestigator = @Caller)
	BEGIN
		DECLARE @message1 nvarchar(100) = 'You ('+@Caller+') are not the principal investigator for project (' + @ProjectId + ')';
		RAISERROR(@message1, 18, 0)
		RETURN 1
	END
	
	DECLARE @LoginCreated BIT = 0;
	DECLARE @UserCreated BIT = 0;
	DECLARE @RoleMemberAdded BIT = 0;
	
	BEGIN TRY
		-- If the Editor is not a database user then add them.
		IF NOT EXISTS (select 1 from sys.database_principals WHERE type = 'U' AND name = @Editor)
		BEGIN
			EXEC ('CREATE USER ['  + @Editor + ']') --  LOGIN defaults to the same name
			SET @UserCreated = 1
		END
			
		-- If the Editor user does not have the editor role, then add it.
		IF NOT EXISTS (SELECT 1 from sys.database_role_members as U 
					   INNER JOIN sys.database_principals AS P1  
					   ON U.member_principal_id = P1.principal_id
					   INNER JOIN sys.database_principals AS P2 
					   ON U.role_principal_id = p2.principal_id 
					   WHERE p1.name = @Editor AND p2.name = 'Editor' )
		BEGIN
			--EXEC ('ALTER ROLE Editor ADD MEMBER ['  + @Editor + ']')  --SQL2012 syntax
			EXEC sp_addrolemember 'Editor', @Editor
			SET @RoleMemberAdded = 1
		END
			
		-- Add them to the Project Editors table
		INSERT [dbo].[ProjectEditors] (ProjectId, Editor) VALUES (@ProjectId, @Editor)
	END TRY
	BEGIN CATCH
		-- Roleback operations
		IF @RoleMemberAdded = 1
		BEGIN
			EXEC sp_droprolemember 'Editor', @Editor
		END
		IF @UserCreated = 1
		BEGIN
			EXEC ('DROP USER ['  + @Editor + ']')
		END
		EXEC [dbo].[Utility_RethrowError]
		RETURN 1
	END CATCH
END

GO
GRANT EXECUTE ON [dbo].[ProjectEditor_Insert] TO [Investigator] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Regan Sarwas
-- Create date: March 27, 2012
-- Description:	Removes an Investigator from the database
-- =============================================
CREATE PROCEDURE [dbo].[ProjectInvestigator_Delete_SA] 
	@Investigator sysname = Null
AS
BEGIN
	SET NOCOUNT ON;

	-- This can only be run be the Sysadmin
	-- This script is mostly to remind the SA of the steps, so there is no error checking.

	-- Remove the user to the Investigator table
	DELETE FROM [dbo].[ProjectInvestigators] WHERE [Login] = @Investigator;

	-- Remove the user from the Investigator role
	EXEC sp_droprolemember 'Investigator', @Investigator
	
	-- remove the user from the db, unless they are an editor
	IF EXISTS(SELECT 1 FROM [dbo].[ProjectEditors] WHERE [Editor] = @Investigator)
	BEGIN
		RETURN
	END
	EXEC ('DROP USER ['  + @Investigator + ']')

	-- remove the login (if it is not used anywhere else)	
	-- If the editor login does not map to a user on have any other databases, then remove the login
	DECLARE	@cmd nvarchar(500) = 'USE ?; IF EXISTS (SELECT 1 FROM sys.database_principals WHERE name = ''' + @Investigator + ''') raiserror('''',18,0)'
	DECLARE @Found int = 0
	begin try
		EXEC sp_MSforeachdb @cmd;
	end try
	BEGIN CATCH
		SET @Found = 1
	END CATCH

	IF @Found = 0
	BEGIN
		-- make sure the login does not have a server role
		IF NOT EXISTS (select 1 from sys.server_role_members AS R INNER JOIN sys.server_principals AS P 
					   ON R.member_principal_id = P.principal_id where P.name = @Investigator)
		BEGIN
			EXEC ('DROP LOGIN ['  + @Investigator + ']')
		END
	END
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[ProjectInvestigator_Insert_SA]
	@Login NVARCHAR(255), 
	@Name  NVARCHAR(255), 
	@Email NVARCHAR(255),
	@Phone NVARCHAR(255)
AS
BEGIN
	SET NOCOUNT ON

	-- This can only be run be the Sysadmin
	-- This script is mostly to remind the SA of the steps, so there is no error checking.

	-- create a login
	IF NOT EXISTS (select 1 from sys.server_principals where name = @Login)
	BEGIN
		EXEC ('CREATE LOGIN [' + @Login + '] FROM WINDOWS')
	END

	-- create a db user
	IF NOT EXISTS (select 1 from sys.database_principals WHERE type = 'U' AND name = @Login)
	BEGIN
		EXEC ('CREATE USER ['  + @Login + ']') --  LOGIN defaults to the same name
	END

	-- Add the user to the Investigator role
	IF NOT EXISTS (SELECT 1 from sys.database_role_members as U 
				   INNER JOIN sys.database_principals AS P1  
				   ON U.member_principal_id = P1.principal_id
				   INNER JOIN sys.database_principals AS P2 
				   ON U.role_principal_id = p2.principal_id 
				   WHERE p1.name = @Login AND p2.name = 'Investigator' )
	BEGIN
		EXEC sp_addrolemember 'Investigator', @Login
	END

	-- Add the user to the Investigator table
	INSERT INTO dbo.ProjectInvestigators ([Login],[Name],[Email],[Phone])
		 VALUES (@Login, @Name, @Email, @Phone);
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Regan Sarwas
-- Create date: April 11, 2012
-- Description: Updates a project investigator
-- =============================================
CREATE PROCEDURE [dbo].[ProjectInvestigator_Update] 
    @Login SYSNAME       = NULL,
    @Name  NVARCHAR(255) = NULL, 
    @Email NVARCHAR(255) = NULL, 
    @Phone NVARCHAR(255) = NULL 
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Get the name of the caller
    DECLARE @Caller SYSNAME = ORIGINAL_LOGIN();

    -- Fix Text Inputs
    SET @Login = NULLIF(@Login,'')
    SET @Name  = NULLIF(@Name,'')
    SET @Email = NULLIF(@Email,'')
    SET @Phone = NULLIF(@Phone,'')

    -- Fix Defaults
    -- I use NULL to indicate don't change value only on non-null fields.
    -- The defaults for nullable field should match the insert SP.
    -- If the client does not want to change a nullable field they must provide the original value.
    SET @Login = ISNULL(@Login,@Caller)
    IF @Name IS NULL
        SELECT @Name = [Name] FROM [dbo].[ProjectInvestigators] WHERE [Login] = @Login;
    IF @Email IS NULL
        SELECT @Email = [Email] FROM [dbo].[ProjectInvestigators] WHERE [Login] = @Login;
    IF @Phone IS NULL
        SELECT @Phone = [Phone] FROM [dbo].[ProjectInvestigators] WHERE [Login] = @Login;

    -- Verify this is an existing project investigator
    IF @Name IS NULL
    BEGIN
        DECLARE @message1 nvarchar(100) = 'There is no project investigator with a login of ' + @Login + '.';
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END

    -- You must be the ProjectInvestigator or assistant to update the ProjectInvestigator
    IF (@Login != @Caller
        AND NOT EXISTS (SELECT 1
                          FROM ProjectInvestigatorAssistants
                         WHERE Assistant = @Caller AND ProjectInvestigator = @Login))
    BEGIN
        DECLARE @message2 nvarchar(100) = 'You can only update your own data (or someone you assist).';
        RAISERROR(@message2, 18, 0)
        RETURN 1
    END


    UPDATE [dbo].[ProjectInvestigators] SET [Name]  = @Name,
                                            [Email] = @Email,
                                            [Phone] = @Phone
                                      WHERE [Login] = @Login

END

GO
GRANT EXECUTE ON [dbo].[ProjectInvestigator_Update] TO [Investigator] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Regan Sarwas
-- Create date: April 25, 2013
-- Description:	Removes an Assistant from a PI
-- =============================================
CREATE PROCEDURE [dbo].[ProjectInvestigatorAssistant_Delete] 
    @ProjectInvestigator sysname,
    @Assistant           sysname
AS
BEGIN
    SET NOCOUNT ON;

    -- Permissions only allow Investigators to execute this procedure
    -- The Invesitigator role has Alter Any User, and Alter Role 'Editor' permissions

    --If this isn't a valid assistant, then there is nothing to do, so return
    IF NOT EXISTS(SELECT 1 FROM [dbo].[ProjectInvestigatorAssistants] WHERE ProjectInvestigator = @ProjectInvestigator AND Assistant = @Assistant)
    BEGIN
        RETURN
    END
        
    -- Get the name of the caller
    DECLARE @Caller sysname = ORIGINAL_LOGIN();
    
    -- If the caller is not the PI or the assistant, then error and return
    IF @ProjectInvestigator <> @Caller AND @Assistant <> @Caller
    BEGIN
        DECLARE @message1 nvarchar(100) = 'You ('+@Caller+') cannot delete an assistant for another principal investigator (' + @ProjectInvestigator + ')';
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END

    -- This is the key step from the applications standpoint
    DELETE FROM [dbo].[ProjectInvestigatorAssistants] WHERE ProjectInvestigator = @ProjectInvestigator AND Assistant = @Assistant
    
END



GO
GRANT EXECUTE ON [dbo].[ProjectInvestigatorAssistant_Delete] TO [Investigator] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Regan Sarwas
-- Create date: April 25, 2013
-- Description:	Adds a new assistant for a PI
-- =============================================
CREATE PROCEDURE [dbo].[ProjectInvestigatorAssistant_Insert] 
    @ProjectInvestigator sysname,
    @Assistant           sysname
AS
BEGIN
    SET NOCOUNT ON;

    -- Get the name of the caller
    DECLARE @Caller sysname = ORIGINAL_LOGIN();
    
    --If editor is already an editor on the project, then there is nothing to do, so return
    IF EXISTS(SELECT 1 FROM [dbo].[ProjectInvestigatorAssistants] WHERE ProjectInvestigator = @ProjectInvestigator AND Assistant = @Assistant)
    BEGIN
        RETURN 0
    END
    
    -- If the caller is not the assistant's PI, then error and return
    IF @ProjectInvestigator <> @Caller
    BEGIN
        DECLARE @message1 nvarchar(100) = 'You ('+@Caller+') cannot create an assistant for another principal investigator (' + @ProjectInvestigator + ')';
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END
    
    DECLARE @LoginCreated BIT = 0;
    DECLARE @UserCreated BIT = 0;
    DECLARE @RoleMemberAdded BIT = 0;
    
    BEGIN TRY
        -- If the Assistant is not a database user then add them.
        IF NOT EXISTS (select 1 from sys.database_principals WHERE type = 'U' AND name = @Assistant)
        BEGIN
            EXEC ('CREATE USER ['  + @Assistant + ']') --  LOGIN defaults to the same name
            SET @UserCreated = 1
        END
            
        -- If the Assistant does not have the editor role, then add it.
        IF NOT EXISTS (SELECT 1 from sys.database_role_members as U 
                       INNER JOIN sys.database_principals AS P1  
                       ON U.member_principal_id = P1.principal_id
                       INNER JOIN sys.database_principals AS P2 
                       ON U.role_principal_id = p2.principal_id 
                       WHERE p1.name = @Assistant AND p2.name = 'Editor' )
        BEGIN
            --EXEC ('ALTER ROLE Editor ADD MEMBER ['  + @Editor + ']')  --SQL2012 syntax
            EXEC sp_addrolemember 'Editor', @Assistant
            SET @RoleMemberAdded = 1
        END
            
        -- Add them to the Project Editors table
        INSERT [dbo].[ProjectInvestigatorAssistants] (ProjectInvestigator, Assistant) VALUES (@ProjectInvestigator, @Assistant)
    END TRY
    BEGIN CATCH
        -- Roleback operations
        IF @RoleMemberAdded = 1
        BEGIN
            EXEC sp_droprolemember 'Editor', @Assistant
        END
        IF @UserCreated = 1
        BEGIN
            EXEC ('DROP USER ['  + @Assistant + ']')
        END
        EXEC [dbo].[Utility_RethrowError]
        RETURN 1
    END CATCH
END


GO
GRANT EXECUTE ON [dbo].[ProjectInvestigatorAssistant_Insert] TO [Investigator] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Regan Sarwas
-- Create date: March 20, 2012
-- Description: Updates the settings table
-- =============================================
CREATE PROCEDURE [dbo].[Settings_Update] 
    @Key   nvarchar(30),
    @Value nvarchar(500)
AS
BEGIN
    SET NOCOUNT ON;

    -- Get the name of the caller
    DECLARE @Caller SYSNAME = ORIGINAL_LOGIN();

    --Fix Text Inputs
    SET @Key   = NULLIF(@Key,'')
    SET @Value = NULLIF(@Value,'')
    
    IF @Key = 'project' AND NOT EXISTS (SELECT 1 FROM [dbo].[Projects] WHERE ProjectId = @Value)
    BEGIN
        DECLARE @message1 nvarchar(100) = 'Project (' + @Value + ') is not an existing project';
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END

    IF     @Key NOT IN ('project', 'collar_manufacturer', 'filter_projects', 'species', 'file_format',
                        'parameter_file_format', 'wants_email', 'ignore_ctn_suffix', 'othervalidkeys...') --Add valid keys to this list
       AND @Key NOT IN (select 'collar_model_' + CollarManufacturer from LookupCollarManufacturers)
    BEGIN
        DECLARE @message2 nvarchar(100) = 'Invalid key (' + @Key + ') for settings';
        RAISERROR(@message2, 18, 0)
        RETURN 2
    END


    IF EXISTS (SELECT 1 FROM [dbo].[Settings] WHERE [Username] = @Caller AND [Key] = @Key)
        UPDATE [dbo].[Settings] SET [Value] = @Value WHERE [Username] = @Caller AND [Key] = @Key
    ELSE
        INSERT [dbo].[Settings] ([Username], [Key], [Value]) VALUES (@Caller, @Key, @Value) 

END


GO
GRANT EXECUTE ON [dbo].[Settings_Update] TO [Viewer] AS [dbo]
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[Summerize]
	@fileId [int]
WITH EXECUTE AS OWNER
AS
EXTERNAL NAME [SqlServer_Files].[SqlServer_Files.CollarFileInfo].[Summerize]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Create the stored procedure to generate an error using 
-- RAISERROR. The original error information is used to
-- construct the msg_str for RAISERROR.
-- must be called from a CATCH block
-- pilfered from http://msdn.microsoft.com/en-us/library/ms179296.aspx
CREATE PROCEDURE [dbo].[Utility_RethrowError] AS
    -- Return if there is no error information to retrieve.
    IF ERROR_NUMBER() IS NULL
        RETURN;

    DECLARE 
        @ErrorMessage    NVARCHAR(4000),
        @ErrorNumber     INT,
        @ErrorSeverity   INT,
        @ErrorState      INT,
        @ErrorLine       INT,
        @ErrorProcedure  NVARCHAR(200);

    -- Assign variables to error-handling functions that 
    -- capture information for RAISERROR.
    SELECT 
        @ErrorNumber = ERROR_NUMBER(),
        @ErrorSeverity = ERROR_SEVERITY(),
        @ErrorState = ERROR_STATE(),
        @ErrorLine = ERROR_LINE(),
        @ErrorProcedure = ISNULL(ERROR_PROCEDURE(), '-');

    -- Build the message string that will contain original
    -- error information.
    SELECT @ErrorMessage = 
        N'Error %d, Level %d, State %d, Procedure %s, Line %d, ' + 
            'Message: '+ ERROR_MESSAGE();

    -- Raise an error: msg_str parameter of RAISERROR will contain
    -- the original error information.
    RAISERROR 
        (
        @ErrorMessage, 
        @ErrorSeverity, 
        1,               
        @ErrorNumber,    -- parameter: original error number.
        @ErrorSeverity,  -- parameter: original error severity.
        @ErrorState,     -- parameter: original error state.
        @ErrorProcedure, -- parameter: original error procedure name.
        @ErrorLine       -- parameter: original error line number.
        );
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Regan Sarwas
-- Create date: June 3, 2012
-- Description:	Add/Remove Locations when Mortality Date is updated
-- =============================================
CREATE TRIGGER [dbo].[AfterAnimalUpdate] 
   ON  [dbo].[Animals] 
   AFTER UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	
	--Mortality date is the change that this trigger cares about.
	--Note: this just means that MortalityDate was included in the update statement,
	--      it does not mean that the value in this column has changed for all the
	--      rows updated. 
	IF UPDATE ([MortalityDate])
	BEGIN
	
		-- triggers always execute in the context of a transaction
		-- so the following code is all or nothing.

/* LOGIC:
   If old mortality date is null then
     if new mortality date is null
       no change, so do nothing
     else new mortality date is NOT null
       we may lose locations, but never gain any.
       delete locations where new mortality date < fix date 
   else
     if new mortality date is null
       if old mortality date is null
         no change, so do nothing     
       else old mortality date is NOT null
         we may gain some locations, but never lose any.
         add location where old mortality date < fix date
     else
       if new mortality date = old mortality date
         no change, so do nothing     
       else 
         delete locations where new mortality date < fix date and fix date < old mortality date
         add locations where old mortality date < fix date and fix date < new mortality date

  This can be simplified to:
    delete locations where new mortality date < fix date AND (fix date < old mortality date OR old mortality date is null) 
       add locations where old mortality date < fix date AND (fix date < new mortality date OR new mortality date is null)
*/
    
		DELETE L FROM dbo.Locations as L
				   -- Join to the animal that owns this location
		   INNER JOIN deleted as D
				   ON L.ProjectId = D.ProjectId
				  AND L.AnimalId = D.AnimalId
		   INNER JOIN inserted as I
				 -- To get the new mortality date, we need to link inserted and deleted animals by PK
				   ON I.ProjectId = D.ProjectId
				  AND I.AnimalId = D.AnimalId
				   -- These are the Locations to delete:
				WHERE I.MortalityDate < L.FixDate AND (L.FixDate < D.MortalityDate OR D.MortalityDate IS NULL) 
				

		INSERT INTO Locations (ProjectId, AnimalId, FixDate, Location, FixId)
			 SELECT I.ProjectId, I.AnimalId, F.FixDate, geography::Point(F.Lat, F.Lon, 4326), F.FixId
			   FROM dbo.CollarFixes AS F
			     -- Join to CollarDeployments to get to the animal
		 INNER JOIN CollarDeployments AS C
				 ON F.CollarManufacturer = C.CollarManufacturer
				AND F.CollarId = C.CollarId
				AND C.DeploymentDate < F.FixDate
				AND (F.FixDate < C.RetrievalDate OR C.RetrievalDate IS NULL)
				 -- Join these deployments to the animal being updated
		 INNER JOIN inserted AS I
				 ON C.ProjectId = I.ProjectId
				AND C.AnimalId = I.AnimalId
	     INNER JOIN deleted as D
				 -- To get the new mortality date, we need to link inserted and deleted animals by PK
			     ON I.ProjectId = D.ProjectId
			    AND I.AnimalId = D.AnimalId
				 -- These are the Fixes to add:
			  WHERE F.HiddenBy IS NULL
				AND D.MortalityDate < F.FixDate AND (F.FixDate < I.MortalityDate OR I.MortalityDate IS NULL) 
	END
END

GO
ALTER TABLE [dbo].[Animals] ENABLE TRIGGER [AfterAnimalUpdate]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Regan Sarwas
-- Create date: March 21, 2013
-- Description: Enforce the business rules for deleting ArgosDeployment
-- =============================================
CREATE TRIGGER [dbo].[AfterArgosDeploymentDelete] 
   ON  [dbo].[ArgosDeployments] 
   AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- collars file referencing this deployment must be deleted before
    -- deleting the deployment.  Because of issues with instead of triggers,
    -- we are putting this requirement on the client.

    -- we do not need to update issues, because the now unprocessed transmissions 
    -- (without issues) will be reflected in the query ArgosFile_NeedsPartialProcessing

    -- If this deployment is related to a non-gps collar then delete all fixes for this collar
    -- during the time of the deployment (A collar can only have one deployment at any time,
    -- so all the fixes for that collar must be associated with this deployment)
    DELETE F
      FROM CollarFixes AS F
      JOIN Collars AS C
        ON C.CollarManufacturer = F.CollarManufacturer AND C.CollarId = F.CollarId
      JOIN deleted as D
        ON D.CollarManufacturer = C.CollarManufacturer AND D.CollarId = C.CollarId
     WHERE C.HasGps = 0
       AND (F.FixDate <= D.EndDate OR D.EndDate IS NULL)
       AND (D.StartDate <= F.FixDate  OR D.StartDate IS NULL)
END

GO
ALTER TABLE [dbo].[ArgosDeployments] ENABLE TRIGGER [AfterArgosDeploymentDelete]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Regan Sarwas
-- Create date: March 21, 2013
-- Description: Validates the business rules for a new ArgosDeployment
-- =============================================
CREATE TRIGGER [dbo].[AfterArgosDeploymentInsert] 
   ON  [dbo].[ArgosDeployments] 
   AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Validate Business Rules
    -- 1. Ensure StartDate is before the EndDate
    IF EXISTS (SELECT 1
                 FROM inserted AS I
                WHERE I.EndDate <= I.StartDate  -- false if either is NULL
              )
    BEGIN
        RAISERROR('The end of the deployment must occur after the start of the deployment.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END

    -- 2a. Ensure start date is before the DisposalDate of the Collar
    IF EXISTS (SELECT 1
                 FROM inserted AS I
           INNER JOIN Collars AS C
                   ON I.CollarManufacturer = C.CollarManufacturer AND I.CollarId = C.CollarId
                WHERE C.DisposalDate < I.StartDate
              )
    BEGIN
        RAISERROR('The deployment must start before the collar is disposed.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END

    -- 2b. Ensure start is before the DisposalDate of the ArgosPlatform
    IF EXISTS (SELECT 1
                 FROM inserted AS I
           INNER JOIN ArgosPlatforms AS P
                   ON P.PlatformId = I.PlatformId
                WHERE P.DisposalDate < I.StartDate
              )
    BEGIN
        RAISERROR('The deployment must start before the Argos platfrom is disposed.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END

    -- 3. Ensure an ArgosPlatform is not deployed on multiple Collars at the same time
    -- We are checking each inserted deployment against all existing deployments, and all other new deployments	 
    IF EXISTS (SELECT 1
                 FROM inserted AS I1
            LEFT JOIN inserted AS I2
                   ON I1.PlatformId = I2.PlatformId AND I1.DeploymentId <> I2.DeploymentId
           INNER JOIN dbo.ArgosDeployments AS D
                   ON D.PlatformId = I1.PlatformId AND D.DeploymentId <> I1.DeploymentId
                WHERE dbo.DoDateRangesOverlap(D.StartDate, D.EndDate, I1.StartDate, I1.EndDate) = 1
                   OR (I2.DeploymentId IS NOT NULL AND
                       dbo.DoDateRangesOverlap(I1.StartDate, I1.EndDate, I2.StartDate, I2.EndDate) = 1)
              )
    BEGIN
        RAISERROR('Argos platforms cannot have overlapping deployment dates.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END

    -- 4. Ensure a Collar is not carrying multiple ArgosPlatforms at the same time
    -- We are checking each inserted deployment against all existing deployments, and all other new deployments 
    IF EXISTS (SELECT 1
                 FROM inserted AS I1
            LEFT JOIN inserted AS I2
                   ON I1.CollarManufacturer = I2.CollarManufacturer AND I1.CollarId = I2.CollarId AND I1.DeploymentId <> I2.DeploymentId
           INNER JOIN dbo.ArgosDeployments AS D
                   ON D.CollarManufacturer = I1.CollarManufacturer AND D.CollarId = I1.CollarId AND D.DeploymentId <> I1.DeploymentId
                WHERE dbo.DoDateRangesOverlap(D.StartDate, D.EndDate, I1.StartDate, I1.EndDate) = 1
                   OR (I2.DeploymentId IS NOT NULL AND
                       dbo.DoDateRangesOverlap(I1.StartDate, I1.EndDate, I2.StartDate, I2.EndDate) = 1)
              )
    BEGIN
        RAISERROR('Collars cannot have overlapping deployment dates.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END


    -- Process (partialy?) all files that might benefit from this deployment
    -- We do not do the processing in the trigger, as it may require an external process
    -- which would block the transaction and hold open a lock that would slow other operations.
    -- Instead, we will monitor for changes that will trigger the external processing.

    -- If a processed file had no issues, then do not reprocess.
    --    This deployment is irrelevant to the file - the file had everything it when it was processed.
    -- If a processed file had issues
    --    without mentioning the argos platform, then it was unable to link the problem to a platform, so
    --      this new information will not due any good.
    --    with the platform in this deployment, then it was probably a missing deployment.
    --      this file should be re-processed for the matching platform(s).  This can be done by
    --      removing the issue.  The (file,platform) will then be recognized in need of re-processing.
    -- If a file is unprocessed for some reason, then it is not in our perview to decide that it should
    --   be processed, so we will skip it, even if it has transmissions for this platform. (The file should get
    --   processed at some point in the future by the user or a scheduled task at which point the new parameter
    --   will be considered.)

    -- May be part of a batch insert (unlikely, and only by SA as all others must use SP)
    -- Triggers always execute in the context of a transaction, so the following code is all or nothing.

        DELETE A 
          FROM inserted AS I
    INNER JOIN ArgosFileProcessingIssues AS A
            ON A.PlatformId = I.PlatformId


    -- If this deployment is for a non-gps collar,
    -- then E & F files with transmission that overlapping the platform in this deployment
    -- should have collarfixes inserted for this just deployed collar/platform
    DECLARE @FileId INT;
    DECLARE @DeploymentId INT;
    DECLARE insert_deployment_cursor2 CURSOR FOR 
             SELECT A.FileId, I.DeploymentId
               FROM inserted AS I
         INNER JOIN Collars AS C
                 ON C.CollarManufacturer = I.CollarManufacturer AND C.CollarId = I.CollarId
         INNER JOIN ArgosFilePlatformDates AS A
                 ON A.PlatformId = I.PlatformId
         INNER JOIN CollarFiles AS F
                 ON F.FileId = A.FileId
              WHERE C.HasGps = 0
                AND (F.Format = 'E' OR F.Format = 'F')
                AND dbo.DoDateRangesOverlap(A.FirstTransmission, A.LastTransmission, I.StartDate, I.EndDate) = 1

    OPEN insert_deployment_cursor2;

    FETCH NEXT FROM insert_deployment_cursor2 INTO @FileId, @DeploymentId;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        EXEC dbo.CollarFixes_Insert @FileId, @DeploymentId
        FETCH NEXT FROM insert_deployment_cursor2 INTO @FileId, @DeploymentId;
    END
    CLOSE insert_deployment_cursor2;
    DEALLOCATE insert_deployment_cursor2;

END

GO
ALTER TABLE [dbo].[ArgosDeployments] ENABLE TRIGGER [AfterArgosDeploymentInsert]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Regan Sarwas
-- Create date: March 21, 2013
-- Modified:    June 29, 2018 - Bug Fixes
-- Description: Validates the business rules for an Updated ArgosDeployment
-- =============================================
CREATE TRIGGER [dbo].[AfterArgosDeploymentUpdate] 
   ON  [dbo].[ArgosDeployments] 
   AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Validate Business Rules
    -- 0. Deny updating the ParameterId
    IF UPDATE(DeploymentId)
    BEGIN
        RAISERROR('The primary key (DeploymnentId) cannot be changed.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END
    
    -- 1. Ensure StartDate is before the EndDate
    IF EXISTS (SELECT 1
                 FROM inserted AS I
                WHERE I.EndDate <= I.StartDate  -- false if either is NULL
              )
    BEGIN
        RAISERROR('The end of the deployment must occur after the start of the deployment.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END

    -- 2a. Ensure start date is before the DisposalDate of the Collar
    IF EXISTS (SELECT 1
                 FROM inserted AS I
           INNER JOIN Collars AS C
                   ON I.CollarManufacturer = C.CollarManufacturer AND I.CollarId = C.CollarId
                WHERE C.DisposalDate < I.StartDate
              )
    BEGIN
        RAISERROR('The deployment must start before the collar is disposed.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END

    -- 2b. Ensure start Date is before the DisposalDate of the ArgosPlatform
    IF EXISTS (SELECT 1
                 FROM inserted AS I
           INNER JOIN ArgosPlatforms AS P
                   ON P.PlatformId = I.PlatformId
                WHERE P.DisposalDate < I.StartDate
              )
    BEGIN
        RAISERROR('The deployment must start before the Argos platfrom is disposed.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END

    -- 3. Ensure an ArgosPlatform is not deployed on multiple Collars at the same time
    -- We are checking each inserted deployment against all existing deployments, and all other new deployments	 
    IF EXISTS (SELECT 1
                 FROM inserted AS I1
            LEFT JOIN inserted AS I2
                   ON I1.PlatformId = I2.PlatformId AND I1.DeploymentId <> I2.DeploymentId
           INNER JOIN dbo.ArgosDeployments AS D
                   ON D.PlatformId = I1.PlatformId AND D.DeploymentId <> I1.DeploymentId
                WHERE dbo.DoDateRangesOverlap(D.StartDate, D.EndDate, I1.StartDate, I1.EndDate) = 1
                   OR (I2.DeploymentId IS NOT NULL AND
                       dbo.DoDateRangesOverlap(I1.StartDate, I1.EndDate, I2.StartDate, I2.EndDate) = 1)
              )
    BEGIN
        RAISERROR('Argos platforms cannot have overlapping deployment dates.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END
    
    -- 4. Ensure a Collar is not carrying multiple ArgosPlatforms at the same time
    -- We are checking each inserted deployment against all existing deployments, and all other new deployments 
    IF EXISTS (SELECT 1
                 FROM inserted AS I1
            LEFT JOIN inserted AS I2
                   ON I1.CollarManufacturer = I2.CollarManufacturer AND I1.CollarId = I2.CollarId AND I1.DeploymentId <> I2.DeploymentId
           INNER JOIN dbo.ArgosDeployments AS D
                   ON D.CollarManufacturer = I1.CollarManufacturer AND D.CollarId = I1.CollarId AND D.DeploymentId <> I1.DeploymentId
                WHERE dbo.DoDateRangesOverlap(D.StartDate, D.EndDate, I1.StartDate, I1.EndDate) = 1
                   OR (I2.DeploymentId IS NOT NULL AND
                       dbo.DoDateRangesOverlap(I1.StartDate, I1.EndDate, I2.StartDate, I2.EndDate) = 1)
              )
    BEGIN
        RAISERROR('Collars cannot have overlapping deployment dates.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END

    /*
    An update can be thought of as a delete and an insert, and the logic in the
    delete and insert triggers is basically replicated here when more than the dates in
    the deployment changes.
    However this is very ineffcient in the typical situation:
      A deployment is created with StartDate = XXX and EndDate = NULL. The user wants
      to create a a new deployment with a StartDate = YYY and EndDate = NULL. where XXX < YYY.
      before the insert can succeed the EndDate of the first deployment must be set to YYY.
    Using a brute force method, all existing processing based on this deployment would
    be deleted then recreated.
    Instead, I will check for these cases:
    If only the dates have changed (no collar or platform change):
      Start or End date has shrunk:
        Flag any files with transmission outside the new date range as
        a processing issue (The same as would happen if the deployment had the non-null dates
        to start with.)  No additional processing needs to happen until a new deployment is created.
      Start or End date has grown:
        Find files with processing issues for this platform, and with transmissions in the new part
        of the date range, then reprocess them, because the expanded dates will add transmissions. 
    Else if only the Collar has changed (no date or platform change)
      I though I might find the result files for the old collar, deactive them, change the collar,
      then reactivate them, but there may be results files for other deployments for the same old
      collar which should not be changed. Since we cannot trivially determine the deployment (or
      even the platform) for a results file, we cannot select just the collar files to change.
      It might be possible to do something more clever, but my head already hurts, so I will
      simply handle it safely as follows.
    Else if the platform changed, or there were multiple items changing,
      use the brute force delete/insert method. 
    */

    -- May be part of a batch update (happens with a cascading update or when the SA bypasses the SP)
    -- Triggers always execute in the context of a transaction, so the following code is all or nothing.
    -- to keep the logic manageble, I will create a cursor to handle each deployment separately.

    IF EXISTS (SELECT 1
                 FROM inserted AS I
                 JOIN deleted AS D
                   ON D.DeploymentId = I.DeploymentId
                WHERE I.CollarId <> D.CollarId
                   OR I.CollarManufacturer <> D.CollarManufacturer
                   OR I.PlatformId <> D.PlatformId
               )
    BEGIN
        -- Brute Force Solution:
        -- 1. Delete => Delete files derived from the old deployment
            DELETE F 
              FROM inserted AS I
        INNER JOIN CollarFiles AS F
                ON F.ArgosDeploymentId = I.DeploymentId
        -- 2. Insert => Delete Processing Issues for the new platform
            DELETE A 
              FROM inserted AS I
        INNER JOIN ArgosFileProcessingIssues AS A
                ON A.PlatformId = I.PlatformId
    END
    ELSE BEGIN
        -- Only the dates have changed
        -- Date range shrunk
			-- Remove Results file, and add processing issue.  Must add issue first - If I delete first, I can't find the issue.
		    -- Create new processing issues to explain any missing results files
            -- Start Shrunk to after first transmission
            INSERT INTO [dbo].[ArgosFileProcessingIssues]
                        ([FileId], [Issue], [PlatformId], [FirstTransmission], [LastTransmission])
                 SELECT P.FileId, 'No deployment for Argos Id from ' + 
                        CONVERT(varchar(20), S.FirstTransmission, 20) + ' to ' + CONVERT(varchar(20), I.StartDate, 20),
                        I.PlatformId, FirstTransmission, LastTransmission
                   FROM CollarFiles AS C
             INNER JOIN CollarFiles AS P
                     ON P.FileId = C.ParentFileId
             INNER JOIN inserted AS I
                     ON I.DeploymentId = C.ArgosDeploymentId
             INNER JOIN deleted AS D
                     ON D.DeploymentId = I.DeploymentId
             INNER JOIN (select FileId, min(FirstTransmission) as FirstTransmission, max(LastTransmission) as LastTransmission from ArgosFilePlatformDates group by FileId) AS S
                     ON S.FileId = P.FileId
                  WHERE ((D.StartDate IS NULL AND I.StartDate IS NOT NULL) OR (D.StartDate < I.StartDate)) -- Deployment start date shrank
                    AND S.FirstTransmission < I.StartDate                                                  -- There is a transmission after the new start date
            -- End Shrunk before last transmission
            INSERT INTO [dbo].[ArgosFileProcessingIssues]
                        ([FileId], [Issue], [PlatformId], [FirstTransmission], [LastTransmission])
                 SELECT P.FileId, 'No deployment for Argos Id from ' + 
                        CONVERT(varchar(20), I.EndDate, 20) + ' to ' + CONVERT(varchar(20), S.LastTransmission, 20),
                        I.PlatformId, FirstTransmission, LastTransmission
                   FROM CollarFiles AS C
             INNER JOIN CollarFiles AS P
                     ON P.FileId = C.ParentFileId
             INNER JOIN inserted AS I
                     ON I.DeploymentId = C.ArgosDeploymentId
             INNER JOIN deleted AS D
                     ON D.DeploymentId = I.DeploymentId
             INNER JOIN (select FileId, min(FirstTransmission) as FirstTransmission, max(LastTransmission) as LastTransmission from ArgosFilePlatformDates group by FileId) AS S
                     ON S.FileId = P.FileId
                  WHERE ((D.EndDate IS NULL AND I.EndDate IS NOT NULL) OR (I.EndDate < D.EndDate)) -- Deployment end date shrank
                    AND I.EndDate < S.LastTransmission                                             -- There is a transmission before the new end date
			-- Remove (unprocess) any result files that are no longer covered by deployment
			-- Can't call the SPROC ArgosFile_UnProcessPlatform(@fileID, @platformId) in a select statement. I could use a cursor, 
			-- but it is simpler (and faster) to rewrite the delete statement from SPROC to delete the correct fileIDs
                 DELETE CollarFiles WHERE FileId in (
				 SELECT DISTINCT C.FileId
                   FROM CollarFiles AS C
             INNER JOIN inserted AS I
                     ON I.DeploymentId = C.ArgosDeploymentId
             INNER JOIN deleted AS D
                     ON D.DeploymentId = I.DeploymentId
             INNER JOIN ArgosFilePlatformDates AS S
                     ON S.FileId = C.ParentFileId
                  WHERE
					    -- Start Shrunk to after first transmission
					    (((D.StartDate IS NULL AND I.StartDate IS NOT NULL) OR (D.StartDate < I.StartDate)) AND S.FirstTransmission < I.StartDate)
					    OR
					    -- End Shrunk before last transmission
					    (((D.EndDate IS NULL AND I.EndDate IS NOT NULL) OR (I.EndDate < D.EndDate)) AND I.EndDate < S.LastTransmission)
				  )

        -- Date range increased - Remove overlapping processing issue to trigger a re-process
                 DELETE P
                   FROM ArgosFileProcessingIssues AS P
             INNER JOIN inserted AS I
                     ON I.PlatformId = P.PlatformId
             INNER JOIN deleted AS D
                     ON D.DeploymentId = I.DeploymentId
                  WHERE (    ((D.StartDate IS NOT NULL AND I.StartDate IS NULL) OR D.StartDate > I.StartDate)                -- Deployment start date grew 
						 AND dbo.DoDateRangesOverlap(P.FirstTransmission, P.LastTransmission, I.StartDate, D.StartDate) = 1) -- issue dates overlap new range at start
					 OR
				        (    ((D.EndDate IS NOT NULL AND I.EndDate IS NULL) OR I.EndDate > D.EndDate)                        -- Deployment end date grew
                         AND dbo.DoDateRangesOverlap(P.FirstTransmission, P.LastTransmission, D.EndDate, I.EndDate) = 1)     -- issue dates overlap new range at end
    END


    -- We need to deal with the E&F files with non-GPS collars.  We do not anticipate a lot of non-gps collars, so I
    -- will handle this a brute force delete and insert.
    -- This should match the delete/insert triggers (see it for comments)
    -- Delete:
    DELETE F
      FROM CollarFixes AS F
      JOIN Collars AS C
        ON C.CollarManufacturer = F.CollarManufacturer AND C.CollarId = F.CollarId
      JOIN deleted as D
        ON D.CollarManufacturer = C.CollarManufacturer AND D.CollarId = C.CollarId
     WHERE C.HasGps = 0
       AND (F.FixDate <= D.EndDate OR D.EndDate IS NULL)
       AND (D.StartDate <= F.FixDate  OR D.StartDate IS NULL)

    -- Insert:
    DECLARE @FileId INT;
    DECLARE @DeploymentId INT;
    DECLARE insert_deployment_cursor2 CURSOR FOR 
             SELECT A.FileId, I.DeploymentId
               FROM inserted AS I
         INNER JOIN Collars AS C
                 ON C.CollarManufacturer = I.CollarManufacturer AND C.CollarId = I.CollarId
         INNER JOIN ArgosFilePlatformDates AS A
                 ON A.PlatformId = I.PlatformId
         INNER JOIN CollarFiles AS F
                 ON F.FileId = A.FileId
              WHERE C.HasGps = 0
                AND (F.Format = 'E' OR F.Format = 'F')
                AND dbo.DoDateRangesOverlap(A.FirstTransmission, A.LastTransmission, I.StartDate, I.EndDate) = 1

    OPEN insert_deployment_cursor2;

    FETCH NEXT FROM insert_deployment_cursor2 INTO @FileId, @DeploymentId;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        EXEC dbo.CollarFixes_Insert @FileId, @DeploymentId
        FETCH NEXT FROM insert_deployment_cursor2 INTO @FileId, @DeploymentId;
    END
    CLOSE insert_deployment_cursor2;
    DEALLOCATE insert_deployment_cursor2;

END

GO
ALTER TABLE [dbo].[ArgosDeployments] ENABLE TRIGGER [AfterArgosDeploymentUpdate]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:      Regan Sarwas
-- Create date: Mar 20, 2013
-- Description: Ensure Data integerity on Insert
-- =============================================
CREATE TRIGGER [dbo].[AfterArgosDownloadsInsert] 
			ON [dbo].[ArgosDownloads] 
			AFTER INSERT
AS 
BEGIN
	SET NOCOUNT ON;
	
	-- Business Rule: Argos Downloads must have one and only one non-null value for (FileId,ErrorMessage)
	IF EXISTS (    SELECT 1
	                 FROM inserted 
	                WHERE (FileId IS NULL AND ErrorMessage IS NULL)
	                   OR (FileId IS NOT NULL AND ErrorMessage IS NOT NULL)
              )
	BEGIN
		RAISERROR('Integrity Violation. Downloads must only one non-null value in FileId or ErrorMessage', 18, 0)
		ROLLBACK TRANSACTION;
		RETURN
	END

	-- Business Rule: Argos Downloads must have one and only one non-null value for (PlatformId, ProgramId)
	IF EXISTS (    SELECT 1
	                 FROM inserted 
	                WHERE (PlatformId IS NULL AND ProgramId IS NULL)
	                   OR (PlatformId IS NOT NULL AND ProgramId IS NOT NULL)
              )
	BEGIN
		RAISERROR('Integrity Violation. Downloads must only one non-null value in PlatformId or ProgramId', 18, 0)
		ROLLBACK TRANSACTION;
		RETURN
	END
END



GO
ALTER TABLE [dbo].[ArgosDownloads] ENABLE TRIGGER [AfterArgosDownloadsInsert]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Regan Sarwas
-- Create date: Mar 20, 2013
-- Description: Ensure Data integerity on Update
-- =============================================
CREATE TRIGGER [dbo].[AfterArgosPlatformUpdate] 
			ON [dbo].[ArgosPlatforms] 
			AFTER UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	
	-- We cannot the PK (PlatformId) and the DisposalDate at the same time
	IF (UPDATE(PlatformId) AND UPDATE(DisposalDate))
	BEGIN
		RAISERROR('Integrity Violation - You cannot update the PlatformId and the DisposalDate in one operation', 18, 0)
		ROLLBACK TRANSACTION;
		RETURN
	END

	-- Changes to Disposal Date
	IF (UPDATE (DisposalDate))
	BEGIN
		-- Business Rule: End the open (null EndDate) related Argos Doeployment
			UPDATE AD SET EndDate = i.DisposalDate
			  FROM dbo.ArgosDeployments AS AD
		INNER JOIN inserted AS i
				ON AD.PlatformId = i.PlatformId
		INNER JOIN deleted AS d
				ON d.PlatformId = i.PlatformId
			 WHERE (d.DisposalDate <> i.DisposalDate OR d.DisposalDate IS NULL)
			   AND i.DisposalDate IS NOT NULL
			   AND AD.EndDate IS NULL
		
		-- Business Rule: Disposal Date must be on or after the EndDate of all related Argos Doeployment
		IF EXISTS (     SELECT 1
						  FROM dbo.ArgosDeployments AS AD
					INNER JOIN inserted AS i
							ON AD.PlatformId = i.PlatformId
					INNER JOIN deleted AS d
							ON d.PlatformId = i.PlatformId
						 WHERE (d.DisposalDate <> i.DisposalDate OR d.DisposalDate IS NULL)
						   AND i.DisposalDate IS NOT NULL
						   AND AD.EndDate > i.DisposalDate
				  )
		BEGIN
			RAISERROR('Integrity Violation - DisposalDate must be on or after the EndDate of all related Argos Deployments', 18, 0)
			ROLLBACK TRANSACTION;
			RETURN
		END

		-- Business Rule: Set ArgosPlatform.Active to false if the new DisposalDate is in the past
			UPDATE P SET Active = 0
			  FROM dbo.ArgosPlatforms AS P
		INNER JOIN inserted AS i
				ON P.PlatformId = i.PlatformId
		INNER JOIN deleted AS d
				ON d.PlatformId = i.PlatformId
			 WHERE (d.DisposalDate <> i.DisposalDate OR d.DisposalDate IS NULL)
			   AND i.DisposalDate < GETDATE()


	END
	
	-- We only care about changes to DisposalDate.

END




GO
ALTER TABLE [dbo].[ArgosPlatforms] ENABLE TRIGGER [AfterArgosPlatformUpdate]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Regan Sarwas
-- Create date: April 3, 2012
-- Description:	Remove Locations when Deployment is deleted
-- =============================================
CREATE TRIGGER [dbo].[AfterCollarDeploymentDelete] 
   ON  [dbo].[CollarDeployments] 
   AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;
	-- Delete records from locations that are no longer deployed
	DELETE L FROM dbo.Locations as L
	   INNER JOIN deleted as D
			   ON L.ProjectId = D.ProjectId
			  AND L.AnimalId = D.AnimalId
			  AND L.FixDate >= D.DeploymentDate
			  AND (D.RetrievalDate IS NULL OR L.FixDate <= D.RetrievalDate)
END

GO
ALTER TABLE [dbo].[CollarDeployments] ENABLE TRIGGER [AfterCollarDeploymentDelete]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Regan Sarwas
-- Create date: April 3, 2012
-- Description:	Adds Locations when Deployment is inserted
-- =============================================
CREATE TRIGGER [dbo].[AfterCollarDeploymentInsert] 
   ON  [dbo].[CollarDeployments] 
   AFTER INSERT
AS 
BEGIN
	SET NOCOUNT ON;
	
	-- Check for violations with date range
	-- Verify the retrieval occurs after the deployment
	IF EXISTS (SELECT 1
	             FROM inserted AS I
				WHERE I.RetrievalDate <= I.DeploymentDate
			  )
	BEGIN
		RAISERROR('The retrevial must occur after the deployment.', 18, 0)
		ROLLBACK TRANSACTION;
		RETURN
	END

	-- Verify deployment start occurs before collar disposal
	IF EXISTS (SELECT 1
	             FROM inserted AS I
	       INNER JOIN Collars AS C
				   ON I.CollarManufacturer = C.CollarManufacturer AND I.CollarId = C.CollarId
				WHERE C.DisposalDate < I.DeploymentDate
			  )
	BEGIN
		RAISERROR('Deployment start date violation.  The collar was disposed before the deployment begins.', 18, 0)
		ROLLBACK TRANSACTION;
		RETURN
	END

	-- Verify that the animal is not wearing another collar during the proposed date range
	-- We are checking each inserted deployment against all existing deployments, and all other new deployments	 
	IF EXISTS (SELECT 1
	             FROM inserted AS I1
		    LEFT JOIN inserted AS I2
			       ON I1.ProjectId = I2.ProjectId AND I1.AnimalId = I2.AnimalId AND I1.DeploymentDate <> I2.DeploymentDate
		   INNER JOIN dbo.CollarDeployments AS D
			       ON D.ProjectId = I1.ProjectId AND D.AnimalId = I1.AnimalId AND D.DeploymentDate <> I1.DeploymentDate
		   		WHERE dbo.DoDateRangesOverlap(D.DeploymentDate, D.RetrievalDate, I1.DeploymentDate, I1.RetrievalDate) = 1
				   OR (I2.DeploymentDate IS NOT NULL AND
				       dbo.DoDateRangesOverlap(I1.DeploymentDate, I1.RetrievalDate, I2.DeploymentDate, I2.RetrievalDate) = 1)
			  )
	BEGIN
		RAISERROR('Insert would result in an animal with overlapping deployment dates violation.', 18, 0)
		ROLLBACK TRANSACTION;
		RETURN
	END
	
	-- Verify that the collar is not on another animal during the proposed date range
	-- We are checking each inserted deployment against all existing deployments, and all other new deployments 
	IF EXISTS (SELECT 1
	             FROM inserted AS I1
		    LEFT JOIN inserted AS I2
			       ON I1.CollarManufacturer = I2.CollarManufacturer AND I1.CollarId = I2.CollarId AND I1.DeploymentDate <> I2.DeploymentDate
		   INNER JOIN dbo.CollarDeployments AS D
			       ON D.CollarManufacturer = I1.CollarManufacturer AND D.CollarId = I1.CollarId AND D.DeploymentDate <> I1.DeploymentDate
		   		WHERE dbo.DoDateRangesOverlap(D.DeploymentDate, D.RetrievalDate, I1.DeploymentDate, I1.RetrievalDate) = 1
				   OR (I2.DeploymentDate IS NOT NULL AND
				       dbo.DoDateRangesOverlap(I1.DeploymentDate, I1.RetrievalDate, I2.DeploymentDate, I2.RetrievalDate) = 1)
			  )
	BEGIN
		RAISERROR('Insert would result in a collar with overlapping deployment dates violation.', 18, 0)
		ROLLBACK TRANSACTION;
		RETURN
	END
		
	
	-- Add locations for fixes that are now deployed
	INSERT INTO Locations (ProjectId, AnimalId, FixDate, Location, FixId)
		 SELECT I.ProjectId, I.AnimalId, F.FixDate, geography::Point(F.Lat, F.Lon, 4326), F.FixId
		   FROM dbo.CollarFixes AS F
	 INNER JOIN inserted AS I
		     ON F.CollarManufacturer = I.CollarManufacturer
		    AND F.CollarId = I.CollarId
	 INNER JOIN dbo.Animals AS A
			 ON A.ProjectId = I.ProjectId
			AND A.AnimalId = I.AnimalId
     INNER JOIN dbo.Collars AS C
             ON C.CollarManufacturer = I.CollarManufacturer
            AND C.CollarId = I.CollarId
		  WHERE F.HiddenBy IS NULL
			AND I.DeploymentDate <= F.FixDate
			AND (I.RetrievalDate IS NULL OR F.FixDate <= I.RetrievalDate)
		    AND (A.MortalityDate IS NULL OR F.FixDate <= A.MortalityDate) 
			AND (C.DisposalDate IS NULL OR F.FixDate <= C.DisposalDate)
END

GO
ALTER TABLE [dbo].[CollarDeployments] ENABLE TRIGGER [AfterCollarDeploymentInsert]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Regan Sarwas
-- Create date: April 3, 2012
-- Description: Add/Remove Locations when Deployment is updated
-- =============================================
CREATE TRIGGER [dbo].[AfterCollarDeploymentUpdate] 
   ON  [dbo].[CollarDeployments] 
   AFTER UPDATE
AS 
BEGIN
    SET NOCOUNT ON;

    -- triggers always execute in the context of a transaction
    -- so the following code is all or nothing.

    -- dates are the only thing that can change (at this point)

    -- I compare inserted with deleted because the Update SPROC includes all columns
    IF (UPDATE(ProjectId) OR UPDATE(AnimalId))
       AND EXISTS (SELECT 1 FROM inserted AS I JOIN deleted AS D ON D.DeploymentId = I.DeploymentId
                    WHERE I.ProjectId != D.ProjectId OR I.AnimalId != D.AnimalId)
    BEGIN
        RAISERROR('Updating the Animal is currently not allowed - Delete and recreate.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END
    IF (UPDATE(CollarManufacturer) OR UPDATE(CollarId))
       AND EXISTS (SELECT 1 FROM inserted AS I JOIN deleted AS D ON D.DeploymentId = I.DeploymentId
                    WHERE I.CollarManufacturer != D.CollarManufacturer OR I.CollarId != D.CollarId)
    BEGIN
        RAISERROR('Updating the Collar is currently not allowed - Delete and recreate.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END


    -- Verify the retrieval occurs after the deployment
    IF EXISTS (SELECT 1
                 FROM inserted AS I
                WHERE I.RetrievalDate <= I.DeploymentDate
              )
    BEGIN
        RAISERROR('The retrevial must occur after the deployment.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END

    -- Verify new deployment start occurs before collar disposal
    IF EXISTS (SELECT 1
                 FROM inserted AS I
           INNER JOIN Collars AS C
                   ON I.CollarManufacturer = C.CollarManufacturer AND I.CollarId = C.CollarId
                WHERE C.DisposalDate < I.DeploymentDate
              )
    BEGIN
        RAISERROR('Deployment start date violation.  The collar was disposed before the deployment begins.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END

    -- Verify that the animal is not wearing another collar during the proposed date range
    -- We are checking each inserted deployment against all existing deployments, and all other new deployments	 
    IF EXISTS (SELECT 1
                 FROM inserted AS I1
            LEFT JOIN inserted AS I2
                   ON I1.ProjectId = I2.ProjectId AND I1.AnimalId = I2.AnimalId AND I1.DeploymentId <> I2.DeploymentId
           INNER JOIN dbo.CollarDeployments AS D
                   ON D.ProjectId = I1.ProjectId AND D.AnimalId = I1.AnimalId AND D.DeploymentId <> I1.DeploymentId
                WHERE dbo.DoDateRangesOverlap(D.DeploymentDate, D.RetrievalDate, I1.DeploymentDate, I1.RetrievalDate) = 1
                   OR (I2.DeploymentDate IS NOT NULL AND
                       dbo.DoDateRangesOverlap(I1.DeploymentDate, I1.RetrievalDate, I2.DeploymentDate, I2.RetrievalDate) = 1)
              )
    BEGIN
        RAISERROR('Insert would result in an animal with overlapping deployment dates violation.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END

    -- Verify that the collar is not on another animal during the proposed date range
    -- We are checking each inserted deployment against all existing deployments, and all other new deployments 
    IF EXISTS (SELECT 1
                 FROM inserted AS I1
            LEFT JOIN inserted AS I2
                   ON I1.CollarManufacturer = I2.CollarManufacturer AND I1.CollarId = I2.CollarId AND I1.DeploymentId <> I2.DeploymentId
           INNER JOIN dbo.CollarDeployments AS D
                   ON D.CollarManufacturer = I1.CollarManufacturer AND D.CollarId = I1.CollarId AND D.DeploymentId <> I1.DeploymentId
                WHERE dbo.DoDateRangesOverlap(D.DeploymentDate, D.RetrievalDate, I1.DeploymentDate, I1.RetrievalDate) = 1
                   OR (I2.DeploymentDate IS NOT NULL AND
                       dbo.DoDateRangesOverlap(I1.DeploymentDate, I1.RetrievalDate, I2.DeploymentDate, I2.RetrievalDate) = 1)
              )
    BEGIN
        RAISERROR('Insert would result in a collar with overlapping deployment dates violation.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END


        -- When new retrevial date < old retrieval date we may lose some Locations
        -- Delete records from locations fix date is greater than the new retrieval date
/*
Example:  Time --->

  *-----D1-----* *------D2--------------->
      *              *               *
      L1             L2              L3
  *-----D1-----* *------D2-----*

L1 is part of D1 but not D2,
L2 and L3 are part of original D2, but not D1
L3 is not part of either deploymnet after changing retrieval date on D2
*/
        DELETE L FROM dbo.Locations as L
                   -- Join to the deployment that created this location
           INNER JOIN deleted as D
                   ON L.ProjectId = D.ProjectId
                  AND L.AnimalId = D.AnimalId
                  AND D.DeploymentDate < L.FixDate
                  AND (L.FixDate < D.RetrievalDate OR D.RetrievalDate IS NULL)
                   -- Match the old (deleted) deployment dates to the new (inserted) deployment dates
           INNER JOIN inserted as I
                   ON I.DeploymentId = D.DeploymentId
                   -- Delete the Locations outside the new (inserted) deployment dates:
                WHERE L.FixDate < I.DeploymentDate OR I.RetrievalDate < L.FixDate 

        -- When new retrevial is null, or is greater than the old retrevial date we may gain some Locations
        -- Add locations for fixes that are now deployed, but were not before
/*
Example:  Time --->

  *-----D1-----* *------D2-----*
      *              *               *
      F1             F2              F3
  *-----D1-----* *------D2-----------------*  or --->

Add Location for F3, but not F1 or F2
*/
        INSERT INTO Locations (ProjectId, AnimalId, FixDate, Location, FixId)
             SELECT I.ProjectId, I.AnimalId, F.FixDate, geography::Point(F.Lat, F.Lon, 4326), F.FixId
               FROM dbo.CollarFixes AS F
                 -- Join to the fixes that are covered by the new deployment dates
         INNER JOIN inserted AS I
                 ON F.CollarManufacturer = I.CollarManufacturer
                AND F.CollarId = I.CollarId
         INNER JOIN deleted as D
                 -- To get the old retrieval date, we need to link inserted and deleted deployments by PK
                 ON I.DeploymentId = D.DeploymentId
         INNER JOIN dbo.Animals AS A
                 ON A.ProjectId = I.ProjectId
                AND A.AnimalId = I.AnimalId
         INNER JOIN dbo.Collars AS C
                 ON C.CollarManufacturer = I.CollarManufacturer
                AND C.CollarId = I.CollarId
              WHERE F.HiddenBy IS NULL
                 -- fix wasn't in old deployment dates, note: NULL < F.FixDate is always false (good) 
                AND (F.FixDate < D.DeploymentDate OR D.RetrievalDate < F.FixDate)
                 -- fix is in new deployment dates
                AND I.DeploymentDate < F.FixDate
                AND (I.RetrievalDate IS NULL OR F.FixDate <= I.RetrievalDate)
                AND (A.MortalityDate IS NULL OR F.FixDate <= A.MortalityDate)
                AND (C.DisposalDate IS NULL OR F.FixDate <= C.DisposalDate)

END
GO
ALTER TABLE [dbo].[CollarDeployments] ENABLE TRIGGER [AfterCollarDeploymentUpdate]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:      Regan Sarwas
-- Create date: Mar 25, 2013
-- Description: Ensure Data integrity on Insert
-- =============================================
CREATE TRIGGER [dbo].[AfterCollarFileInsert] 
    ON [dbo].[CollarFiles] 
    AFTER INSERT
AS 
BEGIN
    SET NOCOUNT ON;

    -- Business Rule: CollarFiles must have one and only one non-null value for (Owner,ProjectId)
    IF EXISTS (    SELECT 1
                     FROM inserted 
                    WHERE (ProjectId IS NULL AND [Owner] IS NULL)
                       OR (ProjectId IS NOT NULL AND [Owner] IS NOT NULL)
              )
    BEGIN
        RAISERROR('Integrity Violation. CollarFiles must have only one non-null value between Owner and ProjectId', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END


    -- Business Rule: A Collar Mfgr/Id is required when the file format says so.
    --                (referential integrity already guarantees a non-null collar is valid)
    IF EXISTS (    SELECT 1
                     FROM inserted AS i
                     JOIN LookupCollarFileFormats AS F
                       ON i.Format = F.Code
                    WHERE F.RequiresCollar = 'Y' AND i.CollarId IS NULL
              )
    BEGIN
        RAISERROR('Integrity Violation. A Collar Mfgr/Id is required unless the file format has Argos data as the parent', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END


    -- Business Rule: A CollarFile with a parent, must have the same owner, project, and status
    --                (since a child cannot be inserted with a parent -- the parentid does not exist
    --                 I do not need to join the inserted with the inserted to find the parent)
    IF EXISTS (    SELECT 1
                     FROM inserted AS C
                     JOIN CollarFiles AS P
                       ON C.ParentFileId = P.FileId
                    WHERE C.[Owner] <> P.[Owner]
                       OR (C.[Owner] IS NULL AND P.[Owner] IS NOT NULL)
                       OR (C.[Owner] IS NOT NULL AND P.[Owner] IS NULL)
                       OR C.[ProjectId] <> P.[ProjectId]
                       OR (C.[ProjectId] IS NULL AND P.[ProjectId] IS NOT NULL)
                       OR (C.[ProjectId] IS NOT NULL AND P.[ProjectId] IS NULL)
                       OR C.[Status] <> P.[Status]
                       OR (C.[Status] IS NULL AND P.[Status] IS NOT NULL)
                       OR (C.[Status] IS NOT NULL AND P.[Status] IS NULL)
              )
    BEGIN
        RAISERROR('Integrity Violation. A CollarFile with a parent, must have the same owner, project, and status as the parent', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END


    -- Business Rule: ArgosDeploymentId and CollarParameterId must be non-null if and only if the ParentFile has argos data
    IF EXISTS (    SELECT 1
                     FROM inserted AS C
               INNER JOIN CollarFiles AS P
                       ON C.ParentFileId = P.FileId 
               INNER JOIN LookupCollarFileFormats AS F
                       ON P.Format = F.Code
                    WHERE ((C.ArgosDeploymentId IS NOT NULL OR C.CollarParameterId IS NOT NULL) AND F.ArgosData = 'N')
                       OR ((C.ArgosDeploymentId IS NULL OR C.CollarParameterId IS NULL) AND F.ArgosData = 'Y')
              )
    BEGIN
        RAISERROR('Integrity Violation. CollarFiles must have only one non-null value between Owner and ProjectId', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END


    -- Business Rule: All new CollarFiles with Argos data need to be summerized
    -- Business Rule: All new files need to have their data parsed in to a collardata table
    DECLARE @FileId int
    DECLARE @ArgosData char
    DECLARE collarfiles_afterinsert_cursor CURSOR FOR 
               SELECT i.FileId, F.ArgosData
                 FROM inserted AS i
                 JOIN LookupCollarFileFormats AS F
                   ON i.Format = F.Code

    OPEN collarfiles_afterinsert_cursor;
    FETCH NEXT FROM collarfiles_afterinsert_cursor INTO @FileId, @ArgosData
    WHILE @@FETCH_STATUS = 0
    BEGIN
        EXEC [dbo].[CollarData_Insert] @FileId 
        IF (@ArgosData = 'Y')
            EXEC [dbo].[Summerize] @FileId
        FETCH NEXT FROM collarfiles_afterinsert_cursor INTO @FileId, @ArgosData
    END
    CLOSE collarfiles_afterinsert_cursor;
    DEALLOCATE collarfiles_afterinsert_cursor;


    -- Business Rule: An active collar file has fixes, and an inactive file does not.
    DECLARE collarfiles_afterinsert_cursor CURSOR FOR 
               SELECT i.FileId
                 FROM inserted AS i
                WHERE i.[Status] = 'A'

    OPEN collarfiles_afterinsert_cursor;
    FETCH NEXT FROM collarfiles_afterinsert_cursor INTO @FileId
    WHILE @@FETCH_STATUS = 0
    BEGIN
        EXEC [dbo].[CollarFixes_Insert] @FileId
        FETCH NEXT FROM collarfiles_afterinsert_cursor INTO @FileId
    END
    CLOSE collarfiles_afterinsert_cursor;
    DEALLOCATE collarfiles_afterinsert_cursor;

END




GO
ALTER TABLE [dbo].[CollarFiles] ENABLE TRIGGER [AfterCollarFileInsert]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Regan Sarwas
-- Create date: April 2, 2013
-- Description:	Validate and enforce business rules
--              when updating collar files
-- =============================================
CREATE TRIGGER [dbo].[AfterCollarFileUpdate] 
    ON  [dbo].[CollarFiles] 
    AFTER UPDATE
AS 
BEGIN
    SET NOCOUNT ON;

    -- triggers always execute in the context of a transaction
    -- so the following code is all or nothing.


    -- Business Rule: FileId, Contents, Format, Username, UploadDate, ParentFileId, Sha1Hash cannot be updated.
    --                Sha1Hash is managed by the DB engine since it is a computed column
    IF    UPDATE(FileId) OR UPDATE(Contents) OR UPDATE(Format) OR UPDATE(UserName)
       OR UPDATE(UploadDate) OR UPDATE(ParentFileId)
    BEGIN
        RAISERROR('Integrity Violation. Attempted to modify an immutable column in CollarFiles', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END


    -- Business Rule: CollarFiles must have one and only one non-null value for (Owner,ProjectId)
    IF EXISTS (    SELECT 1
                     FROM inserted 
                    WHERE (ProjectId IS NULL AND [Owner] IS NULL)
                       OR (ProjectId IS NOT NULL AND [Owner] IS NOT NULL)
              )
    BEGIN
        RAISERROR('Integrity Violation. CollarFiles must have only one non-null value between Owner and ProjectId', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END


    -- Business Rule: A Collar Mfgr/Id is required when the file format says so.
    --                (referential integrity already guarantees a non-null collar is valid)
    IF EXISTS (    SELECT 1
                     FROM inserted AS i
                     JOIN LookupCollarFileFormats AS F
                       ON i.Format = F.Code
                    WHERE F.RequiresCollar = 'Y' AND i.CollarId IS NULL
              )
    BEGIN
        RAISERROR('Integrity Violation. A Collar Mfgr/Id is required unless the file format has Argos data as the parent', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END


    -- Business Rule: A CollarFile that was active cannot change it's Collar Mfgr/Id
    IF EXISTS (SELECT 1 FROM inserted AS i
                        JOIN deleted AS d
                          ON i.FileId = d.FileId
                       WHERE d.[Status] = 'A'
                         AND (	  i.[CollarManufacturer] <> d.[CollarManufacturer]
                              OR (i.[CollarManufacturer] IS NULL AND d.[CollarManufacturer] IS NOT NULL)
                              OR (i.[CollarManufacturer] IS NOT NULL AND d.[CollarManufacturer] IS NULL)
                              OR  i.[CollarId] <> d.[CollarId]
                              OR (i.[CollarId] IS NULL AND d.[CollarId] IS NOT NULL)
                              OR (i.[CollarId] IS NOT NULL AND d.[CollarId] IS NULL)))
    BEGIN
        RAISERROR('Integrity Violation. A CollarFile that was active cannot change it''s Collar', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END


    -- Business Rule: A CollarFile with a parent, must have the same owner, project
    --                If the status of the parent is A, then the status of a child can be A or I
    --                If the status of the parent is I, then the status of a child must be I
    --                If a parent changes status, then that change propugates to the child 
    --                regardless of the childs existing value.
    -- step 1, make sure I am in sync with my parents
        -- case 1: parent is also being updated
        IF EXISTS (SELECT 1 FROM inserted AS C
                            JOIN inserted AS P
                              ON C.ParentFileId = P.FileId
                           WHERE C.[Owner] <> P.[Owner]
                              OR (C.[Owner] IS NULL AND P.[Owner] IS NOT NULL)
                              OR (C.[Owner] IS NOT NULL AND P.[Owner] IS NULL)
                              OR C.[ProjectId] <> P.[ProjectId]
                              OR (C.[ProjectId] IS NULL AND P.[ProjectId] IS NOT NULL)
                              OR (C.[ProjectId] IS NOT NULL AND P.[ProjectId] IS NULL)
                              OR (C.[Status] = 'A' AND P.[Status] = 'I') -- all other combos are allowed
                  )
        -- case 2: parent is not being updated
        OR EXISTS (SELECT 1 FROM inserted AS C
                       LEFT JOIN inserted AS i
                              ON C.ParentFileId = i.FileId AND i.FileId IS NULL
                            JOIN CollarFiles AS P
                              ON C.ParentFileId = P.FileId
                           WHERE C.[Owner] <> P.[Owner]
                              OR (C.[Owner] IS NULL AND P.[Owner] IS NOT NULL)
                              OR (C.[Owner] IS NOT NULL AND P.[Owner] IS NULL)
                              OR C.[ProjectId] <> P.[ProjectId]
                              OR (C.[ProjectId] IS NULL AND P.[ProjectId] IS NOT NULL)
                              OR (C.[ProjectId] IS NOT NULL AND P.[ProjectId] IS NULL)
                              OR (C.[Status] = 'A' AND P.[Status] = 'I') -- all other combos are allowed
                  )
        BEGIN
            RAISERROR('Integrity Violation. A CollarFile with a parent, must have the same owner, project and status as the parent', 18, 0)
            ROLLBACK TRANSACTION;
            RETURN
        END

    -- Step 2, propagate changes to my children 
        UPDATE C SET C.[Status] = P.[Status]
          FROM CollarFiles AS C --child
    INNER JOIN inserted as P    --parent
            ON C.ParentFileId = P.FileId
    INNER JOIN deleted as d
            ON P.FileId = d.FileId
         WHERE P.[Status] <> d.[Status]  -- non-nullable

        UPDATE C SET [ProjectId] = P.[ProjectId]
          FROM CollarFiles AS C --child
    INNER JOIN inserted as P    --parent
            ON C.ParentFileId = P.FileId
    INNER JOIN deleted as d
            ON P.FileId = d.FileId
         WHERE P.[ProjectId] <> d.[ProjectId]
            OR (P.[ProjectId] IS NULL AND d.[ProjectId] IS NOT NULL)
            OR (P.[ProjectId] IS NOT NULL AND d.[ProjectId] IS NULL)

        UPDATE C SET [Owner] = P.[Owner]
          FROM CollarFiles AS C --child
    INNER JOIN inserted as P    --parent
            ON C.ParentFileId = P.FileId
    INNER JOIN deleted as d
            ON P.FileId = d.FileId
         WHERE P.[Owner] <> d.[Owner]
            OR (P.[Owner] IS NULL AND d.[Owner] IS NOT NULL)
            OR (P.[Owner] IS NOT NULL AND d.[Owner] IS NULL)

    -- Business Rule: An active collar file has fixes, and an inactive file does not.
    DECLARE @FileId int
    DECLARE @OldStatus char
    DECLARE @NewStatus char
    DECLARE collarfiles_afterupdate_cursor CURSOR FOR 
               SELECT d.FileId, d.[Status], i.[Status]
                 FROM inserted AS i
           INNER JOIN deleted AS d
                   ON i.FileId = d.FileId
                WHERE i.[Status] <> d.[Status]

    OPEN collarfiles_afterupdate_cursor;
    FETCH NEXT FROM collarfiles_afterupdate_cursor INTO @FileId, @OldStatus, @NewStatus
    WHILE @@FETCH_STATUS = 0
    BEGIN
        IF (@OldStatus = 'I' AND @NewStatus = 'A')
            EXEC [dbo].[CollarFixes_Insert] @FileId 
        IF (@OldStatus = 'A' AND @NewStatus = 'I')
            EXEC [dbo].[CollarFixes_Delete] @FileId
        FETCH NEXT FROM collarfiles_afterupdate_cursor INTO @FileId, @OldStatus, @NewStatus
    END
    CLOSE collarfiles_afterupdate_cursor;
    DEALLOCATE collarfiles_afterupdate_cursor;

END

GO
ALTER TABLE [dbo].[CollarFiles] ENABLE TRIGGER [AfterCollarFileUpdate]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		Regan Sarwas
-- Create date: March 25, 2013
-- Description:	Instead of CollarFile Delete Trigger
--              This trigger must deleted related records before it can delete the files.
--              This trigger deletes related CollarFixes. This can't be done with an 
--              ON DELETE CASCADE because of the INSTEAD OF DELETE trigger on the CollarFixes
--              It also deletes related CollarFiles (i.e sub-files).  This can't be done with
--              an ON DELETE CASCADE because it 'may cause cycles or multiple cascade paths'
--              The Delete SPROC ensures that users (non-SA) cannot delete a child file.
--              Children may be deleted by the system if they are invalidated by a change in
--              the parameters used to derive the child file.
--              SA should not delete parents and their children in one batch (this will cause
--              confusion as a child may be deleted before the parent tries to delete it).
-- =============================================
CREATE TRIGGER [dbo].[InsteadOfCollarFileDelete] 
   ON  [dbo].[CollarFiles] 
   INSTEAD OF DELETE
AS 
BEGIN
    SET NOCOUNT ON;
    
    -- triggers always execute in the context of a transaction
    -- so the following code is all or nothing.

    -- Delete fixes related to the deleted files
    DELETE X FROM dbo.CollarFixes AS X
       INNER JOIN deleted AS D
               ON D.FileId = X.FileId

	-- This trigger will NOT be called when deleting sub files,
	-- so we must delete sub-file fixes before deleting the sub-files
    DELETE X FROM dbo.CollarFixes AS X
       INNER JOIN dbo.CollarFiles AS F
               ON X.FileId = F.FileId
       INNER JOIN deleted AS D
               ON D.FileId = F.ParentFileId

    -- Delete children of the deleted files       
    DELETE F FROM dbo.CollarFiles AS F
       INNER JOIN deleted AS D
               ON D.FileId = F.ParentFileId
    
    -- Finally I can delete the files       
    DELETE F FROM dbo.CollarFiles AS F
       INNER JOIN deleted AS D
               ON D.FileId = F.FileId

END




GO
ALTER TABLE [dbo].[CollarFiles] ENABLE TRIGGER [InsteadOfCollarFileDelete]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE TRIGGER [dbo].[InsteadOfCollarFixesDelete] 
ON [dbo].[CollarFixes] 
INSTEAD OF DELETE AS
BEGIN
	SET NOCOUNT ON;
	
	-- deleted is a virtual table of fixes that are to be deleted.
	-- Multiple fixes can be deleted at once. deleted is not updateable.

	--Logic for removing a SINGLE CollarFix
	--  1) If the fix is hidden do nothing else remove the related location
	--  2) If this fix hides another fix (that is not being deleted)
	--		 then Replace hiddenby on hidden fix with hiddenby from deleted/hiding fix
	--       else do nothing.  Changing hiddenby will trigger changes to Locations
	--  3) Delete the fixes
	--  step 1 must be done before step 2, else there may be overlapping fixes
	--    which will cause a integrity error in the Locations table.
	
	-- Can I delete multiple fixes in one operation?
	--   For step 1, yes, as there is no changes to the fixes table.
	--   For step 2, NO!
	--   For step 3, Yes.
	--   Consider three fixes, 1,2,3 where 2 hides 1 and 3 hides 2.
	--   If I delete 2 and 3 at the same time, then there is no easy way to
	--   unwind the hidden-ness of fix 1 in a bulk operation.
	--   If it is done sequentially: removing 3 unhides 2 then removing 2 unhides 1
	--   alternatively, removing 2 first makes 1 hidden by 3, then removing 3 unhides 1.
	
	-- Step 1) remove record from Locations table
	DELETE L FROM Locations as L
	   INNER JOIN deleted as D
			   ON L.FixId = D.FixId
		    WHERE D.HiddenBy is NULL

	DECLARE
		@FixId    int,
		@HiddenBy int;
	  
	DECLARE delf_cursor CURSOR FOR 
		SELECT [FixId] FROM deleted;

	OPEN delf_cursor;

	FETCH NEXT FROM delf_cursor INTO @FixId;
	WHILE @@FETCH_STATUS = 0
	BEGIN

		-- must be done in this order to maintain uniqueness of CollarId/FixDate/HiddenBy Tuple
		
		-- Step 2) Get the Id of the Fix that hid this fix
		-- get hiddenby from CollarFixes, not deleted, since previous records may
		-- have CollarFixes.
		SET @HiddenBy = (Select HiddenBy from CollarFixes where FixId = @FixId)
		
		-- Step 3) Delete the requested Fix
		DELETE CollarFixes WHERE FixId = @FixId
		
		-- Step 4) What ever was hiding the fix being deleted should now be hiding
		-- the fix that the deleted fix was hiding.
		-- FIXME - searching the CollarFixes by HiddenBy (even when indexed) is slow
		-- PROBLEM - unhiding a fix creates a location, thereby prohibiting the deletion of the fix
		-- SOLUTION - do not unhide fixes in the deleted list  
		UPDATE C SET HiddenBy = @HiddenBy
		  FROM CollarFixes AS C
     LEFT JOIN deleted as D
		    ON D.FixId = C.FixId
		 WHERE C.HiddenBy = @FixId
		   AND D.FixId IS NULL;

		FETCH NEXT FROM delf_cursor INTO @FixId;
	END
	CLOSE delf_cursor;
	DEALLOCATE delf_cursor;
	
END


GO
ALTER TABLE [dbo].[CollarFixes] ENABLE TRIGGER [InsteadOfCollarFixesDelete]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE TRIGGER [dbo].[InsteadOfCollarFixesInsert] 
ON [dbo].[CollarFixes] 
INSTEAD OF INSERT AS
BEGIN
	SET NOCOUNT ON;
	-- inserted is a virtual table of inserted fixes.
	-- Multiple fixes can be inserted at once. inserted is not updateable.
	
	--Logic for adding a SINGLE CollarFix
	--  1) If the new fix has the same collar id/fixdate as an existing unhidden fix
	--     then hide the existing fix, else nothing.  hiding the fix will trigger
	--     a removal of the cooresponding location.
	--  2) Add a location for the new fix.
	--
	-- Can I add multiple fixes in one operation?
	--   For step 2, yes, as there is no changes to the fixes table.
	--   For Step 1, NO!
	--   Consider adding three fixes, 1,2,3 which have the same fixdate.
	--   two must be hidden, and one must be unhidden.  This is ambiguous
	--   when the fixes are inserted 'simultaneously'.  In order to maintain
	--   the chaining of fixes hiding one another, they must be processed
	--   sequentially.

	-- Another problem:
	--   We may need to modify some of the fixes in the inserted set.
	--   We cannot modify the virtual 'inserted' table.  Therefore some of
	--   our state will be in the CollarFixes table, and some in the inserted
	--   table.  This is very difficult to manage.
	
	-- Solution:
	--  Use a cursor to insert fixes one by one
	--  Use a 'instead of insert' trigger to cure the problem with
	--  modifying the virtual inserted table
	
	
	DECLARE
		@HiddenFixId		int,
		@FixId				int,
		@HiddenBy			int,
		@FileId				INT,
		@LineNumber			INT,
		@CollarManufacturer VARCHAR(16),
		@CollarId			VARCHAR(16),
		@FixDate			DATETIME2(7),
		@Lat				FLOAT,
		@Lon				FLOAT;
	  
	DECLARE insf_cursor CURSOR FOR 
		SELECT HiddenBy, FileId, LineNumber, CollarManufacturer, CollarId, FixDate, Lat, Lon
		  FROM inserted
	-- I cannot select FixId (Identity column) from inserted, because it is set to zero in the cursor

	OPEN insf_cursor;

	FETCH NEXT FROM insf_cursor INTO @HiddenBy, @FileId, @LineNumber, @CollarManufacturer, @CollarId, @FixDate, @Lat, @Lon;
	
	WHILE @@FETCH_STATUS = 0
	BEGIN

		-- Step 1) Get the id of the fix we will hide (if it is null, then we are not hiding anything)
		SET @HiddenFixId = NULL  -- The assignment below does not occur if no record is found
		SELECT @HiddenFixId = FixId FROM CollarFixes
		 WHERE CollarFixes.CollarManufacturer = @CollarManufacturer
		   AND CollarFixes.CollarId = @CollarID
		   AND CollarFixes.FixDate = @FixDate
		   AND CollarFixes.HiddenBy IS NULL
		
		-- Step 2) Add new fix to table (do the insert that fired this trigger)
		INSERT CollarFixes (HiddenBy, FileId, LineNumber, CollarManufacturer, CollarId, FixDate, Lat, Lon)
		VALUES (@HiddenBy, @FileId, @LineNumber, @CollarManufacturer, @CollarId, @FixDate, @Lat, @Lon)
		SET @FixId = SCOPE_IDENTITY();

		-- PRINT N'  FixDate:' + cast(@FixDate as nvarchar(30)) + N';  HiddenFixId:' + isnull(cast(@HiddenFixId as nvarchar(30)), N'null') + N';  NewFixId:' + isnull(cast(@FixId as nvarchar(30)), N'null')

		-- IF @HiddenFixId IS NOT NULL We are temporarily unstable becasue we have two active & conflicting fixes
		
		-- Step3) Hide the hidden fix (the update trigger will remove the associated location)
		IF @HiddenFixId IS NOT NULL
		BEGIN
			UPDATE CollarFixes SET HiddenBy = @FixId WHERE FixId = @HiddenFixId;
		END
		
		
		-- Step 4) Add new record to Locations table
		-- This should not create a Location if there is no matching deployment
		-- The new fix is never hidden.
		-- there must be zero or one deployment/animal for this fixdate
		-- A @Lat outside [-90, 90] will throw an exception (https://msdn.microsoft.com/en-us/library/bb964737), so skip.
		IF -90 <= @Lat AND @Lat <= 90
		BEGIN
			INSERT INTO Locations (ProjectId, AnimalId, FixDate, Location, FixId)
				 SELECT D.ProjectId, D.AnimalId, @FixDate, geography::Point(@Lat, @Lon, 4326) AS Location, @FixId
				   FROM CollarDeployments as D 
			 INNER JOIN dbo.Animals AS A
 					 ON A.ProjectId = D.ProjectId
		    		AND A.AnimalId = D.AnimalId
			 INNER JOIN dbo.Collars AS C
					 ON C.CollarManufacturer = D.CollarManufacturer
					AND C.CollarId = D.CollarId
				  WHERE D.CollarManufacturer = @CollarManufacturer AND D.CollarId = @CollarId
					AND D.DeploymentDate <= @FixDate
					AND (D.RetrievalDate IS NULL OR @FixDate <= D.RetrievalDate)
	    			AND (A.MortalityDate IS NULL OR @FixDate <= A.MortalityDate)
					AND (C.DisposalDate IS NULL OR @FixDate <= C.DisposalDate)
		END
		FETCH NEXT FROM insf_cursor INTO @HiddenBy, @FileId, @LineNumber, @CollarManufacturer, @CollarId, @FixDate, @Lat, @Lon;
	END
	CLOSE insf_cursor;
	DEALLOCATE insf_cursor;
END



GO
ALTER TABLE [dbo].[CollarFixes] ENABLE TRIGGER [InsteadOfCollarFixesInsert]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE TRIGGER [dbo].[UpdateLocationAfterCollarFixesUpdate] 
ON [dbo].[CollarFixes]
AFTER UPDATE AS
IF UPDATE ([HiddenBy])
	-- HiddenBy is the only field that is allowed to be updated (all other changes must
	-- be done with a delete/insert.  This is managed with permissions on the table and
	-- the functionality of permitted stored procedures
	BEGIN
		SET NOCOUNT ON;

		-- In an update, the old rows are in 'deleted', and the new rows are in 'inserted'.
		-- deleted/inserted are tables, since multiple records can be changed at once.
		-- All updated rows are included, even those that didn't update hiddenby
		
		-- Basic logic for a changing a SINGLE CollarFix:
		--   if HiddenBy goes from NULL to NOT NULL then remove record from Locations table
		--   if HiddenBy goes from NOT NULL to NULL then add new record to Locations table
		
		-- Can I change multiple fixes at once?
		--  Yes.  Since changes to CollarFixes are managed, and only done by routines
		--        that honor the requirements of the HiddenBy relationship between
		--        records in the table, I do not need to worry about how these changes
		--        effect other records in the table.  I only need to manage the changes
		--        to the locations table.
		
		-- if HiddenBy goes from NULL to NOT NULL then
		-- remove record from Locations table
		DELETE L FROM Locations as L
		   INNER JOIN deleted as D
				   ON L.FixId = D.FixId
		   INNER JOIN inserted as I
				   ON L.FixId = I.FixId
				WHERE D.HiddenBy IS NULL AND I.HiddenBy IS NOT NULL
		
		-- if HiddenBy goes from NOT NULL to NULL then
		-- Add new record to Locations table, if FixDate is acceptable 
		INSERT INTO Locations (ProjectId, AnimalId, FixDate, Location, FixId)
			 SELECT CD.ProjectId, CD.AnimalId, I.FixDate, geography::Point(I.Lat, I.Lon, 4326) AS Location, I.FixId
			   FROM inserted as I
		 INNER JOIN deleted as D
				 ON D.FixId = I.FixId
		 INNER JOIN dbo.CollarDeployments as CD
				 ON CD.CollarManufacturer = I.CollarManufacturer
				AND CD.CollarId = I.CollarId
         INNER JOIN dbo.Animals AS A
 	             ON A.ProjectId = CD.ProjectId
		    	AND A.AnimalId = CD.AnimalId
		 INNER JOIN dbo.Collars AS C
				 ON C.CollarManufacturer = CD.CollarManufacturer
				AND C.CollarId = CD.CollarId
			  WHERE D.HiddenBy IS NOT NULL AND I.HiddenBy IS NULL
			    AND CD.DeploymentDate <= I.FixDate
			    AND (CD.RetrievalDate IS NULL OR I.FixDate <= CD.RetrievalDate)
	    	    AND (A.MortalityDate IS NULL OR I.FixDate <= A.MortalityDate) 
				AND (C.DisposalDate IS NULL OR I.FixDate <= C.DisposalDate)

	END



GO
ALTER TABLE [dbo].[CollarFixes] ENABLE TRIGGER [UpdateLocationAfterCollarFixesUpdate]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Regan Sarwas
-- Create date: May 7, 2013
-- Description:	Validate and enforce business rules
--              when updating collar parameter files
-- =============================================
CREATE TRIGGER [dbo].[AfterCollarParameterFileUpdate] 
    ON  [dbo].[CollarParameterFiles] 
    AFTER UPDATE
AS 
BEGIN
    SET NOCOUNT ON;

    -- triggers always execute in the context of a transaction
    -- so the following code is all or nothing.


    -- Business Rule: FileId, Contents, Username, UploadDate, and Sha1Hash cannot be updated.
    --                Sha1Hash is managed by the DB engine since it is a computed column
    IF    UPDATE(FileId) OR UPDATE(Contents) OR  UPDATE(UploadUser) OR UPDATE(UploadDate)
    BEGIN
        RAISERROR('Integrity Violation. Attempted to modify an immutable column in CollarFiles', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END


    -- Business Rule: Format cannot be changed if CollarParameterFile is used in a CollarParameter
    IF EXISTS (    SELECT 1
                     FROM inserted AS i
                     JOIN deleted AS d
                       ON i.FileId = d.FileId
               INNER JOIN CollarParameters AS P
                       ON P.FileId = i.FileId
                    WHERE i.[Format] <> d.[Format]
              )
    BEGIN
        RAISERROR('Integrity Violation. Format cannot be changed when the file is linked to a Collar', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END


    -- Business Rule: Status cannot be changed if CollarParameterFile is used in a CollarParameter
    IF EXISTS (    SELECT 1
                     FROM inserted AS i
                     JOIN deleted AS d
                       ON i.FileId = d.FileId
               INNER JOIN CollarParameters AS P
                       ON P.FileId = i.FileId
                    WHERE i.[Status] <> d.[Status]
              )
    BEGIN
        RAISERROR('Integrity Violation. Status cannot be changed when the file is linked to a Collar', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END

END

GO
ALTER TABLE [dbo].[CollarParameterFiles] ENABLE TRIGGER [AfterCollarParameterFileUpdate]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Regan Sarwas
-- Create date: May 13, 2013
-- Description: Validates the business rules for a new CollarParameter
-- =============================================
CREATE TRIGGER [dbo].[AfterCollarParameterInsert] 
   ON  [dbo].[CollarParameters] 
   AFTER INSERT
AS 
BEGIN
    SET NOCOUNT ON;

    -- Validate Business Rules
    -- 1. Ensure StartDate is before the EndDate
    IF EXISTS (SELECT 1
                 FROM inserted AS I
                WHERE I.EndDate <= I.StartDate  -- false if either is NULL
              )
    BEGIN
        RAISERROR('The end of the parameter must occur after the start of the parameter.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END

    -- 2. Ensure the Parameter begins before the DisposalDate of the Collar
    IF EXISTS (SELECT 1
                 FROM inserted AS I
           INNER JOIN Collars AS C
                   ON I.CollarManufacturer = C.CollarManufacturer AND I.CollarId = C.CollarId
                WHERE C.DisposalDate < I.StartDate
              )
    BEGIN
        RAISERROR('The parameter must begin before the collar is disposed.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END

    -- 3. Ensure a Collar does not have multiple Parameters at the same time
    -- We are checking each inserted parameter against all existing parameters, and all other inserted parameters 
    IF EXISTS (SELECT 1
                 FROM inserted AS I1
            LEFT JOIN inserted AS I2
                   ON I1.CollarManufacturer = I2.CollarManufacturer AND I1.CollarId = I2.CollarId AND I1.ParameterId <> I2.ParameterId
           INNER JOIN dbo.CollarParameters AS P
                   ON P.CollarManufacturer = I1.CollarManufacturer AND P.CollarId = I1.CollarId AND P.ParameterId <> I1.ParameterId
                WHERE dbo.DoDateRangesOverlap(P.StartDate, P.EndDate, I1.StartDate, I1.EndDate) = 1
                   OR (I2.ParameterId IS NOT NULL AND
                       dbo.DoDateRangesOverlap(I1.StartDate, I1.EndDate, I2.StartDate, I2.EndDate) = 1)
              )
    BEGIN
        RAISERROR('Collars cannot have overlapping parameter dates.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END

    -- 4. Ensure that one and only one of FileId and Gen3Period are non-null
    IF EXISTS (SELECT 1
                 FROM inserted
                WHERE (Gen3Period IS NULL AND FileId IS NULL) OR
                      (Gen3Period IS NOT NULL AND FileId IS NOT NULL)
              )
    BEGIN
        RAISERROR('One and only one of Gen3 period and parameter file must be provided.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END

    -- 5. Inactive parameter files cannot be linked to a collar
    IF EXISTS (SELECT 1
                 FROM inserted AS I
           INNER JOIN CollarParameterFiles AS F
                   ON F.FileId = I.FileId
                WHERE F.[Status] <> 'A'
              )
    BEGIN
        RAISERROR('Inactive parameter files cannot be linked to a collar.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END


    -- Process (partialy?) all files that might benefit from this parameter
    -- We do not do the processing in the trigger, as it may require an external process
    -- which would block the transaction and hold a lock that would slow processing.
    -- Instead, we will monitor for changes that will trigger the external processing.

    -- If a processed file had no issues, then do not reprocess.
    --    This parameter is irrelevant to the file - the file had everything it when it was processed.
    -- If a processed file had issues
    --    without mentioning the collar, then it was unable to link the platform to a collar, so
    --      this new information will not due any good.
    --    with the collar in this parameter, then it was probably a missing parameter.
    --      this file should be re-processed for the matching platfrom(s).  This can be done by
    --      removing the issue.  The (file,platform) will then be recognized in need of re-processing.
    -- If a file is unprocessed for some reason, then it is not in our perview to decide that it should
    --   be processed, so we will skip it, even if it has transmissions for this platform. (The file should get
    --   processed at some point in the future by the user or a scheduled task at which point the new parameter
    --   will be considered.)

    -- May be part of a batch insert (unlikely, and only by SA as all others must use SP)
    -- Triggers always execute in the context of a transaction, so the following code is all or nothing.

        DELETE A 
          FROM inserted AS I
    INNER JOIN ArgosFileProcessingIssues AS A
            ON A.CollarManufacturer = I.CollarManufacturer AND A.CollarId = I.CollarId

END

GO
ALTER TABLE [dbo].[CollarParameters] ENABLE TRIGGER [AfterCollarParameterInsert]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Regan Sarwas
-- Create date: March 21, 2013
-- Modified:    June 29, 2018 - Bug Fixes
-- Description: Validates the business rules for an Updated CollarParameter
-- =============================================
CREATE TRIGGER [dbo].[AfterCollarParameterUpdate] 
   ON  [dbo].[CollarParameters] 
   AFTER UPDATE
AS 
BEGIN
    SET NOCOUNT ON;
    
    -- Validate Business Rules
    
    -- 0. Deny updating the ParameterId
    IF UPDATE(ParameterId)
    BEGIN
        RAISERROR('The primary key (ParameterId) cannot be changed.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END
    
    -- 1. Ensure StartDate is before the EndDate
    IF EXISTS (SELECT 1
                 FROM inserted AS I
                WHERE I.EndDate <= I.StartDate  -- false if either is NULL
              )
    BEGIN
        RAISERROR('The end of the parameter must occur after the start of the parameter.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END

    -- 2a. Ensure the start date is before the DisposalDate of the Collar
    IF EXISTS (SELECT 1
                 FROM inserted AS I
           INNER JOIN Collars AS C
                   ON I.CollarManufacturer = C.CollarManufacturer AND I.CollarId = C.CollarId
                WHERE C.DisposalDate < I.StartDate
              )
    BEGIN
        RAISERROR('The parameter must start before the collar is disposed.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END

    -- 3. Ensure a Collar does not have multiple Parameters at the same time
    -- We are checking each inserted parameter against all existing parameters, and all other inserted parameters 
    IF EXISTS (SELECT 1
                 FROM inserted AS I1
            LEFT JOIN inserted AS I2
                   ON I1.CollarManufacturer = I2.CollarManufacturer AND I1.CollarId = I2.CollarId AND I1.ParameterId <> I2.ParameterId
           INNER JOIN dbo.CollarParameters AS P
                   ON P.CollarManufacturer = I1.CollarManufacturer AND P.CollarId = I1.CollarId AND P.ParameterId <> I1.ParameterId
                WHERE dbo.DoDateRangesOverlap(P.StartDate, P.EndDate, I1.StartDate, I1.EndDate) = 1
                   OR (I2.ParameterId IS NOT NULL AND
                       dbo.DoDateRangesOverlap(I1.StartDate, I1.EndDate, I2.StartDate, I2.EndDate) = 1)
              )
    BEGIN
        RAISERROR('Collars cannot have overlapping parameter dates.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END

    -- 4. Ensure that one and only one of FileId and Gen3Period are non-null
    IF EXISTS (SELECT 1
                 FROM inserted
                WHERE (Gen3Period IS NULL AND FileId IS NULL) OR
                      (Gen3Period IS NOT NULL AND FileId IS NOT NULL)
              )
    BEGIN
        RAISERROR('One and only one of Gen3 period and parameter file must be provided.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END

    -- 5. Inactive parameter files cannot be linked to a collar
    IF EXISTS (SELECT 1
                 FROM inserted AS I
           INNER JOIN CollarParameterFiles AS F
                   ON F.FileId = I.FileId
                WHERE F.[Status] <> 'A'
              )
    BEGIN
        RAISERROR('Inactive parameter files cannot be linked to a collar.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END


    /*
    An update can be thought of as a delete and an insert, and the logic in the
    delete and insert triggers is basically replicated here when more than the
    dates change in a Collar Parameter.
    However this is very ineffcient in the typical situation:
      A parameter is created with StartDate = XXX and EndDate = NULL. The user wants
      to create a a new parameter with a StartDate = YYY and EndDate = NULL. where XXX < YYY.
      before the insert can succeed the EndDate of the existing parameter must be set to YYY.
    Using a brute force method, all existing processing based on the parameter would
    be deleted then recreated.
    Instead, I will check for these cases:
    If only the dates have changed (no collar, fileid, or Gen3period change):
      Start or End date has shrunk:
        Flag any files with transmission outside the new date range as
        a processing issue (The same as would happen if the deployment had the non-null dates
        to start with.)  No additional processing needs to happen until a new deployment is created.
      Start or End date has grown:
        Find files with processing issues for this platform, and with transmissions in the new part
        of the date range, then reprocess them, because the expanded dates will add transmissions. 
    Else if only the Collar has changed (no date or platform change)
      I though I might find the result files for the old collar, deactive them, change the collar,
      then reactivate them, but there may be results files for other deployments for the same old
      collar which should not be changed. Since we cannot trivially determine the deployment (or
      even the platform) for a results file, we cannot select just the collar files to change.
      It might be possible to do something more clever, but my head already hurts, so I will
      simply handle it safely as follows.
    Else if the platform changed, or there were multiple items changing,
      use the brute force delete/insert method. 
    */

    -- May be part of a batch update (happens with a cascading update or when the SA bypasses the SP)
    -- Triggers always execute in the context of a transaction, so the following code is all or nothing.
    -- to keep the logic managable, I will create a cursor to handle each deployment separately.

    IF EXISTS (SELECT 1
                 FROM inserted AS I
                 JOIN deleted AS D
                   ON D.ParameterId = I.ParameterId
                WHERE I.CollarId <> D.CollarId
                   OR I.CollarManufacturer <> D.CollarManufacturer
                   OR I.FileId <> D.FileId OR (I.FileId IS NULL AND D.FileId IS NOT NULL) OR (I.FileId IS NOT NULL AND D.FileId IS NULL)
                   OR I.Gen3Period <> D.Gen3Period OR (I.Gen3Period IS NULL AND D.Gen3Period IS NOT NULL) OR (I.Gen3Period IS NOT NULL AND D.Gen3Period IS NULL)
               )
    BEGIN
        -- Brute Force Solution:
        -- 1. Delete => Delete files derived from this parameter
            DELETE F 
              FROM inserted AS I
        INNER JOIN CollarFiles AS F
                ON F.CollarParameterId = I.ParameterId
        -- 2. Insert => Delete Processing Issues for the new collar
            DELETE A 
              FROM inserted AS I
        INNER JOIN ArgosFileProcessingIssues AS A
                ON A.CollarManufacturer = I.CollarManufacturer AND A.CollarId = I.CollarId
    END
    ELSE BEGIN
        -- Only the dates have changed
        -- Date range shrunk - Create a new processing issue to explain any missed data
            -- Start Shrunk to after first transmission
            INSERT INTO [dbo].[ArgosFileProcessingIssues]
                        ([FileId], [Issue], [PlatformId], [CollarManufacturer], [CollarId], [FirstTransmission], [LastTransmission])
                 SELECT P.FileId, 'No Collar Parameter for Collar from ' + 
                        CONVERT(varchar(20), S.FirstFix, 20) + ' to ' + CONVERT(varchar(20), I.StartDate, 20),
                        A.PlatformId, I.CollarManufacturer, I.CollarId, FirstFix, LastFix
                   FROM CollarFiles AS C
             INNER JOIN CollarFiles AS P
                     ON P.FileId = C.ParentFileId
             INNER JOIN inserted AS I
                     ON I.ParameterId = C.CollarParameterId
             INNER JOIN deleted AS D
                     ON D.ParameterId = I.ParameterId
             INNER JOIN ArgosDeployments AS A
                     ON A.DeploymentId = C.ArgosDeploymentId
             INNER JOIN (select FileID, min(FixDate) as FirstFix, max(FixDate) as LastFix from collarfixes group by Fileid) AS S
                     ON S.FileId = C.FileId
                  WHERE ((D.StartDate IS NULL AND I.StartDate IS NOT NULL) OR (D.StartDate < I.StartDate)) -- Parameter start date shrank
                    AND S.FirstFix < I.StartDate                                                  -- There is a transmission after the new start date
            -- End Shrunk before last transmission
            INSERT INTO [dbo].[ArgosFileProcessingIssues]
                        ([FileId], [Issue], [PlatformId], [CollarManufacturer], [CollarId], [FirstTransmission], [LastTransmission])
                 SELECT P.FileId, 'No Collar Parameter for Collar from ' + 
                        CONVERT(varchar(20), I.EndDate, 20) + ' to ' + CONVERT(varchar(20), S.LastFix, 20),
                        A.PlatformId, I.CollarManufacturer, I.CollarId, FirstFix, LastFix
                   FROM CollarFiles AS C
             INNER JOIN CollarFiles AS P
                     ON P.FileId = C.ParentFileId
             INNER JOIN inserted AS I
                     ON I.ParameterId = C.CollarParameterId
             INNER JOIN deleted AS D
                     ON D.ParameterId = I.ParameterId
             INNER JOIN ArgosDeployments AS A
                     ON A.DeploymentId = C.ArgosDeploymentId
             INNER JOIN (select FileID, min(FixDate) as FirstFix, max(FixDate) as LastFix from collarfixes group by Fileid) AS S
                     ON S.FileId = C.FileId
                  WHERE ((D.EndDate IS NULL AND I.EndDate IS NOT NULL) OR (I.EndDate < D.EndDate)) -- Parameter end date shrank
                    AND I.EndDate < S.LastFix                                             -- There is a transmission before the new end date
			-- Remove (unprocess) any result files that are no longer covered by parameter
			-- Similar to SPROC ArgosFile_UnProcessPlatform(@fileID, @platformId) and update trigger in ArgosDeployments
                 DELETE CollarFiles WHERE FileId in (
				 SELECT C.FileId
                   FROM CollarFiles AS C
             INNER JOIN inserted AS I
                     ON I.ParameterId = C.CollarParameterId
             INNER JOIN deleted AS D
                     ON D.ParameterId = I.ParameterId
             INNER JOIN (select FileID, min(FixDate) as FirstFix, max(FixDate) as LastFix from collarfixes group by Fileid) as F
                     ON F.FileId = C.FileId
                  WHERE
					    -- Start Shrunk to after first Fix
					    (((D.StartDate IS NULL AND I.StartDate IS NOT NULL) OR (D.StartDate < I.StartDate)) AND F.FirstFix < I.StartDate)
					    OR
					    -- End Shrunk before last Fix
					    (((D.EndDate IS NULL AND I.EndDate IS NOT NULL) OR (I.EndDate < D.EndDate)) AND I.EndDate < F.LastFix)
				  )

        -- Date range increased - Remove overlapping processing issue to trigger a re-process
                 DELETE P
                   FROM ArgosFileProcessingIssues AS P
             INNER JOIN inserted AS I
                     ON I.CollarManufacturer = P.CollarManufacturer AND I.CollarId = P.CollarId
             INNER JOIN deleted AS D
                     ON D.ParameterId = I.ParameterId
                  WHERE (    ((D.StartDate IS NOT NULL AND I.StartDate IS NULL) OR D.StartDate > I.StartDate)                -- Parameter start date grew 
						 AND dbo.DoDateRangesOverlap(P.FirstTransmission, P.LastTransmission, I.StartDate, D.StartDate) = 1) -- issue dates overlap new range at start
					 OR
				        (    ((D.EndDate IS NOT NULL AND I.EndDate IS NULL) OR I.EndDate > D.EndDate)                        -- Parameter end date grew
                         AND dbo.DoDateRangesOverlap(P.FirstTransmission, P.LastTransmission, D.EndDate, I.EndDate) = 1)     -- issue dates overlap new range at end
    END
END

GO
ALTER TABLE [dbo].[CollarParameters] ENABLE TRIGGER [AfterCollarParameterUpdate]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:      Regan Sarwas
-- Create date: Feb 20, 2013
-- Description: Ensure data integrity on Collar Updates
-- =============================================
CREATE TRIGGER [dbo].[AfterCollarUpdate] 
			ON [dbo].[Collars] 
			AFTER UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	
	-- FIXME: If we change the disposal date we might want to check
	--   Argos Deployments, and Parameter End dates (I've done collardeployments)
	
	-- FIXME: If we change the HasGPS, then we need to consider the PPT fixes in some files
	
	-- FIXME: If we change the model, then we may need to do change how files are processed
	
	-- FIXME: are changes to PK allowed? if so, are they cascading to all related tables
	
	IF UPDATE ([DisposalDate])
	BEGIN

		-- triggers always execute in the context of a transaction
		-- so the following code is all or nothing.

	-- Business Rule: A CollarDeployment cannot begin after the disposal date of a collar.
	--                Note that the retrieval date can be after the disposal date (it may never be retrieved) 

	-- i.e. illegal to deploy a collar after it is disposed, or dispose a collar before it is deployed.
	-- We do not need to check disposal date on collar creation, because a new collar has NO deployments
	IF EXISTS (SELECT 1
	             FROM inserted AS I
	       INNER JOIN CollarDeployments AS CD
				   ON I.CollarManufacturer = CD.CollarManufacturer AND I.CollarId = CD.CollarId
				WHERE I.DisposalDate < CD.DeploymentDate
			  )
	BEGIN
		RAISERROR('Disposal date violation.  There is a deployment that begins after the new disposal date.', 18, 0)
		ROLLBACK TRANSACTION;
		RETURN
	END


	-- Business Rule: No collar locations after a disposal date
    -- LOGIC:
    --   delete locations where new disposal date < fix date AND (fix date < old disposal date OR old disposal date is null) 
    --      add locations where old disposal date < fix date AND (fix date < new disposal date OR new disposal date is null)

		DELETE L FROM dbo.Locations as L
				   -- Join to the collar that created this location
		   INNER JOIN CollarFixes as F
				   ON L.FixId = F.FixId
		   INNER JOIN deleted as D
				   ON F.CollarManufacturer = D.CollarManufacturer
				  AND F.CollarId = D.CollarId
		   INNER JOIN inserted as I
				   -- To get the new disposal date, we need to link inserted and deleted collars by PK
				   ON I.CollarManufacturer = D.CollarManufacturer
				  AND I.CollarId = D.CollarId
				   -- These are the Locations to delete:
				WHERE I.DisposalDate < L.FixDate AND (L.FixDate < D.DisposalDate OR D.DisposalDate IS NULL) 
				

		  INSERT INTO Locations (ProjectId, AnimalId, FixDate, Location, FixId)
			   SELECT CD.ProjectId, CD.AnimalId, F.FixDate, geography::Point(F.Lat, F.Lon, 4326), F.FixId
				 FROM dbo.CollarFixes AS F
				   -- Join to CollarDeployments to get to the animal
		   INNER JOIN CollarDeployments AS CD
				   ON F.CollarManufacturer = CD.CollarManufacturer
				  AND F.CollarId = CD.CollarId
				  AND CD.DeploymentDate < F.FixDate
				  AND (F.FixDate < CD.RetrievalDate OR CD.RetrievalDate IS NULL)
				   -- Join these deployments to the collar being updated
		   INNER JOIN inserted AS I
				   ON CD.CollarManufacturer = I.CollarManufacturer
				  AND CD.CollarId = I.CollarId
		   INNER JOIN deleted as D
				   -- To get the new disposal date, we need to link inserted and deleted collars by PK
				   ON I.CollarManufacturer = D.CollarManufacturer
				  AND I.CollarId = D.CollarId
				   -- These are the Fixes to add:
				WHERE F.HiddenBy IS NULL
				  AND D.DisposalDate < F.FixDate AND (F.FixDate < I.DisposalDate OR I.DisposalDate IS NULL) 
	END
END


GO
ALTER TABLE [dbo].[Collars] ENABLE TRIGGER [AfterCollarUpdate]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO








-- Microsoft Says: We do not recommend using cursors in triggers because they could potentially reduce performance. 
--                 To design a trigger that affects multiple rows, use rowset-based logic instead of cursors
-- However, the rowset based logic was too difficult to manage:
--   Because I think procedurally, and I needed to break the problem up into procedures to understand/code, however
--   while I can put a function in rowset logic, a function cannot use an INSERT/DELETE
--   while I can put an INSERT/DELETE in a stored procedure, I can't put a stored procedure in rowset logic.
--   Therefore I need to abandon my procedure based thinking, or I can use cursors. 
--   Example rowset logic with multi-column key in deleted and inserted virtual tables
--     DELETE m from [dbo].[Movement] as m inner join deleted as d on m.AnimalID = d.AnimalID and (d.fixdate = m.startdate or d.fixdate = m.enddate);
--     SELECT * FROM Movements as m Where Exists(Select 1 from inserted as i where i.AnimalID = m.AnimalID and (i.FixDate = m.StartDate or i.FixDate = m.EndDate)) 

-- I do not delete all movements in one operation (possible and faster),
--   because it is too hard to then figure out how to close all possible gaps
--   with new movements - the new locations may be in sequence, or random.


CREATE TRIGGER [dbo].[UpdateMovementAfterLocationDelete] 
ON [dbo].[Locations] 
AFTER DELETE AS
BEGIN
	SET NOCOUNT ON;
	DECLARE
		@Project VARCHAR(32),
		@Animal  VARCHAR(32),
		@Time    DATETIME2;
	  
	DECLARE del_cursor CURSOR FOR 
		SELECT [ProjectId], [AnimalId], [FixDate]
		  FROM deleted
		 WHERE [Status] IS NULL  --Deleting a location with a non-null status will not change the movements
	  ORDER BY [ProjectId], [AnimalId], [FixDate];

	OPEN del_cursor;

	FETCH NEXT FROM del_cursor INTO @Project, @Animal, @Time;

	WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC [dbo].[Location_Deleted] @Project, @Animal, @Time;
		FETCH NEXT FROM del_cursor INTO @Project, @Animal, @Time;
	END
	CLOSE del_cursor;
	DEALLOCATE del_cursor;
END





GO
ALTER TABLE [dbo].[Locations] ENABLE TRIGGER [UpdateMovementAfterLocationDelete]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







CREATE TRIGGER [dbo].[UpdateMovementAfterLocationInsert] 
ON [dbo].[Locations] 
AFTER INSERT AS
BEGIN
	SET NOCOUNT ON;
	DECLARE
		@Project VARCHAR(32),
		@Animal  VARCHAR(32),
		@Time    DATETIME2;
	  
	DECLARE ins_cursor CURSOR FOR 
		SELECT [ProjectId], [AnimalId], [FixDate]
		  FROM inserted
		 WHERE [Status] IS NULL  -- Inserting a location with a non-null status will not change the movements
	  ORDER BY [ProjectId], [AnimalId], [FixDate];

	OPEN ins_cursor;

	FETCH NEXT FROM ins_cursor INTO @Project, @Animal, @Time;

	WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC [dbo].[Location_Added] @Project, @Animal, @Time ;
		FETCH NEXT FROM ins_cursor INTO @Project, @Animal, @Time;
	END
	CLOSE ins_cursor;
	DEALLOCATE ins_cursor;
END





GO
ALTER TABLE [dbo].[Locations] ENABLE TRIGGER [UpdateMovementAfterLocationInsert]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







CREATE TRIGGER [dbo].[UpdateMovementAfterLocationUpdate] 
ON [dbo].[Locations]
AFTER UPDATE AS
IF UPDATE ([Status])
	BEGIN
		SET NOCOUNT ON;
		DECLARE
			@Project   VARCHAR(32),
			@Animal    VARCHAR(32),
			@Time      DATETIME2,
			@OldStatus CHAR(1),
			@NewStatus CHAR(1);
		  
		-- In an update, the old row is in 'deleted', and the new row is in 'inserted'.

		DECLARE up_cursor CURSOR FOR 
			SELECT [ProjectId], [AnimalId], [FixDate], [Status] FROM deleted ORDER BY [ProjectId], [AnimalId], [FixDate];

		OPEN up_cursor;

		FETCH NEXT FROM up_cursor INTO @Project, @Animal, @Time, @OldStatus;

		WHILE @@FETCH_STATUS = 0
		BEGIN
			SET @NewStatus = (SELECT [Status] FROM inserted WHERE [ProjectId] = @Project AND [AnimalId] = @Animal AND [FixDate] = @Time);
			
			IF @OldStatus IS NULL and @NewStatus IS NOT NULL
				EXEC [dbo].[Location_Deleted] @Project, @Animal, @Time;
			
			IF @OldStatus IS NOT NULL and @NewStatus IS NULL
				EXEC [dbo].[Location_Added] @Project, @Animal, @Time; 
			
			FETCH NEXT FROM up_cursor INTO @Project, @Animal, @Time, @OldStatus;
		END
		CLOSE up_cursor;
		DEALLOCATE up_cursor;
	END





GO
ALTER TABLE [dbo].[Locations] ENABLE TRIGGER [UpdateMovementAfterLocationUpdate]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Regan Sarwas
-- Create date: June 11, 2012
-- Description:	Drops a user from the database if
--              it is no longer being referenced.
-- =============================================
CREATE TRIGGER [dbo].[AfterProjectEditorDelete] 
ON [dbo].[ProjectEditors] 
AFTER DELETE AS
BEGIN
	SET NOCOUNT ON;
	DECLARE
		@ProjectId nvarchar(255) = Null,
		@Editor sysname  = Null
	
	-- Loop over all the editors to be deleted, and
	-- if 1) they are the not a PI, and
	--    2) they will no longer have a presense in the Editors table,
	-- then drop them from the DB
	
	DECLARE del_cursor CURSOR FOR 
		SELECT [ProjectId], [Editor] FROM deleted

	OPEN del_cursor;

	FETCH NEXT FROM del_cursor INTO @ProjectId, @Editor;

	WHILE @@FETCH_STATUS = 0
	BEGIN
		-- If this Editor is a project investigator, then do nothing more
		IF EXISTS(SELECT 1 FROM [dbo].[ProjectInvestigators] WHERE [Login] = @Editor)
		BEGIN
			FETCH NEXT FROM del_cursor INTO @ProjectId, @Editor;
			CONTINUE
		END

		-- If this Editor is still an editor (on this or other projects after deleting), then do nothing more
		IF EXISTS(select 1 from [dbo].[ProjectEditors] AS E
					where not exists
					     (select * from deleted as D where D.ProjectId = E.ProjectId AND D.Editor = E.Editor)
					 AND Editor = @Editor)
		BEGIN
			FETCH NEXT FROM del_cursor INTO @ProjectId, @Editor;
			CONTINUE
		END
		
		-- This editor is not an editor on any projects, so remove the user from the database
		-- This will also, obviously, remove them from the Editor role
		-- If this fails, the user will still have permissions to run the stored procedures,
		--  however the stored procedures should stop them because they are not in the
		--  ProjectEditor table
		IF EXISTS (SELECT 1 from sys.database_principals WHERE type = 'U' AND name = @Editor)
			EXEC ('DROP USER ['  + @Editor + ']') --  LOGIN defaults to the same name

		FETCH NEXT FROM del_cursor INTO @ProjectId, @Editor;		
	END
	CLOSE del_cursor;
	DEALLOCATE del_cursor;
END


GO
ALTER TABLE [dbo].[ProjectEditors] ENABLE TRIGGER [AfterProjectEditorDelete]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Regan Sarwas
-- Create date: April 25, 2013
-- Description:	Drops a user from the database if
--              it is no longer being referenced.
-- =============================================
CREATE TRIGGER [dbo].[AfterProjectInvestigatorAssistantDelete] 
ON [dbo].[ProjectInvestigatorAssistants] 
AFTER DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE
        @ProjectInvestigator sysname = NULL,
        @Assistant           sysname = NULL
    
    -- Loop over all the assistants to be deleted, and
    -- if 1) they are the not a PI, and
    --    2) they will no longer have a presense in the Assistants table,
    -- then drop them from the DB
    
    DECLARE del_cursor CURSOR FOR 
        SELECT [ProjectInvestigator], [Assistant] FROM deleted

    OPEN del_cursor;

    FETCH NEXT FROM del_cursor INTO @ProjectInvestigator, @Assistant;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- If this Assistant is a project investigator, then do nothing more
        IF EXISTS(SELECT 1 FROM [dbo].[ProjectInvestigators] WHERE [Login] = @Assistant)
        BEGIN
            FETCH NEXT FROM del_cursor INTO @ProjectInvestigator, @Assistant;
            CONTINUE
        END

        -- If this Assistant is still an Assistant (for another project investigator after deleting), then do nothing more
        IF EXISTS(SELECT 1 FROM [dbo].[ProjectInvestigatorAssistants] AS A
                   WHERE A.ProjectInvestigator <> @ProjectInvestigator
                     AND A.Assistant = @Assistant)
        BEGIN
            FETCH NEXT FROM del_cursor INTO @ProjectInvestigator, @Assistant;
            CONTINUE
        END
        
        -- This Assistant is not an Assistant on any projects, so remove the user from the database
        -- This will also, obviously, remove them from the Editor role
        -- If this fails, the user will still have permissions to run the stored procedures,
        --  however the stored procedures should stop them because they are not in the
        --  ProjectInvestigatorAssistants table
        IF EXISTS (SELECT 1 from sys.database_principals WHERE type = 'U' AND name = @Assistant)
            EXEC ('DROP USER ['  + @Assistant + ']') --  LOGIN defaults to the same name

        FETCH NEXT FROM del_cursor INTO @ProjectInvestigator, @Assistant;		
    END
    CLOSE del_cursor;
    DEALLOCATE del_cursor;
END




GO
ALTER TABLE [dbo].[ProjectInvestigatorAssistants] ENABLE TRIGGER [AfterProjectInvestigatorAssistantDelete]
GO
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF
GO
CREATE SPATIAL INDEX [SIndex_Locations_Location] ON [dbo].[Locations]
(
	[Location]
)USING  GEOGRAPHY_GRID 
WITH (GRIDS =(LEVEL_1 = MEDIUM,LEVEL_2 = MEDIUM,LEVEL_3 = MEDIUM,LEVEL_4 = MEDIUM), 
CELLS_PER_OBJECT = 16, PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF
GO
CREATE SPATIAL INDEX [SIndex_Movements_Shape] ON [dbo].[Movements]
(
	[Shape]
)USING  GEOGRAPHY_GRID 
WITH (GRIDS =(LEVEL_1 = MEDIUM,LEVEL_2 = MEDIUM,LEVEL_3 = MEDIUM,LEVEL_4 = MEDIUM), 
CELLS_PER_OBJECT = 16, PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF
GO
CREATE SPATIAL INDEX [SIndex_VHFLocations_Location] ON [dbo].[VhfLocations]
(
	[Location]
)USING  GEOGRAPHY_GRID 
WITH (GRIDS =(LEVEL_1 = MEDIUM,LEVEL_2 = MEDIUM,LEVEL_3 = MEDIUM,LEVEL_4 = MEDIUM), 
CELLS_PER_OBJECT = 16, PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
