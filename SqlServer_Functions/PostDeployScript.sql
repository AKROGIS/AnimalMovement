--this SQL works fine in SSMS, but fails from here.
-- Error: ALTER TABLE failed because the following SET options have incorrect settings: 'CONCAT_NULL_YIELDS_NULL'.
/*
alter table [Animal_Movement].[dbo].[collarfiles] add [Sha1Hash] AS ([dbo].[Sha1Hash]([Contents])) PERSISTED
alter table [Animal_Movement].[dbo].[CollarParameterFiles] add [Sha1Hash] AS ([dbo].[Sha1Hash]([Contents])) PERSISTED
*/