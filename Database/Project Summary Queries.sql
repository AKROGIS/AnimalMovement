-- Project summary statistics
-- ===============================================

    DECLARE @Project NVARCHAR(255) = 'DENA_Wolves'
    DECLARE @LastXdays INT = 30  -- Number of days in the past to consider

----------- Last location for each living animal with a collar in a project
     SELECT A.ProjectId, A.AnimalId, CONVERT(VARCHAR(10), MAX(FixDate), 101) AS [Date of Last Location]
       FROM Locations AS L
 INNER JOIN Animals AS A
         ON L.ProjectId = A.ProjectId AND L.AnimalId = A.AnimalId
 INNER JOIN CollarDeployments AS CD
         ON CD.ProjectId = A.ProjectId AND CD.AnimalId = A.AnimalId
      WHERE L.ProjectId = @Project
        AND MortalityDate IS NULL
        AND RetrievalDate IS NULL
   GROUP BY A.ProjectId, A.AnimalId
   ORDER BY MAX(FixDate) DESC
   
----------- All of a project's current deployments
     SELECT AnimalId, CollarId, DeploymentDate
       FROM CollarDeployments
      WHERE ProjectId = @Project and RetrievalDate IS NULL
   ORDER BY DeploymentDate

----------- A Project's living animals without a collar
     SELECT *
       FROM Animals
      WHERE ProjectId = @Project
        AND MortalityDate IS NULL
        AND AnimalId NOT IN ( 
                      SELECT AnimalId
                        FROM CollarDeployments
                       WHERE ProjectId = @Project
                         AND RetrievalDate IS NULL
                     )
 
----------- Active files in a project with no fixes
     SELECT CF.ProjectId, CF.[FileName], PF.[FileName] AS Parent, CF.Format, C.CollarModel, C.CollarId, C.ArgosId
       FROM CollarFiles AS CF
 INNER JOIN Collars as C
         ON CF.CollarManufacturer = C.CollarManufacturer AND CF.CollarId = C.CollarId
  LEFT JOIN CollarFiles AS PF
         ON CF.ParentFileId = PF.FileId
  LEFT JOIN CollarFixes AS X
         ON CF.FileId = X.FileId
 INNER JOIN LookupCollarFileFormats AS F
         ON CF.Format = F.Code
      WHERE F.ArgosData = 'N'
        AND CF.[Status] = 'A'
        AND X.FixId IS NULL
        AND CF.ProjectId = @Project
   ORDER BY CF.ProjectId, CF.Format, CollarId, PF.[FileName], CF.[FileName]
    
----------- All of a Project's animals that do not have fixes
--  If a animal has had multiple deployments, and one deployment has fixes,
--  and the other does not, this will report a false positive for the
--  listing the animal with the collar with no fixes 
     SELECT A.AnimalId, D.CollarId
       FROM Animals AS A
  LEFT JOIN CollarDeployments AS D
         ON A.ProjectId = D.ProjectId AND A.AnimalId = D.AnimalId
  LEFT JOIN CollarFixes as F
         ON D.CollarId = F.CollarId
      WHERE A.ProjectId = @Project
        AND F.CollarId IS NULL
   ORDER BY A.AnimalId

----------- All conflicting fixes for all collars deployed (at any time) on a project (SLOW!!!)
     SELECT C.CollarManufacturer, C.CollarId, F.*
       FROM (SELECT DISTINCT CollarManufacturer, CollarId, ProjectId FROM CollarDeployments) AS C
CROSS APPLY (SELECT * FROM ConflictingFixes (C.CollarManufacturer, C.CollarId, @LastXdays)) AS F
      WHERE C.ProjectId = @Project

----------- Summary of fixes for all animals in a project (Slow!!!)
     SELECT C.AnimalId, F.*
       FROM CollarDeployments AS C
CROSS APPLY (SELECT * FROM AnimalLocationSummary (C.ProjectId, C.AnimalId)) AS F
      WHERE C.ProjectId = @Project


