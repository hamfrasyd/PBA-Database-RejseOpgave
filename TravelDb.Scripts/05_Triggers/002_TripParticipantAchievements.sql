-- Aktiveres når deltagere tilføjes eller opdateres på en rejse.
-- Kontrollerer om de berørte deltagere har opnået nye achievements.

CREATE OR ALTER TRIGGER dbo.trg_TripParticipant_Achievements
ON dbo.TripParticipant
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Users dbo.AchievementUserIds;

    INSERT @Users (User_Id)
    SELECT DISTINCT User_Id
    FROM inserted;

    IF EXISTS (SELECT 1 FROM @Users)
        EXEC dbo.AwardAchievements @Users = @Users;
END;
