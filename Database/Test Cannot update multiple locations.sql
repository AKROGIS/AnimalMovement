-- ========================================
-- Test Module for Animal Movement Triggers
--
-- To test:
--   1) Set the step to 1 then run.
--   2) verify the results in ArcMap
--   3) Increment the step and repeat
-- ========================================

--Test 1  Inserts
-- The inserts are typically done as singular statements (exception is select into)
-- so we will test various combinations of status to ensure it is cr

DECLARE @Step INT = 3;

IF @Step = 1
-- Clear the slate, and insert some test data;
-- the variety of status, and the ordering of status and dates, tests a variety of combinations
-- however it isn't bvery methodical.
BEGIN
	DELETE FROM [dbo].[Movement] WHERE [AnimalId] = 'test';
	DELETE FROM [dbo].[Locations] WHERE [AnimalId] = 'test';
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-10',65.01,-160.0+0.09,'H');
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-11',65.04,-160.0+0.10,'H');
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-12',65.02,-160.0+0.11,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-13',65.02,-160.0+0.12,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-14',65.04,-160.0+0.13,'H');
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-15',65.00,-160.0+0.14,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-16',65.04,-160.0+0.15,'H');
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-17',65.02,-160.0+0.16,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-18',65.02,-160.0+0.17,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-19',65.00,-160.0+0.18,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-20',65.04,-160.0+0.19,'H');
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-21',65.00,-160.0+0.20,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-22',65.02,-160.0+0.21,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-23',65.00,-160.0+0.22,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-24',65.03,-160.0+0.23,'H');
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-25',65.00,-160.0+0.24,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-26',65.02,-160.0+0.25,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-27',65.00,-160.0+0.26,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-28',65.00,-160.0+0.27,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-29',65.02,-160.0+0.28,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-30',65.00,-160.0+0.29,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-31',65.02,-160.0+0.30,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-02-01',65.00,-160.0+0.31,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-02-02',65.03,-160.0+0.32,'H');
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-02-03',65.01,-160.0+0.33,'H');
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-02-04',65.00,-160.0+0.34,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-07',65.04,-160.0+0.06,'H');
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-08',65.02,-160.0+0.07,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-09',65.04,-160.0+0.08,'H');
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-06',65.04,-160.0+0.05,'H');
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-05',65.02,-160.0+0.04,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-04',65.04,-160.0+0.03,'H');
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-03',65.02,-160.0+0.02,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-02',65.02,-160.0+0.01,NULL);
	INSERT INTO [dbo].[Locations] ([AnimalId],[FixDate],[Lat],[Lon],[Status]) VALUES('test','1990-01-01',65.04,-160.0+0.00,'H');
END;


IF @Step = 2
-- Change the test data
BEGIN
	-- Change a single feature's status
	-- non-null to non-null
	Update Locations SET [Status] = 'V' where AnimalID='test' and [FixDate] = '1990-01-04';
	-- null to null
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] = '1990-01-03';
	-- null to non-null
	Update Locations SET [Status] = 'V' where AnimalID='test' and [FixDate] = '1990-01-08';
	-- non-null to null
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] = '1990-01-06';
END;
	
IF @Step = 3
-- Change the test data
BEGIN
	-- Change multiple Status at once  Various status to null, with adjacent unchanged as non-null
	--beginning
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] < '1990-01-06';
	--middle
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] between '1990-01-14' AND '1990-01-20';
	--ending
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] > '1990-02-02';	
END;
	
IF @Step = 4
BEGIN
	-- Change multiple Status at once  Various status to null, with adjacent unchanged as null
	--beginning
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] < '1990-01-05';
	--middle
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] between '1990-01-15' AND '1990-01-22';
	--ending
	Update Locations SET [Status] = NULL where AnimalID='test' and [FixDate] > '1990-02-01';	
END;
	
IF @Step = 5
BEGIN
	-- Change multiple Status at once  Various status to non-null, with adjacent unchanged as null
	--beginning
	Update Locations SET [Status] = 'V' where AnimalID='test' and [FixDate] < '1990-01-05';
	--middle
	Update Locations SET [Status] = 'V' where AnimalID='test' and [FixDate] between '1990-01-15' AND '1990-01-22';
	--ending
	Update Locations SET [Status] = 'V' where AnimalID='test' and [FixDate] > '1990-02-01';	
END;
	
IF @Step = 6
BEGIN
	-- Change multiple Status at once  Various status to non-null, with adjacent unchanged as non-null
	--beginning
	Update Locations SET [Status] = 'V' where AnimalID='test' and [FixDate] < '1990-01-06';
	--middle
	Update Locations SET [Status] = 'V' where AnimalID='test' and [FixDate] between '1990-01-14' AND '1990-01-20';
	--ending
	Update Locations SET [Status] = 'V' where AnimalID='test' and [FixDate] > '1990-02-02';	
END;
	
IF @Step = 7
BEGIN

	select * from Movements;
	
	--Single insert at begining
	--Single insert at end
	--Single insert at begining
	--multiple insert at begining
	--multiple insert at end
	--multiple insert in middle
	--multiple insert with intermediate skip
	--multiple insert with intermediate delete;
	
	--Single delete at begining
	--Single delete at end
	--Single delete at begining
	--multiple delete at begining
	--multiple delete at end
	--multiple delete in middle
	--multiple delete with intermediate skip
	--multiple delete with intermediate delete;
END;


IF @Step = 13
-- Return everything to a production state
BEGIN
	DELETE FROM [dbo].[Movement] WHERE [AnimalID] = 'test';
	DELETE FROM [dbo].[Locations] WHERE [AnimalID] = 'test';
END;
