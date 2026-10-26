-- Viser statistik for hver by med rejser, aktiviteter og anmeldelser.
-- Test: SELECT * FROM dbo.vw_DestinationStats;

CREATE OR ALTER VIEW dbo.vw_DestinationStats
AS
SELECT
    DestinationCity.Id AS CityId,
    DestinationCity.Name AS City,
    Country.Name AS Country,
    Continent.Name AS Continent,

    ISNULL(TripSummary.TripCount, 0) AS TripCount,
    ISNULL(TripSummary.CompletedTripCount, 0) AS CompletedTripCount,

    (
        SELECT COUNT(*)
        FROM dbo.Activity AS Activity
        WHERE Activity.City_Id = DestinationCity.Id
    ) AS ActivityCount,

    (
        SELECT
            CAST(
                AVG(CAST(Review.Stars AS DECIMAL(4, 2)))
                AS DECIMAL(3, 1)
            )
        FROM dbo.Review AS Review
        WHERE Review.Trip_Id IN (
            SELECT TripStop.Trip_Id
            FROM dbo.TripStop AS TripStop
            WHERE TripStop.City_Id = DestinationCity.Id
        )
    ) AS AvgStars

FROM dbo.City AS DestinationCity

JOIN dbo.Country AS Country
    ON Country.Id = DestinationCity.Country_Id

JOIN dbo.Continent AS Continent
    ON Continent.Id = Country.Continent_Id

OUTER APPLY (
    SELECT
        COUNT(*) AS TripCount,
        COUNT(CASE WHEN Trip.IsCompleted = 1 THEN 1 END) AS CompletedTripCount
    FROM dbo.TripStop AS TripStop
    JOIN dbo.Trip AS Trip
        ON Trip.Id = TripStop.Trip_Id
    WHERE TripStop.City_Id = DestinationCity.Id
) AS TripSummary;
