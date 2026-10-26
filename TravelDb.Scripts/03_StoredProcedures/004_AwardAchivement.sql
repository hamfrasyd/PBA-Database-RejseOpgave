-- Kontrollerer achievements for de angivne brugere baseret på rejser, byer, anmeldelser og venskaber.
-- Tildeler kun achievements, som brugerne endnu ikke har modtaget.

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

    DECLARE @StartedOwnTransaction BIT =
        CASE WHEN @@TRANCOUNT = 0 THEN 1 ELSE 0 END;

    DECLARE @EligibleAchievements TABLE (
        UserId INT NOT NULL,
        AchievementName NVARCHAR(128) NOT NULL,
        PRIMARY KEY (UserId, AchievementName)
    );

    BEGIN TRY
        IF @StartedOwnTransaction = 1
            BEGIN TRANSACTION;

        -- 1. Første rejse og Erfaren rejsende
        ;WITH CompletedTripCounts AS (
            SELECT
                Participant.User_Id AS UserId,
                COUNT(*) AS CompletedTrips
            FROM dbo.TripParticipant AS Participant
            JOIN dbo.Trip AS Trip
                ON Trip.Id = Participant.Trip_Id
            JOIN @Users AS CandidateUser
                ON CandidateUser.User_Id = Participant.User_Id
            WHERE Trip.IsCompleted = 1
            GROUP BY Participant.User_Id
        )
        INSERT INTO @EligibleAchievements (UserId, AchievementName)
        SELECT
            UserId,
            N'Første rejse'
        FROM CompletedTripCounts
        WHERE CompletedTrips >= 1

        UNION ALL

        SELECT
            UserId,
            N'Erfaren rejsende'
        FROM CompletedTripCounts
        WHERE CompletedTrips >= 5;


        -- 2. Byopdager: 10 forskellige byer på gennemførte rejser
        INSERT INTO @EligibleAchievements (UserId, AchievementName)
        SELECT
            Participant.User_Id,
            N'Byopdager'
        FROM dbo.TripParticipant AS Participant
        JOIN @Users AS CandidateUser
            ON CandidateUser.User_Id = Participant.User_Id
        JOIN dbo.Trip AS Trip
            ON Trip.Id = Participant.Trip_Id
        JOIN dbo.TripStop AS TripStop
            ON TripStop.Trip_Id = Trip.Id
        WHERE Trip.IsCompleted = 1
        GROUP BY Participant.User_Id
        HAVING COUNT(DISTINCT TripStop.City_Id) >= 10;


        -- 3. Første anmeldelse
        INSERT INTO @EligibleAchievements (UserId, AchievementName)
        SELECT
            CandidateUser.User_Id,
            N'Første anmeldelse'
        FROM @Users AS CandidateUser
        WHERE EXISTS (
            SELECT 1
            FROM dbo.Review AS Review
            WHERE Review.User_Id = CandidateUser.User_Id
        );


        -- 4. Social rejsende: 5 forskellige venner
        ;WITH UserFriendships AS (
            SELECT
                Friendship.UserA_Id AS UserId,
                Friendship.UserB_Id AS FriendId
            FROM dbo.Friendship AS Friendship
            JOIN @Users AS CandidateUser
                ON CandidateUser.User_Id = Friendship.UserA_Id

            UNION

            SELECT
                Friendship.UserB_Id AS UserId,
                Friendship.UserA_Id AS FriendId
            FROM dbo.Friendship AS Friendship
            JOIN @Users AS CandidateUser
                ON CandidateUser.User_Id = Friendship.UserB_Id
        )
        INSERT INTO @EligibleAchievements (UserId, AchievementName)
        SELECT
            UserId,
            N'Social rejsende'
        FROM UserFriendships
        GROUP BY UserId
        HAVING COUNT(*) >= 5;


        -- 5. Tildel kun achievements, som brugeren mangler
        INSERT INTO dbo.EarnedAchievement (
            User_Id,
            Achievement_Id,
            EarnedAt
        )
        SELECT
            Eligible.UserId,
            Achievement.Id,
            SYSUTCDATETIME()
        FROM @EligibleAchievements AS Eligible
        JOIN dbo.Achievement AS Achievement
            ON Achievement.Name = Eligible.AchievementName
        WHERE NOT EXISTS (
            SELECT 1
            FROM dbo.EarnedAchievement AS Existing
                WITH (UPDLOCK, HOLDLOCK)
            WHERE Existing.User_Id = Eligible.UserId
              AND Existing.Achievement_Id = Achievement.Id
        );

        IF @StartedOwnTransaction = 1
            COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH
        IF @StartedOwnTransaction = 1
           AND XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH;
END;
GO
