/*
To run this script in SSMS, you must first turn on SQLCMD Mode in the SSMS Query Menu
Be sure to check the value of the variables first.

This is intended to be run once to give viewing permission to a global domain user group.
If you want to limit viewing to individual, or smaller groups, then run this script
several times with the ViewerGroup set to the group or individual.
*/
--Set Variables
:setvar ViewerGroup "NPS\Domain Users"
:setvar DatabaseName "Animal_Movement"


-- Create Logons
----------------

USE [master]
GO

--Viewers - typically this will be a windows domain group, but it could be individual users 
CREATE LOGIN [$(ViewerGroup)] FROM WINDOWS WITH DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english]
GO


-- Create Database Users
------------------------

USE [$(DatabaseName)]
GO

--Most viewers
CREATE USER [$(ViewerGroup)] FOR LOGIN [$(ViewerGroup)]
GO


-- Add database Users to Roles
------------------------------

--Viewer
EXEC dbo.sp_addrolemember @rolename=N'Viewer', @membername=N'$(ViewerGroup)'
GO
EXEC dbo.sp_addrolemember @rolename=N'Viewer', @membername=N'Editor'
GO
EXEC dbo.sp_addrolemember @rolename=N'Viewer', @membername=N'Investigator'
GO
EXEC dbo.sp_addrolemember @rolename=N'Viewer', @membername=N'ArgosProcessor'
GO
EXEC dbo.sp_addrolemember @rolename=N'Editor', @membername=N'Investigator'
GO
EXEC dbo.sp_addrolemember @rolename=N'db_datareader', @membername=N'Viewer'
GO

--Grant Permissions
-- SSMS puts most permissions in the CreateDatabaseObjects.sql script,
-- however, these are put in the CreateDatabase.sql script by SSMS.
-- Unfortunately, they rely on server logins that may not exist when
-- the database is created so they were moved to this script.
----------------------------------------------------------------------------------------

--Viewer
GRANT CONNECT TO [$(ViewerGroup)] AS [dbo]
GO

--Investigator
GRANT ALTER ANY USER TO [Investigator] AS [dbo]
GO
