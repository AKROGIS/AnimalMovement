-- To run one of these queries, just copy paste the query into a new query window,
-- or select the query you want, and press execute in SSMS.  Do not execute the entire file
-- unless you want to run all queries as a test.




-- Argos Download tools and checks
-- ==================================================


--Argos Platforms/Collars not being downloaded (for various reasons)
   SELECT P2.Investigator, P.PlatformId, C.*
     FROM ArgosPlatforms AS P
LEFT JOIN ArgosPrograms AS P2
       ON P.ProgramId = P2.ProgramId
LEFT JOIN Collars AS C
       ON C.AlternativeId = P.PlatformId
LEFT JOIN DownloadableAndAnalyzableCollars AS D
       ON P.PlatformId = D.PlatformId
    WHERE D.PlatformId IS NULL
      AND C.DisposalDate IS NULL
      AND P.[Status] <> 'I'
 ORDER BY P2.Investigator, P.PlatformId

 
-- Collars where Argos downloads have yielded no data
   SELECT C.Manager, C.CollarModel, C.AlternativeId AS ArgosId, C.CollarId AS CTN, D.ProjectId, D.AnimalId
     FROM Collars AS C
LEFT JOIN CollarDeployments AS D
	   ON C.CollarManufacturer = D.CollarManufacturer AND C.CollarId = D.CollarId
	WHERE C.CollarId IN (
				SELECT CollarId
				  FROM ArgosDownloads
			  GROUP BY CollarId
			    HAVING Max(FileID) IS NULL
		  )
	  AND C.DisposalDate IS NULL
	  AND D.RetrievalDate IS NULL
 ORDER BY C.Manager, C.CollarModel, C.AlternativeId
  

-- Collars which are downloadable, but which I cannot analyze
   SELECT A.*, C.CollarModel, C.Gen3Period
     FROM DownloadableCollars AS A
LEFT JOIN DownloadableAndAnalyzableCollars AS B
       ON A.CollarId = B.CollarId
LEFT JOIN Collars AS C
       ON A.CollarId = C.CollarId
    WHERE B.CollarId IS NULL


-- All the Email and AWS files that have not been processed 
   SELECT F1.Project, F1.FileId, F1.Format, F1.FileName, F1.UploadDate, F1.UserName
     FROM CollarFiles AS F1
LEFT JOIN CollarFiles AS F2
       ON F1.FileId = F2.ParentFileId
    WHERE F2.FileId IS NULL
      AND F1.Format IN ('F','E')


-- Count/Delete fixes that are based on PTT data
--  DELETE FROM F1
    SELECT C.CollarModel, COUNT(*) AS [PTT Location Count]
      FROM CollarFixes AS F1
INNER JOIN CollarFiles AS F2
        ON F1.FileId = F2.FileId
INNER JOIN Collars as C
        ON F1.CollarManufacturer = C.CollarManufacturer AND F1.CollarId = C.CollarId
     WHERE F2.Format = 'E' OR F2.Format = 'F'
  GROUP BY C.CollarModel




-- Tools helpful for renaming/assigning data files
-- ==================================================


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


--Show all the records for a root collarId
DECLARE @rootID varchar(16) = '643048'
select * from Collars where left(CollarId,6) = @rootID
select * From CollarDeployments where left(CollarId,6) = @rootID
select * from ArgosDownloads where left(CollarId,6) = @rootID
select * from CollarParameters where left(CollarId,6) = @rootID
select FileId, CollarId From CollarFixes where left(CollarId,6) = @rootID group by FileId, CollarId order by fileid
select FileId, CollarId, [FileName], [Format], [Status] From CollarFiles where left(CollarId,6) = @rootID  order by fileid


-- Rename a collar (simple, cannot be used to split a collar into two)
DECLARE @old varchar(16) = '634657';
DECLARE @new varchar(16) = @old + 'A';
UPDATE Collars SET CollarId = @new WHERE CollarId = @old 
UPDATE CollarFiles SET CollarId = @new WHERE CollarId = @old 




-- Checks associated with TPF Files
-- ========================================


-- Collars in TPF Files not in Collars Table
   SELECT T.*
     FROM AllTpfFileData AS T
LEFT JOIN Collars AS C
       ON C.CollarModel = 'TelonicsGen4' AND T.CTN = C.CollarId 
    WHERE C.CollarId IS NULL

-- Telonics Gen4 Collars in Collars table with no TPF File 
   SELECT C.*
     FROM Collars AS C
LEFT JOIN AllTpfFileData AS T
       ON C.CollarId = T.CTN 
    WHERE C.CollarModel = 'TelonicsGen4'
      AND T.CTN IS NULL

-- Collars in TPF Files not in CollarParameters Table
   SELECT T.*
     FROM AllTpfFileData AS T
LEFT JOIN CollarParameters AS C
       ON T.CTN = C.CollarId AND T.FileId = C.FileId
    WHERE C.CollarId IS NULL

-- ERROR Collar Parameters with no TPF File 
   SELECT C.*
     FROM CollarParameters AS C
     JOIN CollarParameterFiles AS F
       ON C.FileId = F.FileId
LEFT JOIN AllTpfFileData AS T
       ON C.CollarId = T.CTN AND C.FileId = T.FileId
    WHERE F.Format = 'A'
      AND T.CTN IS NULL

-- Duplicate collars in TPF file
   SELECT T.CTN, T.[Platform], T.[Status], T.FileId, T.[FileName], T.[TimeStamp], P.StartDate, P.EndDate
     FROM AllTpfFileData AS T
LEFT JOIN CollarParameters AS P
       ON T.FileId = P.FileId AND T.CTN = P.CollarId
    WHERE T.CTN in (SELECT CTN FROM AllTpfFileData GROUP BY CTN HAVING COUNT(*) > 1)
 ORDER BY T.CTN, T.[Status]

-- ERROR - Duplicate collars in ACTIVE TPF file
  SELECT CTN, [Platform], [Status], FileId, [FileName]
    FROM AllTpfFileData
   WHERE CTN in (SELECT CTN FROM AllTpfFileData WHERE [Status] = 'A' GROUP BY CTN HAVING COUNT(*) > 1)
ORDER BY CTN

-- ERROR - Mismatch in Collar TPF Data
    SELECT C.*, T.*
      FROM Collars AS C
INNER JOIN AllTpfFileData AS T
        ON C.CollarId = T.CTN 
     WHERE C.AlternativeId <> T.[Platform]
        OR C.Frequency <> T.Frequency

-- TPF TimeStamp check
   SELECT C.*, T.[TimeStamp], DATEDIFF(day,T.[TimeStamp], C.StartDate) as diff
     FROM CollarParameters AS C
     JOIN CollarParameterFiles AS F
       ON C.FileId = F.FileId
     JOIN AllTpfFileData AS T
       ON C.CollarId = T.CTN AND C.FileId = T.FileId
    WHERE F.Format = 'A'
      AND (C.StartDate IS NULL OR T.[TimeStamp]<> C.StartDate)




-- Collar Displosal/Deployment date checks
-- =======================================


-- Show days between this collars disposal, and the next collars start
-- Assumes all collars have a Alpha suffix, and that there are no gaps.
-- if there is an A version, and then a C version with no B version, the results will be incorrect.
-- Inspect for those manually
    SELECT C1.CollarId, C1.DisposalDate, C2.NextCollar, P.StartDate, P.EndDate, DATEDIFF(DAY, C1.DisposalDate, P.StartDate) AS [Days]
      FROM Collars AS C1
INNER JOIN (SELECT CollarManufacturer, CollarID, LEFT(CollarId,6) + char(ASCII(SUBSTRING(collarId,7,1)) + 1) as NextCollar 
              FROM Collars
             WHERE CollarModel = 'TelonicsGen4') AS C2
        ON C1.CollarManufacturer = C2.CollarManufacturer AND C1.CollarId = C2.CollarId
INNER JOIN CollarParameters AS P
        ON C2.CollarManufacturer = P.CollarManufacturer AND C2.NextCollar = P.CollarId
     WHERE C1.AlternativeId IN (SELECT AlternativeId FROM Collars GROUP BY AlternativeId HAVING COUNT(*) > 1)


-- Show days between this collars disposal, and the next collars start
-- There should be not be two consecutive NULL disposal Dates.
-- The disposal date should be the Start Date of the subsequent record
    SELECT C.AlternativeId, C.CollarId, C.DisposalDate, P.StartDate
      FROM Collars AS C
 LEFT JOIN CollarParameters AS P
        ON C.CollarManufacturer = P.CollarManufacturer AND C.CollarId = P.CollarId
     WHERE C.AlternativeId IN (SELECT AlternativeId FROM Collars GROUP BY AlternativeId HAVING COUNT(*) > 1)
  ORDER BY C.AlternativeId, C.CollarId


-- Collar Deployment Dates for Collars sharing an Argos Id
--    This needs to be reviewed sequentially for errors and issues.
    SELECT C.AlternativeId, C.CollarId, CONVERT(VARCHAR(10), D.DeploymentDate, 101) As Deployed,
           CONVERT(VARCHAR(10), D.RetrievalDate, 101) AS Retrieved,  CONVERT(VARCHAR(10), C.DisposalDate, 101) AS Disposed
      FROM Collars AS C
 LEFT JOIN CollarDeployments AS D
        ON C.CollarManufacturer = D.CollarManufacturer AND C.CollarId = D.CollarId
     WHERE C.AlternativeId IN (SELECT AlternativeId FROM Collars GROUP BY AlternativeId HAVING COUNT(*) > 1)
  ORDER BY C.AlternativeId , C.CollarId, D.DeploymentDate


-- ERROR - Collar Deployments that need to be fixed
    SELECT C.Manager, C.AlternativeId, C.CollarId, CONVERT(VARCHAR(10), D.DeploymentDate, 101) As Deployed,
           CONVERT(VARCHAR(10), D.RetrievalDate, 101) AS Retrieved,  CONVERT(VARCHAR(10), C.DisposalDate, 101) AS Disposed
      FROM Collars AS C
 LEFT JOIN CollarDeployments AS D
        ON C.CollarManufacturer = D.CollarManufacturer AND C.CollarId = D.CollarId
     WHERE C.AlternativeId IN (
				-- ERROR - Collar deployed after they are disposed
				SELECT DISTINCT C.AlternativeId
				  FROM Collars AS C
			 LEFT JOIN CollarDeployments AS D
					ON C.CollarManufacturer = D.CollarManufacturer AND C.CollarId = D.CollarId
				 WHERE C.DisposalDate < D.DeploymentDate OR C.DisposalDate < D.RetrievalDate
                    OR (D.DeploymentDate IS NOT NULL AND D.RetrievalDate IS NULL AND C.DisposalDate IS NOT NULL)
           )
  ORDER BY C.AlternativeId, C.CollarId, D.DeploymentDate


-- FIXMES - Files on collars with deployment errors
    SELECT C.AlternativeId, C.CollarId, F.FileId, F.FileName, F.Format, F.Status
      FROM Collars AS C
 LEFT JOIN CollarFiles AS F
        ON C.CollarManufacturer = F.CollarManufacturer AND C.CollarId = F.CollarId
     WHERE C.AlternativeId IN (
				-- ERROR - Collar deployed after they are disposed
				SELECT DISTINCT C.AlternativeId
				  FROM Collars AS C
			 LEFT JOIN CollarDeployments AS D
					ON C.CollarManufacturer = D.CollarManufacturer AND C.CollarId = D.CollarId
				 WHERE C.DisposalDate < D.DeploymentDate OR C.DisposalDate < D.RetrievalDate
                    OR (D.DeploymentDate IS NOT NULL AND D.RetrievalDate IS NULL AND C.DisposalDate IS NOT NULL)
           )
  ORDER BY C.AlternativeId, C.CollarId, F.FileId




-- Project/PI summary statistics
-- ===============================================


-- Number of locations in database and by project
SELECT MAX(FixId) AS [Max Fix Id], COUNT(*) AS [Fix Count] FROM CollarFixes
SELECT COUNT(*) AS [Unique Fix Count] FROM CollarFixes WHERE HiddenBy IS NULL
SELECT COUNT(*) AS [Total Locations] FROM Locations
SELECT ProjectId, COUNT(*) AS [Location Count] FROM Locations WHERE [Status] IS NULL GROUP BY ProjectId

 
-- All conflicting fixes for all of a PI's collars
    DECLARE @PI varchar(255) = 'NPS\BAMangipane';
     SELECT C.CollarManufacturer, C.CollarId, F.*
       FROM Collars AS C
CROSS APPLY (SELECT * FROM ConflictingFixes (C.CollarManufacturer,C.CollarId)) AS F
      WHERE C.Manager = @PI
   ORDER BY CollarId, LocalFixTime, FixId


-- All conflicting fixes for all collars deployed (at any time) on a project
    DECLARE @ProjectId varchar(255) = 'Yuch_Wolf';
     SELECT C.CollarManufacturer, C.CollarId, F.*
       FROM (SELECT DISTINCT CollarManufacturer, CollarId, ProjectId FROM CollarDeployments) AS C
CROSS APPLY (SELECT * FROM ConflictingFixes (C.CollarManufacturer, C.CollarId)) AS F
      WHERE C.ProjectId = @ProjectId


-- Summary of fixes for all of a PI's collars
    DECLARE @PI varchar(255) = 'NPS\BBorg';
     SELECT C.CollarManufacturer, C.CollarId, F.*
       FROM Collars AS C
CROSS APPLY (SELECT * FROM CollarFixSummary (c.CollarManufacturer,c.CollarId)) AS F
      WHERE C.Manager = @PI


-- Summary of fixes for all animals in a project
    DECLARE @ProjectId varchar(255) = 'Yuch_Wolf';
     SELECT C.AnimalId, F.*
       FROM CollarDeployments AS C
CROSS APPLY (SELECT * FROM AnimalLocationSummary (C.ProjectId, C.AnimalId)) AS F
      WHERE C.ProjectId = @ProjectId


-- All of a PI's collars that do not have fixes
    DECLARE @PI varchar(255) = 'NPS\BAMangipane';
     SELECT C.CollarManufacturer, C.CollarModel, C.CollarId, C.AlternativeId, C.Frequency
       FROM Collars AS C
  LEFT JOIN CollarFixes as F
         ON C.CollarId = F.CollarId
      WHERE C.Manager = @PI
        AND F.CollarId IS NULL
   ORDER BY C.CollarManufacturer, C.CollarModel, C.CollarId


-- All of a PI's collars that do not have files
    DECLARE @PI varchar(255) = 'NPS\JWBurch';
     SELECT C.CollarManufacturer, C.CollarModel, C.CollarId, C.AlternativeId, C.Frequency
       FROM Collars AS C
  LEFT JOIN CollarFiles as F
         ON C.CollarId = F.CollarId
      WHERE C.Manager = @PI
        AND F.CollarId IS NULL
   ORDER BY C.CollarManufacturer, C.CollarModel, C.CollarId


-- All of a Project's animals that do not have fixes
--  If a animal has had multiple deployments, and one deployment has fixes,
--  and the other does not, this will report a false positive for the
--  listing the animal with the collar with no fixes 
    DECLARE @ProjectId varchar(255) = 'Yuch_Wolf';
     SELECT A.AnimalId, D.CollarId
       FROM Animals AS A
  LEFT JOIN CollarDeployments AS D
         ON A.ProjectId = D.ProjectId AND A.AnimalId = D.AnimalId
  LEFT JOIN CollarFixes as F
         ON D.CollarId = F.CollarId
      WHERE A.ProjectId = @ProjectId
        AND F.CollarId IS NULL
   ORDER BY A.AnimalId




-- Project location data cleanup and sanity checking
-- =========================================================


-- Preview/Hide the fixes outside a nominal range for a project
    DECLARE @ProjectId varchar(255) = 'Yuch_Wolf';
    DECLARE @MinLat Real = 63.6;
    DECLARE @MaxLat Real = 66.3;
    DECLARE @MinLon Real = -157.0;
    DECLARE @MaxLon Real = -140.0;
   
  -- Preview fixes outside the range
     SELECT AnimalId, FixDate, Location.Long as Lon, Location.Lat as Lat
       FROM Locations
      WHERE ProjectId = @ProjectId
        AND [Status] IS NULL
        AND (Location.Long < @MinLon OR @MaxLon < Location.Long OR Location.Lat < @MinLat OR @MaxLat < Location.Lat)
   ORDER BY Lon, Lat
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
select * from Movements where ProjectId = 'LACL_Wolf' and (speed > 12000 or duration < 6) order by Speed, AnimalId, StartDate





-- DEBEVEK FILES
-- ===========================================


--Summary of Fixes in Debevek Files (those with NULL collarID are currently ignored)
    SELECT F.[FileName], E.CollarId, E.AnimalId, 
           CONVERT(VARCHAR(10), MIN(convert(datetime2,E.FixDate)), 101) AS FirstFix,
           CONVERT(VARCHAR(10), MAX(convert(datetime2,E.FixDate)), 101) AS LastFix,
           COUNT(*) AS [Count],
           C.Manager, C.CollarId
      FROM CollarDataDebevekFormat AS E
 LEFT JOIN (CollarDeployments AS D
 		    INNER JOIN Collars AS C
		    ON C.CollarManufacturer = D.CollarManufacturer AND C.CollarId = D.CollarId)
        ON (E.AnimalId = D.AnimalId OR E.AnimalId = '0' + D.AnimalId)
       AND C.AlternativeId = E.CollarID
INNER JOIN CollarFiles AS F
        ON F.FileId = E.FileID
  GROUP BY C.Manager, C.CollarId, F.[FileName], E.CollarId, E.AnimalId 
  ORDER BY F.[FileName], C.CollarId, E.CollarId, FirstFix


--Fixes from Ed Debevek Files which will not be used for Animal locations
    SELECT F.[FileName], E.CollarId AS ArgosId, E.AnimalId, 
           CONVERT(VARCHAR(10), MIN(convert(datetime2,E.FixDate)), 101) AS FirstFix,
           CONVERT(VARCHAR(10), MAX(convert(datetime2,E.FixDate)), 101) AS LastFix,
           C.Manager, C.CollarId,
           CONVERT(VARCHAR(10), D.DeploymentDate, 101) AS Deployed,
           CONVERT(VARCHAR(10), D.RetrievalDate, 101) AS Retrieved,
           COUNT(*) AS [Count]
      FROM CollarDataDebevekFormat AS E
INNER JOIN CollarFiles AS F
        ON F.FileId = E.FileID
 LEFT JOIN (CollarDeployments AS D
		   INNER JOIN Collars AS C
		           ON C.CollarManufacturer = D.CollarManufacturer AND C.CollarId = D.CollarId)
        ON (E.AnimalId = D.AnimalId OR E.AnimalId = '0' + D.AnimalId)
       AND C.AlternativeId = E.CollarID
     WHERE C.Manager IS NULL OR E.FixDate < D.DeploymentDate OR E.FixDate > D.RetrievalDate
  GROUP BY C.Manager, C.CollarId, F.[FileName], E.CollarId, E.AnimalId, D.DeploymentDate, D.RetrievalDate
    HAVING C.Manager IS NULL OR MIN(convert(datetime2,E.FixDate)) < D.DeploymentDate OR MAX(convert(datetime2,E.FixDate)) > D.RetrievalDate
  ORDER BY F.[FileName], E.CollarId, FirstFix
 
 
 --Records in Debevek Files not in Fixes
    SELECT CF.FileName, E.CollarID, E.AnimalId, COUNT(*) AS [Count]
      FROM CollarDataDebevekFormat AS E
 LEFT JOIN CollarFixes AS F
        ON F.FileId = E.FileID AND F.LineNumber = E.LineNumber
INNER JOIN CollarFiles AS CF
        ON CF.FileId = E.FileID
     WHERE F.FileId IS NULL
  GROUP BY CF.FileName, E.CollarID, E.AnimalId
