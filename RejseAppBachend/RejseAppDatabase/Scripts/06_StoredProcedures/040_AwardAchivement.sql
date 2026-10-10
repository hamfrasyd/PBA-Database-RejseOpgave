IF TYPE_ID(N'dbo.AchievementUserIds') IS NULL
BEGIN
    EXEC(N'
        CREATE TYPE dbo.AchievementUserIds
        AS TABLE
        (
            User_Id INT NOT NULL PRIMARY KEY
        );
    ');
END;
GO

CREATE OR ALTER PROCEDURE dbo.AwardAchievements
    @Users dbo.AchievementUserIds READONLY
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NOT EXISTS (SELECT 1 FROM @Users)
        RETURN;

    IF (
        SELECT COUNT(*)
        FROM dbo.Achievement
        WHERE Name IN (
            N'Første rejse',
            N'Erfaren rejsende',
            N'Byopdager',
            N'Første anmeldelse',
            N'Social rejsende'
        )
    ) <> 5
    BEGIN
        THROW 50102,
              N'En achievement-definition mangler eller er omdøbt.',
              1;
    END;

    DECLARE @OwnTransaction BIT =
        CASE WHEN @@TRANCOUNT = 0 THEN 1 ELSE 0 END;

    BEGIN TRY

        IF @OwnTransaction = 1
            BEGIN TRANSACTION;

        ;WITH Completed AS
        (
            SELECT
                p.User_Id,
                p.Trip_Id
            FROM dbo.TripParticipant p
            JOIN @Users u
                ON u.User_Id = p.User_Id
            JOIN dbo.Trip t
                ON t.Id = p.Trip_Id
            WHERE t.IsCompleted = 1
        ),
        TripCounts AS
        (
            SELECT
                User_Id,
                COUNT(*) AS Total
            FROM Completed
            GROUP BY User_Id
        ),
        CityCounts AS
        (
            SELECT
                c.User_Id,
                COUNT(DISTINCT s.City_Id) AS Total
            FROM Completed c
            JOIN dbo.TripStop s
                ON s.Trip_Id = c.Trip_Id
            GROUP BY c.User_Id
        ),
        Reviewers AS
        (
            SELECT DISTINCT r.User_Id
            FROM dbo.Review r
            JOIN @Users u
                ON u.User_Id = r.User_Id
        ),
        FriendPairs AS
        (
            SELECT
                f.UserA_Id AS User_Id,
                f.UserB_Id AS Friend_Id
            FROM dbo.Friendship f
            JOIN @Users u
                ON u.User_Id = f.UserA_Id

            UNION

            SELECT
                f.UserB_Id,
                f.UserA_Id
            FROM dbo.Friendship f
            JOIN @Users u
                ON u.User_Id = f.UserB_Id
        ),
        FriendCounts AS
        (
            SELECT
                User_Id,
                COUNT(*) AS Total
            FROM FriendPairs
            GROUP BY User_Id
        ),
        Eligible AS
        (
            SELECT User_Id, N'Første rejse' AS Name
            FROM TripCounts
            WHERE Total >= 1

            UNION ALL

            SELECT User_Id, N'Erfaren rejsende'
            FROM TripCounts
            WHERE Total >= 5

            UNION ALL

            SELECT User_Id, N'Byopdager'
            FROM CityCounts
            WHERE Total >= 10

            UNION ALL

            SELECT User_Id, N'Første anmeldelse'
            FROM Reviewers

            UNION ALL

            SELECT User_Id, N'Social rejsende'
            FROM FriendCounts
            WHERE Total >= 5
        )

        INSERT dbo.EarnedAchievement
        (
            User_Id,
            Achievement_Id,
            EarnedAt
        )
        SELECT
            e.User_Id,
            a.Id,
            SYSUTCDATETIME()
        FROM Eligible e
        JOIN dbo.Achievement a
            ON a.Name = e.Name
        WHERE NOT EXISTS
        (
            SELECT 1
            FROM dbo.EarnedAchievement existing
                WITH (UPDLOCK, HOLDLOCK)
            WHERE existing.User_Id = e.User_Id
              AND existing.Achievement_Id = a.Id
        );

        IF @OwnTransaction = 1
            COMMIT;

    END TRY
    BEGIN CATCH

        IF @OwnTransaction = 1
           AND XACT_STATE() <> 0
            ROLLBACK;

        THROW;

    END CATCH;
END;
GO


