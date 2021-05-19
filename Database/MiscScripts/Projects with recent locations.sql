select projectid, count(*) from locations where FixDate > '20210401' group by projectid

select projectid, count(*) from locations where FixDate > '20210101' group by projectid

select projectid, count(*) from ValidLocations_NPS where FixDate > '20210101' group by projectid

select projectid, count(*) from locations group by projectid

prior years, this year