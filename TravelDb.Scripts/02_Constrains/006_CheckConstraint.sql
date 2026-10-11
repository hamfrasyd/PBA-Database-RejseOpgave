-- Hvis begge datoer findes, må slutdatoen ikke være før startdatoen.
ALTER TABLE dbo.Trip
ADD CONSTRAINT CK_Trip_DateRange
CHECK (StartDate IS NULL OR EndDate IS NULL OR EndDate >= StartDate);

-- Review-rating skal være mellem 1 og 6.
ALTER TABLE dbo.Review
ADD CONSTRAINT CK_Review_Stars
CHECK (Stars BETWEEN 1 AND 6);

-- En bruger kan ikke være ven med sig selv.
ALTER TABLE dbo.Friendship
ADD CONSTRAINT CK_Friendship_DifferentUsers
CHECK (UserA_Id <> UserB_Id);

-- En bruger kan ikke sende en invitation til sig selv.
ALTER TABLE dbo.Invite
ADD CONSTRAINT CK_Invite_DifferentUsers
CHECK (Sender_Id <> Receiver_Id);