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

Create Instance Logons
----------------------

This will be done during the database creation phase.  Depending on the needs of the
installation.  Since Instance logins are shared with all databases, the scripts
should be checked to ensure that the creation of existing logins is commented out

Set Configuration Options
-------------------------

Loading CLR, XP Command Mode
**FIXME - ADD XPCMD stuff to the AutomationUser script **

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
menu of SSMS.  Edit the lines 10th and 11th lines to set the name of the domain group that
has viewing permissions, and the name of the database (if you have changed it) respectively.
Then execute the query.

If you want to will be using an automated process to automatically download Argos data, or
process Argos emails for users that do not have the Telonics Data Convertor on their computer,
then you will need to add the automation user.

Adding the Automation User
++++++++++++++++++++++++++

Create the windows account on the server with the database.
**FIXME expand** 

Open the file ``{installdir}\Database\CreateAutomationUser.sql`` in in SSMS with a connection to
the instance where you wish to create the database.  Turn on ``SQLCMD Mode`` in the Query
menu of SSMS.  Edit the 6th line to reflect the server where the database is installed.
Edit the 7th line to reflect the name of the automation account created on that server.
Edit the 8th line to reflect the name of the database (if you have changed it).  Then
execute the query.

Create Project Investigators
++++++++++++++++++++++++++++

In SSMS expand the new animal movements database.  Expand the Programmability and Stored
Procedures section.  Right click on ``ProjectInvestigator_Insert_SA`` and select
``Execute Stored Procedure...`` from the pop up menu.  Fill in the information for a
project investigator.  The first parameter (``@Login``) is the users network/database
user name with the domain name  i.e. 'NPS\RESarwas'.  The stored procedure will ensure
that the user has a database login.  A project investigator is a database
user that can create and manage projects and collars.  They can also enable other database
users to do editing on thier behalf.  Only project investigators (and their editors) have
permission to make changes in the database. Run the stored procedure as many times as
necessary to create all the project investigators that will be using the database.

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


Other Initialization
--------------------


Create External Services
========================

SqlProxy Account
----------------

Install Applications
--------------------
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




