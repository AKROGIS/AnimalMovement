-- "Heat Map" of caribou tracks
-- ####################################

--  This was a request by Kyle to summariz the caribou data by grid point. A
-- raster was insufficient, because he wanted the details of all the caribou
-- that went through a cell.  The cells were developed by truncating the lat/long
-- location data.
-- 10 million meters is defined as 90 degrees latitude
--   =>  0.0000d to 0.0001d = 11.11 meters => xx.xxxxd is +/- 11.11m  (approximate)
-- by rounding lat/long to 4 significant digits we are "binning" or "gridding"
-- the data into cells approximately 11m^2
-- It might have been that an 11m^2 grid was too many grid points to deal with
-- so I used 3 significant digits in the final analysis (111m^2 grids)

-- This uses the VectorToPoints function in the adjacent file


-- delete the table
drop table  WACH_GridPoints

-- Create the table with the end points of all WACH movement vectors that are not also the start point of a different vector

SELECT * INTO WACH_GridPoints FROM (
    -- End points  with following vector
    select
    --L.ProjectID,
    L.AnimalID,
    --L.FixDate,
    dbo.LocalTime(L.FixDate) LocalFixDate,
    dbo.DateTimeToOrdinal(dbo.LocalTime(L.FixDate)) AS LocalJulian,
    round(Location.Lat,4) AS Lat, Round(Location.Long,4) as Long,
    M1.Speed,
    M1.Duration,
    M1.Distance
    from Locations as L
    left join Movements as M1
    ON L.ProjectID = M1.ProjectID AND L.AnimalID = M1.AnimalID AND L.FixDate = M1.EndDate
    left join Movements as M2
    ON L.ProjectID = M2.ProjectID AND L.AnimalID = M2.AnimalID AND L.FixDate = M2.StartDate
    Where L.Status is null and M2.ProjectID IS NULL
    AND L.ProjectID = 'WACH'
) AS Ends

-- Add all the start and intermediate points for all WACH movement vectors

INSERT INTO WACH_GridPoints
    -- Starting point and intermediate points along vector every .001 degree (111.1meters);
    -- does not include end point (same as starting point of subsequent vector)
    SELECT
        --M.ProjectID,
        M.AnimalID,
        --M.FixDate,
        dbo.LocalTime(P.FixDate) AS LocalFixDate,
        dbo.DateTimeToOrdinal(dbo.LocalTime(P.FixDate)) AS LocalJulian,
        P.Lat, P.Long,
        P.Speed,
        P.Duration,
        P.Distance
    FROM Movements as M
    CROSS APPLY (SELECT * FROM dbo.VectorToPoints(M.ProjectID, M.AnimalId, M.StartDate)) AS P
        WHERE M.ProjectID = 'WACH' -- AND M.AnimalID like '09%'


-- Create the aggregate table

SELECT * INTO WACH_GridPoint_Summary FROM (
    Select P.Lat, P.Long, P.TotalCount, P.AvgSpeed, A.AnimalCount, D.DaysCount
    FROM (
        select Lat, Long, count(*) as TotalCount, AVG(Speed) AS AvgSpeed
        FROM WACH_GridPoints Group by Lat, Long
    ) AS P
    LEFT JOIN (
        -- The number of distinct animals that occupied this grid point for 1 or more days
        select Lat, Long, count(*) as AnimalCount
        -- An animal occupied this grid point for 1 or more days (in any year)
        FROM (select Lat, Long, Animalid, count(*) as c From WACH_GridPoints Group by Lat, Long, animalID having count(*) > 0) as A1
        group by Lat, Long
    ) AS A
    ON P.Lat = A.Lat AND P.Long = A.Long
    LEFT JOIN (
        -- The number of distinct days that had one or more animals occupying this grid point
        select Lat, Long, count(*) as DaysCount
        -- A julian date (in any year) with one or more animal occupying this grid point
        FROM (select Lat, Long, LocalJulian, count(*) as c From WACH_GridPoints Group by Lat, Long, LocalJulian having count(*) > 0) as D1
        group by Lat, Long
    ) AS D
    ON P.Lat = D.Lat AND P.Long = D.Long
) as sum
