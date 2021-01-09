-- Queries helpful for dealing with processing issues

-- Show issues
select * from ArgosFileProcessingIssues order by issue
select * from ArgosFileProcessingIssues where issue like 'ERROR%'

-- NOTE Issue 10334 'TDC Execution error ...' for fileid 13598 and collar 649744A
-- is probably due to some formatting issue in the merged email files.
-- However, this is no longer relevant, as the datalog files superceed this Argos data

-- 'No Collar' issues that have been resolved since first processed
select * from ArgosFileProcessingIssues as i 
left join ArgosDeployments as d
on d.PlatformId = i.PlatformId
where i.Issue like 'No Collar%'
and d.DeploymentId is not null
AND (D.StartDate IS NULL OR D.StartDate < i.FirstTransmission)
AND (D.EndDate IS NULL OR i.LastTransmission < D.EndDate)

-- No Parameter issues that have been resolved since first processed
select * from ArgosFileProcessingIssues as i 
left join CollarParameters as p
on p.CollarManufacturer = i.CollarManufacturer and p.CollarId = i.CollarId
where i.Issue like 'No Telonics Parameters%'
and p.ParameterId is not null
AND (P.StartDate IS NULL OR P.StartDate < i.FirstTransmission)
AND (P.EndDate IS NULL OR i.LastTransmission < P.EndDate)

-- NOTE issues 135 and 136 are due to Gen3 parameter files, but it isn't 
-- relevant since the dates 3/19/2013 to 3/25/2013 are after the
-- collars were generating useful locations, and those Argos Ids were retired 

-- By default the file processor will process all these files:
-- this query does not re-process files with issues
select * from ArgosFile_NeedsPartialProcessing

-- If you delete the issue, then the file processor will try and re-processes the issue
--delete from ArgosFileProcessingIssues where issueid in (347)

-- This is helpful for finding the child files and transmission groups in a parent file
-- replace the number on all three lines with your file(s) with issues
select * from collarfiles where fileid in (8280)
select * from collarfiles where parentfileid in (8280)
select * from ArgosFilePlatformDates where fileid in (8280)
