-- Viser hver rejse med status, gruppe, deltagere, byer og anmeldelser.
-- Test: SELECT * FROM dbo.vw_TripOverview;

CREATE OR ALTER VIEW dbo.vw_TripOverview
AS
SELECT
    Trip.Id AS TripId,
    Trip.StartDate,
    Trip.EndDate,
    DATEDIFF(DAY, Trip.StartDate, Trip.EndDate) + 1 AS DurationDays,

    CASE
        WHEN Trip.IsCompleted = 1 THEN N'Gennemført'
        ELSE N'Planlagt'
    END AS TripStatus,

    -- Vælger den første gruppe alfabetisk, hvis flere grupper deler rejsen.
    (
        SELECT TOP (1) TripGroup.Name
        FROM dbo.GroupTrip AS GroupTrip
        JOIN dbo.[Group] AS TripGroup
            ON TripGroup.Id = GroupTrip.Group_Id
        WHERE GroupTrip.Trip_Id = Trip.Id
        ORDER BY TripGroup.Name
    ) AS GroupName,

    ISNULL((
        SELECT COUNT(*)
        FROM dbo.TripParticipant AS Participant
        WHERE Participant.Trip_Id = Trip.Id
    ), 0) AS ParticipantCount,

    CitySummary.Cities,
    ISNULL(CitySummary.CityCount, 0) AS CityCount,
    ISNULL(ReviewSummary.ReviewCount, 0) AS ReviewCount,
    ReviewSummary.AvgStars

FROM dbo.Trip AS Trip

OUTER APPLY (
    SELECT
        COUNT(*) AS CityCount,
        STRING_AGG(
            CAST(City.Name AS NVARCHAR(MAX)),
            N', '
        ) WITHIN GROUP (ORDER BY City.Name) AS Cities
    FROM dbo.TripStop AS TripStop
    JOIN dbo.City AS City
        ON City.Id = TripStop.City_Id
    WHERE TripStop.Trip_Id = Trip.Id
) AS CitySummary

OUTER APPLY (
    SELECT
        COUNT(*) AS ReviewCount,
        CAST(
            AVG(CAST(Review.Stars AS DECIMAL(4, 2)))
            AS DECIMAL(3, 1)
        ) AS AvgStars
    FROM dbo.Review AS Review
    WHERE Review.Trip_Id = Trip.Id
) AS ReviewSummary;
