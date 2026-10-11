-- Find byerne på en bestemt rejse
CREATE NONCLUSTERED INDEX IX_TripStop_TripId
ON dbo.TripStop (Trip_Id, City_Id);