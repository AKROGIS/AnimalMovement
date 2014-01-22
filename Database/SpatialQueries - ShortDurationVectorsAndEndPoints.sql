-- Spatial queries for identifying and removing short duration vectors

select M1.*
  from Movements as M1
  JOIN Movements as M2 
    on M1.ProjectId = M2.ProjectId AND
       M1.AnimalId = M2.AnimalId AND
       (M1.EndDate = M2.StartDate OR M1.StartDate = M2.StartDate OR M1.StartDate = M2.endDate)
 where M2.Duration < (5/60.0) and M2.ProjectId = 'WACH'
 
 select L.*
  from Locations as L
  JOIN Movements as M 
    on M.ProjectId = L.ProjectId AND
       M.AnimalId = L.AnimalId AND
       (M.EndDate = L.FixDate OR M.StartDate = L.FixDate)
 where M.Duration < (5/60.0) and M.ProjectId = 'WACH'