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

Open and run the proceeding files in SSMS with a connection to the instance where you
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
process Argos emails for users that do not have the Telonics Data Converter (TDC) on their
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
   file as set in `Animal Movements Software`_.
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

If you have TDC (Telonics Data Converter) installed and authorized on your computer you can
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

There are numerous other options in the `Configuration Files`_ which can be edited,
however the defaults are suitable for most installations.


Other Configuration Files
+++++++++++++++++++++++++
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

The database relies on a OS account to run some external processes.
In particular, the Telonics Data Converters are required to convert
the Argos emails (and webservice downloads) into csv-like files
that can be processed by the database.  An OS Account can also
query the Argos Web Server at regularly scheduled intervals to check
for new data.

These instructions document setting up the Windows account, and
adding it to the database, so that these external processes will
work correctly.

These instructions are based on Windows 7 and may vary for other versions of Windows.


Automation Account
------------------
open ``Start Menu -> Control Panel -> Administrative Tools -> Computer Management``.
In the Table of contents on the left, select
``System Tools -> Local Users and Groups -> Users``.
Right click in the main window and select ``New User...``.
Fill out the form as follows:

:User name: sql_proxy
:Full name: SQL Server Proxy
:Description:
  Local account (with Minimal permissions) used by SQL Server to execute external
  processes requested by non-sysadmin accounts

:Password:
  Provide a password that meets the Group Policy requirements for the machine.
  For Alaska Region NPS, see ``T:\PROJECTS\AKR\ArcSDE Deployment\KeePassPortable`` for the
  password used.

:User must change password at next logon: Unchecked
:User cannot change password: Checked
:Password never expires: Checked
:Account is disabled: Unchecked
    
* The user name and password can vary but the values must be consistent with the
  values used in `Create Database Users`_

* Be sure that this user is not included in any groups which may elevate its permissions

The account must be configured with permissions to logon as a batch job.
This is done with
``Start Menu -> Control Panel -> Administrative Tools -> Local Security Policy``.
In the Table of contents on the left, select
``Security Settings -> Local Policies -> User Rights Assignment``.
In the main panel, scroll down to ``Log on as a batch job``.
double click on ``Log on as a batch job`` and add the new account
to the list of authorized users.

**Log on a batch job may be limited by group policies on your domain.  If so, contact
your IT staff for support.**

Automation Applications
-----------------------

Telonics Software
+++++++++++++++++

Telonics software must be installed by an administrator.

Download Telonics Software
~~~~~~~~~~~~~~~~~~~~~~~~~~
   
TDC:
  * Telonics Data Converter - for Gen 4 Argos files (email/web) and datalog (.tdf)
  * http://www.telonics.com/software/tdc.php
  * Current version: http://www.telonics.com/software/setup-TDCv2.02.exe
  * Notes: Should be installed for all users.
    Device drivers do not need to be installed on the server.

The following Telonics software is not used with this version of Animal Movements:
             
DU:
  * Download Utility for Gen2 & Gen3 GPS - for Gen3 Datalog files (.tdf)
  * http://www.telonics.com/software/du-3.php
  * Current version: http://www.telonics.com/software/DU-Setup-1.41.exe
  * Note: The username and organization is not important.
    
ADC-T03:
  * Argos data translator for Gen3 collars
  * http://www.telonics.com/software/adc-t03.php
  * Current version: http://www.telonics.com/software/ADC-T03-Setup-4.04.0011.exe
  * Notes: Should be installed for all users. 
    The username and organization is not important.


Configure Telonics Software
~~~~~~~~~~~~~~~~~~~~~~~~~~~

1. Log on with the new `Automation Account`_
   (be sure to check the domain, and use the local machine name if necessary)
2. Authorize the Telonics software.

   a. For TDC select ``About Telonics Data Convertor...`` in the ``About`` menu.
   b. Click the ``Add...`` button to enter the authorization code
   c. For Alaska Region NPS, see ``T:\PROJECTS\AKR\ArcSDE Deployment\KeePassPortable``
      for the authorization code

3. Animal Movements was written for and tested without changing the options in TDC.
   Animal Movements may not work correctly if the options are changed. 

   
Animal Movements Software
+++++++++++++++++++++++++

1. Log on with the new `Automation Account`_
   (be sure to check the domain, and use the local machine name if necessary)
2. Copy all the files from ``{installdir}\Server`` to some local folder.
   The application can run from any folder and does not need any special administrative
   permissions to be installed or configured.  All the files do need to be installed in
   the same folder.
3. Edit the configuration files `ArgosDownloader.exe.config`_
   and `ArgosProcessor.exe.config`_.
   See `Edit Configuration File`_ in the section `Client Application`_ for more details.
4. The stored procedures ``ArgosFile_Process`` and ``ArgosFile_ProcessPlatform`` have a
   default path to the ArgosProcessor application  of
   ``C:\Users\sql_proxy\ArgosProcessor.exe``.  If the executable is installed in a
   different location, be sure to set that path in the Settings table with
   Username = 'system' and Key = 'argosProcessor'.  See `Settings Table`_ for details.



Set Schedule for Services
-------------------------

Argos Downloader
++++++++++++++++
This program will never be run by the database, so it must be configured as a scheduled
task.

The following instructions are based on Windows Server 2003.  Newer systems should be
similar.

1. Open ``Control Panel -> Scheduled Tasks``
2. Double-click on ``Add Schedule Task``
3. Follow the wizard

   a. Browse to and select ``ArgosDownloader.exe``
   b. Select a period of ``daily``
   c. Select a time that has minimal activity in your location and in France (UTC +1).
      For Alaska, 8PM ADT equals 5AM in France
   d. Provide the password for the Automation User
4. Verify that the new task is added to the list of scheduled tasks.
   
See `Optional Email Notifications`_ if you want the scheduled ArgosDownloader.exe task to
send email notifications of warnings or errors to the project investigators.  If emails
are sent, the admin should check the sent email log in the account used to send the emails
for any issues.  If email notifications are not used, then the log file on the server
should be checked regularly.  Be sure this is option is turned on in the
`Configuration Files`_


Argos Processor
+++++++++++++++
When changes are made to the database (typically uploading a file, but also adding or
changing the Argos Id assigned to a collar), the database will try to reprocess the
file, by calling ArgosProcessor.exe with the id of the file (and argos id) to be
reprocessed.  However in some cases, the external command will not run correctly
(the details and solution to this problem have not been resolved).

It is a good idea to schedule the ArgosProcessor.exe to run on a regular schedule.
if it is run with no arguments, then it will query the database for any outstanding
processing that is required and execute accordingly.  In this way it acts as a backup
in case the processing initiated by the database fails for any reason.

The set up is the same as the Argos Downloader, except:

  1. As the last step in the wizard, check the box to open the advanced options
  2. In the advanced options, select the ``Schedule`` tab
  3. Click the ``Advanced...`` button
  4. Check the ``Repeat Task`` section
  5. Have the task repeat every 10 minutes for 24 hours.   You can adjust 10 minutes up
     or down.  The longer you make the time, the longer users might have to wait to 
     see the results of changes to the database.  Making the time shorter will increase
     the work the server does to wake up and make the check, often to find out there is
     nothing more to do.
 

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

The database design is based on a single master database that all clients can connect to
add and edit thier animal movement data.  The visualization of this data in GIS will
require significant bandwidth to transfer the requested locations to a remote
client.  In cases where the network does not provide a acceptable experience for GIS,
the database can be replicated to a server closer to the GIS client.

The replication strategy is  `Transactional Replication`_ in which a snapshot of the
database is copied from master (publisher) to client (subscriber).  The publisher then
sends a copy of each transaction that occurs on the publisher to the each subscriber,
so that the subscriber dtabases can be kept in sync with the publisher in near real time.
It is not necessary to publish the entire database, in fact we will only publish the table
necessary for GIS visualization.  Other database information must be obtained by
connecting to the master database.

Setting up the publisher and subscriber databases for transactional replication is
clearly described in the `Microsoft Replication Tutorial`_.  The documentation that
is provided here is mearly a summary for those who are already familar with the process.

Any version os SQL Server except Express or Compact 3.5 SP2 can be a publisher.
Any version of SQL Server except SQL Server Compact 3.5 SP2 can be a subscriber.
Replication is installed by default in all versions except SQL Server Express.

* These instructions assume the user executing them has SA permissions on both the
publisher and subscriber SQL server instances*

Create Windows Accounts on the Master (Publisher)
-------------------------------------------------

1. Snapshot Agent:  <machine_name>\repl_snapshot

2. Log Reader Agent: <machine_name>\repl_logreader

3. Distribution Agent: <machine_name>\repl_distribution

4. Merge Agent: <machine_name>\repl_merge


Create Windows Accounts on the Client (Subscriber)
-------------------------------------------------

1. Distribution Agent: <machine_name>\repl_distribution

2. Merge Agent: <machine_name>\repl_merge

Note that these accounts must have the same name and password as the
equivalent accounts on the publication server (except the machine name
of course).


Configure the Master (Publisher)
--------------------------------

1. Prepare the Snapshot Folder

	Create a folder named repldata at a convenient location on the server.  Share it
	with repl_snapshot (full control), repl_distribution (read), and repl_merge (read)

2. Configure the Distribution

	a. In SqlServer, expand Replication and click Configure Distribution
	b. select the server as it's own distributor (this will create a distribtuion database
	on the publisher.
	c. enter \\<publisher machine name>\repldata (as created above) as the snapshot folder
	d. Set up permissions on the published database
		1. in SSMS add login for repl_snapshot and add this login as a user and *db_owner*
		in animal_movements and distribution databases.
		2. repeat for repl_logreader
		3. add login for repl_distribution as a user and *db_owner* in the distribution db
		4. add login for repl_merge as a user in the distribution db
	
3. Create a publication and select the articles for publishing

	a. in SSMS, right click on Replication->Local Publications and select New Publication
	b. Select Animal Movements as a Transactional publication
	c. In the Articles page select:
		All the tables/views/functions that will be available on the client
	d. Select Create Snapshot and keep available
	e. On the security page, provide the name and password of the repl_snapshot account
	e. repeat for the repl_logreader account
	f. finish by giving the publication a name.

4. Create a subscription to the publication

	a. In SSMS right click on the publication just created and select New Subscription
	b. Select Run all agents at the distributor
	c. On the subscriber page, connect to the subscriber server was an SA account
	d. provide a name for the new database to create on the subscriber
	e. enter repl_distribution and the password as the account for the security agent
	f. Add repl_distribution as a user and DB_owner on the new database on the subscriber

Configure the Client (Subscriber)
---------------------------------

1. Connect to the subscriber instance/database, and add repl_distribution as a db user
and memeber of *db_owner*

2. Add any other users ie. domain users who need read access to the database.


 

Configuration Files
===================

AnimalMovement.exe.config
-------------------------
This is the configuration file for the windows application most commonly used by end
users. It contains settings for `connectionStrings`_,
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

The text ``INPAKROMS53AIS`` must be replaced with the SqlServer Instance name (typically
the name of the machine where a default instance of Sql Server is installed).  If more
than one instance of SqlServer is installed on a machine, then the text must include the
machine and instance name.

The text ``Animal_Movement`` must be replaced with the name of the database in the
instance where the animal movement schema has been created.

ArgosDownloader.Properties.Settings
-----------------------------------
Settings that control the Argos downloader library.  This library is used in multiple
executables, and each executable has a copy of these settings, so the defaults may vary.

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
Settings that control the database connection library.  This library is used in multiple
executables, and each executable has a copy of these settings, so the defaults may vary. 
 
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
Settings that control the Argos file processing library.  This library is used in
multiple executables, and each executable has a copy of these settings, so the defaults
may vary.

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
Settings that control the Telonics library.  This library is used in multiple
executables, and each executable has a copy of these settings, so the defaults
may vary.

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
                              <OutputFolder>{2}</OutputFolder>                                         a discussion of the format of this
                              <BatchLog>{3}</BatchLog>                                                 file.  {0} to {4} will be replaced
                              <MoveFiles>false</MoveFiles>                                             the appropriate file/folder name
                              <GoogleEarth>false</GoogleEarth>                                         when the file is created.
                              </BatchSettings>
TdcDatalogBatchFileFormat     ::

                              <BatchSettings>                                                          The TDC batch file template for
                              <DatalogFile>{0}</DatalogFile>                                           datalog (direct download) files
                              <OutputFolder>{1}</OutputFolder>                                         See the TDC documentation for
                              <BatchLog>{2}</BatchLog>                                                 a discussion of the format of this
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
.. _`Transactional Replication`: http://technet.microsoft.com/en-us/library/ms151706(v=sql.105).aspx
.. _`Transaction Documentation`: http://technet.microsoft.com/en-us/library/ms151198(v=sql.105).aspx
.. _`Microsoft Replication Tutorial` http://technet.microsoft.com/en-us/library/bb500344(v=sql.105).aspx

