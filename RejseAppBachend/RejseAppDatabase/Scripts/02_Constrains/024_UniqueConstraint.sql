ALTER TABLE dbo.Friendship
ADD CONSTRAINT UQ_Friendship_NormalizedUsers
UNIQUE (UserLow_Id, UserHigh_Id);

ALTER TABLE dbo.InvitationStatus
ADD CONSTRAINT UQ_InvitationStatus_StatusName
UNIQUE(StatusName);

ALTER TABLE dbo.Achievement
ADD CONSTRAINT UQ_Achievement_Name
UNIQUE (Name);