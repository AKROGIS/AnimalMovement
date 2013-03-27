-- Neither of these two methods will delete the columns, causing failure when replacing the function
-- Curiously, they both work when called from SSMS.
-- more curiouser, the second method will fail (column not found), if the column has already been deleted
/*
USE [Animal_Movement]
GO

IF COL_LENGTH('CollarFiles','Sha1Hash') IS NOT NULL --safely check if column exists
BEGIN
    alter table [Animal_Movement].[dbo].[collarfiles] drop column [Sha1Hash]
END

IF COL_LENGTH('CollarParameterFiles','Sha1Hash') IS NOT NULL --safely check if column exists
BEGIN
    alter table [Animal_Movement].[dbo].[CollarParameterFiles] drop column [Sha1Hash]
END
*/
/*
alter table [Animal_Movement].[dbo].[collarfiles] drop column [Sha1Hash]
alter table [Animal_Movement].[dbo].[CollarParameterFiles] drop column [Sha1Hash]
*/
