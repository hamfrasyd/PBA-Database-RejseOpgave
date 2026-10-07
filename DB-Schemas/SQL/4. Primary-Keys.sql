-- ------------------------------------------------------------
-- Trin 4: Tilføj Primary Keys

ALTER TABLE dbo.Continent
ADD CONSTRAINT PK_Continent PRIMARY KEY (Id);
GO

ALTER TABLE dbo.Country
ADD CONSTRAINT PK_Country PRIMARY KEY (Id);
GO

ALTER TABLE dbo.City
ADD CONSTRAINT PK_City PRIMARY KEY (Id);
GO

ALTER TABLE dbo.Activity
ADD CONSTRAINT PK_Activity PRIMARY KEY (Id);
GO

ALTER TABLE dbo.Trip
ADD CONSTRAINT PK_Trip PRIMARY KEY (Id);
GO

ALTER TABLE dbo.TripStop
ADD CONSTRAINT PK_TripStop PRIMARY KEY (City_Id, Trip_Id);
GO

ALTER TABLE dbo.TripActivity
ADD CONSTRAINT PK_TripActivity PRIMARY KEY (Trip_Id, Activity_Id);
GO

ALTER TABLE dbo.[User]
ADD CONSTRAINT PK_User PRIMARY KEY (Id);
GO

ALTER TABLE dbo.TripParticipant
ADD CONSTRAINT PK_TripParticipant PRIMARY KEY (User_Id, Trip_Id);
GO

ALTER TABLE dbo.Review
ADD CONSTRAINT PK_Review PRIMARY KEY (Id);
GO

ALTER TABLE dbo.Post
ADD CONSTRAINT PK_Post PRIMARY KEY (Id);
GO

ALTER TABLE dbo.Friendship
ADD CONSTRAINT PK_Friendship PRIMARY KEY (Id);
GO

ALTER TABLE dbo.Achievement
ADD CONSTRAINT PK_Achievement PRIMARY KEY (Id);
GO

ALTER TABLE dbo.EarnedAchievement
ADD CONSTRAINT PK_EarnedAchievement PRIMARY KEY (User_Id, Achievement_Id);
GO

ALTER TABLE dbo.InvitationStatus
ADD CONSTRAINT PK_InvitationStatus PRIMARY KEY (Id);
GO

ALTER TABLE dbo.Invite
ADD CONSTRAINT PK_Invite PRIMARY KEY (Id);
GO

ALTER TABLE dbo.[Group]
ADD CONSTRAINT PK_Group PRIMARY KEY (Id);
GO

ALTER TABLE dbo.GroupMembership
ADD CONSTRAINT PK_GroupMembership PRIMARY KEY (User_Id, Group_Id);
GO

ALTER TABLE dbo.GroupTrip
ADD CONSTRAINT PK_GroupTrip PRIMARY KEY (Trip_Id, Group_Id);
GO
