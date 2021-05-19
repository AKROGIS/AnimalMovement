-- A function to return a table of intermediate points given an Animal Movement vector
-- Part of the WACH Gridpoint Analysis

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER FUNCTION [dbo].[VectorToPoints]
(
	@ProjectID  NVARCHAR(255),
	@AnimalId   NVARCHAR(255),
	@StartDate  DateTime2
)
RETURNS @summary TABLE
(
	ProjectID  NVARCHAR(255),
	AnimalId   NVARCHAR(255),
    Lat        FLOAT,
    Long       FLOAT,
	FixDate    DateTime2,
    Speed      Float,
    Duration   Float,
    Distance   Float
)
AS
BEGIN
	DECLARE @lat float;
	DECLARE @lon float;
	DECLARE @lat1 float;
	DECLARE @lat2 float;
	DECLARE @lon1 float;
	DECLARE @lon2 float;
	DECLARE @deltaX float;
	DECLARE @deltaY float;
    DECLARE @n int;
    DECLARE @nX int;
    DECLARE @nY int;
	DECLARE @EndDate DateTime2;
    DECLARE @Date DateTime2;
    DECLARE @deltaS float;
    DECLARE @Speed FLOAT;
    DECLARE @Distance FLOAT;
    DECLARE @Duration FLOAT;

    -- Use 3 and 1000 for 3 significant digits (i.e ~110 meter cells)
    -- Use 4 and 10000 for 4 significant digits (i.e ~11 meter cells)

	SELECT
      @lat1 = Round(Shape.STPointN(1).Lat, 3),
      @lat2 = Round(Shape.STPointN(2).Lat, 3),
      @lon1 = Round(Shape.STPointN(1).Long, 3),
      @lon2 = Round(Shape.STPointN(2).Long, 3),
    --   @lat1 = Round(Shape.STPointN(1).Lat, 4),
    --   @lat2 = Round(Shape.STPointN(2).Lat, 4),
    --   @lon1 = Round(Shape.STPointN(1).Long, 4),
    --   @lon2 = Round(Shape.STPointN(2).Long, 4),
      @Speed = Speed,
      @Distance = Distance,
      @Duration = Duration,
      @EndDate = EndDate
    From Movements
	WHERE ProjectId = @ProjectId AND AnimalId = @AnimalId AND StartDate = @StartDate

    IF @lat1 is not NULL
    BEGIN
        select @deltaX = @lon2 - @lon1
        select @nX = ABS(Round(1000 * @deltaX, 0))
        --select @nX = ABS(Round(10000 * @deltaX, 0))

        select @deltaY = @lat2 - @lat1
        select @nY = ABS(Round(1000 * @deltaY, 0))
        --select @nY = ABS(Round(10000 * @deltaY, 0))

        select @deltaS = DateDiff(SECOND, @StartDate, @EndDate)
        select @n = case when @nX > @nY then @nX else @nY END
        select @n = case when @n = 0 then 1 else @n END
        select @deltaX = @deltaX/@n
        select @deltaY = @deltaY/@n
        select @deltaS = @deltaS/@n
        select @Duration = @Duration/@n
        select @Distance = @Distance/@n

        -- Start point
        insert @summary
        values(@projectId, @animalId, @lat1, @lon1, @startDate, @Speed, @Duration, @Distance)

        -- Intermediate points
        DECLARE @cnt INT = 1;
        WHILE @cnt < @n
        BEGIN
            select @lat = Round(@lat1 + @deltaY * @cnt, 3)
            select @lon = Round(@lon1 + @deltaX * @cnt, 3)
            -- select @lat = Round(@lat1 + @deltaY * @cnt, 4)
            -- select @lon = Round(@lon1 + @deltaX * @cnt, 4)
            select @date = DATEADD(SECOND, @deltaS * @cnt, @StartDate)
            insert @summary
               values(@projectId, @animalId, @lat, @lon, @date, @Speed, @Duration, @Distance)
        SET @cnt = @cnt + 1;
        END;

        -- End point
        --insert @summary
        --values(@projectId, @animalId, @lat2, @lon2, @EndDate, @Speed, @Duration, @Distance)

    END

	RETURN
END


GO
