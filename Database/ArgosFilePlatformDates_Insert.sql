USE [Animal_Movement]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





-- =============================================
-- Author:      Regan Sarwas
-- Create date: March 14, 2013
-- Description: Adds a collar file summary to the database.
-- =============================================
CREATE PROCEDURE [dbo].[ArgosFilePlatformDates_Insert] 
	@FileId INT,
	@PlatformId NVARCHAR(255), 
	@ProgramId NVARCHAR(255), 
	@FirstTransmission DATETIME2(7), 
	@LastTransmission DATETIME2(7) 
AS
BEGIN
	SET NOCOUNT ON;
	
	-- Get the name of the caller
	DECLARE @Caller sysname = ORIGINAL_LOGIN();

	-- Validate permission for this operation
	-- The caller must be uploader of the file
	IF NOT EXISTS (SELECT 1 FROM dbo.CollarFiles WHERE FileId = @FileId AND UserName = @Caller)
	BEGIN
		DECLARE @message1 nvarchar(200) = 'You ('+@Caller+') must have uploaded the source file to create the summary.';
		RAISERROR(@message1, 18, 0)
		RETURN (1)
	END
	
	--All other verification is handled by primary/foreign key and column constraints.
	INSERT INTO dbo.ArgosFilePlatformDates (FileId, PlatformId, ProgramId, FirstTransmission, LastTransmission)
		 VALUES (@FileId, @PlatformId, @ProgramId, @FirstTransmission, @LastTransmission);

END


GO

GRANT EXECUTE ON [dbo].[ArgosFilePlatformDates_Insert] TO [ArgosProcessor] AS [dbo]
GO

GRANT EXECUTE ON [dbo].[ArgosFilePlatformDates_Insert] TO [Editor] AS [dbo]
GO


