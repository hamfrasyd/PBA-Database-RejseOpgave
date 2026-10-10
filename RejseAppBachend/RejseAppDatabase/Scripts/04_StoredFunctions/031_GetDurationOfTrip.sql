-- Returns the number of days a trip lasts (start and end day both included).
-- Returns 0 if the trip doesn't exist or has no start/end date.
CREATE FUNCTION dbo.GetDurationOfTrip (@TripId INT)
RETURNS INT
AS
BEGIN
    DECLARE @StartDate DATETIME2;
    DECLARE @EndDate DATETIME2;

    SELECT @StartDate = StartDate,
           @EndDate = EndDate
    FROM dbo.Trip
    WHERE Id = @TripId;

    IF @StartDate IS NULL OR @EndDate IS NULL
        RETURN 0;

    RETURN DATEDIFF(DAY, @StartDate, @EndDate) + 1;
END;