declare @proj nvarchar(16) = 'WACH'
declare @duration_min int = 5

-- remove the first (or second) point in a brief vector if there is no movement in the vector   
SELECT * 
--UPDATE L SET Status = 'H' 
  from Locations as L
  join Movements as M
    on L.ProjectId = M.ProjectId AND
       L.AnimalId = M.AnimalId AND
       L.FixDate = M.StartDate
 where M.Distance = 0 AND M.Duration < (@duration_min/60.0) and M.ProjectId = @proj -- order by M.Duration


-- remove the first point in a brief vector if there is no movement in the preceeding vector   
SELECT * 
--UPDATE L SET Status = 'H'
  from Locations as L
  join Movements as M2
    on L.ProjectId = M2.ProjectId AND
       L.AnimalId = M2.AnimalId AND
       L.FixDate = M2.StartDate
  JOIN Movements as M1 
    on M1.ProjectId = M2.ProjectId AND
       M1.AnimalId = M2.AnimalId AND
       M1.EndDate = M2.StartDate
  JOIN Movements as M3 
    on M2.ProjectId = M3.ProjectId AND
       M2.AnimalId = M3.AnimalId AND
       M2.EndDate = M3.StartDate
 where M2.Duration < (@duration_min/60.0) and M2.ProjectId = @proj AND
       M1.Distance = 0

-- remove the second point in a brief vector if there is no movement in the following vector   
SELECT * 
--UPDATE L SET Status = 'H'
  from Locations as L
  join Movements as M2
    on L.ProjectId = M2.ProjectId AND
       L.AnimalId = M2.AnimalId AND
       L.FixDate = M2.EndDate
  JOIN Movements as M1 
    on M1.ProjectId = M2.ProjectId AND
       M1.AnimalId = M2.AnimalId AND
       M1.EndDate = M2.StartDate
  JOIN Movements as M3 
    on M2.ProjectId = M3.ProjectId AND
       M2.AnimalId = M3.AnimalId AND
       M2.EndDate = M3.StartDate
 where M2.Duration < (@duration_min/60.0) and M2.ProjectId = @proj AND
       M3.Distance = 0