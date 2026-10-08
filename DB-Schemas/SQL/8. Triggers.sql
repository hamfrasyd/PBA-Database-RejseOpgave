/*
    Opret en tabeltype og en fælles tildelingsprocedure.
    Tabellen med bruger-ID'er gør det muligt at behandle flere brugere samtidig.
    Regler: Alle deltagere regnes for at have besøgt alle stop på gennemførte rejser.
    Optjente achievements tilbagekaldes ikke ved senere ændringer.
    Achievement findes via det unikke Name. Id bruges ved selve tildelingen.
    */
IF TYPE_ID(N'dbo.AchievementUserIds') IS NULL
    EXEC(N'CREATE TYPE dbo.AchievementUserIds AS TABLE (User_Id int NOT NULL PRIMARY KEY);');
GO
CREATE OR ALTER PROCEDURE dbo.AwardAchievements
    @Users dbo.AchievementUserIds READONLY
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    IF NOT EXISTS (SELECT 1 FROM @Users) RETURN;

    -- Stop ved manglende definitioner, så et ændret navn ikke lydløst slår en regel fra.
    IF (SELECT COUNT(*) FROM dbo.Achievement WHERE Name IN
        (N'Første rejse',N'Erfaren rejsende',N'Byopdager',N'Første anmeldelse',N'Social rejsende'))<>5
        THROW 50102,N'En achievement-definition mangler eller er omdøbt. Kontrollér navnene fra fil 01.',1;

    -- Proceduren kan både kaldes fra en trigger og direkte.
    DECLARE @OwnTransaction bit=CASE WHEN @@TRANCOUNT=0 THEN 1 ELSE 0 END;
    BEGIN TRY
        IF @OwnTransaction=1 BEGIN TRANSACTION;

        ;WITH Completed AS (
            SELECT p.User_Id,p.Trip_Id FROM dbo.TripParticipant p
            JOIN @Users u ON u.User_Id=p.User_Id
            JOIN dbo.Trip t ON t.Id=p.Trip_Id WHERE t.IsCompleted=1
        ), TripCounts AS (
            SELECT User_Id,COUNT(*) AS Total FROM Completed GROUP BY User_Id
        ), CityCounts AS (
            SELECT c.User_Id,COUNT(DISTINCT s.City_Id) AS Total
            FROM Completed c JOIN dbo.TripStop s ON s.Trip_Id=c.Trip_Id GROUP BY c.User_Id
        ), Reviewers AS (
            SELECT DISTINCT r.User_Id FROM dbo.Review r JOIN @Users u ON u.User_Id=r.User_Id
        ), FriendPairs AS (
            SELECT f.UserA_Id AS User_Id,f.UserB_Id AS Friend_Id
            FROM dbo.Friendship f JOIN @Users u ON u.User_Id=f.UserA_Id
            UNION
            SELECT f.UserB_Id,f.UserA_Id FROM dbo.Friendship f JOIN @Users u ON u.User_Id=f.UserB_Id
        ), FriendCounts AS (
            SELECT User_Id,COUNT(*) AS Total FROM FriendPairs GROUP BY User_Id
        ), Eligible AS (
            SELECT User_Id,N'Første rejse' AS Name FROM TripCounts WHERE Total>=1
            UNION ALL SELECT User_Id,N'Erfaren rejsende' FROM TripCounts WHERE Total>=5
            UNION ALL SELECT User_Id,N'Byopdager' FROM CityCounts WHERE Total>=10
            UNION ALL SELECT User_Id,N'Første anmeldelse' FROM Reviewers
            UNION ALL SELECT User_Id,N'Social rejsende' FROM FriendCounts WHERE Total>=5
        )
        INSERT dbo.EarnedAchievement(User_Id,Achievement_Id,EarnedAt)
        SELECT e.User_Id,a.Id,SYSUTCDATETIME()
        FROM Eligible e JOIN dbo.Achievement a ON a.Name=e.Name
        WHERE NOT EXISTS (
            -- Lås eksisterende nøgler eller nøgleintervaller indtil transaktionen slutter.
            SELECT 1 FROM dbo.EarnedAchievement existing WITH (UPDLOCK,HOLDLOCK)
            WHERE existing.User_Id=e.User_Id AND existing.Achievement_Id=a.Id
        );
        IF @OwnTransaction=1 COMMIT;
    END TRY
    BEGIN CATCH
        IF @OwnTransaction=1 AND XACT_STATE()<>0 ROLLBACK;
        THROW;
    END CATCH;
END;
GO
-- ---------------------------------------------------------------------------
-- Kald procedure får at tildele achievement til eksisterende brugere.
DECLARE @Users dbo.AchievementUserIds;

INSERT INTO @Users (User_Id)
SELECT Id
FROM dbo.[User];

EXEC dbo.AwardAchievements @Users = @Users;
-- ----------------------------------------------------------------------------
/*
    Opret fem triggers.
    Hver trigger samler bruger-ID'er fra inserted og kalder samme procedure.
    DELETE behandles ikke: Optjente achievements beholdes.
    Tidligere enkelt-trigger fra samtalen erstattes, hvis den er installeret.
*/
IF OBJECT_ID(N'dbo.trg_Trip_AwardFirstCompletedTrip',N'TR') IS NOT NULL
    DISABLE TRIGGER dbo.trg_Trip_AwardFirstCompletedTrip ON dbo.Trip;
GO

-- ----------------------------------------------------------------------------
-- Kontrollér deltagere, når rejser ændres eller indsættes som gennemførte.
CREATE OR ALTER TRIGGER dbo.trg_Trip_Achievements
ON dbo.Trip
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Users dbo.AchievementUserIds;
    INSERT @Users(User_Id)
    SELECT DISTINCT p.User_Id FROM inserted i JOIN dbo.TripParticipant p ON p.Trip_Id=i.Id WHERE i.IsCompleted=1;
    IF EXISTS (SELECT 1 FROM @Users)
        EXEC dbo.AwardAchievements @Users=@Users;
END;
GO

-- ----------------------------------------------------------------------------
-- Kontrollér også deltagere, som tilføjes efter en rejses afslutning.
CREATE OR ALTER TRIGGER dbo.trg_TripParticipant_Achievements
ON dbo.TripParticipant
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Users dbo.AchievementUserIds;
    INSERT @Users(User_Id)
    SELECT DISTINCT User_Id FROM inserted;
    IF EXISTS (SELECT 1 FROM @Users)
        EXEC dbo.AwardAchievements @Users=@Users;
END;
GO

-- ----------------------------------------------------------------------------
-- Kontrollér bytællingen, når stop tilføjes eller ændres.
CREATE OR ALTER TRIGGER dbo.trg_TripStop_Achievements
ON dbo.TripStop
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Users dbo.AchievementUserIds;
    INSERT @Users(User_Id)
    SELECT DISTINCT p.User_Id FROM inserted i JOIN dbo.TripParticipant p ON p.Trip_Id=i.Trip_Id;
    IF EXISTS (SELECT 1 FROM @Users)
        EXEC dbo.AwardAchievements @Users=@Users;
END;
GO

-- ----------------------------------------------------------------------------
-- Kontrollér de nye forfattere ved indsættelse eller ændring.
CREATE OR ALTER TRIGGER dbo.trg_Review_Achievements
ON dbo.Review
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Users dbo.AchievementUserIds;
    INSERT @Users(User_Id)
    SELECT DISTINCT User_Id FROM inserted;
    IF EXISTS (SELECT 1 FROM @Users)
        EXEC dbo.AwardAchievements @Users=@Users;
END;
GO

-- ----------------------------------------------------------------------------
-- Kontrollér begge deltagere i hvert nyt eller ændret venskab.
CREATE OR ALTER TRIGGER dbo.trg_Friendship_Achievements
ON dbo.Friendship
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Users dbo.AchievementUserIds;
    INSERT @Users(User_Id)
    SELECT UserA_Id FROM inserted UNION SELECT UserB_Id FROM inserted;
    IF EXISTS (SELECT 1 FROM @Users)
        EXEC dbo.AwardAchievements @Users=@Users;
END;
GO