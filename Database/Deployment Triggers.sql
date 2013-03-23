USE [Animal_Movement]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:      Regan Sarwas
-- Create date: March 22, 2013
-- Description: Processes only the transmission for a single platform in a file.
--              This is a helper (sub) procedure, and is not available to
--              be called directly by user code
-- =============================================
CREATE PROCEDURE [dbo].[ArgosFile_ProcessPlatform] 
	@FileId INT,
	@PlatformId VARCHAR(255)
AS
BEGIN
	SET NOCOUNT ON;

END



GO


-- =============================================
-- Author:      Regan Sarwas
-- Create date: March 22, 2013
-- Description: Un-processes the transmissions for a single platform in a file.
--              This is a helper (sub) procedure, and is not available to
--              be called directly by user code
-- =============================================
CREATE PROCEDURE [dbo].[ArgosFile_UnProcessPlatform] 
	@FileId INT,
	@PlatformId VARCHAR(255)
AS
BEGIN
	SET NOCOUNT ON;

    -- Find result files derived from @FileId and @PlatformId

    -- FAIL - depends on ArgosDeployments which has been changed. prompting the need to unprocess
     SELECT F.FileId
       FROM CollarFiles AS F
 INNER JOIN CollarFiles AS P
         ON P.FileId = F.ParentFileId
 INNER JOIN ArgosDeployments AS D
         ON D.CollarId = F.CollarId
      WHERE D.PlatformId = @PlatformId
        AND P.[Format] IN ('B', 'E', 'F')

         -- FAIL - finds all results for a source file with the @Platform (also requires date range).
    DECLARE @StartDate datetime2(7) = '2010-01-01'
    DECLARE @EndDate datetime2(7) = '2010-01-01'

     SELECT F.FileId, F.ParentFileId, F.Format, A.FirstTransmission, A.LastTransmission
       FROM CollarFiles AS F
 INNER JOIN ArgosFilePlatformDates AS A
         ON A.FileId = F.ParentFileId AND A.PlatformId = @PlatformId
      WHERE dbo.DoDateRangesOverlap(A.FirstTransmission, A.LastTransmission, @StartDate, @EndDate) = 1

END


GO





-- =============================================
-- Author:		Regan Sarwas
-- Create date: March 21, 2013
-- Description: Enforce the business rules for deleting ArgosDeployment
-- =============================================
CREATE TRIGGER [dbo].[AfterArgosDeploymentDelete] 
   ON  [dbo].[ArgosDeployments] 
   AFTER DELETE
AS 
BEGIN
    SET NOCOUNT ON;
    -- Partialy unprocess the transmissions which overlap the deleted deployment
    /*
      In this example a1 is an argos platform with 3 deployments on two collars (c1, c2).
      Processing a source file (with transmissions in each deployment range) will
      result in 3 files (for 2 collars) for a1, and other files for other platforms.
      
      a1,c1  d1 \
      ..     ..  - f1c1
      a1,c1  d2 /
      a1,c2  d3 \
      ..     ..  - f2c2
      a1,c2  d4 /
      a1,c1  d5 \
      ..     ..  - f3c1
      a1,c1  d6 /
      a2        \
      ..         - many additional files
      an        /
      
      Deleting one deployment should delete only one result file.
      However once a result file is created, it is not trivial to tie it back to a deployment
      except by collar, which as we can see in the example will still delete two files.
      While it is possible to have a single source file with multiple deployments (through
      concatenating Argos emails) it is unlikely since there must be a refurbishment between
      deployments.  So in almost all cases, there will only be one deployment for each Argos
      platform in a source file.  Unprocessing by (FileId,PlatformId), and not
      (FileId, PlatformId, CollarId) or (FileId, PlatformId, CollarId, StartDate, EndDate)
      should be simpler.  However, if we unprocess all results for a (FileId,PlatformId), then
      we must reprocess the (FileId,PlatformId) after the deployment has been deleted to
      replace the files that should not have been deleted.
     */  
    -- May be part of a batch delete (unlikely, and only by SA as all others must use SP)
    -- Triggers always execute in the context of a transaction, so the following code is all or nothing.

    DECLARE @FileId int;
    DECLARE @PlatformId varchar(8);
    DECLARE delete_deployment_cursor CURSOR FOR
          -- select all files (by platform) with transmissions that overlap all deleted deployments
             SELECT A.FileId, A.PlatformId
               FROM deleted AS d
         INNER JOIN ArgosFilePlatformDates AS A
                 ON A.PlatformId = d.PlatformId
              WHERE dbo.DoDateRangesOverlap(A.FirstTransmission, A.LastTransmission, d.StartDate, d.EndDate) = 1
        
    OPEN delete_deployment_cursor;

    FETCH NEXT FROM delete_deployment_cursor INTO @FileId, @PlatformId;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        EXEC dbo.ArgosFile_UnProcessPlatform @FileId, @PlatformId
        EXEC dbo.ArgosFile_ProcessPlatform @FileId, @PlatformId
        FETCH NEXT FROM delete_deployment_cursor INTO @FileId, @PlatformId;
    END
    CLOSE delete_deployment_cursor;
    DEALLOCATE delete_deployment_cursor;
END

GO





-- =============================================
-- Author:		Regan Sarwas
-- Create date: March 21, 2013
-- Description: Validates the business rules for a new ArgosDeployment
-- =============================================
CREATE TRIGGER [dbo].[AfterArgosDeploymentInsert] 
   ON  [dbo].[ArgosDeployments] 
   AFTER INSERT
AS 
BEGIN
    SET NOCOUNT ON;
    
    -- Validate Business Rules
    -- 1. Ensure StartDate is before the EndDate
    IF EXISTS (SELECT 1
                 FROM inserted AS I
                WHERE I.EndDate <= I.StartDate  -- false if either is NULL
              )
    BEGIN
        RAISERROR('The end of the deployment must occur after the start of the deployment.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END

    -- 2a. Ensure both Dates are before the DisposalDate of the Collar
    IF EXISTS (SELECT 1
                 FROM inserted AS I
           INNER JOIN Collars AS C
                   ON I.CollarManufacturer = C.CollarManufacturer AND I.CollarId = C.CollarId
                WHERE C.DisposalDate < I.EndDate
                   OR (I.EndDate IS NULL AND C.DisposalDate IS NOT NULL)
              )
    BEGIN
        RAISERROR('The deployment must end before the collar is disposed.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END

    -- 2b. Ensure both Dates are before the DisposalDate of the ArgosPlatform
    IF EXISTS (SELECT 1
                 FROM inserted AS I
           INNER JOIN ArgosPlatforms AS P
                   ON P.PlatformId = I.PlatformId
                WHERE P.DisposalDate < I.EndDate
                   OR (I.EndDate IS NULL AND P.DisposalDate IS NOT NULL)
              )
    BEGIN
        RAISERROR('The deployment must end before the Argos platfrom is disposed.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END

    -- 3. Ensure an ArgosPlatform is not deployed on multiple Collars at the same time
    -- We are checking each inserted deployment against all existing deployments, and all other new deployments	 
    IF EXISTS (SELECT 1
                 FROM inserted AS I1
            LEFT JOIN inserted AS I2
                   ON I1.PlatformId = I2.PlatformId AND I1.DeploymentId <> I2.DeploymentId
           INNER JOIN dbo.ArgosDeployments AS D
                   ON D.PlatformId = I1.PlatformId AND D.DeploymentId <> I1.DeploymentId
                WHERE dbo.DoDateRangesOverlap(D.StartDate, D.EndDate, I1.StartDate, I1.EndDate) = 1
                   OR (I2.DeploymentId IS NOT NULL AND
                       dbo.DoDateRangesOverlap(I1.StartDate, I1.EndDate, I2.StartDate, I2.EndDate) = 1)
              )
    BEGIN
        RAISERROR('Argos platforms cannot have overlapping deployment dates.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END
    
    -- 4. Ensure a Collar is not carrying multiple ArgosPlatforms at the same time
    -- We are checking each inserted deployment against all existing deployments, and all other new deployments 
    IF EXISTS (SELECT 1
                 FROM inserted AS I1
            LEFT JOIN inserted AS I2
                   ON I1.CollarManufacturer = I2.CollarManufacturer AND I1.CollarId = I2.CollarId AND I1.DeploymentId <> I2.DeploymentId
           INNER JOIN dbo.ArgosDeployments AS D
                   ON D.CollarManufacturer = I1.CollarManufacturer AND D.CollarId = I1.CollarId AND D.DeploymentId <> I1.DeploymentId
                WHERE dbo.DoDateRangesOverlap(D.StartDate, D.EndDate, I1.StartDate, I1.EndDate) = 1
                   OR (I2.DeploymentId IS NOT NULL AND
                       dbo.DoDateRangesOverlap(I1.StartDate, I1.EndDate, I2.StartDate, I2.EndDate) = 1)
              )
    BEGIN
        RAISERROR('Collars cannot have overlapping deployment dates.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END
        
    -- Partialy process all files that might benefit from this deployment
    
    -- If a processed file had no issues with this platform, then do not reprocess.
    --    There are no transmissions in that file by this platform were covered with an existing deployment
    -- If a processed file has issues for this platform, then reprocess the (file,platform).
    --    There are unprocessed transmissions in that file for this platform hopefully this deployment will cover them
    -- If a file is unprocessed for some reason, then it is not in our perview to decide that it should
    --   be processed, so we will skip it, even if it has transmissions for this platform. (The file should get
    --   processed at some point in the future by the user or a scheduled task at which point the new deployment
    --   will be considered.)
    
    -- May be part of a batch insert (unlikely, and only by SA as all others must use SP)
    -- Triggers always execute in the context of a transaction, so the following code is all or nothing.

    DECLARE @FileId int;
    DECLARE @PlatformId varchar(8);
    DECLARE insert_deployment_cursor CURSOR FOR 
          -- select all files (by platform) with processing issues for this platform
             SELECT A.FileId, A.PlatformId
               FROM inserted AS I
         INNER JOIN ArgosFilePlatformDates AS D
                 ON D.PlatformId = I.PlatformId
         INNER JOIN ArgosFileProcessingIssues AS A
                 ON A.PlatformId = D.PlatformId AND A.FileId = D.FileId
              WHERE dbo.DoDateRangesOverlap(D.FirstTransmission, D.LastTransmission, I.StartDate, I.EndDate) = 1
        
    OPEN insert_deployment_cursor;

    FETCH NEXT FROM insert_deployment_cursor INTO @FileId, @PlatformId;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        EXEC dbo.ArgosFile_ProcessPlatform @FileId, @PlatformId
        FETCH NEXT FROM insert_deployment_cursor INTO @FileId, @PlatformId;
    END
    CLOSE insert_deployment_cursor;
    DEALLOCATE insert_deployment_cursor;
END

GO





-- =============================================
-- Author:		Regan Sarwas
-- Create date: March 21, 2013
-- Description: Validates the business rules for an Updated ArgosDeployment
-- =============================================
CREATE TRIGGER [dbo].[AfterArgosDeploymentUpdate] 
   ON  [dbo].[ArgosDeployments] 
   AFTER UPDATE
AS 
BEGIN
    SET NOCOUNT ON;
    
    -- Validate Business Rules
    -- 1. Ensure StartDate is before the EndDate
    IF EXISTS (SELECT 1
                 FROM inserted AS I
                WHERE I.EndDate <= I.StartDate  -- false if either is NULL
              )
    BEGIN
        RAISERROR('The end of the deployment must occur after the start of the deployment.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END

    -- 2a. Ensure both Dates are before the DisposalDate of the Collar
    IF EXISTS (SELECT 1
                 FROM inserted AS I
           INNER JOIN Collars AS C
                   ON I.CollarManufacturer = C.CollarManufacturer AND I.CollarId = C.CollarId
                WHERE C.DisposalDate < I.EndDate
                   OR (I.EndDate IS NULL AND C.DisposalDate IS NOT NULL)
              )
    BEGIN
        RAISERROR('The deployment must end before the collar is disposed.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END

    -- 2b. Ensure both Dates are before the DisposalDate of the ArgosPlatform
    IF EXISTS (SELECT 1
                 FROM inserted AS I
           INNER JOIN ArgosPlatforms AS P
                   ON P.PlatformId = I.PlatformId
                WHERE P.DisposalDate < I.EndDate
                   OR (I.EndDate IS NULL AND P.DisposalDate IS NOT NULL)
              )
    BEGIN
        RAISERROR('The deployment must end before the Argos platfrom is disposed.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END

    -- 3. Ensure an ArgosPlatform is not deployed on multiple Collars at the same time
    -- We are checking each inserted deployment against all existing deployments, and all other new deployments	 
    IF EXISTS (SELECT 1
                 FROM inserted AS I1
            LEFT JOIN inserted AS I2
                   ON I1.PlatformId = I2.PlatformId AND I1.DeploymentId <> I2.DeploymentId
           INNER JOIN dbo.ArgosDeployments AS D
                   ON D.PlatformId = I1.PlatformId AND D.DeploymentId <> I1.DeploymentId
                WHERE dbo.DoDateRangesOverlap(D.StartDate, D.EndDate, I1.StartDate, I1.EndDate) = 1
                   OR (I2.DeploymentId IS NOT NULL AND
                       dbo.DoDateRangesOverlap(I1.StartDate, I1.EndDate, I2.StartDate, I2.EndDate) = 1)
              )
    BEGIN
        RAISERROR('Argos platforms cannot have overlapping deployment dates.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END
    
    -- 4. Ensure a Collar is not carrying multiple ArgosPlatforms at the same time
    -- We are checking each inserted deployment against all existing deployments, and all other new deployments 
    IF EXISTS (SELECT 1
                 FROM inserted AS I1
            LEFT JOIN inserted AS I2
                   ON I1.CollarManufacturer = I2.CollarManufacturer AND I1.CollarId = I2.CollarId AND I1.DeploymentId <> I2.DeploymentId
           INNER JOIN dbo.ArgosDeployments AS D
                   ON D.CollarManufacturer = I1.CollarManufacturer AND D.CollarId = I1.CollarId AND D.DeploymentId <> I1.DeploymentId
                WHERE dbo.DoDateRangesOverlap(D.StartDate, D.EndDate, I1.StartDate, I1.EndDate) = 1
                   OR (I2.DeploymentId IS NOT NULL AND
                       dbo.DoDateRangesOverlap(I1.StartDate, I1.EndDate, I2.StartDate, I2.EndDate) = 1)
              )
    BEGIN
        RAISERROR('Collars cannot have overlapping deployment dates.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END

    /*
    An update can be thought of as a delete and an insert, and the logic in the
    delete and insert triggers is basically replicated here when the platform in
    the deployment changes.
    However this is very ineffcient in the typical situation:
      A deployment is created with StartDate = NULL and EndDate = NULL. The deployment
      is update with a non-null EndDate, so that a new deployment can be created.
    Using a brute force method, all existing processing for the updated collar would
    be deleted then recreated.
    Instead, I will be can check for these case:
    If only the dates have changed (no collar or platform change):
      Start or End date has shrunk:
        Flag any files with transmission outside the new date range as
        a processing issue (The same as would happen if the deployment had the non-null dates
        to start with.)  No additional processing needs to happen until a new deployment is created.
      Start or End date has grown:
        Find files with processing issues for this platform, and with transmissions in the new part
        of the date range, then reprocess them, because the expanded dates will add transmissions. 
    Else if only the Collar has changed (no date or platform change)
      I though I might find the result files for the old collar, deactive them, change the collar,
      then reactivate them, but there may be results files for other deployments for the same old
      collar which should not be changed. Since we cannot trivially determine the deployment (or
      even the platform) for a results file, we cannot select just the collar files to change.
      It might be possible to do something more clever, but my head already hurts, so I will
      simply handle it safely as follows.
    Else if the platform changed, or thre were multiple items changing,
      use the brute force delete/insert method. 
    */

    -- May be part of a batch update (happens with a cascading update or when the SA bypasses the SP)
    -- Triggers always execute in the context of a transaction, so the following code is all or nothing.
    -- to keep the logic manageble, I will create a cursor to handle each deployment separately.
    
    DECLARE @FileId int;
    DECLARE @PlatformId varchar(255);
    DECLARE @OldPlatform varchar(255), @NewPlatform varchar(255);
    DECLARE @OldMfgr varchar(255), @NewMfgr varchar(255);
    DECLARE @OldCollar varchar(255), @NewCollar varchar(255);
    DECLARE @OldStart datetime2(7), @NewStart datetime2(7);
    DECLARE @OldEnd datetime2(7), @NewEnd datetime2(7);
    DECLARE update_deployment_cursor CURSOR FOR 
          -- select all files (by platform) with processing issues for this platform
             SELECT D.PlatformId, I.PlatformId, D.CollarManufacturer, I.CollarManufacturer,
                    D.CollarId, I.CollarId, D.StartDate, I.StartDate, D.EndDate, I.EndDate
               FROM inserted AS I
               JOIN deleted AS D
                 ON I.DeploymentId = D.DeploymentId
        
    OPEN update_deployment_cursor;

    FETCH NEXT FROM update_deployment_cursor INTO @OldPlatform,@NewPlatform,@OldMfgr,@NewMfgr,@OldCollar,@NewCollar,@OldStart,@NewStart,@OldEnd,@NewEnd

    WHILE @@FETCH_STATUS = 0
    BEGIN
        IF (@OldPlatform = @NewPlatform AND @OldMfgr = @NewMfgr AND @OldCollar = @NewCollar)
        BEGIN
            -- End Shrunk
            IF ((@NewEnd IS NOT NULL AND @OldEnd IS NULL) OR @NewEnd < @OldEnd)
            BEGIN
                -- Create a processing issue for any files with transmission outside the new date range
-- FIXME - I should check that this file is processed, i.e. has results or issues
                -- All files with PlatformDates should be processed, and creating an issue will flag it for processing 
                INSERT INTO [dbo].[ArgosFileProcessingIssues]
                            ([FileId], [Issue], [PlatformId])
                     SELECT FileId, 'No Collar for Argos Platform from ' + 
                            CONVERT(varchar(20), @newEnd, 20) + ' to ' + CONVERT(varchar(20), LastTransmission, 20), PlatformId
                       FROM ArgosFilePlatformDates
                      WHERE @newEnd < LastTransmission
            END
            -- Start Shrunk
            IF ((@NewStart IS NOT NULL AND @OldStart IS NULL) OR @OldStart < @NewStart)
            BEGIN
                -- Create a processing issue for any files with transmission outside the new date range
-- FIXME - I should check that this file is processed, i.e. has results or issues
                INSERT INTO [dbo].[ArgosFileProcessingIssues]
                            ([FileId], [Issue], [PlatformId])
                     SELECT FileId, 'No Collar for Argos Platform from ' + 
                            CONVERT(varchar(20), FirstTransmission, 20) + ' to ' + CONVERT(varchar(20), @newStart, 20), PlatformId
                       FROM ArgosFilePlatformDates
                      WHERE FirstTransmission < @newStart
            END
            -- End Grew
            IF ((@OldEnd IS NOT NULL AND @NewEnd IS NULL) OR @OldEnd < @NewEnd)
            BEGIN
                -- Process files with processing issues for this platform, and with transmissions in the new part of the date range
                DECLARE end_grew_update_deployment_cursor CURSOR FOR 
                      -- select all files (by platform) with processing issues for this platform
                         SELECT P.FileId, P.PlatformId
                           FROM ArgosFileProcessingIssues AS P
                     INNER JOIN ArgosFilePlatformDates AS D
                             ON P.PlatformId = D.PlatformId AND P.FileId = D.FileId
                          WHERE P.PlatformId = @OldPlatform
                            AND dbo.DoDateRangesOverlap(D.FirstTransmission, D.LastTransmission, @OldEnd, @NewEnd) = 1
                    
                OPEN end_grew_update_deployment_cursor;

                FETCH NEXT FROM end_grew_update_deployment_cursor INTO @FileId, @PlatformId;

                WHILE @@FETCH_STATUS = 0
                BEGIN
                    EXEC dbo.ArgosFile_ProcessPlatform @FileId, @PlatformId
                    FETCH NEXT FROM end_grew_update_deployment_cursor INTO @FileId, @PlatformId;
                END
                CLOSE end_grew_update_deployment_cursor;
                DEALLOCATE end_grew_update_deployment_cursor;
            END
            -- Start Grew
            IF ((@OldStart IS NOT NULL AND @NewStart IS NULL) OR @NewStart < @OldStart)
            BEGIN
                -- Process files with processing issues for this platform, and with transmissions in the new part of the date range
                DECLARE start_grew_update_deployment_cursor CURSOR FOR 
                      -- select all files (by platform) with processing issues for this platform
                         SELECT P.FileId, P.PlatformId
                           FROM ArgosFileProcessingIssues AS P
                     INNER JOIN ArgosFilePlatformDates AS D
                             ON P.PlatformId = D.PlatformId AND P.FileId = D.FileId
                          WHERE P.PlatformId = @OldPlatform
                            AND dbo.DoDateRangesOverlap(D.FirstTransmission, D.LastTransmission, @NewStart, @OldStart) = 1
                    
                OPEN start_grew_update_deployment_cursor;

                FETCH NEXT FROM start_grew_update_deployment_cursor INTO @FileId, @PlatformId;

                WHILE @@FETCH_STATUS = 0
                BEGIN
                    EXEC dbo.ArgosFile_ProcessPlatform @FileId, @PlatformId
                    FETCH NEXT FROM start_grew_update_deployment_cursor INTO @FileId, @PlatformId;
                END
                CLOSE start_grew_update_deployment_cursor;
                DEALLOCATE start_grew_update_deployment_cursor;
            END
        END  
        ELSE
        BEGIN
            IF (@OldPlatform = @NewPlatform)
            BEGIN
                -- only the collar has changed; find all the collar files and update them
                DECLARE update_collar_deployment_cursor CURSOR FOR 
                      -- find the result files from the old collar and the platform
                      
--FIXME without logic for the limiting to the platform, we will select too many files
                      
                         SELECT F.FileId
                           FROM CollarFiles AS F
                           JOIN CollarFiles AS P
                             ON F.ParentFileId = P.FileId
                          WHERE P.Format IN ('B', 'E', 'F')
                            AND F.CollarManufacturer = @OldMfgr AND F.CollarId = @OldCollar
                    
                OPEN update_collar_deployment_cursor;

                FETCH NEXT FROM update_collar_deployment_cursor INTO @FileId, @PlatformId;

                WHILE @@FETCH_STATUS = 0
                BEGIN
                    EXEC dbo.CollarFile_UpdateStatus @FileId, 'I'
                    UPDATE CollarFiles SET CollarManufacturer = @NewMfgr, CollarId = @NewCollar WHERE FileId = @FileId
                    EXEC dbo.CollarFile_UpdateStatus @FileId, 'A'
                    FETCH NEXT FROM update_collar_deployment_cursor INTO @FileId;
                END
                CLOSE update_collar_deployment_cursor;
                DEALLOCATE update_collar_deployment_cursor;
            END
            ELSE
            BEGIN
                --Platform has changed. Use delete/insert procedure
                --Delete:
                DECLARE delete_platform_update_deployment_cursor CURSOR FOR
                      -- select all files (by platform) with transmissions that overlap all deleted deployments
                         SELECT A.FileId, A.PlatformId
                           FROM deleted AS d
                     INNER JOIN ArgosFilePlatformDates AS A
                             ON A.PlatformId = d.PlatformId
                          WHERE dbo.DoDateRangesOverlap(A.FirstTransmission, A.LastTransmission, d.StartDate, d.EndDate) = 1
                    
                OPEN delete_platform_update_deployment_cursor;

                FETCH NEXT FROM delete_platform_update_deployment_cursor INTO @FileId, @PlatformId;

                WHILE @@FETCH_STATUS = 0
                BEGIN
                    EXEC dbo.ArgosFile_UnProcessPlatform @FileId, @PlatformId
                    EXEC dbo.ArgosFile_ProcessPlatform @FileId, @PlatformId
                    FETCH NEXT FROM delete_platform_update_deployment_cursor INTO @FileId, @PlatformId;
                END
                CLOSE delete_platform_update_deployment_cursor;
                DEALLOCATE delete_platform_update_deployment_cursor;
                
                --Insert:
                DECLARE insert_platform_update_deployment_cursor CURSOR FOR 
                      -- select all files (by platform) with processing issues for this platform
                         SELECT A.FileId, A.PlatformId
                           FROM inserted AS I
                     INNER JOIN ArgosFilePlatformDates AS D
                             ON D.PlatformId = I.PlatformId
                     INNER JOIN ArgosFileProcessingIssues AS A
                             ON A.PlatformId = D.PlatformId AND A.FileId = D.FileId
                          WHERE dbo.DoDateRangesOverlap(D.FirstTransmission, D.LastTransmission, I.StartDate, I.EndDate) = 1
                    
                OPEN insert_platform_update_deployment_cursor;

                FETCH NEXT FROM insert_platform_update_deployment_cursor INTO @FileId, @PlatformId;

                WHILE @@FETCH_STATUS = 0
                BEGIN
                    EXEC dbo.ArgosFile_ProcessPlatform @FileId, @PlatformId
                    FETCH NEXT FROM insert_platform_update_deployment_cursor INTO @FileId, @PlatformId;
                END
                CLOSE insert_platform_update_deployment_cursor;
                DEALLOCATE insert_platform_update_deployment_cursor;
            END
        END 
        FETCH NEXT FROM update_deployment_cursor INTO @OldPlatform,@NewPlatform,@OldMfgr,@NewMfgr,@OldCollar,@NewCollar,@OldStart,@NewStart,@OldEnd,@NewEnd
    END
    CLOSE update_deployment_cursor;
    DEALLOCATE update_deployment_cursor;
END

GO
