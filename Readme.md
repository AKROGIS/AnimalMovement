# Animal Movements

Animal Movements is a database and a suite of tools for managing GPS location
data from collared animals.  The user can input collar data files directly
or configure the database to collect the data files from web services and
email.  The database archives the data files and converts them to tabular data
in several derived forms and associate the collar locations with animals
based on deployment information provided by the users.  The data is available
to any application that can communicate with a SQL server database.  In
particular, the data is consumed as point locations and movement vectors
 (derived from temporally adjacent point locations) in ArcGIS maps.

The bulk of the folders in this repo are Visual Studio Projects (included in
the `AnimalMovements.sln` solution file)

* `AnimalMovement` - A Windows .Net Framework 4.5 and WinForms application to
  allow users to manage the collar and animal data.
* `ArcMap_Addin` - An ArcMap Add-In for allowing users to flag locations as
  invalid while viewing the data in ArcMap.
* `ArgosDownloader` - A command line tool intended to be run as a scheduled
  task to download collar data files from the Argos web service and then upload
  and process the files.
* `ArgosProcessor` - A command line tool intended to be run as a scheduled
  task to process Telonics data manually uploaded to the database by users.
* `CollarFileLoader` - A command line tool to upload collar files. Intended for
  bulk uploading historical data or data downloaded manually from a collar.
* `DataModel` - A *Linq to SQL* library mapping C# objects to SQL data
  tables.  Used by most of the other projects.
* `FileLibrary` - A library providing tools for downloading, uploading and
  processing collar files.  Used by most of the other tools.
* `IridiumDownloader` - A command line tool intended to be run as a scheduled
  task to download Telonics collar data files from Iridium email messages and
  then upload and process the files.
* `QueryLayerBuilder` - A command line tool for building an ArcGIS layer file
  which dynamically queries the database based on query parameters provided
  by the user.
* `SqlServer_Files` - A SQL Server CLR library providing custom file processing
  functions for the database.
* `SqlServer_Functions` - A SQL Server CLR library providing date and hashing
  functions for the database.
* `SqlServer_Parsers` - A SQL Server CLR library providing file parsing
  functions for the database.
* `SqlServer_TpfSummerizer` - A SQL Server CLR library providing functions for
  summarizing Telonics Parameter Files in the database.  Duplicates code in
  the `Telonics` library to avoid assembly dependencies in SQL Server.
* `Telonics` - A library of tools to process Telonics data and parameter files.
* `TelonicsTest` - A command line tool for testing the `Telonics` library.
* `TpfFilesSummerizer` - A command line tool for testing the functionality for
  summarizing Telonics Parameter Files in the `Telonics` library.
* `VectronicDownloader` - A command line tool intended to be run as a scheduled
  task to download collar data files from the Vectronic web service and then
  upload and process the files.

The other folders are
* `Database` - SQL scripts to create, test and query the database.
* `Distribution` - Tools to assist with deploying the Animal Movements system.
* `Documentation` - Information for developers, administrators, and users.
* `LayerFiles` - ArcGIS 10.x layer files with query layers to the animal
  movements data.  See the readme file for details.
* `Toolbox` - An ArcGIS toolbox and Python scripts for analyzing the movement
  data with ArcGIS.

## Build

See [Documentation/Build Instructions.rst](Documentation/Build%20Instructions.rst)

## Deploy

See [Documentation/Installation Instructions.rst](Documentation/Installation%20Instructions.rst)

## Using

See [Documentation/UserGuide.rst](Documentation/UserGuide.rst)
