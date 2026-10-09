ALTER TABLE dbo.Friendship
ADD CONSTRAINT UQ_Friendship_NormalizedUsers
UNIQUE (UserLow_Id, UserHigh_Id);
