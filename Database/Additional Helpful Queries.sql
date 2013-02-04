-- To run one of these queries, just copy paste the query into a new query window,
-- or select the query you want, and press execute in SSMS.  Do not execute the entire file
-- unless you want to run all queries as a test.


-- ERROR Show Orphaned files (records in CollarFiles where CollarId is unmatched)
   SELECT F.FileId, F.CollarManufacturer, F.CollarId, F.[Status], X.CollarId AS CollarId_From_Fixes
     FROM CollarFiles AS F
LEFT JOIN Collars AS C
       ON F.CollarManufacturer = C.CollarManufacturer AND F.CollarId = C.CollarId
LEFT JOIN (SELECT CollarManufacturer, CollarId, FileId FROM CollarFixes GROUP BY CollarManufacturer, CollarId, FileId) AS X
       ON F.FileId = X.FileId
    WHERE F.Format <> 'B' AND C.CollarId IS NULL AND F.CollarId IS NOT NULL


-- ERROR Collar mismatch between Files and Fixes
--       I.E. CollarFile.CollarId != CollarFix.CollarId when CollarFile.FileId = CollarFix.FileID)
   SELECT F.FileId, F.CollarManufacturer, F.CollarId, F.[Status], X.CollarId AS CollarId_From_Fixes
     FROM CollarFiles AS F
LEFT JOIN Collars AS C
       ON F.CollarManufacturer = C.CollarManufacturer AND F.CollarId = C.CollarId
LEFT JOIN (SELECT CollarManufacturer, CollarId, FileId FROM CollarFixes GROUP BY CollarManufacturer, CollarId, FileId) AS X
       ON F.FileId = X.FileId
    WHERE F.Format <> 'B' AND (F.CollarManufacturer <> X.CollarManufacturer OR X.CollarId <> F.CollarId)



-- Collars which are downloadable, but which I cannot analyze
   SELECT A.*, C.CollarModel, C.Gen3Period
     FROM DownloadableCollars AS A
LEFT JOIN DownloadableAndAnalyzableCollars AS B
       ON A.CollarId = B.CollarId
LEFT JOIN Collars AS C
       ON A.CollarId = C.CollarId
    WHERE B.CollarId IS NULL



-- Change status of all files in a project
--   If you only want to change files of a specific format, uncomment the two parts with @Format
DECLARE @ProjectId varchar(255) = 'ARCNVSID022';
--DECLARE @Format char(1) = 'D'; -- See LookupCollarFileFormat
DECLARE @NewStatus char(1) = 'A'; -- must be 'A' or 'I'

DECLARE @FileId int;
DECLARE change_status_cursor CURSOR FOR 
	SELECT FileId FROM CollarFiles Where Project = @ProjectId AND [Status]  <> @NewStatus --AND [Format]  <> @Format
	
OPEN change_status_cursor;

FETCH NEXT FROM change_status_cursor INTO @FileId;

WHILE @@FETCH_STATUS = 0
BEGIN

	EXEC CollarFile_UpdateStatus @FileId, @NewStatus
	FETCH NEXT FROM change_status_cursor INTO @FileId;
END
CLOSE change_status_cursor;
DEALLOCATE change_status_cursor;



-- All conflicting fixes for all of a PI's collars
    DECLARE @PI varchar(255) = 'NPS\SDMIller';
     SELECT C.CollarManufacturer, C.CollarId, F.*
       FROM Collars AS C
CROSS APPLY (SELECT * FROM ConflictingFixes (C.CollarManufacturer,C.CollarId)) AS F
      WHERE C.Manager = @PI


-- All conflicting fixes for all collars deployed (at any time) on a project
    DECLARE @ProjectId varchar(255) = 'ARCNVSID022';
     SELECT C.CollarManufacturer, C.CollarId, F.*
       FROM (SELECT DISTINCT CollarManufacturer, CollarId, ProjectId FROM CollarDeployments) AS C
CROSS APPLY (SELECT * FROM ConflictingFixes (C.CollarManufacturer, C.CollarId)) AS F
      WHERE C.ProjectId = @ProjectId


-- Summary of fixes for all of a PI's collars
    DECLARE @PI varchar(255) = 'NPS\SDMIller';
     SELECT C.CollarManufacturer, C.CollarId, F.*
       FROM Collars AS C
CROSS APPLY (SELECT * FROM CollarFixSummary (c.CollarManufacturer,c.CollarId)) AS F
      WHERE C.Manager = @PI


-- Summary of fixes for all animals in a project
    DECLARE @ProjectId varchar(255) = 'ARCNVSID022';
     SELECT C.AnimalId, F.*
       FROM CollarDeployments AS C
CROSS APPLY (SELECT * FROM AnimalLocationSummary (C.ProjectId, C.AnimalId)) AS F
      WHERE C.ProjectId = @ProjectId



-- All of a PI's collars that do not have fixes
    DECLARE @PI varchar(255) = 'NPS\SDMIller';
     SELECT C.CollarManufacturer, C.CollarId, C.AlternativeId, C.Frequency
       FROM Collars AS C
  LEFT JOIN CollarFixes as F
         ON C.CollarId = F.CollarId
      WHERE C.Manager = @PI
        AND F.CollarId IS NULL



-- All of a PI's collars that do not have files
    DECLARE @PI varchar(255) = 'NPS\SDMIller';
     SELECT C.CollarManufacturer, C.CollarId, C.AlternativeId, C.Frequency
       FROM Collars AS C
  LEFT JOIN CollarFiles as F
         ON C.CollarId = F.CollarId
      WHERE C.Manager = @PI
        AND F.CollarId IS NULL



-- All of a Project's animals that do not have fixes
--  If a animal has had multiple deployments, and one deployment has fixes,
--  and the other does not, this will report a false positive for the
--  listing the animal with the collar with no fixes 
    DECLARE @ProjectId varchar(255) = 'ARCNVSID022';
     SELECT A.AnimalId, D.CollarId
       FROM Animals AS A
  LEFT JOIN CollarDeployments AS D
         ON A.ProjectId = D.ProjectId AND A.AnimalId = D.AnimalId
  LEFT JOIN CollarFixes as F
         ON D.CollarId = F.CollarId
      WHERE A.ProjectId = @ProjectId
        AND F.CollarId IS NULL
   ORDER BY A.AnimalId



-- Collars where an Argos download yielded no data
select c.Manager, c.CollarModel, C.AlternativeId as ArgosId, C.CollarId as CTN from Collars as C
where collarId in (
SELECT [CollarId]
  FROM [Animal_Movement].[dbo].[ArgosDownloads]
  where ErrorMessage is not null
  group by CollarId
  )
  and c.DisposalDate is null
  order by C.Manager, c.CollarModel, C.AlternativeId
  
 
 

-- Collars added by RES from TPF files that do not match existing files.
-- Finding and fixing collar names
select c1.AlternativeId, c1.Manager, c1.CollarId, c2.Manager, c2.CollarId --, F.FileID, D.ProjectId, D.AnimalId
from Collars as C1
inner join Collars as C2
on C1.AlternativeId = C2.AlternativeId and c1.CollarId <> c2.CollarId

/*
left Join CollarFiles as F
on F.CollarId = c2.CollarId

left Join CollarDeployments as D
on D.CollarId = c2.CollarId
*/

 where C1.Manager = 'NPS\resarwas'
 and c1.Manager <> c2.Manager
 order by c1.AlternativeId



--Show all the records for a root collarId
DECLARE @rootID varchar(16) = '630028'
select * from Collars where left(CollarId,6) = @rootID
select * From CollarDeployments where left(CollarId,6) = @rootID
select * from ArgosDownloads where left(CollarId,6) = @rootID
select * from CollarParameters where left(CollarId,6) = @rootID
select FileId, CollarId From CollarFixes where left(CollarId,6) = @rootID group by FileId, CollarId
select * From CollarFiles where left(CollarId,6) = @rootID -- CollarId = @OldID



-- Preview/Hide the fixes outside a nominal range for a project
    DECLARE @ProjectId varchar(255) = 'ARCNVSID022';
    DECLARE @MinLat Real = 65.0;
    DECLARE @MaxLat Real = 75.0;
    DECLARE @MinLon Real = -170.0;
    DECLARE @MaxLon Real = -161.0;
   
  -- Preview fixes outside the range
     SELECT AnimalId, FixDate, Location.Long as Lon, Location.Lat as Lat
       FROM Locations
      WHERE ProjectId = @ProjectId
        AND [Status] IS NULL
        AND (Location.Long < @MinLon OR @MaxLon < Location.Long OR Location.Lat < @MinLat OR @MaxLat < Location.Lat)
/*   
  -- Hide fixes outside the range
     UPDATE Locations
        SET [Status] = 'H'
      WHERE ProjectId = @ProjectId
        AND [Status] IS NULL
        AND (Location.Long < @MinLon OR @MaxLon < Location.Long OR Location.Lat < @MinLat OR @MaxLat < Location.Lat)
*/



--Find bad locations based on improbable velocity vectors
--  The speed, distance are animal dependent.  The duration is dependent on the collar settings
--  speed is meters/hour, and distance is meters
--  Use ArcGIS to zoom in on the animal at the time, and use the addin tool to hide invalid locations.
select * from Movements where ProjectId = 'ARCNVSID022' and (speed > 7500 or duration < .92 or Distance > 40000) order by AnimalId, StartDate
