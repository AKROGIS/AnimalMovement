USE [Animal_Movement]
GO

DROP VIEW [dbo].[AllTpfFileData]
GO

DROP FUNCTION [dbo].[SummarizeTpfFile]
GO

DROP ASSEMBLY [SqlServer_TpfSummerizer]

GO

CREATE ASSEMBLY [SqlServer_TpfSummerizer]
-- Path is from the SQL Server perspective not the SSMS client, or user prespective
-- FROM 'C:\Users\resarwas\Documents\GitHub\AnimalMovement\SqlServer_TpfSummerizer\bin\Release\SqlServer_TpfSummerizer.dll'
FROM 'E:\transfer\assemblies\SqlServer_TpfSummerizer.dll'
WITH PERMISSION_SET = SAFE

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

