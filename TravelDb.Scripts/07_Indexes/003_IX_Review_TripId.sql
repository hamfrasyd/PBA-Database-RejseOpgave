-- Find anmeldelser og stjerner for en rejse
CREATE NONCLUSTERED INDEX IX_Review_TripId
ON dbo.Review (Trip_Id)
INCLUDE (Stars);
