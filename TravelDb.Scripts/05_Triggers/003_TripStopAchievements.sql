-- Aktiveres når destinationer tilføjes eller opdateres på en rejse.
-- Kontrollerer achievements for alle deltagere på de berørte rejser.

CREATE OR ALTER TRIGGER dbo.trg_TripStop_Achievements
ON dbo.TripStop
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Users dbo.AchievementUserIds;

    INSERT @Users (User_Id)
    SELECT DISTINCT p.User_Id
    FROM inserted i
    JOIN dbo.TripParticipant p
        ON p.Trip_Id = i.Trip_Id;

    IF EXISTS (SELECT 1 FROM @Users)
        EXEC dbo.AwardAchievements @Users = @Users;
END;