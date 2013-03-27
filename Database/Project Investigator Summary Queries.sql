-- PI summary statistics
-- ===============================================

    DECLARE @PI NVARCHAR(255) = 'NPS\BBorg'
    DECLARE @LastXdays INT = 30  -- Number of days in the past to consider

----------- WARNING: Telonics Gen4 GPS Collars without TPF file
     SELECT C.CollarModel, C.CollarId, C.ArgosId, C.Frequency
       FROM Collars AS C
  LEFT JOIN CollarParameters AS P
         ON C.CollarManufacturer = P.CollarManufacturer AND C.CollarId = P.CollarId
      WHERE C.CollarManufacturer = 'Telonics' AND C.CollarModel = 'Gen4'
        AND P.CollarId IS NULL
        AND C.Manager = @PI
        
----------- WARNING: Telonics Gen3 GPS Collars with an active PPF File
     SELECT C.CollarModel, C.CollarId, C.ArgosId, C.Frequency
       FROM Collars AS C
  LEFT JOIN CollarParameters AS P
         ON C.CollarManufacturer = P.CollarManufacturer AND C.CollarId = P.CollarId
  LEFT JOIN CollarParameterFiles AS F
         ON P.FileId = F.FileId
      WHERE C.CollarManufacturer = 'Telonics' AND C.CollarModel = 'Gen3'
        AND F.Format = 'B'
        AND F.Status = 'A'
        AND C.Manager = @PI

----------- WARNING: Telonics Gen3 GPS Collars without a Gen3 Period
     SELECT C.CollarModel, C.CollarId, C.ArgosId, C.Frequency
       FROM Collars AS C
      WHERE C.CollarManufacturer = 'Telonics' AND C.CollarModel = 'Gen3'
        AND C.Gen3Period IS NULL
        AND C.Manager = @PI

----------- ERROR: Collars with multiple Active TPF files
     SELECT T.CTN, T.[Platform], T.[Status], T.FileId, T.[FileName], T.[TimeStamp], P.StartDate, P.EndDate
       FROM AllTpfFileData AS T
  LEFT JOIN CollarParameters AS P
         ON T.FileId = P.FileId AND T.CTN = P.CollarId
  LEFT JOIN Collars AS C
         ON C.CollarManufacturer = 'Telonics' AND T.CTN = C.CollarId
      WHERE T.CTN in (SELECT CTN FROM AllTpfFileData WHERE [Status] = 'A' GROUP BY CTN HAVING COUNT(*) > 1)
        AND C.Manager = @PI
   ORDER BY T.CTN, T.[Status]

----------- Collars in multiple TPF files
     SELECT T.CTN, T.[Platform], T.[Status], T.FileId, T.[FileName], T.[TimeStamp], P.StartDate, P.EndDate
       FROM AllTpfFileData AS T
  LEFT JOIN CollarParameters AS P
         ON T.FileId = P.FileId AND T.CTN = P.CollarId
  LEFT JOIN Collars AS C
         ON C.CollarManufacturer = 'Telonics' AND T.CTN = C.CollarId
      WHERE T.CTN in (SELECT CTN FROM AllTpfFileData GROUP BY CTN HAVING COUNT(*) > 1)
        AND C.Manager = @PI
   ORDER BY T.CTN, T.[Status]

----------- Active dates for all the collars of a PI
     SELECT C.CollarManufacturer, C.CollarId, MIN(D.DeploymentDate) AS [FirstDeployment], C.DisposalDate
       FROM Collars as C
  LEFT JOIN CollarDeployments AS D
         ON C.CollarManufacturer = D.CollarManufacturer AND C.CollarId = D.CollarId
      WHERE C.Manager = @PI
   GROUP BY C.CollarManufacturer, C.CollarId, C.DisposalDate
   ORDER BY C.CollarManufacturer, C.CollarId

----------- Count of unused fixes for all of a PI's Collars
     SELECT C.Manager, F.CollarManufacturer, F.CollarId, MIN(F.FixDate) AS [First Fix], MAX(F.FixDate) AS [Last Fix], COUNT(F.FixDate) AS [Fix Count]
       FROM CollarFixes AS F
  LEFT JOIN Locations AS L
         ON F.FixId = L.FixId
  LEFT JOIN Collars AS C
         ON F.CollarManufacturer = C.CollarManufacturer AND  F.CollarId = C.CollarId
      WHERE L.FixId IS NULL
        AND F.HiddenBy IS NULL
        AND C.Manager = @PI
   GROUP BY C.Manager, F.CollarManufacturer, F.CollarId
   ORDER BY COUNT(F.FixDate) DESC

----------- All of a PI's collars that do not have fixes
     SELECT C.CollarManufacturer, C.CollarModel, C.CollarId, C.ArgosId, C.Frequency
       FROM Collars AS C
  LEFT JOIN CollarFixes as F
         ON C.CollarId = F.CollarId
      WHERE C.Manager = @PI
        AND F.CollarId IS NULL
   ORDER BY C.CollarManufacturer, C.CollarModel, C.CollarId

----------- All of a PI's collars that do not have files
     SELECT C.CollarManufacturer, C.CollarModel, C.CollarId, C.ArgosId, C.Frequency
       FROM Collars AS C
  LEFT JOIN CollarFiles as F
         ON C.CollarId = F.CollarId
      WHERE C.Manager = @PI
        AND F.CollarId IS NULL
   ORDER BY C.CollarManufacturer, C.CollarModel, C.CollarId

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
        AND P2.Manager = @PI
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
        AND C.Manager = @PI
   ORDER BY C.Manager, C.CollarModel, C.ArgosId


----------- Argos Platforms I have downloaded, but which I cannot process
     SELECT I.PlatformId
       FROM ArgosFileProcessingIssues AS I
       JOIN ArgosPlatforms AS P1
         ON I.PlatformId = P1.PlatformId
       JOIN ArgosPrograms AS P2
         ON P1.ProgramId = P2.ProgramId
      WHERE I.PlatformId IS NOT NULL
        AND P2.Manager = @PI
   GROUP BY I.PlatformId


----------- Conflicting fixes for all of a PI's collars in the last X days (SLOW!!)
     SELECT C.CollarManufacturer, C.CollarId, F.*
       FROM Collars AS C
CROSS APPLY (SELECT * FROM ConflictingFixes (C.CollarManufacturer,C.CollarId, @LastXdays)) AS F
      WHERE C.Manager = @PI
   ORDER BY CollarId, LocalFixTime, FixId

----------- Summary of fixes for all of a PI's collars (SLOW!!)
     SELECT C.CollarManufacturer, C.CollarId, F.*
       FROM Collars AS C
CROSS APPLY (SELECT * FROM CollarFixSummary (c.CollarManufacturer,c.CollarId)) AS F
      WHERE C.Manager = @PI


