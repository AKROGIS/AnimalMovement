USE [Animal_Movement]
GO
CREATE USER [NPS\BAMangipane] FOR LOGIN [NPS\BAMangipane] WITH DEFAULT_SCHEMA=[dbo]
GO
CREATE USER [NPS\Domain Users] FOR LOGIN [NPS\Domain Users]
GO
CREATE USER [NPS\JWBurch] FOR LOGIN [NPS\JWBurch] WITH DEFAULT_SCHEMA=[dbo]
GO
CREATE USER [NPS\KCJoly] FOR LOGIN [NPS\KCJoly] WITH DEFAULT_SCHEMA=[dbo]
GO
CREATE USER [NPS\RESarwas] FOR LOGIN [NPS\RESarwas] WITH DEFAULT_SCHEMA=[dbo]
GO
CREATE USER [NPS\SDMiller] FOR LOGIN [NPS\SDMiller] WITH DEFAULT_SCHEMA=[dbo]
GO
CREATE ROLE [Editor] AUTHORIZATION [dbo]
GO
CREATE ROLE [Investigator] AUTHORIZATION [dbo]
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LookupCollarManufacturers](
	[CollarManufacturer] [varchar](16) NOT NULL,
	[Name] [nvarchar](200) NULL,
	[Website] [nvarchar](200) NULL,
	[Description] [nvarchar](2000) NULL,
 CONSTRAINT [PK_CollarManufacturers] PRIMARY KEY CLUSTERED 
(
	[CollarManufacturer] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LookupCollarFileFormats](
	[Code] [char](1) NOT NULL,
	[CollarManufacturer] [varchar](16) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[TableName] [sysname] NOT NULL,
	[HasCollarIdColumn] [char](1) NOT NULL,
 CONSTRAINT [PK_CollarFileFormats] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LookupCollarFileStatus](
	[Code] [char](1) NOT NULL,
	[Name] [nvarchar](32) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [PK_CollarFileStatus] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CollarFiles](
	[FileId] [int] IDENTITY(1,1) NOT NULL,
	[FileName] [nvarchar](255) NOT NULL,
	[UploadDate] [datetime2](7) NOT NULL,
	[Project] [varchar](16) NOT NULL,
	[UserName] [sysname] NOT NULL,
	[CollarManufacturer] [varchar](16) NOT NULL,
	[CollarId] [varchar](16) NULL,
	[Format] [char](1) NOT NULL,
	[Status] [char](1) NOT NULL,
	[Contents] [varbinary](max) NOT NULL,
 CONSTRAINT [PK_CollarFiles] PRIMARY KEY CLUSTERED 
(
	[FileId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LookupCollarModels](
	[CollarModel] [varchar](24) NOT NULL,
 CONSTRAINT [PK_LookupCollarModels] PRIMARY KEY CLUSTERED 
(
	[CollarModel] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Collars](
	[CollarManufacturer] [varchar](16) NOT NULL,
	[CollarId] [varchar](16) NOT NULL,
	[CollarModel] [varchar](24) NOT NULL,
	[Manager] [sysname] NOT NULL,
	[Owner] [nvarchar](100) NULL,
	[AlternativeId] [varchar](16) NULL,
	[SerialNumber] [varchar](100) NULL,
	[Frequency] [float] NULL,
	[DownloadInfo] [varchar](200) NULL,
	[Notes] [nvarchar](max) NULL,
 CONSTRAINT [PK_Collars] PRIMARY KEY CLUSTERED 
(
	[CollarManufacturer] ASC,
	[CollarId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CollarFixes](
	[FixId] [bigint] IDENTITY(1,1) NOT NULL,
	[HiddenBy] [bigint] NULL,
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE NONCLUSTERED INDEX [IX_CollarFixes_Collar] ON [dbo].[CollarFixes] 
(
	[CollarManufacturer] ASC,
	[CollarId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CollarFixes_FileId] ON [dbo].[CollarFixes] 
(
	[FileId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CollarFixes_FixDate] ON [dbo].[CollarFixes] 
(
	[FixDate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CollarFixes_HiddenBy] ON [dbo].[CollarFixes] 
(
	[HiddenBy] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
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
-- =============================================
CREATE FUNCTION [dbo].[CollarFixSummary] 
(
	@CollarManufacturer NVARCHAR(255), 
	@CollarId           NVARCHAR(255)
)
RETURNS TABLE 
AS
	RETURN
		SELECT  COUNT(*) AS [Count],
				MIN(FixDate) AS [First],
				MAX(FixDate) AS [Last]
		FROM [dbo].[CollarFixes]
		WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId
		GROUP BY CollarManufacturer, CollarId
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
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Locations](
	[ProjectId] [varchar](16) NOT NULL,
	[AnimalId] [varchar](16) NOT NULL,
	[FixDate] [datetime2](7) NOT NULL,
	[Location] [geography] NOT NULL,
	[FixId] [bigint] NOT NULL,
	[Status] [char](1) NULL,
 CONSTRAINT [PK_Locations] PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC,
	[AnimalId] ASC,
	[FixDate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Locations_FixId] ON [dbo].[Locations] 
(
	[FixId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE SPATIAL INDEX [SIndex_Locations_Location] ON [dbo].[Locations] 
(
	[Location]
)USING  GEOGRAPHY_GRID 
WITH (
GRIDS =(LEVEL_1 = MEDIUM,LEVEL_2 = MEDIUM,LEVEL_3 = MEDIUM,LEVEL_4 = MEDIUM), 
CELLS_PER_OBJECT = 16, PAD_INDEX  = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LookupGender](
	[Sex] [char](1) NOT NULL,
 CONSTRAINT [PK_LookupSex] PRIMARY KEY CLUSTERED 
(
	[Sex] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LookupSpecies](
	[Species] [varchar](32) NOT NULL,
 CONSTRAINT [PK_LookupSpecies] PRIMARY KEY CLUSTERED 
(
	[Species] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Animals](
	[ProjectId] [varchar](16) NOT NULL,
	[AnimalId] [varchar](16) NOT NULL,
	[Species] [varchar](32) NULL,
	[Gender] [char](1) NULL,
	[MortalityDate] [datetime2](7) NULL,
	[GroupName] [nvarchar](500) NULL,
	[Description] [nvarchar](2000) NULL,
 CONSTRAINT [PK_Animals] PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC,
	[AnimalId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CollarDeployments](
	[ProjectId] [varchar](16) NOT NULL,
	[AnimalId] [varchar](16) NOT NULL,
	[CollarManufacturer] [varchar](16) NOT NULL,
	[CollarId] [varchar](16) NOT NULL,
	[DeploymentDate] [datetime2](7) NOT NULL,
	[RetrievalDate] [datetime2](7) NULL,
 CONSTRAINT [PK_CollarDeployments] PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC,
	[AnimalId] ASC,
	[CollarManufacturer] ASC,
	[CollarId] ASC,
	[DeploymentDate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CurrentDeployments]
AS
SELECT     dbo.CollarDeployments.*
FROM         dbo.CollarDeployments
WHERE     (DeploymentDate < GETDATE()) AND (RetrievalDate > GETDATE() OR
                      RetrievalDate IS NULL)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Regan Sarwas
-- Create date: May 30, 2012
-- Description:	Returns a table of conflicting fixes for a specific collar.
-- Example:     SELECT * FROM ConflictingFixes('Telonics', '96007')
-- =============================================
CREATE FUNCTION [dbo].[ConflictingFixes] 
(
	@CollarManufacturer NVARCHAR(255), 
	@CollarId           NVARCHAR(255)
)
RETURNS TABLE 
AS
	RETURN
		SELECT FixId, HiddenBy, FileId, LineNumber, C.FixDate, Lat, Lon
		FROM CollarFixes AS C
		INNER JOIN 
			(SELECT   CollarManufacturer, CollarId, FixDate 
			 FROM     CollarFixes 
			 WHERE    CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId
			 GROUP BY CollarManufacturer, CollarId, FixDate
			 HAVING   COUNT(FixDate) > 1) AS D
		ON  C.CollarManufacturer = D.CollarManufacturer
		AND C.CollarId = D.CollarId
		AND C.FixDate = D.FixDate
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CollarsWithConflictingFixes]
AS
SELECT DISTINCT CollarManufacturer, CollarId
FROM         dbo.CollarFixes
GROUP BY CollarManufacturer, CollarId, FixDate
HAVING      (COUNT(FixDate) > 1)
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
				MIN(FixDate) AS [First], MAX(FixDate) as [Last]
		 FROM     [dbo].[Locations]
		 WHERE    ProjectId = @ProjectId AND AnimalId = @AnimalId
		 GROUP BY ProjectId, AnimalId
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Regan Sarwas
-- Create date: April 3, 2012
-- Description:	Add/Remove Locations when Deployment is updated
-- =============================================
CREATE TRIGGER [dbo].[AfterCollarDeploymentUpdate] 
   ON  [dbo].[CollarDeployments] 
   AFTER UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	
	--Retrevial date is the only thing that can change.
	IF UPDATE ([RetrievalDate])
	BEGIN
	
		-- triggers always execute in the context of a transaction
		-- so the following code is all or nothing.



  		-- When new retrevial date < old retrevial date we may lose some Locations
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
		   INNER JOIN inserted as I
				   ON I.ProjectId = D.ProjectId
				  AND I.AnimalId = D.AnimalId
				  AND I.CollarManufacturer = D.CollarManufacturer
				  AND I.CollarId = D.CollarId
				  AND I.DeploymentDate = D.DeploymentDate
				   -- These are the Locations to delete:
				WHERE I.RetrievalDate < L.FixDate 
				
				
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
				AND I.DeploymentDate < F.FixDate
				AND (F.FixDate < I.RetrievalDate OR I.RetrievalDate IS NULL)
	     INNER JOIN deleted as D
				 -- To get the old retrieval date, we need to link inserted and deleted deployments by PK
			     ON I.ProjectId = D.ProjectId
			    AND I.AnimalId = D.AnimalId
			    AND I.CollarManufacturer = D.CollarManufacturer
			    AND I.CollarId = D.CollarId
				AND I.DeploymentDate = D.DeploymentDate
--       INNER JOIN dbo.Animals AS A
-- 	             ON A.ProjectId = I.ProjectId
--		    	AND A.AnimalId = I.AnimalId
			  WHERE F.HiddenBy IS NULL
			    AND D.RetrievalDate < F.FixDate
			     -- NULL < F.FixDate is always false (good) 
--	    	    AND F.FixDate < A.MortalityDate OR A.MortalityDate IS NULL

		/*
		These stored procedures would be clean, but they would require a cursor,
		stored procedures cannot be used in a set based solution.
		EXEC [dbo].[AddLocationForDeployment] @CollarManufacturer, @CollarId, @ProjectId, @AnimalId, @StartDate, @EndDate
		EXEC [dbo].[DeleteLocationForAnimalAndDateRange] @ProjectId, @AnimalId, @StartDate, @EndDate
		*/
	END
END
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
	
	-- Add locations for fixes that are now deployed
	INSERT INTO Locations (ProjectId, AnimalId, FixDate, Location, FixId)
		 SELECT I.ProjectId, I.AnimalId, F.FixDate, geography::Point(F.Lat, F.Lon, 4326), F.FixId
		   FROM dbo.CollarFixes AS F
     INNER JOIN inserted AS I
		     ON F.CollarManufacturer = I.CollarManufacturer
		    AND F.CollarId = I.CollarId
			AND F.HiddenBy IS NULL
			AND F.FixDate > I.DeploymentDate
			AND (I.RetrievalDate IS NULL OR F.FixDate < I.RetrievalDate)
--   INNER JOIN dbo.Animals AS A
--			 ON A.ProjectId = I.ProjectId
--			AND A.AnimalId = I.AnimalId
--		  WHERE F.FixDate < A.MortalityDate OR A.MortalityDate IS NULL
END
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
			  AND L.FixDate > D.DeploymentDate
			  AND (D.RetrievalDate IS NULL OR L.FixDate < D.RetrievalDate)
END
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[InvalidLocations]
AS
SELECT     ProjectId, AnimalId, FixDate, Status, Location
FROM         dbo.Locations
WHERE     (Status IS NOT NULL)
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
		@HiddenFixId		bigint,
		@FixId				bigint,
		@HiddenBy			bigint,
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
		INSERT INTO Locations (ProjectId, AnimalId, FixDate, Location, FixId)
			 SELECT C.ProjectId, C.AnimalId, @FixDate, geography::Point(@Lat, @Lon, 4326) AS Location, @FixId
			   FROM CollarDeployments as C 
--       INNER JOIN dbo.Animals AS A
-- 	             ON A.ProjectId = C.ProjectId
--		    	AND A.AnimalId = C.AnimalId
			  WHERE C.CollarManufacturer = @CollarManufacturer AND C.CollarId = @CollarId
				AND C.DeploymentDate <= @FixDate AND (C.RetrievalDate IS NULL OR @FixDate <= C.RetrievalDate)
--	    	    AND @FixDate < A.MortalityDate OR A.MortalityDate IS NULL

		FETCH NEXT FROM insf_cursor INTO @HiddenBy, @FileId, @LineNumber, @CollarManufacturer, @CollarId, @FixDate, @Lat, @Lon;
	END
	CLOSE insf_cursor;
	DEALLOCATE insf_cursor;
END
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
		@FixId    bigint,
		@HiddenBy bigint;
	  
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
		UPDATE CollarFixes SET HiddenBy = @HiddenBy WHERE HiddenBy = @FixId;

		FETCH NEXT FROM delf_cursor INTO @FixId;
	END
	CLOSE delf_cursor;
	DEALLOCATE delf_cursor;
	
END
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
CREATE VIEW [dbo].[FixesByLocation]
AS
SELECT     dbo.CollarFixes.FixDate
FROM         dbo.CollarFixes INNER JOIN
                      dbo.Locations ON dbo.CollarFixes.FixDate = dbo.Locations.FixDate INNER JOIN
                      dbo.CollarDeployments ON dbo.CollarFixes.CollarManufacturer = dbo.CollarDeployments.CollarManufacturer AND 
                      dbo.Locations.ProjectId = dbo.CollarDeployments.ProjectId AND dbo.Locations.AnimalId = dbo.CollarDeployments.AnimalId AND 
                      dbo.CollarFixes.CollarId = dbo.CollarDeployments.CollarId AND dbo.CollarFixes.FixDate > dbo.CollarDeployments.DeploymentDate AND 
                      (dbo.CollarFixes.FixDate < dbo.CollarDeployments.RetrievalDate OR
                      dbo.CollarDeployments.RetrievalDate IS NULL)
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Movements_EndDate] ON [dbo].[Movements] 
(
	[ProjectId] ASC,
	[AnimalId] ASC,
	[EndDate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Movements_StartDate_EndDate] ON [dbo].[Movements] 
(
	[StartDate] ASC,
	[EndDate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE SPATIAL INDEX [SIndex_Movements_Shape] ON [dbo].[Movements] 
(
	[Shape]
)USING  GEOGRAPHY_GRID 
WITH (
GRIDS =(LEVEL_1 = MEDIUM,LEVEL_2 = MEDIUM,LEVEL_3 = MEDIUM,LEVEL_4 = MEDIUM), 
CELLS_PER_OBJECT = 16, PAD_INDEX  = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[NoMovement]
AS
SELECT     ProjectId, AnimalId, StartDate, EndDate, Duration, Distance, Speed, Shape
FROM         dbo.Movements
WHERE     (Distance = 0)
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
		   --INNER JOIN CollarDeployments AS C 
				   --ON C.ProjectId = L.ProjectId AND C.AnimalId = L.AnimalId
		   INNER JOIN deleted as D
				 --ON C.CollarManufacturer = D.CollarManufacturer AND C.CollarId = D.CollarId
				   ON L.FixId = D.FixId
		   INNER JOIN inserted as I
				   ON L.FixId = I.FixId
				WHERE D.HiddenBy IS NULL AND I.HiddenBy IS NOT NULL
				  AND L.FixDate = D.FixDate
		
		-- if HiddenBy goes from NOT NULL to NULL then
		-- Add new record to Locations table
		INSERT INTO Locations (ProjectId, AnimalId, FixDate, Location, FixId)
			 SELECT C.ProjectId, C.AnimalId, I.FixDate, geography::Point(I.Lat, I.Lon, 4326) AS Location, I.FixId
			   FROM inserted as I INNER JOIN CollarDeployments as C
				 ON C.CollarManufacturer = I.CollarManufacturer AND C.CollarId = I.CollarId
		 INNER JOIN deleted as D
				 ON D.FixId = I.FixId
--       INNER JOIN dbo.Animals AS A
-- 	             ON A.ProjectId = C.ProjectId
--		    	AND A.AnimalId = C.AnimalId
			  WHERE D.HiddenBy IS NOT NULL AND I.HiddenBy IS NULL
			    AND C.DeploymentDate <= I.FixDate AND (C.RetrievalDate IS NULL OR I.FixDate <= C.RetrievalDate)
--	    	    AND I.FixDate < A.MortalityDate OR A.MortalityDate IS NULL

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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[VelocityVectors]
AS
SELECT     ProjectId, AnimalId, StartDate, EndDate, Duration, Distance, Speed, Shape
FROM         dbo.Movements
WHERE     (Distance <> 0)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ValidLocations]
AS
SELECT     ProjectId, AnimalId, FixDate, Status, Location
FROM         dbo.Locations
WHERE     (Status IS NULL)
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
CREATE TABLE [dbo].[Settings](
	[Username] [sysname] NOT NULL,
	[Key] [nvarchar](30) NOT NULL,
	[Value] [nvarchar](500) NULL,
 CONSTRAINT [PK_Settings] PRIMARY KEY CLUSTERED 
(
	[Username] ASC,
	[Key] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LookupQueryLayerServers](
	[Location] [nvarchar](128) NOT NULL,
	[Connection] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_QueryLayerServers] PRIMARY KEY CLUSTERED 
(
	[Location] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
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
	
	-- There are nine cases: (NULL2DATE, DATE2DATE, DATE2NULL)^2
	
	IF @StartDate1 IS NULL and @EndDate1 IS NOT NULL
	BEGIN
		IF @StartDate2 IS NULL and @EndDate2 IS NOT NULL
		BEGIN
			RETURN 1
		END
		IF @StartDate2 IS NOT NULL and @EndDate2 IS NOT NULL
		BEGIN
			IF @EndDate1 < @StartDate2 RETURN 0 ELSE RETURN 1
		END	
		IF @StartDate2 IS NOT NULL and @EndDate2 IS NULL
		BEGIN
			IF @EndDate1 < @StartDate2 RETURN 0 ELSE RETURN 1
		END	
	END
	
	IF @StartDate1 IS NOT NULL and @EndDate1 IS NOT NULL
	BEGIN
		IF @StartDate2 IS NULL and @EndDate2 IS NOT NULL
		BEGIN
			IF @EndDate2 < @StartDate1 RETURN 0 ELSE RETURN 1
		END
		IF @StartDate2 IS NOT NULL and @EndDate2 IS NOT NULL
		BEGIN
			IF @EndDate2 < @StartDate1 OR @EndDate1 < @StartDate2  RETURN 0 ELSE RETURN 1
		END	
		IF @StartDate2 IS NOT NULL and @EndDate2 IS NULL
		BEGIN
			IF @EndDate1 < @StartDate2 RETURN 0 ELSE RETURN 1
		END	
	END	

	IF @StartDate1 IS NOT NULL and @EndDate1 IS NULL
	BEGIN
		IF @StartDate2 IS NULL and @EndDate2 IS NOT NULL
		BEGIN
			IF @EndDate2 < @StartDate1 RETURN 0 ELSE RETURN 1
		END
		IF @StartDate2 IS NOT NULL and @EndDate2 IS NOT NULL
		BEGIN
			IF @EndDate2 < @StartDate1 RETURN 0 ELSE RETURN 1
		END	
		IF @StartDate2 IS NOT NULL and @EndDate2 IS NULL
		BEGIN
			RETURN 1
		END	
	END
	-- We should never get here
	RETURN 1
END
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
CREATE FUNCTION [dbo].[LocalTime](@utcDateTime [datetime])
RETURNS [datetime] WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [SqlServerExtensions].[SqlServerExtensions.AnimalMovementFunctions].[LocalTime]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CollarsNotCurrentlyDeployed]
AS
SELECT     dbo.Collars.CollarId, dbo.Collars.CollarModel, dbo.Collars.Frequency, dbo.Collars.DownloadInfo, dbo.LookupCollarManufacturers.CollarManufacturer, 
                      dbo.LookupCollarManufacturers.Name, dbo.LookupCollarManufacturers.Website, dbo.LookupCollarManufacturers.Description, dbo.Collars.Notes, 
                      dbo.Collars.SerialNumber, dbo.Collars.AlternativeId, dbo.Collars.Owner, dbo.Collars.Manager
FROM         dbo.Collars INNER JOIN
                      dbo.CollarDeployments ON dbo.Collars.CollarManufacturer = dbo.CollarDeployments.CollarManufacturer AND 
                      dbo.Collars.CollarId = dbo.CollarDeployments.CollarId INNER JOIN
                      dbo.LookupCollarManufacturers ON dbo.Collars.CollarManufacturer = dbo.LookupCollarManufacturers.CollarManufacturer LEFT OUTER JOIN
                      dbo.CurrentDeployments ON dbo.CollarDeployments.ProjectId = dbo.CurrentDeployments.ProjectId AND 
                      dbo.CollarDeployments.AnimalId = dbo.CurrentDeployments.AnimalId AND 
                      dbo.CollarDeployments.CollarManufacturer = dbo.CurrentDeployments.CollarManufacturer AND 
                      dbo.CollarDeployments.CollarId = dbo.CurrentDeployments.CollarId
WHERE     (dbo.CurrentDeployments.ProjectId IS NULL)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CollarsNeverDeployed]
AS
SELECT     dbo.Collars.CollarModel, dbo.Collars.Frequency, dbo.Collars.DownloadInfo, dbo.LookupCollarManufacturers.Name, dbo.LookupCollarManufacturers.Website, 
                      dbo.LookupCollarManufacturers.Description, dbo.Collars.Manager, dbo.Collars.Owner, dbo.Collars.AlternativeId, dbo.Collars.SerialNumber, dbo.Collars.Notes, 
                      dbo.CollarDeployments.ProjectId, dbo.CollarDeployments.AnimalId, dbo.CollarDeployments.DeploymentDate, dbo.CollarDeployments.RetrievalDate, 
                      dbo.Collars.CollarManufacturer, dbo.Collars.CollarId
FROM         dbo.Collars INNER JOIN
                      dbo.LookupCollarManufacturers ON dbo.Collars.CollarManufacturer = dbo.LookupCollarManufacturers.CollarManufacturer LEFT OUTER JOIN
                      dbo.CollarDeployments ON dbo.Collars.CollarManufacturer = dbo.CollarDeployments.CollarManufacturer AND 
                      dbo.Collars.CollarId = dbo.CollarDeployments.CollarId
WHERE     (dbo.CollarDeployments.CollarId IS NULL)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CollarsCurrentlyDeployed]
AS
SELECT     dbo.CollarDeployments.ProjectId, dbo.CollarDeployments.AnimalId, dbo.CollarDeployments.CollarManufacturer AS Expr1, dbo.CollarDeployments.CollarId AS Expr2, 
                      dbo.CollarDeployments.DeploymentDate, dbo.CollarDeployments.RetrievalDate, dbo.LookupCollarManufacturers.Name, dbo.LookupCollarManufacturers.Website, 
                      dbo.LookupCollarManufacturers.Description, dbo.Collars.CollarModel, dbo.Collars.Frequency, dbo.Collars.DownloadInfo, dbo.Collars.Manager, dbo.Collars.Owner, 
                      dbo.Collars.AlternativeId, dbo.Collars.SerialNumber, dbo.Collars.Notes
FROM         dbo.CollarDeployments INNER JOIN
                      dbo.Collars ON dbo.CollarDeployments.CollarManufacturer = dbo.Collars.CollarManufacturer AND dbo.CollarDeployments.CollarId = dbo.Collars.CollarId INNER JOIN
                      dbo.LookupCollarManufacturers ON dbo.Collars.CollarManufacturer = dbo.LookupCollarManufacturers.CollarManufacturer
WHERE     (dbo.CollarDeployments.DeploymentDate < GETDATE()) AND (dbo.CollarDeployments.RetrievalDate > GETDATE() OR
                      dbo.CollarDeployments.RetrievalDate IS NULL)
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[AnimalsNotCurrentlyCollared]
AS
SELECT     dbo.Animals.AnimalId, dbo.Animals.Species, dbo.Animals.Gender, dbo.Animals.GroupName, dbo.Projects.ProjectId, dbo.Projects.ProjectName, 
                      dbo.Projects.ProjectInvestigator AS UnitCode, dbo.Projects.UnitCode AS PrincipalInvestigator, dbo.Projects.Description
FROM         dbo.Animals INNER JOIN
                      dbo.CollarDeployments ON dbo.Animals.ProjectId = dbo.CollarDeployments.ProjectId AND dbo.Animals.AnimalId = dbo.CollarDeployments.AnimalId INNER JOIN
                      dbo.Projects ON dbo.Animals.ProjectId = dbo.Projects.ProjectId LEFT OUTER JOIN
                      dbo.CurrentDeployments ON dbo.CollarDeployments.ProjectId = dbo.CurrentDeployments.ProjectId AND 
                      dbo.CollarDeployments.AnimalId = dbo.CurrentDeployments.AnimalId AND 
                      dbo.CollarDeployments.CollarManufacturer = dbo.CurrentDeployments.CollarManufacturer AND 
                      dbo.CollarDeployments.CollarId = dbo.CurrentDeployments.CollarId
WHERE     (dbo.CurrentDeployments.ProjectId IS NULL)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[AnimalsNeverCollared]
AS
SELECT     dbo.Animals.AnimalId, dbo.Animals.Species, dbo.Animals.Gender, dbo.Animals.GroupName, dbo.Projects.ProjectId, dbo.Projects.ProjectName, 
                      dbo.Projects.ProjectInvestigator AS UnitCode, dbo.Projects.UnitCode AS PrincipalInvestigator, dbo.Projects.Description
FROM         dbo.Projects INNER JOIN
                      dbo.Animals ON dbo.Projects.ProjectId = dbo.Animals.ProjectId LEFT OUTER JOIN
                      dbo.CollarDeployments ON dbo.Animals.ProjectId = dbo.CollarDeployments.ProjectId AND dbo.Animals.AnimalId = dbo.CollarDeployments.AnimalId
WHERE     (dbo.CollarDeployments.ProjectId IS NULL)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[AnimalsCurrentlyCollared]
AS
SELECT     dbo.Animals.AnimalId, dbo.Animals.Species, dbo.Animals.Gender, dbo.Animals.GroupName, dbo.Projects.ProjectId, dbo.Projects.ProjectName, 
                      dbo.Projects.ProjectInvestigator AS UnitCode, dbo.Projects.UnitCode AS PrincipalInvestigator, dbo.Projects.Description
FROM         dbo.CollarDeployments INNER JOIN
                      dbo.Animals ON dbo.CollarDeployments.ProjectId = dbo.Animals.ProjectId AND dbo.CollarDeployments.AnimalId = dbo.Animals.AnimalId INNER JOIN
                      dbo.Projects ON dbo.Animals.ProjectId = dbo.Projects.ProjectId
WHERE     (dbo.CollarDeployments.DeploymentDate < GETDATE()) AND (dbo.CollarDeployments.RetrievalDate > GETDATE() OR
                      dbo.CollarDeployments.RetrievalDate IS NULL)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[AnimalFixesByFile]
AS
SELECT     dbo.CollarFixes.FileId, dbo.LookupCollarManufacturers.Name AS Manufacturer, dbo.CollarFixes.CollarId, dbo.Projects.ProjectName AS Project, 
                      dbo.CollarDeployments.AnimalId, MIN(dbo.CollarFixes.FixDate) AS [First Fix], MAX(dbo.CollarFixes.FixDate) AS [Last Fix], COUNT(dbo.CollarFixes.FixDate) 
                      AS [Number of Fixes]
FROM         dbo.CollarFixes INNER JOIN
                      dbo.CollarDeployments ON dbo.CollarFixes.CollarManufacturer = dbo.CollarDeployments.CollarManufacturer AND 
                      dbo.CollarFixes.CollarId = dbo.CollarDeployments.CollarId AND dbo.CollarFixes.FixDate > dbo.CollarDeployments.DeploymentDate AND 
                      (dbo.CollarFixes.FixDate < dbo.CollarDeployments.RetrievalDate OR
                      dbo.CollarDeployments.RetrievalDate IS NULL) INNER JOIN
                      dbo.LookupCollarManufacturers ON dbo.CollarFixes.CollarManufacturer = dbo.LookupCollarManufacturers.CollarManufacturer LEFT OUTER JOIN
                      dbo.Projects ON dbo.CollarDeployments.ProjectId = dbo.Projects.ProjectId
GROUP BY dbo.Projects.ProjectName, dbo.CollarDeployments.AnimalId, dbo.CollarFixes.FileId, dbo.LookupCollarManufacturers.Name, dbo.CollarFixes.CollarId
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Tests_DoDateRangesOverlap] 

AS
BEGIN
	DECLARE @StartDate1 DATETIME2
	DECLARE @StartDate2 DATETIME2
	DECLARE @EndDate1 DATETIME2
	DECLARE @EndDate2 DATETIME2
	DECLARE @OVERLAP NVARCHAR(32)
	DECLARE @RESULT NVARCHAR(32)
	
	-- NULL to DATE
	
	SET @StartDate1 = NULL
	SET @EndDate1 = '2000-01-01'
	SET @StartDate2 = NULL
	SET @EndDate2 = '2001-01-01'
	IF dbo.DoDateRangesOverlap(@StartDate1,@EndDate1,@StartDate2,@EndDate2) = 1
		SET @OVERLAP = 'Overlaps' ELSE SET @OVERLAP = 'Is disjointed'
	IF @OVERLAP = 'Overlaps'
		SET @RESULT = 'Pass' ELSE SET @RESULT = 'Fail'
	print '1  (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate2)),'NULL') + ') '+ @OVERLAP + '.  ' + @RESULT
	
	
	
	SET @StartDate1 = NULL
	SET @EndDate1 = '2000-01-01'
	SET @StartDate2 = '2001-01-01'
	SET @EndDate2 = '2002-01-01'
	IF dbo.DoDateRangesOverlap(@StartDate1,@EndDate1,@StartDate2,@EndDate2) = 1
		SET @OVERLAP = 'Overlaps' ELSE SET @OVERLAP = 'Is disjointed'
	IF @OVERLAP <> 'Overlaps'
		SET @RESULT = 'Pass' ELSE SET @RESULT = 'Fail'
	print '2a (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate2)),'NULL') + ') '+ @OVERLAP + '.  ' + @RESULT
	
	SET @StartDate1 = NULL
	SET @EndDate1 = '2001-01-01'
	SET @StartDate2 = '2000-01-01'
	SET @EndDate2 = '2002-01-01'
	IF dbo.DoDateRangesOverlap(@StartDate1,@EndDate1,@StartDate2,@EndDate2) = 1
		SET @OVERLAP = 'Overlaps' ELSE SET @OVERLAP = 'Is disjointed'
	IF @OVERLAP = 'Overlaps'
		SET @RESULT = 'Pass' ELSE SET @RESULT = 'Fail'
	print '2b (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate2)),'NULL') + ') '+ @OVERLAP + '.  ' + @RESULT
	
	
	
	SET @StartDate1 = NULL
	SET @EndDate1 = '2000-01-01'
	SET @StartDate2 = '2001-01-01'
	SET @EndDate2 = NULL
	IF dbo.DoDateRangesOverlap(@StartDate1,@EndDate1,@StartDate2,@EndDate2) = 1
		SET @OVERLAP = 'Overlaps' ELSE SET @OVERLAP = 'Is disjointed'
	IF @OVERLAP <> 'Overlaps'
		SET @RESULT = 'Pass' ELSE SET @RESULT = 'Fail'
	print '3a (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate2)),'NULL') + ') '+ @OVERLAP + '.  ' + @RESULT
	
	SET @StartDate1 = NULL
	SET @EndDate1 = '2001-01-01'
	SET @StartDate2 = '2000-01-01'
	SET @EndDate2 = NULL
	IF dbo.DoDateRangesOverlap(@StartDate1,@EndDate1,@StartDate2,@EndDate2) = 1
		SET @OVERLAP = 'Overlaps' ELSE SET @OVERLAP = 'Is disjointed'
	IF @OVERLAP = 'Overlaps'
		SET @RESULT = 'Pass' ELSE SET @RESULT = 'Fail'
	print '3b (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate2)),'NULL') + ') '+ @OVERLAP + '.  ' + @RESULT
	
	
	-- DATE to DATE
	
	SET @StartDate1 = '2000-01-01'
	SET @EndDate1 =  '2002-01-01'
	SET @StartDate2 = NULL
	SET @EndDate2 = '2001-01-01'
	IF dbo.DoDateRangesOverlap(@StartDate1,@EndDate1,@StartDate2,@EndDate2) = 1
		SET @OVERLAP = 'Overlaps' ELSE SET @OVERLAP = 'Is disjointed'
	IF @OVERLAP = 'Overlaps'
		SET @RESULT = 'Pass' ELSE SET @RESULT = 'Fail'
	print '4a (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate2)),'NULL') + ') '+ @OVERLAP + '.  ' + @RESULT
	
	SET @StartDate1 = '2001-01-01'
	SET @EndDate1 =  '2002-01-01'
	SET @StartDate2 = NULL
	SET @EndDate2 = '2000-01-01'
	IF dbo.DoDateRangesOverlap(@StartDate1,@EndDate1,@StartDate2,@EndDate2) = 1
		SET @OVERLAP = 'Overlaps' ELSE SET @OVERLAP = 'Is disjointed'
	IF @OVERLAP <> 'Overlaps'
		SET @RESULT = 'Pass' ELSE SET @RESULT = 'Fail'
	print '4b (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate2)),'NULL') + ') '+ @OVERLAP + '.  ' + @RESULT
	
	
	
	SET @StartDate1 = '2001-01-01'
	SET @EndDate1 =  '2003-01-01'
	SET @StartDate2 = '2000-01-01'
	SET @EndDate2 = '2002-01-01'
	IF dbo.DoDateRangesOverlap(@StartDate1,@EndDate1,@StartDate2,@EndDate2) = 1
		SET @OVERLAP = 'Overlaps' ELSE SET @OVERLAP = 'Is disjointed'
	IF @OVERLAP = 'Overlaps'
		SET @RESULT = 'Pass' ELSE SET @RESULT = 'Fail'
	print '5a (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate2)),'NULL') + ') '+ @OVERLAP + '.  ' + @RESULT
	
	SET @StartDate1 = '2002-01-01'
	SET @EndDate1 =  '2003-01-01'
	SET @StartDate2 = '2000-01-01'
	SET @EndDate2 = '2001-01-01'
	IF dbo.DoDateRangesOverlap(@StartDate1,@EndDate1,@StartDate2,@EndDate2) = 1
		SET @OVERLAP = 'Overlaps' ELSE SET @OVERLAP = 'Is disjointed'
	IF @OVERLAP <> 'Overlaps'
		SET @RESULT = 'Pass' ELSE SET @RESULT = 'Fail'
	print '5b (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate2)),'NULL') + ') '+ @OVERLAP + '.  ' + @RESULT
	
	SET @StartDate1 = '2000-01-01'
	SET @EndDate1 =  '2004-01-01'
	SET @StartDate2 = '2001-01-01'
	SET @EndDate2 = '2003-01-01'
	IF dbo.DoDateRangesOverlap(@StartDate1,@EndDate1,@StartDate2,@EndDate2) = 1
		SET @OVERLAP = 'Overlaps' ELSE SET @OVERLAP = 'Is disjointed'
	IF @OVERLAP = 'Overlaps'
		SET @RESULT = 'Pass' ELSE SET @RESULT = 'Fail'
	print '5c (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate2)),'NULL') + ') '+ @OVERLAP + '.  ' + @RESULT
	
	SET @StartDate1 = '2000-01-01'
	SET @EndDate1 =  '2002-01-01'
	SET @StartDate2 = '2001-01-01'
	SET @EndDate2 = '2003-01-01'
	IF dbo.DoDateRangesOverlap(@StartDate1,@EndDate1,@StartDate2,@EndDate2) = 1
		SET @OVERLAP = 'Overlaps' ELSE SET @OVERLAP = 'Is disjointed'
	IF @OVERLAP = 'Overlaps'
		SET @RESULT = 'Pass' ELSE SET @RESULT = 'Fail'
	print '5d (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate2)),'NULL') + ') '+ @OVERLAP + '.  ' + @RESULT
	
	SET @StartDate1 = '2000-01-01'
	SET @EndDate1 =  '2001-01-01'
	SET @StartDate2 = '2002-01-01'
	SET @EndDate2 = '2003-01-01'
	IF dbo.DoDateRangesOverlap(@StartDate1,@EndDate1,@StartDate2,@EndDate2) = 1
		SET @OVERLAP = 'Overlaps' ELSE SET @OVERLAP = 'Is disjointed'
	IF @OVERLAP <> 'Overlaps'
		SET @RESULT = 'Pass' ELSE SET @RESULT = 'Fail'
	print '5e (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate2)),'NULL') + ') '+ @OVERLAP + '.  ' + @RESULT
	
	
	
	SET @StartDate1 = '2000-01-01'
	SET @EndDate1 =  '2001-01-01'
	SET @StartDate2 = '2002-01-01'
	SET @EndDate2 = NULL
	IF dbo.DoDateRangesOverlap(@StartDate1,@EndDate1,@StartDate2,@EndDate2) = 1
		SET @OVERLAP = 'Overlaps' ELSE SET @OVERLAP = 'Is disjointed'
	IF @OVERLAP <> 'Overlaps'
		SET @RESULT = 'Pass' ELSE SET @RESULT = 'Fail'
	print '6a (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate2)),'NULL') + ') '+ @OVERLAP + '.  ' + @RESULT

	SET @StartDate1 = '2000-01-01'
	SET @EndDate1 =  '2002-01-01'
	SET @StartDate2 = '2001-01-01'
	SET @EndDate2 = NULL
	IF dbo.DoDateRangesOverlap(@StartDate1,@EndDate1,@StartDate2,@EndDate2) = 1
		SET @OVERLAP = 'Overlaps' ELSE SET @OVERLAP = 'Is disjointed'
	IF @OVERLAP = 'Overlaps'
		SET @RESULT = 'Pass' ELSE SET @RESULT = 'Fail'
	print '6b (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate2)),'NULL') + ') '+ @OVERLAP + '.  ' + @RESULT


	-- NULL to DATE

	SET @StartDate1 = '2002-01-01'
	SET @EndDate1 =  NULL
	SET @StartDate2 = NULL
	SET @EndDate2 = '2001-01-01'
	IF dbo.DoDateRangesOverlap(@StartDate1,@EndDate1,@StartDate2,@EndDate2) = 1
		SET @OVERLAP = 'Overlaps' ELSE SET @OVERLAP = 'Is disjointed'
	IF @OVERLAP <> 'Overlaps'
		SET @RESULT = 'Pass' ELSE SET @RESULT = 'Fail'
	print '7a (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate2)),'NULL') + ') '+ @OVERLAP + '.  ' + @RESULT
	
	SET @StartDate1 = '2000-01-01'
	SET @EndDate1 =  NULL
	SET @StartDate2 = NULL
	SET @EndDate2 = '2002-01-01'
	IF dbo.DoDateRangesOverlap(@StartDate1,@EndDate1,@StartDate2,@EndDate2) = 1
		SET @OVERLAP = 'Overlaps' ELSE SET @OVERLAP = 'Is disjointed'
	IF @OVERLAP = 'Overlaps'
		SET @RESULT = 'Pass' ELSE SET @RESULT = 'Fail'
	print '7b (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate2)),'NULL') + ') '+ @OVERLAP + '.  ' + @RESULT
	
	
	
	SET @StartDate1 = '2001-01-01'
	SET @EndDate1 =  NULL
	SET @StartDate2 = '2000-01-01'
	SET @EndDate2 = '2002-01-01'
	IF dbo.DoDateRangesOverlap(@StartDate1,@EndDate1,@StartDate2,@EndDate2) = 1
		SET @OVERLAP = 'Overlaps' ELSE SET @OVERLAP = 'Is disjointed'
	IF @OVERLAP = 'Overlaps'
		SET @RESULT = 'Pass' ELSE SET @RESULT = 'Fail'
	print '8a (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate2)),'NULL') + ') '+ @OVERLAP + '.  ' + @RESULT
	
	SET @StartDate1 = '2002-01-01'
	SET @EndDate1 =  NULL
	SET @StartDate2 = '2000-01-01'
	SET @EndDate2 = '2001-01-01'
	IF dbo.DoDateRangesOverlap(@StartDate1,@EndDate1,@StartDate2,@EndDate2) = 1
		SET @OVERLAP = 'Overlaps' ELSE SET @OVERLAP = 'Is disjointed'
	IF @OVERLAP <> 'Overlaps'
		SET @RESULT = 'Pass' ELSE SET @RESULT = 'Fail'
	print '8b (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate2)),'NULL') + ') '+ @OVERLAP + '.  ' + @RESULT
	
	
	
	SET @StartDate1 = '2001-01-01'
	SET @EndDate1 =  NULL
	SET @StartDate2 = '2000-01-01'
	SET @EndDate2 = NULL
	IF dbo.DoDateRangesOverlap(@StartDate1,@EndDate1,@StartDate2,@EndDate2) = 1
		SET @OVERLAP = 'Overlaps' ELSE SET @OVERLAP = 'Is disjointed'
	IF @OVERLAP = 'Overlaps'
		SET @RESULT = 'Pass' ELSE SET @RESULT = 'Fail'
	print '9  (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@EndDate2)),'NULL') + ') '+ @OVERLAP + '.  ' + @RESULT
	
END
GO
CREATE FUNCTION [dbo].[ParseFormatC](@fileId [int])
RETURNS  TABLE (
	[LineNumber] [int] NULL,
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
	[Error] [nvarchar](250) NULL
) WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [SqlServerExtensions].[SqlServerExtensions.AnimalMovementFunctions].[ParseFormatC]
GO
CREATE FUNCTION [dbo].[ParseFormatB](@fileId [int])
RETURNS  TABLE (
	[LineNumber] [int] NULL,
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
	[Other] [nvarchar](255) NULL
) WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [SqlServerExtensions].[SqlServerExtensions.AnimalMovementFunctions].[ParseFormatB]
GO
CREATE FUNCTION [dbo].[ParseFormatA](@fileId [int])
RETURNS  TABLE (
	[LineNumber] [int] NULL,
	[Fix #] [nvarchar](50) NULL,
	[Date] [nchar](10) NULL,
	[Time] [nchar](8) NULL,
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
EXTERNAL NAME [SqlServerExtensions].[SqlServerExtensions.AnimalMovementFunctions].[ParseFormatA]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Regan Sarwas
-- Create date: April 11, 2012
-- Description:	Updates a project investigator
-- =============================================
CREATE PROCEDURE [dbo].[ProjectInvestigator_Update] 
	@Login sysname       = NULL,
	@Name  NVARCHAR(255) = NULL, 
	@Email NVARCHAR(255) = NULL, 
	@Phone NVARCHAR(255) = NULL 
AS
BEGIN
	SET NOCOUNT ON;
	
	IF @Login IS NULL
		SET @Login = ORIGINAL_LOGIN();

	-- Verify this is an existing project investigator
	IF NOT EXISTS (SELECT 1 FROM [dbo].[ProjectInvestigators] WHERE [Login] = @Login)
	BEGIN
		DECLARE @message1 nvarchar(100) = 'There is no project investigator with a login of ' + @Login + '.';
		RAISERROR(@message1, 18, 0)
		RETURN 1
	END
	
	-- You must be the ProjectInvestigator to update the ProjectInvestigator
	IF @Login <> ORIGINAL_LOGIN()
	BEGIN
		DECLARE @message2 nvarchar(100) = 'You can only update your own record.';
		RAISERROR(@message2, 18, 0)
		RETURN 1
	END
	
	-- If a parameter is not provided, use the existing value.
	-- (to put null in a field the user will need to pass an empty string)
	IF @Name IS NULL
	BEGIN
		SELECT @Name = [Name] FROM [dbo].[ProjectInvestigators] WHERE [Login] = @Login;
	END
	
	IF @Email IS NULL
	BEGIN
		SELECT @Email = [Email] FROM [dbo].[ProjectInvestigators] WHERE [Login] = @Login;
	END
	
	IF @Phone IS NULL
	BEGIN
		SELECT @Phone = [Phone] FROM [dbo].[ProjectInvestigators] WHERE [Login] = @Login;
	END
	
	-- Do the update, replacing empty strings with NULLs
	UPDATE [dbo].[ProjectInvestigators] SET [Name]	= nullif(@Name,''),
											[Email] = nullif(@Email,''),
											[Phone] = nullif(@Phone,'')
									  WHERE [Login] = @Login

END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[ProjectInvestigator_Insert_SA]
	@Login sysname, 
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
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProjectEditors](
	[ProjectId] [varchar](16) NOT NULL,
	[Editor] [sysname] NOT NULL,
 CONSTRAINT [PK_ProjectEditors] PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC,
	[Editor] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
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
	-- The caller must be the PI or editor on the project
	IF NOT EXISTS (SELECT 1 FROM dbo.Projects WHERE ProjectId = @ProjectId AND ProjectInvestigator = @Caller)
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM dbo.ProjectEditors WHERE ProjectId = @ProjectId AND Editor = @Caller)
		BEGIN
			DECLARE @message1 nvarchar(200) = 'You ('+@Caller+') must be the principal investigator or editor on this project ('+@ProjectId+') to update this location.';
			RAISERROR(@message1, 18, 0)
			RETURN (1)
		END
	END
	
	UPDATE [Locations]
	   SET [Status] = @Status
	 WHERE [ProjectId] = @ProjectId
	   AND [AnimalId] = @AnimalId
	   AND [FixDate] = @FixDate;
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Regan Sarwas
-- Create date: April 2, 2012
-- Description:	Adds a new Collar Deployment.
-- =============================================
CREATE PROCEDURE [dbo].[CollarDeployment_UpdateRetrievalDate] 
	@ProjectId			NVARCHAR(255) = NULL,
	@AnimalId			NVARCHAR(255) = NULL, 
	@CollarManufacturer NVARCHAR(255) = NULL,
	@CollarId			NVARCHAR(255) = NULL, 
	@DeploymentDate		DATETIME2 = NULL, 
	@RetrievalDate		DATETIME2 = NULL
AS
BEGIN
	SET NOCOUNT ON;

	-- Get the name of the caller
	DECLARE @Caller sysname = ORIGINAL_LOGIN();

	-- Validate permission for this operation
	-- The caller must be the PI or editor on the project
	IF NOT EXISTS (SELECT 1 FROM dbo.Projects WHERE ProjectId = @ProjectId AND ProjectInvestigator = @Caller)
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM dbo.ProjectEditors WHERE ProjectId = @ProjectId AND Editor = @Caller)
		BEGIN
			DECLARE @message1 nvarchar(200) = 'You ('+@Caller+') must be the principal investigator or editor on this project ('+@ProjectId+') to deploy a collar on the project.';
			RAISERROR(@message1, 18, 0)
			RETURN (1)
		END
	END

	-- Verify that the deployment exists (this is done now to avoid a confusing silent No-op)
	IF NOT EXISTS (SELECT 1 FROM dbo.CollarDeployments
				   WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId
				     AND ProjectId = @ProjectId AND AnimalId = @AnimalId
				     AND DeploymentDate = @DeploymentDate
			      )
	BEGIN
		RAISERROR('The deployment you want to change was not found.', 18, 0)
		RETURN (1)
	END

	-- Verify the retrieval occurs after the deployment
	IF @RetrievalDate IS NOT NULL AND @RetrievalDate <= @DeploymentDate
	BEGIN
		RAISERROR('The retrieval must occur after the deployment.', 18, 0)
		RETURN (1)
	END

	-- Get the old retrieval date
	DECLARE @OldRetrievalDate DATETIME2 = NULL
	SELECT @OldRetrievalDate = RetrievalDate FROM dbo.CollarDeployments
			   WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId
			     AND ProjectId = @ProjectId AND AnimalId = @AnimalId
			     AND DeploymentDate = @DeploymentDate;
			     
			

	IF @OldRetrievalDate = @RetrievalDate	
		Return(0) -- Nothing needs to be done.
	
	if @RetrievalDate IS NULL OR (@OldRetrievalDate IS NOT NULL AND @OldRetrievalDate < @RetrievalDate)
	BEGIN
		-- We are expanding the duration of this deployment which may cause a conflict
		
		-- Verify that the animal is not wearing another collar during the proposed date range
		IF EXISTS (SELECT 1 FROM dbo.CollarDeployments
					WHERE ProjectId = @ProjectId AND AnimalId = @AnimalId
					  AND DeploymentDate <> @DeploymentDate  -- make sure we ignore the deployment being changed
					  AND dbo.DoDateRangesOverlap(DeploymentDate, RetrievalDate, @DeploymentDate, @RetrievalDate) = 1
				  )
		BEGIN
			DECLARE @message3 nvarchar(200) = 'The new retreival date conflicts with another deployment for animal ('+@ProjectId+'-'+@AnimalId+').'
			RAISERROR(@message3, 18, 0)
			RETURN (1)
		END
	
		-- Verify that the collar is not on another animal during the proposed date range
		IF EXISTS (SELECT 1 FROM dbo.CollarDeployments
					WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId
					  AND DeploymentDate <> @DeploymentDate  -- make sure we ignore the deployment being changed
					  AND dbo.DoDateRangesOverlap(DeploymentDate, RetrievalDate, @DeploymentDate, @RetrievalDate) = 1
				  )
		BEGIN
			DECLARE @message4 nvarchar(200) = 'The new retreival date conflicts with another deployment for collar ('+@CollarManufacturer+'-'+@CollarId+').'
			RAISERROR(@message4, 18, 0)
			RETURN (1)
		END
	END
	
	-- All other verification is handled by primary/foreign key and column constraints.
	UPDATE dbo.CollarDeployments SET RetrievalDate = @RetrievalDate
			   WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId
			     AND ProjectId = @ProjectId AND AnimalId = @AnimalId
			     AND DeploymentDate = @DeploymentDate;
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Regan Sarwas
-- Create date: April 2, 2012
-- Description:	Adds a new Collar Deployment.
-- =============================================
CREATE PROCEDURE [dbo].[CollarDeployment_Insert] 
	@ProjectId			NVARCHAR(255) = NULL,
	@AnimalId			NVARCHAR(255) = NULL, 
	@CollarManufacturer NVARCHAR(255) = NULL,
	@CollarId			NVARCHAR(255) = NULL, 
	@DeploymentDate		DATETIME2 = NULL, 
	@RetrievalDate		DATETIME2 = NULL
AS
BEGIN
	SET NOCOUNT ON;

	-- Get the name of the caller
	DECLARE @Caller sysname = ORIGINAL_LOGIN();

	-- Validate permission for this operation
	-- The caller must be the PI or editor on the project
	IF NOT EXISTS (SELECT 1 FROM dbo.Projects WHERE ProjectId = @ProjectId AND ProjectInvestigator = @Caller)
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM dbo.ProjectEditors WHERE ProjectId = @ProjectId AND Editor = @Caller)
		BEGIN
			DECLARE @message1 nvarchar(200) = 'You ('+@Caller+') must be the principal investigator or editor on this project ('+@ProjectId+') to deploy a collar on the project.';
			RAISERROR(@message1, 18, 0)
			RETURN (1)
		END
	END

	-- And, the collar must belong to the PI of the project
	DECLARE @ProjectInvestigator sysname
	SELECT @ProjectInvestigator = ProjectInvestigator FROM dbo.Projects WHERE ProjectId  = @ProjectId;
	DECLARE @Manager sysname
	SELECT @Manager = Manager FROM dbo.Collars WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId;
	
	IF @Manager <> @ProjectInvestigator
	BEGIN
		DECLARE @message2 nvarchar(200) = 'The collar ('+@CollarManufacturer+'/'+@CollarId+') does not belong to the PI ('+@ProjectInvestigator+') of the project ('+@ProjectId+').';
		RAISERROR(@message2, 18, 0)
		RETURN (1)
	END
	
	-- Verify the retrieval occurs after the deployment
	IF @RetrievalDate IS NOT NULL AND @RetrievalDate <= @DeploymentDate
	BEGIN
		RAISERROR('The retrieval must occur after the deployment.', 18, 0)
		RETURN (1)
	END

	-- Verify that the animal is not wearing another collar during the proposed date range
	IF EXISTS (SELECT 1 FROM dbo.CollarDeployments
				WHERE ProjectId = @ProjectId AND AnimalId = @AnimalId
				  AND dbo.DoDateRangesOverlap(DeploymentDate, RetrievalDate, @DeploymentDate, @RetrievalDate) = 1
			  )
	BEGIN
		DECLARE @message3 nvarchar(200) = 'The animal ('+@ProjectId+'-'+@AnimalId+') is already wearing a collar during your date range.'
		RAISERROR(@message3, 18, 0)
		RETURN (1)
	END
	
	-- Verify that the collar is not on another animal during the proposed date range
	IF EXISTS (SELECT 1 FROM dbo.CollarDeployments
				WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId
				  AND dbo.DoDateRangesOverlap(DeploymentDate, RetrievalDate, @DeploymentDate, @RetrievalDate) = 1
			  )
	BEGIN
		DECLARE @message4 nvarchar(200) = 'The collar ('+@CollarManufacturer+'-'+@CollarId+') is already deployed during your date range.'
		RAISERROR(@message4, 18, 0)
		RETURN (1)
	END
	

	-- All other verification is handled by primary/foreign key and column constraints.
	INSERT INTO dbo.CollarDeployments
	            ([ProjectId], [AnimalId], [CollarManufacturer], [CollarId],  
	             [DeploymentDate], [RetrievalDate])
		 VALUES (nullif(@ProjectId,''), nullif(@AnimalId,''), nullif(@CollarManufacturer,''), 
				 nullif(@CollarId,''), nullif(@DeploymentDate,''), nullif(@RetrievalDate,''))
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Regan Sarwas
-- Create date: April 2, 2012
-- Description:	Delete a Collar Deployment.
-- =============================================
CREATE PROCEDURE [dbo].[CollarDeployment_Delete] 
	@ProjectId			NVARCHAR(255) = NULL,
	@AnimalId			NVARCHAR(255) = NULL, 
	@CollarManufacturer NVARCHAR(255) = NULL,
	@CollarId			NVARCHAR(255) = NULL, 
	@DeploymentDate		DATETIME2 = NULL 
AS
BEGIN
	SET NOCOUNT ON;

	-- Get the name of the caller
	DECLARE @Caller sysname = ORIGINAL_LOGIN();

	-- Validate permission for this operation
	-- The caller must be the PI or editor on the project
	IF NOT EXISTS (SELECT 1 FROM dbo.Projects WHERE ProjectId = @ProjectId AND ProjectInvestigator = @Caller)
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM dbo.ProjectEditors WHERE ProjectId = @ProjectId AND Editor = @Caller)
		BEGIN
			DECLARE @message1 nvarchar(200) = 'You ('+@Caller+') must be the principal investigator or editor on this project ('+@ProjectId+') to delete this collar.';
			RAISERROR(@message1, 18, 0)
			RETURN (1)
		END
	END


	-- deleting a non-existing deployment will silently succeed.
	--All other verification is handled by primary/foreign key and column constraints.
	DELETE FROM dbo.CollarDeployments
		  WHERE [ProjectId] = @ProjectId
		    AND [AnimalId] = @AnimalId
		    AND [CollarManufacturer] = @CollarManufacturer
		    AND [CollarId] = @CollarId
		    AND [DeploymentDate] = @DeploymentDate
END
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
	@Gender NCHAR(1) = NULL, 
	@MortalityDate DATETIME2(7),
	@GroupName NVARCHAR(255) = NULL, 
	@Description NVARCHAR(4000) = NULL 
AS
BEGIN
	SET NOCOUNT ON;

	-- Get the name of the caller
	DECLARE @Caller sysname = ORIGINAL_LOGIN();

	-- Validate permission for this operation
	-- The caller must be the PI or editor on the project
	IF NOT EXISTS (SELECT 1 FROM dbo.Projects WHERE ProjectId = @ProjectId AND ProjectInvestigator = @Caller)
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM dbo.ProjectEditors WHERE ProjectId = @ProjectId AND Editor = @Caller)
		BEGIN
			DECLARE @message1 nvarchar(200) = 'You ('+@Caller+') must be the principal investigator or editor on this project ('+@ProjectId+') to update an animal.';
			RAISERROR(@message1, 18, 0)
			RETURN (1)
		END
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
	@Gender NCHAR(1) = NULL, 
	@MortalityDate DATETIME2(7) = NULL, 
	@GroupName NVARCHAR(255) = NULL, 
	@Description NVARCHAR(4000) = NULL 
AS
BEGIN
	SET NOCOUNT ON;
	
	-- Get the name of the caller
	DECLARE @Caller sysname = ORIGINAL_LOGIN();

	-- Validate permission for this operation
	-- The caller must be the PI or editor on the project
	IF NOT EXISTS (SELECT 1 FROM dbo.Projects WHERE ProjectId = @ProjectId AND ProjectInvestigator = @Caller)
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM dbo.ProjectEditors WHERE ProjectId = @ProjectId AND Editor = @Caller)
		BEGIN
			DECLARE @message1 nvarchar(200) = 'You ('+@Caller+') must be the principal investigator or editor on this project ('+@ProjectId+') to add an animal.';
			RAISERROR(@message1, 18, 0)
			RETURN (1)
		END
	END
	
	--All other verification is handled by primary/foreign key and column constraints.
	INSERT INTO dbo.Animals ([ProjectId], [AnimalId], [Species], [Gender], [MortalityDate], [GroupName], [Description])
		 VALUES (nullif(@ProjectId,''), nullif(@AnimalId,''), nullif(@Species,''), nullif(@Gender,''),
		         @MortalityDate, nullif(@GroupName,''), nullif(@Description,''));

END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Regan Sarwas
-- Create date: March 20, 2012
-- Description:	Updates the settings table
-- =============================================
CREATE PROCEDURE [dbo].[Settings_Update] 
	@Key   nvarchar(30)  = Null,
	@Value nvarchar(500) = Null
AS
BEGIN
	SET NOCOUNT ON;

	-- Get the name of the caller
	DECLARE @Caller sysname = ORIGINAL_LOGIN();
	
	IF @Key = 'project' AND NOT EXISTS (SELECT 1 FROM [dbo].[Projects] WHERE ProjectId = @Value)
	BEGIN
		DECLARE @message1 nvarchar(100) = 'Project (' + @Value + ') is not an existing project';
		RAISERROR(@message1, 18, 0)
		RETURN 1
	END
	IF @Key in ('project', 'collar_manufacturer', 'filter_projects', 'species', 'file_format', 'collar_model', 'othervalidkeys...') --Add valid keys to this list
	BEGIN
		BEGIN TRY
			BEGIN TRAN
				DELETE dbo.Settings WHERE [Username] = @Caller AND [Key] = @Key 
				INSERT dbo.Settings ([Username], [Key], [Value]) VALUES (@Caller, @Key, @Value) 
			COMMIT TRANSACTION
		END TRY
		BEGIN CATCH
			IF XACT_STATE() <> 0
				ROLLBACK TRANSACTION;
			EXEC [dbo].[Utility_RethrowError]
			RETURN 1
		END CATCH
	END
	ELSE
	BEGIN
		DECLARE @message2 nvarchar(100) = 'Invalid key (' + @Key + ') for settings';
		RAISERROR(@message2, 18, 0)
		RETURN 1
	END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Regan Sarwas
-- Create date: March 28, 2012
-- Description:	Adds a new project to the database.
-- =============================================
CREATE PROCEDURE [dbo].[Project_Insert] 
	@ProjectId NVARCHAR(255)= NULL,
	@ProjectName NVARCHAR(255) = NULL, 
	@ProjectInvestigator sysname = NULL, 
	@UnitCode NVARCHAR(255) = NULL, 
	@Description NVARCHAR(4000) = NULL 
AS
BEGIN
	SET NOCOUNT ON;

	-- Get the name of the caller
	DECLARE @Caller sysname = ORIGINAL_LOGIN();
		
	--default PI is the calling user
	IF nullif(@ProjectInvestigator,'') IS NULL
	BEGIN
		SET @ProjectInvestigator = @Caller;
	END

	-- If the caller is not a PI then error and return
	IF NOT EXISTS (SELECT 1 FROM [dbo].[ProjectInvestigators] WHERE [Login] = @Caller)
	BEGIN
		DECLARE @message1 nvarchar(100) = 'You ('+@Caller+') must be a principal investigator to create a project';
		RAISERROR(@message1, 18, 0)
		RETURN 1
	END

	INSERT INTO dbo.Projects ([ProjectId], [ProjectName], [ProjectInvestigator], [UnitCode], [Description])
		 VALUES (nullif(@ProjectId,''), nullif(@ProjectName,''), nullif(@ProjectInvestigator,''),
		         nullif(@UnitCode,''), nullif(@Description,''))

END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LookupCollarFileHeaders](
	[Header] [nvarchar](450) NOT NULL,
	[FileFormat] [char](1) NOT NULL,
 CONSTRAINT [PK_CollarFileHeaders] PRIMARY KEY CLUSTERED 
(
	[Header] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Regan Sarwas
-- Create date: April 2, 2012
-- Description:	Updates a Collar
-- =============================================
CREATE PROCEDURE [dbo].[Collar_Update] 
	@CollarManufacturer NVARCHAR(255)= NULL,
	@CollarId NVARCHAR(255) = NULL, 
	@CollarModel NVARCHAR(255) = NULL, 
	@Manager sysname = NULL, 
	@Owner NVARCHAR(255) = NULL, 
	@AlternativeId NVARCHAR(255) = NULL, 
	@SerialNumber NVARCHAR(255) = NULL, 
	@Frequency FLOAT = NULL, 
	@DownloadInfo NVARCHAR(255) = NULL, 
	@Notes NVARCHAR(max) = NULL 
AS
BEGIN
	SET NOCOUNT ON;

	-- Get the name of the caller
	DECLARE @Caller sysname = ORIGINAL_LOGIN();

	-- Verify this is an existing collar
	-- Otherwise, the update will silently succeed, which could be confusing.
	IF NOT EXISTS (SELECT 1 FROM dbo.Collars WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId)
	BEGIN
		DECLARE @message2 nvarchar(100) = 'There is no such collar ('+@CollarManufacturer+'/'+@CollarId+').';
		RAISERROR(@message2, 18, 0)
		RETURN (1)
	END
		
	-- Validate permission for this operation
	-- The caller must be the Manager of the collar
	IF NOT EXISTS (SELECT 1 FROM dbo.Collars WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId AND [Manager] = @Caller)
	BEGIN
		DECLARE @message1 nvarchar(200) = 'You ('+@Caller+') must be the manger of this collar ('+@CollarManufacturer+'/'+@CollarId+') to update it.';
		RAISERROR(@message1, 18, 0)
		RETURN (1)
	END

	-- If a parameter is not provided, use the existing value.
	-- (to put null in a field the user will need to pass an empty string)
	IF @CollarModel IS NULL
	BEGIN
		SELECT @CollarModel = [CollarModel] FROM dbo.Collars WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId;
	END
	
	IF @Manager IS NULL
	BEGIN
		SELECT @Manager = [Manager] FROM dbo.Collars WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId;
	END
	
	IF @Owner IS NULL
	BEGIN
		SELECT @Owner = [Owner] FROM dbo.Collars WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId;
	END
	
	IF @AlternativeId IS NULL
	BEGIN
		SELECT @AlternativeId = [AlternativeId] FROM dbo.Collars WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId;
	END
	
	IF @SerialNumber IS NULL
	BEGIN
		SELECT @SerialNumber = [SerialNumber] FROM dbo.Collars WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId;
	END
	
	IF @Frequency IS NULL
	BEGIN
		SELECT @Frequency = [Frequency] FROM dbo.Collars WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId;
	END
	
	IF @DownloadInfo IS NULL
	BEGIN
		SELECT @DownloadInfo = [DownloadInfo] FROM dbo.Collars WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId;
	END
	
	IF @Notes IS NULL
	BEGIN
		SELECT @Notes = [Notes] FROM dbo.Collars WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId;
	END
	
	-- Do the update, replacing empty strings with NULLs
	-- All other verification is handled by primary/foreign key and column constraints.

	UPDATE dbo.Collars SET [CollarModel] = nullif(@CollarModel,''),
						   [Manager] = nullif(@Manager,''),
						   [Owner] = nullif(@Owner,''),
						   [AlternativeId] = nullif(@AlternativeId,''),
						   [SerialNumber] = nullif(@SerialNumber,''),
						   [Frequency] = nullif(@Frequency,0),
						   [DownloadInfo] = nullif(@DownloadInfo,''),
						   [Notes] = nullif(@Notes,'')
					 WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId;

END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Regan Sarwas
-- Create date: April 2, 2012
-- Description:	Adds a new Collar.
-- =============================================
CREATE PROCEDURE [dbo].[Collar_Insert] 
	@CollarManufacturer NVARCHAR(255)= NULL,
	@CollarId NVARCHAR(255) = NULL, 
	@CollarModel NVARCHAR(255) = NULL, 
	@Owner NVARCHAR(255) = NULL, 
	@AlternativeId NVARCHAR(255) = NULL, 
	@SerialNumber NVARCHAR(255) = NULL, 
	@Frequency FLOAT = NULL, 
	@DownloadInfo NVARCHAR(255) = NULL, 
	@Notes NVARCHAR(max) = NULL 
AS
BEGIN
	SET NOCOUNT ON;

	-- Get the name of the caller
	DECLARE @Caller sysname = ORIGINAL_LOGIN();

	-- Validate permission for this operation
	-- If the caller is not a PI then error and return
	-- This is enforced by the defaults and referential integrity,
	-- but this makes the returned error message much easier to interpret.
	IF NOT EXISTS (SELECT 1 FROM [dbo].[ProjectInvestigators] WHERE [Login] = @Caller)
	BEGIN
		DECLARE @message1 nvarchar(100) = 'You ('+@Caller+') must be a principal investigator to create a collar';
		RAISERROR(@message1, 18, 0)
		RETURN
	END
	
	--All other verification is handled by primary/foreign key and column constraints.
	INSERT INTO dbo.Collars ([CollarManufacturer], [CollarId], [CollarModel], [Owner],  
							 [AlternativeId], [SerialNumber], [Frequency], [DownloadInfo], [Notes])
			 VALUES (nullif(@CollarManufacturer,''), nullif(@CollarId,''), nullif(@CollarModel,''),
					 nullif(@Owner,''), nullif(@AlternativeId,''), nullif(@SerialNumber,''),
					 nullif(@Frequency,''), nullif(@DownloadInfo,''), nullif(@Notes,''))
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Regan Sarwas
-- Create date: April 2, 2012
-- Description:	Delete a collar
-- =============================================
CREATE PROCEDURE [dbo].[Collar_Delete] 
	@CollarManufacturer NVARCHAR(255)= NULL,
	@CollarId NVARCHAR(255) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	-- Get the name of the caller
	DECLARE @Caller sysname = ORIGINAL_LOGIN();

	-- Validate permission for this operation
	-- The caller must be the Manager of the collar
	IF NOT EXISTS (SELECT 1 FROM dbo.Collars WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId AND [Manager] = @Caller)
	BEGIN
		DECLARE @message1 nvarchar(200) = 'You ('+@Caller+') must be the manger of this collar ('+@CollarManufacturer+'/'+@CollarId+') to delete it.';
		RAISERROR(@message1, 18, 0)
		RETURN (1)
	END

	-- deleting a non-existing collar will silently succeed.
	-- All other verification is handled by primary/foreign key and column constraints.
	DELETE FROM dbo.Collars WHERE CollarManufacturer = @CollarManufacturer AND CollarId = @CollarId;
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CollarDataTelonicsStoreOnBoard](
	[FileId] [int] NOT NULL,
	[LineNumber] [int] NOT NULL,
	[Fix #] [varchar](50) NULL,
	[Date] [char](10) NULL,
	[Time] [char](8) NULL,
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
 CONSTRAINT [PK_CollarDataTelonicsStoreOnBoard] PRIMARY KEY CLUSTERED 
(
	[FileId] ASC,
	[LineNumber] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[UpdateCollarFixesAfterCollarDataTelonicsStoreOnBoardInsert] 
ON [dbo].[CollarDataTelonicsStoreOnBoard] 
AFTER INSERT AS
BEGIN
	SET NOCOUNT ON;

	-- Add new record to CollarFixes table
	INSERT INTO CollarFixes (FileId, LineNumber, CollarManufacturer, CollarId, FixDate, Lat, Lon)
		 SELECT I.FileId, I.LineNumber, F.CollarManufacturer, F.CollarId,
		        CONVERT(datetime2, I.[Date]+ ' ' + I.[Time]),
		        CONVERT(float, I.Latitude), CONVERT(float, I.Longitude) - 360.0
		   FROM inserted as I INNER JOIN CollarFiles as F 
			 ON I.FileId = F.FileId
		  WHERE F.[Status] = 'A' AND I.[Fix Status] = 'Fix Available'

END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CollarDataDebevekFormat](
	[FileID] [int] NOT NULL,
	[LineNumber] [int] NOT NULL,
	[CollarID] [char](6) NULL,
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[UpdateCollarFixesAfterCollarDataDebevekFormatInsert] 
ON [dbo].[CollarDataDebevekFormat] 
AFTER INSERT AS
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO CollarFixes (FileId, LineNumber, CollarManufacturer, CollarId, FixDate, Lat, Lon)
	 SELECT I.FileId, I.LineNumber, F.CollarManufacturer, I.CollarId,
	        CONVERT(datetime2, I.[FixDate]+ ' ' + I.[FixTime]),
	        I.LatWGS84, I.LonWGS84
	   FROM inserted as I INNER JOIN CollarFiles as F 
		 ON I.FileId = F.FileId
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[StoreOnBoardLocations]
AS
SELECT     dbo.CollarDataTelonicsStoreOnBoard.*, dbo.Animals.*, dbo.Locations.Location, dbo.CollarFiles.FileName, dbo.CollarFiles.UserName, 
                      dbo.CollarFiles.UploadDate
FROM         dbo.CollarDataTelonicsStoreOnBoard INNER JOIN
                      dbo.CollarFixes ON dbo.CollarDataTelonicsStoreOnBoard.FileId = dbo.CollarFixes.FileId AND 
                      dbo.CollarDataTelonicsStoreOnBoard.LineNumber = dbo.CollarFixes.LineNumber INNER JOIN
                      dbo.Locations ON dbo.CollarFixes.FixId = dbo.Locations.FixId INNER JOIN
                      dbo.Animals ON dbo.Locations.ProjectId = dbo.Animals.ProjectId AND dbo.Locations.AnimalId = dbo.Animals.AnimalId INNER JOIN
                      dbo.CollarFiles ON dbo.CollarDataTelonicsStoreOnBoard.FileId = dbo.CollarFiles.FileId AND dbo.CollarFixes.FileId = dbo.CollarFiles.FileId
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
				F.[FileName]+'.csv' AS [File],
				COUNT(*) AS [FixCount],
				MIN(FixDate) AS [First],
				MAX(FixDate) AS [Last]
		FROM [dbo].[CollarFixes] AS X INNER JOIN [dbo].[CollarFiles] AS F
		ON X.FileId = F.FileId
		WHERE X.CollarManufacturer = @CollarManufacturer AND X.CollarId = @CollarId
		GROUP BY F.[FileId], F.[FileName]
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
	@FixId bigint = Null
AS
BEGIN
	SET NOCOUNT ON;
	
	-- Get some information about the fix to update
	DECLARE @HiddenBy  BIGINT;
	DECLARE @ProjectId NVARCHAR(255);
	 SELECT @HiddenBy  = [HiddenBy],
		    @ProjectId = [Project]
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
	-- The caller must be the PI or editor on the project
	IF NOT EXISTS (SELECT 1 FROM dbo.Projects WHERE ProjectId = @ProjectId AND ProjectInvestigator = @Caller)
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM dbo.ProjectEditors WHERE ProjectId = @ProjectId AND Editor = @Caller)
		BEGIN
			DECLARE @message2 nvarchar(200) = 'You ('+@Caller+') must be the principal investigator or editor of the project ('+@ProjectId+') to update this file.';
			RAISERROR(@message2, 18, 0)
			RETURN (1)
		END
	END
	
	IF  @HiddenBy IS NULL
	BEGIN
		RETURN 0  --Nothing to do.
	END
	
	-- Fix is hidden, and we have permissions, lets do it.
	DECLARE @ConflictingFixes TABLE
	(
		FixId bigint,
		HiddenBy bigint
	);
	DECLARE @NotHidden bigint;
	DECLARE @NotHider bigint;

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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CollarDataTelonicsGen4Condensed](
	[FileId] [int] NOT NULL,
	[LineNumber] [int] NOT NULL,
	[AcquisitionTime] [varchar](50) NULL,
	[AcquisitionStartTime] [varchar](50) NULL,
	[ArgosLocationClass] [varchar](50) NULL,
	[ArgosLatitude] [varchar](50) NULL,
	[ArgosLongitude] [varchar](50) NULL,
	[ArgosAltitude] [varchar](50) NULL,
	[GpsFixAttempt] [varchar](50) NULL,
	[GpsLatitude] [varchar](50) NULL,
	[GpsLongitude] [varchar](50) NULL,
	[GpsUtmZone] [varchar](50) NULL,
	[GpsUtmNorthing] [varchar](50) NULL,
	[GpsUtmEasting] [varchar](50) NULL,
	[Temperature] [varchar](50) NULL,
	[SatelliteUplink] [varchar](50) NULL,
	[ReceiveTime] [varchar](50) NULL,
	[SatelliteName] [varchar](50) NULL,
	[RepetitionCount] [varchar](50) NULL,
	[LowVoltage] [varchar](50) NULL,
	[Mortality] [varchar](50) NULL,
	[PredeploymentData] [varchar](50) NULL,
	[Error] [varchar](250) NULL,
 CONSTRAINT [PK_CollarDataTelonicsGen4Condensed] PRIMARY KEY CLUSTERED 
(
	[FileId] ASC,
	[LineNumber] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
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
	@Format CHAR
AS
BEGIN
	SET NOCOUNT ON;

	-- This is not executed directly, only by CollarFile_Insert & CollarFile_UpdateStatus 

	-- if FileId is not found, or file is Inactive, then no fixes will be added.
	
	--FIXME - CollarFile.CollarId, and CollarDataDebevekFormat.CollarId are not
	--        related to Collars.CollarId, but CollarFixes.CollarId is.
	--        therefore, we could try to insert a foreign key violation, and
	--        this entire update could fail.
	
	IF @Format = 'A'  -- Store on board
	BEGIN
		INSERT INTO dbo.CollarFixes (FileId, LineNumber, CollarManufacturer, CollarId, FixDate, Lat, Lon)
		 SELECT I.FileId, I.LineNumber, F.CollarManufacturer, F.CollarId,
		        CONVERT(datetime2, I.[Date]+ ' ' + I.[Time]),
		        CONVERT(float, I.Latitude), CONVERT(float, I.Longitude) - 360.0
		   FROM dbo.CollarDataTelonicsStoreOnBoard as I INNER JOIN CollarFiles as F 
			 ON I.FileId = F.FileId
		  WHERE F.[Status] = 'A'
		    AND I.[Fix Status] = 'Fix Available'
		    AND I.FileId = @FileId
	END
		
	IF @Format = 'B'  -- Debevek Format
	BEGIN
		INSERT INTO dbo.CollarFixes (FileId, LineNumber, CollarManufacturer, CollarId, FixDate, Lat, Lon)
		 SELECT I.FileId, I.LineNumber, F.CollarManufacturer, I.CollarId,
		        CONVERT(datetime2, I.[FixDate]+ ' ' + ISNULL(I.[FixTime],'')),
		        I.LatWGS84, I.LonWGS84
		   FROM dbo.CollarDataDebevekFormat as I INNER JOIN CollarFiles as F 
			 ON I.FileId = F.FileId
		  WHERE F.[Status] = 'A'
		    AND I.FileId = @FileId
		    AND I.LatWGS84 IS NOT NULL AND I.LonWGS84 IS NOT NULL
		    AND I.[FixDate] IS NOT NULL
	END
	
	IF @Format = 'C'  -- Telonics Gen4 Condensed Convertor Format
	BEGIN
		INSERT INTO dbo.CollarFixes (FileId, LineNumber, CollarManufacturer, CollarId, FixDate, Lat, Lon)
		 SELECT I.FileId, I.LineNumber, F.CollarManufacturer, F.CollarId,
		        CONVERT(datetime2, I.[AcquisitionTime]),
		        CONVERT(float, I.GpsLatitude), CONVERT(float, I.GpsLongitude)
		   FROM dbo.CollarDataTelonicsGen4Condensed as I INNER JOIN CollarFiles as F 
			 ON I.FileId = F.FileId
		  WHERE F.[Status] = 'A'
		    AND I.FileId = @FileId
		    AND I.GpsLatitude IS NOT NULL AND I.GpsLongitude IS NOT NULL
		    AND I.[AcquisitionTime] IS NOT NULL
	END
	
	-- FIXME: Add other formats 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ===============================================
-- Author:		Regan Sarwas
-- Create date: March 2, 2012
-- Description:	Updates the Status of a CollarFile
-- ===============================================
CREATE PROCEDURE [dbo].[CollarFile_UpdateStatus] 
	@FileId INT  = NULL, 
	@Status CHAR = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	-- Get some information about the file to update
	DECLARE @OldStatus CHAR;
	DECLARE @Format    CHAR;
	DECLARE @ProjectId NVARCHAR(255);
	 SELECT @Format    = [Format],
		    @OldStatus = [Status],
		    @ProjectId = [Project]
	   FROM [dbo].[CollarFiles]
	  WHERE [FileId] = @FileId;

	IF @OldStatus IS NULL
	BEGIN
		DECLARE @message2 nvarchar(100) = 'Invalid parameter: (' + @FileId + ') was not found in the CollarFiles table';
		RAISERROR(@message2, 18, 0)
		RETURN 1
	END
	-- If OldStatus was found, then Format and Project are guaranteed.

	-- Get the name of the caller
	DECLARE @Caller sysname = ORIGINAL_LOGIN();

	-- Validate permission for this operation
	-- The caller must be the PI or editor on the project
	IF NOT EXISTS (SELECT 1 FROM dbo.Projects WHERE ProjectId = @ProjectId AND ProjectInvestigator = @Caller)
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM dbo.ProjectEditors WHERE ProjectId = @ProjectId AND Editor = @Caller)
		BEGIN
			DECLARE @message4 nvarchar(200) = 'You ('+@Caller+') must be the principal investigator or editor of the project ('+@ProjectId+') to update this file.';
			RAISERROR(@message4, 18, 0)
			RETURN (1)
		END
	END
		
	IF NOT EXISTS (SELECT 1 FROM [dbo].[LookupCollarFileStatus] WHERE [Code] = @Status)
	BEGIN
		DECLARE @message1 nvarchar(100) = 'Invalid parameter: (' + @Status + ') is not a recognized status';
		RAISERROR(@message1, 18, 0)
		RETURN 1
	END
	
	-- Do update	
	BEGIN TRY
		BEGIN TRAN
			IF (@OldStatus = 'I' AND @Status = 'A')
			BEGIN
				UPDATE [dbo].[CollarFiles] SET [Status] = @Status WHERE [FileId] = @FileId;
				EXEC [dbo].[CollarFixes_Insert] @FileId, @Format 
			END
			IF (@OldStatus = 'A' AND @Status = 'I')
			BEGIN
				UPDATE [dbo].[CollarFiles] SET [Status] = @Status WHERE [FileId] = @FileId;
				EXEC [dbo].[CollarFixes_Delete] @FileId
			END
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
	@FileId INT,
	@Format CHAR
AS
BEGIN
	SET NOCOUNT ON;
	
	-- This is not executed directly, only by CollarFile_Insert 

	IF @Format = 'A'  -- Store on board
	BEGIN
		-- only parse the data if it is not already in the file
		IF NOT EXISTS (SELECT 1 FROM [dbo].[CollarDataTelonicsStoreOnBoard] WHERE [FileId] = @FileId)
		BEGIN
			INSERT INTO [dbo].[CollarDataTelonicsStoreOnBoard] SELECT @FileId as FileId, * FROM [dbo].[ParseFormatA] (@FileId) 
		END
	END
		
	IF @Format = 'B'  -- Debevek Format
	BEGIN
		-- only parse the data if it is not already in the file
		IF NOT EXISTS (SELECT 1 FROM [dbo].[CollarDataDebevekFormat] WHERE [FileId] = @FileId)
		BEGIN
			INSERT INTO [dbo].[CollarDataDebevekFormat] SELECT @FileId as FileId, * FROM [dbo].[ParseFormatB] (@FileId) 
		END
	END
	
	IF @Format = 'C'  -- Telonics Gen4 Condensed Convertor Format
	BEGIN
		-- only parse the data if it is not already in the file
		IF NOT EXISTS (SELECT 1 FROM [dbo].[CollarDataTelonicsGen4Condensed] WHERE [FileId] = @FileId)
		BEGIN
			INSERT INTO [dbo].[CollarDataTelonicsGen4Condensed] SELECT @FileId as FileId, * FROM [dbo].[ParseFormatC] (@FileId) 
		END
	END
	
	-- FIXME: Add other formats
	
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Regan Sarwas
-- Create date: March 3, 2012
-- Description:	Adds a new collar file to the database.
--              All files start out as inactive
-- =============================================
CREATE PROCEDURE [dbo].[CollarFile_Insert] 
	@FileName NVARCHAR(255) = NULL,
	@ProjectId NVARCHAR(255) = NULL, 
	@CollarManufacturer NVARCHAR(255) = NULL, 
	@CollarId NVARCHAR(255) = NULL, 
	@Format CHAR = NULL, 
	@Status CHAR = NULL, 
	@Contents VARBINARY(max) = NULL,
	@FileId INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
    
	-- Get the name of the caller
	DECLARE @Caller sysname = ORIGINAL_LOGIN();

	-- Validate permission for this operation
	-- The caller must be the PI or editor on the project
	IF NOT EXISTS (SELECT 1 FROM dbo.Projects WHERE ProjectId = @ProjectId AND ProjectInvestigator = @Caller)
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM dbo.ProjectEditors WHERE ProjectId = @ProjectId AND Editor = @Caller)
		BEGIN
			DECLARE @message4 nvarchar(200) = 'You ('+@Caller+') must be the principal investigator or editor on this project ('+@ProjectId+') to add an animal.';
			RAISERROR(@message4, 18, 0)
			RETURN (1)
		END
	END
	
	--CollarId is not managed by referential integrity, since there may be multiple collars in a data file
	
	-- The need to provide a CollarId is determined by the file format, so check it. 	
	DECLARE @HasCollarId CHAR(1) = NULL
	SELECT @HasCollarId = HasCollarIdColumn FROM dbo.LookupCollarFileFormats WHERE Code = @Format;
	IF @HasCollarId IS NULL
	BEGIN
		DECLARE @message1 nvarchar(100) = 'Invalid parameter: Format (' + @Format + ') was not found in the LookupCollarFileFormats table';
		RAISERROR(@message1, 18, 0)
		RETURN (1)
	END

	-- If the collar is required, make sure it is provided
	IF @CollarId IS NULL AND (@HasCollarId = 'N')
	BEGIN
		DECLARE @message2 nvarchar(100) = 'Invalid parameter: CollarId must not be null with file format ' + @Format + '.';
		RAISERROR(@message2, 18, 0)
		RETURN (1)
	END
	
	-- If the collar was provided, make sure it is a valid collar
	IF @CollarId IS NOT NULL AND
	   NOT EXISTS (SELECT 1 FROM [dbo].[Collars] WHERE [CollarManufacturer] = @CollarManufacturer
												   AND [CollarId] = @CollarId)
	BEGIN
		DECLARE @message3 nvarchar(100) = 'Invalid parameter: CollarId (' + @CollarId + ') was not found in the Collars table';
		RAISERROR(@message3, 18, 0)
		RETURN (1)
	END
	
	-- FIXME - add transaction support
	BEGIN TRY
		BEGIN TRAN
			INSERT INTO dbo.CollarFiles ([FileName], [Project], [CollarManufacturer], [CollarId], [Format], [Status], [Contents])
				 VALUES (@FileName, @ProjectId, @CollarManufacturer, @CollarId, @Format, @Status, @Contents)

			SET @FileId = SCOPE_IDENTITY();

			EXEC [dbo].[CollarData_Insert] @FileId, @Format
			EXEC [dbo].[CollarFixes_Insert] @FileId, @Format
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF XACT_STATE() <> 0
			ROLLBACK TRANSACTION;
		EXEC [dbo].[Utility_RethrowError]
		RETURN (1)
	END CATCH
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[IsEditor] 
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
	IF EXISTS( SELECT 1 FROM dbo.Projects AS P 
				LEFT JOIN dbo.ProjectEditors AS E ON P.ProjectId = E.ProjectId 
				WHERE P.ProjectId = @Project
				  AND (E.Editor = @User OR P.ProjectInvestigator = @User))
		RETURN 1
	RETURN 0
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[IsFixEditor] 
(
	@FixId BIGINT = NULL, 
	@User sysname = NULL
)
RETURNS BIT
AS
BEGIN
	DECLARE @Result BIT
	
		SELECT @Result = [dbo].[IsEditor]([f].[Project], @User)
		  FROM [dbo].[CollarFixes] AS [x]
	INNER JOIN [dbo].[CollarFiles] AS [f]
			ON [x].[FileId] = [f].[FileId]
		 WHERE [x].[FixId] = @FixId;
		 
	RETURN @Result
END
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
	IF NOT EXISTS (SELECT 1 WHERE [dbo].[IsEditor](@ProjectId, @Caller) = 1)
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Regan Sarwas
-- Create date: March 2, 2012
-- Description:	Returns the name of the table with the raw collar data for a file 
-- =============================================

CREATE FUNCTION [dbo].[GetCollarDataTableName]
(
	@FileId INT = NULL
)
RETURNS sysname
AS
BEGIN
	DECLARE @Result sysname
	SET @Result = (SELECT F.[TableName]
			 	     FROM [CollarFiles] as C
			   INNER JOIN [LookupCollarFileFormats] as F
					   ON C.[Format] = F.[Code]
				    WHERE C.[FileId] = @FileId);
	RETURN @Result
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Regan Sarwas
-- Create date: March 2, 2012
-- Description:	Deletes a CollarFile from the database by first removing all dependent records
-- =============================================
CREATE PROCEDURE [dbo].[CollarFile_Delete] 
	@FileId int = -1
WITH EXECUTE AS OWNER -- Needed for dynamic sql
AS
BEGIN
	SET NOCOUNT ON;
	-- Get the projectId
	DECLARE @ProjectId NVARCHAR(255) = NULL
	SELECT @ProjectID = Project FROM [dbo].[CollarFiles] WHERE [FileId] = @FileId;

	IF @ProjectID IS NULL
	BEGIN
			DECLARE @message1 nvarchar(200) = 'Invalid Parameter: There is no FileId = '+@FileId+' in CollarFiles.';
			RAISERROR(@message1, 18, 0)
			RETURN (1)
	END
	
	-- Get the name of the caller
	DECLARE @Caller sysname = ORIGINAL_LOGIN();

	-- Validate permission for this operation
	-- The caller must be the PI or editor on the project
	-- Do not check the uploader. i.e. Do not allow someone who lost their privileges to remove a file. 
	IF NOT EXISTS (SELECT 1 FROM dbo.Projects WHERE ProjectId = @ProjectId AND ProjectInvestigator = @Caller)
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM dbo.ProjectEditors WHERE ProjectId = @ProjectId AND Editor = @Caller)
		BEGIN
			DECLARE @message2 nvarchar(200) = 'You ('+@Caller+') must be the principal investigator or editor on this project ('+@ProjectId+') to delete a file.';
			RAISERROR(@message2, 18, 0)
			RETURN (1)
		END
	END

	BEGIN TRY
		BEGIN TRAN
			DELETE L FROM dbo.Locations as L
			   INNER JOIN dbo.CollarFixes as C
					   ON C.FixID = L.FixId
					WHERE C.[FileId] = @FileId;

			DELETE FROM [dbo].[CollarFixes] WHERE [FileId] = @FileId;

			DECLARE @TableName sysname = [dbo].[GetCollarDataTableName](@FileId);
			DECLARE @sql NVARCHAR(500) = N'DELETE FROM ' + @TableName + '  WHERE FileId = @file';
			
			-- Execute dynamic SQL with parameters
			EXEC sp_ExecuteSQL @sql, N'@file int', @file = @FileId;
			
			DELETE FROM [dbo].[CollarFiles] WHERE [FileId] = @FileId;
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Regan Sarwas
-- Create date: March 27, 2012
-- Description:	Adds a new editor to a project
-- =============================================
CREATE PROCEDURE [dbo].[Editor_Insert] 
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Regan Sarwas
-- Create date: March 27, 2012
-- Description:	Removes an editor from a project
-- =============================================
CREATE PROCEDURE [dbo].[Editor_Delete] 
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
	
	-- If this Editor is still an editor (on this or other projects), then do nothing more
	IF EXISTS(SELECT 1 FROM [dbo].[ProjectEditors] WHERE Editor = @Editor)
	BEGIN
		RETURN
	END

	-- If this Editor is a project investigator, then do nothing more
	IF EXISTS(SELECT 1 FROM [dbo].[ProjectInvestigators] WHERE [Login] = @Editor)
	BEGIN
		RETURN
	END

	-- This editor is not an editor on any projects, so remove the user from the database
	-- This will also, obviously, remove them from the Editor role
	-- If this fails, the user will still have permissions to run the stored procedures,
	--  however the application should stop them because they are not in the
	--  ProjectEditor table
	BEGIN
		EXEC ('DROP USER ['  + @Editor + ']') --  LOGIN defaults to the same name
	END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Regan Sarwas
-- Create date: March 2, 2012
-- Description:	Deletes a project
-- =============================================
CREATE PROCEDURE [dbo].[Project_Delete] 
	@ProjectId NVARCHAR(255) = NULL 
AS
BEGIN
	SET NOCOUNT ON;

	-- Get the name of the caller
	DECLARE @Caller sysname = ORIGINAL_LOGIN();
	
	IF NOT EXISTS (SELECT 1 FROM [dbo].[Projects] WHERE [ProjectId] = @ProjectId)
	BEGIN
		DECLARE @message1 nvarchar(100) = 'Project (' + @ProjectId + ') is not an existing project.';
		RAISERROR(@message1, 18, 0)
		RETURN 1
	END
	
	-- You must be the ProjectInvestigator to delete the project
	IF NOT EXISTS (SELECT 1 FROM [dbo].[Projects] WHERE [ProjectId] = @ProjectId AND [ProjectInvestigator] = @Caller)
	BEGIN
		DECLARE @message2 nvarchar(100) = 'You are not the owner of the ' + @ProjectId + ' project.';
		RAISERROR(@message2, 18, 0)
		RETURN 1
	END

	DELETE FROM dbo.ProjectEditors WHERE [ProjectId] = @ProjectId
	DELETE FROM dbo.Projects WHERE [ProjectId] = @ProjectId
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ===============================================
-- Author:		Regan Sarwas
-- Create date: March 2, 2012
-- Description:	Updates the Status of a CollarFile
-- ===============================================
CREATE PROCEDURE [dbo].[CollarFile_Update] 
	@FileId INT  = NULL, 
	@FileName NVARCHAR(255) = NULL,
	@CollarId NVARCHAR(255) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	-- Get some information about the file to update
	DECLARE @Status CHAR;
	DECLARE @CollarManufacturer NVARCHAR(255);
	DECLARE @OldFileName NVARCHAR(255);
	DECLARE @OldCollarId NVARCHAR(255);
	DECLARE @ProjectId NVARCHAR(255);
	 SELECT @Status = [Status],
		    @CollarManufacturer = [CollarManufacturer],
		    @OldFileName = [FileName],
		    @OldCollarId = [CollarId],
		    @ProjectId = [Project]
	   FROM [dbo].[CollarFiles]
	  WHERE [FileId] = @FileId;

	IF @Status IS NULL
	BEGIN
		DECLARE @message2 nvarchar(100) = 'Invalid parameter: (' + @FileId + ') was not found in the CollarFiles table';
		RAISERROR(@message2, 18, 0)
		RETURN 1
	END
	-- If OldStatus was found, then Project is guaranteed.

	-- Get the name of the caller
	DECLARE @Caller sysname = ORIGINAL_LOGIN();

	-- Validate permission for this operation
	-- The caller must be the PI or editor on the project
	IF NOT EXISTS (SELECT 1 FROM dbo.Projects WHERE ProjectId = @ProjectId AND ProjectInvestigator = @Caller)
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM dbo.ProjectEditors WHERE ProjectId = @ProjectId AND Editor = @Caller)
		BEGIN
			DECLARE @message4 nvarchar(200) = 'You ('+@Caller+') must be the principal investigator or editor of the project ('+@ProjectId+') to update this file.';
			RAISERROR(@message4, 18, 0)
			RETURN 1
		END
	END
	
	-- Change the file name;  this should never fail.
	IF @FileName IS NOT NULL AND @FileName <> @OldFileName
	BEGIN
		BEGIN TRY
			UPDATE [dbo].[CollarFiles] SET [FileName] = @FileName WHERE [FileId] = @FileId; 
		END TRY
		BEGIN CATCH
			EXEC [dbo].[Utility_RethrowError]
			RETURN 1
		END CATCH
	END

	
	-- Can we update the CollarId?
	IF @CollarId IS NOT NULL AND @CollarId <> @OldCollarId AND @Status = 'A'
	BEGIN
		DECLARE @message1 nvarchar(100) = 'Unable to change the collar of an active file.  Change the status and try again.';
		RAISERROR(@message1, 18, 0)
		RETURN 1
	END
	
	-- If the collar was provided, make sure it is a valid collar
	IF @CollarId IS NOT NULL AND @CollarId <> @OldCollarId AND
	   NOT EXISTS (SELECT 1 FROM [dbo].[Collars] WHERE [CollarManufacturer] = @CollarManufacturer
												   AND [CollarId] = @CollarId)
	BEGIN
		DECLARE @message3 nvarchar(100) = 'Invalid parameter: CollarId (' + @CollarId + ') was not found in the Collars table';
		RAISERROR(@message3, 18, 0)
		RETURN 1
	END

	-- Update CollarId; This should never fail
	IF @CollarId IS NOT NULL AND @CollarId <> @OldCollarId
	BEGIN
		BEGIN TRY
			UPDATE [dbo].[CollarFiles] SET [CollarId] = @CollarId WHERE [FileId] = @FileId; 
		END TRY
		BEGIN CATCH
			EXEC [dbo].[Utility_RethrowError]
			RETURN 1
		END CATCH
	END
END
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
/* Protection Model
 * ================
 * All PIs and editors need permission to run this SP. - Done.
 * PIs can only modify projects for which they are the PI or an editor. - Done.
 * Editors can only modify projects for which they are an editor. - Done.
 * No one is allowed to modify the Project Id. - Done. (to change the ID you must deleted and recreate)
 * Only the PI is allowed to change the PI column. - Undone.  FIXME FIXME FIXME FIXME FIXME FIXME FIXME
 */

-- =============================================
-- Author:		Regan Sarwas
-- Create date: March 28, 2012
-- Description:	Updates a project name
-- =============================================
CREATE PROCEDURE [dbo].[Project_Update] 
	@ProjectId			 NVARCHAR(255)  = NULL,
	@ProjectName		 NVARCHAR(255)  = NULL, 
	@UnitCode			 NVARCHAR(255)  = NULL, 
	@ProjectInvestigator NVARCHAR(255)  = NULL, 
	@Description		 NVARCHAR(4000) = NULL 
AS
BEGIN
	SET NOCOUNT ON;

	-- Verify this is an existing project
	IF NOT EXISTS (SELECT 1 FROM [dbo].[Projects] WHERE [ProjectId] = @ProjectId)
	BEGIN
		DECLARE @message1 nvarchar(100) = 'Project (' + @ProjectId + ') is not an existing project.';
		RAISERROR(@message1, 18, 0)
		RETURN 1
	END
	
	-- You must be the ProjectInvestigator or and Editor on the project to update it
	--IF NOT EXISTS (SELECT 1 FROM [dbo].[Projects] WHERE [ProjectId] = @ProjectId AND [ProjectInvestigator] = ORIGINAL_LOGIN())
	IF [dbo].[IsEditor](@ProjectId, ORIGINAL_LOGIN()) = 0
	BEGIN
		DECLARE @message2 nvarchar(100) = 'You are not the owner or an editor of the ' + @ProjectId + ' project.';
		RAISERROR(@message2, 18, 0)
		RETURN 1
	END
	
	-- If a parameter is not provided, use the existing value.
	-- (to put null in a field the user will need to pass an empty string)
	IF @ProjectName IS NULL
	BEGIN
		SELECT @ProjectName = [ProjectName] FROM [dbo].[Projects] WHERE [ProjectId] = @ProjectId;
	END
	
	IF @UnitCode IS NULL
	BEGIN
		SELECT @UnitCode = [UnitCode] FROM [dbo].[Projects] WHERE [ProjectId] = @ProjectId;
	END
	
	IF @ProjectInvestigator IS NULL
	BEGIN
		SELECT @ProjectInvestigator = [ProjectInvestigator] FROM [dbo].[Projects] WHERE [ProjectId] = @ProjectId;
	END
	
	IF @Description IS NULL
	BEGIN
		SELECT @Description = [Description] FROM [dbo].[Projects] WHERE [ProjectId] = @ProjectId;
	END

	-- Do the update, replacing empty strings with NULLs
	UPDATE dbo.Projects SET [ProjectName]		  = nullif(@ProjectName,''),
							[UnitCode]			  = nullif(@UnitCode,''),
							[ProjectInvestigator] = nullif(@ProjectInvestigator,''),
							[Description]		  = nullif(@Description,'')
					  WHERE [ProjectId]			  = @ProjectId

END
GO
ALTER TABLE [dbo].[CollarFiles] ADD  CONSTRAINT [DF_CollarFiles_UploadDate]  DEFAULT (getdate()) FOR [UploadDate]
GO
ALTER TABLE [dbo].[CollarFiles] ADD  CONSTRAINT [DF_CollarFiles_UserName]  DEFAULT (original_login()) FOR [UserName]
GO
ALTER TABLE [dbo].[CollarFiles] ADD  CONSTRAINT [DF_CollarFiles_Status]  DEFAULT ('I') FOR [Status]
GO
ALTER TABLE [dbo].[Collars] ADD  CONSTRAINT [DF_Collars_Manager]  DEFAULT (original_login()) FOR [Manager]
GO
ALTER TABLE [dbo].[Collars] ADD  CONSTRAINT [DF_Collars_Owner]  DEFAULT ('NPS') FOR [Owner]
GO
ALTER TABLE [dbo].[Projects] ADD  CONSTRAINT [DF_Projects_PrincipalInvestigator]  DEFAULT (original_login()) FOR [ProjectInvestigator]
GO
ALTER TABLE [dbo].[LookupCollarFileFormats]  WITH CHECK ADD  CONSTRAINT [CK_LookupCollarFileFormats] CHECK  (([HasCollarIdColumn]='Y' OR [HasCollarIdColumn]='N'))
GO
ALTER TABLE [dbo].[LookupCollarFileFormats] CHECK CONSTRAINT [CK_LookupCollarFileFormats]
GO
ALTER TABLE [dbo].[Animals]  WITH CHECK ADD  CONSTRAINT [FK_Animals_Gender] FOREIGN KEY([Gender])
REFERENCES [dbo].[LookupGender] ([Sex])
GO
ALTER TABLE [dbo].[Animals] CHECK CONSTRAINT [FK_Animals_Gender]
GO
ALTER TABLE [dbo].[Animals]  WITH CHECK ADD  CONSTRAINT [FK_Animals_Projects] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Projects] ([ProjectId])
GO
ALTER TABLE [dbo].[Animals] CHECK CONSTRAINT [FK_Animals_Projects]
GO
ALTER TABLE [dbo].[Animals]  WITH CHECK ADD  CONSTRAINT [FK_Animals_Species] FOREIGN KEY([Species])
REFERENCES [dbo].[LookupSpecies] ([Species])
GO
ALTER TABLE [dbo].[Animals] CHECK CONSTRAINT [FK_Animals_Species]
GO
ALTER TABLE [dbo].[CollarDataDebevekFormat]  WITH CHECK ADD  CONSTRAINT [FK_CollarDataDebevekFormat_CollarFiles] FOREIGN KEY([FileID])
REFERENCES [dbo].[CollarFiles] ([FileId])
GO
ALTER TABLE [dbo].[CollarDataDebevekFormat] CHECK CONSTRAINT [FK_CollarDataDebevekFormat_CollarFiles]
GO
ALTER TABLE [dbo].[CollarDataTelonicsGen4Condensed]  WITH CHECK ADD  CONSTRAINT [FK_CollarDataTelonicsGen4Condensed_CollarFiles] FOREIGN KEY([FileId])
REFERENCES [dbo].[CollarFiles] ([FileId])
GO
ALTER TABLE [dbo].[CollarDataTelonicsGen4Condensed] CHECK CONSTRAINT [FK_CollarDataTelonicsGen4Condensed_CollarFiles]
GO
ALTER TABLE [dbo].[CollarDataTelonicsStoreOnBoard]  WITH CHECK ADD  CONSTRAINT [FK_CollarDataTelonicsStoreOnBoard_CollarFiles] FOREIGN KEY([FileId])
REFERENCES [dbo].[CollarFiles] ([FileId])
GO
ALTER TABLE [dbo].[CollarDataTelonicsStoreOnBoard] CHECK CONSTRAINT [FK_CollarDataTelonicsStoreOnBoard_CollarFiles]
GO
ALTER TABLE [dbo].[CollarDeployments]  WITH CHECK ADD  CONSTRAINT [FK_CollarDeployments_Animals] FOREIGN KEY([ProjectId], [AnimalId])
REFERENCES [dbo].[Animals] ([ProjectId], [AnimalId])
GO
ALTER TABLE [dbo].[CollarDeployments] CHECK CONSTRAINT [FK_CollarDeployments_Animals]
GO
ALTER TABLE [dbo].[CollarDeployments]  WITH CHECK ADD  CONSTRAINT [FK_CollarDeployments_Collars] FOREIGN KEY([CollarManufacturer], [CollarId])
REFERENCES [dbo].[Collars] ([CollarManufacturer], [CollarId])
GO
ALTER TABLE [dbo].[CollarDeployments] CHECK CONSTRAINT [FK_CollarDeployments_Collars]
GO
ALTER TABLE [dbo].[CollarFiles]  WITH CHECK ADD  CONSTRAINT [FK_CollarFiles_LookupCollarFileFormats] FOREIGN KEY([Format])
REFERENCES [dbo].[LookupCollarFileFormats] ([Code])
GO
ALTER TABLE [dbo].[CollarFiles] CHECK CONSTRAINT [FK_CollarFiles_LookupCollarFileFormats]
GO
ALTER TABLE [dbo].[CollarFiles]  WITH CHECK ADD  CONSTRAINT [FK_CollarFiles_LookupCollarFileStatus] FOREIGN KEY([Status])
REFERENCES [dbo].[LookupCollarFileStatus] ([Code])
GO
ALTER TABLE [dbo].[CollarFiles] CHECK CONSTRAINT [FK_CollarFiles_LookupCollarFileStatus]
GO
ALTER TABLE [dbo].[CollarFiles]  WITH CHECK ADD  CONSTRAINT [FK_CollarFiles_LookupCollarManufacturers] FOREIGN KEY([CollarManufacturer])
REFERENCES [dbo].[LookupCollarManufacturers] ([CollarManufacturer])
GO
ALTER TABLE [dbo].[CollarFiles] CHECK CONSTRAINT [FK_CollarFiles_LookupCollarManufacturers]
GO
ALTER TABLE [dbo].[CollarFiles]  WITH CHECK ADD  CONSTRAINT [FK_CollarFiles_Projects] FOREIGN KEY([Project])
REFERENCES [dbo].[Projects] ([ProjectId])
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
GO
ALTER TABLE [dbo].[CollarFixes] CHECK CONSTRAINT [FK_CollarFixes_Collars]
GO
ALTER TABLE [dbo].[Collars]  WITH CHECK ADD  CONSTRAINT [FK_Collars_LookupCollarManufacturers] FOREIGN KEY([CollarManufacturer])
REFERENCES [dbo].[LookupCollarManufacturers] ([CollarManufacturer])
GO
ALTER TABLE [dbo].[Collars] CHECK CONSTRAINT [FK_Collars_LookupCollarManufacturers]
GO
ALTER TABLE [dbo].[Collars]  WITH CHECK ADD  CONSTRAINT [FK_Collars_LookupCollarModels] FOREIGN KEY([CollarModel])
REFERENCES [dbo].[LookupCollarModels] ([CollarModel])
GO
ALTER TABLE [dbo].[Collars] CHECK CONSTRAINT [FK_Collars_LookupCollarModels]
GO
ALTER TABLE [dbo].[Collars]  WITH CHECK ADD  CONSTRAINT [FK_Collars_Managers] FOREIGN KEY([Manager])
REFERENCES [dbo].[ProjectInvestigators] ([Login])
GO
ALTER TABLE [dbo].[Collars] CHECK CONSTRAINT [FK_Collars_Managers]
GO
ALTER TABLE [dbo].[Locations]  WITH CHECK ADD  CONSTRAINT [FK_Locations_Animals] FOREIGN KEY([ProjectId], [AnimalId])
REFERENCES [dbo].[Animals] ([ProjectId], [AnimalId])
GO
ALTER TABLE [dbo].[Locations] CHECK CONSTRAINT [FK_Locations_Animals]
GO
ALTER TABLE [dbo].[Locations]  WITH CHECK ADD  CONSTRAINT [FK_Locations_CollarFixes] FOREIGN KEY([FixId])
REFERENCES [dbo].[CollarFixes] ([FixId])
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
ALTER TABLE [dbo].[Movements]  WITH CHECK ADD  CONSTRAINT [FK_Movements_Animals] FOREIGN KEY([ProjectId], [AnimalId])
REFERENCES [dbo].[Animals] ([ProjectId], [AnimalId])
GO
ALTER TABLE [dbo].[Movements] CHECK CONSTRAINT [FK_Movements_Animals]
GO
ALTER TABLE [dbo].[ProjectEditors]  WITH CHECK ADD  CONSTRAINT [FK_ProjectEditors_Projects] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Projects] ([ProjectId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProjectEditors] CHECK CONSTRAINT [FK_ProjectEditors_Projects]
GO
ALTER TABLE [dbo].[Projects]  WITH CHECK ADD  CONSTRAINT [FK_Projects_ProjectInvestigators] FOREIGN KEY([ProjectInvestigator])
REFERENCES [dbo].[ProjectInvestigators] ([Login])
GO
ALTER TABLE [dbo].[Projects] CHECK CONSTRAINT [FK_Projects_ProjectInvestigators]
GO
GRANT EXECUTE ON [dbo].[Animal_Delete] TO [Editor] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[Animal_Insert] TO [Editor] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[Animal_Update] TO [Editor] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[Collar_Delete] TO [Editor] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[Collar_Insert] TO [Editor] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[Collar_Update] TO [Editor] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[CollarDeployment_Delete] TO [Editor] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[CollarDeployment_Insert] TO [Editor] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[CollarDeployment_UpdateRetrievalDate] TO [Editor] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[CollarFile_Delete] TO [Editor] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[CollarFile_Insert] TO [Editor] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[CollarFile_Update] TO [Editor] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[CollarFile_UpdateStatus] TO [Editor] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[CollarFixes_UpdateUnhideFix] TO [Editor] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[Editor_Delete] TO [Investigator] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[Editor_Insert] TO [Investigator] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[Location_UpdateStatus] TO [Editor] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[Project_Delete] TO [Investigator] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[Project_Insert] TO [Investigator] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[Project_Update] TO [Editor] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[ProjectInvestigator_Update] TO [Investigator] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[Settings_Update] TO [NPS\Domain Users] AS [dbo]
GO
GRANT SELECT ON [dbo].[AnimalLocationSummary] TO [NPS\Domain Users] AS [dbo]
GO
GRANT SELECT ON [dbo].[CollarFixesByFile] TO [NPS\Domain Users] AS [dbo]
GO
GRANT SELECT ON [dbo].[CollarFixSummary] TO [NPS\Domain Users] AS [dbo]
GO
GRANT SELECT ON [dbo].[ConflictingFixes] TO [NPS\Domain Users] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[DateTimeToOrdinal] TO [NPS\Domain Users] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[IsEditor] TO [NPS\Domain Users] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[IsFixEditor] TO [NPS\Domain Users] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[NextAnimalId] TO [Editor] AS [dbo]
GO
GRANT ALTER ON ROLE::[Editor] TO [Investigator] AS [dbo]
GO
EXEC dbo.sp_addrolemember @rolename=N'Editor', @membername=N'NPS\SDMiller'
GO
EXEC dbo.sp_addrolemember @rolename=N'Editor', @membername=N'NPS\RESarwas'
GO
EXEC dbo.sp_addrolemember @rolename=N'Editor', @membername=N'NPS\KCJoly'
GO
EXEC dbo.sp_addrolemember @rolename=N'Editor', @membername=N'NPS\BAMangipane'
GO
EXEC dbo.sp_addrolemember @rolename=N'Editor', @membername=N'NPS\JWBurch'
GO
EXEC dbo.sp_addrolemember @rolename=N'Editor', @membername=N'Investigator'
GO
EXEC dbo.sp_addrolemember @rolename=N'Investigator', @membername=N'NPS\SDMiller'
GO
EXEC dbo.sp_addrolemember @rolename=N'Investigator', @membername=N'NPS\RESarwas'
GO
EXEC dbo.sp_addrolemember @rolename=N'Investigator', @membername=N'NPS\KCJoly'
GO
EXEC dbo.sp_addrolemember @rolename=N'Investigator', @membername=N'NPS\BAMangipane'
GO
EXEC dbo.sp_addrolemember @rolename=N'Investigator', @membername=N'NPS\JWBurch'
GO
