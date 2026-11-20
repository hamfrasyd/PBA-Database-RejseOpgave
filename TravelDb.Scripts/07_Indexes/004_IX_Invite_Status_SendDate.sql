-- A user can only have one pending invitation per trip.
CREATE UNIQUE NONCLUSTERED INDEX UX_Invite_Pending
ON dbo.Invite (Trip_Id, Receiver_Id)
WHERE IsAccepted IS NULL;

-- Efficiently retrieve pending invitations for a user.
CREATE NONCLUSTERED INDEX IX_Invite_Receiver
ON dbo.Invite (Receiver_Id, IsAccepted);