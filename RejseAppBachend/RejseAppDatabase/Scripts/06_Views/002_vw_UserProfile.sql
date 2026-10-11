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