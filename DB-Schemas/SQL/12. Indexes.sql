-- 1. Find deltagere på en bestemt rejse
CREATE NONCLUSTERED INDEX IX_TripParticipant_TripId
ON dbo.TripParticipant (Trip_Id, User_Id);
GO

-- 2. Find byerne på en bestemt rejse
CREATE NONCLUSTERED INDEX IX_TripStop_TripId
ON dbo.TripStop (Trip_Id, City_Id);
GO

-- 3. Find anmeldelser og stjerner for en rejse
CREATE NONCLUSTERED INDEX IX_Review_TripId
ON dbo.Review (Trip_Id)
INCLUDE (Stars);
GO

-- 4. Find invitationer ud fra status og dato
CREATE NONCLUSTERED INDEX IX_Invite_Status_SendDate
ON dbo.Invite (InvitationStatus_Id, SendDate);
GO
