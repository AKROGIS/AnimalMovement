USE [Animal_Movement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Regan Sarwas
-- Create date: Feb 20, 2013
-- Description: Add/Remove Locations when Disposal Date is updated
-- =============================================
CREATE TRIGGER [dbo].[AfterCollarDisposalDateUpdate] 
			ON [dbo].[Collars] 
			AFTER UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	
	--Disposal date is the change that this trigger cares about.
	--Note: this just means that DisposalDate was included in the update statement,
	--      it does not mean that the value in this column has changed for all the
	--      rows updated. 
	IF UPDATE ([DisposalDate])
	BEGIN

		-- triggers always execute in the context of a transaction
		-- so the following code is all or nothing.

/* LOGIC:
    delete locations where new disposal date < fix date AND (fix date < old disposal date OR old disposal date is null) 
       add locations where old disposal date < fix date AND (fix date < new disposal date OR new disposal date is null)
*/
    
		DELETE L FROM dbo.Locations as L
				   -- Join to the collar that created this location
		   INNER JOIN CollarFixes as F
				   ON L.FixId = F.FixId
		   INNER JOIN deleted as D
				   ON F.CollarManufacturer = D.CollarManufacturer
				  AND F.CollarId = D.CollarId
		   INNER JOIN inserted as I
				   -- To get the new disposal date, we need to link inserted and deleted collars by PK
				   ON I.CollarManufacturer = D.CollarManufacturer
				  AND I.CollarId = D.CollarId
				   -- These are the Locations to delete:
				WHERE I.DisposalDate < L.FixDate AND (L.FixDate < D.DisposalDate OR D.DisposalDate IS NULL) 
				

		  INSERT INTO Locations (ProjectId, AnimalId, FixDate, Location, FixId)
			   SELECT CD.ProjectId, CD.AnimalId, F.FixDate, geography::Point(F.Lat, F.Lon, 4326), F.FixId
				 FROM dbo.CollarFixes AS F
				   -- Join to CollarDeployments to get to the animal
		   INNER JOIN CollarDeployments AS CD
				   ON F.CollarManufacturer = CD.CollarManufacturer
				  AND F.CollarId = CD.CollarId
				  AND CD.DeploymentDate < F.FixDate
				  AND (F.FixDate < CD.RetrievalDate OR CD.RetrievalDate IS NULL)
				   -- Join these deployments to the collar being updated
		   INNER JOIN inserted AS I
				   ON CD.CollarManufacturer = I.CollarManufacturer
				  AND CD.CollarId = I.CollarId
		   INNER JOIN deleted as D
				   -- To get the new disposal date, we need to link inserted and deleted collars by PK
				   ON I.CollarManufacturer = D.CollarManufacturer
				  AND I.CollarId = D.CollarId
				   -- These are the Fixes to add:
				WHERE F.HiddenBy IS NULL
				  AND D.DisposalDate < F.FixDate AND (F.FixDate < I.DisposalDate OR I.DisposalDate IS NULL) 
	END
END

