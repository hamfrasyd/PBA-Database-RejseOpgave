-- Find brugere på en bestemt rejse
CREATE NONCLUSTERED INDEX IX_TripParticipant_TripId
ON dbo.TripParticipant (Trip_Id, User_Id);