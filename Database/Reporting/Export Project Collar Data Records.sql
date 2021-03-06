--Extracts all of the collar file data for a project
DECLARE @Project varchar(255) = 'ARCNVSID022';

-- Argos Email Files
SELECT F.FileName, A.*
  FROM [CollarDataArgosEmail] AS A
  JOIN CollarFiles AS F ON F.FileId = A.FileId
  JOIN ArgosDeployments AS D ON D.PlatformId = A.PlatformId
  JOIN CollarDeployments AS C ON (C.CollarManufacturer = D.CollarManufacturer and C.CollarId = D.CollarId)
 WHERE C.ProjectId = @Project

-- Argos Web Service Files
SELECT F.FileName, A.*
  FROM [CollarDataArgosWebService] AS A
  JOIN CollarFiles AS F ON F.FileId = A.FileId
  JOIN ArgosDeployments AS D ON D.PlatformId = A.PlatformId
  JOIN CollarDeployments AS C ON (C.CollarManufacturer = D.CollarManufacturer and C.CollarId = D.CollarId)
 WHERE C.ProjectId = @Project

-- Debevek Files
SELECT F.FileName, A.*
  FROM [CollarDataDebevekFormat] AS A
  JOIN CollarFiles AS F ON F.FileId = A.FileId
  JOIN ArgosDeployments AS D ON D.PlatformId = A.PlatformId
  JOIN CollarDeployments AS C ON (C.CollarManufacturer = D.CollarManufacturer and C.CollarId = D.CollarId)
 WHERE C.ProjectId = @Project

-- Telonics Gen3 Datalog Files
SELECT F.FileName, A.*
  FROM [CollarDataTelonicsGen3] AS A
  JOIN CollarFiles AS F ON F.FileId = A.FileId
  JOIN CollarDeployments AS C ON (C.CollarManufacturer = F.CollarManufacturer and C.CollarId = F.CollarId)
 WHERE F.ProjectId = @Project OR C.ProjectId = @Project

-- Telonics Gen3 Store-On-Board Files
SELECT F.FileName, A.*
  FROM [CollarDataTelonicsGen3StoreOnBoard] AS A
  JOIN CollarFiles AS F ON F.FileId = A.FileId
  JOIN CollarDeployments AS C ON (C.CollarManufacturer = F.CollarManufacturer and C.CollarId = F.CollarId)
 WHERE F.ProjectId = @Project OR C.ProjectId = @Project

-- Telonics Gen4 Datalog Files (may includes Store-On-Board)
SELECT F.FileName, A.*
  FROM [CollarDataTelonicsGen4] AS A
  JOIN CollarFiles AS F ON F.FileId = A.FileId
  JOIN CollarDeployments AS C ON (C.CollarManufacturer = F.CollarManufacturer and C.CollarId = F.CollarId)
 WHERE F.ProjectId = @Project OR C.ProjectId = @Project
