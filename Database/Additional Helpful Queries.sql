-- To run one of these queries, just copy paste the query into a new query window,
-- or select the query you want, and press execute in SSMS.  Do not execute the entire file
-- unless you want to run all queries as a test.


-- Argos Download tools and checks
-- ==================================================


----------- Argos Platforms with no download data
     SELECT P2.Manager, P.PlatformId, C.*
       FROM ArgosPlatforms AS P
  LEFT JOIN ArgosPrograms AS P2
         ON P.ProgramId = P2.ProgramId
  LEFT JOIN ArgosDeployments AS AD
         ON AD.PlatformId = P.PlatformId
  LEFT JOIN Collars AS C
         ON AD.CollarManufacturer = C.CollarManufacturer AND AD.CollarId = C.CollarId
  LEFT JOIN ArgosFilePlatformDates AS T
         ON T.PlatformId = P.PlatformId
      WHERE T.PlatformId IS NULL
   ORDER BY P2.Manager, P.PlatformId

 
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
  

----------- ERROR: ArgosFiles_WithoutSummary
     SELECT P.ProjectId, P.Owner, P.FileId, P.Format, P.FileName, P.UploadDate, P.UserName
       FROM CollarFiles AS P
  LEFT JOIN ArgosFilePlatformDates AS T
         ON P.FileId = T.FileId
 INNER JOIN LookupCollarFileFormats AS F
         ON P.Format = F.Code
      WHERE T.FileId IS NULL
        AND F.ArgosData = 'Y'


----------- ArgosFile_No children
     SELECT P.ProjectId, P.Owner, P.FileId, P.Format, P.FileName, P.UploadDate, P.UserName
       FROM CollarFiles AS P
  LEFT JOIN CollarFiles AS C
         ON P.FileId = C.ParentFileId
 INNER JOIN LookupCollarFileFormats AS F
         ON P.Format = F.Code
      WHERE C.FileId IS NULL
        AND F.ArgosData = 'Y'


----------- ArgosFile_HasProcessingIssues
     SELECT FileId
       FROM ArgosFileProcessingIssues
   GROUP BY FileId


----------- Argos Platforms I have downloaded, but which I cannot process
     SELECT PlatformId
       FROM ArgosFileProcessingIssues
      WHERE PlatformId IS NOT NULL
   GROUP BY PlatformId


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


----------- Days since last program download
     SELECT ProgramId, DATEDIFF(day, MAX(TimeStamp), GETDATE())
       FROM ArgosDownloads
      WHERE FileId IS NOT NULL AND ProgramId IS NOT NULL
   GROUP BY ProgramId


----------- Days since last platform download
     SELECT PlatformId, DATEDIFF(day, MAX(TimeStamp), GETDATE())
       FROM ArgosDownloads
      WHERE FileId IS NOT NULL AND PlatformId IS NOT NULL
   GROUP BY PlatformId


----------- Days by ArgosId since last processed collar file was uploaded
     SELECT P.PlatformId, DATEDIFF(day, MAX(F.UploadDate), GETDATE())
       FROM ArgosPlatforms AS P
  LEFT JOIN ArgosDeployments AS D
         ON P.PlatformId = D.PlatformId
  LEFT JOIN CollarFiles AS F
         ON F.CollarManufacturer = D.CollarManufacturer AND F.CollarId = D.CollarId
      WHERE F.ParentFileId IS NOT NULL
   GROUP BY P.PlatformId


----------- Days by ArgosId since last AWS file was uploaded
     SELECT P.PlatformId, DATEDIFF(day, MAX(F.UploadDate), GETDATE())
       FROM ArgosPlatforms AS P
  LEFT JOIN ArgosDeployments AS D
         ON P.PlatformId = D.PlatformId
  LEFT JOIN CollarFiles AS F
         ON F.CollarManufacturer = D.CollarManufacturer AND F.CollarId = D.CollarId
  LEFT JOIN CollarFiles AS F2
         ON F.ParentFileId = F2.FileId
      WHERE F2.Format = 'F'
   GROUP BY P.PlatformId


----------- Recent downloads
     SELECT *
       FROM ArgosDownloads
      WHERE DATEDIFF(hour, [TimeStamp], GETDATE()) < 72
   ORDER BY [TimeStamp] DESC





-- Checks associated with Hidden Locations 
-- =======================================


----------- Locations which are not but should be hidden    
     SELECT L.FixDate, F.Sha1Hash, L.ProjectId, L.AnimalId, L.[Status]
       FROM HiddenFixes AS H
 INNER JOIN (
						Locations AS L
			 INNER JOIN CollarFixes AS X
					 ON L.FixId = X.FixId
			 INNER JOIN CollarFiles AS F
					 ON F.FileId = X.FileId
            )
         ON H.FixDate = L.FixDate AND H.Sha1Hash = F.Sha1Hash
      WHERE L.[Status] IS NULL

----------- FixDate and FileHash for hidden locations    
     SELECT L.FixDate, F.Sha1Hash --, L.ProjectId, L.AnimalId, L.[Status]
       FROM Locations AS L
 INNER JOIN CollarFixes AS X
         ON L.FixId = X.FixId
 INNER JOIN CollarFiles AS F
         ON F.FileId = X.FileId
      WHERE L.[Status] IS NOT NULL





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
   ORDER BY T.CTN, T.[Status], StartDate

----------- Warning - Duplicate collars (CTN, timestamp) in TPF file
     SELECT T.CTN, T.[Platform], T.[TimeStamp], T.[Status], T.FileId, T.[FileName], T3.[Count] AS [CTNs in File]
       FROM AllTpfFileData AS T
       JOIN (SELECT CTN, [TimeStamp] FROM AllTpfFileData GROUP BY CTN, [TimeStamp] HAVING COUNT(*) > 1) AS T2
         ON T.CTN = T2.CTN AND T.[TimeStamp] = T2.[TimeStamp]
       JOIN (SELECT [FileName], COUNT(*) AS [Count] FROM AllTpfFileData GROUP BY [FileName]) AS T3
         ON T.[FileName] = T3.[FileName]
   ORDER BY T.CTN, T.[Status]

----------- Warning - Mismatch in Collar TPF Data (Changes in Frequency are permissible)
     SELECT C.*, T.*
       FROM Collars AS C
 INNER JOIN AllTpfFileData AS T
         ON C.CollarId = T.CTN 
      WHERE C.ArgosId <> T.[Platform]
         OR C.Frequency <> T.Frequency

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

----------- ERROR - Mismatch in Collar TPF Data (Changes in Frequency are permissible)
     SELECT C.*, T.*
       FROM Collars AS C
 INNER JOIN AllTpfFileData AS T
         ON C.CollarId = T.CTN 
      WHERE C.ArgosId <> T.[Platform]

----------- TPF TimeStamp check
     SELECT C.*, T.[TimeStamp], DATEDIFF(day,T.[TimeStamp], C.StartDate) as diff
       FROM CollarParameters AS C
       JOIN CollarParameterFiles AS F
         ON C.FileId = F.FileId
       JOIN AllTpfFileData AS T
         ON C.CollarId = T.CTN AND C.FileId = T.FileId
      WHERE F.Format = 'A'
        AND (C.StartDate IS NULL OR T.[TimeStamp]<> C.StartDate)

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

----------- Collar File Business rule: A CollarFile with a parent, must have the same owner, project, and status as the parent
     SELECT C.FileId, C.ParentFileId, P.[FileName], P.UploadDate, C.[Owner], P.[Owner], C.ProjectId, P.ProjectId
       FROM CollarFiles AS P
 INNER JOIN CollarFiles AS C
         ON C.ParentFileId = P.FileId
      WHERE C.[Owner] != P.[Owner] or C.ProjectId != P.ProjectId
         OR (C.[Owner] IS NULL AND P.[Owner] IS NOT NULL)
         OR (C.[Owner] IS NOT NULL AND P.[Owner] IS NULL)
         OR (C.ProjectId IS NULL AND P.ProjectId IS NOT NULL)
         OR (C.ProjectId IS NOT NULL AND P.ProjectId IS NULL)
         OR (C.[Status] = 'A' AND P.[Status] = 'I')

----------- Collar File Business rule: must have one and only one non-null value for (Owner,ProjectId)
     SELECT FileId, ProjectId, [Owner]
       FROM Collarfiles
      WHERE (ProjectId IS NOT NULL AND [Owner] IS NOT NULL)
         OR (ProjectId IS NULL AND [Owner] IS NULL)

----------- Collar File Business rule: A Collar Mfgr/Id is required unless the file format has Argos data.
     SELECT C.FileId, C.[FileName], C.CollarManufacturer, C.CollarId, C.[Format], F.ArgosData
       FROM Collarfiles AS C
 INNER JOIN LookupCollarFileFormats AS F
         ON C.Format = F.Code
      WHERE (C.CollarId IS NULL AND F.ArgosData = 'N')
         OR (C.CollarId IS NOT NULL AND F.ArgosData = 'Y')

----------- Collar File Business rule: ArgosProgramId and CollarParameterId must be non-null only when parent has ArgosData
     SELECT C.FileId, C.[FileName], C.ArgosDeploymentId, C.CollarParameterId, C.[Format], F.ArgosData
       FROM CollarFiles AS P
 INNER JOIN CollarFiles AS C
         ON C.ParentFileId = P.FileId
 INNER JOIN LookupCollarFileFormats AS F
         ON P.Format = F.Code
      WHERE ((C.ArgosDeploymentId IS NULL OR C.CollarParameterId IS NULL) AND F.ArgosData = 'Y')
         OR ((C.ArgosDeploymentId IS NOT NULL OR C.CollarParameterId IS NOT NULL) AND F.ArgosData = 'N')

----------- Collar File Business rule: All Collar files with ArgosData must be summerized
     SELECT C.FileId, C.[FileName], C.ArgosDeploymentId, C.CollarParameterId, C.[Format], F.ArgosData
       FROM CollarFiles AS C
 INNER JOIN LookupCollarFileFormats AS F
         ON C.Format = F.Code
  LEFT JOIN ArgosFilePlatformDates AS A
         ON C.FileId = A.FileId
      WHERE (A.FileId IS NULL AND F.ArgosData = 'Y')
         OR (A.FileId IS NOT NULL AND F.ArgosData = 'N')



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


----------- Really Bad Fixes
     SELECT MIN(Lat), MAX(Lat), MIN(Lon), MAX(Lon) FROM CollarFixes 
     SELECT F.FileId
       FROM CollarFixes AS X
 INNER JOIN CollarFiles AS F
         ON X.fileId = F.fileId
      WHERE X.Lon < -180
   GROUP BY F.FileId

----------- Preview/Hide the fixes outside a nominal range for a project
/*
    DECLARE @Project varchar(255) = 'DENA_Wolves';
    DECLARE @MinLat Real = 62.5;
    DECLARE @MaxLat Real = 66.6;
    DECLARE @MinLon Real = -163.0;
    DECLARE @MaxLon Real = -142.0;
*/
    DECLARE @Project varchar(255) = 'Yuch_Wolf';
    DECLARE @MinLat Real = 63.7;
    DECLARE @MaxLat Real = 66.0;
    DECLARE @MinLon Real = -146.8;
    DECLARE @MaxLon Real = -140.3;
   
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
      WHERE ProjectId = @Project
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
     SELECT F.[FileName], E.PlatformID, E.AnimalId, 
            CONVERT(VARCHAR(10), MIN(convert(datetime2,E.FixDate)), 101) AS FirstFix,
            CONVERT(VARCHAR(10), MAX(convert(datetime2,E.FixDate)), 101) AS LastFix,
            COUNT(*) AS [Count],
            C.Manager, C.CollarId
       FROM CollarDataDebevekFormat AS E
 INNER JOIN CollarFiles AS F
         ON F.FileId = E.FileID
 INNER JOIN Collars AS C
         ON C.CollarManufacturer = F.CollarManufacturer AND C.CollarId = F.CollarId
   GROUP BY C.Manager, C.CollarId, F.[FileName], E.PlatformID, E.AnimalId 
   ORDER BY F.[FileName], C.CollarId, E.PlatformID, FirstFix


----------- Fixes from Ed Debevek Files not be used for Animal locations
     SELECT F.[FileName], D.PlatformId, X.CollarManufacturer, X.CollarId,
            D.AnimalId AS [Debevek AnimalId],
            CONVERT(VARCHAR(10), MIN(X.FixDate), 101) AS [First Fix],
            CONVERT(VARCHAR(10), MAX(X.FixDate), 101) AS [Last Fix],
            COUNT(X.FixDate) AS [Fix Count]
       FROM CollarDataDebevekFormat AS D
  LEFT JOIN CollarFixes AS X
         ON X.FileId = D.FileID AND X.LineNumber = D.LineNumber
  LEFT JOIN Locations AS L
         ON L.FixId = X.FixId
  LEFT JOIN CollarFiles AS F
         ON D.FileID = F.FileId
      WHERE L.FixId IS NULL
   GROUP BY F.[FileName], D.PlatformId, D.AnimalId, X.CollarManufacturer, X.CollarId 
   ORDER BY F.[FileName], D.PlatformID, [First Fix]

 
----------- Records in Debevek Files not in Fixes
     SELECT CF.[FileName], E.PlatformID, E.AnimalId, COUNT(*) AS [Count]
       FROM CollarDataDebevekFormat AS E
  LEFT JOIN CollarFixes AS F
         ON F.FileId = E.FileID AND F.LineNumber = E.LineNumber
 INNER JOIN CollarFiles AS CF
         ON CF.FileId = E.FileID
      WHERE F.FileId IS NULL
   GROUP BY CF.[FileName], E.PlatformID, E.AnimalId




-- Tools for checking collar data files
-- ==================================================


----------- Collar Files without data in the appropriate data file
     SELECT F1.FileId, F1.[Filename]
       FROM CollarFiles AS F1
  LEFT JOIN CollarDataTelonicsGen3StoreOnBoard AS F2
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
      WHERE C.CollarId IS NULL AND F.CollarId IS NOT NULL


----------- ERROR Collar mismatch between Files and Fixes
-----------     I.E. CollarFile.CollarId != CollarFix.CollarId when CollarFile.FileId = CollarFix.FileID)
     SELECT F.FileId, F.CollarManufacturer, F.CollarId, F.[Status], X.CollarId AS CollarId_From_Fixes
       FROM CollarFiles AS F
  LEFT JOIN Collars AS C
         ON F.CollarManufacturer = C.CollarManufacturer AND F.CollarId = C.CollarId
  LEFT JOIN (SELECT CollarManufacturer, CollarId, FileId FROM CollarFixes GROUP BY CollarManufacturer, CollarId, FileId) AS X
         ON F.FileId = X.FileId
      WHERE (F.CollarManufacturer <> X.CollarManufacturer OR X.CollarId <> F.CollarId)


----------- Show all the records for a root collarId
    DECLARE @rootID VARCHAR(16) = '646252'
     SELECT * FROM Collars WHERE LEFT(CollarId,6) = @rootID
     SELECT * FROM CollarDeployments WHERE LEFT(CollarId,6) = @rootID
     SELECT * FROM ArgosDownloads WHERE PlatformId in (SELECT PlatformID FROM ArgosDeployments WHERE LEFT(CollarId,6) = @rootID)
     SELECT * FROM CollarParameters WHERE LEFT(CollarId,6) = @rootID
     SELECT * FROM ArgosDeployments WHERE LEFT(CollarId,6) = @rootID
     SELECT FileId, CollarId FROM CollarFixes WHERE left(CollarId,6) = @rootID GROUP BY FileId, CollarId ORDER BY fileid
     SELECT FileId, CollarId, [FileName], [Format], [Status] FROM CollarFiles WHERE LEFT(CollarId,6) = @rootID  ORDER BY fileid


----------- Show all the records for an ArgosId
    DECLARE @ArgosID VARCHAR(16) = '37779'
     SELECT * FROM Collars WHERE ArgosId = @ArgosID
     SELECT * FROM CollarDeployments WHERE CollarId IN (SELECT CollarID FROM Collars WHERE ArgosId = @ArgosID)
     SELECT FileId, CollarId, [FileName], [Format], [Status] FROM CollarFiles WHERE CollarId in (SELECT CollarID FROM Collars WHERE ArgosId = @ArgosID) ORDER BY fileid
     SELECT FileId, CollarId FROM CollarFixes WHERE CollarId in (SELECT CollarID FROM Collars WHERE ArgosId = @ArgosID) GROUP BY FileId, CollarId ORDER BY fileid
     SELECT * FROM AllTpfFileData WHERE Platform = @ArgosID
     SELECT * FROM CollarParameters WHERE CollarId in (SELECT CollarID FROM Collars WHERE ArgosId = @ArgosID)
     SELECT * FROM ArgosDeployments WHERE PlatformId = @ArgosID
     SELECT * FROM ArgosDownloads WHERE PlatformId = @ArgosID


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
             WHERE ProjectId = @ProjectId AND [Status]  <> @NewStatus --AND [Format]  <> @Format
        
    OPEN change_status_cursor;

    FETCH NEXT FROM change_status_cursor INTO @FileId;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        EXEC CollarFile_UpdateStatus @FileId, @NewStatus
        FETCH NEXT FROM change_status_cursor INTO @FileId;
    END
    CLOSE change_status_cursor;
    DEALLOCATE change_status_cursor;


----------- Rename a collar (simple, cannot be used to split a collar into two)
    DECLARE @old VARCHAR(16) = '634657';
    DECLARE @new VARCHAR(16) = @old + 'A';
     UPDATE Collars SET CollarId = @new WHERE CollarId = @old 
     UPDATE CollarFiles SET CollarId = @new WHERE CollarId = @old
*/