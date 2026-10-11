-- 3. Destinationsstatistik
CREATE OR ALTER VIEW dbo.vw_DestinationStats
AS
SELECT
    c.Id                                    AS CityId,
    c.Name                                  AS City,
    co.Name                                 AS Country,
    ct.Name                                 AS Continent,
    ISNULL(v.TripCount, 0)                  AS TripCount,
    ISNULL(v.CompletedTripCount, 0)         AS CompletedTripCount,
    (SELECT COUNT(*) FROM dbo.Activity a WHERE a.City_Id = c.Id) AS ActivityCount,
    rv.AvgStars
FROM dbo.City c
JOIN dbo.Country co   ON co.Id = c.Country_Id
JOIN dbo.Continent ct ON ct.Id = co.Continent_Id
OUTER APPLY (
    SELECT
        COUNT(DISTINCT ts.Trip_Id) AS TripCount,
        COUNT(DISTINCT CASE WHEN t.IsCompleted = 1 THEN ts.Trip_Id END) AS CompletedTripCount
    FROM dbo.TripStop ts
    JOIN dbo.Trip t ON t.Id = ts.Trip_Id
    WHERE ts.City_Id = c.Id
) v
OUTER APPLY (
    SELECT CAST(AVG(CAST(r.Stars AS DECIMAL(4,2))) AS DECIMAL(3,1)) AS AvgStars
    FROM dbo.Review r
    WHERE r.Trip_Id IN (SELECT ts.Trip_Id FROM dbo.TripStop ts WHERE ts.City_Id = c.Id)
) rv;