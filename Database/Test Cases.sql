-- ========================================
-- Test Module for Animal Movement Triggers
--
-- To test:
--   1) Set the step to 1 then run.
--   2) verify correct display in ArcMap
--   3) Set the step to a test number then run
--   4) verify correct display in ArcMap
--   5) Repeat for as many tests as desired
--   6) Set the step to 99 then run (to clear test data).
-- ========================================

DECLARE @Step INT = 99;

IF @Step = 1
-- Clear the slate, and insert some test data;
BEGIN
	DELETE FROM [dbo].[Movement] WHERE [AnimalId] = 'test';
	DELETE FROM [dbo].[Locations] WHERE [AnimalId] = 'test';
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-10',65.01,-160.0+0.09,'H');
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-11',65.04,-160.0+0.10,'H');
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-12',65.02,-160.0+0.11,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-13',65.02,-160.0+0.12,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-14',65.04,-160.0+0.13,'H');
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-16',65.04,-160.0+0.15,'H');
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-15',65.00,-160.0+0.14,NULL); -- put a V between two H
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-17',65.02,-160.0+0.16,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-19',65.00,-160.0+0.18,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-18',65.02,-160.0+0.17,NULL); -- put a V between two V
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-20',65.04,-160.0+0.19,'H');
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-21',65.00,-160.0+0.20,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-22',65.02,-160.0+0.21,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-23',65.00,-160.0+0.22,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-25',65.00,-160.0+0.24,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-24',65.03,-160.0+0.23,'H'); -- put a H between two V
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-26',65.02,-160.0+0.25,'H');
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-27',65.00,-160.0+0.26,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-28',65.00,-160.0+0.27,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-29',65.02,-160.0+0.28,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-30',65.00,-160.0+0.29,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-31',65.02,-160.0+0.30,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-02-01',65.00,-160.0+0.31,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-02-02',65.03,-160.0+0.32,'H');
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-02-03',65.01,-160.0+0.33,'H');
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-02-05',65.03,-160.0+0.35,'H');
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-02-04',65.02,-160.0+0.34,'H'); -- put a H between two H
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-02-06',65.01,-160.0+0.36,'H');
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-02-07',65.00,-160.0+0.37,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-02-08',65.01,-160.0+0.38,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-02-09',65.00,-160.0+0.39,'H');
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-07',65.04,-160.0+0.06,'H'); --Add stuff at the beginning
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-08',65.02,-160.0+0.07,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-09',65.04,-160.0+0.08,'H');
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-06',65.04,-160.0+0.05,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-05',65.02,-160.0+0.04,'H');
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-04',65.04,-160.0+0.03,'H');
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-03',65.02,-160.0+0.02,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-02',65.02,-160.0+0.01,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-01',65.04,-160.0+0.00,'H');
END;

-- ===========================
-- ==       INSERTION       ==
-- ===========================

-- Done above, the various statuses, and the ordering of status and dates
-- tests a variety of insert combinations however it isn't completely methodical.
-- The test below does the same inserts as one operation.

IF @Step = 2
BEGIN
	-- Bulk insert from temp table
	DELETE FROM [dbo].[Movement] WHERE [AnimalId] = 'test';
	DELETE FROM [dbo].[Locations] WHERE [AnimalId] = 'test';

	DECLARE @tmp TABLE (
		[AnimalId] CHAR(4),
		[FixDate] DATETIME,
		[Lat] DECIMAL(18,8),
		[Lon] DECIMAL(18,8),
		[Status] CHAR(1)
	)
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-10',65.01,-160.0+0.09,'H');
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-11',65.04,-160.0+0.10,'H');
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-12',65.02,-160.0+0.11,NULL);
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-13',65.02,-160.0+0.12,NULL);
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-14',65.04,-160.0+0.13,'H');
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-16',65.04,-160.0+0.15,'H');
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-15',65.00,-160.0+0.14,NULL); -- put a V between two H
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-17',65.02,-160.0+0.16,NULL);
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-19',65.00,-160.0+0.18,NULL);
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-18',65.02,-160.0+0.17,NULL); -- put a V between two V
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-20',65.04,-160.0+0.19,'H');
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-21',65.00,-160.0+0.20,NULL);
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-22',65.02,-160.0+0.21,NULL);
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-23',65.00,-160.0+0.22,NULL);
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-25',65.00,-160.0+0.24,NULL);
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-24',65.03,-160.0+0.23,'H'); -- put a H between two V
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-26',65.02,-160.0+0.25,'H');
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-27',65.00,-160.0+0.26,NULL);
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-28',65.00,-160.0+0.27,NULL);
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-29',65.02,-160.0+0.28,NULL);
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-30',65.00,-160.0+0.29,NULL);
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-31',65.02,-160.0+0.30,NULL);
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-02-01',65.00,-160.0+0.31,NULL);
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-02-02',65.03,-160.0+0.32,'H');
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-02-03',65.01,-160.0+0.33,'H');
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-02-05',65.03,-160.0+0.35,'H');
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-02-04',65.02,-160.0+0.34,'H'); -- put a H between two H
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-02-06',65.01,-160.0+0.36,'H');
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-02-07',65.00,-160.0+0.37,NULL);
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-02-08',65.01,-160.0+0.38,NULL);
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-02-09',65.00,-160.0+0.39,'H');
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-07',65.04,-160.0+0.06,'H'); --Add stuff at the beginning
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-08',65.02,-160.0+0.07,NULL);
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-09',65.04,-160.0+0.08,'H');
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-06',65.04,-160.0+0.05,NULL);
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-05',65.02,-160.0+0.04,'H');
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-04',65.04,-160.0+0.03,'H');
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-03',65.02,-160.0+0.02,NULL);
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-02',65.02,-160.0+0.01,NULL);
	INSERT INTO @tmp ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-01',65.04,-160.0+0.00,'H');
	
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) SELECT [AnimalId],[FixDate],[Lat],[Lon],[Status] FROM @tmp;
END;



-- ===========================
-- ==       DELETION        ==
-- ===========================

-- SINGLE Locations

IF @Step = 21
BEGIN
	-- Delete single Locations (HIDDEN with VISIBLE adjacent)
	--beginning (HV)
	DELETE FROM Locations where AnimalID='test' and [FixDate] = '1990-01-01';
	--middle (VISIBLE Before, HIDDEN After - VHH)
	DELETE FROM Locations where AnimalID='test' and [FixDate] = '1990-01-04';
	--middle (HIDDEN Before, VISIBLE After - HHV)
	DELETE FROM Locations where AnimalID='test' and [FixDate] = '1990-01-11';
	--middle (Both VISIBLE - VHV)
	DELETE FROM Locations where AnimalID='test' and [FixDate] = '1990-01-20';
	--ending (VH)
	DELETE FROM Locations where AnimalID='test' and [FixDate] = '1990-02-09';	
END;

IF @Step = 22
BEGIN
	-- Delete single Locations (VISIBLE with VISIBLE adjacent)
	--beginning (VV) - Assumes previous test worked correctly, and data was reset
	DELETE FROM Locations where AnimalID='test' and [FixDate] = '1990-01-01';
	DELETE FROM Locations where AnimalID='test' and [FixDate] = '1990-01-02';
	--middle (VISIBLE Before, HIDDEN After - VVH)
	DELETE FROM Locations where AnimalID='test' and [FixDate] = '1990-01-13';
	--middle (HIDDEN Before, VISIBLE After - HVV)
	DELETE FROM Locations where AnimalID='test' and [FixDate] = '1990-01-17';
	--middle (Both VISIBLE - VVV)
	DELETE FROM Locations where AnimalID='test' and [FixDate] = '1990-01-22';
	--ending (VV)
	DELETE FROM Locations where AnimalID='test' and [FixDate] = '1990-02-09';	
	DELETE FROM Locations where AnimalID='test' and [FixDate] = '1990-02-08';	
END;

IF @Step = 23
BEGIN
	-- Delete single Locations (VISIBLE with HIDDEN adjacent)
	--beginning (VH)
	DELETE FROM Locations where AnimalID='test' and [FixDate] = '1990-01-01';
	DELETE FROM Locations where AnimalID='test' and [FixDate] = '1990-01-02';
	DELETE FROM Locations where AnimalID='test' and [FixDate] = '1990-01-03';
	--middle (BOTH HIDDEN - HVH)
	DELETE FROM Locations where AnimalID='test' and [FixDate] = '1990-01-25';
	--ending (HV)
	DELETE FROM Locations where AnimalID='test' and [FixDate] = '1990-02-09';	
	DELETE FROM Locations where AnimalID='test' and [FixDate] = '1990-02-08';	
	DELETE FROM Locations where AnimalID='test' and [FixDate] = '1990-02-07';	
END;

IF @Step = 24
BEGIN
	-- Delete single Locations (HIDDEN with HIDDEN adjacent)
	--beginning (HH) 
	DELETE FROM Locations where AnimalID='test' and [FixDate] = '1990-01-01';
	DELETE FROM Locations where AnimalID='test' and [FixDate] = '1990-01-02';
	DELETE FROM Locations where AnimalID='test' and [FixDate] = '1990-01-03';
	DELETE FROM Locations where AnimalID='test' and [FixDate] = '1990-01-04';
	--middle (BOTH HIDDEN - HHH)
	DELETE FROM Locations where AnimalID='test' and [FixDate] = '1990-02-03';
	--ending (HH)
	DELETE FROM Locations where AnimalID='test' and [FixDate] = '1990-02-09';	
	DELETE FROM Locations where AnimalID='test' and [FixDate] = '1990-02-08';	
	DELETE FROM Locations where AnimalID='test' and [FixDate] = '1990-02-07';	
	DELETE FROM Locations where AnimalID='test' and [FixDate] = '1990-02-06';	
END;


-- MULTIPLE Locations


IF @Step = 31
BEGIN
	-- Delete multiple Locations at once.  Various status with last as HIDDEN, and adjacent as HIDDEN
	--beginning
	DELETE FROM Locations where AnimalID='test' and [FixDate] < '1990-01-05';
	--middle
	DELETE FROM Locations where AnimalID='test' and [FixDate] between '1990-01-11' AND '1990-02-02';
	--ending
	DELETE FROM Locations where AnimalID='test' and [FixDate] > '1990-02-05';	
END;
	
IF @Step = 32
BEGIN
	-- Delete multiple Locations at once.  Various status with last as VISIBLE, and adjacent as VISIBLE
	--beginning
	DELETE FROM Locations where AnimalID='test' and [FixDate] < '1990-01-03';
	--middle
	DELETE FROM Locations where AnimalID='test' and [FixDate] between '1990-01-13' AND '1990-01-17';
	--ending
	DELETE FROM Locations where AnimalID='test' and [FixDate] > '1990-02-07';	
END;
	
IF @Step = 33
BEGIN
	-- Delete multiple Locations at once.  Various status with last as VISIBLE, and adjacent as HIDDEN
	--beginning
	DELETE FROM Locations where AnimalID='test' and [FixDate] < '1990-01-04';
	--middle
	DELETE FROM Locations where AnimalID='test' and [FixDate] between '1990-01-12' AND '1990-01-19';
	--ending
	DELETE FROM Locations where AnimalID='test' and [FixDate] > '1990-02-06';	
END;
	
IF @Step = 34
BEGIN
	-- Delete multiple Locations at once.  Various status with last as HIDDEN, and adjacent as VISIBLE
	--beginning
	DELETE FROM Locations where AnimalID='test' and [FixDate] < '1990-01-06';
	--middle
	DELETE FROM Locations where AnimalID='test' and [FixDate] between '1990-01-14' AND '1990-01-20';
	--ending
	DELETE FROM Locations where AnimalID='test' and [FixDate] > '1990-02-01';	
END;
	


-- ===========================
-- ==        UPDATES        ==
-- ===========================


IF @Step = 41
BEGIN
	-- Change a single feature's status
	-- Non-null to different non-null (no-op)
	Update Locations SET [Status] = 'V' where AnimalID='test' AND [Status] = 'H' and [FixDate] = '1990-01-20';
	-- VVV -> VHV
	Update Locations SET [Status] = 'H' where AnimalID='test' AND [Status] IS NULL and [FixDate] = '1990-01-29';
	-- VVH -> VHH
	Update Locations SET [Status] = 'H' where AnimalID='test' AND [Status] IS NULL and [FixDate] = '1990-01-03';
	-- HVV -> HHV
	Update Locations SET [Status] = 'H' where AnimalID='test' AND [Status] IS NULL and [FixDate] = '1990-01-12';
	-- HVH -> HHH
	Update Locations SET [Status] = 'H' where AnimalID='test' AND [Status] IS NULL and [FixDate] = '1990-01-15';
	-- VHV -> VVV
	Update Locations SET [Status] = NULL where AnimalID='test' AND [Status] = 'H' and [FixDate] = '1990-01-26';
	-- VHH -> VVH
	Update Locations SET [Status] = NULL where AnimalID='test' AND [Status] = 'H' and [FixDate] = '1990-02-02';
	-- HHV -> HVV
	Update Locations SET [Status] = NULL where AnimalID='test' AND [Status] = 'H' and [FixDate] = '1990-02-06';
	-- HHH -> HVH
	Update Locations SET [Status] = NULL where AnimalID='test' AND [Status] = 'H' and [FixDate] = '1990-02-04';
END;

IF @Step = 42
BEGIN
	-- Swap all in two steps
	Update Locations SET [Status] = 'V' where AnimalID = 'test' AND [Status] IS NULL;
	Update Locations SET [Status] = NULL where AnimalID = 'test' AND [Status] = 'H';
END;

IF @Step = 43
BEGIN
	-- Swap all in one step
	Update Locations SET [Status] = CASE WHEN [Status] IS NOT NULL THEN NULL ELSE 'H' END
	 where AnimalID = 'test';
END;
	
	
IF @Step = 51
BEGIN
	-- VV..VV -> VH..HV
	--beginning
	Update Locations SET [Status] = 'H' where AnimalID='test' and [FixDate] < '1990-01-03';
	--middle
	Update Locations SET [Status] = 'H' where AnimalID='test' and [FixDate] between '1990-01-29' AND '1990-01-31';
	--ending
	Update Locations SET [Status] = 'H' where AnimalID='test' and [FixDate] > '1990-02-07';	
END;
	
IF @Step = 52
BEGIN
	-- VV..HV -> VH..HV
	--beginning
	Update Locations SET [Status] = 'H' where AnimalID='test' and [FixDate] < '1990-01-06';
	--middle
	Update Locations SET [Status] = 'H' where AnimalID='test' and [FixDate] between '1990-01-22' AND '1990-01-24';
	--ending
	Update Locations SET [Status] = 'H' where AnimalID='test' and [FixDate] > '1990-01-31';	
END;
	
IF @Step = 53
BEGIN
	-- VH..VV -> VH..HV
	--beginning
	Update Locations SET [Status] = 'H' where AnimalID='test' and [FixDate] < '1990-01-13';
	--middle
	Update Locations SET [Status] = 'H' where AnimalID='test' and [FixDate] between '1990-01-20' AND '1990-01-27';
	--ending
	Update Locations SET [Status] = 'H' where AnimalID='test' and [FixDate] > '1990-02-01';	
END;
	
IF @Step = 54
BEGIN
	-- VH..VV -> VV..VV
	--beginning
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] < '1990-01-13';
	--middle
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] between '1990-01-20' AND '1990-01-27';
	--ending
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] > '1990-02-01';	
END;
	
IF @Step = 55
BEGIN
	-- VV..HV -> VV..VV
	--beginning
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] < '1990-01-06';
	--middle
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] between '1990-01-22' AND '1990-01-24';
	--ending
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] > '1990-01-31';	
END;
	
IF @Step = 56
BEGIN
	-- VH..HV -> VV..VV
	--beginning
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] < '1990-01-06';
	--middle
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] between '1990-01-24' AND '1990-01-26';
	--ending
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] > '1990-02-01';	
END;


	
IF @Step = 61
BEGIN
	-- VV..VH -> VH..HH
	--beginning
	Update Locations SET [Status] = 'H' where AnimalID='test' and [FixDate] < '1990-01-07';
	--middle
	Update Locations SET [Status] = 'H' where AnimalID='test' and [FixDate] between '1990-01-19' AND '1990-01-23';
	--ending
	Update Locations SET [Status] = 'H' where AnimalID='test' and [FixDate] > '1990-01-31';	
END;
	
IF @Step = 62
BEGIN
	-- VV..HH -> VH..HH
	--beginning
	Update Locations SET [Status] = 'H' where AnimalID='test' and [FixDate] < '1990-01-05';
	--middle
	Update Locations SET [Status] = 'H' where AnimalID='test' and [FixDate] between '1990-01-29' AND '1990-02-02';
	--ending
	Update Locations SET [Status] = 'H' where AnimalID='test' and [FixDate] > '1990-02-07';	
END;
	
IF @Step = 63
BEGIN
	-- VH..VH -> VH..HH
	--beginning
	Update Locations SET [Status] = 'H' where AnimalID='test' and [FixDate] < '1990-01-04';
	--middle
	Update Locations SET [Status] = 'H' where AnimalID='test' and [FixDate] between '1990-01-20' AND '1990-01-25';
	--ending
	Update Locations SET [Status] = 'H' where AnimalID='test' and [FixDate] > '1990-02-01';	
END;
	
IF @Step = 64
BEGIN
	-- VH..VH -> VV..VH
	--beginning
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] < '1990-01-04';
	--middle
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] between '1990-01-20' AND '1990-01-25';
	--ending
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] > '1990-02-01';	
END;
	
IF @Step = 65
BEGIN
	-- VV..HH -> VV..VH
	--beginning
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] < '1990-01-05';
	--middle
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] between '1990-01-29' AND '1990-02-02';
	--ending
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] > '1990-02-07';	
END;
	
IF @Step = 66
BEGIN
	-- VH..HH -> VV..VH
	--beginning
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] < '1990-01-10';
	--middle
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] between '1990-01-26' AND '1990-02-02';
	--ending
	--Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] > '1990-02-01';	
END;


	
IF @Step = 71
BEGIN
	-- HV..VV -> HH..HV
	--beginning
	Update Locations SET [Status] = 'H' where AnimalID='test' and [FixDate] < '1990-01-03';
	--middle
	Update Locations SET [Status] = 'H' where AnimalID='test' and [FixDate] between '1990-01-17' AND '1990-01-22';
	--ending
	Update Locations SET [Status] = 'H' where AnimalID='test' and [FixDate] > '1990-01-26';	
END;
	
IF @Step = 72
BEGIN
	-- HV..HV -> HH..HV
	--beginning
	Update Locations SET [Status] = 'H' where AnimalID='test' and [FixDate] < '1990-01-06';
	--middle
	Update Locations SET [Status] = 'H' where AnimalID='test' and [FixDate] between '1990-01-17' AND '1990-01-26';
	--ending
	Update Locations SET [Status] = 'H' where AnimalID='test' and [FixDate] > '1990-02-06';	
END;
	
IF @Step = 73
BEGIN
	-- HH..VV -> HH..HV
	--beginning
	--Update Locations SET [Status] = 'H' where AnimalID='test' and [FixDate] < '1990-01-13';
	--middle
	Update Locations SET [Status] = 'H' where AnimalID='test' and [FixDate] between '1990-01-05' AND '1990-01-18';
	--ending
	Update Locations SET [Status] = 'H' where AnimalID='test' and [FixDate] > '1990-02-04';	
END;
	
IF @Step = 74
BEGIN
	-- HH..VV -> HV..VV
	--beginning
	--Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] < '1990-01-13';
	--middle
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] between '1990-01-05' AND '1990-01-18';
	--ending
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] > '1990-02-04';	
END;
	
IF @Step = 75
BEGIN
	-- HV..HV -> HV..VV
	--beginning
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] < '1990-01-06';
	--middle
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] between '1990-01-15' AND '1990-01-26';
	--ending
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] > '1990-02-06';	
END;
	
IF @Step = 76
BEGIN
	-- HH..HV -> HV..VV
	--beginning
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] < '1990-01-08';
	--middle
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] between '1990-01-11' AND '1990-01-20';
	--ending
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] > '1990-02-05';
END;


	
IF @Step = 81
BEGIN
	-- HV..VH -> HH..HH
	--beginning
	Update Locations SET [Status] = 'H' where AnimalID='test' and [FixDate] < '1990-01-04';
	--middle
	Update Locations SET [Status] = 'H' where AnimalID='test' and [FixDate] between '1990-01-17' AND '1990-01-19';
	--ending
	Update Locations SET [Status] = 'H' where AnimalID='test' and [FixDate] > '1990-02-06';	
END;
	
IF @Step = 82
BEGIN
	-- HV..HH -> HH..HH
	--beginning
	Update Locations SET [Status] = 'H' where AnimalID='test' and [FixDate] < '1990-01-05';
	--middle
	Update Locations SET [Status] = 'H' where AnimalID='test' and [FixDate] between '1990-01-21' AND '1990-02-02';
	--ending
	Update Locations SET [Status] = 'H' where AnimalID='test' and [FixDate] > '1990-02-06';	
END;
	
IF @Step = 83
BEGIN
	-- HH..VH -> HH..HH
	--beginning
	Update Locations SET [Status] = 'H' where AnimalID='test' and [FixDate] < '1990-01-06';
	--middle
	Update Locations SET [Status] = 'H' where AnimalID='test' and [FixDate] between '1990-01-11' AND '1990-01-25';
	--ending
	Update Locations SET [Status] = 'H' where AnimalID='test' and [FixDate] > '1990-02-04';	
END;
	
IF @Step = 84
BEGIN
	-- HH..VH -> HV..VH
	--beginning
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] < '1990-01-06';
	--middle
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] between '1990-01-11' AND '1990-01-25';
	--ending
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] > '1990-02-04';	
END;
	
IF @Step = 85
BEGIN
	-- HV..HH -> HV..VH
	--beginning
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] < '1990-01-05';
	--middle
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] between '1990-01-21' AND '1990-02-02';
	--ending
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] > '1990-02-06';	
END;
	
IF @Step = 86
BEGIN
	-- HH..HH -> HV..VH
	--beginning
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] < '1990-01-05';
	--middle
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] between '1990-01-11' AND '1990-02-02';
	--ending
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] > '1990-02-05';	
END;



	
-- ===========================
-- ==        CLEANUP        ==
-- ===========================


IF @Step = 99
-- Return everything to a production state
BEGIN
	DELETE FROM [dbo].[Movement] WHERE [AnimalID] = 'test';
	DELETE FROM [dbo].[Locations] WHERE [AnimalID] = 'test';
END;
