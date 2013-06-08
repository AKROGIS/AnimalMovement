/*
To run this script in SSMS, you must first turn on SQLCMD Mode in the SSMS Query Menu
Besure to check the value of the variables first.
*/
--Set Variables
:setvar ServerName "INPAKROMS53AIS"
:setvar AutomationUserName "sql_proxy"
:setvar DatabaseName "Animal_Movement"


-- Create Logons
----------------

USE [master]
GO

CREATE LOGIN [$(ServerName)\$(AutomationUserName)] FROM WINDOWS WITH DEFAULT_DATABASE=[$(DatabaseName)], DEFAULT_LANGUAGE=[us_english]
GO

-- Create Database Users
------------------------

USE [$(DatabaseName)]
GO

CREATE USER [$(ServerName)\$(AutomationUserName)] FOR LOGIN [$(ServerName)\$(AutomationUserName)] WITH DEFAULT_SCHEMA=[dbo]
GO

-- Add database Users to Roles
------------------------------

--Viewer
--Argos Processor
EXEC dbo.sp_addrolemember @rolename=N'ArgosProcessor', @membername=N'$(ServerName)\$(AutomationUserName)'
GO

--Grant Permissions
-- SSMS puts most permissions in the CreateDatabaseObjects.sql script,
-- however, these are put in the CreateDatabase.sql script by SSMS.
-- Unfortunately, they rely on server logins that may not exist when
-- the database is created so they were moved to this script.
----------------------------------------------------------------------------------------

--Automation
GRANT CONNECT TO [$(ServerName)\$(AutomationUserName)] AS [dbo]
GO


-- Turn on execution of external commands for Automation user



