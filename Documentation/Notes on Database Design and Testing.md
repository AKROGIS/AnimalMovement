# Publicly editable tables

**NONE** - All interaction is through Stored procedures. This is necessary to
maintain the project level permissions without creating project level users.

# Stored procedures vs. Triggers

A public stored procedure standardizes the insert/update/delete operations on
the underlying tables for clients.  It can do any of the following

1) Hides columns the clients should not muck with,
2) Provides optional default values.  Mandatory default values (i.e. a
   timestamp) are implemented with constraints, and the hidden from the client
   on insert and update.
3) Validates execute permission when the rights are dependent on the state of
   the database.
4) Groups a number of operations that should be all or nothing from the clients
   perspective, but which could be executed non-atomically by the SA without
   loss of validity.

A trigger is responsible for verifying and enforcing any business rules that
cannot be expressed with Keys and Constraints.  For example, a collar cannot be
on two animals at the same time.  Note that the SPROCs are required for clients,
but not the SA.  The SA can issues DML directly against the tables, without fear
of violating integrity of the relationships amongst the database objects.


## More on Stored Procedures

For *Linq to SQL*, there is one and only one SPROC that is assigned to a table
for each of the insert, update, and delete operations.  This isn't a problem
with insert and delete, but it requires updates to be an all or nothing
operation. *Linq to SQL* handles this by sending the original values for any
column that is not being updated, but this will fire an update trigger on every
column that can be updated.  Additional SPROCs can be provided to update a
limited set of columns, but these must be called by the client directly.

To make the update SPROCs easier to use for non *Linq to SQL* clients, any
non-nullable columns are given a default value of null when the SPROC is called.
Since null is not a valid column value, the SPROC can use this to mean use the
existing value of the column. For nullable columns, the SPROC can use the same
defaults that the insert SPROC uses. The insert and update SPROCS should
generally convert empty strings to null unless the empty string has a specific
meaning. 


# Problems & known limitations

* Animals Group Name is constant
  - We can change it, but we cannot capture that it was once something, and is
    now something else. (e.g. when a juvenile wolf leaves his birth pack to join
    a different pack.)
* When bulk loading a Debevek file, it will fail unless all the collar Ids
  already exist.  If I relaxed this requirement, I would need to add a trigger
  to the Collars table to scan file data and create new fixes/locations whenever
  a new collar/animal was added to the database.  Calling code should scan the
  Debevek file, and help the user create the collar/animal/deployment records
  before loading the file.
* Adding Other Agency Data???


# Questions

## Triggers on Projects/Animals Tables?

A Project/Animal is needed for a location. By referential integrity constraints,
a Project/Animal cannot be deleted if it is used by a related table.  Similarly,
the primary key cannot be changed in such as way as to break a relationship.
Adding a Project/Animal, may warrant the creation of new location data, if there
is existing collar file data that pertains to this project/animal.  However,
the collar animal relationship is managed through the CollarDeployments table,
so monitoring that is all that is required.


## Triggers on Manufacturer/Collar Tables?

A Manufacturer/Collar is needed to create fixes from collar data files.  By
referential integrity constraints, a Manufacturer/Collar cannot be deleted if
it is used by a related table.  Similarly, the primary keys cannot be changed
in such as way as to break a relationship. Adding a Manufacturer/Collar, may
warrant the creation of new fixes/locations, if there is existing collar file
data that pertains to this Manufacturer/Collar.  This can be managed by failing
to add a file that relates to a Manufacturer/Collar that does not exist
(currently the default), or by adding triggers to Manufacturer and Collar tables
which scan all data files for related fix data.


## Constraints to CollarData{Format} Tables?

I decided no.  Each file must have a FileId and LineNumber column to act as the
primary key, beyond that the data is what the data is.  This may cause some some
records to be skipped if the data is bad, or it may abort the transaction (i.e.
referencing a collar that does not exist). Exact semantics for bad data need to
be further developed.


# Adding a new file format

* Add a record describing this file format in the table LookupCollarFileFormats

  - Use the File Format code for naming the functions in the following step
  
* Create table definition for CollarData{Format} - must have FileId and
  LineNumber primary keys

  - Be as liberal as possible regarding column types.  See 
    CollarDataTelonicsGen3StoreOnBoard for an example.
  
* Edit AnimalMovementFunctions.cs In the SqlServerExtensions Project of the
  AnimalMovements Solution

  - The other file formats should provide a good template for creating a
    table-valued function.

* Build/Deploy this project - the domain user doing the deployment must be a sa
  on the target database
* Add a record providing a unique match for this file formats header string.
* Modify the stored procedure CollarData_Insert and copy/paste the last few rows
  and modify to support the new format
* Modify the stored procedure CollarFixes_Insert and copy/paste the last few
  rows and modify to support the new format
* Write tests for the above code
* Run the tests


# Test Cases for Triggers on `CollarFixes`

## Update

* Updates are only done by stored procedures (or triggers) which enforce
  only changing HiddenBy, and enforcing HiddenBy relationship.
* Single record HiddenBy null to not null -> new location record
* Single record HiddenBy null to null -> no action
* Single record HiddenBy not null to not null -> no action
* Single record HiddenBy not null to null -> delete location record
* Multiple records HiddenBy null to not null -> multiple new location records
* Multiple records HiddenBy not null to null -> delete multiple location records

## Insert

* Insert a single non conflict (no existing record)
* Insert a single conflict (single existing record)
* Insert a single conflict (multiple existing records)
* multiple versions of the above

## Delete

* Delete single (A) hidden record that does not hide another
* Delete single (B) hidden record that hides another
* Delete single (C) unhidden record that hides another
* Delete single (D) unhidden record that does not hide another
* Delete Multiple (A) from different chains
* Delete (A) & (B) from same chain
* Delete (B) & (A) from same chain
* Delete Multiple (B) From same chain
* Delete Multiple (B) From same chain (adjacent, not adjacent, does order matter)
* Delete Multiple (B) From different chains
* Delete (B) & (C) from same chain
* Delete (C) & (B) from same chain
* Delete Multiple (C) from different chains
* Delete Multiple (D) from different chains

# To Do

*  Check - should `Collar_*` stored procedures executable by editor or PI?
   in `InsteadOfCollarFixesInsert` get the `FixId` for setting `hiddenby` to 0;
   this could speedup selects.
*  Test Transaction support for procedures marked %+
*  Test triggers for `CollarDeployment` table
*  Document/Test the add file, add data, add fixes process
*  Write queries to show conflicting Fixes
*  Write test suite to exercise all public stored procedures
*  Write test suite to exercise all triggers
*  Write test code for triggers on `CollarFixes` and `Locations`
*  Add on-line download format
*  Explore using insert triggers in each data table instead of
   `AddFixesForCollarFile`
*  Define error handling when creating fixes for bad data in the 
   `CollarData{Format}` table.
*  See Kyle/Bucks collar tables for more possible attributes 

