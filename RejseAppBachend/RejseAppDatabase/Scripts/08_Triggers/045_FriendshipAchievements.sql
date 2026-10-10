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

/*
Se alle triggerne i databasen:
SELECT
name,
parent_class_desc,
create_date,
modify_date
FROM sys.triggers
ORDER BY name;
*/