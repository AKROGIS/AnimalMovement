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

SELECT TOP 100 * FROM FixesByLocation
SELECT TOP 100 * FROM InvalidLocations
SELECT TOP 100 * FROM NoMovement
SELECT TOP 100 * FROM ValidLocations
SELECT TOP 100 * FROM VelocityVectors

SELECT TOP 100 * FROM StoreOnBoardLocations
