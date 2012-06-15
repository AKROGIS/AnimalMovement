-- Query lists any animals wearing two collars simultaneously
--         or any collar on two animals simultaneously

	  SELECT *
        FROM CollarDeployments AS D1
  INNER JOIN CollarDeployments AS D2
		 ON  NOT(D1.DeploymentDate = D2.DeploymentDate
		     AND D1.ProjectId = D2.ProjectId
		     AND D1.AnimalId = D2.AnimalId
		     AND D1.CollarManufacturer = D2.CollarManufacturer
		     AND D1.CollarId = D2.CollarId)
		AND (   (D1.ProjectId = D2.ProjectId AND D1.AnimalId = D2.AnimalId)
			 OR (D1.CollarManufacturer = D2.CollarManufacturer AND D1.CollarId = D2.CollarId))
	   WHERE  D1.DeploymentDate <= D2.DeploymentDate AND D1.RetrievalDate is null
	   	  OR D2.DeploymentDate <= D1.DeploymentDate AND D2.RetrievalDate is null
	   	  OR D1.DeploymentDate <= D2.DeploymentDate AND D2.DeploymentDate <= D1.RetrievalDate
	   	  OR D2.DeploymentDate <= D1.DeploymentDate AND D1.DeploymentDate <= D2.RetrievalDate
