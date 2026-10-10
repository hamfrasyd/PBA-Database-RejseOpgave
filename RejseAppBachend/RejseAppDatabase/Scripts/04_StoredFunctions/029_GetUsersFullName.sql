-- Returns the full name for a given user
CREATE FUNCTION dbo.GetUsersFullName(@UserId INT)
RETURNS NVARCHAR(255)
AS
BEGIN
    DECLARE @FullName NVARCHAR(255);

    SELECT @FullName = FirstName + ' ' + LastName
    FROM dbo.[User]
    WHERE Id = @UserId;

    RETURN @FullName;
END;