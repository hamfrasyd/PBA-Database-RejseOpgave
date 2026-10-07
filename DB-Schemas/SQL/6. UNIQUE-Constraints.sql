-- ------------------------------------------------------------
-- Trin 6: UNIQUE-constraints

-- Både (5,10) og (10,5) bliver normaliseret til (5,10).
-- Derfor kan det samme venskab kun gemmes én gang.
ALTER TABLE dbo.Friendship
ADD CONSTRAINT UQ_Friendship_NormalizedUsers
UNIQUE (UserLow_Id, UserHigh_Id);
GO
