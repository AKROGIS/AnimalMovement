USE [Animal_Movement]
GO

DROP FUNCTION [dbo].[LocalTime] 
DROP FUNCTION [dbo].[UtcTime]
DROP FUNCTION [dbo].[Sha1Hash]
GO

DROP ASSEMBLY [SqlServer_Functions]
GO

CREATE ASSEMBLY [SqlServer_Functions]
-- Path is from the SQL Server perspective not the SSMS client, or user prespective
--FROM 'C:\Users\resarwas\Documents\GitHub\AnimalMovement\SqlServer_Functions\bin\Release\SqlServer_Functions.dll'
FROM 'E:\transfer\assemblies\SqlServer_Functions.dll'
WITH PERMISSION_SET = SAFE
GO

SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER OFF
GO


CREATE FUNCTION [dbo].[LocalTime] (@utcDateTime [datetime])
RETURNS [datetime]
AS EXTERNAL NAME [SqlServer_Functions].[SqlServer_Functions.SimpleFunctions].[LocalTime];
GO

CREATE FUNCTION [dbo].[UtcTime] (@localDateTime [datetime])
RETURNS [datetime]
AS EXTERNAL NAME [SqlServer_Functions].[SqlServer_Functions.SimpleFunctions].[UtcTime];
GO

CREATE FUNCTION [dbo].[Sha1Hash] (@data [varbinary](MAX))
RETURNS [varbinary](8000)
AS EXTERNAL NAME [SqlServer_Functions].[SqlServer_Functions.SimpleFunctions].[Sha1Hash];
GO

