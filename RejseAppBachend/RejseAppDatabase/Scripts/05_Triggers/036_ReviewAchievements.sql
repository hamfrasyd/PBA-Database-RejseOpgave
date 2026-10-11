CREATE OR ALTER TRIGGER dbo.trg_Review_Achievements
ON dbo.Review
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