==========================================
Animal Movements Installation Instructions
==========================================

.. contents:: Table of Contents
   :depth: 2

Prerequisites and Assumptions
=============================
MSSMS = Microsoft SQL Server Management Studio

Install SqlServer
=================

Create Instance Logons
----------------------

Set Configuration Options
-------------------------


Create Animal Movements Database
================================

Create The Database Files
-------------------------

Copy and edit the file ``{installdir}\Database\CreateDatabase.sql``.
The edits must include:

1. If you already have a database called ``Animal_Movement`` or you would like to use
   a different name, the do a global search and replace on ``Animal_Movement``.
2. Ensure that the Name and path of the mdf and log files are valid and appropriate
   (lines 5 and 7)

Open and run the file in MSSMS with a connection to the instance where you wish to create
the database.
 
Create The Empty Schema
-----------------------

Create Database Users
---------------------

Populate Domains
----------------

Load CLR assemblies
-------------------

Other Initialization
--------------------


Create External Services
========================

SqlProxy Account
----------------

Install Applications
--------------------

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




