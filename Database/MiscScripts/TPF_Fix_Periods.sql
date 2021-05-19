-- Report the Fix frequency and period from the TPF (Telonics Parameter File)
-- ##############################################################
--
-- The TPF data can change over time.
-- It is reported with the animal that was wearing the collar for each period
-- Depends on the `TPF_GPS_Fix_Period` which must be created with a Python
-- script elsewhere in this repo.

   SELECT A.ProjectID, A.AnimalId, D.CollarId, P.FileId, F.TPF_Filename,
          --D.DeploymentDate, D.RetrievalDate, P.StartDate, P.EndDate, F.Start AS TPF_Start, F.Stop As TPF_Stop,
          (SELECT MAX(v) FROM (VALUES (D.DeploymentDate), (P.StartDate), (F.Start)) AS value(v)) as Start,
          (SELECT MIN(v) FROM (VALUES (D.RetrievalDate), (P.EndDate), (F.Stop)) AS value(v)) as Stop,
          F.Interval, F.Period, F.Period2, F.Period3
     FROM Animals AS A
     JOIN CollarDeployments as D
       ON A.ProjectId = D.ProjectId AND A.AnimalId = D.AnimalId
     JOIN CollarParameters as P
       ON  P.CollarManufacturer = D.CollarManufacturer AND P.CollarId = D.CollarId
       -- Parameter and deployment must overlap on dates
      AND COALESCE(D.DeploymentDate, '1900-01-01') <= COALESCE(P.EndDate, '2200-01-01')
      AND COALESCE(P.StartDate, '1900-01-01') <= COALESCE(D.RetrievalDate, '2200-01-01')
     JOIN TPF_GPS_Fix_Period as F
       ON F.TPF_FileId = P.FileID
       -- Period and deployment must overlap on dates
      AND COALESCE(D.DeploymentDate, '1900-01-01') <= COALESCE(F.Stop, '2200-01-01')
      AND COALESCE(F.Start, '1900-01-01') <= COALESCE(D.RetrievalDate, '2200-01-01')
       -- Period and parameter must overlap on dates
      AND COALESCE(P.StartDate, '1900-01-01') <= COALESCE(F.Stop, '2200-01-01')
      AND COALESCE(F.Start, '1900-01-01') <= COALESCE(P.EndDate, '2200-01-01')
    WHERE D.CollarManufacturer = 'Telonics'
--    AND A.ProjectID = 'DENA_Wolves' --AND A.AnimalId = '1308'
 ORDER BY A.ProjectId, A.AnimalId, D.DeploymentDate, P.StartDate, F.Start
