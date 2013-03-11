Use [Animal_Movement]
Go

-- Test All Views 


-- Get all the Collar data embeded in the uploaded TPF files
-- Used by multiple queries in external files
SELECT TOP 100 * FROM AllTpfFileData

-- Used by AnimalMovement/FileDetailsForm.cs via the SQL to Linq DataModel
SELECT TOP 10 * FROM AnimalFixesByFile

-- used by ArgosDownloader/Program.cs to determine collars to download 
SELECT TOP 100 * FROM DownloadableAndAnalyzableCollars

-- Used by multiple queries in external files
SELECT TOP 100 * FROM DownloadableCollars

--Spatial layers
SELECT TOP 100 * FROM InvalidLocations
SELECT TOP 100 * FROM LastLocationOfKnownMortalities
SELECT TOP 100 * FROM MostRecentLocations
SELECT TOP 100 * FROM NoMovement
SELECT TOP 100 * FROM StoreOnBoardLocations
SELECT TOP 100 * FROM ValidLocations
SELECT TOP 100 * FROM ValidLocationsWithTempAndActivity
SELECT TOP 100 * FROM VelocityVectors

-- All the Email and AWS files that have not been processed 
SELECT TOP 100 * FROM UnprocessedArgosFile

