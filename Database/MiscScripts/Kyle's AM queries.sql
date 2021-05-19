-- Miscellaneous queries for Kyle
--- #############################

-- I've forgotten why I wrote this, but it Looks like mostly stuff for
-- finding duplicates and normalizing the fix period

select top 10 * from movements
select top 10 * from locations

-- typical sampling intervals
select animalid, round(duration,1) duration, count(*) as count
from Movements
where Projectid = 'DENA_Bears'
  and startdate >= '2018-09-01'
group by animalid, round(duration,1)
order by animalid, count(*) desc

select animalid, round(duration,1) duration, count(*) as count
from Movements
where Projectid = 'WACH'
  and startdate >= '2018-09-01'
group by animalid, round(duration,1) having round(duration,1) <= 9
order by round(duration,1) --count(*) desc
-- '1902', '1931', '1918', '1905', '1906', ' 1915', '1926', '1802', '1911', '1916', '1923', '1703', '1704', '1908'

select top 10 *, cast(fixdate as time) as [time], datepart(hour, cast(fixdate as time)) as hour, datepart(minute, cast(fixdate as time)) as minute  from Locations
where projectid = 'WACH'
  and [Status] is null
  and AnimalID in ('1902', '1931', '1918', '1905', '1906', ' 1915', '1926', '1802', '1911', '1916', '1923', '1703', '1704', '1908')

select datepart(minute, cast(fixdate as time)) as minute, count(*)  from Locations
where projectid = 'WACH'
  and [Status] is null
  and AnimalID in ('1902', '1931', '1918', '1905', '1906', ' 1915', '1926', '1802', '1911', '1916', '1923', '1703', '1704', '1908')
group by datepart(minute, cast(fixdate as time))
order by datepart(minute, cast(fixdate as time))

-- 2019 wach "double" fix (two fixes at same location within a few minutes)
select * from Movements
where projectid = 'WACH'
  and duration < 0.95
  and startdate >= '2017-09-01'
  order by duration

-- wach 'second' locations with "double" fix (two fixes at same location within a few minutes)
  select l.* from Locations as l
  join (
    select projectid, animalid, Enddate from Movements
    where projectid = 'WACH'
    and duration < 0.95
    --and distance =0
    and startdate >= '2018-09-01'
  ) as m on l.projectid = m.projectid and l.animalid = m.animalid and l.fixdate = m.enddate

-- hide wach 'second' locations
  --update l set [Status] = 'H' from Locations as l
  join (
    select projectid, animalid, Enddate from Movements
    where projectid = 'WACH'
    and duration < 0.95
    and distance =0
    and startdate >= '2018-09-01'
  ) as m on l.projectid = m.projectid and l.animalid = m.animalid and l.fixdate = m.enddate


select fixdate, Location.Lat, Location.Long, status from locations where projectid = 'WACH' and animalid = '1501' and fixdate > '2019-07-05' and fixdate < '2019-07-08' order by fixdate


-- wach caribou this year only points at 00, 08, 16, and 24 hours (typical sampling period
select top 10 *, cast(fixdate as time) as [time], datepart(hour, cast(fixdate as time)) as hour, datepart(minute, cast(fixdate as time)) as minute  from Locations
where projectid = 'WACH'
  and [Status] is null
  and AnimalID in ('1902', '1931', '1918', '1905', '1906', ' 1915', '1926', '1802', '1911', '1916', '1923', '1703', '1704', '1908')
  and datepart(hour, cast(fixdate as time)) not in (0,8,16)
  and datepart(minute, cast(fixdate as time)) not in (0,1)

--- ***************************************
-- wach caribou this year only points at 00, 08, 16, and 24 hours (typical sampling period
-- ***************************************
select datepart(hour, cast(fixdate as time)) as hour, datepart(minute, cast(fixdate as time)) as minute, count(*) as ignored FROM (
select * from Locations
where projectid = 'WACH'
  and [Status] is null
  and fixdate >= '2018-09-01'
  and fixdate < '2019-09-01'
  and (datepart(hour, cast(fixdate as time)) not in (0,8,16, 5,13,21)
  OR datepart(minute, cast(fixdate as time)) not in (0, 1) )
) as skip
group by datepart(hour, cast(fixdate as time)), datepart(minute, cast(fixdate as time))
order by datepart(hour, cast(fixdate as time)), datepart(minute, cast(fixdate as time))

-- ***************************************
-- wach caribou this year only points at 00, 08, 16, and 24 hours (typical sampling period
-- ***************************************
select datepart(hour, cast(fixdate as time)) as hour, datepart(minute, cast(fixdate as time)) as minute, count(*) as count FROM (
select * from Locations
where projectid = 'WACH'
  and [Status] is null
  and fixdate >= '2018-09-01'
  and fixdate < '2019-09-01'
  and datepart(hour, cast(fixdate as time)) in (0,8,16, 5,13,21)
  and datepart(minute, cast(fixdate as time)) in (0, 1)
) as skip
group by datepart(hour, cast(fixdate as time)), datepart(minute, cast(fixdate as time))
order by datepart(hour, cast(fixdate as time)), datepart(minute, cast(fixdate as time))


-- GPS metadata for dup fixes (first fix)
select * from CollarDataTelonicsGen4 as d
  join collarfixes as f on d.FileId = f.FileId and d.LineNumber = f.LineNumber
where f.fixid in (
  select l.fixid from Locations as l
  join (
    select projectid, animalid, StartDate from Movements
    where projectid = 'WACH'
    and duration < 0.1
    and startdate >= '2018-09-01'
  ) as m on l.projectid = m.projectid and l.animalid = m.animalid and l.fixdate = m.StartDate
)
order by f.FixDate -- d.fileid, d.LineNumber

-- GPS metadata for dup fixes (second fix)
select * from CollarDataTelonicsGen4 as d
  join collarfixes as f on d.FileId = f.FileId and d.LineNumber = f.LineNumber
where f.fixid in (
  select l.fixid from Locations as l
  join (
    select projectid, animalid, Enddate from Movements
    where projectid = 'WACH'
    and duration < 0.1
    and startdate >= '2018-09-01'
  ) as m on l.projectid = m.projectid and l.animalid = m.animalid and l.fixdate = m.EndDate
)
order by f.FixDate -- d.fileid, d.LineNumber

select * from CollarDataTelonicsGen4 as d
join collarfixes as f on d.FileId = f.FileId and d.LineNumber = f.LineNumber
join locations as l on l.fixid = f.fixid
where d.fileid = 155331	 and PredeploymentData = 'Yes'

-- ***************************************
-- Locations which should be hidden
-- ***************************************
select l.* from Locations as L
join collarfixes as f on l.fixid = f.fixid
join CollarDataTelonicsGen4 as d on d.FileId = f.FileId and d.LineNumber = f.LineNumber
where projectid <> 'WACH'
    and l.fixdate >= '2018-09-01'
    --and l.fixdate < '2018-09-01'
    and status is null
and d.PredeploymentData = 'Yes'

select * from Locations
where projectid = 'WACH'
and animalid = '1105'
    and fixdate > '2018-09-05'
    and fixdate < '2018-09-12'

select * from CollarDataTelonicsGen4 as d
join collarfixes as f on d.FileId = f.FileId and d.LineNumber = f.LineNumber
join locations as l on l.fixid = f.fixid
where f.fixid in (5879660	, 5886707	) d.fileid = 155331	 and PredeploymentData = 'Yes'


--update locations set Status = null where fixid = 5886707
--update locations set Status = 'H' where fixid in (5879660, 6242761, 6197004	)
