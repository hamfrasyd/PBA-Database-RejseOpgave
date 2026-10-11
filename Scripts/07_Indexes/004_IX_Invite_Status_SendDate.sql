-- Find invitationer ud fra status og dato
CREATE NONCLUSTERED INDEX IX_Invite_Status_SendDate
ON dbo.Invite (InvitationStatus_Id, SendDate);