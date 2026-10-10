CREATE OR ALTER TRIGGER dbo.trg_Trip_Achievements
ON dbo.Trip
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Users dbo.AchievementUserIds;

    INSERT @Users (User_Id)
    SELECT DISTINCT p.User_Id
    FROM inserted i
    JOIN dbo.TripParticipant p
        ON p.Trip_Id = i.Id
    WHERE i.IsCompleted = 1;

    IF EXISTS (SELECT 1 FROM @Users)
        EXEC dbo.AwardAchievements @Users = @Users;
END;