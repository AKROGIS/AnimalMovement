
--  ************************************************************
    PRINT 'Test_Function_DoDateRangesOverlap'
--  ************************************************************
--  Author:	     Regan Sarwas
--  Create date: June 11, 2012
--  Description: Tests the code for determining if a two date
--               ranges overlap
--  ============================================================
        
    DECLARE @F1_StartDate1 DATETIME2
    DECLARE @F1_StartDate2 DATETIME2
    DECLARE @F1_EndDate1 DATETIME2
    DECLARE @F1_EndDate2 DATETIME2
    DECLARE @F1_OVERLAP NVARCHAR(32)
    DECLARE @F1_RESULT NVARCHAR(32)
    
    -- NULL to DATE
    
    SET @F1_StartDate1 = NULL
    SET @F1_EndDate1 = '2000-01-01'
    SET @F1_StartDate2 = NULL
    SET @F1_EndDate2 = '2001-01-01'
    IF dbo.DoDateRangesOverlap(@F1_StartDate1,@F1_EndDate1,@F1_StartDate2,@F1_EndDate2) = 1
        SET @F1_OVERLAP = 'Overlaps' ELSE SET @F1_OVERLAP = 'Is disjointed'
    IF @F1_OVERLAP = 'Overlaps'
        SET @F1_RESULT = 'Pass' ELSE SET @F1_RESULT = 'Fail'
    print '  1  (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate2)),'NULL') + ') '+ @F1_OVERLAP + '.  ' + @F1_RESULT
    
    
    
    SET @F1_StartDate1 = NULL
    SET @F1_EndDate1 = '2000-01-01'
    SET @F1_StartDate2 = '2001-01-01'
    SET @F1_EndDate2 = '2002-01-01'
    IF dbo.DoDateRangesOverlap(@F1_StartDate1,@F1_EndDate1,@F1_StartDate2,@F1_EndDate2) = 1
        SET @F1_OVERLAP = 'Overlaps' ELSE SET @F1_OVERLAP = 'Is disjointed'
    IF @F1_OVERLAP <> 'Overlaps'
        SET @F1_RESULT = 'Pass' ELSE SET @F1_RESULT = 'Fail'
    print '  2a (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate2)),'NULL') + ') '+ @F1_OVERLAP + '.  ' + @F1_RESULT
    
    SET @F1_StartDate1 = NULL
    SET @F1_EndDate1 = '2001-01-01'
    SET @F1_StartDate2 = '2000-01-01'
    SET @F1_EndDate2 = '2002-01-01'
    IF dbo.DoDateRangesOverlap(@F1_StartDate1,@F1_EndDate1,@F1_StartDate2,@F1_EndDate2) = 1
        SET @F1_OVERLAP = 'Overlaps' ELSE SET @F1_OVERLAP = 'Is disjointed'
    IF @F1_OVERLAP = 'Overlaps'
        SET @F1_RESULT = 'Pass' ELSE SET @F1_RESULT = 'Fail'
    print '  2b (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate2)),'NULL') + ') '+ @F1_OVERLAP + '.  ' + @F1_RESULT
    
    
    
    SET @F1_StartDate1 = NULL
    SET @F1_EndDate1 = '2000-01-01'
    SET @F1_StartDate2 = '2001-01-01'
    SET @F1_EndDate2 = NULL
    IF dbo.DoDateRangesOverlap(@F1_StartDate1,@F1_EndDate1,@F1_StartDate2,@F1_EndDate2) = 1
        SET @F1_OVERLAP = 'Overlaps' ELSE SET @F1_OVERLAP = 'Is disjointed'
    IF @F1_OVERLAP <> 'Overlaps'
        SET @F1_RESULT = 'Pass' ELSE SET @F1_RESULT = 'Fail'
    print '  3a (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate2)),'NULL') + ') '+ @F1_OVERLAP + '.  ' + @F1_RESULT
    
    SET @F1_StartDate1 = NULL
    SET @F1_EndDate1 = '2001-01-01'
    SET @F1_StartDate2 = '2000-01-01'
    SET @F1_EndDate2 = NULL
    IF dbo.DoDateRangesOverlap(@F1_StartDate1,@F1_EndDate1,@F1_StartDate2,@F1_EndDate2) = 1
        SET @F1_OVERLAP = 'Overlaps' ELSE SET @F1_OVERLAP = 'Is disjointed'
    IF @F1_OVERLAP = 'Overlaps'
        SET @F1_RESULT = 'Pass' ELSE SET @F1_RESULT = 'Fail'
    print '  3b (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate2)),'NULL') + ') '+ @F1_OVERLAP + '.  ' + @F1_RESULT
    
    
    -- DATE to DATE
    
    SET @F1_StartDate1 = '2000-01-01'
    SET @F1_EndDate1 =  '2002-01-01'
    SET @F1_StartDate2 = NULL
    SET @F1_EndDate2 = '2001-01-01'
    IF dbo.DoDateRangesOverlap(@F1_StartDate1,@F1_EndDate1,@F1_StartDate2,@F1_EndDate2) = 1
        SET @F1_OVERLAP = 'Overlaps' ELSE SET @F1_OVERLAP = 'Is disjointed'
    IF @F1_OVERLAP = 'Overlaps'
        SET @F1_RESULT = 'Pass' ELSE SET @F1_RESULT = 'Fail'
    print '  4a (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate2)),'NULL') + ') '+ @F1_OVERLAP + '.  ' + @F1_RESULT
    
    SET @F1_StartDate1 = '2001-01-01'
    SET @F1_EndDate1 =  '2002-01-01'
    SET @F1_StartDate2 = NULL
    SET @F1_EndDate2 = '2000-01-01'
    IF dbo.DoDateRangesOverlap(@F1_StartDate1,@F1_EndDate1,@F1_StartDate2,@F1_EndDate2) = 1
        SET @F1_OVERLAP = 'Overlaps' ELSE SET @F1_OVERLAP = 'Is disjointed'
    IF @F1_OVERLAP <> 'Overlaps'
        SET @F1_RESULT = 'Pass' ELSE SET @F1_RESULT = 'Fail'
    print '  4b (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate2)),'NULL') + ') '+ @F1_OVERLAP + '.  ' + @F1_RESULT
    
    
    
    SET @F1_StartDate1 = '2001-01-01'
    SET @F1_EndDate1 =  '2003-01-01'
    SET @F1_StartDate2 = '2000-01-01'
    SET @F1_EndDate2 = '2002-01-01'
    IF dbo.DoDateRangesOverlap(@F1_StartDate1,@F1_EndDate1,@F1_StartDate2,@F1_EndDate2) = 1
        SET @F1_OVERLAP = 'Overlaps' ELSE SET @F1_OVERLAP = 'Is disjointed'
    IF @F1_OVERLAP = 'Overlaps'
        SET @F1_RESULT = 'Pass' ELSE SET @F1_RESULT = 'Fail'
    print '  5a (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate2)),'NULL') + ') '+ @F1_OVERLAP + '.  ' + @F1_RESULT
    
    SET @F1_StartDate1 = '2002-01-01'
    SET @F1_EndDate1 =  '2003-01-01'
    SET @F1_StartDate2 = '2000-01-01'
    SET @F1_EndDate2 = '2001-01-01'
    IF dbo.DoDateRangesOverlap(@F1_StartDate1,@F1_EndDate1,@F1_StartDate2,@F1_EndDate2) = 1
        SET @F1_OVERLAP = 'Overlaps' ELSE SET @F1_OVERLAP = 'Is disjointed'
    IF @F1_OVERLAP <> 'Overlaps'
        SET @F1_RESULT = 'Pass' ELSE SET @F1_RESULT = 'Fail'
    print '  5b (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate2)),'NULL') + ') '+ @F1_OVERLAP + '.  ' + @F1_RESULT
    
    SET @F1_StartDate1 = '2000-01-01'
    SET @F1_EndDate1 =  '2004-01-01'
    SET @F1_StartDate2 = '2001-01-01'
    SET @F1_EndDate2 = '2003-01-01'
    IF dbo.DoDateRangesOverlap(@F1_StartDate1,@F1_EndDate1,@F1_StartDate2,@F1_EndDate2) = 1
        SET @F1_OVERLAP = 'Overlaps' ELSE SET @F1_OVERLAP = 'Is disjointed'
    IF @F1_OVERLAP = 'Overlaps'
        SET @F1_RESULT = 'Pass' ELSE SET @F1_RESULT = 'Fail'
    print '  5c (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate2)),'NULL') + ') '+ @F1_OVERLAP + '.  ' + @F1_RESULT
    
    SET @F1_StartDate1 = '2000-01-01'
    SET @F1_EndDate1 =  '2002-01-01'
    SET @F1_StartDate2 = '2001-01-01'
    SET @F1_EndDate2 = '2003-01-01'
    IF dbo.DoDateRangesOverlap(@F1_StartDate1,@F1_EndDate1,@F1_StartDate2,@F1_EndDate2) = 1
        SET @F1_OVERLAP = 'Overlaps' ELSE SET @F1_OVERLAP = 'Is disjointed'
    IF @F1_OVERLAP = 'Overlaps'
        SET @F1_RESULT = 'Pass' ELSE SET @F1_RESULT = 'Fail'
    print '  5d (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate2)),'NULL') + ') '+ @F1_OVERLAP + '.  ' + @F1_RESULT
    
    SET @F1_StartDate1 = '2000-01-01'
    SET @F1_EndDate1 =  '2001-01-01'
    SET @F1_StartDate2 = '2002-01-01'
    SET @F1_EndDate2 = '2003-01-01'
    IF dbo.DoDateRangesOverlap(@F1_StartDate1,@F1_EndDate1,@F1_StartDate2,@F1_EndDate2) = 1
        SET @F1_OVERLAP = 'Overlaps' ELSE SET @F1_OVERLAP = 'Is disjointed'
    IF @F1_OVERLAP <> 'Overlaps'
        SET @F1_RESULT = 'Pass' ELSE SET @F1_RESULT = 'Fail'
    print '  5e (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate2)),'NULL') + ') '+ @F1_OVERLAP + '.  ' + @F1_RESULT
    
    
    
    SET @F1_StartDate1 = '2000-01-01'
    SET @F1_EndDate1 =  '2001-01-01'
    SET @F1_StartDate2 = '2002-01-01'
    SET @F1_EndDate2 = NULL
    IF dbo.DoDateRangesOverlap(@F1_StartDate1,@F1_EndDate1,@F1_StartDate2,@F1_EndDate2) = 1
        SET @F1_OVERLAP = 'Overlaps' ELSE SET @F1_OVERLAP = 'Is disjointed'
    IF @F1_OVERLAP <> 'Overlaps'
        SET @F1_RESULT = 'Pass' ELSE SET @F1_RESULT = 'Fail'
    print '  6a (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate2)),'NULL') + ') '+ @F1_OVERLAP + '.  ' + @F1_RESULT

    SET @F1_StartDate1 = '2000-01-01'
    SET @F1_EndDate1 =  '2002-01-01'
    SET @F1_StartDate2 = '2001-01-01'
    SET @F1_EndDate2 = NULL
    IF dbo.DoDateRangesOverlap(@F1_StartDate1,@F1_EndDate1,@F1_StartDate2,@F1_EndDate2) = 1
        SET @F1_OVERLAP = 'Overlaps' ELSE SET @F1_OVERLAP = 'Is disjointed'
    IF @F1_OVERLAP = 'Overlaps'
        SET @F1_RESULT = 'Pass' ELSE SET @F1_RESULT = 'Fail'
    print '  6b (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate2)),'NULL') + ') '+ @F1_OVERLAP + '.  ' + @F1_RESULT


    -- NULL to DATE

    SET @F1_StartDate1 = '2002-01-01'
    SET @F1_EndDate1 =  NULL
    SET @F1_StartDate2 = NULL
    SET @F1_EndDate2 = '2001-01-01'
    IF dbo.DoDateRangesOverlap(@F1_StartDate1,@F1_EndDate1,@F1_StartDate2,@F1_EndDate2) = 1
        SET @F1_OVERLAP = 'Overlaps' ELSE SET @F1_OVERLAP = 'Is disjointed'
    IF @F1_OVERLAP <> 'Overlaps'
        SET @F1_RESULT = 'Pass' ELSE SET @F1_RESULT = 'Fail'
    print '  7a (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate2)),'NULL') + ') '+ @F1_OVERLAP + '.  ' + @F1_RESULT
    
    SET @F1_StartDate1 = '2000-01-01'
    SET @F1_EndDate1 =  NULL
    SET @F1_StartDate2 = NULL
    SET @F1_EndDate2 = '2002-01-01'
    IF dbo.DoDateRangesOverlap(@F1_StartDate1,@F1_EndDate1,@F1_StartDate2,@F1_EndDate2) = 1
        SET @F1_OVERLAP = 'Overlaps' ELSE SET @F1_OVERLAP = 'Is disjointed'
    IF @F1_OVERLAP = 'Overlaps'
        SET @F1_RESULT = 'Pass' ELSE SET @F1_RESULT = 'Fail'
    print '  7b (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate2)),'NULL') + ') '+ @F1_OVERLAP + '.  ' + @F1_RESULT
    
    
    
    SET @F1_StartDate1 = '2001-01-01'
    SET @F1_EndDate1 =  NULL
    SET @F1_StartDate2 = '2000-01-01'
    SET @F1_EndDate2 = '2002-01-01'
    IF dbo.DoDateRangesOverlap(@F1_StartDate1,@F1_EndDate1,@F1_StartDate2,@F1_EndDate2) = 1
        SET @F1_OVERLAP = 'Overlaps' ELSE SET @F1_OVERLAP = 'Is disjointed'
    IF @F1_OVERLAP = 'Overlaps'
        SET @F1_RESULT = 'Pass' ELSE SET @F1_RESULT = 'Fail'
    print '  8a (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate2)),'NULL') + ') '+ @F1_OVERLAP + '.  ' + @F1_RESULT
    
    SET @F1_StartDate1 = '2002-01-01'
    SET @F1_EndDate1 =  NULL
    SET @F1_StartDate2 = '2000-01-01'
    SET @F1_EndDate2 = '2001-01-01'
    IF dbo.DoDateRangesOverlap(@F1_StartDate1,@F1_EndDate1,@F1_StartDate2,@F1_EndDate2) = 1
        SET @F1_OVERLAP = 'Overlaps' ELSE SET @F1_OVERLAP = 'Is disjointed'
    IF @F1_OVERLAP <> 'Overlaps'
        SET @F1_RESULT = 'Pass' ELSE SET @F1_RESULT = 'Fail'
    print '  8b (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate2)),'NULL') + ') '+ @F1_OVERLAP + '.  ' + @F1_RESULT
    
    
    
    SET @F1_StartDate1 = '2001-01-01'
    SET @F1_EndDate1 =  NULL
    SET @F1_StartDate2 = '2000-01-01'
    SET @F1_EndDate2 = NULL
    IF dbo.DoDateRangesOverlap(@F1_StartDate1,@F1_EndDate1,@F1_StartDate2,@F1_EndDate2) = 1
        SET @F1_OVERLAP = 'Overlaps' ELSE SET @F1_OVERLAP = 'Is disjointed'
    IF @F1_OVERLAP = 'Overlaps'
        SET @F1_RESULT = 'Pass' ELSE SET @F1_RESULT = 'Fail'
    print '  9  (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate1)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate1)),'NULL') + ') TO (' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_StartDate2)),'NULL') +',' + ISNULL(CONVERT(NVARCHAR(32),Year(@F1_EndDate2)),'NULL') + ') '+ @F1_OVERLAP + '.  ' + @F1_RESULT








--  ************************************************************
    PRINT 'Test_Trigger_Animals_Update'
--  ************************************************************
--  Author:	     Regan Sarwas
--  Create date: June 11, 2012
--  Description: Test the Animals UpdateTrigger
--  ============================================================
    
    PRINT '  Test not written yet'








--  ************************************************************
    PRINT 'Test_Trigger_CollarDeployments_Delete'
--  ************************************************************
--  Author:	     Regan Sarwas
--  Create date: June 15, 2012
--  Description: Test the CollarDeployments DeleteTrigger
--  ============================================================

    PRINT '  See Test_Trigger_CollarDeployments_Insert'










--  ************************************************************
    PRINT 'Test_Trigger_CollarDeployments_Insert'
--  ************************************************************
--  Author:	     Regan Sarwas
--  Create date: June 13, 2012
--  Description: Test the CollarDeployments InsertTrigger
--  ============================================================
    
    DECLARE
            @T1_sa nvarchar(16) = 'sa',
            @T1_project nvarchar(16) = 'p1__',
            @T1_animal1 nvarchar(16) = 'a1__',
            @T1_animal2 nvarchar(16) = 'a2__',
            @T1_mfgr nvarchar(16) = 'Telonics',
            @T1_model nvarchar(16) = 'TelonicsGen4',
            @T1_collar1 nvarchar(16) = 'c1__',
            @T1_collar2 nvarchar(16) = 'c2__',
            @T1_file1 nvarchar(16) = 'f1__',
            @T1_file1id int,
            @T1_file2 nvarchar(16) = 'f2__',
            @T1_file2id int,
            @T1_format nvarchar(16) = 'A', -- Store on board format
            @T1_test nvarchar(255) = null,
            @T1_msg nvarchar(255) = null,
            @T1_fix11 bigint,
            @T1_fix12 bigint,
            @T1_fix13 bigint,
            @T1_fix21 bigint,
            @T1_fix22 bigint,
            @T1_fix23 bigint,
            @T1_fix_l bigint,
            @T1_deploy1 int,
            @T1_deploy2 int,
            @T1_deploy3 int,
            @T1_deploy4 int
            
    -- This test must be run as SA, since others cannot operate on table directly
    -- This test the integrity of the tables underlying the Store Procedures available to users.
    -- Also SA can check user tables

    IF ORIGINAL_LOGIN() <> @T1_sa
    BEGIN
        PRINT '  You must be the sa to run this test'
        RETURN
    END

/*
    -- Clean up mess if previous test failed; need to find and set the file id first
    DECLARE @T1_project nvarchar(16) = 'p1__',
            @T1_mfgr nvarchar(16) = 'Telonics',
            @T1_collar1 nvarchar(16) = 'c1__',
            @T1_collar2 nvarchar(16) = 'c2__',
            @T1_file1 nvarchar(16) = 'f1__',
            @T1_file2 nvarchar(16) = 'f2__'
    select fileid from collarfiles where filename IN (@T1_file1, @T1_file2)
    EXEC [dbo].[CollarFile_Delete] 879
    EXEC [dbo].[CollarFile_Delete] 880
    DELETE CollarDeployments Where [ProjectId] = @T1_project 
    DELETE Collars where CollarManufacturer = @T1_mfgr and CollarId IN (@T1_collar1,@T1_collar2)
    Delete Animals where ProjectId = @T1_project -- SP checks if user is in role, which sa is not
    EXEC [dbo].[Project_Delete] @T1_project
*/

    --Check to make sure we are not going to overwrite any data
    IF EXISTS (SELECT 1 from Projects Where [ProjectId] = @T1_project)
    OR EXISTS (SELECT 1 from Collars Where [CollarManufacturer] = @T1_mfgr AND [CollarId] IN (@T1_collar1,@T1_collar2))
    BEGIN
        PRINT '  Aborting tests.  Existing data conflicts with test data.'
        RETURN
    END
    
    -- Add the sa is a project investigator
    IF NOT EXISTS(SELECT 1 FROM [dbo].[ProjectInvestigators] WHERE [Login] = @T1_sa)
    BEGIN
        INSERT [dbo].[ProjectInvestigators] ([Login],[Name],[Email],[Phone]) VALUES (@T1_sa,@T1_sa,@T1_sa,@T1_sa)
    END


    -- Setup for test
    EXEC [dbo].[Project_Insert] @T1_project, @T1_project, @T1_sa
    EXEC [dbo].[Animal_Insert] @T1_project, @T1_animal1
    EXEC [dbo].[Animal_Insert] @T1_project, @T1_animal2, null, null, '2012-05-12 12:00'
    EXEC [dbo].[Collar_Insert] @T1_mfgr, @T1_collar1, @T1_model ,@T1_sa
    EXEC [dbo].[Collar_Insert] @T1_mfgr, @T1_collar2, @T1_model ,@T1_sa

    INSERT INTO dbo.CollarFiles ([FileName], [Project], [CollarManufacturer], [CollarId], [Format], [Status], [Contents])
         VALUES (@T1_file1, @T1_project, @T1_mfgr, @T1_collar1, @T1_format, 'A', convert(varbinary,'data'))  -- 'A' = Store on board
    SET @T1_file1id = SCOPE_IDENTITY();
    INSERT INTO dbo.CollarDataTelonicsStoreOnBoard ([FileId], [LineNumber], [Date], [Latitude], [Longitude], [Fix Status])
         VALUES (@T1_file1id, 1, '2012-05-11', '60.11', '-154.11', 'Fix Available'),
                (@T1_file1id, 2, '2012-05-12', '60.12', '-154.12', 'Fix Available'),
                (@T1_file1id, 3, '2012-05-13', '60.13', '-154.13', 'Fix Available')
    EXEC [dbo].[CollarFixes_Insert] @T1_file1id,@T1_format

    INSERT INTO dbo.CollarFiles ([FileName], [Project], [CollarManufacturer], [CollarId], [Format], [Status], [Contents])
         VALUES (@T1_file2, @T1_project, @T1_mfgr, @T1_collar2, @T1_format, 'A', convert(varbinary,'data'))  -- 'A' = Store on board
    SET @T1_file2id = SCOPE_IDENTITY();
    INSERT INTO dbo.CollarDataTelonicsStoreOnBoard ([FileId], [LineNumber], [Date], [Latitude], [Longitude], [Fix Status])
         VALUES (@T1_file2id, 1, '2012-05-11', '60.21', '-154.21', 'Fix Available'),
                (@T1_file2id, 2, '2012-05-12', '60.22', '-154.22', 'Fix Available'),
                (@T1_file2id, 3, '2012-05-13', '60.23', '-154.23', 'Fix Available')
    EXEC [dbo].[CollarFixes_Insert] @T1_file2id,@T1_format

    SELECT @T1_fix11 = FixId FROM CollarFixes Where [FileId] = @T1_file1id AND [LineNumber] = 1
    SELECT @T1_fix12 = FixId FROM CollarFixes Where [FileId] = @T1_file1id AND [LineNumber] = 2
    SELECT @T1_fix13 = FixId FROM CollarFixes Where [FileId] = @T1_file1id AND [LineNumber] = 3
    SELECT @T1_fix21 = FixId FROM CollarFixes Where [FileId] = @T1_file2id AND [LineNumber] = 1
    SELECT @T1_fix22 = FixId FROM CollarFixes Where [FileId] = @T1_file2id AND [LineNumber] = 2
    SELECT @T1_fix23 = FixId FROM CollarFixes Where [FileId] = @T1_file2id AND [LineNumber] = 3

    -- Check initial conditions
    IF    NOT EXISTS (SELECT 1 from Animals Where [ProjectId] = @T1_project AND [AnimalId] IN (@T1_animal1, @T1_animal2))
       OR NOT EXISTS (SELECT 1 from Collars Where [CollarManufacturer] = @T1_mfgr AND [CollarId] IN (@T1_collar1,@T1_collar2))
       OR NOT EXISTS (SELECT 1 from CollarFiles Where [FileId] IN (@T1_File1Id, @T1_File2Id))
       OR NOT EXISTS (SELECT 1 from CollarFixes Where [FileId] = @T1_file1id AND [CollarManufacturer] = @T1_mfgr AND [CollarId] = @T1_collar1)
       OR NOT EXISTS (SELECT 1 from CollarFixes Where [FileId] = @T1_file2id AND [CollarManufacturer] = @T1_mfgr AND [CollarId] = @T1_collar2)
       OR     EXISTS (SELECT 1 from CollarDeployments Where [ProjectId] = @T1_project)
       OR     EXISTS (SELECT 1 from Locations Where [ProjectId] = @T1_project AND [AnimalId] IN (@T1_animal1, @T1_animal2))
       OR     EXISTS (SELECT 1 from Movements Where [ProjectId] = @T1_project AND [AnimalId] IN (@T1_animal1, @T1_animal2))
    BEGIN
        PRINT '  Test data not initialized properly'
        RETURN
    END
    

    -- Do tests	

    -- Test1
    SET @T1_test = '  Test1: multiple valid deployments: '
    --Action
    EXEC [dbo].[CollarDeployment_Insert] @T1_project, @T1_animal1, @T1_mfgr, @T1_collar1, '2012-05-10 12:00', '2012-05-11 12:00'
    SELECT @T1_deploy1 = DeploymentId FROM CollarDeployments WHERE ProjectId = @T1_project and AnimalId = @T1_animal1 AND CollarManufacturer = @T1_mfgr AND CollarId = @T1_collar1 AND DeploymentDate = '2012-05-10 12:00'
    EXEC [dbo].[CollarDeployment_Insert] @T1_project, @T1_animal1, @T1_mfgr, @T1_collar2, '2012-05-11 12:01'
    SELECT @T1_deploy2 = DeploymentId FROM CollarDeployments WHERE ProjectId = @T1_project and AnimalId = @T1_animal1 AND CollarManufacturer = @T1_mfgr AND CollarId = @T1_collar2 AND DeploymentDate = '2012-05-11 12:01'
    EXEC [dbo].[CollarDeployment_Insert] @T1_project, @T1_animal2, @T1_mfgr, @T1_collar2, '2012-05-10 12:00', '2012-05-11 12:00'
    SELECT @T1_deploy3 = DeploymentId FROM CollarDeployments WHERE ProjectId = @T1_project and AnimalId = @T1_animal2 AND CollarManufacturer = @T1_mfgr AND CollarId = @T1_collar2 AND DeploymentDate = '2012-05-10 12:00'
    EXEC [dbo].[CollarDeployment_Insert] @T1_project, @T1_animal2, @T1_mfgr, @T1_collar1, '2012-05-11 12:01'
    SELECT @T1_deploy4 = DeploymentId FROM CollarDeployments WHERE ProjectId = @T1_project and AnimalId = @T1_animal2 AND CollarManufacturer = @T1_mfgr AND CollarId = @T1_collar1 AND DeploymentDate = '2012-05-11 12:01'
    --Test
    SET @T1_msg = ''

--SELECT * FROM Animals where [AnimalId] IN (@T1_animal1, @T1_animal2)
--SELECT * FROM CollarDeployments where [AnimalId] IN (@T1_animal1, @T1_animal2)
--SELECT * from CollarFixes Where [FileId] IN (@T1_file1id, @T1_file2id)
--SELECT * from Locations Where [ProjectId] = @T1_project AND [AnimalId] IN (@T1_animal1, @T1_animal2)
--SELECT * from Movements Where [ProjectId] = @T1_project AND [AnimalId] IN (@T1_animal1, @T1_animal2)

    SELECT 1 from CollarFixes Where [FileId] IN (@T1_file1id, @T1_file2id)
    IF @@ROWCOUNT <> 6
        SET @T1_msg = @T1_msg + ' Not 6 fixes'
    SELECT 1 from Locations Where [ProjectId] = @T1_project AND [AnimalId] = @T1_animal1
    IF @@ROWCOUNT <> 3
        SET @T1_msg = @T1_msg + ' Not 3 locations for animal 1'
    SELECT 1 from Movements Where [ProjectId] = @T1_project AND [AnimalId] = @T1_animal1
    IF @@ROWCOUNT <> 2
        SET @T1_msg = @T1_msg + ' Not 2 movements for animal 1'
    SELECT 1 from Locations Where [ProjectId] = @T1_project AND [AnimalId] = @T1_animal2
    IF @@ROWCOUNT <> 2
        SET @T1_msg = @T1_msg + ' Not 2 locations for animal 2'
    SELECT 1 from Movements Where [ProjectId] = @T1_project AND [AnimalId] = @T1_animal2
    IF @@ROWCOUNT <> 1
        SET @T1_msg = @T1_msg + ' Not 1 movements for animal 2'
        
    SELECT top 1 @T1_fix_l = [FixId] FROM Locations Where [ProjectId] = @T1_project AND [AnimalId] = @T1_animal1 order by FixDate
    IF @T1_fix11 <> @T1_fix_l
        SET @T1_msg = @T1_msg + ' a1 loc1 is based on wrong fix'
        
    IF NOT EXISTS (SELECT 1 FROM Locations Where [ProjectId] = @T1_project AND [AnimalId] = @T1_animal1 AND FixId = @T1_fix22)
        SET @T1_msg = @T1_msg + ' a1 loc2 is based on wrong fix'
    
    SELECT top 1 @T1_fix_l = [FixId] FROM Locations Where [ProjectId] = @T1_project AND [AnimalId] = @T1_animal1 order by FixDate desc
    IF @T1_fix23 <> @T1_fix_l
        SET @T1_msg = @T1_msg + ' a1 loc3 is based on wrong fix'
    
    SELECT top 1 @T1_fix_l = [FixId] FROM Locations Where [ProjectId] = @T1_project AND [AnimalId] = @T1_animal2 order by FixDate
    IF @T1_fix21 <> @T1_fix_l
        SET @T1_msg = @T1_msg + ' a2 loc1 is based on wrong fix'
    
    SELECT top 1 @T1_fix_l = [FixId] FROM Locations Where [ProjectId] = @T1_project AND [AnimalId] = @T1_animal2 order by FixDate desc
    IF @T1_fix12 <> @T1_fix_l
        SET @T1_msg = @T1_msg + ' a2 loc2 is based on wrong fix'
    
    IF @T1_msg = ''
        SET @T1_msg = 'Passed'
    ELSE
        SET @T1_msg = 'Failed' + @T1_msg
    --Report
    SET @T1_msg = @T1_test + @T1_msg
    PRINT @T1_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T1_deploy1
    EXEC [dbo].[CollarDeployment_Delete] @T1_deploy2
    EXEC [dbo].[CollarDeployment_Delete] @T1_deploy3
    EXEC [dbo].[CollarDeployment_Delete] @T1_deploy4
    

    -- Test2
    SET @T1_test = '  Test2: 2 collars on 1 animal Rdate1 = null : '
    --Action/Test
    EXEC [dbo].[CollarDeployment_Insert] @T1_project, @T1_animal1, @T1_mfgr, @T1_collar1, '2012-05-10 12:00', NULL
    SELECT @T1_deploy1 = DeploymentId FROM CollarDeployments WHERE ProjectId = @T1_project and AnimalId = @T1_animal1 AND CollarManufacturer = @T1_mfgr AND CollarId = @T1_collar1 AND DeploymentDate = '2012-05-10 12:00'
    BEGIN TRY
        EXEC [dbo].[CollarDeployment_Insert] @T1_project, @T1_animal1, @T1_mfgr, @T1_collar2, '2012-05-11 12:00'
		SELECT @T1_deploy2 = DeploymentId FROM CollarDeployments WHERE ProjectId = @T1_project and AnimalId = @T1_animal1 AND CollarManufacturer = @T1_mfgr AND CollarId = @T1_collar2 AND DeploymentDate = '2012-05-11 12:00'
        SET @T1_msg = 'Failed'
    END TRY
    BEGIN CATCH
        SET @T1_msg = 'Passed'
    END CATCH
    --Report
    SET @T1_msg = @T1_test + @T1_msg
    PRINT @T1_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T1_deploy1
    EXEC [dbo].[CollarDeployment_Delete] @T1_deploy2

        
    -- Test3
    SET @T1_test = '  Test3: 2 collars on 1 animal Rdate1 <> null : '
    --Action/Test
    EXEC [dbo].[CollarDeployment_Insert] @T1_project, @T1_animal1, @T1_mfgr, @T1_collar1, '2012-05-10 12:00', '2012-05-12 12:00'
    SELECT @T1_deploy1 = DeploymentId FROM CollarDeployments WHERE ProjectId = @T1_project and AnimalId = @T1_animal1 AND CollarManufacturer = @T1_mfgr AND CollarId = @T1_collar1 AND DeploymentDate = '2012-05-10 12:00'
    BEGIN TRY
        EXEC [dbo].[CollarDeployment_Insert] @T1_project, @T1_animal1, @T1_mfgr, @T1_collar2, '2012-05-11 12:00'
		SELECT @T1_deploy2 = DeploymentId FROM CollarDeployments WHERE ProjectId = @T1_project and AnimalId = @T1_animal1 AND CollarManufacturer = @T1_mfgr AND CollarId = @T1_collar2 AND DeploymentDate = '2012-05-11 12:00'
        SET @T1_msg = 'Failed'
    END TRY
    BEGIN CATCH
        SET @T1_msg = 'Passed'
    END CATCH
    --Report
    SET @T1_msg = @T1_test + @T1_msg
    PRINT @T1_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T1_deploy1
    EXEC [dbo].[CollarDeployment_Delete] @T1_deploy2

    -- Test4
    SET @T1_test = '  Test4: 2 collars on 1 animal Rdate1 = Ddate2 : '
    --Action/Test
    EXEC [dbo].[CollarDeployment_Insert] @T1_project, @T1_animal1, @T1_mfgr, @T1_collar1, '2012-05-10 12:00', '2012-05-11 12:00'
    SELECT @T1_deploy1 = DeploymentId FROM CollarDeployments WHERE ProjectId = @T1_project and AnimalId = @T1_animal1 AND CollarManufacturer = @T1_mfgr AND CollarId = @T1_collar1 AND DeploymentDate = '2012-05-10 12:00'
    BEGIN TRY
        EXEC [dbo].[CollarDeployment_Insert] @T1_project, @T1_animal1, @T1_mfgr, @T1_collar2, '2012-05-11 12:00'
		SELECT @T1_deploy2 = DeploymentId FROM CollarDeployments WHERE ProjectId = @T1_project and AnimalId = @T1_animal1 AND CollarManufacturer = @T1_mfgr AND CollarId = @T1_collar2 AND DeploymentDate = '2012-05-11 12:00'
        SET @T1_msg = 'Failed'
    END TRY
    BEGIN CATCH
        SET @T1_msg = 'Passed'
    END CATCH
    --Report
    SET @T1_msg = @T1_test + @T1_msg
    PRINT @T1_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T1_deploy1
    EXEC [dbo].[CollarDeployment_Delete] @T1_deploy2


    -- Test5
    SET @T1_test = '  Test5: 1 collar on 2 animals Rdate1 = null: '
    --Action/Test
    EXEC [dbo].[CollarDeployment_Insert] @T1_project, @T1_animal1, @T1_mfgr, @T1_collar1, '2012-05-10 12:00', NULL
    SELECT @T1_deploy1 = DeploymentId FROM CollarDeployments WHERE ProjectId = @T1_project and AnimalId = @T1_animal1 AND CollarManufacturer = @T1_mfgr AND CollarId = @T1_collar1 AND DeploymentDate = '2012-05-10 12:00'
    BEGIN TRY
        EXEC [dbo].[CollarDeployment_Insert] @T1_project, @T1_animal2, @T1_mfgr, @T1_collar1, '2012-05-11 12:00'
		SELECT @T1_deploy2 = DeploymentId FROM CollarDeployments WHERE ProjectId = @T1_project and AnimalId = @T1_animal2 AND CollarManufacturer = @T1_mfgr AND CollarId = @T1_collar1 AND DeploymentDate = '2012-05-11 12:00'
        SET @T1_msg = 'Failed'
    END TRY
    BEGIN CATCH
        SET @T1_msg = 'Passed'
    END CATCH
    --Report
    SET @T1_msg = @T1_test + @T1_msg
    PRINT @T1_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T1_deploy1
    EXEC [dbo].[CollarDeployment_Delete] @T1_deploy2
        
        
    -- Test6
    SET @T1_test = '  Test6: 1 collar on 2 animals Rdate1 <> null: '
    --Action/Test
    EXEC [dbo].[CollarDeployment_Insert] @T1_project, @T1_animal1, @T1_mfgr, @T1_collar1, '2012-05-10 12:00', '2012-05-12 12:00'
    SELECT @T1_deploy1 = DeploymentId FROM CollarDeployments WHERE ProjectId = @T1_project and AnimalId = @T1_animal1 AND CollarManufacturer = @T1_mfgr AND CollarId = @T1_collar1 AND DeploymentDate = '2012-05-10 12:00'
    BEGIN TRY
        EXEC [dbo].[CollarDeployment_Insert] @T1_project, @T1_animal2, @T1_mfgr, @T1_collar1, '2012-05-11 12:00'
		SELECT @T1_deploy2 = DeploymentId FROM CollarDeployments WHERE ProjectId = @T1_project and AnimalId = @T1_animal2 AND CollarManufacturer = @T1_mfgr AND CollarId = @T1_collar1 AND DeploymentDate = '2012-05-11 12:00'
        SET @T1_msg = 'Failed'
    END TRY
    BEGIN CATCH
        SET @T1_msg = 'Passed'
    END CATCH
    --Report
    SET @T1_msg = @T1_test + @T1_msg
    PRINT @T1_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T1_deploy1
    EXEC [dbo].[CollarDeployment_Delete] @T1_deploy2
        

    -- Test7
    SET @T1_test = '  Test7: 1 collar on 2 animals Rdate1 <> Ddate2: '
    --Action/Test
    EXEC [dbo].[CollarDeployment_Insert] @T1_project, @T1_animal1, @T1_mfgr, @T1_collar1, '2012-05-10 12:00', '2012-05-11 12:00'
    SELECT @T1_deploy1 = DeploymentId FROM CollarDeployments WHERE ProjectId = @T1_project and AnimalId = @T1_animal1 AND CollarManufacturer = @T1_mfgr AND CollarId = @T1_collar1 AND DeploymentDate = '2012-05-10 12:00'
    BEGIN TRY
        EXEC [dbo].[CollarDeployment_Insert] @T1_project, @T1_animal2, @T1_mfgr, @T1_collar1, '2012-05-11 12:00'
		SELECT @T1_deploy2 = DeploymentId FROM CollarDeployments WHERE ProjectId = @T1_project and AnimalId = @T1_animal2 AND CollarManufacturer = @T1_mfgr AND CollarId = @T1_collar1 AND DeploymentDate = '2012-05-11 12:00'
        SET @T1_msg = 'Failed'
    END TRY
    BEGIN CATCH
        SET @T1_msg = 'Passed'
    END CATCH
    --Report
    SET @T1_msg = @T1_test + @T1_msg
    PRINT @T1_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T1_deploy1
    EXEC [dbo].[CollarDeployment_Delete] @T1_deploy2


    -- Test8
    SET @T1_test = '  Test8: Rdate = Ddate: '
    --Action/Test
    BEGIN TRY
        EXEC [dbo].[CollarDeployment_Insert] @T1_project, @T1_animal1, @T1_mfgr, @T1_collar1, '2012-05-10 12:00', '2012-05-10 12:00'
		SELECT @T1_deploy2 = DeploymentId FROM CollarDeployments WHERE ProjectId = @T1_project and AnimalId = @T1_animal1 AND CollarManufacturer = @T1_mfgr AND CollarId = @T1_collar1 AND DeploymentDate = '2012-05-10 12:00'
        SET @T1_msg = 'Failed'
    END TRY
    BEGIN CATCH
        SET @T1_msg = 'Passed'
    END CATCH
    --Report
    SET @T1_msg = @T1_test + @T1_msg
    PRINT @T1_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T1_deploy2


    -- Test9
    SET @T1_test = '  Test9: Rdate < Ddate: '
    --Action/Test
    BEGIN TRY
        EXEC [dbo].[CollarDeployment_Insert] @T1_project, @T1_animal1, @T1_mfgr, @T1_collar1, '2012-05-11 12:00', '2012-05-10 12:00'
		SELECT @T1_deploy2 = DeploymentId FROM CollarDeployments WHERE ProjectId = @T1_project and AnimalId = @T1_animal1 AND CollarManufacturer = @T1_mfgr AND CollarId = @T1_collar1 AND DeploymentDate = '2012-05-11 12:00'
        SET @T1_msg = 'Failed'
    END TRY
    BEGIN CATCH
        SET @T1_msg = 'Passed'
    END CATCH
    --Report
    SET @T1_msg = @T1_test + @T1_msg
    PRINT @T1_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T1_deploy2


    -- Clean up
    PRINT '  Cleaning up'
    EXEC [dbo].[CollarFile_Delete] @T1_file1id
    EXEC [dbo].[CollarFile_Delete] @T1_file2id
    DELETE CollarDeployments Where [ProjectId] = @T1_project 
    EXEC [dbo].[Collar_Delete] @T1_mfgr, @T1_collar1
    EXEC [dbo].[Collar_Delete] @T1_mfgr, @T1_collar2
    Delete Animals where ProjectId = @T1_project -- SP checks if user is in role, which sa is not
    EXEC [dbo].[Project_Delete] @T1_project

    -- Check that cleanup worked
    IF    EXISTS (SELECT 1 from Animals Where [ProjectId] = @T1_project AND [AnimalId] IN (@T1_animal1, @T1_animal2))
       OR EXISTS (SELECT 1 from Projects Where [ProjectId] = @T1_project)
       OR EXISTS (SELECT 1 from Collars Where [CollarManufacturer] = @T1_mfgr AND [CollarId] IN (@T1_collar1,@T1_collar2))
       OR EXISTS (SELECT 1 from CollarDeployments Where [ProjectId] = @T1_project)
       OR EXISTS (SELECT 1 from CollarFiles Where [FileId] IN (@T1_File1Id, @T1_File2Id))
       OR EXISTS (SELECT 1 from CollarDataTelonicsStoreOnBoard Where [FileId] IN (@T1_File1Id, @T1_File2Id))
       OR EXISTS (SELECT 1 from CollarFixes Where [FileId] = @T1_file1id AND [CollarManufacturer] = @T1_mfgr AND [CollarId] = @T1_collar1)
       OR EXISTS (SELECT 1 from CollarFixes Where [FileId] = @T1_file2id AND [CollarManufacturer] = @T1_mfgr AND [CollarId] = @T1_collar2)
       OR EXISTS (SELECT 1 from Locations Where [ProjectId] = @T1_project AND [AnimalId] IN (@T1_animal1, @T1_animal2))
       OR EXISTS (SELECT 1 from Movements Where [ProjectId] = @T1_project AND [AnimalId] IN (@T1_animal1, @T1_animal2))
    BEGIN
        PRINT 'Test data not completely removed'
        RETURN
    END

    -- Remove the sa from project investigator
    DELETE [dbo].[ProjectInvestigators] WHERE  [Login] = @T1_sa








--  ************************************************************
    PRINT 'Test_Trigger_CollarDeployments_Update'
--  ************************************************************
--  Author:	     Regan Sarwas
--  Create date: June 15, 2012
--  Description: Test the CollarDeployments UpdateTrigger
--  ============================================================

/*  TESTS:
Check locations after changing deployment
 rdate t1 -> null
 rdate null -> t1
 rdate t1 -> t2
 rdate t2 -> t1
 repeat with animal with a mortality date

Check illegal modifications of deployment
 ddate = t1, rdate null -> t1
 ddate = t2, rdate null -> t1
 same animal, same collar
 d1date =  t3, r1date = t5, d2date =  t1, r2date t2 -> t3
 d1date =  t3, r1date = t5, d2date =  t1, r2date t2 -> t4
 d1date =  t3, r1date = t5, d2date =  t1, r2date t2 -> t6
 d1date =  t3, r1date = t5, d2date =  t1, r2date t2 -> null
 same animal, different collar
 d1date =  t3, r1date = t5, d2date =  t1, r2date t2 -> t3
 d1date =  t3, r1date = t5, d2date =  t1, r2date t2 -> t4
 d1date =  t3, r1date = t5, d2date =  t1, r2date t2 -> t6
 d1date =  t3, r1date = t5, d2date =  t1, r2date t2 -> null
 different animal, same collar
 d1date =  t3, r1date = t5, d2date =  t1, r2date t2 -> t3
 d1date =  t3, r1date = t5, d2date =  t1, r2date t2 -> t4
 d1date =  t3, r1date = t5, d2date =  t1, r2date t2 -> t6
 d1date =  t3, r1date = t5, d2date =  t1, r2date t2 -> null
*/

    DECLARE
            @T2_sa nvarchar(16) = 'sa',
            @T2_project nvarchar(16) = 'p1__',
            @T2_animal1 nvarchar(16) = 'a1__',
            @T2_animal2 nvarchar(16) = 'a2__',
            @T2_mfgr nvarchar(16) = 'Telonics',
            @T2_model nvarchar(16) = 'TelonicsGen4',
            @T2_collar1 nvarchar(16) = 'c1__',
            @T2_collar2 nvarchar(16) = 'c2__',
            @T2_file nvarchar(16) = 'f1__',
            @T2_fileid int,
            @T2_format nvarchar(16) = 'A', -- Store on board format
            @T2_test nvarchar(255) = null,
            @T2_msg nvarchar(255) = null,
            @T2_fix1 bigint,
            @T2_fix2 bigint,
            @T2_fix3 bigint,
            @T2_fix4 bigint,
            @T2_fix5 bigint,
            @T2_fix6 bigint,
            @T2_fix_l bigint
            
    -- This test must be run as SA, since others cannot operate on table directly
    -- This test the integrity of the tables underlying the Store Procedures available to users.
    -- Also SA can check user tables

    IF ORIGINAL_LOGIN() <> @T2_sa
    BEGIN
        PRINT '  You must be the sa to run this test'
        RETURN 1
    END

/*
    -- Clean up mess if previous test failed; need to find and set the file id first
    SET @T2_fileid = 140
    EXEC [dbo].[CollarFile_Delete] @T2_fileid
    DELETE CollarDeployments Where [ProjectId] = @T2_project 
    DELETE Collars where CollarManufacturer = @T2_mfgr and CollarId = @T2_collar
    Delete Animals where ProjectId = @T2_project -- SP checks if user is in role, which sa is not
    EXEC [dbo].[Project_Delete] @T2_project
    return 1
*/

    --Check to make sure we are not going to overwrite any data
    IF EXISTS (SELECT 1 from Projects Where [ProjectId] = @T2_project)
    OR EXISTS (SELECT 1 from Collars Where [CollarManufacturer] = @T2_mfgr AND [CollarId] IN (@T2_collar1,@T2_collar2))
    BEGIN
        PRINT '  Aborting tests.  Existing data conflicts with test data.'
        RETURN 1
    END
    
    -- Add the sa is a project investigator
    IF NOT EXISTS(SELECT 1 FROM [dbo].[ProjectInvestigators] WHERE [Login] = @T2_sa)
    BEGIN
        INSERT [dbo].[ProjectInvestigators] ([Login],[Name],[Email],[Phone]) VALUES (@T2_sa,@T2_sa,@T2_sa,@T2_sa)
    END


    -- Setup for test
    EXEC [dbo].[Project_Insert] @T2_project, @T2_project, @T2_sa
    EXEC [dbo].[Animal_Insert] @T2_project, @T2_animal1
    EXEC [dbo].[Animal_Insert] @T2_project, @T2_animal2, null, null, '2012-05-14 12:00'
    EXEC [dbo].[Collar_Insert] @T2_mfgr, @T2_collar1, @T2_model ,@T2_sa
    EXEC [dbo].[Collar_Insert] @T2_mfgr, @T2_collar2, @T2_model ,@T2_sa

    INSERT INTO dbo.CollarFiles ([FileName], [Project], [CollarManufacturer], [CollarId], [Format], [Status], [Contents])
         VALUES (@T2_file, @T2_project, @T2_mfgr, @T2_collar1, @T2_format, 'A', convert(varbinary,'data'))  -- 'A' = Store on board
    SET @T2_fileid = SCOPE_IDENTITY();
    INSERT INTO dbo.CollarDataTelonicsStoreOnBoard ([FileId], [LineNumber], [Date], [Latitude], [Longitude], [Fix Status])
         VALUES (@T2_fileid, 1, '2012-05-11', '60.11', '-154.11', 'Fix Available'),
                (@T2_fileid, 2, '2012-05-12', '60.12', '-154.12', 'Fix Available'),
                (@T2_fileid, 3, '2012-05-13', '60.13', '-154.13', 'Fix Available'),
                (@T2_fileid, 4, '2012-05-14', '60.14', '-154.14', 'Fix Available'),
                (@T2_fileid, 5, '2012-05-15', '60.15', '-154.15', 'Fix Available'),
                (@T2_fileid, 6, '2012-05-16', '60.16', '-154.16', 'Fix Available')
    EXEC [dbo].[CollarFixes_Insert] @T2_fileid,@T2_format

    SELECT @T2_fix1 = FixId FROM CollarFixes Where [FileId] = @T2_fileid AND [LineNumber] = 1
    SELECT @T2_fix2 = FixId FROM CollarFixes Where [FileId] = @T2_fileid AND [LineNumber] = 2
    SELECT @T2_fix3 = FixId FROM CollarFixes Where [FileId] = @T2_fileid AND [LineNumber] = 3
    SELECT @T2_fix4 = FixId FROM CollarFixes Where [FileId] = @T2_fileid AND [LineNumber] = 4
    SELECT @T2_fix5 = FixId FROM CollarFixes Where [FileId] = @T2_fileid AND [LineNumber] = 5
    SELECT @T2_fix6 = FixId FROM CollarFixes Where [FileId] = @T2_fileid AND [LineNumber] = 6

    -- Check initial conditions
    IF    NOT EXISTS (SELECT 1 from Animals Where [ProjectId] = @T2_project AND [AnimalId] IN (@T2_animal1, @T2_animal2))
       OR NOT EXISTS (SELECT 1 from Collars Where [CollarManufacturer] = @T2_mfgr AND [CollarId] IN (@T2_collar1,@T2_collar2))
       OR NOT EXISTS (SELECT 1 from CollarFiles Where [FileId] = @T2_FileId)
       OR NOT EXISTS (SELECT 1 from CollarFixes Where [FileId] = @T2_fileid AND [CollarManufacturer] = @T2_mfgr AND [CollarId] = @T2_collar1)
       OR     EXISTS (SELECT 1 from CollarDeployments Where [ProjectId] = @T2_project)
       OR     EXISTS (SELECT 1 from Locations Where [ProjectId] = @T2_project AND [AnimalId] IN (@T2_animal1, @T2_animal2))
       OR     EXISTS (SELECT 1 from Movements Where [ProjectId] = @T2_project AND [AnimalId] IN (@T2_animal1, @T2_animal2))
    BEGIN
        PRINT '  Test data not initialized properly'
        RETURN 1
    END
    

    -- Do tests
    PRINT '  Check locations after legal changes to deployment retrieval date'

    -- Test1
    SET @T2_test = '  Test1: rdate t1 -> null: '
    --Action
    EXEC [dbo].[CollarDeployment_Insert] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11 12:00', '2012-05-14 12:00'
    --Test
    SET @T2_msg = ''

    SELECT 1 from Locations Where [ProjectId] = @T2_project AND [AnimalId] = @T2_animal1
    IF @@ROWCOUNT <> 3
        SET @T2_msg = @T2_msg + ' Not 3 locations at start'

    EXEC [dbo].[CollarDeployment_UpdateRetrievalDate] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11 12:00', NULL

    SELECT 1 from Locations Where [ProjectId] = @T2_project AND [AnimalId] = @T2_animal1
    IF @@ROWCOUNT <> 5
        SET @T2_msg = @T2_msg + ' Not 5 locations at end'
        
    IF @T2_msg = ''
        SET @T2_msg = 'Passed'
    ELSE
        SET @T2_msg = 'Failed' + @T2_msg
    --Report
    SET @T2_msg = @T2_test + @T2_msg
    PRINT @T2_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11 12:00'


    -- Test2
    SET @T2_test = '  Test2: rdate null -> t1: '
    --Action
    EXEC [dbo].[CollarDeployment_Insert] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11 12:00', NULL
    --Test
    SET @T2_msg = ''

    SELECT 1 from Locations Where [ProjectId] = @T2_project AND [AnimalId] = @T2_animal1
    IF @@ROWCOUNT <> 5
        SET @T2_msg = @T2_msg + ' Not 5 locations at start'

    EXEC [dbo].[CollarDeployment_UpdateRetrievalDate] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11 12:00', '2012-05-14 12:00'

    SELECT 1 from Locations Where [ProjectId] = @T2_project AND [AnimalId] = @T2_animal1
    IF @@ROWCOUNT <> 3
        SET @T2_msg = @T2_msg + ' Not 3 locations at end'
        
    IF @T2_msg = ''
        SET @T2_msg = 'Passed'
    ELSE
        SET @T2_msg = 'Failed' + @T2_msg
    --Report
    SET @T2_msg = @T2_test + @T2_msg
    PRINT @T2_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11 12:00'


    -- Test3
    SET @T2_test = '  Test3: t1 -> t2: '
    --Action
    EXEC [dbo].[CollarDeployment_Insert] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11 12:00', '2012-05-13 12:00'
    --Test
    SET @T2_msg = ''

    SELECT 1 from Locations Where [ProjectId] = @T2_project AND [AnimalId] = @T2_animal1
    IF @@ROWCOUNT <> 2
        SET @T2_msg = @T2_msg + ' Not 2 locations at start'

    EXEC [dbo].[CollarDeployment_UpdateRetrievalDate] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11 12:00', '2012-05-15 12:00'

    SELECT 1 from Locations Where [ProjectId] = @T2_project AND [AnimalId] = @T2_animal1
    IF @@ROWCOUNT <> 4
        SET @T2_msg = @T2_msg + ' Not 4 locations at end'
        
    IF @T2_msg = ''
        SET @T2_msg = 'Passed'
    ELSE
        SET @T2_msg = 'Failed' + @T2_msg
    --Report
    SET @T2_msg = @T2_test + @T2_msg
    PRINT @T2_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11 12:00'


    -- Test4
    SET @T2_test = '  Test4: t2 -> t1: '
    --Action
    EXEC [dbo].[CollarDeployment_Insert] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11 12:00', '2012-05-15 12:00'
    --Test
    SET @T2_msg = ''

    SELECT 1 from Locations Where [ProjectId] = @T2_project AND [AnimalId] = @T2_animal1
    IF @@ROWCOUNT <> 4
        SET @T2_msg = @T2_msg + ' Not 4 locations at start'

    EXEC [dbo].[CollarDeployment_UpdateRetrievalDate] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11 12:00', '2012-05-13 12:00'

    SELECT 1 from Locations Where [ProjectId] = @T2_project AND [AnimalId] = @T2_animal1
    IF @@ROWCOUNT <> 2
        SET @T2_msg = @T2_msg + ' Not 2 locations at end'
        
    IF @T2_msg = ''
        SET @T2_msg = 'Passed'
    ELSE
        SET @T2_msg = 'Failed' + @T2_msg
    --Report
    SET @T2_msg = @T2_test + @T2_msg
    PRINT @T2_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11 12:00'




    -- Test1a
    SET @T2_test = '  Test1a: rdate t1 -> null; w/ mort: '
    --Action
    EXEC [dbo].[CollarDeployment_Insert] @T2_project, @T2_animal2, @T2_mfgr, @T2_collar1, '2012-05-11 12:00', '2012-05-13 12:00'
    --Test
    SET @T2_msg = ''

    SELECT 1 from Locations Where [ProjectId] = @T2_project AND [AnimalId] = @T2_animal2
    IF @@ROWCOUNT <> 2
        SET @T2_msg = @T2_msg + ' Not 2 locations at start'

    EXEC [dbo].[CollarDeployment_UpdateRetrievalDate] @T2_project, @T2_animal2, @T2_mfgr, @T2_collar1, '2012-05-11 12:00', NULL

    SELECT 1 from Locations Where [ProjectId] = @T2_project AND [AnimalId] = @T2_animal2
    IF @@ROWCOUNT <> 3
        SET @T2_msg = @T2_msg + ' Not 3 locations at end'
        
    IF @T2_msg = ''
        SET @T2_msg = 'Passed'
    ELSE
        SET @T2_msg = 'Failed' + @T2_msg
    --Report
    SET @T2_msg = @T2_test + @T2_msg
    PRINT @T2_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T2_project, @T2_animal2, @T2_mfgr, @T2_collar1, '2012-05-11 12:00'


    -- Test2a
    SET @T2_test = '  Test2a: rdate null -> t1; w/ mort: '
    --Action
    EXEC [dbo].[CollarDeployment_Insert] @T2_project, @T2_animal2, @T2_mfgr, @T2_collar1, '2012-05-11 12:00', NULL
    --Test
    SET @T2_msg = ''

    SELECT 1 from Locations Where [ProjectId] = @T2_project AND [AnimalId] = @T2_animal2
    IF @@ROWCOUNT <> 3
        SET @T2_msg = @T2_msg + ' Not 3 locations at start'

    EXEC [dbo].[CollarDeployment_UpdateRetrievalDate] @T2_project, @T2_animal2, @T2_mfgr, @T2_collar1, '2012-05-11 12:00', '2012-05-13 12:00'

    SELECT 1 from Locations Where [ProjectId] = @T2_project AND [AnimalId] = @T2_animal2
    IF @@ROWCOUNT <> 2
        SET @T2_msg = @T2_msg + ' Not 2 locations at end'
        
    IF @T2_msg = ''
        SET @T2_msg = 'Passed'
    ELSE
        SET @T2_msg = 'Failed' + @T2_msg
    --Report
    SET @T2_msg = @T2_test + @T2_msg
    PRINT @T2_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T2_project, @T2_animal2, @T2_mfgr, @T2_collar1, '2012-05-11 12:00'


    -- Test3a
    SET @T2_test = '  Test3a: t1 -> t2; w/ mort: '
    --Action
    EXEC [dbo].[CollarDeployment_Insert] @T2_project, @T2_animal2, @T2_mfgr, @T2_collar1, '2012-05-11 12:00', '2012-05-13 12:00'
    --Test
    SET @T2_msg = ''

    SELECT 1 from Locations Where [ProjectId] = @T2_project AND [AnimalId] = @T2_animal2
    IF @@ROWCOUNT <> 2
        SET @T2_msg = @T2_msg + ' Not 2 locations at start'

    EXEC [dbo].[CollarDeployment_UpdateRetrievalDate] @T2_project, @T2_animal2, @T2_mfgr, @T2_collar1, '2012-05-11 12:00', '2012-05-15 12:00'

    SELECT 1 from Locations Where [ProjectId] = @T2_project AND [AnimalId] = @T2_animal2
    IF @@ROWCOUNT <> 3
        SET @T2_msg = @T2_msg + ' Not 3 locations at end'
        
    IF @T2_msg = ''
        SET @T2_msg = 'Passed'
    ELSE
        SET @T2_msg = 'Failed' + @T2_msg
    --Report
    SET @T2_msg = @T2_test + @T2_msg
    PRINT @T2_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T2_project, @T2_animal2, @T2_mfgr, @T2_collar1, '2012-05-11 12:00'


    -- Test4a
    SET @T2_test = '  Test4a: t2 -> t1; w/ mort: '
    --Action
    EXEC [dbo].[CollarDeployment_Insert] @T2_project, @T2_animal2, @T2_mfgr, @T2_collar1, '2012-05-11 12:00', '2012-05-15 12:00'
    --Test
    SET @T2_msg = ''

    SELECT 1 from Locations Where [ProjectId] = @T2_project AND [AnimalId] = @T2_animal2
    IF @@ROWCOUNT <> 3
        SET @T2_msg = @T2_msg + ' Not 3 locations at start'

    EXEC [dbo].[CollarDeployment_UpdateRetrievalDate] @T2_project, @T2_animal2, @T2_mfgr, @T2_collar1, '2012-05-11 12:00', '2012-05-13 12:00'

    SELECT 1 from Locations Where [ProjectId] = @T2_project AND [AnimalId] = @T2_animal2
    IF @@ROWCOUNT <> 2
        SET @T2_msg = @T2_msg + ' Not 2 locations at end'
        
    IF @T2_msg = ''
        SET @T2_msg = 'Passed'
    ELSE
        SET @T2_msg = 'Failed' + @T2_msg
    --Report
    SET @T2_msg = @T2_test + @T2_msg
    PRINT @T2_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T2_project, @T2_animal2, @T2_mfgr, @T2_collar1, '2012-05-11 12:00'





    print '  Checking illegal modifications of deployment'

    -- Test5
    SET @T2_test = '  Test5: ddate = t1, rdate null -> t1: '
    --Action/Test
    EXEC [dbo].[CollarDeployment_Insert] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11', NULL
    BEGIN TRY
        EXEC [dbo].[CollarDeployment_UpdateRetrievalDate] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11', '2012-05-11'
        SET @T2_msg = 'Failed'
    END TRY
    BEGIN CATCH
        SET @T2_msg = 'Passed'
    END CATCH
    --Report
    SET @T2_msg = @T2_test + @T2_msg
    PRINT @T2_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11'
        
        
    -- Test6
    SET @T2_test = '  Test6:  ddate = t2, rdate null -> t1: '
    --Action/Test
    EXEC [dbo].[CollarDeployment_Insert] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11', NULL
    BEGIN TRY
        EXEC [dbo].[CollarDeployment_UpdateRetrievalDate] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11', '2012-05-10'
        SET @T2_msg = 'Failed'
    END TRY
    BEGIN CATCH
        SET @T2_msg = 'Passed'
    END CATCH
    --Report
    SET @T2_msg = @T2_test + @T2_msg
    PRINT @T2_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11'


    -- Test7a
    SET @T2_test = '  Test7a: same animal; same collar; d1date =  t3, r1date = t5, d2date =  t1, r2date t2 -> t3: '
    --Action/Test
    EXEC [dbo].[CollarDeployment_Insert] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-13', '2012-05-15'
    EXEC [dbo].[CollarDeployment_Insert] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11', '2012-05-12'
    BEGIN TRY
        EXEC [dbo].[CollarDeployment_UpdateRetrievalDate] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11', '2012-05-13'
        SET @T2_msg = 'Failed'
    END TRY
    BEGIN CATCH
        SET @T2_msg = 'Passed'
    END CATCH
    --Report
    SET @T2_msg = @T2_test + @T2_msg
    PRINT @T2_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-13'
    EXEC [dbo].[CollarDeployment_Delete] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11'

    -- Test7b
    SET @T2_test = '  Test7b: same animal; same collar; d1date =  t3, r1date = t5, d2date =  t1, r2date t2 -> t4: '
    --Action/Test
    EXEC [dbo].[CollarDeployment_Insert] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-13', '2012-05-15'
    EXEC [dbo].[CollarDeployment_Insert] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11', '2012-05-12'
    BEGIN TRY
        EXEC [dbo].[CollarDeployment_UpdateRetrievalDate] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11', '2012-05-14'
        SET @T2_msg = 'Failed'
    END TRY
    BEGIN CATCH
        SET @T2_msg = 'Passed'
    END CATCH
    --Report
    SET @T2_msg = @T2_test + @T2_msg
    PRINT @T2_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-13'
    EXEC [dbo].[CollarDeployment_Delete] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11'

    -- Test7c
    SET @T2_test = '  Test7c: same animal; same collar;  d1date =  t3, r1date = t5, d2date =  t1, r2date t2 -> t6: '
    --Action/Test
    EXEC [dbo].[CollarDeployment_Insert] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-13', '2012-05-15'
    EXEC [dbo].[CollarDeployment_Insert] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11', '2012-05-12'
    BEGIN TRY
        EXEC [dbo].[CollarDeployment_UpdateRetrievalDate] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11', '2012-05-16'
        SET @T2_msg = 'Failed'
    END TRY
    BEGIN CATCH
        SET @T2_msg = 'Passed'
    END CATCH
    --Report
    SET @T2_msg = @T2_test + @T2_msg
    PRINT @T2_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-13'
    EXEC [dbo].[CollarDeployment_Delete] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11'

    -- Test7d
    SET @T2_test = '  Test7d: same animal; same collar; d1date =  t3, r1date = t5, d2date =  t1, r2date t2 -> null: '
    --Action/Test
    EXEC [dbo].[CollarDeployment_Insert] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-13', '2012-05-15'
    EXEC [dbo].[CollarDeployment_Insert] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11', '2012-05-12'
    BEGIN TRY
        EXEC [dbo].[CollarDeployment_UpdateRetrievalDate] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11', NULL
        SET @T2_msg = 'Failed'
    END TRY
    BEGIN CATCH
        SET @T2_msg = 'Passed'
    END CATCH
    --Report
    SET @T2_msg = @T2_test + @T2_msg
    PRINT @T2_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-13'
    EXEC [dbo].[CollarDeployment_Delete] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11'




    -- Test8a
    SET @T2_test = '  Test8a: same animal; different collar; d1date =  t3, r1date = t5, d2date =  t1, r2date t2 -> t3: '
    --Action/Test
    EXEC [dbo].[CollarDeployment_Insert] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar2, '2012-05-13', '2012-05-15'
    EXEC [dbo].[CollarDeployment_Insert] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11', '2012-05-12'
    BEGIN TRY
        EXEC [dbo].[CollarDeployment_UpdateRetrievalDate] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11', '2012-05-13'
        SET @T2_msg = 'Failed'
    END TRY
    BEGIN CATCH
        SET @T2_msg = 'Passed'
    END CATCH
    --Report
    SET @T2_msg = @T2_test + @T2_msg
    PRINT @T2_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar2, '2012-05-13'
    EXEC [dbo].[CollarDeployment_Delete] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11'

    -- Test8b
    SET @T2_test = '  Test8b: same animal; different collar; d1date =  t3, r1date = t5, d2date =  t1, r2date t2 -> t4: '
    --Action/Test
    EXEC [dbo].[CollarDeployment_Insert] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar2, '2012-05-13', '2012-05-15'
    EXEC [dbo].[CollarDeployment_Insert] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11', '2012-05-12'
    BEGIN TRY
        EXEC [dbo].[CollarDeployment_UpdateRetrievalDate] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11', '2012-05-14'
        SET @T2_msg = 'Failed'
    END TRY
    BEGIN CATCH
        SET @T2_msg = 'Passed'
    END CATCH
    --Report
    SET @T2_msg = @T2_test + @T2_msg
    PRINT @T2_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar2, '2012-05-13'
    EXEC [dbo].[CollarDeployment_Delete] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11'

    -- Test8c
    SET @T2_test = '  Test8c: same animal; different collar;  d1date =  t3, r1date = t5, d2date =  t1, r2date t2 -> t6: '
    --Action/Test
    EXEC [dbo].[CollarDeployment_Insert] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar2, '2012-05-13', '2012-05-15'
    EXEC [dbo].[CollarDeployment_Insert] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11', '2012-05-12'
    BEGIN TRY
        EXEC [dbo].[CollarDeployment_UpdateRetrievalDate] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11', '2012-05-16'
        SET @T2_msg = 'Failed'
    END TRY
    BEGIN CATCH
        SET @T2_msg = 'Passed'
    END CATCH
    --Report
    SET @T2_msg = @T2_test + @T2_msg
    PRINT @T2_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar2, '2012-05-13'
    EXEC [dbo].[CollarDeployment_Delete] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11'

    -- Test8d
    SET @T2_test = '  Test8d: same animal; different collar; d1date =  t3, r1date = t5, d2date =  t1, r2date t2 -> null: '
    --Action/Test
    EXEC [dbo].[CollarDeployment_Insert] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar2, '2012-05-13', '2012-05-15'
    EXEC [dbo].[CollarDeployment_Insert] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11', '2012-05-12'
    BEGIN TRY
        EXEC [dbo].[CollarDeployment_UpdateRetrievalDate] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11', NULL
        SET @T2_msg = 'Failed'
    END TRY
    BEGIN CATCH
        SET @T2_msg = 'Passed'
    END CATCH
    --Report
    SET @T2_msg = @T2_test + @T2_msg
    PRINT @T2_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar2, '2012-05-13'
    EXEC [dbo].[CollarDeployment_Delete] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11'




    -- Test9a
    SET @T2_test = '  Test9a: different animal; same collar; d1date =  t3, r1date = t5, d2date =  t1, r2date t2 -> t3: '
    --Action/Test
    EXEC [dbo].[CollarDeployment_Insert] @T2_project, @T2_animal2, @T2_mfgr, @T2_collar1, '2012-05-13', '2012-05-15'
    EXEC [dbo].[CollarDeployment_Insert] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11', '2012-05-12'
    BEGIN TRY
        EXEC [dbo].[CollarDeployment_UpdateRetrievalDate] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11', '2012-05-13'
        SET @T2_msg = 'Failed'
    END TRY
    BEGIN CATCH
        SET @T2_msg = 'Passed'
    END CATCH
    --Report
    SET @T2_msg = @T2_test + @T2_msg
    PRINT @T2_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T2_project, @T2_animal2, @T2_mfgr, @T2_collar1, '2012-05-13'
    EXEC [dbo].[CollarDeployment_Delete] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11'

    -- Test9b
    SET @T2_test = '  Test9b: different animal; same collar; d1date =  t3, r1date = t5, d2date =  t1, r2date t2 -> t4: '
    --Action/Test
    EXEC [dbo].[CollarDeployment_Insert] @T2_project, @T2_animal2, @T2_mfgr, @T2_collar1, '2012-05-13', '2012-05-15'
    EXEC [dbo].[CollarDeployment_Insert] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11', '2012-05-12'
    BEGIN TRY
        EXEC [dbo].[CollarDeployment_UpdateRetrievalDate] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11', '2012-05-14'
        SET @T2_msg = 'Failed'
    END TRY
    BEGIN CATCH
        SET @T2_msg = 'Passed'
    END CATCH
    --Report
    SET @T2_msg = @T2_test + @T2_msg
    PRINT @T2_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T2_project, @T2_animal2, @T2_mfgr, @T2_collar1, '2012-05-13'
    EXEC [dbo].[CollarDeployment_Delete] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11'

    -- Test9c
    SET @T2_test = '  Test9c: different animal; same collar;  d1date =  t3, r1date = t5, d2date =  t1, r2date t2 -> t6: '
    --Action/Test
    EXEC [dbo].[CollarDeployment_Insert] @T2_project, @T2_animal2, @T2_mfgr, @T2_collar1, '2012-05-13', '2012-05-15'
    EXEC [dbo].[CollarDeployment_Insert] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11', '2012-05-12'
    BEGIN TRY
        EXEC [dbo].[CollarDeployment_UpdateRetrievalDate] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11', '2012-05-16'
        SET @T2_msg = 'Failed'
    END TRY
    BEGIN CATCH
        SET @T2_msg = 'Passed'
    END CATCH
    --Report
    SET @T2_msg = @T2_test + @T2_msg
    PRINT @T2_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T2_project, @T2_animal2, @T2_mfgr, @T2_collar1, '2012-05-13'
    EXEC [dbo].[CollarDeployment_Delete] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11'

    -- Test9d
    SET @T2_test = '  Test9d: different animal; same collar; d1date =  t3, r1date = t5, d2date =  t1, r2date t2 -> null: '
    --Action/Test
    EXEC [dbo].[CollarDeployment_Insert] @T2_project, @T2_animal2, @T2_mfgr, @T2_collar1, '2012-05-13', '2012-05-15'
    EXEC [dbo].[CollarDeployment_Insert] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11', '2012-05-12'
    BEGIN TRY
        EXEC [dbo].[CollarDeployment_UpdateRetrievalDate] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11', NULL
        SET @T2_msg = 'Failed'
    END TRY
    BEGIN CATCH
        SET @T2_msg = 'Passed'
    END CATCH
    --Report
    SET @T2_msg = @T2_test + @T2_msg
    PRINT @T2_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T2_project, @T2_animal2, @T2_mfgr, @T2_collar1, '2012-05-13'
    EXEC [dbo].[CollarDeployment_Delete] @T2_project, @T2_animal1, @T2_mfgr, @T2_collar1, '2012-05-11'

    -- Test10
    SET @T2_test = '  Test10: delete deployments: '
    --Test
    IF EXISTS (SELECT 1 from Locations Where [ProjectId] = @T2_project AND [AnimalId] IN (@T2_animal1, @T2_animal2))
        SET @T2_msg = 'Failed'
    ELSE
        SET @T2_msg = 'Passed'
    --Report
    SET @T2_msg = @T2_test + @T2_msg
    PRINT @T2_msg
    --Reset



    -- Clean up
    PRINT '  Cleaning up'
    EXEC [dbo].[CollarFile_Delete] @T2_fileid
    DELETE CollarDeployments Where [ProjectId] = @T2_project 
    EXEC [dbo].[Collar_Delete] @T2_mfgr, @T2_collar1
    EXEC [dbo].[Collar_Delete] @T2_mfgr, @T2_collar2
    Delete Animals where ProjectId = @T2_project -- SP checks if user is in role, which sa is not
    EXEC [dbo].[Project_Delete] @T2_project

    -- Check that cleanup worked
    IF    EXISTS (SELECT 1 from Animals Where [ProjectId] = @T2_project AND [AnimalId] IN (@T2_animal1, @T2_animal2))
       OR EXISTS (SELECT 1 from Projects Where [ProjectId] = @T2_project)
       OR EXISTS (SELECT 1 from Collars Where [CollarManufacturer] = @T2_mfgr AND [CollarId] IN (@T2_collar1,@T2_collar2))
       OR EXISTS (SELECT 1 from CollarDeployments Where [ProjectId] = @T2_project)
       OR EXISTS (SELECT 1 from CollarFiles Where [FileId] = @T2_FileId)
       OR EXISTS (SELECT 1 from CollarDataTelonicsStoreOnBoard Where [FileId] = @T2_FileId)
       OR EXISTS (SELECT 1 from CollarFixes Where [FileId] = @T2_fileid AND [CollarManufacturer] = @T2_mfgr AND [CollarId] = @T2_collar1)
       OR EXISTS (SELECT 1 from Locations Where [ProjectId] = @T2_project AND [AnimalId] IN (@T2_animal1, @T2_animal2))
       OR EXISTS (SELECT 1 from Movements Where [ProjectId] = @T2_project AND [AnimalId] IN (@T2_animal1, @T2_animal2))
    BEGIN
        PRINT 'Test data not completely removed'
        RETURN 1
    END

    -- Remove the sa from project investigator
    DELETE [dbo].[ProjectInvestigators] WHERE  [Login] = @T2_sa








--  ************************************************************
    PRINT 'Test_Trigger_CollarFixes_Delete'
--  ************************************************************
--  Author:	     Regan Sarwas
--  Create date: June 18, 2012
--  Description: Test the CollarFixes DeleteTrigger
--  ============================================================
    
/*  TESTS:
    Delete hidden fix
    Delete Hidding fix
    Delete hidding & hidden fix
  Repeat with multiple fixes

    Check:
    all associated locations are deleted
    hidden fixes - nothing else
    hidding fixes - unhide hidden fixes
    hidden/hidding - hidden fixes inherit hidden by of deleted
*/

    DECLARE
            @T3_sa nvarchar(16) = 'sa',
            @T3_project nvarchar(16) = 'p1__',
            @T3_animal1 nvarchar(16) = 'a1__',
            @T3_mfgr nvarchar(16) = 'Telonics',
            @T3_model nvarchar(16) = 'TelonicsGen4',
            @T3_collar1 nvarchar(16) = 'c1__',
            @T3_file nvarchar(16) = 'f1__',
            @T3_fileid int,
            @T3_format nvarchar(16) = 'A', -- Store on board format
            @T3_test nvarchar(255) = null,
            @T3_msg nvarchar(255) = null,
            @T3_fix1 bigint,
            @T3_fix2 bigint,
            @T3_fix3 bigint,
            @T3_fix4 bigint,
            @T3_fix5 bigint,
            @T3_fix6 bigint,
            @T3_fix_l bigint
            
    -- This test must be run as SA, since others cannot operate on table directly
    -- This test the integrity of the tables underlying the Store Procedures available to users.
    -- Also SA can check user tables

    IF ORIGINAL_LOGIN() <> @T3_sa
    BEGIN
        PRINT '  You must be the sa to run this test'
        RETURN 1
    END

/*
    -- Clean up mess if previous test failed; need to find and set the file id first
    SET @T3_fileid = 203
    EXEC [dbo].[CollarFile_Delete] @T3_fileid
    DELETE CollarDeployments Where [ProjectId] = @T3_project 
    DELETE Collars where CollarManufacturer = @T3_mfgr and CollarId = @T3_collar1
    Delete Animals where ProjectId = @T3_project -- SP checks if user is in role, which sa is not
    EXEC [dbo].[Project_Delete] @T3_project
    return 1
*/

    --Check to make sure we are not going to overwrite any data
    IF EXISTS (SELECT 1 from Projects Where [ProjectId] = @T3_project)
    OR EXISTS (SELECT 1 from Collars Where [CollarManufacturer] = @T3_mfgr AND [CollarId] = @T3_collar1)
    BEGIN
        PRINT '  Aborting tests.  Existing data conflicts with test data.'
        RETURN 1
    END
    
    -- Add the sa is a project investigator
    IF NOT EXISTS(SELECT 1 FROM [dbo].[ProjectInvestigators] WHERE [Login] = @T3_sa)
    BEGIN
        INSERT [dbo].[ProjectInvestigators] ([Login],[Name],[Email],[Phone]) VALUES (@T3_sa,@T3_sa,@T3_sa,@T3_sa)
    END


    -- Setup for test
    EXEC [dbo].[Project_Insert] @T3_project, @T3_project, @T3_sa
    EXEC [dbo].[Animal_Insert] @T3_project, @T3_animal1
    EXEC [dbo].[Collar_Insert] @T3_mfgr, @T3_collar1, @T3_model ,@T3_sa

    INSERT INTO dbo.CollarFiles ([FileName], [Project], [CollarManufacturer], [CollarId], [Format], [Status], [Contents])
         VALUES (@T3_file, @T3_project, @T3_mfgr, @T3_collar1, @T3_format, 'A', convert(varbinary,'data'))  -- 'A' = Store on board
    SET @T3_fileid = SCOPE_IDENTITY();
    INSERT INTO dbo.CollarDataTelonicsStoreOnBoard ([FileId], [LineNumber], [Date], [Latitude], [Longitude], [Fix Status])
         VALUES (@T3_fileid, 1, '2012-05-11', '60.11', '-154.11', 'Fix Available'),
                (@T3_fileid, 2, '2012-05-12', '60.12', '-154.12', 'Fix Available'),
                (@T3_fileid, 3, '2012-05-11', '60.13', '-154.13', 'Fix Available'),
                (@T3_fileid, 4, '2012-05-11', '60.14', '-154.14', 'Fix Available'),
                (@T3_fileid, 5, '2012-05-15', '60.15', '-154.15', 'Fix Available'),
                (@T3_fileid, 6, '2012-05-11', '60.16', '-154.16', 'Fix Available')
    EXEC [dbo].[CollarFixes_Insert] @T3_fileid,@T3_format

    SELECT @T3_fix1 = FixId FROM CollarFixes Where [FileId] = @T3_fileid AND [LineNumber] = 1
    SELECT @T3_fix2 = FixId FROM CollarFixes Where [FileId] = @T3_fileid AND [LineNumber] = 2
    SELECT @T3_fix3 = FixId FROM CollarFixes Where [FileId] = @T3_fileid AND [LineNumber] = 3
    SELECT @T3_fix4 = FixId FROM CollarFixes Where [FileId] = @T3_fileid AND [LineNumber] = 4
    SELECT @T3_fix5 = FixId FROM CollarFixes Where [FileId] = @T3_fileid AND [LineNumber] = 5
    SELECT @T3_fix6 = FixId FROM CollarFixes Where [FileId] = @T3_fileid AND [LineNumber] = 6

    -- Check initial conditions
    IF    NOT EXISTS (SELECT 1 from Animals Where [ProjectId] = @T3_project AND [AnimalId] = @T3_animal1)
       OR NOT EXISTS (SELECT 1 from Collars Where [CollarManufacturer] = @T3_mfgr AND [CollarId] = @T3_collar1)
       OR NOT EXISTS (SELECT 1 from CollarFiles Where [FileId] = @T3_FileId)
       OR NOT EXISTS (SELECT 1 from CollarFixes Where [FileId] = @T3_fileid AND [CollarManufacturer] = @T3_mfgr AND [CollarId] = @T3_collar1)
       OR     EXISTS (SELECT 1 from CollarDeployments Where [ProjectId] = @T3_project)
       OR     EXISTS (SELECT 1 from Locations Where [ProjectId] = @T3_project AND [AnimalId] = @T3_animal1)
       OR     EXISTS (SELECT 1 from Movements Where [ProjectId] = @T3_project AND [AnimalId] = @T3_animal1)
    BEGIN
        PRINT '  Test data not initialized properly'
        RETURN 1
    END
    
    
    -- Check initial Fixes
    IF    NOT EXISTS (SELECT 1 from CollarFixes Where [FixId] = @T3_fix1 AND [HiddenBy] = @T3_fix3)
       OR NOT EXISTS (SELECT 1 from CollarFixes Where [FixId] = @T3_fix2 AND [HiddenBy] IS NULL)
       OR NOT EXISTS (SELECT 1 from CollarFixes Where [FixId] = @T3_fix3 AND [HiddenBy] = @T3_fix4)
       OR NOT EXISTS (SELECT 1 from CollarFixes Where [FixId] = @T3_fix4 AND [HiddenBy] = @T3_fix6)
       OR NOT EXISTS (SELECT 1 from CollarFixes Where [FixId] = @T3_fix5 AND [HiddenBy] IS NULL)
       OR NOT EXISTS (SELECT 1 from CollarFixes Where [FixId] = @T3_fix6 AND [HiddenBy] IS NULL)
    BEGIN
        PRINT '  Fixes not initialized properly'
        RETURN 1
    END

    -- Check initial Locations
    EXEC [dbo].[CollarDeployment_Insert] @T3_project, @T3_animal1, @T3_mfgr, @T3_collar1, '2012-05-10 12:00', '2012-05-16 12:00'
    IF    NOT EXISTS (SELECT 1 from Locations Where [FixId] = @T3_fix2)
       OR NOT EXISTS (SELECT 1 from Locations Where [FixId] = @T3_fix5)
       OR NOT EXISTS (SELECT 1 from Locations Where [FixId] = @T3_fix6)
       OR     EXISTS (SELECT 1 from Locations Where [ProjectId] = @T3_project AND [AnimalId] = @T3_animal1 AND [FixId] NOT IN (@T3_fix2,@T3_fix5,@T3_fix6))
    BEGIN
        PRINT '  Locations not initialized properly'
        RETURN 1
    END

    
    -- Do tests

    -- Test1
    SET @T3_test = '  Test1: Delete Hidden Fix: '
    --Action
    Delete [dbo].[CollarFixes] WHERE [FixId] = @T3_fix1
    --Test
    SET @T3_msg = ''
    IF EXISTS (SELECT 1 from CollarFixes Where [FixId] = @T3_fix1)
        SET @T3_msg = @T3_msg + ' Fix1 not deleted'
    IF NOT EXISTS (SELECT 1 from CollarFixes Where [FixId] = @T3_fix2 AND [HiddenBy] IS NULL)
        SET @T3_msg = @T3_msg + ' Fix2 is hidden'
    IF NOT EXISTS (SELECT 1 from CollarFixes Where [FixId] = @T3_fix3 AND [HiddenBy] = @T3_fix4)
        SET @T3_msg = @T3_msg + ' Fix3 is not hidden by Fix4'
    IF NOT EXISTS (SELECT 1 from CollarFixes Where [FixId] = @T3_fix4 AND [HiddenBy] = @T3_fix6)
        SET @T3_msg = @T3_msg + ' Fix4 is not hidden by Fix6'
    IF NOT EXISTS (SELECT 1 from CollarFixes Where [FixId] = @T3_fix5 AND [HiddenBy] IS NULL)
        SET @T3_msg = @T3_msg + ' Fix5 is hidden'
    IF NOT EXISTS (SELECT 1 from CollarFixes Where [FixId] = @T3_fix6 AND [HiddenBy] IS NULL)
        SET @T3_msg = @T3_msg + ' Fix6 is hidden'
    IF NOT EXISTS (SELECT 1 from Locations Where [FixId] = @T3_fix2)
        SET @T3_msg = @T3_msg + ' No Location for Fix2'
    IF NOT EXISTS (SELECT 1 from Locations Where [FixId] = @T3_fix5)
        SET @T3_msg = @T3_msg + ' No Location for Fix5'
    IF NOT EXISTS (SELECT 1 from Locations Where [FixId] = @T3_fix6)
        SET @T3_msg = @T3_msg + ' No Location for Fix6'
    IF EXISTS (SELECT 1 from Locations Where [ProjectId] = @T3_project AND [AnimalId] = @T3_animal1 AND [FixId] NOT IN (@T3_fix2,@T3_fix5,@T3_fix6))
        SET @T3_msg = @T3_msg + ' Unexpected Locations'
    IF @T3_msg = ''
        SET @T3_msg = 'Passed'
    ELSE
        SET @T3_msg = 'Failed' + @T3_msg
    --Report
    SET @T3_msg = @T3_test + @T3_msg
    PRINT @T3_msg
    --Reset


    -- Test2
    SET @T3_test = '  Test2: Delete Hidden/Hidding Fix: '
    --Action
    Delete [dbo].[CollarFixes] WHERE [FixId] = @T3_fix4
    --Test
    SET @T3_msg = ''
    IF EXISTS (SELECT 1 from CollarFixes Where [FixId] = @T3_fix1)
        SET @T3_msg = @T3_msg + ' Fix1 not deleted'
    IF NOT EXISTS (SELECT 1 from CollarFixes Where [FixId] = @T3_fix2 AND [HiddenBy] IS NULL)
        SET @T3_msg = @T3_msg + ' Fix2 is hidden'
    IF NOT EXISTS (SELECT 1 from CollarFixes Where [FixId] = @T3_fix3 AND [HiddenBy] = @T3_fix6)
        SET @T3_msg = @T3_msg + ' Fix3 is not hidden by Fix 6'
    IF EXISTS (SELECT 1 from CollarFixes Where [FixId] = @T3_fix4)
        SET @T3_msg = @T3_msg + ' Fix4 not deleted'
    IF NOT EXISTS (SELECT 1 from CollarFixes Where [FixId] = @T3_fix5 AND [HiddenBy] IS NULL)
        SET @T3_msg = @T3_msg + ' Fix5 is hidden'
    IF NOT EXISTS (SELECT 1 from CollarFixes Where [FixId] = @T3_fix6 AND [HiddenBy] IS NULL)
        SET @T3_msg = @T3_msg + ' Fix6 is hidden'
    IF NOT EXISTS (SELECT 1 from Locations Where [FixId] = @T3_fix2)
        SET @T3_msg = @T3_msg + ' No Location for Fix2'
    IF NOT EXISTS (SELECT 1 from Locations Where [FixId] = @T3_fix5)
        SET @T3_msg = @T3_msg + ' No Location for Fix5'
    IF NOT EXISTS (SELECT 1 from Locations Where [FixId] = @T3_fix6)
        SET @T3_msg = @T3_msg + ' No Location for Fix6'
    IF EXISTS (SELECT 1 from Locations Where [ProjectId] = @T3_project AND [AnimalId] = @T3_animal1 AND [FixId] NOT IN (@T3_fix2,@T3_fix5,@T3_fix6))
        SET @T3_msg = @T3_msg + ' Unexpected Locations'
    IF @T3_msg = ''
        SET @T3_msg = 'Passed'
    ELSE
        SET @T3_msg = 'Failed' + @T3_msg
    --Report
    SET @T3_msg = @T3_test + @T3_msg
    PRINT @T3_msg
    --Reset


    -- Test3
    SET @T3_test = '  Test3: Delete Hidding Fix: '
    --Action
    Delete [dbo].[CollarFixes] WHERE [FixId] = @T3_fix6
    --Test
    SET @T3_msg = ''
    IF EXISTS (SELECT 1 from CollarFixes Where [FixId] = @T3_fix1)
        SET @T3_msg = @T3_msg + ' Fix1 not deleted'
    IF NOT EXISTS (SELECT 1 from CollarFixes Where [FixId] = @T3_fix2 AND [HiddenBy] IS NULL)
        SET @T3_msg = @T3_msg + ' Fix2 is hidden'
    IF NOT EXISTS (SELECT 1 from CollarFixes Where [FixId] = @T3_fix3 AND [HiddenBy] IS NULL)
        SET @T3_msg = @T3_msg + ' Fix3 is hidden'
    IF EXISTS (SELECT 1 from CollarFixes Where [FixId] = @T3_fix4)
        SET @T3_msg = @T3_msg + ' Fix4 not deleted'
    IF NOT EXISTS (SELECT 1 from CollarFixes Where [FixId] = @T3_fix5 AND [HiddenBy] IS NULL)
        SET @T3_msg = @T3_msg + ' Fix5 is hidden'
    IF EXISTS (SELECT 1 from CollarFixes Where [FixId] = @T3_fix6)
        SET @T3_msg = @T3_msg + ' Fix6 not deleted'
    IF NOT EXISTS (SELECT 1 from Locations Where [FixId] = @T3_fix2)
        SET @T3_msg = @T3_msg + ' No Location for Fix2'
    IF NOT EXISTS (SELECT 1 from Locations Where [FixId] = @T3_fix5)
        SET @T3_msg = @T3_msg + ' No Location for Fix5'
    IF NOT EXISTS (SELECT 1 from Locations Where [FixId] = @T3_fix3)
        SET @T3_msg = @T3_msg + ' No Location for Fix3'
    IF EXISTS (SELECT 1 from Locations Where [ProjectId] = @T3_project AND [AnimalId] = @T3_animal1 AND [FixId] NOT IN (@T3_fix2,@T3_fix5,@T3_fix3))
        SET @T3_msg = @T3_msg + ' Unexpected Locations'
    IF @T3_msg = ''
        SET @T3_msg = 'Passed'
    ELSE
        SET @T3_msg = 'Failed' + @T3_msg
    --Report
    SET @T3_msg = @T3_test + @T3_msg
    PRINT @T3_msg
    --Reset
    
    
    -- Test4
    SET @T3_test = '  Test4: Delete Several Plain Fixes: '
    --Action
    Delete [dbo].[CollarFixes] WHERE [FixId] IN (@T3_fix1, @T3_fix2, @T3_fix3, @T3_fix4, @T3_fix5, @T3_fix6)
    --Test
    SET @T3_msg = ''
    IF EXISTS (SELECT 1 from CollarFixes Where [FixId] IN (@T3_fix1, @T3_fix2, @T3_fix3, @T3_fix4, @T3_fix5, @T3_fix6))
        SET @T3_msg = @T3_msg + ' Fixes not deleted'

    IF EXISTS (SELECT 1 from Locations Where [ProjectId] = @T3_project AND [AnimalId] = @T3_animal1)
        SET @T3_msg = @T3_msg + ' Unexpected Locations'
    IF @T3_msg = ''
        SET @T3_msg = 'Passed'
    ELSE
        SET @T3_msg = 'Failed' + @T3_msg
    --Report
    SET @T3_msg = @T3_test + @T3_msg
    PRINT @T3_msg
    --Reset
    EXEC [dbo].[CollarFile_UpdateStatus] @T3_fileid, 'I'
    EXEC [dbo].[CollarFile_UpdateStatus] @T3_fileid, 'A'
    --EXEC [dbo].[CollarFixes_Insert] @T3_fileid,@T3_format

    SELECT @T3_fix1 = FixId FROM CollarFixes Where [FileId] = @T3_fileid AND [LineNumber] = 1
    SELECT @T3_fix2 = FixId FROM CollarFixes Where [FileId] = @T3_fileid AND [LineNumber] = 2
    SELECT @T3_fix3 = FixId FROM CollarFixes Where [FileId] = @T3_fileid AND [LineNumber] = 3
    SELECT @T3_fix4 = FixId FROM CollarFixes Where [FileId] = @T3_fileid AND [LineNumber] = 4
    SELECT @T3_fix5 = FixId FROM CollarFixes Where [FileId] = @T3_fileid AND [LineNumber] = 5
    SELECT @T3_fix6 = FixId FROM CollarFixes Where [FileId] = @T3_fileid AND [LineNumber] = 6

    -- Test5
    SET @T3_test = '  Test5: Delete Multiple Hidden Fixes: '
    --Action
    Delete [dbo].[CollarFixes] WHERE [FixId] IN (@T3_fix2, @T3_fix3, @T3_fix6)
    --Test
    SET @T3_msg = ''
    IF EXISTS (SELECT 1 from CollarFixes Where [FixId] IN (@T3_fix2, @T3_fix3, @T3_fix6))
        SET @T3_msg = @T3_msg + ' Fixes 2,3,6 not deleted'
    IF NOT EXISTS (SELECT 1 from CollarFixes Where [FixId] = @T3_fix1 AND [HiddenBy] = @T3_fix4)
        SET @T3_msg = @T3_msg + ' Fix1 is not hidden by Fix4'
    IF NOT EXISTS (SELECT 1 from CollarFixes Where [FixId] = @T3_fix4 AND [HiddenBy] IS NULL)
        SET @T3_msg = @T3_msg + ' Fix4 is hidden'
    IF NOT EXISTS (SELECT 1 from CollarFixes Where [FixId] = @T3_fix5 AND [HiddenBy] IS NULL)
        SET @T3_msg = @T3_msg + ' Fix5 is hidden'
    IF NOT EXISTS (SELECT 1 from Locations Where [FixId] = @T3_fix4)
        SET @T3_msg = @T3_msg + ' No Location for Fix4'
    IF NOT EXISTS (SELECT 1 from Locations Where [FixId] = @T3_fix5)
        SET @T3_msg = @T3_msg + ' No Location for Fix5'
    IF EXISTS (SELECT 1 from Locations Where [ProjectId] = @T3_project AND [AnimalId] = @T3_animal1 AND [FixId] NOT IN (@T3_fix4,@T3_fix5))
        SET @T3_msg = @T3_msg + ' Unexpected Locations'
    IF @T3_msg = ''
        SET @T3_msg = 'Passed'
    ELSE
        SET @T3_msg = 'Failed' + @T3_msg
    --Report
    SET @T3_msg = @T3_test + @T3_msg
    PRINT @T3_msg
    --Reset

    
    
    -- Clean up
    PRINT '  Cleaning up'
    EXEC [dbo].[CollarFile_Delete] @T3_fileid
    DELETE CollarDeployments Where [ProjectId] = @T3_project 
    EXEC [dbo].[Collar_Delete] @T3_mfgr, @T3_collar1
    Delete Animals where ProjectId = @T3_project -- SP checks if user is in role, which sa is not
    EXEC [dbo].[Project_Delete] @T3_project

    -- Check that cleanup worked
    IF    EXISTS (SELECT 1 from Animals Where [ProjectId] = @T3_project AND [AnimalId] = @T3_animal1)
       OR EXISTS (SELECT 1 from Projects Where [ProjectId] = @T3_project)
       OR EXISTS (SELECT 1 from Collars Where [CollarManufacturer] = @T3_mfgr AND [CollarId] = @T3_collar1)
       OR EXISTS (SELECT 1 from CollarDeployments Where [ProjectId] = @T3_project)
       OR EXISTS (SELECT 1 from CollarFiles Where [FileId] = @T3_FileId)
       OR EXISTS (SELECT 1 from CollarDataTelonicsStoreOnBoard Where [FileId] = @T3_FileId)
       OR EXISTS (SELECT 1 from CollarFixes Where [FileId] = @T3_fileid AND [CollarManufacturer] = @T3_mfgr AND [CollarId] = @T3_collar1)
       OR EXISTS (SELECT 1 from Locations Where [ProjectId] = @T3_project AND [AnimalId] = @T3_animal1)
       OR EXISTS (SELECT 1 from Movements Where [ProjectId] = @T3_project AND [AnimalId] = @T3_animal1)
    BEGIN
        PRINT 'Test data not completely removed'
        RETURN 1
    END

    -- Remove the sa from project investigator
    DELETE [dbo].[ProjectInvestigators] WHERE  [Login] = @T3_sa








--  ************************************************************
    PRINT 'Test_Trigger_CollarFixes_Insert'
--  ************************************************************
--  Author:	     Regan Sarwas
--  Create date: June 13, 2012
--  Description: Test the CollarFixes InsertTrigger
--  ============================================================
    
    DECLARE
            @T4_sa nvarchar(16) = 'sa',
            @T4_project nvarchar(16) = 'p1__',
            @T4_animal nvarchar(16) = 'a1__',
            @T4_mfgr nvarchar(16) = 'Telonics',
            @T4_model nvarchar(16) = 'TelonicsGen4',
            @T4_collar nvarchar(16) = 'c1__',
            @T4_file nvarchar(16) = 'f1__',
            @T4_format nvarchar(16) = 'A', -- Store on board format
            @T4_test nvarchar(255) = null,
            @T4_msg nvarchar(255) = null,
            @T4_fileid int,
            @T4_date datetime2,
            @T4_fix1 bigint,
            @T4_fix2 bigint
            
    -- This test must be run as SA, since others cannot operate on table directly
    -- This test the integrity of the tables underlying the Store Procedures available to users.
    -- Also SA can check user tables

    IF ORIGINAL_LOGIN() <> @T4_sa
    BEGIN
        PRINT '  You must be the sa to run this test'
        RETURN 1
    END

/*
    -- Clean up mess if previous test failed; need to find and set the file id first
    SET @T4_fileid = 133
    EXEC [dbo].[CollarFile_Delete] @T4_fileid
    DELETE CollarDeployments Where [ProjectId] = @T4_project 
    DELETE Collars where CollarManufacturer = @T4_mfgr and CollarId = @T4_collar
    Delete Animals where ProjectId = @T4_project -- SP checks if user is in role, which sa is not
    EXEC [dbo].[Project_Delete] @T4_project
    return 1
*/

    --Check to make sure we are not going to overwrite any data
    IF EXISTS (SELECT 1 from Projects Where [ProjectId] = @T4_project)
    OR EXISTS (SELECT 1 from Collars Where [CollarManufacturer] = @T4_mfgr AND [CollarId] = @T4_collar)
    BEGIN
        PRINT '  Aborting tests.  Existing data conflicts with test data.'
        RETURN 1
    END
    
    -- Add the sa is a project investigator
    IF NOT EXISTS(SELECT 1 FROM [dbo].[ProjectInvestigators] WHERE [Login] = @T4_sa)
    BEGIN
        INSERT [dbo].[ProjectInvestigators] ([Login],[Name],[Email],[Phone]) VALUES (@T4_sa,@T4_sa,@T4_sa,@T4_sa)
    END


    -- Setup for test
    EXEC [dbo].[Project_Insert] @T4_project, @T4_project, @T4_sa
    EXEC [dbo].[Animal_Insert] @T4_project, @T4_animal
    EXEC [dbo].[Collar_Insert] @T4_mfgr, @T4_collar, @T4_model ,@T4_sa
    -- do not use the [dbo].[CollarFile_Insert] because it calls 
    -- [dbo].[CollarData_Insert] which tries to read the binary contents field to populate fixes
    -- EXEC [dbo].[CollarFile_Insert] @T4_file, @T4_project, @T4_mfgr, @T4_collar,'A','I',NULL,@T4_fileid
    INSERT INTO dbo.CollarFiles ([FileName], [Project], [CollarManufacturer], [CollarId], [Format], [Status], [Contents])
         VALUES (@T4_file, @T4_project, @T4_mfgr, @T4_collar, @T4_format, 'A', convert(varbinary,'data'))  -- 'A' = Store on board
    SET @T4_fileid = SCOPE_IDENTITY();
    -- Load multiple Fixes Manually into the collar data table
    INSERT INTO dbo.CollarDataTelonicsStoreOnBoard ([FileId], [LineNumber], [Date], [Latitude], [Longitude], [Fix Status])
         VALUES (@T4_fileid, 1, '2012-05-11', '60.1', '-154.1', 'Fix Available'),
                (@T4_fileid, 2, '2012-05-12', '60.2', '-154.2', 'Fix Available'),
                (@T4_fileid, 3, '2012-05-13', '60.3', '-154.3', 'Fix Available'),
                (@T4_fileid, 4, '2012-05-14', '60.4', '-154.4', 'Fix Available'),
                (@T4_fileid, 5, '2012-05-15', '60.5', '-154.5', 'Fix Available'),
                (@T4_fileid, 6, '2012-05-16', '60.6', '-154.6', 'Fix Available');

    -- Check initial conditions
    IF    NOT EXISTS (SELECT 1 from Animals Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal) 
       OR NOT EXISTS (SELECT 1 from Collars Where [CollarManufacturer] = @T4_mfgr AND [CollarId] = @T4_collar)
       OR NOT EXISTS (SELECT 1 from CollarFiles Where [FileId] = @T4_FileId)
       OR NOT EXISTS (SELECT 1 from CollarDataTelonicsStoreOnBoard Where [FileId] = @T4_fileid)
       OR     EXISTS (SELECT 1 from CollarFixes Where [FileId] = @T4_fileid AND [CollarManufacturer] = @T4_mfgr AND [CollarId] = @T4_collar)
       OR     EXISTS (SELECT 1 from Locations Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal)
       OR     EXISTS (SELECT 1 from Movements Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal)
    BEGIN
        PRINT '  Test data not initialized properly'
        RETURN 1
    END
    

    -- Do tests
        
    -- Test1
    SET @T4_test = '  Test1: Add Fixes, no deployment: '
    --Action
    EXEC [dbo].[CollarFixes_Insert] @T4_fileid, @T4_format
    --Test
    SET @T4_msg = ''
    SELECT 1 from CollarFixes Where [FileId] = @T4_fileid AND [CollarManufacturer] = @T4_mfgr AND [CollarId] = @T4_collar
    IF @@ROWCOUNT <> 6
        SET @T4_msg = @T4_msg + ' Not 6 fixes'
    SELECT 1 from Locations Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal
    IF @@ROWCOUNT <> 0
        SET @T4_msg = @T4_msg + ' Not 0 locations'
    SELECT 1 from Movements Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal
    IF @@ROWCOUNT <> 0
        SET @T4_msg = @T4_msg + ' Not 0 movements'
    IF @T4_msg = ''
        SET @T4_msg = 'Passed'
    ELSE
        SET @T4_msg = 'Failed' + @T4_msg
    --Report
    SET @T4_msg = @T4_test + @T4_msg
    PRINT @T4_msg
    --Reset
    EXEC [dbo].[CollarFixes_Delete] @T4_fileid
    
    -- Test2
    SET @T4_test = '  Test2: 6 Fixes, Deployment starts after first fix: '
    --Action
    EXEC [dbo].[CollarDeployment_Insert] @T4_project, @T4_animal, @T4_mfgr, @T4_collar, '2012-05-11 12:00'
    EXEC [dbo].[CollarFixes_Insert] @T4_fileid, @T4_format
    --Test
    SET @T4_msg = ''
    SELECT 1 from CollarFixes Where [FileId] = @T4_fileid AND [CollarManufacturer] = @T4_mfgr AND [CollarId] = @T4_collar
    IF @@ROWCOUNT <> 6
        SET @T4_msg = @T4_msg + ' Not 6 fixes'
    SELECT 1 from Locations Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal
    IF @@ROWCOUNT <> 5
        SET @T4_msg = @T4_msg + ' Not 5 locations'
    SELECT 1 from Movements Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal
    IF @@ROWCOUNT <> 4
        SET @T4_msg = @T4_msg + ' Not 4 movements'
    SELECT @T4_date = MIN([FixDate]) FROM Locations Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal
    IF @T4_date <> '2012-05-12'
        SET @T4_msg = @T4_msg + ' First location has wrong date'
    SELECT @T4_date = MAX([FixDate]) FROM Locations Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal
    IF @T4_date <> '2012-05-16'
        SET @T4_msg = @T4_msg + ' Last location has wrong date'
    IF @T4_msg = ''
        SET @T4_msg = 'Passed'
    ELSE
        SET @T4_msg = 'Failed' + @T4_msg
    --Report
    SET @T4_msg = @T4_test + @T4_msg
    PRINT @T4_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T4_project, @T4_animal, @T4_mfgr, @T4_collar, '2012-05-11 12:00'
    EXEC [dbo].[CollarFixes_Delete] @T4_fileid
    
    
    -- Test3
    SET @T4_test = '  Test3: 6 Fixes, Deployment starts after first fix end before last fix: '
    --Action
    EXEC [dbo].[CollarDeployment_Insert] @T4_project, @T4_animal, @T4_mfgr, @T4_collar, '2012-05-11 12:00', '2012-05-15 12:00'
    EXEC [dbo].[CollarFixes_Insert] @T4_fileid, @T4_format
    --Test
    SET @T4_msg = ''
    SELECT 1 from CollarFixes Where [FileId] = @T4_fileid AND [CollarManufacturer] = @T4_mfgr AND [CollarId] = @T4_collar
    IF @@ROWCOUNT <> 6
        SET @T4_msg = @T4_msg + ' Not 6 fixes'
    SELECT 1 from Locations Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal
    IF @@ROWCOUNT <> 4
        SET @T4_msg = @T4_msg + ' Not 4 locations'
    SELECT 1 from Movements Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal
    IF @@ROWCOUNT <> 3
        SET @T4_msg = @T4_msg + ' Not 3 movements'
    SELECT @T4_date = MIN([FixDate]) FROM Locations Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal
    IF @T4_date <> '2012-05-12'
        SET @T4_msg = @T4_msg + ' First location has wrong date'
    SELECT @T4_date = MAX([FixDate]) FROM Locations Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal
    IF @T4_date <> '2012-05-15'
        SET @T4_msg = @T4_msg + ' Last location has wrong date'
    IF @T4_msg = ''
        SET @T4_msg = 'Passed'
    ELSE
        SET @T4_msg = 'Failed' + @T4_msg
    --Report
    SET @T4_msg = @T4_test + @T4_msg
    PRINT @T4_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T4_project, @T4_animal, @T4_mfgr, @T4_collar, '2012-05-11 12:00'
    EXEC [dbo].[CollarFixes_Delete] @T4_fileid
    
    
    -- Test4
    SET @T4_test = '  Test4: 6 Fixes, Deployment after last fix: '
    --Action
    EXEC [dbo].[CollarDeployment_Insert] @T4_project, @T4_animal, @T4_mfgr, @T4_collar, '2012-05-20 12:00', '2012-05-25 12:00'
    EXEC [dbo].[CollarFixes_Insert] @T4_fileid, @T4_format
    --Test
    SET @T4_msg = ''
    SELECT 1 from CollarFixes Where [FileId] = @T4_fileid AND [CollarManufacturer] = @T4_mfgr AND [CollarId] = @T4_collar
    IF @@ROWCOUNT <> 6
        SET @T4_msg = @T4_msg + ' Not 6 fixes'
    SELECT 1 from Locations Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal
    IF @@ROWCOUNT <> 0
        SET @T4_msg = @T4_msg + ' Not 0 locations'
    SELECT 1 from Movements Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal
    IF @@ROWCOUNT <> 0
        SET @T4_msg = @T4_msg + ' Not 0 movements'
    IF @T4_msg = ''
        SET @T4_msg = 'Passed'
    ELSE
        SET @T4_msg = 'Failed' + @T4_msg
    --Report
    SET @T4_msg = @T4_test + @T4_msg
    PRINT @T4_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T4_project, @T4_animal, @T4_mfgr, @T4_collar, '2012-05-20 12:00'
    EXEC [dbo].[CollarFixes_Delete] @T4_fileid
    
    
    -- Test5
    SET @T4_test = '  Test5: 2 Deployment before and after middle fixes: '
    --Action
    EXEC [dbo].[CollarDeployment_Insert] @T4_project, @T4_animal, @T4_mfgr, @T4_collar, '2012-05-05 12:00', '2012-05-12 12:00'
    EXEC [dbo].[CollarDeployment_Insert] @T4_project, @T4_animal, @T4_mfgr, @T4_collar, '2012-05-14 12:00', '2012-05-25 12:00'
    EXEC [dbo].[CollarFixes_Insert] @T4_fileid, @T4_format
    --Test
    SET @T4_msg = ''
    SELECT 1 from CollarFixes Where [FileId] = @T4_fileid AND [CollarManufacturer] = @T4_mfgr AND [CollarId] = @T4_collar
    IF @@ROWCOUNT <> 6
        SET @T4_msg = @T4_msg + ' Not 6 fixes'
    SELECT 1 from Locations Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal
    IF @@ROWCOUNT <> 4
        SET @T4_msg = @T4_msg + ' Not 4 locations'
    SELECT 1 from Movements Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal
    -- TODO, debateable whether there should be 2 or 3 movements.  With 3 the deployments are linked
    IF @@ROWCOUNT <> 3
        SET @T4_msg = @T4_msg + ' Not 3 movements'
    SELECT @T4_date = MIN([FixDate]) FROM Locations Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal
    IF @T4_date <> '2012-05-11'
        SET @T4_msg = @T4_msg + ' First location has wrong date'
    SELECT @T4_date = MAX([FixDate]) FROM Locations Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal
    IF @T4_date <> '2012-05-16'
        SET @T4_msg = @T4_msg + ' Last location has wrong date'
    IF @T4_msg = ''
        SET @T4_msg = 'Passed'
    ELSE
        SET @T4_msg = 'Failed' + @T4_msg
    --Report
    SET @T4_msg = @T4_test + @T4_msg
    PRINT @T4_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T4_project, @T4_animal, @T4_mfgr, @T4_collar, '2012-05-05 12:00'
    EXEC [dbo].[CollarDeployment_Delete] @T4_project, @T4_animal, @T4_mfgr, @T4_collar, '2012-05-14 12:00'
    EXEC [dbo].[CollarFixes_Delete] @T4_fileid
    
    
    -- Test6
    SET @T4_test = '  Test6: Mortality before Deployment ends: '
    --Action
    EXEC [dbo].[Animal_Update] @T4_project, @T4_animal,null,null,'2012-05-15 12:00'
    EXEC [dbo].[CollarDeployment_Insert] @T4_project, @T4_animal, @T4_mfgr, @T4_collar, '2012-05-11 12:00'
    EXEC [dbo].[CollarFixes_Insert] @T4_fileid, @T4_format
    --Test
    SET @T4_msg = ''
    SELECT 1 from CollarFixes Where [FileId] = @T4_fileid AND [CollarManufacturer] = @T4_mfgr AND [CollarId] = @T4_collar
    IF @@ROWCOUNT <> 6
        SET @T4_msg = @T4_msg + ' Not 6 fixes'
    SELECT 1 from Locations Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal
    IF @@ROWCOUNT <> 4
        SET @T4_msg = @T4_msg + ' Not 4 locations'
    SELECT 1 from Movements Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal
    IF @@ROWCOUNT <> 3
        SET @T4_msg = @T4_msg + ' Not 3 movements'
    SELECT @T4_date = MIN([FixDate]) FROM Locations Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal
    IF @T4_date <> '2012-05-12'
        SET @T4_msg = @T4_msg + ' First location has wrong date'
    SELECT @T4_date = MAX([FixDate]) FROM Locations Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal
    IF @T4_date <> '2012-05-15'
        SET @T4_msg = @T4_msg + ' Last location has wrong date'
    IF @T4_msg = ''
        SET @T4_msg = 'Passed'
    ELSE
        SET @T4_msg = 'Failed' + @T4_msg
    --Report
    SET @T4_msg = @T4_test + @T4_msg
    PRINT @T4_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T4_project, @T4_animal, @T4_mfgr, @T4_collar, '2012-05-11 12:00'
    EXEC [dbo].[CollarFixes_Delete] @T4_fileid
    EXEC [dbo].[Animal_Update] @T4_project, @T4_animal,null,null,null
    
    
    -- Test7
    SET @T4_test = '  Test7: Mortality after deployment ends: '
    --Action
    EXEC [dbo].[Animal_Update] @T4_project, @T4_animal,null,null,'2012-05-15 12:00'
    EXEC [dbo].[CollarDeployment_Insert] @T4_project, @T4_animal, @T4_mfgr, @T4_collar, '2012-05-11 12:00', '2012-05-14 12:00'
    EXEC [dbo].[CollarFixes_Insert] @T4_fileid, @T4_format
    --Test
    SET @T4_msg = ''
    SELECT 1 from CollarFixes Where [FileId] = @T4_fileid AND [CollarManufacturer] = @T4_mfgr AND [CollarId] = @T4_collar
    IF @@ROWCOUNT <> 6
        SET @T4_msg = @T4_msg + ' Not 6 fixes'
    SELECT 1 from Locations Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal
    IF @@ROWCOUNT <> 3
        SET @T4_msg = @T4_msg + ' Not 3 locations'
    SELECT 1 from Movements Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal
    IF @@ROWCOUNT <> 2
        SET @T4_msg = @T4_msg + ' Not 2 movements'
    SELECT @T4_date = MIN([FixDate]) FROM Locations Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal
    IF @T4_date <> '2012-05-12'
        SET @T4_msg = @T4_msg + ' First location has wrong date'
    SELECT @T4_date = MAX([FixDate]) FROM Locations Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal
    IF @T4_date <> '2012-05-14'
        SET @T4_msg = @T4_msg + ' Last location has wrong date'
    IF @T4_msg = ''
        SET @T4_msg = 'Passed'
    ELSE
        SET @T4_msg = 'Failed' + @T4_msg
    --Report
    SET @T4_msg = @T4_test + @T4_msg
    PRINT @T4_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T4_project, @T4_animal, @T4_mfgr, @T4_collar, '2012-05-11 12:00'
    EXEC [dbo].[CollarFixes_Delete] @T4_fileid
    EXEC [dbo].[Animal_Update] @T4_project, @T4_animal,null,null,null

    
    -- Test8
    SET @T4_test = '  Test8: Hide first with new fix: '
    --Action
    INSERT INTO dbo.CollarDataTelonicsStoreOnBoard ([FileId], [LineNumber], [Date], [Latitude], [Longitude], [Fix Status])
         VALUES (@T4_fileid, 7, '2012-05-11', '60.7', '-154.7', 'Fix Available')
    EXEC [dbo].[CollarDeployment_Insert] @T4_project, @T4_animal, @T4_mfgr, @T4_collar, '2012-05-10 12:00'
    EXEC [dbo].[CollarFixes_Insert] @T4_fileid, @T4_format
    --Test
    SET @T4_msg = ''
    SELECT 1 from CollarFixes Where [FileId] = @T4_fileid AND [CollarManufacturer] = @T4_mfgr AND [CollarId] = @T4_collar
    IF @@ROWCOUNT <> 7
        SET @T4_msg = @T4_msg + ' Not 7 fixes'
    SELECT 1 from Locations Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal
    IF @@ROWCOUNT <> 6
        SET @T4_msg = @T4_msg + ' Not 6 locations'
    SELECT 1 from Movements Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal
    IF @@ROWCOUNT <> 5
        SET @T4_msg = @T4_msg + ' Not 5 movements'
    SELECT @T4_date = MIN([FixDate]) FROM Locations Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal
    IF @T4_date <> '2012-05-11'
        SET @T4_msg = @T4_msg + ' First location has wrong date'
    SELECT @T4_date = MAX([FixDate]) FROM Locations Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal
    IF @T4_date <> '2012-05-16'
        SET @T4_msg = @T4_msg + ' Last location has wrong date'
    SELECT @T4_fix1 = MIN([FixId]) FROM CollarFixes Where [FileId] = @T4_fileid AND [CollarManufacturer] = @T4_mfgr AND [CollarId] = @T4_collar
    SELECT @T4_fix2 = MAX([FixId]) FROM CollarFixes Where [FileId] = @T4_fileid AND [CollarManufacturer] = @T4_mfgr AND [CollarId] = @T4_collar
    IF NOT EXISTS (SELECT 1 FROM CollarFixes where FixId = @T4_fix1 AND HiddenBy = @T4_fix2)
        SET @T4_msg = @T4_msg + ' last fix is not hiding first fix'
    IF @T4_msg = ''
        SET @T4_msg = 'Passed'
    ELSE
        SET @T4_msg = 'Failed' + @T4_msg
    --Report
    SET @T4_msg = @T4_test + @T4_msg
    PRINT @T4_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T4_project, @T4_animal, @T4_mfgr, @T4_collar, '2012-05-10 12:00'
    EXEC [dbo].[CollarFixes_Delete] @T4_fileid

    
    -- Test9
    SET @T4_test = '  Test9: Hide 1w/2; 2w/4; 3w/5 5w/6: '
    --Action
    Delete from dbo.CollarDataTelonicsStoreOnBoard where FileId = @T4_fileid
    INSERT INTO dbo.CollarDataTelonicsStoreOnBoard ([FileId], [LineNumber], [Date], [Latitude], [Longitude], [Fix Status])
         VALUES (@T4_fileid, 1, '2012-05-11', '60.1', '-154.1', 'Fix Available'),
                (@T4_fileid, 2, '2012-05-11', '60.2', '-154.2', 'Fix Available'),
                (@T4_fileid, 3, '2012-05-13', '60.3', '-154.3', 'Fix Available'),
                (@T4_fileid, 4, '2012-05-11', '60.4', '-154.4', 'Fix Available'),
                (@T4_fileid, 5, '2012-05-13', '60.5', '-154.5', 'Fix Available'),
                (@T4_fileid, 6, '2012-05-13', '60.6', '-154.6', 'Fix Available');
    EXEC [dbo].[CollarDeployment_Insert] @T4_project, @T4_animal, @T4_mfgr, @T4_collar, '2012-05-10 12:00'
    EXEC [dbo].[CollarFixes_Insert] @T4_fileid, @T4_format
    --Test
    SET @T4_msg = ''
    SELECT 1 from CollarFixes Where [FileId] = @T4_fileid AND [CollarManufacturer] = @T4_mfgr AND [CollarId] = @T4_collar
    IF @@ROWCOUNT <> 6
        SET @T4_msg = @T4_msg + ' Not 6 fixes'
    SELECT 1 from Locations Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal
    IF @@ROWCOUNT <> 2
        SET @T4_msg = @T4_msg + ' Not 2 locations'
    SELECT 1 from Movements Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal
    IF @@ROWCOUNT <> 1
        SET @T4_msg = @T4_msg + ' Not 1 movements'
    SELECT @T4_date = MIN([FixDate]) FROM Locations Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal
    IF @T4_date <> '2012-05-11'
        SET @T4_msg = @T4_msg + ' First location has wrong date'
    SELECT @T4_date = MAX([FixDate]) FROM Locations Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal
    IF @T4_date <> '2012-05-13'
        SET @T4_msg = @T4_msg + ' Last location has wrong date'
    
    SELECT @T4_fix1 = MIN([FixId]) FROM CollarFixes Where [FileId] = @T4_fileid AND [CollarManufacturer] = @T4_mfgr AND [CollarId] = @T4_collar
    SET @T4_fix2 = @T4_fix1 + 1
    IF NOT EXISTS (SELECT 1 FROM CollarFixes where FixId = @T4_fix1 AND HiddenBy = @T4_fix2)
        SET @T4_msg = @T4_msg + ' fix 2 not hiding fix 1'
    
    SET @T4_fix1 = @T4_fix1 + 1
    SET @T4_fix2 = @T4_fix1 + 2
    IF NOT EXISTS (SELECT 1 FROM CollarFixes where FixId = @T4_fix1 AND HiddenBy = @T4_fix2)
        SET @T4_msg = @T4_msg + ' fix 4 not hiding fix 2'
    
    SET @T4_fix1 = @T4_fix1 + 1
    SET @T4_fix2 = @T4_fix1 + 2
    IF NOT EXISTS (SELECT 1 FROM CollarFixes where FixId = @T4_fix1 AND HiddenBy = @T4_fix2)
        SET @T4_msg = @T4_msg + ' fix 5 not hiding fix 3'
    
    SET @T4_fix1 = @T4_fix1 + 2
    SET @T4_fix2 = @T4_fix1 + 1
    IF NOT EXISTS (SELECT 1 FROM CollarFixes where FixId = @T4_fix1 AND HiddenBy = @T4_fix2)
        SET @T4_msg = @T4_msg + ' fix 6 not hiding fix 5'
    IF @T4_msg = ''
        SET @T4_msg = 'Passed'
    ELSE
        SET @T4_msg = 'Failed' + @T4_msg
    --Report
    SET @T4_msg = @T4_test + @T4_msg
    PRINT @T4_msg
    --Reset
    EXEC [dbo].[CollarDeployment_Delete] @T4_project, @T4_animal, @T4_mfgr, @T4_collar, '2012-05-10 12:00'
    EXEC [dbo].[CollarFixes_Delete] @T4_fileid
    
    
        
    -- Clean up
    PRINT '  Cleaning up'
    EXEC [dbo].[CollarFile_Delete] @T4_fileid
    DELETE CollarDeployments Where [ProjectId] = @T4_project 
    EXEC [dbo].[Collar_Delete] @T4_mfgr, @T4_collar
    Delete Animals where ProjectId = @T4_project -- SP checks if user is in role, which sa is not
    EXEC [dbo].[Project_Delete] @T4_project

    -- Check that cleanup worked
    IF    EXISTS (SELECT 1 from Animals Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal) 
       OR EXISTS (SELECT 1 from Projects Where [ProjectId] = @T4_project)
       OR EXISTS (SELECT 1 from Collars Where [CollarManufacturer] = @T4_mfgr AND [CollarId] = @T4_collar)
       OR EXISTS (SELECT 1 from CollarDeployments Where [ProjectId] = @T4_project)
       OR EXISTS (SELECT 1 from CollarFiles Where [FileId] = @T4_fileid)
       OR EXISTS (SELECT 1 from CollarDataTelonicsStoreOnBoard Where [FileId] = @T4_fileid)
       OR EXISTS (SELECT 1 from CollarFixes Where [FileId] = @T4_fileid AND [CollarManufacturer] = @T4_mfgr AND [CollarId] = @T4_collar)
       OR EXISTS (SELECT 1 from Locations Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal)
       OR EXISTS (SELECT 1 from Movements Where [ProjectId] = @T4_project AND [AnimalId] = @T4_animal)
    BEGIN
        PRINT 'Test data not completely removed'
        RETURN 1
    END

    -- Remove the sa from project investigator
    DELETE [dbo].[ProjectInvestigators] WHERE  [Login] = @T4_sa
    
END








--  ************************************************************
    PRINT 'Test_Trigger_CollarFixes_Update'
--  ************************************************************
--  Author:	     Regan Sarwas
--  Create date: June 13, 2012
--  Description: Test the CollarFixes UpdateTrigger
--  ============================================================
    
    DECLARE
            @T5_sa nvarchar(16) = 'sa',
            @T5_project nvarchar(16) = 'p1__',
            @T5_animal nvarchar(16) = 'a1__',
            @T5_mfgr nvarchar(16) = 'Telonics',
            @T5_model nvarchar(16) = 'TelonicsGen4',
            @T5_collar nvarchar(16) = 'c1__',
            @T5_file nvarchar(16) = 'f1__',
            @T5_format nvarchar(16) = 'A', -- Store on board format
            @T5_test nvarchar(255) = null,
            @T5_msg nvarchar(255) = null,
            @T5_fileid int,
            @T5_fix1 bigint,
            @T5_fix2 bigint,
            @T5_fix3 bigint,
            @T5_fix4 bigint,
            @T5_fix_l bigint
            
    -- This test must be run as SA, since others cannot operate on table directly
    -- This test the integrity of the tables underlying the Store Procedures available to users.
    -- Also SA can check user tables

    IF ORIGINAL_LOGIN() <> @T5_sa
    BEGIN
        PRINT '  You must be the sa to run this test'
        RETURN 1
    END

/*
    -- Clean up mess if previous test failed; need to find and set the file id first
    SET @T5_fileid = 140
    EXEC [dbo].[CollarFile_Delete] @T5_fileid
    DELETE CollarDeployments Where [ProjectId] = @T5_project 
    DELETE Collars where CollarManufacturer = @T5_mfgr and CollarId = @T5_collar
    Delete Animals where ProjectId = @T5_project -- SP checks if user is in role, which sa is not
    EXEC [dbo].[Project_Delete] @T5_project
    return 1
*/

    --Check to make sure we are not going to overwrite any data
    IF EXISTS (SELECT 1 from Projects Where [ProjectId] = @T5_project)
    OR EXISTS (SELECT 1 from Collars Where [CollarManufacturer] = @T5_mfgr AND [CollarId] = @T5_collar)
    BEGIN
        PRINT '  Aborting tests.  Existing data conflicts with test data.'
        RETURN 1
    END
    
    -- Add the sa is a project investigator
    IF NOT EXISTS(SELECT 1 FROM [dbo].[ProjectInvestigators] WHERE [Login] = @T5_sa)
    BEGIN
        INSERT [dbo].[ProjectInvestigators] ([Login],[Name],[Email],[Phone]) VALUES (@T5_sa,@T5_sa,@T5_sa,@T5_sa)
    END


    -- Setup for test
    EXEC [dbo].[Project_Insert] @T5_project, @T5_project, @T5_sa
    EXEC [dbo].[Animal_Insert] @T5_project, @T5_animal
    EXEC [dbo].[Collar_Insert] @T5_mfgr, @T5_collar, @T5_model ,@T5_sa
    EXEC [dbo].[CollarDeployment_Insert] @T5_project, @T5_animal, @T5_mfgr, @T5_collar, '2012-05-10 12:00'
    -- do not use the [dbo].[CollarFile_Insert] because it calls 
    -- [dbo].[CollarData_Insert] which tries to read the binary contents field to populate fixes
    -- EXEC [dbo].[CollarFile_Insert] @T5_file, @T5_project, @T5_mfgr, @T5_collar,'A','I',NULL,@T5_fileid
    INSERT INTO dbo.CollarFiles ([FileName], [Project], [CollarManufacturer], [CollarId], [Format], [Status], [Contents])
         VALUES (@T5_file, @T5_project, @T5_mfgr, @T5_collar, @T5_format, 'A', convert(varbinary,'data'))  -- 'A' = Store on board
    SET @T5_fileid = SCOPE_IDENTITY();

    -- Check initial conditions
    IF    NOT EXISTS (SELECT 1 from Animals Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal) 
       OR NOT EXISTS (SELECT 1 from Collars Where [CollarManufacturer] = @T5_mfgr AND [CollarId] = @T5_collar)
       OR NOT EXISTS (SELECT 1 from CollarFiles Where [FileId] = @T5_FileId)
       OR NOT EXISTS (SELECT 1 from CollarDeployments Where [ProjectId] = @T5_project)
       OR     EXISTS (SELECT 1 from CollarFixes Where [FileId] = @T5_fileid AND [CollarManufacturer] = @T5_mfgr AND [CollarId] = @T5_collar)
       OR     EXISTS (SELECT 1 from Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal)
       OR     EXISTS (SELECT 1 from Movements Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal)
    BEGIN
        PRINT '  Test data not initialized properly'
        RETURN 1
    END
    

    -- Do tests
    
    -- Test1
    -- Mortality is null and retrieval date is null
    SET @T5_test = '  Test1: (3 hide 2): '
    --Action
    Delete from dbo.CollarDataTelonicsStoreOnBoard where FileId = @T5_fileid
    INSERT INTO dbo.CollarDataTelonicsStoreOnBoard ([FileId], [LineNumber], [Date], [Latitude], [Longitude], [Fix Status])
         VALUES (@T5_fileid, 1, '2012-05-11', '60.1', '-154.1', 'Fix Available'),
                (@T5_fileid, 2, '2012-05-12', '60.2', '-154.2', 'Fix Available'),
                (@T5_fileid, 3, '2012-05-12', '60.3', '-154.3', 'Fix Available')
    EXEC [dbo].[CollarFixes_Insert] @T5_fileid, @T5_format
    
    SELECT @T5_fix1 = FixId FROM CollarFixes Where [FileId] = @T5_fileid AND [LineNumber] = 1
    SELECT @T5_fix2 = FixId FROM CollarFixes Where [FileId] = @T5_fileid AND [LineNumber] = 2
    SELECT @T5_fix3 = FixId FROM CollarFixes Where [FileId] = @T5_fileid AND [LineNumber] = 3

    --Test
    SET @T5_msg = ''
    SELECT 1 from CollarFixes Where [FileId] = @T5_fileid AND [CollarManufacturer] = @T5_mfgr AND [CollarId] = @T5_collar
    IF @@ROWCOUNT <> 3
        SET @T5_msg = @T5_msg + ' Not 3 fixes'
    SELECT 1 from Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @@ROWCOUNT <> 2
        SET @T5_msg = @T5_msg + ' Not 2 locations'
    SELECT 1 from Movements Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @@ROWCOUNT <> 1
        SET @T5_msg = @T5_msg + ' Not 1 movements'
        
    IF NOT EXISTS (SELECT 1 FROM CollarFixes where FixId = @T5_fix2 AND HiddenBy = @T5_fix3)
        SET @T5_msg = @T5_msg + ' fix 2 not hidden by fix 3'
    
    SELECT @T5_fix_l = MIN([FixId]) FROM Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @T5_fix1 <> @T5_fix_l
        SET @T5_msg = @T5_msg + ' First location is based on wrong fix'
        
    SELECT @T5_fix_l = MAX([FixId]) FROM Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @T5_fix3 <> @T5_fix_l
        SET @T5_msg = @T5_msg + ' Last location is based on wrong fix'
    
    IF @T5_msg = ''
        SET @T5_msg = 'Passed'
    ELSE
        SET @T5_msg = 'Failed' + @T5_msg
    --Report
    SET @T5_msg = @T5_test + @T5_msg
    PRINT @T5_msg
    --Reset
    
    
    -- Test1a
    SET @T5_test = '  Test1a: Unhide 2 (2 hide 3): '
    --Action
    EXEC [dbo].[CollarFixes_UpdateUnhideFix] @T5_fix2
    --Test
    SET @T5_msg = ''
    SELECT 1 from CollarFixes Where [FileId] = @T5_fileid AND [CollarManufacturer] = @T5_mfgr AND [CollarId] = @T5_collar
    IF @@ROWCOUNT <> 3
        SET @T5_msg = @T5_msg + ' Not 3 fixes'
    SELECT 1 from Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @@ROWCOUNT <> 2
        SET @T5_msg = @T5_msg + ' Not 2 locations'
    SELECT 1 from Movements Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @@ROWCOUNT <> 1
        SET @T5_msg = @T5_msg + ' Not 1 movements'

    IF NOT EXISTS (SELECT 1 FROM CollarFixes where FixId = @T5_fix3 AND HiddenBy = @T5_fix2)
        SET @T5_msg = @T5_msg + ' fix 3 not hidden by fix 2'
    
    SELECT @T5_fix_l = MIN([FixId]) FROM Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @T5_fix1 <> @T5_fix_l
        SET @T5_msg = @T5_msg + ' First location is based on wrong fix'

    SELECT @T5_fix_l = MAX([FixId]) FROM Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @T5_fix2 <> @T5_fix_l
        SET @T5_msg = @T5_msg + ' Last location is based on wrong fix'
    
    IF @T5_msg = ''
        SET @T5_msg = 'Passed'
    ELSE
        SET @T5_msg = 'Failed' + @T5_msg
    --Report
    SET @T5_msg = @T5_test + @T5_msg
    PRINT @T5_msg
    --Reset
    EXEC [dbo].[CollarFixes_Delete] @T5_fileid
    
    
    
    
    
    
    -- Test2
    -- Mortality is null and retrieval date is null
    SET @T5_test = '  Test2: (4 hide 3 hide 2): '
    --Action
    Delete from dbo.CollarDataTelonicsStoreOnBoard where FileId = @T5_fileid
    INSERT INTO dbo.CollarDataTelonicsStoreOnBoard ([FileId], [LineNumber], [Date], [Latitude], [Longitude], [Fix Status])
         VALUES (@T5_fileid, 1, '2012-05-11', '60.1', '-154.1', 'Fix Available'),
                (@T5_fileid, 2, '2012-05-12', '60.2', '-154.2', 'Fix Available'),
                (@T5_fileid, 3, '2012-05-12', '60.3', '-154.3', 'Fix Available'),
                (@T5_fileid, 4, '2012-05-12', '60.4', '-154.4', 'Fix Available')
    EXEC [dbo].[CollarFixes_Insert] @T5_fileid, @T5_format
    
    SELECT @T5_fix1 = FixId FROM CollarFixes Where [FileId] = @T5_fileid AND [LineNumber] = 1
    SELECT @T5_fix2 = FixId FROM CollarFixes Where [FileId] = @T5_fileid AND [LineNumber] = 2
    SELECT @T5_fix3 = FixId FROM CollarFixes Where [FileId] = @T5_fileid AND [LineNumber] = 3
    SELECT @T5_fix4 = FixId FROM CollarFixes Where [FileId] = @T5_fileid AND [LineNumber] = 4

    --Test
    SET @T5_msg = ''
    SELECT 1 from CollarFixes Where [FileId] = @T5_fileid AND [CollarManufacturer] = @T5_mfgr AND [CollarId] = @T5_collar
    IF @@ROWCOUNT <> 4
        SET @T5_msg = @T5_msg + ' Not 4 fixes'
    SELECT 1 from Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @@ROWCOUNT <> 2
        SET @T5_msg = @T5_msg + ' Not 2 locations'
    SELECT 1 from Movements Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @@ROWCOUNT <> 1
        SET @T5_msg = @T5_msg + ' Not 1 movements'

    IF NOT EXISTS (SELECT 1 FROM CollarFixes where FixId = @T5_fix2 AND HiddenBy = @T5_fix3)
        SET @T5_msg = @T5_msg + ' fix 2 not hidden by fix 3'

    IF NOT EXISTS (SELECT 1 FROM CollarFixes where FixId = @T5_fix3 AND HiddenBy = @T5_fix4)
        SET @T5_msg = @T5_msg + ' fix 3 not hidden by fix 4'
    
    SELECT @T5_fix_l = MIN([FixId]) FROM Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @T5_fix1 <> @T5_fix_l
        SET @T5_msg = @T5_msg + ' First location is based on wrong fix'
        
    SELECT @T5_fix_l = MAX([FixId]) FROM Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @T5_fix4 <> @T5_fix_l
        SET @T5_msg = @T5_msg + ' Last location is based on wrong fix'
    
    IF @T5_msg = ''
        SET @T5_msg = 'Passed'
    ELSE
        SET @T5_msg = 'Failed' + @T5_msg
    --Report
    SET @T5_msg = @T5_test + @T5_msg
    PRINT @T5_msg
    --Reset
    
    
    -- Test2a
    SET @T5_test = '  Test2a: Unhide 2 (2 hide 4 hide 3): '
    --Action
    EXEC [dbo].[CollarFixes_UpdateUnhideFix] @T5_fix2
    --Test
    SET @T5_msg = ''
    SELECT 1 from CollarFixes Where [FileId] = @T5_fileid AND [CollarManufacturer] = @T5_mfgr AND [CollarId] = @T5_collar
    IF @@ROWCOUNT <> 4
        SET @T5_msg = @T5_msg + ' Not 4 fixes'
    SELECT 1 from Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @@ROWCOUNT <> 2
        SET @T5_msg = @T5_msg + ' Not 2 locations'
    SELECT 1 from Movements Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @@ROWCOUNT <> 1
        SET @T5_msg = @T5_msg + ' Not 1 movements'

    IF NOT EXISTS (SELECT 1 FROM CollarFixes where FixId = @T5_fix4 AND HiddenBy = @T5_fix2)
        SET @T5_msg = @T5_msg + ' fix 4 not hidden by fix 2'
    
    IF NOT EXISTS (SELECT 1 FROM CollarFixes where FixId = @T5_fix3 AND HiddenBy = @T5_fix4)
        SET @T5_msg = @T5_msg + ' fix 3 not hidden by fix 4'
    
    SELECT @T5_fix_l = MIN([FixId]) FROM Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @T5_fix1 <> @T5_fix_l
        SET @T5_msg = @T5_msg + ' First location is based on wrong fix'

    SELECT @T5_fix_l = MAX([FixId]) FROM Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @T5_fix2 <> @T5_fix_l
        SET @T5_msg = @T5_msg + ' Last location is based on wrong fix'
    
    IF @T5_msg = ''
        SET @T5_msg = 'Passed'
    ELSE
        SET @T5_msg = 'Failed' + @T5_msg
    --Report
    SET @T5_msg = @T5_test + @T5_msg
    PRINT @T5_msg
    --Reset
    
    
    -- Test2b
    SET @T5_test = '  Test2b: Unhide 4 (4 hide 3 hide 2): '
    --Action
    EXEC [dbo].[CollarFixes_UpdateUnhideFix] @T5_fix4
    --Test
    SET @T5_msg = ''
    SELECT 1 from CollarFixes Where [FileId] = @T5_fileid AND [CollarManufacturer] = @T5_mfgr AND [CollarId] = @T5_collar
    IF @@ROWCOUNT <> 4
        SET @T5_msg = @T5_msg + ' Not 4 fixes'
    SELECT 1 from Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @@ROWCOUNT <> 2
        SET @T5_msg = @T5_msg + ' Not 2 locations'
    SELECT 1 from Movements Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @@ROWCOUNT <> 1
        SET @T5_msg = @T5_msg + ' Not 1 movements'

    IF NOT EXISTS (SELECT 1 FROM CollarFixes where FixId = @T5_fix2 AND HiddenBy = @T5_fix3)
        SET @T5_msg = @T5_msg + ' fix 2 not hidden by fix 3'

    IF NOT EXISTS (SELECT 1 FROM CollarFixes where FixId = @T5_fix3 AND HiddenBy = @T5_fix4)
        SET @T5_msg = @T5_msg + ' fix 3 not hidden by fix 4'
        
    SELECT @T5_fix_l = MIN([FixId]) FROM Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @T5_fix1 <> @T5_fix_l
        SET @T5_msg = @T5_msg + ' First location is based on wrong fix'

    SELECT @T5_fix_l = MAX([FixId]) FROM Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @T5_fix4 <> @T5_fix_l
        SET @T5_msg = @T5_msg + ' Last location is based on wrong fix'
    
    IF @T5_msg = ''
        SET @T5_msg = 'Passed'
    ELSE
        SET @T5_msg = 'Failed' + @T5_msg
    --Report
    SET @T5_msg = @T5_test + @T5_msg
    PRINT @T5_msg
    --Reset
    EXEC [dbo].[CollarFixes_Delete] @T5_fileid
    
    
    
    
    
    
    -- Test3
    -- Mortality is not null (after hidden)  and retrieval date is null
    SET @T5_test = '  Test3: Mort after hidden (4 hide 3 hide 2): '
    --Action
    EXEC [dbo].[Animal_Update] @T5_project, @T5_animal, null, null, '2012-05-12 12:00'
    Delete from dbo.CollarDataTelonicsStoreOnBoard where FileId = @T5_fileid
    INSERT INTO dbo.CollarDataTelonicsStoreOnBoard ([FileId], [LineNumber], [Date], [Latitude], [Longitude], [Fix Status])
         VALUES (@T5_fileid, 1, '2012-05-11', '60.1', '-154.1', 'Fix Available'),
                (@T5_fileid, 2, '2012-05-12', '60.2', '-154.2', 'Fix Available'),
                (@T5_fileid, 3, '2012-05-12', '60.3', '-154.3', 'Fix Available'),
                (@T5_fileid, 4, '2012-05-12', '60.4', '-154.4', 'Fix Available'),
                (@T5_fileid, 5, '2012-05-13', '60.5', '-154.5', 'Fix Available')
    EXEC [dbo].[CollarFixes_Insert] @T5_fileid, @T5_format
    
    SELECT @T5_fix1 = FixId FROM CollarFixes Where [FileId] = @T5_fileid AND [LineNumber] = 1
    SELECT @T5_fix2 = FixId FROM CollarFixes Where [FileId] = @T5_fileid AND [LineNumber] = 2
    SELECT @T5_fix3 = FixId FROM CollarFixes Where [FileId] = @T5_fileid AND [LineNumber] = 3
    SELECT @T5_fix4 = FixId FROM CollarFixes Where [FileId] = @T5_fileid AND [LineNumber] = 4

    --Test
    SET @T5_msg = ''
    SELECT 1 from CollarFixes Where [FileId] = @T5_fileid AND [CollarManufacturer] = @T5_mfgr AND [CollarId] = @T5_collar
    IF @@ROWCOUNT <> 5
        SET @T5_msg = @T5_msg + ' Not 5 fixes'
    SELECT 1 from Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @@ROWCOUNT <> 2
        SET @T5_msg = @T5_msg + ' Not 2 locations'
    SELECT 1 from Movements Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @@ROWCOUNT <> 1
        SET @T5_msg = @T5_msg + ' Not 1 movements'

    IF NOT EXISTS (SELECT 1 FROM CollarFixes where FixId = @T5_fix2 AND HiddenBy = @T5_fix3)
        SET @T5_msg = @T5_msg + ' fix 2 not hidden by fix 3'

    IF NOT EXISTS (SELECT 1 FROM CollarFixes where FixId = @T5_fix3 AND HiddenBy = @T5_fix4)
        SET @T5_msg = @T5_msg + ' fix 3 not hidden by fix 4'

    SELECT @T5_fix_l = MIN([FixId]) FROM Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @T5_fix1 <> @T5_fix_l
        SET @T5_msg = @T5_msg + ' First location is based on wrong fix'
        
    SELECT @T5_fix_l = MAX([FixId]) FROM Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @T5_fix4 <> @T5_fix_l
        SET @T5_msg = @T5_msg + ' Last location is based on wrong fix'
    
    IF @T5_msg = ''
        SET @T5_msg = 'Passed'
    ELSE
        SET @T5_msg = 'Failed' + @T5_msg
    --Report
    SET @T5_msg = @T5_test + @T5_msg
    PRINT @T5_msg
    --Reset
    
    
    -- Test3a
    SET @T5_test = '  Test3a: Mort after hidden; Unhide 2 (2 hide 4 hide 3): '
    --Action
    EXEC [dbo].[CollarFixes_UpdateUnhideFix] @T5_fix2
    --Test
    SET @T5_msg = ''
    SELECT 1 from CollarFixes Where [FileId] = @T5_fileid AND [CollarManufacturer] = @T5_mfgr AND [CollarId] = @T5_collar
    IF @@ROWCOUNT <> 5
        SET @T5_msg = @T5_msg + ' Not 5 fixes'
    SELECT 1 from Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @@ROWCOUNT <> 2
        SET @T5_msg = @T5_msg + ' Not 2 locations'
    SELECT 1 from Movements Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @@ROWCOUNT <> 1
        SET @T5_msg = @T5_msg + ' Not 1 movements'

    IF NOT EXISTS (SELECT 1 FROM CollarFixes where FixId = @T5_fix4 AND HiddenBy = @T5_fix2)
        SET @T5_msg = @T5_msg + ' fix 4 not hidden by fix 2'
    
    IF NOT EXISTS (SELECT 1 FROM CollarFixes where FixId = @T5_fix3 AND HiddenBy = @T5_fix4)
        SET @T5_msg = @T5_msg + ' fix 3 not hidden by fix 4'
    
    SELECT @T5_fix_l = MIN([FixId]) FROM Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @T5_fix1 <> @T5_fix_l
        SET @T5_msg = @T5_msg + ' First location is based on wrong fix'

    SELECT @T5_fix_l = MAX([FixId]) FROM Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @T5_fix2 <> @T5_fix_l
        SET @T5_msg = @T5_msg + ' Last location is based on wrong fix'
    
    IF @T5_msg = ''
        SET @T5_msg = 'Passed'
    ELSE
        SET @T5_msg = 'Failed' + @T5_msg
    --Report
    SET @T5_msg = @T5_test + @T5_msg
    PRINT @T5_msg
    --Reset
    EXEC [dbo].[CollarFixes_Delete] @T5_fileid






    -- Test4
    -- Mortality is not null and retrieval date is null
    SET @T5_test = '  Test4: Mort before hidden (4 hide 3 hide 2): '
    --Action
    EXEC [dbo].[Animal_Update] @T5_project, @T5_animal, null, null, '2012-05-11 12:00'
    Delete from dbo.CollarDataTelonicsStoreOnBoard where FileId = @T5_fileid
    INSERT INTO dbo.CollarDataTelonicsStoreOnBoard ([FileId], [LineNumber], [Date], [Latitude], [Longitude], [Fix Status])
         VALUES (@T5_fileid, 1, '2012-05-11', '60.1', '-154.1', 'Fix Available'),
                (@T5_fileid, 2, '2012-05-12', '60.2', '-154.2', 'Fix Available'),
                (@T5_fileid, 3, '2012-05-12', '60.3', '-154.3', 'Fix Available'),
                (@T5_fileid, 4, '2012-05-12', '60.4', '-154.4', 'Fix Available'),
                (@T5_fileid, 5, '2012-05-13', '60.5', '-154.5', 'Fix Available')
    EXEC [dbo].[CollarFixes_Insert] @T5_fileid, @T5_format
    
    SELECT @T5_fix1 = FixId FROM CollarFixes Where [FileId] = @T5_fileid AND [LineNumber] = 1
    SELECT @T5_fix2 = FixId FROM CollarFixes Where [FileId] = @T5_fileid AND [LineNumber] = 2
    SELECT @T5_fix3 = FixId FROM CollarFixes Where [FileId] = @T5_fileid AND [LineNumber] = 3
    SELECT @T5_fix4 = FixId FROM CollarFixes Where [FileId] = @T5_fileid AND [LineNumber] = 4

    --Test
    SET @T5_msg = ''
    SELECT 1 from CollarFixes Where [FileId] = @T5_fileid AND [CollarManufacturer] = @T5_mfgr AND [CollarId] = @T5_collar
    IF @@ROWCOUNT <> 5
        SET @T5_msg = @T5_msg + ' Not 5 fixes'
    SELECT 1 from Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @@ROWCOUNT <> 1
        SET @T5_msg = @T5_msg + ' Not 1 locations'
    SELECT 1 from Movements Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @@ROWCOUNT <> 0
        SET @T5_msg = @T5_msg + ' Not 0 movements'

    IF NOT EXISTS (SELECT 1 FROM CollarFixes where FixId = @T5_fix2 AND HiddenBy = @T5_fix3)
        SET @T5_msg = @T5_msg + ' fix 2 not hidden by fix 3'

    IF NOT EXISTS (SELECT 1 FROM CollarFixes where FixId = @T5_fix3 AND HiddenBy = @T5_fix4)
        SET @T5_msg = @T5_msg + ' fix 3 not hidden by fix 4'
    
    SELECT @T5_fix_l = MIN([FixId]) FROM Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @T5_fix1 <> @T5_fix_l
        SET @T5_msg = @T5_msg + ' First location is based on wrong fix'
        
    SELECT @T5_fix_l = MAX([FixId]) FROM Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @T5_fix1 <> @T5_fix_l
        SET @T5_msg = @T5_msg + ' Last location is based on wrong fix'
    
    IF @T5_msg = ''
        SET @T5_msg = 'Passed'
    ELSE
        SET @T5_msg = 'Failed' + @T5_msg
    --Report
    SET @T5_msg = @T5_test + @T5_msg
    PRINT @T5_msg
    --Reset
    
    
    -- Test4a
    SET @T5_test = '  Test4a: Mort before hidden; Unhide 2 (2 hide 4 hide 3): '
    --Action
    EXEC [dbo].[CollarFixes_UpdateUnhideFix] @T5_fix2
    --Test
    SET @T5_msg = ''
    SELECT 1 from CollarFixes Where [FileId] = @T5_fileid AND [CollarManufacturer] = @T5_mfgr AND [CollarId] = @T5_collar
    IF @@ROWCOUNT <> 5
        SET @T5_msg = @T5_msg + ' Not 5 fixes'
    SELECT 1 from Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @@ROWCOUNT <> 1
        SET @T5_msg = @T5_msg + ' Not 1 locations'
    SELECT 1 from Movements Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @@ROWCOUNT <> 0
        SET @T5_msg = @T5_msg + ' Not 0 movements'

    IF NOT EXISTS (SELECT 1 FROM CollarFixes where FixId = @T5_fix4 AND HiddenBy = @T5_fix2)
        SET @T5_msg = @T5_msg + ' fix 4 not hidden by fix 2'
    
    IF NOT EXISTS (SELECT 1 FROM CollarFixes where FixId = @T5_fix3 AND HiddenBy = @T5_fix4)
        SET @T5_msg = @T5_msg + ' fix 3 not hidden by fix 4'
    
    SELECT @T5_fix_l = MIN([FixId]) FROM Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @T5_fix1 <> @T5_fix_l
        SET @T5_msg = @T5_msg + ' First location is based on wrong fix'

    SELECT @T5_fix_l = MAX([FixId]) FROM Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @T5_fix1 <> @T5_fix_l
        SET @T5_msg = @T5_msg + ' Last location is based on wrong fix'
    
    IF @T5_msg = ''
        SET @T5_msg = 'Passed'
    ELSE
        SET @T5_msg = 'Failed' + @T5_msg
    --Report
    SET @T5_msg = @T5_test + @T5_msg
    PRINT @T5_msg
    --Reset
    EXEC [dbo].[CollarFixes_Delete] @T5_fileid
    EXEC [dbo].[Animal_Update] @T5_project, @T5_animal, null, null, null
    
    
    
    
    
    
    -- Test5
    -- Mortality is null and retrieval date is not null (after hidden)
    SET @T5_test = '  Test5: Retrieve after hidden (4 hide 3 hide 2): '
    --Action
    EXEC [dbo].[CollarDeployment_UpdateRetrievalDate] @T5_project, @T5_animal, @T5_mfgr, @T5_collar, '2012-05-10 12:00', '2012-05-12 12:00'
    Delete from dbo.CollarDataTelonicsStoreOnBoard where FileId = @T5_fileid
    INSERT INTO dbo.CollarDataTelonicsStoreOnBoard ([FileId], [LineNumber], [Date], [Latitude], [Longitude], [Fix Status])
         VALUES (@T5_fileid, 1, '2012-05-11', '60.1', '-154.1', 'Fix Available'),
                (@T5_fileid, 2, '2012-05-12', '60.2', '-154.2', 'Fix Available'),
                (@T5_fileid, 3, '2012-05-12', '60.3', '-154.3', 'Fix Available'),
                (@T5_fileid, 4, '2012-05-12', '60.4', '-154.4', 'Fix Available'),
                (@T5_fileid, 5, '2012-05-13', '60.5', '-154.5', 'Fix Available')
    EXEC [dbo].[CollarFixes_Insert] @T5_fileid, @T5_format
    
    SELECT @T5_fix1 = FixId FROM CollarFixes Where [FileId] = @T5_fileid AND [LineNumber] = 1
    SELECT @T5_fix2 = FixId FROM CollarFixes Where [FileId] = @T5_fileid AND [LineNumber] = 2
    SELECT @T5_fix3 = FixId FROM CollarFixes Where [FileId] = @T5_fileid AND [LineNumber] = 3
    SELECT @T5_fix4 = FixId FROM CollarFixes Where [FileId] = @T5_fileid AND [LineNumber] = 4

    --Test
    SET @T5_msg = ''
    SELECT 1 from CollarFixes Where [FileId] = @T5_fileid AND [CollarManufacturer] = @T5_mfgr AND [CollarId] = @T5_collar
    IF @@ROWCOUNT <> 5
        SET @T5_msg = @T5_msg + ' Not 5 fixes'
    SELECT 1 from Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @@ROWCOUNT <> 2
        SET @T5_msg = @T5_msg + ' Not 2 locations'
    SELECT 1 from Movements Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @@ROWCOUNT <> 1
        SET @T5_msg = @T5_msg + ' Not 1 movements'

    IF NOT EXISTS (SELECT 1 FROM CollarFixes where FixId = @T5_fix2 AND HiddenBy = @T5_fix3)
        SET @T5_msg = @T5_msg + ' fix 2 not hidden by fix 3'

    IF NOT EXISTS (SELECT 1 FROM CollarFixes where FixId = @T5_fix3 AND HiddenBy = @T5_fix4)
        SET @T5_msg = @T5_msg + ' fix 3 not hidden by fix 4'

    SELECT @T5_fix_l = MIN([FixId]) FROM Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @T5_fix1 <> @T5_fix_l
        SET @T5_msg = @T5_msg + ' First location is based on wrong fix'
        
    SELECT @T5_fix_l = MAX([FixId]) FROM Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @T5_fix4 <> @T5_fix_l
        SET @T5_msg = @T5_msg + ' Last location is based on wrong fix'
    
    IF @T5_msg = ''
        SET @T5_msg = 'Passed'
    ELSE
        SET @T5_msg = 'Failed' + @T5_msg
    --Report
    SET @T5_msg = @T5_test + @T5_msg
    PRINT @T5_msg
    --Reset
    
    
    -- Test5a
    SET @T5_test = '  Test5a: Retrieve after hidden; Unhide 2 (2 hide 4 hide 3): '
    --Action
    EXEC [dbo].[CollarFixes_UpdateUnhideFix] @T5_fix2
    --Test
    SET @T5_msg = ''
    SELECT 1 from CollarFixes Where [FileId] = @T5_fileid AND [CollarManufacturer] = @T5_mfgr AND [CollarId] = @T5_collar
    IF @@ROWCOUNT <> 5
        SET @T5_msg = @T5_msg + ' Not 5 fixes'
    SELECT 1 from Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @@ROWCOUNT <> 2
        SET @T5_msg = @T5_msg + ' Not 2 locations'
    SELECT 1 from Movements Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @@ROWCOUNT <> 1
        SET @T5_msg = @T5_msg + ' Not 1 movements'

    IF NOT EXISTS (SELECT 1 FROM CollarFixes where FixId = @T5_fix4 AND HiddenBy = @T5_fix2)
        SET @T5_msg = @T5_msg + ' fix 4 not hidden by fix 2'
    
    IF NOT EXISTS (SELECT 1 FROM CollarFixes where FixId = @T5_fix3 AND HiddenBy = @T5_fix4)
        SET @T5_msg = @T5_msg + ' fix 3 not hidden by fix 4'
    
    SELECT @T5_fix_l = MIN([FixId]) FROM Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @T5_fix1 <> @T5_fix_l
        SET @T5_msg = @T5_msg + ' First location is based on wrong fix'

    SELECT @T5_fix_l = MAX([FixId]) FROM Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @T5_fix2 <> @T5_fix_l
        SET @T5_msg = @T5_msg + ' Last location is based on wrong fix'
    
    IF @T5_msg = ''
        SET @T5_msg = 'Passed'
    ELSE
        SET @T5_msg = 'Failed' + @T5_msg
    --Report
    SET @T5_msg = @T5_test + @T5_msg
    PRINT @T5_msg
    --Reset
    EXEC [dbo].[CollarFixes_Delete] @T5_fileid






    -- Test6
    -- Mortality is null and retrieval date is not null (before hidden)
    SET @T5_test = '  Test6: Retrieve before hidden (4 hide 3 hide 2): '
    --Action
    EXEC [dbo].[CollarDeployment_UpdateRetrievalDate] @T5_project, @T5_animal, @T5_mfgr, @T5_collar, '2012-05-10 12:00', '2012-05-11 12:00'
    Delete from dbo.CollarDataTelonicsStoreOnBoard where FileId = @T5_fileid
    INSERT INTO dbo.CollarDataTelonicsStoreOnBoard ([FileId], [LineNumber], [Date], [Latitude], [Longitude], [Fix Status])
         VALUES (@T5_fileid, 1, '2012-05-11', '60.1', '-154.1', 'Fix Available'),
                (@T5_fileid, 2, '2012-05-12', '60.2', '-154.2', 'Fix Available'),
                (@T5_fileid, 3, '2012-05-12', '60.3', '-154.3', 'Fix Available'),
                (@T5_fileid, 4, '2012-05-12', '60.4', '-154.4', 'Fix Available'),
                (@T5_fileid, 5, '2012-05-13', '60.5', '-154.5', 'Fix Available')
    EXEC [dbo].[CollarFixes_Insert] @T5_fileid, @T5_format
    
    SELECT @T5_fix1 = FixId FROM CollarFixes Where [FileId] = @T5_fileid AND [LineNumber] = 1
    SELECT @T5_fix2 = FixId FROM CollarFixes Where [FileId] = @T5_fileid AND [LineNumber] = 2
    SELECT @T5_fix3 = FixId FROM CollarFixes Where [FileId] = @T5_fileid AND [LineNumber] = 3
    SELECT @T5_fix4 = FixId FROM CollarFixes Where [FileId] = @T5_fileid AND [LineNumber] = 4

    --Test
    SET @T5_msg = ''
    SELECT 1 from CollarFixes Where [FileId] = @T5_fileid AND [CollarManufacturer] = @T5_mfgr AND [CollarId] = @T5_collar
    IF @@ROWCOUNT <> 5
        SET @T5_msg = @T5_msg + ' Not 5 fixes'
    SELECT 1 from Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @@ROWCOUNT <> 1
        SET @T5_msg = @T5_msg + ' Not 1 locations'
    SELECT 1 from Movements Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @@ROWCOUNT <> 0
        SET @T5_msg = @T5_msg + ' Not 0 movements'

    IF NOT EXISTS (SELECT 1 FROM CollarFixes where FixId = @T5_fix2 AND HiddenBy = @T5_fix3)
        SET @T5_msg = @T5_msg + ' fix 2 not hidden by fix 3'

    IF NOT EXISTS (SELECT 1 FROM CollarFixes where FixId = @T5_fix3 AND HiddenBy = @T5_fix4)
        SET @T5_msg = @T5_msg + ' fix 3 not hidden by fix 4'
    
    SELECT @T5_fix_l = MIN([FixId]) FROM Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @T5_fix1 <> @T5_fix_l
        SET @T5_msg = @T5_msg + ' First location is based on wrong fix'
        
    SELECT @T5_fix_l = MAX([FixId]) FROM Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @T5_fix1 <> @T5_fix_l
        SET @T5_msg = @T5_msg + ' Last location is based on wrong fix'
    
    IF @T5_msg = ''
        SET @T5_msg = 'Passed'
    ELSE
        SET @T5_msg = 'Failed' + @T5_msg
    --Report
    SET @T5_msg = @T5_test + @T5_msg
    PRINT @T5_msg
    --Reset
    
    
    -- Test6a
    SET @T5_test = '  Test6a: Retrieve before hidden; Unhide 2 (2 hide 4 hide 3): '
    --Action
    EXEC [dbo].[CollarFixes_UpdateUnhideFix] @T5_fix2
    --Test
    SET @T5_msg = ''
    SELECT 1 from CollarFixes Where [FileId] = @T5_fileid AND [CollarManufacturer] = @T5_mfgr AND [CollarId] = @T5_collar
    IF @@ROWCOUNT <> 5
        SET @T5_msg = @T5_msg + ' Not 5 fixes'
    SELECT 1 from Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @@ROWCOUNT <> 1
        SET @T5_msg = @T5_msg + ' Not 1 locations'
    SELECT 1 from Movements Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @@ROWCOUNT <> 0
        SET @T5_msg = @T5_msg + ' Not 0 movements'

    IF NOT EXISTS (SELECT 1 FROM CollarFixes where FixId = @T5_fix4 AND HiddenBy = @T5_fix2)
        SET @T5_msg = @T5_msg + ' fix 4 not hidden by fix 2'
    
    IF NOT EXISTS (SELECT 1 FROM CollarFixes where FixId = @T5_fix3 AND HiddenBy = @T5_fix4)
        SET @T5_msg = @T5_msg + ' fix 3 not hidden by fix 4'
    
    SELECT @T5_fix_l = MIN([FixId]) FROM Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @T5_fix1 <> @T5_fix_l
        SET @T5_msg = @T5_msg + ' First location is based on wrong fix'

    SELECT @T5_fix_l = MAX([FixId]) FROM Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal
    IF @T5_fix1 <> @T5_fix_l
        SET @T5_msg = @T5_msg + ' Last location is based on wrong fix'
    
    IF @T5_msg = ''
        SET @T5_msg = 'Passed'
    ELSE
        SET @T5_msg = 'Failed' + @T5_msg
    --Report
    SET @T5_msg = @T5_test + @T5_msg
    PRINT @T5_msg
    --Reset
    EXEC [dbo].[CollarFixes_Delete] @T5_fileid
    EXEC [dbo].[CollarDeployment_UpdateRetrievalDate] @T5_project, @T5_animal, @T5_mfgr, @T5_collar, '2012-05-10 12:00', null



        
    -- Clean up
    PRINT '  Cleaning up'
    EXEC [dbo].[CollarFile_Delete] @T5_fileid
    DELETE CollarDeployments Where [ProjectId] = @T5_project 
    EXEC [dbo].[Collar_Delete] @T5_mfgr, @T5_collar
    Delete Animals where ProjectId = @T5_project -- SP checks if user is in role, which sa is not
    EXEC [dbo].[Project_Delete] @T5_project

    -- Check that cleanup worked
    IF    EXISTS (SELECT 1 from Animals Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal) 
       OR EXISTS (SELECT 1 from Projects Where [ProjectId] = @T5_project)
       OR EXISTS (SELECT 1 from Collars Where [CollarManufacturer] = @T5_mfgr AND [CollarId] = @T5_collar)
       OR EXISTS (SELECT 1 from CollarDeployments Where [ProjectId] = @T5_project)
       OR EXISTS (SELECT 1 from CollarFiles Where [FileId] = @T5_fileid)
       OR EXISTS (SELECT 1 from CollarDataTelonicsStoreOnBoard Where [FileId] = @T5_fileid)
       OR EXISTS (SELECT 1 from CollarFixes Where [FileId] = @T5_fileid AND [CollarManufacturer] = @T5_mfgr AND [CollarId] = @T5_collar)
       OR EXISTS (SELECT 1 from Locations Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal)
       OR EXISTS (SELECT 1 from Movements Where [ProjectId] = @T5_project AND [AnimalId] = @T5_animal)
    BEGIN
        PRINT 'Test data not completely removed'
        RETURN 1
    END

    -- Remove the sa from project investigator
    DELETE [dbo].[ProjectInvestigators] WHERE  [Login] = @T5_sa








--  ************************************************************
    PRINT 'Test_Trigger_Collars_DisposalDateUpdate'
--  ************************************************************
--  Author:	     Regan Sarwas
--  Create date: Feb 26, 2013
--  Description: Test the update trigger on the Collars table
--  ============================================================

    PRINT '  Test not written yet'








--  ************************************************************
    PRINT 'Test_Trigger_Locations_Delete'
--  ************************************************************
--  Author:	     Regan Sarwas
--  Create date: June 11, 2012
--  Description: Test the Locations DeleteTrigger
--  ============================================================

    PRINT '  Test not written yet'








--  ************************************************************
    PRINT 'Test_Trigger_Locations_Insert'
--  ************************************************************
--  Author:	     Regan Sarwas
--  Create date: June 15, 2012
--  Description: Test the Locations InsertTrigger
--  ============================================================

    PRINT '  Test not written yet (some testing in CollarFixes_Insert)'








--  ************************************************************
    PRINT 'Test_Trigger_Locations_Update'
--  ************************************************************
--  Author:      Regan Sarwas
--  Create date: June 11, 2012
--  Description: Test the Locations UpdateTrigger
--  ============================================================

    PRINT '  Test not written yet'
    







--  ************************************************************
    PRINT 'Test_Trigger_ProjectEditor_Delete'
--  ************************************************************
--  Author:      Regan Sarwas
--  Create date: June 11, 2012
--  Description: Test the ProjectEditor DeleteTrigger
--  ============================================================

    DECLARE
            @T6_sa nvarchar(16) = 'sa',
            @T6_p1 nvarchar(16) = 'p1',
            @T6_p2 nvarchar(16) = 'p2',
            @T6_e1 sysname  = 'NPS\JLPiercy',
            @T6_e2 sysname  = 'NPS\JJCusick',
            @T6_msg nvarchar(255) = null
            
    -- This test must be run as SA, since others cannot operate on table directly
    -- This test the integrity of the tables underlying the Store Procedures available to users.
    -- Also SA can check user tables

    IF ORIGINAL_LOGIN() <> @T6_sa
    BEGIN
        PRINT '  You must be the sa to run this test'
        RETURN 1
    END

    -- Add the sa is a project investigator
    IF NOT EXISTS(SELECT 1 FROM [dbo].[ProjectInvestigators] WHERE [Login] = @T6_sa)
    BEGIN
        INSERT [dbo].[ProjectInvestigators] ([Login],[Name],[Email],[Phone]) VALUES (@T6_sa,@T6_sa,@T6_sa,@T6_sa)
    END

    -- Make sure test projects/editors do not exist
    DELETE Projects Where ProjectId in (@T6_p1, @T6_p2)
    DELETE ProjectEditors Where Editor in (@T6_e1, @T6_e2)
    IF EXISTS (SELECT 1 from sys.database_principals WHERE type = 'U' AND name = @T6_e1)
        EXEC ('DROP USER ['  + @T6_e1 + ']')
    IF EXISTS (SELECT 1 from sys.database_principals WHERE type = 'U' AND name = @T6_e2)
        EXEC ('DROP USER ['  + @T6_e2 + ']')

    -- Create clean test projects
    INSERT Projects (ProjectId, ProjectName, ProjectInvestigator) VALUES (@T6_p1, @T6_p1, @T6_sa)
    INSERT Projects (ProjectId, ProjectName, ProjectInvestigator) VALUES (@T6_p2, @T6_p2, @T6_sa)

    -- Add Project Editors
    EXEC [dbo].[ProjectEditor_Insert] @T6_p1, @T6_e1
    EXEC [dbo].[ProjectEditor_Insert] @T6_p2, @T6_e1
    EXEC [dbo].[ProjectEditor_Insert] @T6_p2, @T6_e2


    -- Check that the editors exist and are in the editor role
    SELECT 1 from sys.database_role_members as M 
             INNER JOIN sys.database_principals AS U  
             ON M.member_principal_id = U.principal_id
             INNER JOIN sys.database_principals AS R 
             ON M.role_principal_id = R.principal_id 
             WHERE R.name = 'Editor' AND U.name in (@T6_e1, @T6_e2)
    IF (@@ROWCOUNT <> 2)
    BEGIN
        PRINT '  Unable to initialize test'
        RETURN 1
    END
    

    -- Do tests
    PRINT '  Start with p1,e1 ; p2,e1 ; p2,e2'
    
    -- Test1
    --Action
    DELETE ProjectEditors where ProjectId = @T6_p1 AND Editor = @T6_e1
    --Test
    SET @T6_msg = 'Failed (e1 or e2 is not in editor role)'
    SELECT 1 from sys.database_role_members as M 
             INNER JOIN sys.database_principals AS U  
             ON M.member_principal_id = U.principal_id
             INNER JOIN sys.database_principals AS R 
             ON M.role_principal_id = R.principal_id 
             WHERE R.name = 'Editor' AND U.name in (@T6_e1, @T6_e2)
    IF (@@ROWCOUNT = 2)
        SET @T6_msg = 'Passed'
    --Report
    SET @T6_msg = '  Test1: delete p1,e1: ' + @T6_msg
    PRINT @T6_msg
    --Reset
    EXEC [dbo].[ProjectEditor_Insert] @T6_p1, @T6_e1

    -- Test2
    --Action
    DELETE ProjectEditors where ProjectId = @T6_p2
    --Test
    SET @T6_msg = 'Failed (e2 is a user, or e1 is not in editor role)'
    IF NOT EXISTS (SELECT 1 from sys.database_principals WHERE type = 'U' AND name = @T6_e2)
       AND EXISTS (SELECT 1 from sys.database_role_members as M 
             INNER JOIN sys.database_principals AS U  
             ON M.member_principal_id = U.principal_id
             INNER JOIN sys.database_principals AS R 
             ON M.role_principal_id = R.principal_id 
             WHERE R.name = 'Editor' AND U.name = @T6_e1)
        SET @T6_msg = 'Passed'
    --Report
    SET @T6_msg = '  Test2: delete p2, *: ' + @T6_msg
    PRINT @T6_msg
    --Reset
    EXEC [dbo].[ProjectEditor_Insert] @T6_p2, @T6_e1
    EXEC [dbo].[ProjectEditor_Insert] @T6_p2, @T6_e2
    
    -- Test3
    --Action
    DELETE ProjectEditors where (ProjectId = @T6_p1 AND Editor = @T6_e1) OR (ProjectId = @T6_p2 AND Editor = @T6_e2)
    --Test
    SET @T6_msg = 'Failed (e2 is a user, or e1 is not in editor role)'
    IF NOT EXISTS (SELECT 1 from sys.database_principals WHERE type = 'U' AND name = @T6_e2)
       AND EXISTS (SELECT 1 from sys.database_role_members as M 
             INNER JOIN sys.database_principals AS U  
             ON M.member_principal_id = U.principal_id
             INNER JOIN sys.database_principals AS R 
             ON M.role_principal_id = R.principal_id 
             WHERE R.name = 'Editor' AND U.name = @T6_e1)
        SET @T6_msg = 'Passed'
    --Report
    SET @T6_msg = '  Test3: delete p1,e1 and p2,e2: ' + @T6_msg
    PRINT @T6_msg
    --Reset
    EXEC [dbo].[ProjectEditor_Insert] @T6_p1, @T6_e1
    EXEC [dbo].[ProjectEditor_Insert] @T6_p2, @T6_e2
    
    -- Test4
    --Action
    DELETE ProjectEditors where Editor = @T6_e1
    --Test
    SET @T6_msg = 'Failed (e1 is a user, or e2 is not in editor role)'
    IF NOT EXISTS (SELECT 1 from sys.database_principals WHERE type = 'U' AND name = @T6_e1)
       AND EXISTS (SELECT 1 from sys.database_role_members as M 
             INNER JOIN sys.database_principals AS U  
             ON M.member_principal_id = U.principal_id
             INNER JOIN sys.database_principals AS R 
             ON M.role_principal_id = R.principal_id 
             WHERE R.name = 'Editor' AND U.name = @T6_e2)
        SET @T6_msg = 'Passed'
    --Report
    SET @T6_msg = '  Test4: delete *, e1: ' + @T6_msg
    PRINT @T6_msg
    --Reset
    EXEC [dbo].[ProjectEditor_Insert] @T6_p1, @T6_e1
    EXEC [dbo].[ProjectEditor_Insert] @T6_p2, @T6_e1
    
    
    
    -- Clean up
    PRINT '  Cleaning up'
    DELETE Projects Where ProjectId in (@T6_p1, @T6_p2)
    DELETE ProjectEditors Where Editor in (@T6_e1, @T6_e2)
    IF EXISTS (SELECT 1 from sys.database_principals WHERE type = 'U' AND name = @T6_e1)
        EXEC ('DROP USER ['  + @T6_e1 + ']')
    IF EXISTS (SELECT 1 from sys.database_principals WHERE type = 'U' AND name = @T6_e2)
        EXEC ('DROP USER ['  + @T6_e2 + ']')

    -- Remove the sa from project investigator
    DELETE [dbo].[ProjectInvestigators] WHERE  [Login] = @T6_sa
    