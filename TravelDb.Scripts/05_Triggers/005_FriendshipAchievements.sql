-- Aktiveres når et venskab oprettes eller opdateres.
-- Kontrollerer achievements for begge brugere i venskabet.

CREATE OR ALTER TRIGGER dbo.trg_Friendship_Achievements
ON dbo.Friendship
AFTER INSERT, UPDATE
AS
BEGIN
SET NOCOUNT ON;

DECLARE @Users dbo.AchievementUserIds;

INSERT @Users (User_Id)
SELECT UserA_Id
FROM inserted

UNION

SELECT UserB_Id
FROM inserted;

IF EXISTS (SELECT 1 FROM @Users)
EXEC dbo.AwardAchievements @Users = @Users;
END;