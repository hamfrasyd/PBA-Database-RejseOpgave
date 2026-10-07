-- ------------------------------------------------------------
-- Trin 5: Tilføj Foreign Keys og udvalgte cascade deletes

ALTER TABLE dbo.Country
ADD CONSTRAINT FK_Country_Continent_ContinentId
FOREIGN KEY (Continent_Id) REFERENCES dbo.Continent(Id);
GO

ALTER TABLE dbo.City
ADD CONSTRAINT FK_City_Country_CountryId
FOREIGN KEY (Country_Id) REFERENCES dbo.Country(Id);
GO

ALTER TABLE dbo.Activity
ADD CONSTRAINT FK_Activity_City_CityId
FOREIGN KEY (City_Id) REFERENCES dbo.City(Id);
GO

ALTER TABLE dbo.[User]
ADD CONSTRAINT FK_User_City_CityId
FOREIGN KEY (City_Id) REFERENCES dbo.City(Id);
GO

-- TripStop afhænger af Trip. Sletning af City cascader ikke,
-- fordi historiske TripStop-data ikke skal forsvinde automatisk ved sletning af referencedata.
ALTER TABLE dbo.TripStop
ADD CONSTRAINT FK_TripStop_City_CityId
FOREIGN KEY (City_Id) REFERENCES dbo.City(Id);
GO

ALTER TABLE dbo.TripStop
ADD CONSTRAINT FK_TripStop_Trip_TripId
FOREIGN KEY (Trip_Id) REFERENCES dbo.Trip(Id) ON DELETE CASCADE;
GO

-- TripActivity afhænger af Trip. Sletning af Activity ændrer ikke automatisk historikken for en Trip.
ALTER TABLE dbo.TripActivity
ADD CONSTRAINT FK_TripActivity_Trip_TripId
FOREIGN KEY (Trip_Id) REFERENCES dbo.Trip(Id) ON DELETE CASCADE;
GO

ALTER TABLE dbo.TripActivity
ADD CONSTRAINT FK_TripActivity_Activity_ActivityId
FOREIGN KEY (Activity_Id) REFERENCES dbo.Activity(Id);
GO

-- TripParticipant er kun en relation og giver ikke mening, hvis User eller Trip ikke længere findes.
ALTER TABLE dbo.TripParticipant
ADD CONSTRAINT FK_TripParticipant_User_UserId
FOREIGN KEY (User_Id) REFERENCES dbo.[User](Id) ON DELETE CASCADE;
GO

ALTER TABLE dbo.TripParticipant
ADD CONSTRAINT FK_TripParticipant_Trip_TripId
FOREIGN KEY (Trip_Id) REFERENCES dbo.Trip(Id) ON DELETE CASCADE;
GO

-- Et Review kan ikke eksistere uden sin Trip, så reviews slettes med Trip.
-- (Skal diskusteres... -> Sletning af User cascader ikke,  fordi det er en forretningsregel, om brugerens indhold skal bevares eller slettes).
ALTER TABLE dbo.Review
ADD CONSTRAINT FK_Review_Trip_TripId
FOREIGN KEY (Trip_Id) REFERENCES dbo.Trip(Id) ON DELETE CASCADE;
GO

ALTER TABLE dbo.Review
ADD CONSTRAINT FK_Review_User_UserId
FOREIGN KEY (User_Id) REFERENCES dbo.[User](Id);
GO

-- Post bruger NO ACTION, indtil regler for sletning af brugere og indhold er besluttet.
ALTER TABLE dbo.Post
ADD CONSTRAINT FK_Post_User_UserId
FOREIGN KEY (User_Id) REFERENCES dbo.[User](Id);
GO

-- Friendship cascader ikke fra User, fordi to foreign keys peger på samme tabel.
-- Oprydning kan i stedet håndteres af applikationen, når en bruger slettes.
ALTER TABLE dbo.Friendship
ADD CONSTRAINT FK_Friendship_User_UserAId
FOREIGN KEY (UserA_Id) REFERENCES dbo.[User](Id);
GO

ALTER TABLE dbo.Friendship
ADD CONSTRAINT FK_Friendship_User_UserBId
FOREIGN KEY (UserB_Id) REFERENCES dbo.[User](Id);
GO

-- EarnedAchievement er kun en relation og slettes, hvis en af de tilknyttede rækker slettes.
ALTER TABLE dbo.EarnedAchievement
ADD CONSTRAINT FK_EarnedAchievement_User_UserId
FOREIGN KEY (User_Id) REFERENCES dbo.[User](Id) ON DELETE CASCADE;
GO

ALTER TABLE dbo.EarnedAchievement
ADD CONSTRAINT FK_EarnedAchievement_Achievement_AchievementId
FOREIGN KEY (Achievement_Id) REFERENCES dbo.Achievement(Id) ON DELETE CASCADE;
GO

-- Et Invite hører til en Trip og slettes derfor sammen med den.
-- Sender og Receiver bruger NO ACTION, fordi begge peger på [User].
ALTER TABLE dbo.Invite
ADD CONSTRAINT FK_Invite_Trip_TripId
FOREIGN KEY (Trip_Id) REFERENCES dbo.Trip(Id) ON DELETE CASCADE;
GO

ALTER TABLE dbo.Invite
ADD CONSTRAINT FK_Invite_User_ReceiverId
FOREIGN KEY (Receiver_Id) REFERENCES dbo.[User](Id);
GO

ALTER TABLE dbo.Invite
ADD CONSTRAINT FK_Invite_User_SenderId
FOREIGN KEY (Sender_Id) REFERENCES dbo.[User](Id);
GO

ALTER TABLE dbo.Invite
ADD CONSTRAINT FK_Invite_InvitationStatus_InvitationStatusId
FOREIGN KEY (InvitationStatus_Id) REFERENCES dbo.InvitationStatus(Id);
GO

-- GroupMembership er kun en relation og slettes med User eller Group.
ALTER TABLE dbo.GroupMembership
ADD CONSTRAINT FK_GroupMembership_User_UserId
FOREIGN KEY (User_Id) REFERENCES dbo.[User](Id) ON DELETE CASCADE;
GO

ALTER TABLE dbo.GroupMembership
ADD CONSTRAINT FK_GroupMembership_Group_GroupId
FOREIGN KEY (Group_Id) REFERENCES dbo.[Group](Id) ON DELETE CASCADE;
GO

-- GroupTrip er kun en relation og slettes med Trip eller Group.
ALTER TABLE dbo.GroupTrip
ADD CONSTRAINT FK_GroupTrip_Trip_TripId
FOREIGN KEY (Trip_Id) REFERENCES dbo.Trip(Id) ON DELETE CASCADE;
GO

ALTER TABLE dbo.GroupTrip
ADD CONSTRAINT FK_GroupTrip_Group_GroupId
FOREIGN KEY (Group_Id) REFERENCES dbo.[Group](Id) ON DELETE CASCADE;
GO
