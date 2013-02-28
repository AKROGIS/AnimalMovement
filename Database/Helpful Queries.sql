----------- Current Deployments
     SELECT *
       FROM CollarDeployments
      WHERE DeploymentDate < GETUTCDATE()
        AND (RetrievalDate > GETUTCDATE() OR RetrievalDate IS NULL)


----------- Animals Never Collared
      SELECT A.ProjectId, A.AnimalId, A.Species, A.Gender, A.GroupName, P.ProjectInvestigator
       FROM dbo.Projects AS P
 INNER JOIN dbo.Animals AS A
         ON P.ProjectId = A.ProjectId
  LEFT JOIN dbo.CollarDeployments AS D
         ON A.ProjectId = D.ProjectId AND A.AnimalId = D.AnimalId
      WHERE D.ProjectId IS NULL


----------- Animals Currently Collared
     SELECT A.ProjectId, A.AnimalId, A.Species, A.Gender, A.GroupName, P.ProjectInvestigator 
       FROM dbo.CollarDeployments AS D
 INNER JOIN dbo.Animals AS A
         ON D.ProjectId = A.ProjectId AND D.AnimalId = A.AnimalId
 INNER JOIN dbo.Projects AS P
         ON A.ProjectId = P.ProjectId
      WHERE (D.DeploymentDate < GETUTCDATE())
        AND (D.RetrievalDate > GETUTCDATE() OR D.RetrievalDate IS NULL)


----------- Animals Not Currently Collared
     SELECT A.ProjectId, A.AnimalId, A.Species, A.MortalityDate, A.Gender, A.GroupName, P.ProjectInvestigator
       FROM dbo.CollarDeployments AS D
 INNER JOIN dbo.Animals AS A
         ON A.ProjectId = D.ProjectId AND A.AnimalId = D.AnimalId
 INNER JOIN dbo.Projects AS P
         ON A.ProjectId = P.ProjectId
  LEFT JOIN (
             ------ CurrentDeployments
             SELECT DeploymentId
               FROM CollarDeployments
              WHERE DeploymentDate < GETUTCDATE()
                AND (RetrievalDate > GETUTCDATE() OR RetrievalDate IS NULL)  
            ) AS CD
         ON D.DeploymentId = CD.DeploymentId
      WHERE CD.DeploymentId IS NULL


----------- Collars Currently Deployed
     SELECT C.*
       FROM dbo.Collars AS C
 INNER JOIN dbo.CollarDeployments AS D
         ON D.CollarManufacturer = C.CollarManufacturer AND D.CollarId = C.CollarId
      WHERE D.DeploymentDate < GETUTCDATE()
        AND (D.RetrievalDate > GETUTCDATE() OR D.RetrievalDate IS NULL)


----------- Collars Never Deployed
     SELECT C.*
       FROM dbo.Collars AS C
 INNER JOIN dbo.CollarDeployments AS D
         ON D.CollarManufacturer = C.CollarManufacturer AND D.CollarId = C.CollarId
      WHERE D.CollarId IS NULL


----------- Collars Not Currently Deployed
     SELECT L.Name, C.CollarId, C.CollarModel, C.Frequency, 
            C.SerialNumber, C.ArgosId, C.Owner, C.Manager, C.Notes
       FROM dbo.Collars AS C
 INNER JOIN dbo.CollarDeployments D
         ON C.CollarManufacturer = D.CollarManufacturer AND C.CollarId = D.CollarId 
 INNER JOIN dbo.LookupCollarManufacturers AS L
         ON C.CollarManufacturer = L.CollarManufacturer
  LEFT JOIN (
             ------ CurrentDeployments
             SELECT DeploymentId
               FROM CollarDeployments
              WHERE DeploymentDate < GETUTCDATE()
                AND (RetrievalDate > GETUTCDATE() OR RetrievalDate IS NULL)  
            ) AS CD
         ON D.DeploymentId = CD.DeploymentId
      WHERE CD.DeploymentId IS NULL


----------- Collars With Conflicting Fixes
     SELECT DISTINCT CollarManufacturer, CollarId
       FROM dbo.CollarFixes
   GROUP BY CollarManufacturer, CollarId, FixDate
     HAVING COUNT(FixDate) > 1


----------- ERROR_AnimalsWithOverlappingDeployments
     SELECT CD1.ProjectId, CD1.AnimalId,
            CD1.DeploymentId, CD1.CollarManufacturer, CD1.CollarId, CD1.DeploymentDate, CD1.RetrievalDate,
            CD2.DeploymentId, CD2.CollarManufacturer, CD2.CollarId, CD2.DeploymentDate, CD2.RetrievalDate
       FROM CollarDeployments AS CD1
 INNER JOIN CollarDeployments AS CD2
         ON CD1.ProjectId = CD2.ProjectId AND CD1.AnimalId = CD2.AnimalId
      WHERE (CD1.CollarManufacturer <> CD2.CollarManufacturer OR CD1.CollarId <> CD2.CollarId)
        AND (CD1.DeploymentDate <= CD2.DeploymentDate AND (CD2.DeploymentDate <= CD1.RetrievalDate OR CD1.RetrievalDate IS NULL)
         OR CD2.DeploymentDate <= CD1.DeploymentDate AND (CD1.DeploymentDate <= CD2.RetrievalDate OR CD2.RetrievalDate IS NULL))


----------- ERROR_CollarsWithOverlappingDeployments
     SELECT CD1.CollarManufacturer, CD1.CollarId,
            CD1.DeploymentId, CD1.ProjectId, CD1.AnimalId, CD1.DeploymentDate, CD1.RetrievalDate,
            CD2.DeploymentId, CD2.ProjectId, CD2.AnimalId, CD2.DeploymentDate, CD2.RetrievalDate
       FROM CollarDeployments AS CD1
 INNER JOIN CollarDeployments AS CD2
         ON CD1.CollarManufacturer = CD2.CollarManufacturer AND CD1.CollarId = CD2.CollarId
      WHERE (CD1.ProjectId <> CD2.ProjectId OR CD1.AnimalId <> CD2.AnimalId)
        AND (CD1.DeploymentDate <= CD2.DeploymentDate AND (CD2.DeploymentDate <= CD1.RetrievalDate OR CD1.RetrievalDate IS NULL)
         OR CD2.DeploymentDate <= CD1.DeploymentDate AND (CD1.DeploymentDate <= CD2.RetrievalDate OR CD2.RetrievalDate IS NULL))


----------- ERROR_FixesWhichShouldBeLocations
     SELECT FD.*, A.MortalityDate
       FROM (
             ------ Fixes that are within a deployed date range and not hidden (i.e. should be locations)
             SELECT CF.FixId, CD.ProjectId, CD.AnimalId, CF.FixDate
               FROM CollarDeployments AS CD
          LEFT JOIN CollarFixes AS CF
                 ON CF.CollarManufacturer = CD.CollarManufacturer AND CF.CollarId = CD.CollarId
              WHERE CF.FixDate > CD.DeploymentDate
                AND (CF.FixDate < CD.RetrievalDate OR CD.RetrievalDate IS NULL)
                AND CF.HiddenBy IS NULL
            ) AS FD
 INNER JOIN Animals AS A
         ON A.ProjectId = FD.ProjectId AND A.AnimalId = FD.AnimalId
  LEFT JOIN Locations AS L
         ON L.FixId = FD.FixId
      WHERE FD.FixId IS NOT NULL
        AND L.FixId IS NULL
        AND (A.MortalityDate IS NULL OR FD.FixDate < A.MortalityDate)


----------- ERROR_LocationsAfterAnimalsMortality
     SELECT L.*
       FROM Locations AS L
 INNER JOIN Animals AS A
         ON A.ProjectId = L.ProjectId AND A.AnimalId = L.AnimalId
      WHERE L.FixDate > A.MortalityDate


----------- ERROR_LocationsOutsideBoundsOfDeployments
     SELECT L.*
       FROM Locations as L
  LEFT JOIN (
             ------ Fixes that should not be a location (i.e. hidden or outside a deployed date range)
             SELECT CF.FixId
               FROM CollarFixes as CF
          LEFT JOIN (
                     ------ Fixes that are within a deployed date range and not hidden (i.e. should be locations)
                     SELECT CF.FixId
                       FROM CollarDeployments AS CD
                  LEFT JOIN CollarFixes AS CF
                         ON CF.CollarManufacturer = CD.CollarManufacturer AND CF.CollarId = CD.CollarId
                      WHERE CF.FixDate > CD.DeploymentDate
                        AND (CF.FixDate < CD.RetrievalDate OR CD.RetrievalDate IS NULL)
                        AND CF.HiddenBy IS NULL
                    ) AS FD
                 ON CF.FixId = FD.FixId
              WHERE FD.FixId IS NULL
            ) AS FND
         ON L.FixId = FND.FixId
      WHERE FND.FixId IS NOT NULL


----------- FixesByLocation
     SELECT dbo.CollarFixes.FixDate
       FROM dbo.CollarFixes
 INNER JOIN dbo.Locations
         ON dbo.CollarFixes.FixDate = dbo.Locations.FixDate
 INNER JOIN dbo.CollarDeployments
         ON dbo.CollarFixes.CollarManufacturer = dbo.CollarDeployments.CollarManufacturer
        AND dbo.Locations.ProjectId = dbo.CollarDeployments.ProjectId
        AND dbo.Locations.AnimalId = dbo.CollarDeployments.AnimalId
        AND dbo.CollarFixes.CollarId = dbo.CollarDeployments.CollarId
      WHERE dbo.CollarFixes.FixDate > dbo.CollarDeployments.DeploymentDate
        AND (dbo.CollarFixes.FixDate < dbo.CollarDeployments.RetrievalDate OR dbo.CollarDeployments.RetrievalDate IS NULL)


----------- WARNING_ActiveArgosPlatformsWithAnalysisProblems
----------- -- Active Argos platforms with active PPF or without Gen3period or TPF file
     SELECT C.Manager, CD.ProjectId, CD.AnimalId, C.CollarModel, C.CollarId AS CTN,
            A.PlatformId AS ArgosId, C.Gen3Period, CPF.[FileName] AS ParameterFile
       FROM ArgosPlatforms AS A
 INNER JOIN ArgosPrograms AS P
         ON A.ProgramId = P.ProgramId
 INNER JOIN Collars AS C
         ON C.ArgosId = A.PlatformId
 INNER JOIN CollarDeployments as CD
         ON C.CollarManufacturer = CD.CollarManufacturer AND C.CollarId = CD.CollarId
  LEFT JOIN CollarParameters as CP
         ON C.CollarManufacturer = CP.CollarManufacturer AND C.CollarId = CP.CollarId
  LEFT JOIN CollarParameterFiles as CPF
         ON CP.FileId = CPF.FileId           
  LEFT JOIN (
               SELECT CollarManufacturer, CollarId, Max([Timestamp]) AS [Timestamp]
                 FROM ArgosDownloads
                WHERE ErrorMessage IS NULL
                GROUP BY CollarManufacturer, CollarId
            ) AS D
         ON C.CollarManufacturer = D.CollarManufacturer AND C.CollarId = D.CollarId
      WHERE A.[Status] = 'A'
        AND P.[Status] = 'A'
        AND (CPF.[Status] = 'A' OR CPF.[Status] IS NULL)
        AND (P.EndDate IS NULL OR getdate() < P.EndDate)
        AND (C.DisposalDate IS NULL OR getdate() < C.DisposalDate)
        AND (CD.RetrievalDate IS NULL OR getdate() < CD.RetrievalDate)
        AND (  (C.CollarManufacturer = 'Telonics' AND C.CollarModel = 'Gen3' AND CPF.Format <> 'B')
            OR (C.CollarManufacturer = 'Telonics' AND C.CollarModel = 'Gen3' AND C.Gen3Period IS NULL)
            OR (C.CollarManufacturer = 'Telonics' AND C.CollarModel = 'Gen4' AND CPF.Format IS NULL)
            OR (C.CollarManufacturer = 'Telonics' AND C.CollarModel = 'Gen4' AND CPF.Format <> 'A')
            )


----------- WARNING_AllTelonicGen4CollarsWithoutActiveTpfFile
----------- -- All Telonics Gen4 Argos Collars without a active TPF File
     SELECT C.Manager, CD.ProjectId, CD.AnimalId, C.CollarId as CTN, C.ArgosId as ArgosID,  
            C.Frequency, C.DisposalDate, CD.RetrievalDate, C.Notes,
            CPF.[FileName] AS ParameterFile, CPF.Format, CPF.[Status]
       FROM Collars AS C
  LEFT JOIN CollarDeployments AS CD
         ON C.CollarManufacturer = CD.CollarManufacturer AND C.CollarId = CD.CollarId
  LEFT JOIN CollarParameters as CP
         ON C.CollarManufacturer = CP.CollarManufacturer AND C.CollarId = CP.CollarId
  LEFT JOIN CollarParameterFiles as CPF
         ON CP.FileId = CPF.FileId           
      WHERE C.CollarManufacturer = 'Telonics' AND C.CollarModel = 'Gen4'
        AND C.ArgosId IS NOT NULL
        AND (CPF.Format IS NULL -- no parameter file
         OR C.CollarId NOT IN ( -- collars with active TPF files
               SELECT CP.CollarId
                 FROM CollarParameters as CP
           INNER JOIN CollarParameterFiles as CPF
                   ON CP.FileId = CPF.FileId           
                WHERE CPF.Format = 'A' AND CPF.[Status] = 'A')
            )


----------- WARNING_ArgosPlatformsNotInCollars
----------- -- Known Argos Platforms that are not in the Collars table 
     SELECT A.Investigator, P.ProgramId, P.PlatformId, P.[Status], P.Remarks
       FROM ArgosPlatforms AS P
 INNER JOIN ArgosPrograms AS A
         ON P.ProgramId = A.ProgramId
  LEFT JOIN Collars AS C
         ON P.PlatformId = C.ArgosId
      WHERE C.CollarManufacturer IS NULL


----------- WARNING_ArgosPlatformsNotTelonicsGen3or4
----------- -- Known Argos Platforms that are not a Telonics Gen3/4 GPS Collars 
     SELECT P.*, C.*
       FROM ArgosPlatforms AS P
  LEFT JOIN Collars AS C
         ON P.PlatformId = C.ArgosId
      WHERE C.CollarManufacturer IS NOT NULL
        AND C.CollarManufacturer <> 'Telonics' OR C.CollarModel NOT IN ('Gen3', 'Gen4')


----------- WARNING_CollarsWithMultipleParameterFiles
     SELECT cpf.[FileName], cpf.[Status], cp.CollarManufacturer, cp.CollarId, cp.FileId, cp.StartDate, cp.EndDate
       FROM dbo.CollarParameters AS cp
 INNER JOIN (
                  SELECT CollarManufacturer, CollarId
                    FROM dbo.CollarParameters
                GROUP BY CollarManufacturer, CollarId
                  HAVING COUNT(*) > 1
            ) AS C
         ON cp.CollarId = c.CollarId AND cp.CollarManufacturer = c.CollarManufacturer
 INNER JOIN dbo.CollarParameterFiles AS cpf
         ON cp.FileId = cpf.FileId


----------- WARNING_TelonicArgosCollarsWithNoPlatform
----------- -- Telonics Gen3/4 Argos GPS Collars with no matching record in ArgosPlatforms
    SELECT C.Manager, C.Owner, C.CollarModel, C.CollarId as CTN, C.ArgosId as ArgosID, C.Frequency, C.DisposalDate
      FROM Collars AS C
 LEFT JOIN ArgosPlatforms AS P
        ON P.PlatformId = C.ArgosId
     WHERE C.ArgosId IS NOT NULL
       AND P.PlatformId IS NULL
       AND C.CollarManufacturer = 'Telonics' AND C.CollarModel IN ('Gen3', 'Gen4')


----------- WARNING_TelonicsCollarsMissingArgosId
     SELECT CollarId, CollarModel, Manager, DisposalDate
       FROM Collars
      WHERE CollarManufacturer = 'Telonics' AND CollarModel IN ('Gen3', 'Gen4')
        AND ArgosId IS NULL


----------- WARNING_TelonicsCollarsSharingAnArgosId
     SELECT C1.ArgosId, C1.CollarId, C1.DisposalDate, C1.Manager
       FROM dbo.Collars AS C1
 INNER JOIN (
             SELECT ArgosId
               FROM dbo.Collars
              WHERE CollarManufacturer = 'Telonics' AND CollarModel IN ('Gen3', 'Gen4')
           GROUP BY ArgosId, DisposalDate
             HAVING COUNT(*) > 1
            ) AS C2
         ON C1.ArgosId = C2.ArgosId


----------- WARNING_TelonicsGen3CollarsWithActivePpfFile
     SELECT cp.CollarManufacturer, cp.CollarId, cpf.[FileName], cpf.[Owner], cp.StartDate, cp.EndDate
       FROM CollarParameters AS cp
 INNER JOIN CollarParameterFiles AS cpf
         ON cp.FileId = cpf.FileId
      WHERE cpf.Format = 'B' AND cpf.[Status] = 'A'


----------- WARNING_TelonicsGen3CollarsWithoutPeriod
     SELECT CollarId, Manager, Gen3Period
       FROM Collars
      WHERE CollarManufacturer = 'Telonics' AND CollarModel = 'Gen3'
        AND Gen3Period IS NULL
        AND ArgosId IS NOT NULL


----------- WARNING_TelonicsGenParameterFileMismatch
     SELECT c.CollarManufacturer, c.CollarId, c.CollarModel, c.Manager, cpf.[FileName], lcpf.[Name], cpf.[Owner], cp.StartDate, cp.EndDate
       FROM CollarParameters AS cp
 INNER JOIN CollarParameterFiles AS cpf
         ON cp.FileId = cpf.FileId
 INNER JOIN LookupCollarParameterFileFormats AS lcpf
         ON lcpf.Code = cpf.Format
 INNER JOIN Collars AS c
         ON cp.CollarManufacturer = c.CollarManufacturer AND cp.CollarId = c.CollarId
      WHERE (cpf.Format = 'A' AND c.CollarManufacturer = 'Telonics' AND c.CollarModel <> 'Gen4')
         OR (cpf.Format = 'B' AND c.CollarManufacturer = 'Telonics' AND c.CollarModel <> 'Gen3')

