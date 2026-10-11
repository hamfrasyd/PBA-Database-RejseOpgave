ALTER TABLE dbo.Country
ADD CONSTRAINT FK_Country_Continent_ContinentId
FOREIGN KEY (Continent_Id) REFERENCES dbo.Continent(Id);

ALTER TABLE dbo.City
ADD CONSTRAINT FK_City_Country_CountryId
FOREIGN KEY (Country_Id) REFERENCES dbo.Country(Id);

ALTER TABLE dbo.Activity
ADD CONSTRAINT FK_Activity_City_CityId
FOREIGN KEY (City_Id) REFERENCES dbo.City(Id);

ALTER TABLE dbo.[User]
ADD CONSTRAINT FK_User_City_CityId
FOREIGN KEY (City_Id) REFERENCES dbo.City(Id);

ALTER TABLE dbo.TripStop
ADD CONSTRAINT FK_TripStop_City_CityId
FOREIGN KEY (City_Id) REFERENCES dbo.City(Id);

-- Sletter alle stop på rejsen, når selve rejsen slettes.
ALTER TABLE dbo.TripStop
ADD CONSTRAINT FK_TripStop_Trip_TripId
FOREIGN KEY (Trip_Id) REFERENCES dbo.Trip(Id) ON DELETE CASCADE;

-- Sletter rejsens planlagte aktiviteter, når rejsen slettes.
ALTER TABLE dbo.TripActivity
ADD CONSTRAINT FK_TripActivity_Trip_TripId
FOREIGN KEY (Trip_Id) REFERENCES dbo.Trip(Id) ON DELETE CASCADE;

ALTER TABLE dbo.TripActivity
ADD CONSTRAINT FK_TripActivity_Activity_ActivityId
FOREIGN KEY (Activity_Id) REFERENCES dbo.Activity(Id);

-- Fjerner brugeren fra alle rejser, når brugerkontoen slettes
ALTER TABLE dbo.TripParticipant
ADD CONSTRAINT FK_TripParticipant_User_UserId
FOREIGN KEY (User_Id) REFERENCES dbo.[User](Id) ON DELETE CASCADE;

-- Fjerner alle deltagere fra rejsen, når rejsen slettes
ALTER TABLE dbo.TripParticipant
ADD CONSTRAINT FK_TripParticipant_Trip_TripId
FOREIGN KEY (Trip_Id) REFERENCES dbo.Trip(Id) ON DELETE CASCADE;

-- Sletter anmeldelser af rejsen, når rejsen slettes.
ALTER TABLE dbo.Review
ADD CONSTRAINT FK_Review_Trip_TripId
FOREIGN KEY (Trip_Id) REFERENCES dbo.Trip(Id) ON DELETE CASCADE;

ALTER TABLE dbo.Review
ADD CONSTRAINT FK_Review_User_UserId
FOREIGN KEY (User_Id) REFERENCES dbo.[User](Id);

ALTER TABLE dbo.Post
ADD CONSTRAINT FK_Post_User_UserId
FOREIGN KEY (User_Id) REFERENCES dbo.[User](Id);

ALTER TABLE dbo.Friendship
ADD CONSTRAINT FK_Friendship_User_UserAId
FOREIGN KEY (UserA_Id) REFERENCES dbo.[User](Id);

ALTER TABLE dbo.Friendship
ADD CONSTRAINT FK_Friendship_User_UserBId
FOREIGN KEY (UserB_Id) REFERENCES dbo.[User](Id);

-- Sletter brugerens optjente achievements, når brugerkontoen slettes.
ALTER TABLE dbo.EarnedAchievement
ADD CONSTRAINT FK_EarnedAchievement_User_UserId
FOREIGN KEY (User_Id) REFERENCES dbo.[User](Id) ON DELETE CASCADE;

-- Fjerner achievementen hos alle brugere, når achievement-definitionen slettes.
ALTER TABLE dbo.EarnedAchievement
ADD CONSTRAINT FK_EarnedAchievement_Achievement_AchievementId
FOREIGN KEY (Achievement_Id) REFERENCES dbo.Achievement(Id) ON DELETE CASCADE;

-- Sletter invitationer til rejsen, når rejsen slettes.
ALTER TABLE dbo.Invite
ADD CONSTRAINT FK_Invite_Trip_TripId
FOREIGN KEY (Trip_Id) REFERENCES dbo.Trip(Id) ON DELETE CASCADE;

ALTER TABLE dbo.Invite
ADD CONSTRAINT FK_Invite_User_ReceiverId
FOREIGN KEY (Receiver_Id) REFERENCES dbo.[User](Id);

ALTER TABLE dbo.Invite
ADD CONSTRAINT FK_Invite_User_SenderId
FOREIGN KEY (Sender_Id) REFERENCES dbo.[User](Id);

ALTER TABLE dbo.Invite
ADD CONSTRAINT FK_Invite_InvitationStatus_InvitationStatusId
FOREIGN KEY (InvitationStatus_Id) REFERENCES dbo.InvitationStatus(Id);

ALTER TABLE dbo.GroupMembership
ADD CONSTRAINT FK_GroupMembership_User_UserId
FOREIGN KEY (User_Id) REFERENCES dbo.[User](Id) ON DELETE CASCADE;

ALTER TABLE dbo.GroupMembership
ADD CONSTRAINT FK_GroupMembership_Group_GroupId
FOREIGN KEY (Group_Id) REFERENCES dbo.[Group](Id) ON DELETE CASCADE;

ALTER TABLE dbo.GroupTrip
ADD CONSTRAINT FK_GroupTrip_Trip_TripId
FOREIGN KEY (Trip_Id) REFERENCES dbo.Trip(Id) ON DELETE CASCADE;

ALTER TABLE dbo.GroupTrip
ADD CONSTRAINT FK_GroupTrip_Group_GroupId
FOREIGN KEY (Group_Id) REFERENCES dbo.[Group](Id) ON DELETE CASCADE;