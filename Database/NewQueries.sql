USE [Animal_Movement]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[DownloadableAndAnalyzableCollars]
AS

SELECT CD.ProjectId, C.CollarManufacturer, C.CollarId
      ,I.Email, P.[UserName], P.[Password], A.PlatformId
      ,datediff(day,D.[TimeStamp],getdate()) AS [Days]
      ,C.CollarModel, C.Gen3Period, CPF.Contents AS TpfFile
  FROM
	           ArgosPlatforms AS A
	INNER JOIN ArgosPrograms AS P
	        ON A.ProgramId = P.ProgramId
	INNER JOIN ProjectInvestigators AS I
	        ON I.Login = P.Investigator
	INNER JOIN Collars AS C
	        ON C.AlternativeId = A.PlatformId
    INNER JOIN CollarDeployments as CD
            ON C.CollarManufacturer = CD.CollarManufacturer AND C.CollarId = CD.CollarId
     LEFT JOIN CollarParameters as CP
            ON C.CollarManufacturer = CP.CollarManufacturer AND C.CollarId = CP.CollarId
     LEFT JOIN CollarParameterFiles as CPF
            ON CP.FileId = CPF.FileId           
     LEFT JOIN (
               SELECT CollarManufacturer, CollarId, Max([Timestamp]) AS [Timestamp]
                 FROM ArgosDownloads
                WHERE ErrorMessage IS NULL
                GROUP BY CollarManufacturer, CollarId
               ) AS D
            ON C.CollarManufacturer = D.CollarManufacturer AND C.CollarId = D.CollarId

 WHERE A.[Status] = 'A'
   AND P.[Status] = 'A'
   AND (P.EndDate IS NULL OR getdate() < P.EndDate)
   AND (C.DisposalDate IS NULL OR getdate() < C.DisposalDate)
   AND (CD.RetrievalDate IS NULL OR getdate() < CD.RetrievalDate)
   AND (CPF.Format <> 'B' or CPF.[Status] <> 'A') -- Ignore collars with an active Gen3 PPF file 
   AND (C.Gen3Period IS NOT NULL OR (CPF.Format = 'A' AND CPF.[Status] = 'A')) -- Ignore collars without a Gen3 period or and active TPF file

GO



CREATE VIEW [dbo].[WARNING_ActiveArgosPlatformsWithAnalysisProblems]
AS

-- Active Argos platforms with active PPF or without Gen3period or TPF file
SELECT C.Manager, CD.ProjectId, CD.AnimalId, C.CollarModel, C.CollarId AS CTN
      ,A.PlatformId AS ArgosId
      ,C.Gen3Period, CPF.[FileName] AS ParameterFile
  FROM
	           ArgosPlatforms AS A
	INNER JOIN ArgosPrograms AS P
	        ON A.ProgramId = P.ProgramId
	INNER JOIN Collars AS C
	        ON C.AlternativeId = A.PlatformId
    INNER JOIN CollarDeployments as CD
            ON C.CollarManufacturer = CD.CollarManufacturer AND C.CollarId = CD.CollarId
     LEFT JOIN CollarParameters as CP
            ON C.CollarManufacturer = CP.CollarManufacturer AND C.CollarId = CP.CollarId
     LEFT JOIN CollarParameterFiles as CPF
            ON CP.FileId = CPF.FileId           
     LEFT JOIN (
               SELECT CollarManufacturer, CollarId, Max([Timestamp]) AS [Timestamp]
                 FROM ArgosDownloads
                WHERE ErrorMessage IS NULL
                GROUP BY CollarManufacturer, CollarId
               ) AS D
            ON C.CollarManufacturer = D.CollarManufacturer AND C.CollarId = D.CollarId

 WHERE A.[Status] = 'A'
   AND P.[Status] = 'A'
   AND (CPF.[Status] = 'A' OR CPF.[Status] IS NULL)
   AND (P.EndDate IS NULL OR getdate() < P.EndDate)
   AND (C.DisposalDate IS NULL OR getdate() < C.DisposalDate)
   AND (CD.RetrievalDate IS NULL OR getdate() < CD.RetrievalDate)
   AND (  (C.CollarModel = 'TelonicsGen3' AND CPF.Format <> 'B')
       OR (C.CollarModel = 'TelonicsGen3' AND C.Gen3Period IS NULL)
       OR (C.CollarModel = 'TelonicsGen4' AND CPF.Format IS NULL)
       OR (C.CollarModel = 'TelonicsGen4' AND CPF.Format <> 'A')
       )

GO



CREATE VIEW [dbo].[WARNING_ArgosPlatformsNotInCollars]
AS

-- Known Argos Platforms that are not in the Collars table 
    SELECT P.*
      FROM ArgosPlatforms AS P
 LEFT JOIN Collars AS C
        ON P.PlatformId = C.AlternativeId
     WHERE C.CollarManufacturer IS NULL

GO


CREATE VIEW [dbo].[WARNING_ArgosPlatformsNotTelonicsGen3or4]
AS

-- Known Argos Platforms that are not a Telonics Gen3/4 GPS Collars 
    SELECT P.*, C.*
      FROM ArgosPlatforms AS P
 LEFT JOIN Collars AS C
        ON P.PlatformId = C.AlternativeId
     WHERE C.CollarManufacturer IS NOT NULL
       AND C.CollarModel NOT IN ('TelonicsGen3', 'TelonicsGen4')

GO


CREATE VIEW [dbo].[WARNING_TelonicArgosCollarsWithNoPlatform]
AS

-- Telonics Gen3/4 Argos GPS Collars with no matching record in ArgosPlatforms
    SELECT C.Manager, C.CollarModel, C.CollarId as CTN, C.AlternativeId as ArgosID, C.Frequency, C.DisposalDate
      FROM Collars AS C
 LEFT JOIN ArgosPlatforms AS P
        ON P.PlatformId = C.AlternativeId
     WHERE C.AlternativeId IS NOT NULL
       AND P.PlatformId IS NULL
       AND C.CollarModel IN ('TelonicsGen3', 'TelonicsGen4')

GO


CREATE VIEW [dbo].[WARNING_AllTelonicGen4CollarsWithoutActiveTpfFile]
AS

-- All Telonics Gen4 Argos Collars without a active TPF File
    SELECT C.Manager, CD.ProjectId, CD.AnimalId, C.CollarId as CTN, C.AlternativeId as ArgosID,  
           C.Frequency, C.DisposalDate, CD.RetrievalDate, C.Notes,
           CPF.[FileName] AS ParameterFile, CPF.Format, CPF.[Status]
      FROM Collars AS C
INNER JOIN CollarDeployments AS CD
        ON C.CollarManufacturer = CD.CollarManufacturer AND C.CollarId = CD.CollarId
 LEFT JOIN CollarParameters as CP
        ON C.CollarManufacturer = CP.CollarManufacturer AND C.CollarId = CP.CollarId
 LEFT JOIN CollarParameterFiles as CPF
        ON CP.FileId = CPF.FileId           
     WHERE C.CollarModel = 'TelonicsGen4'
       AND C.AlternativeId IS NOT NULL
       AND (CPF.Format IS NULL OR (CPF.Format = 'A' AND CPF.[Status] = 'I'))
GO



