USE [Animal_Movement]
GO

DROP VIEW [dbo].[AllTpfFileData]
GO

DROP FUNCTION [dbo].[FileFormat]
DROP FUNCTION [dbo].[ParseFormatE]
DROP PROCEDURE [dbo].[Summerize]
GO

DROP ASSEMBLY [SqlServer_Files]

GO

CREATE ASSEMBLY [SqlServer_Files]
-- Path is from the SQL Server perspective not the SSMS client, or user prespective
--FROM 'C:\Users\resarwas\Documents\GitHub\AnimalMovement\SqlServer_TpfSummerizer\bin\Release\SqlServer_Files.dll'
FROM 'E:\transfer\assemblies\SqlServer_Files.dll'
WITH PERMISSION_SET = SAFE

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

CREATE FUNCTION [dbo].[FileFormat](@data [varbinary](max))
RETURNS [nchar](1) WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [SqlServer_Files].[SqlServer_Files.CollarFileInfo].[FileFormat]
GO

