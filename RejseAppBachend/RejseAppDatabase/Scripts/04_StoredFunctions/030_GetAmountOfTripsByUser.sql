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