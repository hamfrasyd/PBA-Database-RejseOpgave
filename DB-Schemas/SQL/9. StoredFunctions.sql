-- Returns the full name for a given user
CREATE FUNCTION dbo.GetUsersFullName(@UserId INT)
RETURNS NVARCHAR(255)
AS
BEGIN
    DECLARE @FullName NVARCHAR(255);

    SELECT @FullName = FirstName + ' ' + LastName
    FROM dbo.User
    WHERE Id = @UserId;

    RETURN @FullName;
END;
GO

-- Returns the total number of completed trips for a given user
CREATE FUNCTION dbo.GetAmountOfTripsByUser(@UserId INT)
RETURNS INT
AS
BEGIN
    DECLARE @TripCount INT;

    SELECT @TripCount = COUNT(*)
    FROM dbo.TripParticipant tp
    INNER JOIN dbo.Trip t ON t.Id = tp.Trip_Id
    WHERE tp.User_Id = @UserId
    AND t.IsCompleted = 1;

    RETURN @TripCount;
END;
GO

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
GO