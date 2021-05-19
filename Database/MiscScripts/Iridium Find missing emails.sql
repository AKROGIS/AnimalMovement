--select top 8 * from CollarDataIridiumMail where Imei = '300434063599390'

--select collarid, manager from Collars where CollarManufacturer = 'Telonics' and CollarID between '713460' and '713470'
--select collarid, manager from Collars where CollarManufacturer = 'Telonics' and CollarID between '713460' and '713468'
--select collarid, manager from Collars where CollarManufacturer = 'Telonics' and CollarID = '713470'

--update collars set manager = 'NPS\BAMangipane' where CollarManufacturer = 'Telonics' and CollarID between '713460' and '713468'

select top 1 * from CollarDataIridiumMail group by left(MessageTime, 13)
select top 10  left(MessageTime, 13), count(*) from CollarDataIridiumMail group by left(MessageTime, 13) order by  left(MessageTime, 13) desc

select * from LookupCollarFileFormats

SELECT * INTO #Temp FROM (
select datepart(DAYOFYEAR, uploaddate) as day, datepart(HOUR, uploaddate) as hour, count(*) as count
from collarFiles where format = 'I' and UploadDate > '2020'
group by datepart(DAYOFYEAR, uploaddate), datepart(HOUR, uploaddate)
--order by datepart(DAYOFYEAR, uploaddate) desc, datepart(HOUR, uploaddate) desc
) as temp

select hour, count(*) from #Temp group by hour

select day from #Temp group by day having count(*) < 3

select * from #Temp where day in (select day from #Temp group by day having count(*) < 3) order by day, hour

select dateadd(day,154,'2020'), dateadd(day,64,'2020'), dateadd(day,65,'2020'),dateadd(day,165,'2020'),dateadd(day,187,'2020'),dateadd(day,231,'2020')

--averages: 4am: 49, 9am: 8, 1pm: 10

--1/9 9am, 3/4 9am, 3/5 9am, 6/13 4am, 7/5 9am, 8/18 1pm
-- + 28 files at 10am on 6/3 (155) ater 4 at 9am

select hour, avg(count) from #Temp group by hour
select * from #Temp where day = 155

select * from Settings


select left(MessageTime, 10) as date, count(*) as cnt
from CollarDataIridiumMail where left(MessageTime, 4) = '2020'
group by left(MessageTime, 10) order by left(MessageTime, 10)

select top 3 * from CollarDataIridiumMail order by MessageTime desc

select dateadd(hour, -8, convert(datetime2, '2020.08.18 12:44:29'))

SELECT * INTO #Temp2 FROM (
select dateadd(hour, -8, convert(datetime2, MessageTime)) as date, Imei from CollarDataIridiumMail where left(MessageTime, 4) = '2020' --order by MessageTime desc
) as x2

SELECT * INTO #Temp3 FROM (
select datepart(month, [date]) as month, DATEPART(DAY, [date]) as day, count(*) as cnt from #Temp2 group by DATEPART(DAY, [date]), datepart(month, [date])
) as x3

select * from #temp3 order by month desc, day desc

select count(*) as db_count, count(*) - 1900 as gmail_processed from CollarDataIridiumMail

select MessageTime, Imei, dateadd(hour, -8, convert(datetime2, MessageTime)) as mdate  from CollarDataIridiumMail
where dateadd(hour, -8, convert(datetime2, MessageTime)) < '2020-08-10' and dateadd(hour, -8, convert(datetime2, MessageTime)) > '2020-08-09'
order by MessageTime desc

drop table #Temp2
drop table #Temp3


select top 200 f.fileid, f.[Owner], f.ProjectId, f.CollarId, i.Imei from CollarDataIridiumMail as i 
left join CollarFiles as f on i.FileId = f.FileId
 where i.imei in (
'300434064633890',
'300434064633900',
'300434064638900',
'300434064639890',
'300434064732540',
'300434064733180',
'300434064733190',
'300434064733200',
'300434064733350',
'300434064733540',
'300434064734170',
'300434064734360',
'300434064735180',
'300434064735190',
'300434064735360',
'300434064736350',
'300434064736360',
'300434064737160',
'300434064738150',
'300434064739170',
'300434064739540'
)
