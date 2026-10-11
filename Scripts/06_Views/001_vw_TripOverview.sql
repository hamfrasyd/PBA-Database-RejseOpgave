-- 1. Rejseoversigt
CREATE OR ALTER VIEW dbo.vw_TripOverview
AS
SELECT
    t.Id                                    AS TripId,
    t.StartDate,
    t.EndDate,
    DATEDIFF(DAY, t.StartDate, t.EndDate) + 1 AS DurationDays,
    CASE WHEN t.IsCompleted = 1 THEN N'Gennemført' ELSE N'Planlagt' END AS TripStatus,
    g.GroupName,
    ISNULL(p.ParticipantCount, 0)           AS ParticipantCount,
    s.Cities,
    ISNULL(s.CityCount, 0)                  AS CityCount,
    ISNULL(r.ReviewCount, 0)                AS ReviewCount,
    r.AvgStars
FROM dbo.Trip t
OUTER APPLY (
    SELECT TOP (1) gr.Name AS GroupName
    FROM dbo.GroupTrip gt
    JOIN dbo.[Group] gr ON gr.Id = gt.Group_Id
    WHERE gt.Trip_Id = t.Id
    ORDER BY gr.Name
) g
OUTER APPLY (
    SELECT COUNT(*) AS ParticipantCount
    FROM dbo.TripParticipant tp
    WHERE tp.Trip_Id = t.Id
) p
OUTER APPLY (
    SELECT
        COUNT(*) AS CityCount,
        STRING_AGG(CAST(c.Name AS NVARCHAR(MAX)), N', ') WITHIN GROUP (ORDER BY c.Name) AS Cities
    FROM dbo.TripStop ts
    JOIN dbo.City c ON c.Id = ts.City_Id
    WHERE ts.Trip_Id = t.Id
) s
OUTER APPLY (
    SELECT
        COUNT(*) AS ReviewCount,
        CAST(AVG(CAST(rv.Stars AS DECIMAL(4,2))) AS DECIMAL(3,1)) AS AvgStars
    FROM dbo.Review rv
    WHERE rv.Trip_Id = t.Id
) r;