USE [Animal_Movement]
GO
/*
-- It may be much simpler to just alter  the assembly if the function names/interfaces have not changed

ALTER ASSEMBLY ComplexNumber 
FROM 'E:\transfer\assemblies\SqlServer_Functions.dll'

*/


/*
-- Need to remove all reference (in tables, views, etc) to the functions
-- WARNING: The following is incomplete:

IF COL_LENGTH('CollarFiles','Sha1Hash') IS NOT NULL --safely check if column exists
BEGIN
    alter table [Animal_Movement].[dbo].[collarfiles] drop column [Sha1Hash]
END

IF COL_LENGTH('CollarParameterFiles','Sha1Hash') IS NOT NULL --safely check if column exists
BEGIN
    alter table [Animal_Movement].[dbo].[CollarParameterFiles] drop column [Sha1Hash]
END
*/

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

/*
-- Need to recreate all reference (in tables, views, etc) to the functions
-- WARNING: The following is incomplete:

alter table [Animal_Movement].[dbo].[collarfiles] add [Sha1Hash] AS ([dbo].[Sha1Hash]([Contents])) PERSISTED
alter table [Animal_Movement].[dbo].[CollarParameterFiles] add [Sha1Hash] AS ([dbo].[Sha1Hash]([Contents])) PERSISTED

*/
