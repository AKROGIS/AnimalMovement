# Creating Database Exports using SSMS

## `CreateDatabase.sql`

This create a script to create a new empty database object.
It only needs to be done once.

* Right click on `Animal_Movement` database,
  select `Script Database as -> CREATE To -> File...`
* Make sure it is saved as ANSI, not unicode. Microsoft save unicode as
  UTF-16 (not UTF-8) and git diff doesn't like UTF-16.


## `CreateDatabaseObjects.sql`

Do this when ever a database object is created, deleted, or the
definition is changed.

* Right click on `Animal_Movement` database,
  select `Tasks -> Generate Scripts...`
* Choose `Objects`
  * Select `Specific database objects`
    * Select all then unselect SQL Assemblies
* Set Scripting Options
  * Save to single ANSI text file
* Advanced (check these settings - defaults can be set in SSMS Settings)
	* `Include Descriptive Header` -> `False`
    (header includes an export date/time which causes unnecessary diffs)
	* `Script Extended Properties` -> `False`
	* `Script Object-Level Permissions` -> `True`
	* `Script Indexes` -> `True`
	* `Script Triggers` -> `True`
 

## `LookupTableData.sql`

Do this whenever the records in the lookup tables change.

* Right click on `Animal_Movement` database,
  select `Tasks -> Generate Scripts...`
* Choose `Objects`
  * Select `Specific database objects`
    * Select only and all the tables starting with `Lookup`.
* Set Scripting Options
  * Save to single ANSI text file
* Advanced
	* same as above except
	* set `Types of data to script` -> `Data Only`

## Settings

* Set personal settings in SSMS `Tools -> Options`
* under `SQL Server Object Explorer -> Scripting`
  * set `Include descriptive Headers` to `False`
  * set `Script extended properties` to `False`
  * set `script permissions` to `True`
  * set `Script indexes` to `True`
  * set `Script triggers` to `True`
