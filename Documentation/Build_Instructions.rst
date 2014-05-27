Build Instructions
==================

These instructions document how to build the project to create the files necessary for the
installation instructions.  This document are my notes that I hope will be helpful when I
need to refere to it in the future.  It may even be helpful for somebody else who might
need it.  I can only hope.

First,  The project is now set up to build with Visual Studio 2013 pro.
The express version may also work.  I have preserved the VS 2010 solution file
as AnimalMovement_VS10x.sln.  This file worked with Visual Studio 2010 pro SP1, when I did
the upgrade to VS2013.  It isn't guaranteed to work now.

ArcGIS integration
------------------

Two projects (QueryLayerBuilder and ArcMap_Addin) depend on ESRI's ArcObjects libraries.
This SDK is an optional install if you have a license to ArcMap Desktop.

The projects are not required, and you can disable them if you want.

  * QueryLayerBuilder
  
  Provides a query builder to allow the user to create an ArcMap
  query layer to display a subset of the Animal Movements database in ArcMap.  This tool
  is fairly limited. If you are familiar with SQL and the structure of the database
  you will have better luck building the query layers by hand, if not, you should find
  someone who is.

  * ArcMap_Addin
  
  This ArcMap add-in provides a convenient method of hiding (or 
  un-hiding) locations, by selecting them in ArcMap. Bad locations are often very
  obvious when looking at the movement vectors in ArcMap.

The ESRI dependent code works only with ArcMap 10.x.  It will not work with ArcMap 9.x,
nor with the new ArcGIS Pro (at least as of the Beta release).  The build for ArcMap 10.0
is different than 10.1 and 10.2.  The projects are currently set up to build with 10.2.
If you need to build with 10.0, then make the following changes:

  * In QueryLayerBuilder properties add a Conditional Compilation Symbol of ARCGIS_10_0 on the build tab
  * In ArcMap_Addin properties add a Conditional Compilation Symbol of ARCGIS_10_0 on the build tab
  * In ArcMap_Addin/Config.esri.addinx, change <Target name="Desktop" version="10.2" /> to 10.0
  
Building
--------

Select the release build configuration, and then build the solution (Sorry, this is not
a VS tutorial)

Creating Installation Files
---------------------------

There is a very simple DOS batch file in the Distribution directory called make_dist.bat.
This file will copy files from the various locatations in the project to subfolders in
the Distribution directory.  The batch file does not have any error checking, so if
things do not work, just take a look at the file and do things manually.

Two of the database files need to be manually edited, so they are not automatically
copied with the previous batch file:

CreateDatabase.sql
~~~~~~~~~~~~~~~~~~

if `~\AnimalMovement\Database\CreateDatabase.sql` is newer than `~\AnimalMovement\Distribution\Database\CreateDatabase.sql` then

 1. Copy and replace
 2. Remove all the GRANT commands at the end of the script
 3. Edit the 5th and 7th lines to change the size of the files to 10240KB and 10240KB respectively

CreateDatabaseObjects.sql
~~~~~~~~~~~~~~~~~~~~~~~~~

if `~\AnimalMovement\Database\CreateDatabaseObjects.sql` is newer than `~\AnimalMovement\Distribution\Database\\CreateDatabaseObjects.sql` then

 1. Copy and replace
 2. Do a global search and replace removing all SET ANSI_PADDING OFF
 
	# This setting causes creation of spatial indices to fail.  The setting is deprecated (see http://msdn.microsoft.com/en-us/library/ms187403(v=sql.90).aspx)

	# SSMS incorrectly inserts it in the script (see https://connect.microsoft.com/SQLServer/feedback/details/127167/trailing-set-ansi-padding-off-when-scripting-tables#details)

 3. Remove all the CREATE USER commands at the start of the script
 4. Remove the CREATE ROLE commands for the replication roles
 5. Remove all the EXEC dbo.sp_addrolemember commands from the end of the script
 6. Search for "CREATE PROCEDURE [dbo].[Summerize]", and add "WITH EXECUTE AS OWNER" on a
    new line before the line with only "AS".
	
	# That this is missing may be a bug that gets fixed in an update of SSMS

SqlServer_CLR.sql
~~~~~~~~~~~~~~~~~

This file adds new functions to the SQL Server database.  It is built from the C# code
in various library projects in the solution.  The dlls are built when the project is compliled
This dll code needs to be put into a SQL script that can be run in the SQL Server.

The following instructions were valid for VS2010 (after a deployment was done) and SQL Server 2008R2.

 * if ..\SqlServer_Files\bin\Release\SqlServer_Files.sql is newer than .\Database\SqlServer_CLR.sql
   * then copy the CREATE ASSEMBLY and ALTER ASSEMBLY commands from the source to the correct section in the destination
 * repeat if ..\SqlServer_Functions\bin\Release\SqlServer_Functions.sql is newer than Database\SqlServer_CLR.sql
 * repeat if ..\SqlServer_Parsers\bin\Release\SqlServer_Parsers.sql is newer than Database\SqlServer_CLR.sql
 * repeat if ..\SqlServer_TpfSummerizer\bin\Release\SqlServer_TpfSummerizer.sql is newer than Database\SqlServer_CLR.sql

The most reliable method now is to use the command line or Visual Studio deployment feature
to deploy the dll to a test database.  Then you can use the generate script tool in SSMS
in the test database that to created this script. Instruction are at `http://msdn.microsoft.com/en-us/library/ms345099(v=sql.105).aspx`

Distributing
------------

Zip up the Distribution folder (without the make_dist.bat file) and provide to the
User.   Thuser can then use the installation Instructions in the Documentation folder
to build and configure the SQL Server database.
