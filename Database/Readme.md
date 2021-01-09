
# Database Queries

This folder contains SQL queries for building, testing and querying the
database.

The queries in this folder are:

* `CreateAutomationUser.sql` -  Creates the user that will run the scheduled
  tasks for automatically downloading, uploading and processing collar files.
* `CreateDatabase.sql` - Creates the a new blank database.
* `CreateDatabaseObjects.sql` - Creates all the tables, views, functions,
  triggers, procedures, and other database objects needed by the system.
* `CreateReplicationUsers.sql` - Creates users that can be used to replicate
  the database to remote servers.
* `CreateUsers.sql` - Creates generic users and roles. 
* `LookupTableData.sql` - Populates look up tables (domains) with well known
  values.
* `Settings.sql` - Populates the Settings table with expected system settings.
* `Project Investigator Summary Queries.sql` - A list of queries that are
  provided to project investigators in the Animal Movements application.
* `Project Summary Queries.sql` - A list of queries that are provided to check
  the status of projects in the Animal Movements application.

## Reports

Reports and miscellaneous queries in `Reporting`.
Most of these queries addressed a specific one time question or task.  They
are retained here as examples in case there is a similar need in the future.

* `Additional Helpful Queries.sql` - A suite of queries to check for errors
  and/or invalid state in the database.  See the code for comments on the
  various queries. 
* `CollarReports.sql` - A collection of reports on the collars in the
  database.
* `Example - Change Animals Table Schema.sql` - An example of how to change
  the schema of an existing table while preserving the existing data and
  relations. It is up to the user to ensure that the change doesn't break
  other views, procedures, triggers, etc.
* `Export Project Collar Data Records.sql` - Extracts all of the collar file
  data for a given project.
* `Helpful Queries.sql` - A suite of queries to check for errors and/or
  invalid state in the database.  See the code for comments on the various
  queries. 
* `HideFirstLocationInZeroDistanceShortDurationMovements.sql` - A modification
  query to hide bad locations.
* `PTT Locations for Kyle.sql` - Locations of collars using the Argos
  based location method (not GPS)
* `Processing Issues Queries.sql` - 
* `Spatial Queries.sql` - A collection of queries suitable for ArcMap.
* `SpatialQueries - ShortDurationVectorsAndEndPoints.sql` - An ArcMap
  query that displays the end points and line of a unusual short duration
  between fixes (helpful for finding bogus GPS data)
* `TPF_Fix_Periods.sql` - Reports on the GPS collection schedules in the
  Telonic Parameter Files (TPF) in the database.

## Tests

Testing queries are in `Testing`
These queries were written while the database was being developed to ensure
procedures behaved properly and were performed well.

* `Check Database Permissions.sql` - Lists the permissions on database object
  in order to verify that the permissions are as expected. See the design
  documents to determine the expected permissions.
* `Check Server Permissions.sql` - Lists the server permissions.
* `Test Cases.sql` - Tests the building of movement vectors based on changes
  to the locations table.
* `Test Code.sql` - Numerous tests for the custom functions, triggers and
  procedures.  See the code for details.
* `Test Views.sql` - Simply checks that the views are not broken by issuing
  a query against the view.  Good to run this after any schema change.
* `Timing Tests for Locations.sql` - Timing of the location triggers 
  (creation of the movement vectors).  Validated that the solution was not
  horribly slow. Could be used to test alternative optimizations.
