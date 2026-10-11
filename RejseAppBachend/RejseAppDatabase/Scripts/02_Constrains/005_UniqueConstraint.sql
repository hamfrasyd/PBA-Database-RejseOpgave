-- Både (5,10) og (10,5) bliver normaliseret til (5,10), grundet Computed Column.
-- Derfor kan det samme venskab kun gemmes én gang.
ALTER TABLE dbo.Friendship
ADD CONSTRAINT UQ_Friendship_NormalizedUsers
UNIQUE (UserLow_Id, UserHigh_Id);

-- Unik statusnavn for invitationstatus.
ALTER TABLE dbo.InvitationStatus
ADD CONSTRAINT UQ_InvitationStatus_StatusName
UNIQUE(StatusName);

-- Unik navn for achievement.
ALTER TABLE dbo.Achievement
ADD CONSTRAINT UQ_Achievement_Name
UNIQUE (Name);