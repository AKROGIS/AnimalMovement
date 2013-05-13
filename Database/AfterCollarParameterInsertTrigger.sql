USE [Animal_Movement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Regan Sarwas
-- Create date: May 13, 2013
-- Description: Validates the business rules for a new CollarParameter
-- =============================================
ALTER TRIGGER [dbo].[AfterCollarParameterInsert] 
   ON  [dbo].[CollarParameters] 
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
        RAISERROR('The end of the parameter must occur after the start of the parameter.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END

    -- 2. Ensure the Deployment begins before the DisposalDate of the Collar
    IF EXISTS (SELECT 1
                 FROM inserted AS I
           INNER JOIN Collars AS C
                   ON I.CollarManufacturer = C.CollarManufacturer AND I.CollarId = C.CollarId
                WHERE C.DisposalDate < I.StartDate
                   OR (I.StartDate IS NULL AND C.DisposalDate IS NOT NULL)
              )
    BEGIN
        RAISERROR('The parameter must begin before the collar is disposed.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END

    -- 3. Ensure a Collar does not have multiple Parameters at the same time
    -- We are checking each inserted parameter against all existing parameters, and all other inserted parameters 
    IF EXISTS (SELECT 1
                 FROM inserted AS I1
            LEFT JOIN inserted AS I2
                   ON I1.CollarManufacturer = I2.CollarManufacturer AND I1.CollarId = I2.CollarId AND I1.ParameterId <> I2.ParameterId
           INNER JOIN dbo.CollarParameters AS P
                   ON P.CollarManufacturer = I1.CollarManufacturer AND P.CollarId = I1.CollarId AND P.ParameterId <> I1.ParameterId
                WHERE dbo.DoDateRangesOverlap(P.StartDate, P.EndDate, I1.StartDate, I1.EndDate) = 1
                   OR (I2.ParameterId IS NOT NULL AND
                       dbo.DoDateRangesOverlap(I1.StartDate, I1.EndDate, I2.StartDate, I2.EndDate) = 1)
              )
    BEGIN
        RAISERROR('Collars cannot have overlapping parameter dates.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END

    -- 4. Ensure that one and only one of FileId and Gen3Period are non-null
    IF EXISTS (SELECT 1
                 FROM inserted
                WHERE (Gen3Period IS NULL AND FileId IS NULL) OR
                      (Gen3Period IS NOT NULL AND FileId IS NOT NULL)
              )
    BEGIN
        RAISERROR('One and only one of Gen3 period and parameter file must be provided.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END

    -- 5. Inactive parameter files cannot be linked to a collar
    IF EXISTS (SELECT 1
                 FROM inserted AS I
           INNER JOIN CollarParameterFiles AS F
                   ON F.FileId = I.FileId
                WHERE F.[Status] <> 'A'
              )
    BEGIN
        RAISERROR('Inactive parameter files cannot be linked to a collar.', 18, 0)
        ROLLBACK TRANSACTION;
        RETURN
    END


    -- Process (partialy?) all files that might benefit from this parameter
    -- We do not do the processing in the trigger, as it may require an external process
    -- which would block the transaction and hold a lock that would slow processing.
    -- Instead, we will monitor for changes that will trigger the external processing.

    -- If a processed file had no issues, then do not reprocess.
    --    This parameter is irrelevant to the file - the file had everything it when it was processed.
    -- If a processed file had issues
    --    without mentioning the collar, then it was unable to link the platform to a collar, so
    --      this new information will not due any good.
    --    with the collar in this parameter, then it was probably a missing parameter.
    --      this file should be re-processed for the matching platfrom(s).  This can be done by
    --      removing the issue.  The (file,platform) will then be recognized in need of re-processing.
    -- If a file is unprocessed for some reason, then it is not in our perview to decide that it should
    --   be processed, so we will skip it, even if it has transmissions for this platform. (The file should get
    --   processed at some point in the future by the user or a scheduled task at which point the new parameter
    --   will be considered.)

    -- May be part of a batch insert (unlikely, and only by SA as all others must use SP)
    -- Triggers always execute in the context of a transaction, so the following code is all or nothing.

        DELETE A 
          FROM inserted AS I
    INNER JOIN ArgosFileProcessingIssues AS A
            ON A.CollarManufacturer = I.CollarManufacturer AND A.CollarId = I.CollarId

END

