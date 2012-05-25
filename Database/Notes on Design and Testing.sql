/*
To Do
=====
  Check - should Collar_* stored procedures executable by editor or PI?
  in InsteadOfCollarFixesInsert get the FixId for settting hiddenby to 0; this could speedup selects.
  Test Transaction support for procedures marked %+
  Test triggers for CollarDeployment table
  Document/Test the add file, add data, add fixes process
  Write queries to show conflicting Fixes
  Write test suite to exercise all public stored procedures
  Write test suite to exercise all triggers
  Write test code for triggers on CollarFixes/Locations
  Add on-line download format
  Explore using insert triggers in each data table instead of AddFixesForCollarFile
  Define error handling when creating fixes for bad data in the CollarData{Format} table.
  See Kyle/Bucks collar tables for more possible attributes 
*/

/*
Publicly editable tables
========================
  NONE - All interaction is through Stored procedures,
  this is necessary to maintain the project level permissions
  without creating project level users
*/

/*
Public Stored Procedures
========================
 InsertAnimal
 DeleteAnimal
 UpdateAnimal 
 
 InsertCollar
 DeleteCollar
 UpdateCollar
 
 InsertCollarDeployments
 DeleteCollarDeployments
 UpdateCollarDeploymentsRetrievalDate
 
 InsertCollarFile %+
 DeleteCollarFile %+
 UpdateCollarFileStatus %+
 
 InsertEditor %+
 DeleteEditor %+
 
 InsertProject
 DeleteProject
 UpdateProject
 
 UpdateCollarFixesUnHideFix %+
 UpdateLocatonStatus
 UpdateSettings %+
*/


/*
Problems & known limitations
============================
  Animals Group Name is constant
    - We can change it, but we cannot capture that it was once something, and is now something else.
    - (e.g. when a juvenial wolf leaves his birth pack to join another)
  When bulk loading a debevek file, it will fail unless all the collar Ids
    already exist.  If I relaxed this requirement, I would need to add a trigger to the Collars
    table to scan file data and create new fixes/locations whenever a new collar/animal was added
    to the database.  Calling code should scan the debevek file, and help the user create the 
    collar/animal/deployment records before loading the file
  Adding Other Agency Data???
*/

/*
Triggers on Projects/Animals Tables?
==================================-=
A Project/Animal is needed for a location.  By referential integrity constraints, a Project/Animal
cannot be deleted if it is used by a related table.  Similarly, the primary key cannot be changed
in such as way as to break a relationship.
Adding a Project/Animal, may warrant the creation of new location data, if there is existing collar file
data that pertains to this project/animal.  However, the collar animal relationship is managed through
the CollarDeployments table, so monitoring that is all that is required.

Triggers on Mfgr/Collar Tables?
===============================
A Mfgr/Collar is needed to create fixes from collar data files.  By referential integrity constraints, a Mfgr/Collar
cannot be deleted if it is used by a related table.  Similarly, the primary keys cannot be changed
in such as way as to break a relationship.
Adding a Mfgr/Collar, may warrant the creation of new fixes/locations, if there is existing collar file
data that pertains to this Mfgr/Collar.  This can be managed by failing to add a file that relates
to a mfgr/collar that does not exist (currently the default), or by adding triggers to mfgr and collar tables
which scan all data files for related fix data.

Constraints to CollarData{Format} Tables?
=========================================
I decided no.  Each file must have a FileId and LineNumber column to act as the primary key, beyond that
the data is what the data is.  This may cause some some records to be skipped if the data is bad, or
it may abort the transaction (i.e. referencing a collar that does not exist).
Exact semanitics for bad data need to be further developed.

*/

/*
Adding a new file format
========================
 Add a record describing this file format in the table LookupCollarFileFormats
	Use the FileFormat code for naming the functions in the following step
 Create table definition for CollarData{Format} - must have FileId and LineNumber primary keys
	Be as liberal as possible regarding column types.  See CollarDataTelonicsStoreOnBoard for an example.
 Edit AnimalMovementFunctions.cs In the SqlServerExtensions Project of the AnimalMovements Solution
    The other file formats should provide a good template for creating a table-valued funtion
 Build/Deploy this project - the domain user doing the deployment must be a sa on the target database
 Add a record providing a unique match for this file formats header string.
 Modify the stored procedure CollarData_Insert and copy/paste the last few rows and modify to support the new format
 Modify the stored procedure CollarFixes_Insert and copy/paste the last few rows and modify to support the new format
 Write tests for the above code
 Run the tests
*/


/*
Test Cases for Triggers on CollarFixes
======================================
 Update
   ** Updates are only done by stored procedures (or triggers) which enforce
   ** only changing HiddenBy, and enforcing HiddenBy relationship.
   Single record HiddenBy null to not null -> new location record
   Single record HiddenBy null to null -> no action
   Single record HiddenBy not null to not null -> no action
   Single record HiddenBy not null to null -> delete location record
   Multiple records HiddenBy null to not null -> multiple new location records
   Multiple records HiddenBy not null to null -> delete multiple location records
 Insert
  Insert a single non conflict (no existing record)
  Insert a single conflict (single existing record)
  Insert a single conflict (multiple existing records)
  multiple versions of the above
 Delete
  Delete single (A) hidden record that does not hide another
  Delete single (B) hidden record that hides another
  Delete single (C) unhidden record that hides another
  Delete single (D) unhidden record that does not hide another
  Delete Multiple (A) from different chains
  Delete (A) & (B) from same chain
  Delete (B) & (A) from same chain
  Delete Multiple (B) From same chain
  Delete Multiple (B) From same chain (adjacent, not adjacent, does order matter)
  Delete Multiple (B) From different chains
  Delete (B) & (C) from same chain
  Delete (C) & (B) from same chain
  Delete Multiple (C) from different chains
  Delete Multiple (D) from different chains
*/

--delete from CollarFixes
--select FixId, HiddenBy From CollarFixes
--delete from Locations
--insert into CollarFixes (FileId, CollarManufacturer, CollarId, FixDate, Lat, Lon)
--VALUES ( 5, 'telonics', '96006', '2001-01-01 00:00', 60.1234, -145.1234)

/*
insert into CollarFixes (FileId, CollarManufacturer, CollarId, FixDate, Lat, Lon)
VALUES ( 5, 'telonics', '96006', '2012-01-01 00:00', 60.1234, -145.1234),
       ( 5, 'telonics', '96006', '2012-01-01 08:00', 60.1244, -145.1244),
	   ( 5, 'telonics', '96006', '2012-01-01 00:00', 60.1254, -145.1254),
	   ( 5, 'telonics', '96006', '2012-01-01 00:00', 60.1264, -145.1264),
	   ( 5, 'telonics', '96006', '2012-01-01 00:00', 60.1264, -145.1264)
*/
--UPDATE CollarFixes Set HiddenBy = NULL where HiddenBy = 0

--select FixId, HiddenBy From CollarFixes
--Select * FROM Locations

--Delete From CollarFixes WHERE FixId in (27601, 27602);
--Delete From CollarFixes WHERE FixId = 27566;
--Delete From CollarFixes WHERE FixId = 27567;
--Update CollarFixes Set HiddenBy=27583 where FixId = 27580
--Update CollarFixes Set HiddenBy=27580 where FixId = 27582

--select FixId, HiddenBy From CollarFixes
--Select * FROM Locations



/*
SELECT * FROM CollarFiles
--select * from CollarDeployments where CollarId = '96006'
--select * from CollarDataDebevekFormat
select * from CollarDataTelonicsStoreOnBoard
select * from Collarfixes
select ProjectId, AnimalId, FixDate, Location.Lat, Location.Long, SourceFile, [Status] from Locations
select * from Movements
*/

--delete from CollarFiles where FileId = 2
--EXEC dbo.DeleteCollarFile 2
--EXEC dbo.DeleteFixesForCollarFile 5
--EXEC dbo.AddFixesForCollarFile 5
--EXEC dbo.UpdateCollarFileStatus 5, 'I'
--EXEC dbo.UpdateCollarFileStatus 5, 'A'
--EXEC dbo.UpdateCollarFileStatus 6, 'I'
--EXEC dbo.UpdateCollarFileStatus 6, 'A'

--update CollarFixes set HiddenBy = 27 where FixId = 25
--DELETE FROM CollarFixes --WHERE FixDate = '2012-01-01 00:00'
--DELETE FROM CollarFixes WHERE FixId < 43
--Delete from Locations

--update Locations set Status = Null where FixDate = '2012-01-01 08:00:00.000'
/*
insert into CollarFixes (FileId, CollarManufacturer, CollarId, FixDate, Lat, Lon) VALUES ( 5, 'telonics', '96006', '2012-01-01 00:00', 60.1234, -145.1234)
insert into CollarFixes (FileId, CollarManufacturer, CollarId, FixDate, Lat, Lon) VALUES ( 5, 'telonics', '96006', '2012-01-01 08:00', 60.1244, -145.1244)
insert into CollarFixes (FileId, CollarManufacturer, CollarId, FixDate, Lat, Lon) VALUES ( 5, 'telonics', '96006', '2012-01-01 00:00', 60.1254, -145.1254)
insert into CollarFixes (FileId, CollarManufacturer, CollarId, FixDate, Lat, Lon) VALUES ( 5, 'telonics', '96006', '2012-01-01 00:00', 60.1264, -145.1264)
insert into CollarFixes (FileId, CollarManufacturer, CollarId, FixDate, Lat, Lon) VALUES ( 5, 'telonics', '96006', '2012-01-01 00:00', 60.1264, -145.1264)
*/
--insert into Locations (ProjectId, AnimalId, FixDate, Location, Status) select ProjectId, AnimalId, FixDate, geography::Point(Lat, Lon, 4326) as Location, NULL as [Status] from CollarFixes INNER JOIN CollarDeployments 
--ON CollarDeployments.CollarManufacturer = CollarFixes.CollarManufacturer AND CollarDeployments.CollarId = CollarFixes.CollarId
--where DeploymentDate <= FixDate and (RetrievalDate is NULL or FixDate <= RetrievalDate) and HiddenBy is not null

--	DELETE L FROM Locations as L
--	   INNER JOIN CollarDeployments AS D 
--			   ON D.ProjectId = L.ProjectId AND D.AnimalId = L.AnimalId
--	   INNER JOIN CollarFixes as F
--			   ON D.CollarManufacturer = F.CollarManufacturer AND D.CollarId = F.CollarId
--		    WHERE L.FixDate = F.FixDate and F.FixDate = '2012-01-01 00:00'

--update CollarFixes set HiddenBy = NULL where HiddenBy = $PARAM;


--UPDATE C SET C.HiddenBy = O.FixId FROM CollarFixes as C INNER JOIN CollarFixes as O
--    ON C.CollarManufacturer = O.CollarManufacturer AND C.CollarId = O.CollarID AND C.FixDate = O.FixDate
-- WHERE O.FixDate = '2012-01-01 00:00'

--SELECT BulkColumn FROM OPENROWSET(BULK N'c:\somefile.txt', SINGLE_BLOB) as i


--SELECT TOP 100 * FROM [Animal_Movement].[dbo].[xx0501_569754_0806132213]

--insert into CollarFiles ([FileName],[Project],[CollarManufacturer],[CollarId],[Format],[Status],[Contents])
--                 VALUES ('xyz2', 'test', 'telonics', '96006', 'B', 'I', NULL)

--insert into CollarDataTelonicsStoreOnBoard (FileId, [Fix #], [Fix Status], [Date], [Time], [Latitude], [Longitude])
--									VALUES (6,5,'Fix Available', '2010.12.03', '16:06:55', 60.189860, 205.679892)

--		 SELECT I.FileId, F.CollarManufacturer, F.CollarId,
--		        CONVERT(datetime2, I.[Date]+ ' ' + I.[Time]),
--		        CONVERT(float, I.Latitude), CONVERT(float, I.Longitude) - 360.0
--		   FROM CollarDataTelonicsStoreOnBoard as I INNER JOIN CollarFiles as F 
--			 ON I.FileId = F.FileId
--		  WHERE F.[Status] = 'A' AND I.[Fix Status] = 'Fix Available'

