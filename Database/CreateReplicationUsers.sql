/*
To run this script in SSMS, you must first turn on SQLCMD Mode in the SSMS Query Menu
Be sure to check the value of the variables first.
*/
--Set Variables
:setvar ServerName "INPAKROMS53AIS"
:setvar DatabaseName "Animal_Movement"


-- Create Logons
----------------

USE [master]
GO

CREATE LOGIN [$(ServerName)\repl_distribution] FROM WINDOWS WITH DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english]
GO
CREATE LOGIN [$(ServerName)\repl_logreader] FROM WINDOWS WITH DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english]
GO
CREATE LOGIN [$(ServerName)\repl_merge] FROM WINDOWS WITH DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english]
GO
CREATE LOGIN [$(ServerName)\repl_snapshot] FROM WINDOWS WITH DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english]
GO


-- Create Database Users
------------------------

USE [$(DatabaseName)]
GO

CREATE USER [$(ServerName)\repl_distribution] FOR LOGIN [$(ServerName)\repl_distribution] WITH DEFAULT_SCHEMA=[dbo]
GO
CREATE USER [$(ServerName)\repl_logreader] FOR LOGIN [$(ServerName)\repl_logreader] WITH DEFAULT_SCHEMA=[dbo]
GO
CREATE USER [$(ServerName)\repl_merge] FOR LOGIN [$(ServerName)\repl_merge] WITH DEFAULT_SCHEMA=[dbo]
GO
CREATE USER [$(ServerName)\repl_snapshot] FOR LOGIN [$(ServerName)\repl_snapshot] WITH DEFAULT_SCHEMA=[dbo]
GO


-- Create Database Roles
------------------------

CREATE ROLE [MSReplPAL_7_1] AUTHORIZATION [dbo]
GO
CREATE ROLE [MStran_PAL_role] AUTHORIZATION [dbo]
GO



-- Add database Users to Roles
------------------------------

EXEC dbo.sp_addrolemember @rolename=N'MSReplPAL_7_1', @membername=N'$(ServerName)\repl_distribution'
GO
EXEC dbo.sp_addrolemember @rolename=N'MStran_PAL_role', @membername=N'$(ServerName)\repl_distribution'
GO
EXEC dbo.sp_addrolemember @rolename=N'MStran_PAL_role', @membername=N'MSReplPAL_7_1'
GO
EXEC dbo.sp_addrolemember @rolename=N'db_owner', @membername=N'$(ServerName)\repl_logreader'
GO
EXEC dbo.sp_addrolemember @rolename=N'db_owner', @membername=N'$(ServerName)\repl_snapshot'
GO


--Grant Permissions
-- SSMS puts most permissions in the CreateDatabaseObjects.sql script,
-- however, these are put in the CreateDatabase.sql script by SSMS.
-- Unfortunately, they rely on server logins that may not exist when
-- the database is created so they were moved to this script.
----------------------------------------------------------------------------------------

GRANT CONNECT TO [$(ServerName)\repl_distribution] AS [dbo]
GO
GRANT CONNECT REPLICATION TO [$(ServerName)\repl_distribution] AS [dbo]
GO
GRANT CONNECT TO [$(ServerName)\repl_logreader] AS [dbo]
GO
GRANT CONNECT TO [$(ServerName)\repl_merge] AS [dbo]
GO
GRANT CONNECT TO [$(ServerName)\repl_snapshot] AS [dbo]
GO

