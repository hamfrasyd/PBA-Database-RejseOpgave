/*
    Views
    Opretter fire views, der samler data fra flere tabeller
    og gør det lettere at hente relevante oplysninger.
    Views indeholder oversigter over rejser, brugerprofiler,
    destinationsstatistik og afventende invitationer.
*/

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
GO

-- ------------------------------------------------------------
-- 2. Brugerprofil
CREATE OR ALTER VIEW dbo.vw_UserProfile
AS
SELECT
    u.Id                                    AS UserId,
    u.FirstName + N' ' + u.LastName         AS FullName,
    DATEDIFF(YEAR, u.Birthday, CAST(GETDATE() AS DATE))
      - CASE WHEN DATEADD(YEAR, DATEDIFF(YEAR, u.Birthday, CAST(GETDATE() AS DATE)), u.Birthday)
                  > CAST(GETDATE() AS DATE) THEN 1 ELSE 0 END AS Age,
    c.Name                                  AS HomeCity,
    co.Name                                 AS HomeCountry,
    u.IsPrivate,
    u.CreatedAt                             AS MemberSince,
    (SELECT COUNT(*) FROM dbo.Friendship f
      WHERE f.UserA_Id = u.Id OR f.UserB_Id = u.Id)            AS FriendCount,
    (SELECT COUNT(*) FROM dbo.TripParticipant tp
      JOIN dbo.Trip t ON t.Id = tp.Trip_Id
      WHERE tp.User_Id = u.Id AND t.IsCompleted = 1)           AS CompletedTrips,
    (SELECT COUNT(*) FROM dbo.TripParticipant tp
      JOIN dbo.Trip t ON t.Id = tp.Trip_Id
      WHERE tp.User_Id = u.Id AND t.IsCompleted = 0)           AS PlannedTrips,
    (SELECT COUNT(*) FROM dbo.Review rv WHERE rv.User_Id = u.Id) AS ReviewCount,
    (SELECT COUNT(*) FROM dbo.EarnedAchievement ea
      WHERE ea.User_Id = u.Id)                                 AS AchievementCount
FROM dbo.[User] u
JOIN dbo.City c     ON c.Id  = u.City_Id
JOIN dbo.Country co ON co.Id = c.Country_Id;
GO

-- ------------------------------------------------------------
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
GO

-- ------------------------------------------------------------
-- 4. Åbne invitationer
CREATE OR ALTER VIEW dbo.vw_PendingInvitations
AS
SELECT
    i.Id                                    AS InviteId,
    i.Trip_Id                               AS TripId,
    t.StartDate                             AS TripStartDate,
    s.FirstName + N' ' + s.LastName         AS SenderName,
    r.FirstName + N' ' + r.LastName         AS ReceiverName,
    i.SendDate,
    DATEADD(DAY, 14, i.SendDate)            AS ExpiresAt,
    DATEDIFF(DAY, SYSUTCDATETIME(), DATEADD(DAY, 14, i.SendDate)) AS DaysUntilExpiry
FROM dbo.Invite i
JOIN dbo.InvitationStatus st ON st.Id = i.InvitationStatus_Id
JOIN dbo.Trip t              ON t.Id  = i.Trip_Id
JOIN dbo.[User] s            ON s.Id  = i.Sender_Id
JOIN dbo.[User] r            ON r.Id  = i.Receiver_Id
WHERE st.StatusName = N'Pending';
GO

-- Eksempler:
-- SELECT TOP 10 * FROM dbo.vw_TripOverview WHERE TripStatus = N'Gennemført' ORDER BY AvgStars DESC;
-- SELECT TOP 10 * FROM dbo.vw_UserProfile ORDER BY CompletedTrips DESC;
-- SELECT TOP 10 * FROM dbo.vw_DestinationStats ORDER BY TripCount DESC;
-- SELECT * FROM dbo.vw_PendingInvitations ORDER BY DaysUntilExpiry;