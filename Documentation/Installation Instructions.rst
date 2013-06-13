==========================================
Animal Movements Installation Instructions
==========================================

.. contents:: Table of Contents
   :depth: 2

Prerequisites and Assumptions
=============================
SSMS = Microsoft SQL Server Management Studio

Install SqlServer
=================

See the Microsoft documentation for this step.  The application was developed
on Microsoft SqlServer 2008R2 enterprise edition, but any edition, including
express should work, as well as any subsequent version.  The database uses features
new in 2008R2, so previous versions will *NOT* work.

Create Instance Logons
----------------------

This will be done during the database creation phase.  Depending on the needs of the
installation.  Since Instance logins are shared with all databases, the scripts
should be checked to ensure that the creation of existing logins is commented out

Enable CLR Integration
----------------------

To run custom code in the database, you must enable CLR (Common Language Runtime)
integration.  This is equivalent to enabling "plugins" for the database.
Open a SSMS query and enter the following text::

  sp_configure 'show advanced options', 1;
  GO
  RECONFIGURE;
  GO
  sp_configure 'clr enabled', 1
  GO
  RECONFIGURE
  GO

then press the execute button.
For more details, see `Enabling CLR Integration`_ on the Microsoft Developer Network

Create Animal Movements Database
================================

The scripts are written to create a database called ``Animal_Movement``.  If you already
have a database with this name or you would like to use a different name, then you will
need to edit each of the files as described below.

Create The Database Files
-------------------------

Copy and edit the file ``{installdir}\Database\CreateDatabase.sql``.
The edits must include:

1. If you already have a database called ``Animal_Movement`` or you would like to use
   a different name, the do a global search and replace on ``Animal_Movement``.
   
2. Ensure that the Name and path of the mdf and log files are valid and appropriate
   (lines 5 and 7)

Open and run the file in SSMS with a connection to the instance where you wish to create
the database.
 
Load CLR assemblies
-------------------

If you have changed the name of the database, then copy and edit the tenth line of the
file ``{installdir}\Database\SqlServer_CLR.sql`` to reflect the correct name.

Open and run the preceeding files in SSMS with a connection to the instance where you
created the database.

Create The Empty Schema
-----------------------

If you have changed the name of the database, then copy and edit the first line of the
file ``{installdir}\Database\CreateDatabaseObjects.sql`` to reflect the correct name.

Open and run the file in SSMS with a connection to the instance where you wish to create
the database.

Create Database Users
---------------------

Open the file ``{installdir}\Database\CreateUsers.sql`` in in SSMS with a connection to
the instance where you wish to create the database.  Turn on ``SQLCMD Mode`` in the Query
menu of SSMS.  Edit the 10th and 11th lines to set the name of the domain group that
has viewing permissions, and the name of the database (if you have changed it)
respectively. Then execute the query.

Adding the Automation User
++++++++++++++++++++++++++

If you want to use an automated process to automatically download Argos data, or
process Argos emails for users that do not have the Telonics Data Convertor (TDC) on their
computer, then you will need to add the automation user.

You will need the create a local windows account on the database server.  See the
section `Automation Account`_ for details.

Open the file ``{installdir}\Database\CreateAutomationUser.sql`` in in SSMS with a
connection to the instance where you wish to create the database.  Turn on
``SQLCMD Mode`` in the Query menu of SSMS.  Edit the 6th line to reflect the server
where the database is installed. Edit the 7th line to reflect the name of the automation
account created on that server. Edit the 8th line to reflect the password of the
automation account.  Edit the 9th line to reflect the name of the database
(if you have changed it).  Then execute the query.


Create Project Investigators
++++++++++++++++++++++++++++

In the Object Explorer in SSMS browse to the server, then 
``Databases -> Animal_Movement -> Programmability -> Stored Procedures``.
Right click on ``ProjectInvestigator_Insert_SA`` and select
``Execute Stored Procedure...`` from the pop up menu.  Fill in the information for a
project investigator.  The first parameter (``@Login``) is the users network/database
login name with the domain  i.e. ``NPS\RESarwas``.  The stored procedure will ensure
that the user has a database login.  A project investigator is a database
user that can create and manage projects and collars.  They can also enable other database
users to do editing on their behalf.  Only project investigators (and their editors) have
permission to make changes in the database. Run the stored procedure as many times as
necessary to create all the project investigators that will be using the database.
A collar, project, and file can only be *owned* by one project investigator, so if an
item is *jointly* managed, then pick one manager as the project investigator, and make
the other an assistant.

Populate Domains
----------------

If you have changed the name of the database, then copy and edit the first line of the
file ``{installdir}\Database\LookupTableData.sql`` to reflect the correct name.

You will also need to edit the 11th line to set the correct name of the server and the
database.  Line 11 can be copied multiple times for each replication server you will
set up.  This table can be edited later.  This table is used to provide the users with
the ability to specify the server to query in the ArcMap layer files created with these
tools.  The users should select the replication (or master) server that is closest to them
for optimal performance.

Open and run the file in SSMS with a connection to the instance where you wish to create
the database.

Settings Table
--------------
Open the file ``{installdir}\Database\Settings.sql`` in SSMS with a connection to
the instance where you wish to create the database.  Edit the file as follows

1. Change the database name on the first line as appropriate.
  
2. Change the value of ``dba_contact`` to reflect your (the admin/installer's)
   contact information
  
3. Change the value of ``argosProcessor`` to the path of the ArgosProcessor.exe
   file as set in `Automation Applications`_.
   If you are not using the automation account to process Argos files,
   then remove this line.
  
4. Change the value of ``sa_email`` and ``sa_email_password`` to reflect the name
   and password of the email account to be used by the automation account to email
   project investigators of problems encountered while downloading Argos data.
   See section `Optional Email Notifications`_ for more details.
   If you are not using email notifications, remove these lines.

Then execute the query.


Client Application
==================

Interaction with the Animal Movements database occurs in two distinct flavors.

1. The Animal Movements Application (``AnimalMovement.exe``) - For creating lists of
   animals, collars, and deployments
   and for uploading data files, or configuring the automatic download options.
 
2. ArcMap layer files - for viewing animal locations, and movement vectors.
 
Installing the Animal Movements Application
-------------------------------------------
Copy all the files from ``{installdir}\Client`` to some local or network drive.
The application can run from any folder and does not need any special administrative
permissions to be installed or configured.  All the files do need to be installed in
the same folder, so if you want a copy on your desktop you will need to create a shortcut
to AnimalMovement.exe, and not a copy.

Edit Configuration File 
+++++++++++++++++++++++
The configuration file is ``AnimalMovement.exe.config``.  If your computer settings are
hiding file extensions, then the file name will appear as ``AnimalMovement.exe``, and
the application (the file with the paw icon) ``AnimalMovement.exe`` will appear as
``AnimalMovement``. Edit, *with a text editor like notepad, not MS word*, the
connection string in this file (line 15). The line should look like::

  connectionString="Data Source=INPAKROms53ais;Initial Catalog=Animal_Movement;Integrated Security=True"

Change ``INPAKROms53ais`` to reflect the name of the SqlServer instance where the database
is installed.  By default this is the server name of the machine where SqlServer is
installed.  Change ``Animal_Movement`` to reflect the name of the database if you have
changed it.

If you have TDC (Telonics Data Convertor) installed and autorized on your computer you can
use your local copy to process files files when uploading (as an alternative to setting
up an automation account on the server to do the processing).  The Setting looks
like (starting on line 44)::

        <setting name="TdcPathToExecutable" serializeAs="String">
          <value>C:\Program Files (x86)\Telonics\Data Converter\TDC.exe</value>
        </setting>

Edit the path to reflect the location of TDC on your machine.  If you do not have TDC on
your computer or it is installed at a different location and you do not change this
setting then Argos and direct download files will be processed on the server (if you set
up the automation account), or left unprocessed (i.e. there will be no fixes/locations
derived from those files).

Animal Movements was developed and tested with TDC version 2.02, with default settings for
formating dates and lat/long.  It is possible that different versions and/or different
settings may result in unexpected behavior.

There are numerous other options in this file which can be edited, however the defaults
are suitable for most installations.

Other Config Files
++++++++++++++++++
You can also edit ``InvestigatorReports.xml`` and ``ProjectReports.xml`` to add or remove
quality control queries to suit your tastes.  If the program is installed in a network
location, then these changes will be visible to all users.  If you want to make changes
for just yourself, then make a copy of the entire folder to a private location, and edit
and run your copy.

Installing ArcMap
-----------------

See the ESRI documentation for installation instruction of ArcMap.
The Animal Movements tools require ArcMap 10.0 or higher with only a ArcView license.
No additional configuration of ArcMap is required to view animal movement data.

Use the *Create Map File* button on the Animal Movements
Application will create a 10.1 layer file to your specifications.

Alternatively, you can use the Query Layer feature of ArcMap 10.0 or higher
(from the menu select ``File -> Add Data -> Add Query Layer...``).
See the online help for `Query Layers`_ for more information.
This option requires experience with SQL and an understanding of
the database schema, but provides the most flexibility, power and
efficiency.  


Create External Services
========================

Automation Account
------------------

Automation Applications
-----------------------
Install, configure and authorize TDC
Configure Gmail (optional)

Edit Configuration Files
------------------------

Set Schedule for Services
-------------------------



Optional Email Notifications
============================

Create Gmail Account
--------------------

Add Account Information to Database
-----------------------------------

Set Argos Downloader Configuration
----------------------------------


Configuring Replication
=======================

Configure the Master
--------------------

Configure the Slave(s)
----------------------


Configuration Files
===================

AnimalMovement.exe.config
-------------------------
This is the configuration file for the windows application most commonly used by end users.
It contains settings for `connectionStrings`_,
`DataModel.Properties.Settings`_, `FileLibrary.Properties.Settings`_ and
`Telonics.Properties.Settings`_.  See those sections for more details.
The file also contains a copy of the default user settings
(typically size and location of the windows on the screen).

CollarFileLoader.exe.config
---------------------------
This is the configuration file for the command line application which may be used by some
power users to bulk load collar files.
It contains settings for `connectionStrings`_, `DataModel.Properties.Settings`_,
`FileLibrary.Properties.Settings`_ and `Telonics.Properties.Settings`_.
See those sections for more details.

ArgosDownloader.exe.config
--------------------------
This is the configuration file for the command line application that is used by the
automation user to download Argos Program/Platforms
It contains settings for `connectionStrings`_, `DataModel.Properties.Settings`_,
`ArgosDownloader.Properties.Settings`_,
`FileLibrary.Properties.Settings`_ and `Telonics.Properties.Settings`_.
See those sections for more details.

ArgosProcessor.exe.config
-------------------------
This is the configuration file for the command line application that is used by the
automation user to process un-processed Argos files.
It contains settings for `connectionStrings`_, `DataModel.Properties.Settings`_,
`FileLibrary.Properties.Settings`_ and `Telonics.Properties.Settings`_.
See those sections for more details.

connectionStrings
-----------------
The default connection sting configuration settings look like::

  <connectionStrings>
    <add name="DataModel.Properties.Settings.Animal_MovementConnectionString"
        connectionString="Data Source=INPAKROMS53AIS;Initial Catalog=Animal_Movement;Integrated Security=True"
        providerName="System.Data.SqlClient" />
  </connectionStrings>

The text ``INPAKROMS53AIS`` must be replaced with the SqlServer Instance name (typically the name of the
machine where a default instance of Sql Server is installed).  If more than one instance of SqlServer is
installed on a machine, then the text must include tha machine and instance name.

The text ``Animal_Movement`` must be replaced with the name of the database in the instance where
the animal movement schema has been created.

ArgosDownloader.Properties.Settings
-----------------------------------
Settings that control the Argos downloader library.  This library is used in multiple executables, and each executable
has a copy of these settings, so the defaults may be vary.

============================  ===================  ====================================================================================
Setting                       Default              Valid Values
============================  ===================  ====================================================================================
LogFile                       ArgosDownloader.log  Any valid filename, the file will be created or appended to in the folder where
                                                   the executable is started.
MailServer                    smtp.gmail.com       The domain name of the mail server, must be consistent with the email address
                                                   used in the settings table.
MailServerPort                587                  Port used to connect to the mail server.  This is defined by the mail server.
MailServerMilliSecondTimeout  20000                The time in milliseconds to wait for a mail server to respond to our request
                                                   before giving up.
============================  ===================  ====================================================================================

DataModel.Properties.Settings
-----------------------------
Settings that control the database connection library.  This library is used in multiple executables, and each executable
has a copy of these settings, so the defaults may be vary. 
 
===================  =======  ====================================================================================
Setting              Default  Valid Values
===================  =======  ====================================================================================
CommandTimeout       300      A valid positive integer.
                              The time in seconds to wait for a SqlServer request to complete before giving up.
                              This time should take into consideration the time the command takes on
                              the server, as well as the time for the network to send the request and receive the
                              results
===================  =======  ====================================================================================

FileLibrary.Properties.Settings
-------------------------------
Settings that control the Argos file processing library.  This library is used in multiple executables, and each executable
has a copy of these settings, so the defaults may be vary.

========================  ===================  ====================================================================================
Setting                   Default              Valid Values
========================  ===================  ====================================================================================
FileProcessorLogFilePath  ArgosProcessor.log   Any valid filename, the file will be created or appended to in the folder where
                                               executable is started.
LogMessagesToConsole      False                True or False - Should the processor write messages in the console screen?
LogMessagesToLogFile      True                 True or False - Should the processor write messages in the log file?
LogErrorsToConsole        False                True or False - Should the processor write errors in the console screen?
LogErrorsToLogFile        True                 True or False - Should the processor write errors in the log file?
========================  ===================  ====================================================================================

Telonics.Properties.Settings
----------------------------
Settings that control the Telonics library.  This library is used in multiple executables, and each executable
has a copy of these settings, so the defaults may be vary.

============================  =======================================================================  =====================================================
Setting                       Default                                                                  Valid Values
============================  =======================================================================  =====================================================
TdcPathToExecutable           C:\\Program Files (x86)\\
                              Telonics\\Data Converter\\TDC.exe                                        A valid file path to the TDC executable
TdcMillisecondTimeout         20000                                                                    Any positive integer.  The number of milliseconds to
                                                                                                       to wait the TDC application to yield a result before
                                                                                                       giving up.  Default is 20 seconds.
TdcArgosBatchFileFormat       ::

                              <BatchSettings>                                                          The TDC batch file template for
                              <ArgosFile>{0}</ArgosFile>                                               processing Argos email/web files
                              <ParameterFile>{1}</ParameterFile>                                       See the TDC documentation for
                              <OutputFolder>{2}</OutputFolder>                                         a discusion of the format of this
                              <BatchLog>{3}</BatchLog>                                                 file.  {0} to {4} will be replaced
                              <MoveFiles>false</MoveFiles>                                             the appropriate file/folder name
                              <GoogleEarth>false</GoogleEarth>                                         when the file is created.
                              </BatchSettings>
TdcDatalogBatchFileFormat     ::

                              <BatchSettings>                                                          The TDC batch file template for
                              <DatalogFile>{0}</DatalogFile>                                           datalog (direct download) files
                              <OutputFolder>{1}</OutputFolder>                                         See the TDC documentation for
                              <BatchLog>{2}</BatchLog>                                                 a discusion of the format of this
                              <MoveFiles>false</MoveFiles>                                             file.  {0} to {4} will be replaced
                              <GoogleEarth>false</GoogleEarth>                                         the appropriate file/folder name
                              </BatchSettings>                                                         when the file is created.
ArgosServerMinDownloadDays    1                                                                        An integer between 0 and
                                                                                                       ArgosServerMaxDownloadDays.  The user
                                                                                                       must provide a value in this range.
ArgosServerMaxDownloadDays    10                                                                       Max number of days available for
                                                                                                       download (set by the Argos service)
ArgosUrl                      http://ws-argos.clsamerica.com/argosDws/services/DixService              The URL of the Argos web service.
ArgosPlatformSoapRequest      ::

                               <soap:Envelope xmlns:soap=""http://www.w3.org/2003/05/soap-envelope""   Message to send to the Argos web
                               xmlns:argos=""http://service.dataxmldistribution.argos.cls.fr/types"">  server to request data for a platform.
                               <soap:Header/>                                                          {0} to {3} will be replaced by the
                               <soap:Body>                                                             appropriate values before the file is
                               <argos:csvRequest>                                                      sent to the web server.  See Argos
                               <argos:username>{0}</argos:username>                                    website for details on the web service
                               <argos:password>{1}</argos:password>                                    request protocol.  Changing the request
                               <argos:platformId>{2}</argos:platformId>                                may cause the results file to  be
                               <argos:nbDaysFromNow>{3}</argos:nbDaysFromNow>                          un-recognizable by the database.
                               <argos:displayLocation>true</argos:displayLocation>
                               <argos:displayDiagnostic>true</argos:displayDiagnostic>
                               <argos:displayRawData>true</argos:displayRawData>
                               <argos:displayImageLocation>true</argos:displayImageLocation>
                               <argos:displayHexId>true</argos:displayHexId>
                               <argos:showHeader>true</argos:showHeader>
                               </argos:csvRequest>
                               </soap:Body>
                               </soap:Envelope>
ArgosProgramSoapRequest       ::

                               <soap:Envelope xmlns:soap=""http://www.w3.org/2003/05/soap-envelope""   Message to send to the Argos web
                               xmlns:argos=""http://service.dataxmldistribution.argos.cls.fr/types"">  server to request data for a program.
                               <soap:Header/>                                                          {0} to {3} will be replaced by the
                               <soap:Body>                                                             appropriate values before the file is
                               <argos:csvRequest>                                                      sent to the web server.  See Argos
                               <argos:username>{0}</argos:username>                                    website for details on the web service
                               <argos:password>{1}</argos:password>                                    request protocol.  Changing the request
                               <argos:programNumber>{2}</argos:programNumber>                          may cause the results file to  be
                               <argos:nbDaysFromNow>{3}</argos:nbDaysFromNow>                          un-recognizable by the database.
                               <argos:displayLocation>true</argos:displayLocation>
                               <argos:displayDiagnostic>true</argos:displayDiagnostic>
                               <argos:displayRawData>true</argos:displayRawData>
                               <argos:displayImageLocation>true</argos:displayImageLocation>
                               <argos:displayHexId>true</argos:displayHexId>
                               <argos:showHeader>true</argos:showHeader>
                               </argos:csvRequest>
                               </soap:Body>
                               </soap:Envelope>
ArgosPlatformListSoapRequest  ::

                               <soap:Envelope xmlns:soap=""http://www.w3.org/2003/05/soap-envelope""   Message to send to the Argos web
                               xmlns:argos=""http://service.dataxmldistribution.argos.cls.fr/types"">  server to request a list of programs
                               <soap:Header/>                                                          and platforms for a given user.
                               <soap:Body>                                                             {0} and {1} will be replaced by the
                               <argos:platformListRequest>                                             appropriate values before the file is
                               <argos:username>{0}</argos:username>                                    sent to the web server.  See Argos
                               <argos:password>{1}</argos:password>                                    website for details on the web service
                               </argos:platformListRequest>                                            request protocol.
                               </soap:Body>
                               </soap:Envelope>
============================  =======================================================================  =====================================================

.. _`Query Layers`: http://resources.arcgis.com/en/help/main/10.1/index.html#//00s50000000n000000 
.. _`Enabling CLR Integration`: http://msdn.microsoft.com/en-us/library/ms131048(v=SQL.105).aspx
