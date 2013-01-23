Use [Animal_Movement]
Go

-- Test All Views 
SELECT TOP 100 * FROM AnimalFixesByFile

SELECT TOP 100 * FROM AnimalsCurrentlyCollared
SELECT TOP 100 * FROM AnimalsNeverCollared
SELECT TOP 100 * FROM AnimalsNotCurrentlyCollared

SELECT TOP 100 * FROM CollarsCurrentlyDeployed
SELECT TOP 100 * FROM CollarsNeverDeployed
SELECT TOP 100 * FROM CollarsNotCurrentlyDeployed
SELECT TOP 100 * FROM CurrentDeployments

SELECT TOP 100 * FROM CollarsWithConflictingFixes

SELECT TOP 100 * FROM ERROR_CollarsWithOverlappingDeployments
SELECT TOP 100 * FROM ERROR_FixesWhichShouldBeLocations
SELECT TOP 100 * FROM ERROR_LocationsAfterAnimalsMortality
SELECT TOP 100 * FROM ERROR_LocationsOutsideBoundsOfDeployments

SELECT TOP 100 * FROM FixesByLocation

SELECT TOP 100 * FROM FixesNotHiddenInDeployment
SELECT TOP 100 * FROM FixesNotHiddenAndNotDeployed

SELECT TOP 100 * FROM InvalidLocations
SELECT TOP 100 * FROM NoMovement
SELECT TOP 100 * FROM ValidLocations
SELECT TOP 100 * FROM ValidLocationsWithTempAndActivity
SELECT TOP 100 * FROM VelocityVectors
SELECT TOP 100 * FROM MostRecentLocations
SELECT TOP 100 * FROM LastLocationOfKnownMortalities

SELECT TOP 100 * FROM StoreOnBoardLocations

SELECT TOP 100 * FROM WARNING_CollarsWithMultipleParameterFiles
SELECT TOP 100 * FROM WARNING_TelonicsCollarsMissingArgosId
SELECT TOP 100 * FROM WARNING_TelonicsCollarsSharingAnArgosId
SELECT TOP 100 * FROM WARNING_TelonicsGen3CollarsWithActivePpfFile
SELECT TOP 100 * FROM WARNING_TelonicsGen3CollarsWithoutPeriod
SELECT TOP 100 * FROM WARNING_TelonicsGenParameterFileMismatch