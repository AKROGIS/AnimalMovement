-- Determine the "Owner" of an Iridium message
-- ###########################################

-- Iridium numbers that can be processed must be in a TPF file.  The TPF file has an owner.  The TPF file
-- links the IMEI number with a Telonics Collar which has a Manager.  Nominally, the TPF file owner and the
-- Collar Manager are the same Project Investigator.

-- 1) Find the Iridium IDs that have different file owner and collar manager
select t.PlatformId as IMEI, f.[Owner] as [TPF Owner], c.manager as [Collar Owner]
from AllTpfFileData as t 
left join CollarParameterFiles as f on f.filename = t.[FileName]
left join Collars as c on c.CollarManufacturer = 'Telonics' and (c.CollarId = t.CTN or left(c.CollarId,6) = left(t.CTN, 6))
where t.Platform = 'Iridium' and  f.owner <> c.Manager
group by f.owner, c.Manager, platformID
order by f.owner, c.Manager, platformID

-- At this point (2021-05-18) there is only one IMEI number with different owners, which is good.
-- IMEI: 300434063599390, TPF Owner: NPS\BAMangipane, Collar Manager: NPS\BBorg
-- I will use the TPF Owner, since that is most likely the person who is currently managing the collar.

-- Collars usually do not change Iridium IDs, so we are probably only seeing the current owner of the collar
-- The Iridium ID could be in several TPF files with different owners.  Need to check if the Iridium ID
-- belongs to different TPF files and possibly differnt TPF file owners over it's life.

-- 2) Iridium IMEI # with multiple files
select PlatformId, count(*) as cnt from AllTpfFileData where Platform = 'Iridium' group by PlatformId having count(*) > 1

-- 3) Iridium IDs and owners with multiple files
select t.PlatformId as IMEI, f.[Owner] as [TPF Owner], c.manager as [Collar Owner] from 
(select PlatformId from AllTpfFileData where Platform = 'Iridium' group by PlatformId having count(*) > 1) as t2
left join  AllTpfFileData as t on t.PlatformId = t2.PlatformId
left join CollarParameterFiles as f on f.filename = t.[FileName]
left join Collars as c on c.CollarManufacturer = 'Telonics' and (c.CollarId = t.CTN or left(c.CollarId,6) = left(t.CTN, 6))
group by t.platformID, f.owner, c.Manager
order by  t.platformID, f.owner, c.Manager

-- Ideally, the count of query #3 will equal the count of query #2 (I.e, no IMEI numbers are owned by different people)
-- However if the counts do not match query3 > query2, then scan Query3 results for changes in owners.  If the IMEI
-- number does not change, then note the different owners.  Run queries (to be developed) to determine the date of the
-- change, and make sure the correct IMEI emails are filed with the correct owner.

-- 4) Count of unique Iridium IMEI Numbers
Select count(*) from (Select 1 as c from AllTpfFileData where Platform = 'Iridium' group by PlatformId) as t

-- 5) Count of unique Iridium IMEI Numbers by TPF Owner
select owner, count(*) as Count from (
    select f.owner as owner, t.platformID FROM
    AllTpfFileData as t
    left join CollarParameterFiles as f on f.FileId = t.FileId
    where t.Platform = 'Iridium'
    group by f.owner, t.platformID
) as d
group by owner
order by owner

-- The count of query 4 must match the total of the counts in query 5
-- For each owner in the results of query 5, run the following query and save as a file for that owner

-- 6) All IMEIs owned by the given PI
select PlatformID as IMEI from AllTpfFileData as t left join CollarParameterFiles as f on f.FileId = t.FileId
where t.Platform = 'Iridium' and f.owner = 'NPS\BAMangipane' group by PlatformID order by PlatformID

-- 6) All IMEIs owned by the given PI
select PlatformID as IMEI from AllTpfFileData as t left join CollarParameterFiles as f on f.FileId = t.FileId
where t.Platform = 'Iridium' and f.owner = 'NPS\BBorg' group by PlatformID order by PlatformID

-- 6) All IMEIs owned by the given PI
select PlatformID as IMEI from AllTpfFileData as t left join CollarParameterFiles as f on f.FileId = t.FileId
where t.Platform = 'Iridium' and f.owner = 'NPS\JAPutera' group by PlatformID order by PlatformID

-- 6 ) All IMEIs owned by the given PI
select PlatformID as IMEI from AllTpfFileData as t left join CollarParameterFiles as f on f.FileId = t.FileId
where t.Platform = 'Iridium' and f.owner = 'NPS\KCJoly' group by PlatformID order by PlatformID

-- 6) All IMEIs owned by the given PI
select PlatformID as IMEI from AllTpfFileData as t left join CollarParameterFiles as f on f.FileId = t.FileId
where t.Platform = 'Iridium' and f.owner = 'NPS\MDCameron' group by PlatformID order by PlatformID

-- 6) All IMEIs owned by the given PI
select PlatformID as IMEI from AllTpfFileData as t left join CollarParameterFiles as f on f.FileId = t.FileId
where t.Platform = 'Iridium' and f.owner = 'NPS\msorum' group by PlatformID order by PlatformID

-- Note that not all of the IMEI numbers may be active

-- 7) Active IMEI Numbers and the count of email messages recieved between the first and last date
select Imei, count(*) as count, min(MessageTime) as first, max(MessageTime) as last from CollarDataIridiumMail group by Imei

-- 8) Count of unique Iridium IMEI Numbers by TPF Owner
select owner, count(*) as Count from (
    select f.owner as owner, t.platformID FROM
    (select Imei from CollarDataIridiumMail group by Imei) as i
    left join AllTpfFileData as t on i.Imei = t.PlatformId
    left join CollarParameterFiles as f on f.FileId = t.FileId
    group by f.owner, t.platformID
) as d
group by owner
order by owner

-- 9) Count of IMEI messages by most recent TPF file owner and IMEI
select Owner, Imei, count(*) as count, min(MessageTime) as first, max(MessageTime) as last
from CollarDataIridiumMail as i
left join (Select PlatformId, Max(FileId) as FileId from AllTpfFileData where Platform = 'Iridium' group by PlatformId) as t on i.Imei = t.PlatformId
left join CollarParameterFiles as f on f.FileId = t.FileId
group by owner, Imei
order by owner, Imei

-- Pat Owen has "borrowed" collars from Bridget Borg (and 1 from Buck Mangipane).  She has elected not to send her Iridium emails to AKR.
-- She is or has used the using the following numbers,  All but the last one are "owned" by Bridget.  The last one is owned by Buck
-- 300434061185990 | 300434061232970 | 300434061234960 | 300434061235970 | 300434061236950 | 300434061237990 | 300434061281030 | 300434061283000 | 300434061285020 | 300434061285030 | 300434061286000 | 300434061289030 | 300434061331010 | 300434063599390
-- '300434061185990','300434061232970','300434061234960','300434061235970','300434061236950','300434061237990','300434061281030','300434061283000','300434061285020','300434061285030','300434061286000','300434061289030','300434061331010','300434063599390'

-- 10) The "Owners" of Pats collars not sending data to AKR email
select Owner, Imei, count(*) as count, min(MessageTime) as first, max(MessageTime) as last
from CollarDataIridiumMail as i
left join (Select PlatformId, Max(FileId) as FileId from AllTpfFileData where Platform = 'Iridium' group by PlatformId) as t on i.Imei = t.PlatformId
left join CollarParameterFiles as f on f.FileId = t.FileId
where imei in ('300434061185990','300434061232970','300434061234960','300434061235970','300434061236950','300434061237990','300434061281030','300434061283000','300434061285020','300434061285030','300434061286000','300434061289030','300434061331010','300434063599390')
group by owner, Imei
order by owner, Imei

-- Count of total email files for Pat; This should match gmail right after the DB processes Iridium messages (@ 04:00, 09:00, and 13:00)
select count(*) from CollarDataIridiumMail
where Imei in ('300434061185990','300434061232970','300434061234960','300434061235970','300434061236950','300434061237990','300434061281030','300434061283000','300434061285020','300434061285030','300434061286000','300434061289030','300434061331010','300434063599390')

-- Count of total email files for Other owners; This should match AKR right after the DB processes Iridium messages (@ 04:00, 09:00, and 13:00)
select Owner, sum(cnt) as total from (
    select Owner, imei, count(*) as cnt
    from CollarDataIridiumMail as i
    left join (Select PlatformId, Max(FileId) as FileId from AllTpfFileData where Platform = 'Iridium' group by PlatformId) as t on i.Imei = t.PlatformId
    left join CollarParameterFiles as f on f.FileId = t.FileId
    where Imei not in ('300434061185990','300434061232970','300434061234960','300434061235970','300434061236950','300434061237990','300434061281030','300434061283000','300434061285020','300434061285030','300434061286000','300434061289030','300434061331010','300434063599390')
    group by owner, imei
) as d 
group by [Owner]

-- Iridium messages by year and month (based on messagetime in UTC)
select left(messageTime,4) as year, SUBSTRING(messageTime,6,2) as month, count(*) as cnt from CollarDataIridiumMail
group by left(messageTime,4), SUBSTRING(messageTime,6,2)
order by left(messageTime,4), SUBSTRING(messageTime,6,2)

-- Iridium messages for every day in Feb 2020 Google -> Outlook was on 2/12/2020 (based on messagetime in UTC)
select '2020' as Year, '12' as month, SUBSTRING(messageTime,9,2) as day, count(*) as cnt from CollarDataIridiumMail
where messageTime like '2020.12.%'
  and Imei not in ('300434061185990','300434061232970','300434061234960','300434061235970','300434061236950','300434061237990','300434061281030','300434061283000','300434061285020','300434061285030','300434061286000','300434061289030','300434061331010','300434063599390')
group by SUBSTRING(messageTime,9,2)
order by SUBSTRING(messageTime,9,2)

-- All Messages on a given day
select IMEI, messageTime from CollarDataIridiumMail where messageTime like '2020.09.04%'
  and Imei not in ('300434061185990','300434061232970','300434061234960','300434061235970','300434061236950','300434061237990','300434061281030','300434061283000','300434061285020','300434061285030','300434061286000','300434061289030','300434061331010','300434063599390')
 order by MessageTime desc

-- x) Show the IMEI Numbers are used by a project (The DENA Bear collars are not sent to AKR)
-- **Note:** This will not show IMEIs where the Iridium data has been "replaced" by store on board data
select i.IMei from CollarDataIridiumMail as i 
join collarfiles as f on f.ParentFileId = i.FileId
join CollarFixes as f2 on f2.fileid = f.fileid
join locations as l on l.FixId = f2.fixid 
where l.projectid = 'DENA_Bears'
group by i.IMei
order by i.IMei

-- x) Show which project is using an IMEI
-- **Note:** This will not show projects if the Iridium data has been "replaced" by store on board data
select l.ProjectId from CollarDataIridiumMail as i 
join collarfiles as f on f.ParentFileId = i.FileId
join CollarFixes as f2 on f2.fileid = f.fileid
join locations as l on l.FixId = f2.fixid 
where i.Imei in ('300434061232970', '300434061237990', '300434061286000')
group by l.ProjectId

-- x) Show which collar is using an IMEI
-- **Note:** This will not show projects if the Iridium data has been "replaced" by store on board data
select f2.CollarId from CollarDataIridiumMail as i 
join collarfiles as f on f.ParentFileId = i.FileId
join CollarFixes as f2 on f2.fileid = f.fileid
where i.Imei in ('300434061232970', '300434061237990', '300434061286000')
group by f2.CollarId

