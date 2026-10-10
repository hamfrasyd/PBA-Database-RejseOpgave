/*
    Tilføjer en anmeldelse til en rejse.
    Regler:
    - Rejsen skal eksistere.
    - Brugeren skal eksistere.
    - Brugeren skal være deltager på rejsen.
    - Rejsen skal være gennemført.
    - Stars skal være mellem 1 og 6.
*/

CREATE OR ALTER PROCEDURE dbo.AddReview
    @TripId INT,
    @UserId INT,
    @Content NVARCHAR(MAX),
    @Stars TINYINT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    -- Kontrollér at rejsen findes
    IF NOT EXISTS (SELECT 1 FROM dbo.Trip WHERE Id = @TripId)
    BEGIN
        THROW 51001, N'Rejsen findes ikke.', 1;
    END;

    -- Kontrollér at brugeren findes
    IF NOT EXISTS (SELECT 1 FROM dbo.[User] WHERE Id = @UserId)
    BEGIN
        THROW 51002, N'Brugeren findes ikke.', 1;
    END;

    -- Kontrollér at brugeren deltager på rejsen
    IF NOT EXISTS (SELECT 1 FROM dbo.TripParticipant WHERE Trip_Id = @TripId AND User_Id = @UserId)
    BEGIN
        THROW 51003,
              N'Brugeren skal være deltager på rejsen for at anmelde den.', 1;
    END;

    -- Kontrollér at rejsen er gennemført
    IF NOT EXISTS (SELECT 1 FROM dbo.Trip WHERE Id = @TripId AND IsCompleted = 1)
    BEGIN
        THROW 51004,
              N'Rejsen skal være gennemført før den kan anmeldes.', 1;
    END;

    -- Kontrollér rating
    IF @Stars < 1 OR @Stars > 6
    BEGIN
        THROW 51005,
              N'Antal stjerner skal være mellem 1 og 6.',
              1;
    END;

    INSERT INTO dbo.Review (Trip_Id, User_Id, Content, Stars)
    VALUES (@TripId, @UserId, @Content, @Stars);
END;

--EXEC dbo.AddReview
--    @TripId = 1,
--    @UserId = 1,
--    @Content = N'En rigtig god rejseeeeeeeeeeeeeeeeeeeeeeeeeee.',
--    @Stars = 5;
