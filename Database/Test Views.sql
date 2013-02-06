Use [Animal_Movement]
Go

-- Test All Views 

-- Get all the Collar data embeded in the uploaded TPF files
SELECT TOP 100 * FROM AllTpfFileData

-- query for display in Application
SELECT TOP 100 * FROM AnimalFixesByFile

-- old queries (no longer used?) that filter animals/collars by deployment
SELECT TOP 100 * FROM AnimalsCurrentlyCollared
SELECT TOP 100 * FROM AnimalsNeverCollared
SELECT TOP 100 * FROM AnimalsNotCurrentlyCollared
SELECT TOP 100 * FROM CollarsCurrentlyDeployed
SELECT TOP 100 * FROM CollarsNeverDeployed
SELECT TOP 100 * FROM CollarsNotCurrentlyDeployed

-- query for display in Application
SELECT TOP 100 * FROM CollarsWithConflictingFixes

-- query for display in Application
SELECT TOP 100 * FROM CurrentDeployments

-- used by ArgosDownloader to determine collars to download 
SELECT TOP 100 * FROM DownloadableAndAnalyzableCollars
SELECT TOP 100 * FROM DownloadableCollars

-- Checks for error conditions
SELECT TOP 100 * FROM ERROR_CollarsWithOverlappingDeployments
SELECT TOP 100 * FROM ERROR_FixesWhichShouldBeLocations
SELECT TOP 100 * FROM ERROR_LocationsAfterAnimalsMortality
SELECT TOP 100 * FROM ERROR_LocationsOutsideBoundsOfDeployments

-- query for display in Application
SELECT TOP 100 * FROM FixesByLocation

-- Check for fixes that are not displayable
SELECT TOP 100 * FROM FixesNotHiddenAndNotDeployed
-- Pre-query to make previous query easier to understand
SELECT TOP 100 * FROM FixesNotHiddenInDeployment

--Spatial layers
SELECT TOP 100 * FROM InvalidLocations
SELECT TOP 100 * FROM LastLocationOfKnownMortalities
SELECT TOP 100 * FROM MostRecentLocations
SELECT TOP 100 * FROM NoMovement
SELECT TOP 100 * FROM StoreOnBoardLocations
SELECT TOP 100 * FROM ValidLocations
SELECT TOP 100 * FROM ValidLocationsWithTempAndActivity
SELECT TOP 100 * FROM VelocityVectors

-- Warnings that may prevent downloading/analysis of collars
SELECT TOP 100 * FROM WARNING_ActiveArgosPlatformsWithAnalysisProblems
SELECT TOP 100 * FROM WARNING_AllTelonicGen4CollarsWithoutActiveTpfFile
SELECT TOP 100 * FROM WARNING_ArgosPlatformsNotInCollars
SELECT TOP 100 * FROM WARNING_ArgosPlatformsNotTelonicsGen3or4
SELECT TOP 100 * FROM WARNING_CollarsWithMultipleParameterFiles
SELECT TOP 100 * FROM WARNING_TelonicArgosCollarsWithNoPlatform
SELECT TOP 100 * FROM WARNING_TelonicsCollarsMissingArgosId
SELECT TOP 100 * FROM WARNING_TelonicsCollarsSharingAnArgosId
SELECT TOP 100 * FROM WARNING_TelonicsGen3CollarsWithActivePpfFile
SELECT TOP 100 * FROM WARNING_TelonicsGen3CollarsWithoutPeriod
SELECT TOP 100 * FROM WARNING_TelonicsGenParameterFileMismatch