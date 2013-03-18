-- To run one of these queries, just copy paste the query into a new query window,
-- or select the query you want, and press execute in SSMS.  Do not execute the entire file
-- unless you want to run all queries as a test.


-- Argos Download tools and checks
-- ==================================================


----------- Argos Platforms/Collars not being downloaded (for various reasons)
     SELECT P2.Investigator, P.PlatformId, C.*
       FROM ArgosPlatforms AS P
  LEFT JOIN ArgosPrograms AS P2
         ON P.ProgramId = P2.ProgramId
  LEFT JOIN Collars AS C
         ON C.ArgosId = P.PlatformId
  LEFT JOIN DownloadableAndAnalyzableCollars AS D
         ON P.PlatformId = D.PlatformId
      WHERE D.PlatformId IS NULL
        AND C.DisposalDate IS NULL
        AND P.[Status] <> 'I'
   ORDER BY P2.Investigator, P.PlatformId

 
----------- Collars where Argos downloads have yielded no data
     SELECT C.Manager, C.CollarModel, C.ArgosId AS ArgosId, C.CollarId AS CTN, D.ProjectId, D.AnimalId
       FROM Collars AS C
  LEFT JOIN CollarDeployments AS D
         ON C.CollarManufacturer = D.CollarManufacturer AND C.CollarId = D.CollarId
  LEFT JOIN ArgosDeployments AS A
         ON C.CollarManufacturer = A.CollarManufacturer AND C.CollarId = A.CollarId
      WHERE A.PlatformId IN (
                            SELECT PlatformId
                              FROM ArgosDownloads
                          GROUP BY PlatformId
                            HAVING Max(FileID) IS NULL
                          )
        AND C.DisposalDate IS NULL
        AND D.RetrievalDate IS NULL
   ORDER BY C.Manager, C.CollarModel, C.ArgosId
  

----------- Collars which are downloadable, but which I cannot analyze
     SELECT A.*, C.CollarModel, C.Gen3Period
       FROM DownloadableCollars AS A
  LEFT JOIN DownloadableAndAnalyzableCollars AS B
         ON A.CollarId = B.CollarId
  LEFT JOIN Collars AS C
         ON A.CollarId = C.CollarId
      WHERE B.CollarId IS NULL


----------- ERROR Argos Downloaded files that are not the right file format
     SELECT F.FileId, F.Format, F.[FileName], F.UserName, F.UploadDate
       FROM ArgosDownloads AS D
 INNER JOIN CollarFiles AS F
         ON F.FileId = D.FileId
      WHERE F.Format <> 'F'


----------- Count/Delete fixes that are based on PTT data
----------- DELETE FROM F1
     SELECT C.CollarModel, COUNT(*) AS [PTT Location Count]
       FROM CollarFixes AS F1
 INNER JOIN CollarFiles AS F2
         ON F1.FileId = F2.FileId
 INNER JOIN Collars as C
         ON F1.CollarManufacturer = C.CollarManufacturer AND F1.CollarId = C.CollarId
      WHERE F2.Format = 'E' OR F2.Format = 'F'
   GROUP BY C.CollarModel


----------- Recent downloads
     SELECT *
       FROM ArgosDownloads
      WHERE DATEDIFF(hour, [TimeStamp], GETDATE()) < 24
   ORDER BY [TimeStamp] DESC





-- Checks associated with TPF Files
-- ========================================


----------- Telonics Gen4 Collars in Collars table with no TPF File 
     SELECT C.*
       FROM Collars AS C
  LEFT JOIN AllTpfFileData AS T
         ON C.CollarId = T.CTN 
      WHERE C.CollarManufacturer = 'Telonics' AND C.CollarModel = 'Gen4'
        AND T.CTN IS NULL

----------- Collars in multiple TPF files
     SELECT T.CTN, T.[Platform], T.[Status], T.FileId, T.[FileName], T.[TimeStamp], P.StartDate, P.EndDate
       FROM AllTpfFileData AS T
  LEFT JOIN CollarParameters AS P
         ON T.FileId = P.FileId AND T.CTN = P.CollarId
      WHERE T.CTN in (SELECT CTN FROM AllTpfFileData GROUP BY CTN HAVING COUNT(*) > 1)
   ORDER BY T.CTN, T.[Status]

----------- The following queries should return no records
----------- Collars in TPF Files not in Collars Table
     SELECT T.*
       FROM AllTpfFileData AS T
  LEFT JOIN Collars AS C
         ON C.CollarManufacturer = 'Telonics' AND C.CollarModel = 'Gen4' AND T.CTN = C.CollarId 
      WHERE C.CollarId IS NULL

----------- Collars in a TPF Files not in CollarParameters Table (checks each collar only once)
     SELECT T.*
       FROM AllTpfFileData AS T
  LEFT JOIN CollarParameters AS C
         ON T.CTN = C.CollarId
      WHERE C.CollarId IS NULL

----------- TPF Collars not in CollarParameters Table (expects parameter for each tpf file a collar is in)
     SELECT T.*
       FROM AllTpfFileData AS T
  LEFT JOIN CollarParameters AS C
         ON T.CTN = C.CollarId AND T.FileId = C.FileId
      WHERE C.CollarId IS NULL AND T.Status = 'A'

----------- WARNING: Collars/TPF files where the respective owners do not match
     SELECT F.[Owner] AS [TPF Manager], C.Manager AS [Collar Manager], F.[FileName], F.FileId, C.CollarId
       FROM AllTpfFileData AS T
 INNER JOIN CollarParameterFiles AS F
         ON F.FileId = T.FileId
 INNER JOIN Collars AS C
         ON C.CollarManufacturer = 'Telonics' AND C.CollarModel = 'Gen4' and C.CollarId = T.CTN
      WHERE F.Owner <> C.Manager
   ORDER BY CTN


----------- ERROR Collar Parameters with no TPF File 
     SELECT C.*
       FROM CollarParameters AS C
  LEFT JOIN CollarParameterFiles AS F
         ON C.FileId = F.FileId
  LEFT JOIN AllTpfFileData AS T
         ON C.CollarId = T.CTN AND C.FileId = T.FileId
      WHERE (F.Format IS NULL OR F.Format = 'A')
        AND T.CTN IS NULL
        AND C.Gen3Period IS NULL

----------- ERROR - Duplicate collars (id, status, timestamp) in TPF file
     SELECT CTN, [Platform], [Status], FileId, [FileName], [TimeStamp]
       FROM AllTpfFileData
      WHERE CTN in (SELECT CTN FROM AllTpfFileData GROUP BY CTN, [Status], [TimeStamp] HAVING COUNT(*) > 1)
   ORDER BY CTN

----------- ERROR - Mismatch in Collar TPF Data
     SELECT C.*, T.*
       FROM Collars AS C
 INNER JOIN AllTpfFileData AS T
         ON C.CollarId = T.CTN 
      WHERE C.ArgosId <> T.[Platform]
         OR C.Frequency <> T.Frequency

----------- TPF TimeStamp check
     SELECT C.*, T.[TimeStamp], DATEDIFF(day,T.[TimeStamp], C.StartDate) as diff
       FROM CollarParameters AS C
       JOIN CollarParameterFiles AS F
         ON C.FileId = F.FileId
       JOIN AllTpfFileData AS T
         ON C.CollarId = T.CTN AND C.FileId = T.FileId
      WHERE F.Format = 'A'
        AND (C.StartDate IS NULL OR T.[TimeStamp]<> C.StartDate)

----------- WARNING Collar Parameter End Date that does not match Collar Disposal Date
     SELECT *
       FROM CollarParameters AS P
 INNER JOIN Collars AS C
         ON C.CollarManufacturer = P.CollarManufacturer AND C.CollarId = P.CollarId
      WHERE C.DisposalDate IS NOT NULL
        AND P.EndDate IS NULL OR P.EndDate > C.DisposalDate

----------- ERROR Collar Parameter Start Date that violates Collar Disposal Date
     SELECT *
       FROM CollarParameters AS P
 INNER JOIN Collars AS C
         ON C.CollarManufacturer = P.CollarManufacturer AND C.CollarId = P.CollarId
      WHERE C.DisposalDate IS NOT NULL
        AND P.StartDate > C.DisposalDate





-- File checks
-- =======================================


----------- Duplicate Collar Data Files
     SELECT Sha1Hash, COUNT(*) AS [Duplicate Count], MIN(FileId) as [First File Id], MAX(FileId) as [Last File Id]
       FROM CollarFiles
   GROUP BY Sha1Hash
     HAVING COUNT(*) > 1

----------- Duplicate Collar Parameter Files
     SELECT Sha1Hash, COUNT(*) AS [Duplicate Count], MIN(FileId) as [First File Id], MAX(FileId) as [Last File Id]
       FROM CollarParameterFiles
   GROUP BY Sha1Hash
     HAVING COUNT(*) > 1





-- Collar Displosal/Deployment date checks
-- =======================================


----------- Show collars with multiple parameter settings
     SELECT *
       FROM CollarParameters
      WHERE CollarId IN (
	  		     SELECT CollarId
				   FROM CollarParameters
			   GROUP BY CollarId
				 HAVING COUNT (*) > 1
            )
   ORDER BY CollarId, startdate

----------- Show Argos platforms with multiple collar deployments
     SELECT *
       FROM ArgosDeployments
      WHERE PlatformId IN (
				   SELECT PlatformId
				     FROM ArgosDeployments
			     GROUP BY PlatformId
				   HAVING COUNT (*) > 1
            )
   ORDER BY PlatformId, startdate


----------- Show days between this collars disposal, and the next collars start
-----------     Assumes all collars have a Alpha suffix, and that there are no gaps.
-----------     if there is an A version, and then a C version with no B version, the results will be incorrect.
-----------     Inspect for those manually
     SELECT C1.CollarId, C1.DisposalDate, C2.NextCollar, P.StartDate, P.EndDate, DATEDIFF(DAY, C1.DisposalDate, P.StartDate) AS [Days]
       FROM Collars AS C1
 INNER JOIN (SELECT CollarManufacturer, CollarID, LEFT(CollarId,6) + CHAR(ASCII(SUBSTRING(collarId,7,1)) + 1) AS NextCollar 
               FROM Collars
              WHERE CollarManufacturer = 'Telonics' AND CollarModel = 'Gen4') AS C2
         ON C1.CollarManufacturer = C2.CollarManufacturer AND C1.CollarId = C2.CollarId
 INNER JOIN CollarParameters AS P
         ON C2.CollarManufacturer = P.CollarManufacturer AND C2.NextCollar = P.CollarId
      WHERE C1.ArgosId IN (SELECT ArgosId FROM Collars GROUP BY ArgosId HAVING COUNT(*) > 1)


----------- Show days between this collars disposal, and the next collars start
-----------     There should be not be two consecutive NULL disposal Dates.
-----------     The disposal date should be the Start Date of the subsequent record
     SELECT C.ArgosId, C.CollarId, C.DisposalDate, P.StartDate
       FROM Collars AS C
  LEFT JOIN CollarParameters AS P
         ON C.CollarManufacturer = P.CollarManufacturer AND C.CollarId = P.CollarId
      WHERE C.ArgosId IN (SELECT ArgosId FROM Collars GROUP BY ArgosId HAVING COUNT(*) > 1)
   ORDER BY C.ArgosId, C.CollarId


----------- Collar Deployment Dates for Collars sharing an Argos Id
-----------     This needs to be reviewed sequentially for errors and issues.
     SELECT C.ArgosId, C.CollarId, CONVERT(VARCHAR(10), D.DeploymentDate, 101) As Deployed,
            CONVERT(VARCHAR(10), D.RetrievalDate, 101) AS Retrieved,  CONVERT(VARCHAR(10), C.DisposalDate, 101) AS Disposed
       FROM Collars AS C
  LEFT JOIN CollarDeployments AS D
         ON C.CollarManufacturer = D.CollarManufacturer AND C.CollarId = D.CollarId
      WHERE C.ArgosId IN (SELECT ArgosId FROM Collars GROUP BY ArgosId HAVING COUNT(*) > 1)
   ORDER BY C.ArgosId , C.CollarId, D.DeploymentDate


----------- ERROR - Collar Deployments that need to be fixed
     SELECT C.Manager, C.ArgosId, C.CollarId, CONVERT(VARCHAR(10), D.DeploymentDate, 101) As Deployed,
            CONVERT(VARCHAR(10), D.RetrievalDate, 101) AS Retrieved,  CONVERT(VARCHAR(10), C.DisposalDate, 101) AS Disposed
       FROM Collars AS C
  LEFT JOIN CollarDeployments AS D
         ON C.CollarManufacturer = D.CollarManufacturer AND C.CollarId = D.CollarId
      WHERE C.ArgosId IN (
                -- ERROR - Collar deployed after they are disposed
                SELECT DISTINCT C.ArgosId
                  FROM Collars AS C
             LEFT JOIN CollarDeployments AS D
                    ON C.CollarManufacturer = D.CollarManufacturer AND C.CollarId = D.CollarId
                 WHERE C.DisposalDate < D.DeploymentDate OR C.DisposalDate < D.RetrievalDate
                    OR (D.DeploymentDate IS NOT NULL AND D.RetrievalDate IS NULL AND C.DisposalDate IS NOT NULL)
            )
   ORDER BY C.ArgosId, C.CollarId, D.DeploymentDate


----------- FIXMES - Files on collars with deployment errors
     SELECT C.ArgosId, C.CollarId, F.FileId, F.FileName, F.Format, F.Status
       FROM Collars AS C
  LEFT JOIN CollarFiles AS F
         ON C.CollarManufacturer = F.CollarManufacturer AND C.CollarId = F.CollarId
      WHERE C.ArgosId IN (
                -- ERROR - Collar deployed after they are disposed
                SELECT DISTINCT C.ArgosId
                  FROM Collars AS C
             LEFT JOIN CollarDeployments AS D
                    ON C.CollarManufacturer = D.CollarManufacturer AND C.CollarId = D.CollarId
                 WHERE C.DisposalDate < D.DeploymentDate OR C.DisposalDate < D.RetrievalDate
                    OR (D.DeploymentDate IS NOT NULL AND D.RetrievalDate IS NULL AND C.DisposalDate IS NOT NULL)
            )
   ORDER BY C.ArgosId, C.CollarId, F.FileId





-- Database summary statistics
-- ===============================================


----------- Number of locations in database and by project
     SELECT MAX(FixId) AS [Max Fix Id], COUNT(*) AS [Fix Count] FROM CollarFixes
     SELECT COUNT(*) AS [Project Count] FROM Projects
     SELECT COUNT(*) AS [Animal Count] FROM Animals
     SELECT COUNT(*) AS [Collar Count] FROM Collars
     SELECT COUNT(*) AS [Unique Fix Count] FROM CollarFixes WHERE HiddenBy IS NULL
     SELECT COUNT(*) AS [Total Fix Count] FROM CollarFixes
     SELECT COUNT(*) AS [Total Locations], MIN(FixDate) AS [Earliest Fix], MAX(FixDate) AS [Last Fix] FROM Locations
     EXECUTE sp_helpdb Animal_Movement
     SELECT ProjectId, COUNT(*) AS [Location Count] FROM Locations WHERE [Status] IS NULL GROUP BY ProjectId





-- Project location data cleanup and sanity checking
-- =========================================================


----------- Preview/Hide the fixes outside a nominal range for a project
    DECLARE @Project varchar(255) = 'Yuch_Wolf';
    DECLARE @MinLat Real = 63.6;
    DECLARE @MaxLat Real = 66.3;
    DECLARE @MinLon Real = -157.0;
    DECLARE @MaxLon Real = -140.0;
   
----------- Preview fixes outside the range
     SELECT AnimalId, FixDate, Location.Long as Lon, Location.Lat as Lat
       FROM Locations
      WHERE ProjectId = @Project
        AND [Status] IS NULL
        AND (Location.Long < @MinLon OR @MaxLon < Location.Long OR Location.Lat < @MinLat OR @MaxLat < Location.Lat)
   ORDER BY Lon, Lat
/*
----------- Hide fixes outside the range
     UPDATE Locations
        SET [Status] = 'H'
      WHERE ProjectId = @ProjectId
        AND [Status] IS NULL
        AND (Location.Long < @MinLon OR @MaxLon < Location.Long OR Location.Lat < @MinLat OR @MaxLat < Location.Lat)
*/


----------- Find bad locations based on improbable velocity vectors
-----------     The speed, distance are animal dependent.  The duration is dependent on the collar settings
-----------     speed is meters/hour, and distance is meters
-----------     Use ArcGIS to zoom in on the animal at the time, and use the addin tool to hide invalid locations.
     SELECT *
       FROM Movements
      WHERE ProjectId = 'Yuch_Wolf'
        AND (speed > 12000 or duration < 6)
   ORDER BY Speed, AnimalId, StartDate





-- DEBEVEK FILES
-- ===========================================


----------- Summary of Fixes in Debevek Files (those with NULL collarID are currently ignored)
     SELECT F.[FileName], E.CollarId, E.AnimalId, 
            CONVERT(VARCHAR(10), MIN(convert(datetime2,E.FixDate)), 101) AS FirstFix,
            CONVERT(VARCHAR(10), MAX(convert(datetime2,E.FixDate)), 101) AS LastFix,
            COUNT(*) AS [Count],
            C.Manager, C.CollarId
       FROM CollarDataDebevekFormat AS E
  LEFT JOIN (
                        CollarDeployments AS D
             INNER JOIN Collars AS C
                     ON C.CollarManufacturer = D.CollarManufacturer AND C.CollarId = D.CollarId
            )
         ON (E.AnimalId = D.AnimalId OR E.AnimalId = '0' + D.AnimalId)
        AND C.ArgosId = E.CollarID
 INNER JOIN CollarFiles AS F
         ON F.FileId = E.FileID
   GROUP BY C.Manager, C.CollarId, F.[FileName], E.CollarId, E.AnimalId 
   ORDER BY F.[FileName], C.CollarId, E.CollarId, FirstFix


----------- Fixes from Ed Debevek Files which will not be used for Animal locations
     SELECT F.[FileName], E.CollarId AS ArgosId, E.AnimalId, 
            CONVERT(VARCHAR(10), MIN(CONVERT(DATETIME2,E.FixDate)), 101) AS FirstFix,
            CONVERT(VARCHAR(10), MAX(CONVERT(DATETIME2,E.FixDate)), 101) AS LastFix,
            C.Manager, C.CollarId,
            CONVERT(VARCHAR(10), D.DeploymentDate, 101) AS Deployed,
            CONVERT(VARCHAR(10), D.RetrievalDate, 101) AS Retrieved,
            COUNT(*) AS [Count]
       FROM CollarDataDebevekFormat AS E
 INNER JOIN CollarFiles AS F
         ON F.FileId = E.FileID
  LEFT JOIN (
             CollarDeployments AS D
             INNER JOIN Collars AS C
             ON C.CollarManufacturer = D.CollarManufacturer AND C.CollarId = D.CollarId
            )
         ON (E.AnimalId = D.AnimalId OR E.AnimalId = '0' + D.AnimalId)
        AND C.ArgosId = E.CollarID
      WHERE C.Manager IS NULL OR E.FixDate < D.DeploymentDate OR E.FixDate > D.RetrievalDate
   GROUP BY C.Manager, C.CollarId, F.[FileName], E.CollarId, E.AnimalId, D.DeploymentDate, D.RetrievalDate
     HAVING C.Manager IS NULL OR MIN(CONVERT(DATETIME2,E.FixDate)) < D.DeploymentDate OR MAX(CONVERT(DATETIME2,E.FixDate)) > D.RetrievalDate
   ORDER BY F.[FileName], E.CollarId, FirstFix
 
 
----------- Records in Debevek Files not in Fixes
     SELECT CF.[FileName], E.CollarID, E.AnimalId, COUNT(*) AS [Count]
       FROM CollarDataDebevekFormat AS E
  LEFT JOIN CollarFixes AS F
         ON F.FileId = E.FileID AND F.LineNumber = E.LineNumber
 INNER JOIN CollarFiles AS CF
         ON CF.FileId = E.FileID
      WHERE F.FileId IS NULL
   GROUP BY CF.[FileName], E.CollarID, E.AnimalId




-- Tools for checking collar data files
-- ==================================================


----------- Collar Files without data in the appropriate data file
     SELECT F1.FileId, F1.[Filename]
       FROM CollarFiles AS F1
  LEFT JOIN CollarDataTelonicsStoreOnBoard AS F2
         ON F1.FileId = F2.FileId
      WHERE F1.Format = 'A' AND F2.FileId IS NULL
      
     SELECT F1.FileId, F1.[Filename]
       FROM CollarFiles AS F1
  LEFT JOIN CollarDataDebevekFormat AS F2
         ON F1.FileId = F2.FileId
      WHERE f1.Format = 'B' AND f2.FileId IS NULL
           
     SELECT F1.FileId, F1.[Filename]
       FROM CollarFiles AS F1
  LEFT JOIN CollarDataTelonicsGen4 AS F2
         ON F1.FileId = F2.FileId
      WHERE F1.Format = 'C' AND F2.FileId IS NULL
           
     SELECT F1.FileId, F1.[Filename]
       FROM CollarFiles AS F1
  LEFT JOIN CollarDataTelonicsGen3 AS F2
         ON F1.FileId = F2.FileId
      WHERE F1.Format = 'D' AND F2.FileId IS NULL
           
     SELECT F1.FileId, F1.[Filename]
       FROM CollarFiles AS F1
  LEFT JOIN CollarDataArgosEmail AS F2
         ON F1.FileId = F2.FileId
      WHERE F1.Format = 'E' AND F2.FileId IS NULL
      
     SELECT F1.FileId, F1.[Filename]
       FROM CollarFiles AS F1
  LEFT JOIN CollarDataArgosWebService AS F2
         ON F1.FileId = F2.FileId
      WHERE F1.Format = 'F' AND F2.FileId IS NULL




-- Tools helpful for renaming/assigning data files
-- ==================================================


----------- ERROR Show Orphaned files (records in CollarFiles where CollarId is unmatched)
     SELECT F.FileId, F.CollarManufacturer, F.CollarId, F.[Status], X.CollarId AS CollarId_From_Fixes
       FROM CollarFiles AS F
  LEFT JOIN Collars AS C
         ON F.CollarManufacturer = C.CollarManufacturer AND F.CollarId = C.CollarId
  LEFT JOIN (SELECT CollarManufacturer, CollarId, FileId FROM CollarFixes GROUP BY CollarManufacturer, CollarId, FileId) AS X
         ON F.FileId = X.FileId
      WHERE F.Format <> 'B' AND C.CollarId IS NULL AND F.CollarId IS NOT NULL


----------- ERROR Collar mismatch between Files and Fixes
-----------     I.E. CollarFile.CollarId != CollarFix.CollarId when CollarFile.FileId = CollarFix.FileID)
     SELECT F.FileId, F.CollarManufacturer, F.CollarId, F.[Status], X.CollarId AS CollarId_From_Fixes
       FROM CollarFiles AS F
  LEFT JOIN Collars AS C
         ON F.CollarManufacturer = C.CollarManufacturer AND F.CollarId = C.CollarId
  LEFT JOIN (SELECT CollarManufacturer, CollarId, FileId FROM CollarFixes GROUP BY CollarManufacturer, CollarId, FileId) AS X
         ON F.FileId = X.FileId
      WHERE F.Format <> 'B' AND (F.CollarManufacturer <> X.CollarManufacturer OR X.CollarId <> F.CollarId)

/*
----------- Change status of all files in a project
-----------     If you only want to change files of a specific format, uncomment the two parts with @Format
    DECLARE @ProjectId varchar(255) = 'ARCNVSID022';
    --DECLARE @Format char(1) = 'D'; -- See LookupCollarFileFormat
    DECLARE @NewStatus char(1) = 'A'; -- must be 'A' or 'I'

    DECLARE @FileId int;
    DECLARE change_status_cursor CURSOR FOR 
            SELECT FileId 
              FROM CollarFiles
             WHERE Project = @ProjectId AND [Status]  <> @NewStatus --AND [Format]  <> @Format
        
    OPEN change_status_cursor;

    FETCH NEXT FROM change_status_cursor INTO @FileId;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        EXEC CollarFile_UpdateStatus @FileId, @NewStatus
        FETCH NEXT FROM change_status_cursor INTO @FileId;
    END
    CLOSE change_status_cursor;
    DEALLOCATE change_status_cursor;


----------- Show all the records for a root collarId
    DECLARE @rootID VARCHAR(16) = '631645'
     SELECT * FROM Collars WHERE LEFT(CollarId,6) = @rootID
     SELECT * FROM CollarDeployments WHERE LEFT(CollarId,6) = @rootID
     SELECT * FROM ArgosDownloads WHERE PlatformId in (SELECT PlatformID FROM ArgosDeployments WHERE LEFT(CollarId,6) = @rootID)
     SELECT * FROM CollarParameters WHERE LEFT(CollarId,6) = @rootID
     SELECT FileId, CollarId FROM CollarFixes WHERE left(CollarId,6) = @rootID GROUP BY FileId, CollarId ORDER BY fileid
     SELECT FileId, CollarId, [FileName], [Format], [Status] FROM CollarFiles WHERE LEFT(CollarId,6) = @rootID  ORDER BY fileid

----------- Show all the records for an ArgosId
    DECLARE @ArgosID VARCHAR(16) = '96008'
     SELECT * FROM Collars WHERE ArgosId = @ArgosID
     SELECT * FROM CollarDeployments WHERE CollarId IN (SELECT CollarID FROM Collars WHERE ArgosId = @ArgosID)
     SELECT FileId, CollarId, [FileName], [Format], [Status] FROM CollarFiles WHERE CollarId in (SELECT CollarID FROM Collars WHERE ArgosId = @ArgosID) ORDER BY fileid
     SELECT FileId, CollarId FROM CollarFixes WHERE CollarId in (SELECT CollarID FROM Collars WHERE ArgosId = @ArgosID) GROUP BY FileId, CollarId ORDER BY fileid
     SELECT * FROM AllTpfFileData WHERE Platform = @ArgosID
     SELECT * FROM CollarParameters WHERE CollarId in (SELECT CollarID FROM Collars WHERE ArgosId = @ArgosID)
     SELECT * FROM ArgosDownloads WHERE PlatformId = @ArgosID


----------- Rename a collar (simple, cannot be used to split a collar into two)
    DECLARE @old VARCHAR(16) = '634657';
    DECLARE @new VARCHAR(16) = @old + 'A';
     UPDATE Collars SET CollarId = @new WHERE CollarId = @old 
     UPDATE CollarFiles SET CollarId = @new WHERE CollarId = @old
*/