--  Different Species
SELECT * FROM LookupSpecies


-- Frequency/Species for all non-disposed collars
SELECT C.Manager, A.Species, C.Frequency
FROM Collars AS C
LEFT JOIN CollarDeployments AS D on C.CollarManufacturer = D.CollarManufacturer AND C.CollarId = D.CollarId
LEFT JOIN Animals AS A on D.ProjectId = A.ProjectId AND D.AnimalId = A.AnimalId
WHERE Frequency IS NOT NULL
AND DisposalDate IS NULL
ORDER BY C.Manager, A.Species, C.Frequency


-- Collars without a frequenccy
SELECT Manager, CollarManufacturer, CollarId
FROM Collars
WHERE Frequency IS NULL
ORDER BY Manager, CollarManufacturer, CollarId


-- Managers with Duplicate frequencies on non-disposed collars
SELECT Manager, Frequency, COUNT(*) AS Dups
FROM Collars
WHERE Frequency IS NOT NULL
AND DisposalDate IS NULL
GROUP BY Frequency, Manager
HAVING COUNT(*) > 1
ORDER BY Manager, Frequency


-- Collar Details for Duplicate Frequencies
SELECT C.*, A.*
FROM Collars AS C
LEFT JOIN (
	-- Deployment Details for the last (or current) deployment
	SELECT D1.CollarManufacturer, D1.CollarId, D1. DeploymentDate, ProjectId, AnimalId
	FROM CollarDeployments AS D1
	JOIN (
	    -- last (or current) deployment
		SELECT CollarManufacturer, CollarId, MAX(ISNULL(DeploymentDate, '1900-01-01')) DeploymentDate
		FROM CollarDeployments
		GROUP BY CollarManufacturer, CollarId
	) AS D2 on D1.CollarManufacturer = D2.CollarManufacturer AND D1.CollarId = D2.CollarId AND D1.DeploymentDate = D2.DeploymentDate
) AS D on C.CollarManufacturer = D.CollarManufacturer AND C.CollarId = D.CollarId
LEFT JOIN Animals AS A on D.ProjectId = A.ProjectId AND D.AnimalId = A.AnimalId
WHERE Frequency  IN (
	SELECT Frequency
	FROM Collars
	WHERE Frequency IS NOT NULL
	AND DisposalDate IS NULL
	GROUP BY Frequency, Manager
	HAVING COUNT(*) > 1
)
ORDER BY C.Manager, C.Frequency, A.Species


-- Current or Last deployment (1 per collar); does not include collars never deployed
-- Every collar has a Deployment date, but that is not required; use a really early data if it is null
SELECT CollarManufacturer, CollarId, MAX(ISNULL(DeploymentDate, '1900-01-01'))
FROM CollarDeployments
GROUP BY CollarManufacturer, CollarId
ORDER BY MAX(ISNULL(DeploymentDate, '1900-01-01'))


-- Collars that have never been deployed
SELECT C.Manager, C.CollarManufacturer, C.CollarId
FROM Collars AS C
LEFT JOIN CollarDeployments AS D on C.CollarManufacturer = D.CollarManufacturer AND C.CollarId = D.CollarId
WHERE D.CollarManufacturer IS NULL
ORDER BY C.Manager, C.CollarManufacturer, C.CollarId


-- How many collars are there
SELECT count(*) from Collars

-- As of 2019-04-12:  743 Deployed collar + 60 collar never deployed = 803 collars; Check




-- Sample PIVOT Table
/*
SELECT LocalFixTime, [0901], [0903], [0904]
FROM 
  ( SELECT AnimalID, Speed, dateadd(hour, datediff(hour, 0, LocalDateTime), 0) as LocalFixTime
    FROM VelocityVectors
	where LocalDateTime >= '2010-05-15' and LocalDateTime < '2010-07-16' and ProjectID = 'WACH' and DATEPART(HOUR, LocalDateTime) != 13) p 
PIVOT (
       max(Speed) for AnimalID in ([0901], [0903], [0904])
      ) AS pvt
ORDER BY pvt.LocalFixTime;
*/