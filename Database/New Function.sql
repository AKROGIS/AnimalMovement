USE [Animal_Movement]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[DaysSinceLastDownload] 
(
)
RETURNS INT
AS
BEGIN
    DECLARE @Result INT;

	SELECT @Result = DATEDIFF(day, MAX(TimeStamp), GETDATE())
	  FROM ArgosDownloads
	 WHERE ErrorMessage IS NULL
	
	RETURN @Result
END


GO

GRANT EXECUTE ON [dbo].[DaysSinceLastDownload] TO [NPS\Domain Users] AS [dbo]
GO


