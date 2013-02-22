-- PI summary statistics
-- ===============================================

    DECLARE @PI NVARCHAR(255) = 'NPS\BBorg'
    DECLARE @LastXdays INT = 30  -- Number of days in the past to consider

----------- All of a PI's collars that do not have fixes
     SELECT C.CollarManufacturer, C.CollarModel, C.CollarId, C.AlternativeId, C.Frequency
       FROM Collars AS C
  LEFT JOIN CollarFixes as F
         ON C.CollarId = F.CollarId
      WHERE C.Manager = @PI
        AND F.CollarId IS NULL
   ORDER BY C.CollarManufacturer, C.CollarModel, C.CollarId

----------- All of a PI's collars that do not have files
     SELECT C.CollarManufacturer, C.CollarModel, C.CollarId, C.AlternativeId, C.Frequency
       FROM Collars AS C
  LEFT JOIN CollarFiles as F
         ON C.CollarId = F.CollarId
      WHERE C.Manager = @PI
        AND F.CollarId IS NULL
   ORDER BY C.CollarManufacturer, C.CollarModel, C.CollarId

----------- Argos Platforms/Collars not being downloaded (for various reasons)
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
       AND P2.Investigator = @PI
  ORDER BY P2.Investigator, P.PlatformId

----------- Collars where Argos downloads have yielded no data
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
        AND C.Manager = @PI
   ORDER BY C.Manager, C.CollarModel, C.AlternativeId

----------- Collars which are downloadable, but which I cannot analyze
     SELECT A.*, C.CollarModel, C.Gen3Period
       FROM DownloadableCollars AS A
  LEFT JOIN DownloadableAndAnalyzableCollars AS B
         ON A.CollarId = B.CollarId
  LEFT JOIN Collars AS C
         ON A.CollarId = C.CollarId
      WHERE B.CollarId IS NULL
        AND C.Manager = @PI

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


