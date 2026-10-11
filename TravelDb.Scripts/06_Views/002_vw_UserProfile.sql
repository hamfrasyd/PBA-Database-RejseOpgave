-- Viser brugerprofiler med bopæl, alder, rejser og aktivitet.
-- Test: SELECT * FROM dbo.vw_UserProfile;

CREATE OR ALTER VIEW dbo.vw_UserProfile
AS
SELECT
    ProfileUser.Id AS UserId,
    ProfileUser.FirstName + N' ' + ProfileUser.LastName AS FullName,

    AgeCalculation.CalendarYears -
        CASE
            WHEN DATEADD(
                YEAR,
                AgeCalculation.CalendarYears,
                ProfileUser.Birthday
            ) > AgeCalculation.Today THEN 1
            ELSE 0
        END AS Age,

    HomeCity.Name AS HomeCity,
    HomeCountry.Name AS HomeCountry,
    ProfileUser.IsPrivate,
    ProfileUser.CreatedAt AS MemberSince,

    (
        SELECT COUNT(*)
        FROM dbo.Friendship AS Friendship
        WHERE Friendship.UserA_Id = ProfileUser.Id
           OR Friendship.UserB_Id = ProfileUser.Id
    ) AS FriendCount,

    TripSummary.CompletedTrips,
    TripSummary.PlannedTrips,

    (
        SELECT COUNT(*)
        FROM dbo.Review AS Review
        WHERE Review.User_Id = ProfileUser.Id
    ) AS ReviewCount,

    (
        SELECT COUNT(*)
        FROM dbo.EarnedAchievement AS Achievement
        WHERE Achievement.User_Id = ProfileUser.Id
    ) AS AchievementCount

FROM dbo.[User] AS ProfileUser

JOIN dbo.City AS HomeCity
    ON HomeCity.Id = ProfileUser.City_Id

JOIN dbo.Country AS HomeCountry
    ON HomeCountry.Id = HomeCity.Country_Id

CROSS APPLY (
    SELECT
        CAST(GETDATE() AS DATE) AS Today,
        DATEDIFF(
            YEAR,
            ProfileUser.Birthday,
            CAST(GETDATE() AS DATE)
        ) AS CalendarYears
) AS AgeCalculation

OUTER APPLY (
    SELECT
        COUNT(CASE WHEN Trip.IsCompleted = 1 THEN 1 END) AS CompletedTrips,
        COUNT(CASE WHEN Trip.IsCompleted = 0 THEN 1 END) AS PlannedTrips
    FROM dbo.TripParticipant AS Participant
    JOIN dbo.Trip AS Trip
        ON Trip.Id = Participant.Trip_Id
    WHERE Participant.User_Id = ProfileUser.Id
) AS TripSummary;
