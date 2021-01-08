REM See ..\Documentation\Build_Instructions for details on using this script

SET src_root=..
SET dest_root=.

mkdir %dest_root%\Client
mkdir %dest_root%\Server
mkdir %dest_root%\Database
mkdir %dest_root%\Documentation

SET docs=%src_root%\Documentation
copy /Y %docs%\UserGuide.pdf  %dest_root%\Documentation
copy /Y "%docs%\Installation Instructions.pdf"  %dest_root%\Documentation

SET client=%src_root%\AnimalMovement\bin\Release
copy /Y %client%\AnimalMovement.exe  %dest_root%\Client
copy /Y %client%\QueryLayerBuilder.exe  %dest_root%\Client
copy /Y %client%\AnimalMovement.exe.config  %dest_root%\Client
copy /Y %client%\*.dll  %dest_root%\Client
copy /Y %client%\*.xml  %dest_root%\Client

copy /Y %src_root%\ArgosDownloader\bin\Release\ArgosDownloader.exe  %dest_root%\Server
copy /Y %src_root%\ArgosDownloader\bin\Release\ArgosDownloader.exe.config  %dest_root%\Server
copy /Y %src_root%\ArgosDownloader\bin\Release\*.dll  %dest_root%\Server
copy /Y %src_root%\ArgosProcessor\bin\Release\ArgosProcessor.exe  %dest_root%\Server
copy /Y %src_root%\ArgosProcessor\bin\Release\ArgosProcessor.exe.config  %dest_root%\Server
copy /Y %src_root%\CollarFileLoader\bin\Release\CollarFileLoader.exe  %dest_root%\Server
copy /Y %src_root%\CollarFileLoader\bin\Release\CollarFileLoader.exe.config  %dest_root%\Server

copy /Y %src_root%\Database\CreateAutomationUser.sql  %dest_root%\Database
copy /Y %src_root%\Database\CreateReplicationUsers.sql  %dest_root%\Database
copy /Y %src_root%\Database\CreateUsers.sql  %dest_root%\Database
copy /Y %src_root%\Database\LookupTableData.sql  %dest_root%\Database
copy /Y %src_root%\Database\Settings.sql  %dest_root%\Database

REM These files need to be manually edited, so only copy/replace if they are out of date.
REM copy /Y %src_root%\Database\CreateDatabase.sql  %dest_root%\Database
REM copy /Y %src_root%\Database\CreateDatabaseObjects.sql  %dest_root%\Database

REM The following worked for Visual Studio 2010.  There is a new system for VS2013, that I haven't figured out yet.
REM copy /Y %src_root%\SqlServer_Files\bin\Release\SqlServer_Files.sql  %dest_root%\Database\SqlServer_CLRx.sql
REM copy /Y %src_root%\SqlServer_Functions\bin\Release\SqlServer_Functions.sql  %dest_root%\Database\SqlServer_CLRx.sql
REM copy /Y %src_root%\SqlServer_Parsers\bin\Release\SqlServer_Parsers.sql  %dest_root%\Database\SqlServer_CLRx.sql
REM copy /Y %src_root%\SqlServer_TpfSummerizer\bin\Release\SqlServer_TpfSummerizer.sql  %dest_root%\Database\SqlServer_CLRx.sql

