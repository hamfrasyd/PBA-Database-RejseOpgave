-- Opretter et samlet testsæt: geografi, 100 brugere, 25 grupper og 100 rejser.
-- Tilføjer også deltagere, aktiviteter, 100 venskaber, 50 opslag og 50 anmeldelser.
-- Hver kørsel tilføjer et nyt testsæt, men genbruger eksisterende geografiske data.

SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @CurrentUtc DATETIME2 = SYSUTCDATETIME();
    DECLARE @FixtureRunCode NVARCHAR(8) = LEFT(REPLACE(CONVERT(NVARCHAR(36), NEWID()), '-', ''), 8);
    DECLARE @DestinationCount INT = 20;
    DECLARE @MembersPerGroup INT = 4;
    DECLARE @GroupCount INT = 25;
    DECLARE @GroupTripCount INT = 50;
    DECLARE @CompletedTripCount INT = 60;

    DECLARE @InsertedUsers TABLE (
        UserNumber INT PRIMARY KEY,
        UserId INT UNIQUE
    );

    DECLARE @InsertedGroups TABLE (
        GroupNumber INT PRIMARY KEY,
        GroupId INT UNIQUE
    );

    DECLARE @InsertedTrips TABLE (
        TripNumber INT PRIMARY KEY,
        TripId INT UNIQUE
    );

    -- 1. Geografi: genbrug eksisterende kontinenter, lande og byer.

    DECLARE @Destinations TABLE (
        DestinationNumber INT PRIMARY KEY,
        ContinentName NVARCHAR(64),
        CountryName NVARCHAR(64),
        CityName NVARCHAR(64),
        ContinentId INT NULL,
        CountryId INT NULL,
        CityId INT NULL
    );

    INSERT INTO @Destinations (DestinationNumber, ContinentName, CountryName, CityName)
    VALUES
        (1, N'Europe', N'Denmark', N'Copenhagen'),
        (2, N'Europe', N'Denmark', N'Aarhus'),
        (3, N'Europe', N'Sweden', N'Stockholm'),
        (4, N'Europe', N'Norway', N'Oslo'),
        (5, N'Europe', N'Germany', N'Berlin'),
        (6, N'Europe', N'France', N'Paris'),
        (7, N'Europe', N'Italy', N'Rome'),
        (8, N'Europe', N'Spain', N'Barcelona'),
        (9, N'Europe', N'Portugal', N'Lisbon'),
        (10, N'Europe', N'Netherlands', N'Amsterdam'),
        (11, N'Europe', N'United Kingdom', N'London'),
        (12, N'Europe', N'Czechia', N'Prague'),
        (13, N'Asia', N'Japan', N'Tokyo'),
        (14, N'Asia', N'Thailand', N'Bangkok'),
        (15, N'Asia', N'South Korea', N'Seoul'),
        (16, N'North America', N'United States', N'New York'),
        (17, N'North America', N'Canada', N'Vancouver'),
        (18, N'South America', N'Argentina', N'Buenos Aires'),
        (19, N'Africa', N'Morocco', N'Marrakesh'),
        (20, N'Oceania', N'Australia', N'Sydney');

    INSERT INTO dbo.Continent (Name)
    SELECT DISTINCT Destination.ContinentName
    FROM @Destinations AS Destination
    WHERE NOT EXISTS (
        SELECT 1
        FROM dbo.Continent AS Continent
        WHERE Continent.Name = Destination.ContinentName
    );

    UPDATE Destination
    SET ContinentId = (
        SELECT MIN(Continent.Id)
        FROM dbo.Continent AS Continent
        WHERE Continent.Name = Destination.ContinentName
    )
    FROM @Destinations AS Destination;

    INSERT INTO dbo.Country (Continent_Id, Name)
    SELECT DISTINCT
        Destination.ContinentId,
        Destination.CountryName
    FROM @Destinations AS Destination
    WHERE NOT EXISTS (
        SELECT 1
        FROM dbo.Country AS Country
        WHERE Country.Continent_Id = Destination.ContinentId
          AND Country.Name = Destination.CountryName
    );

    UPDATE Destination
    SET CountryId = (
        SELECT MIN(Country.Id)
        FROM dbo.Country AS Country
        WHERE Country.Continent_Id = Destination.ContinentId
          AND Country.Name = Destination.CountryName
    )
    FROM @Destinations AS Destination;

    INSERT INTO dbo.City (Country_Id, Name)
    SELECT
        Destination.CountryId,
        Destination.CityName
    FROM @Destinations AS Destination
    WHERE NOT EXISTS (
        SELECT 1
        FROM dbo.City AS City
        WHERE City.Country_Id = Destination.CountryId
          AND City.Name = Destination.CityName
    );

    UPDATE Destination
    SET CityId = (
        SELECT MIN(City.Id)
        FROM dbo.City AS City
        WHERE City.Country_Id = Destination.CountryId
          AND City.Name = Destination.CityName
    )
    FROM @Destinations AS Destination;

    -- 2. Brugere: 100 brugere med sammensatte navne og forskellige hjembyer.

    DECLARE @FirstNames TABLE (
        NameNumber INT PRIMARY KEY,
        FirstName NVARCHAR(128)
    );

    DECLARE @LastNames TABLE (
        NameNumber INT PRIMARY KEY,
        LastName NVARCHAR(128)
    );

    INSERT INTO @FirstNames (NameNumber, FirstName)
    VALUES
        (1, N'Anna'), (2, N'Oliver'), (3, N'Freja'), (4, N'William'), (5, N'Emma'),
        (6, N'Noah'), (7, N'Alma'), (8, N'Oscar'), (9, N'Clara'), (10, N'Lucas'),
        (11, N'Sofia'), (12, N'Emil'), (13, N'Ida'), (14, N'Victor'), (15, N'Laura'),
        (16, N'Elias'), (17, N'Maja'), (18, N'Felix'), (19, N'Asta'), (20, N'Adam');

    INSERT INTO @LastNames (NameNumber, LastName)
    VALUES
        (1, N'Jensen'), (2, N'Nielsen'), (3, N'Hansen'), (4, N'Andersen'), (5, N'Pedersen'),
        (6, N'Christensen'), (7, N'Larsen'), (8, N'Sørensen'), (9, N'Rasmussen'), (10, N'Jørgensen'),
        (11, N'Madsen'), (12, N'Kristensen'), (13, N'Olsen'), (14, N'Thomsen'), (15, N'Christiansen'),
        (16, N'Poulsen'), (17, N'Johansen'), (18, N'Møller'), (19, N'Mortensen'), (20, N'Knudsen');

    -- Gem de faktiske IDENTITY-værdier, så vi aldrig antager, at ID starter på 1.
    DECLARE @UserNumber INT = 1;
    DECLARE @NewUserId INT;
    DECLARE @HomeCityId INT;

    WHILE @UserNumber <= 100
    BEGIN
        SELECT @HomeCityId = Destination.CityId
        FROM @Destinations AS Destination
        WHERE Destination.DestinationNumber = ((@UserNumber - 1) % @DestinationCount) + 1;

        INSERT INTO dbo.[User] (FirstName, LastName, Birthday, IsPrivate, CreatedAt, City_Id)
        SELECT
            FirstName.FirstName,
            LastName.LastName,
            DATEADD(DAY, -(@UserNumber * 73), CONVERT(DATE, '2001-09-15')),
            CASE WHEN @UserNumber % 4 = 0 THEN 1 ELSE 0 END,
            DATEADD(DAY, -(180 + @UserNumber), @CurrentUtc),
            @HomeCityId
        FROM @FirstNames AS FirstName
        CROSS JOIN @LastNames AS LastName
        WHERE FirstName.NameNumber = ((@UserNumber - 1) % 20) + 1
          AND LastName.NameNumber = (
              ((@UserNumber - 1) / 20 + (@UserNumber - 1) % 20) % 20
          ) + 1;

        SET @NewUserId = CONVERT(INT, SCOPE_IDENTITY());

        INSERT INTO @InsertedUsers (UserNumber, UserId)
        VALUES (@UserNumber, @NewUserId);

        SET @UserNumber += 1;
    END;

    -- 3. Grupper: 25 grupper med fire brugere i hver.

    DECLARE @GroupNumber INT = 1;
    DECLARE @NewGroupId INT;

    WHILE @GroupNumber <= @GroupCount
    BEGIN
        INSERT INTO dbo.[Group] (Name, Description)
        VALUES (
            CONCAT(
                CASE @GroupNumber % 5
                    WHEN 0 THEN N'Weekend explorers'
                    WHEN 1 THEN N'University friends'
                    WHEN 2 THEN N'Food and culture'
                    WHEN 3 THEN N'Hiking friends'
                    ELSE N'City break crew'
                END,
                N' ', @GroupNumber, N' [', @FixtureRunCode, N']'
            ),
            N'Friends planning shared journeys and local experiences.'
        );

        SET @NewGroupId = CONVERT(INT, SCOPE_IDENTITY());

        INSERT INTO @InsertedGroups (GroupNumber, GroupId)
        VALUES (@GroupNumber, @NewGroupId);

        SET @GroupNumber += 1;
    END;

    INSERT INTO dbo.GroupMembership (User_Id, Group_Id)
    SELECT
        InsertedUser.UserId,
        InsertedGroup.GroupId
    FROM @InsertedUsers AS InsertedUser
    JOIN @InsertedGroups AS InsertedGroup
        ON InsertedGroup.GroupNumber = ((InsertedUser.UserNumber - 1) / @MembersPerGroup) + 1;

    -- 4. Rejser: 100 rejser, hvor de første 50 er grupperejser.

    DECLARE @TripNumber INT = 1;
    DECLARE @NewTripId INT;
    DECLARE @TripStartDate DATETIME2;
    DECLARE @TripEndDate DATETIME2;

    WHILE @TripNumber <= 100
    BEGIN
        -- Rejse 1-60 ligger i fortiden; 61-100 ligger i fremtiden.
        SET @TripStartDate = DATEADD(
            DAY,
            CASE
                WHEN @TripNumber <= @CompletedTripCount THEN -210 + @TripNumber * 3
                ELSE (@TripNumber - @CompletedTripCount) * 3
            END,
            CONVERT(DATETIME2, CONVERT(DATE, @CurrentUtc))
        );

        SET @TripEndDate = DATEADD(DAY, 2 + (@TripNumber % 8), @TripStartDate);

        -- Gennemførelse sættes senere, når triggers kan finde deltagere og byer.
        INSERT INTO dbo.Trip (StartDate, EndDate, IsCompleted, ExperienceNotes, PlanNotes)
        VALUES (
            @TripStartDate,
            @TripEndDate,
            0,
            CASE
                WHEN @TripNumber <= @CompletedTripCount THEN
                    N'Enjoyed the local food and neighbourhood walks. Book popular sights in advance.'
                ELSE NULL
            END,
            CASE @TripNumber % 4
                WHEN 0 THEN N'Travel by train, visit the old town and a local museum.'
                WHEN 1 THEN N'Book central accommodation and join a guided food tour.'
                WHEN 2 THEN N'Allow one free day for exploring and a nearby day trip.'
                ELSE N'Compare flights, arrange airport transport and reserve dinner.'
            END
        );

        SET @NewTripId = CONVERT(INT, SCOPE_IDENTITY());

        INSERT INTO @InsertedTrips (TripNumber, TripId)
        VALUES (@TripNumber, @NewTripId);

        SET @TripNumber += 1;
    END;

    -- 5. Rejsedeltagere: gruppe 1-25 får hver to grupperejser; øvrige rejser er individuelle.

    INSERT INTO dbo.GroupTrip (Trip_Id, Group_Id)
    SELECT
        InsertedTrip.TripId,
        InsertedGroup.GroupId
    FROM @InsertedTrips AS InsertedTrip
    JOIN @InsertedGroups AS InsertedGroup
        ON InsertedGroup.GroupNumber = ((InsertedTrip.TripNumber - 1) % @GroupCount) + 1
    WHERE InsertedTrip.TripNumber <= @GroupTripCount;

    INSERT INTO dbo.TripParticipant (User_Id, Trip_Id)
    SELECT
        Membership.User_Id,
        InsertedTrip.TripId
    FROM @InsertedTrips AS InsertedTrip
    JOIN @InsertedGroups AS InsertedGroup
        ON InsertedGroup.GroupNumber = ((InsertedTrip.TripNumber - 1) % @GroupCount) + 1
    JOIN dbo.GroupMembership AS Membership
        ON Membership.Group_Id = InsertedGroup.GroupId
    WHERE InsertedTrip.TripNumber <= @GroupTripCount;

    INSERT INTO dbo.TripParticipant (User_Id, Trip_Id)
    SELECT
        InsertedUser.UserId,
        InsertedTrip.TripId
    FROM @InsertedTrips AS InsertedTrip
    JOIN @InsertedUsers AS InsertedUser
        ON InsertedUser.UserNumber = InsertedTrip.TripNumber - @GroupTripCount
    WHERE InsertedTrip.TripNumber > @GroupTripCount;

    -- 6. Destinationer og aktiviteter: hver rejse besøger en by, hver femte besøger to.

    INSERT INTO dbo.TripStop (City_Id, Trip_Id)
    SELECT
        Destination.CityId,
        InsertedTrip.TripId
    FROM @InsertedTrips AS InsertedTrip
    JOIN @Destinations AS Destination
        ON Destination.DestinationNumber = ((InsertedTrip.TripNumber - 1) % @DestinationCount) + 1;

    INSERT INTO dbo.TripStop (City_Id, Trip_Id)
    SELECT
        Destination.CityId,
        InsertedTrip.TripId
    FROM @InsertedTrips AS InsertedTrip
    JOIN @Destinations AS Destination
        ON Destination.DestinationNumber = (InsertedTrip.TripNumber % @DestinationCount) + 1
    WHERE InsertedTrip.TripNumber % 5 = 0;

    DECLARE @InsertedActivities TABLE (
        ActivityId INT PRIMARY KEY,
        CityId INT
    );

    INSERT INTO dbo.Activity (City_Id, Name, Description)
    OUTPUT inserted.Id, inserted.City_Id
        INTO @InsertedActivities (ActivityId, CityId)
    SELECT
        Destination.CityId,
        CONCAT(Destination.CityName, N' ', ActivityTemplate.ActivityName, N' [', @FixtureRunCode, N']'),
        ActivityTemplate.Description
    FROM @Destinations AS Destination
    CROSS JOIN (VALUES
        (N'walking tour', N'A guided walk through historic streets and local landmarks.'),
        (N'food tasting', N'A small-group introduction to regional food at local venues.')
    ) AS ActivityTemplate (ActivityName, Description);

    INSERT INTO dbo.TripActivity (Trip_Id, Activity_Id)
    SELECT
        TripStop.Trip_Id,
        InsertedActivity.ActivityId
    FROM dbo.TripStop AS TripStop
    JOIN @InsertedTrips AS InsertedTrip
        ON InsertedTrip.TripId = TripStop.Trip_Id
    JOIN @InsertedActivities AS InsertedActivity
        ON InsertedActivity.CityId = TripStop.City_Id;

    -- Nu kan achievement-triggers behandle de 60 gennemførte rejser korrekt.
    UPDATE Trip
    SET IsCompleted = 1
    FROM dbo.Trip AS Trip
    JOIN @InsertedTrips AS InsertedTrip
        ON InsertedTrip.TripId = Trip.Id
    WHERE InsertedTrip.TripNumber <= @CompletedTripCount;

    -- 7. Venskaber: 100 brugere forbindes i en ring uden gentagne par.

    INSERT INTO dbo.Friendship (UserA_Id, UserB_Id, StartDate)
    SELECT
        FirstUser.UserId,
        NextUser.UserId,
        DATEADD(DAY, -(100 + FirstUser.UserNumber), @CurrentUtc)
    FROM @InsertedUsers AS FirstUser
    JOIN @InsertedUsers AS NextUser
        ON NextUser.UserNumber = (FirstUser.UserNumber % 100) + 1;

    -- 8. Opslag: 50 opslag fordelt på de første 50 brugere.

    DECLARE @InsertedPosts TABLE (PostId INT PRIMARY KEY);

    INSERT INTO dbo.Post (User_Id, Content)
    OUTPUT inserted.Id INTO @InsertedPosts (PostId)
    SELECT
        InsertedUser.UserId,
        CONCAT(
            CASE InsertedUser.UserNumber % 5
                WHEN 0 THEN N'Looking for recommendations for '
                WHEN 1 THEN N'Just returned from a great trip to '
                WHEN 2 THEN N'My favourite travel memory this year is from '
                WHEN 3 THEN N'Planning a weekend in '
                ELSE N'Sharing a few highlights from '
            END,
            Destination.CityName,
            N'. ',
            CASE InsertedUser.UserNumber % 3
                WHEN 0 THEN N'The local food is definitely worth exploring.'
                WHEN 1 THEN N'I recommend leaving time for walking around without a fixed plan.'
                ELSE N'Next time I would book a longer stay and use public transport.'
            END
        )
    FROM @InsertedUsers AS InsertedUser
    JOIN @Destinations AS Destination
        ON Destination.DestinationNumber = ((InsertedUser.UserNumber - 1) % @DestinationCount) + 1
    WHERE InsertedUser.UserNumber <= 50;

    -- 9. Anmeldelser: 50 gennemførte rejser anmeldes af en af deres deltagere.

    DECLARE @InsertedReviews TABLE (ReviewId INT PRIMARY KEY);

    INSERT INTO dbo.Review (Trip_Id, User_Id, Content, Stars)
    OUTPUT inserted.Id INTO @InsertedReviews (ReviewId)
    SELECT
        InsertedTrip.TripId,
        Reviewer.UserId,
        CASE InsertedTrip.TripNumber % 5
            WHEN 0 THEN N'Excellent trip. The walking tour and local food were the highlights.'
            WHEN 1 THEN N'Good destination, but accommodation was more expensive than expected.'
            WHEN 2 THEN N'Well organised and easy to get around. I would happily return.'
            WHEN 3 THEN N'Enjoyable trip with a few transport delays. Allow more time between stops.'
            ELSE N'A memorable experience. Booking the activities in advance worked well.'
        END,
        CONVERT(TINYINT, 2 + (InsertedTrip.TripNumber % 5))
    FROM @InsertedTrips AS InsertedTrip
    CROSS APPLY (
        SELECT MIN(Participant.User_Id) AS UserId
        FROM dbo.TripParticipant AS Participant
        WHERE Participant.Trip_Id = InsertedTrip.TripId
    ) AS Reviewer
    WHERE InsertedTrip.TripNumber <= 50;

    -- 10. Kontroller testsættet før transaktionen gemmes.

    IF (SELECT COUNT(*) FROM @InsertedUsers) <> 100
       OR (SELECT COUNT(*) FROM @InsertedTrips) <> 100
       OR (SELECT COUNT(*) FROM @InsertedGroups) <> 25
       OR (SELECT COUNT(*) FROM @InsertedPosts) <> 50
       OR (SELECT COUNT(*) FROM @InsertedReviews) <> 50
    BEGIN
        THROW 50001, 'Fixture counts do not match the requested counts.', 1;
    END;

    IF (
        SELECT COUNT(*)
        FROM dbo.Friendship AS Friendship
        JOIN @InsertedUsers AS FirstUser ON FirstUser.UserId = Friendship.UserA_Id
        JOIN @InsertedUsers AS SecondUser ON SecondUser.UserId = Friendship.UserB_Id
    ) <> 100
    BEGIN
        THROW 50002, 'Expected 100 friendships between fixture users.', 1;
    END;

    IF EXISTS (
        SELECT 1
        FROM @InsertedTrips AS InsertedTrip
        WHERE NOT EXISTS (
            SELECT 1
            FROM dbo.TripStop AS TripStop
            WHERE TripStop.Trip_Id = InsertedTrip.TripId
        )
        OR NOT EXISTS (
            SELECT 1
            FROM dbo.TripParticipant AS Participant
            WHERE Participant.Trip_Id = InsertedTrip.TripId
        )
    )
    BEGIN
        THROW 50003, 'A fixture trip is missing a stop or participant.', 1;
    END;

    IF EXISTS (
        SELECT 1
        FROM @InsertedReviews AS InsertedReview
        JOIN dbo.Review AS Review ON Review.Id = InsertedReview.ReviewId
        WHERE NOT EXISTS (
            SELECT 1
            FROM dbo.TripParticipant AS Participant
            WHERE Participant.Trip_Id = Review.Trip_Id
              AND Participant.User_Id = Review.User_Id
        )
    )
    BEGIN
        THROW 50004, 'A reviewer is not a trip participant.', 1;
    END;

    COMMIT TRANSACTION;

    -- Resultat og ID-oversigt til manuel kontrol af testsættet.
    SELECT
        @FixtureRunCode AS FixtureRun,
        100 AS AddedUsers,
        100 AS AddedTrips,
        50 AS GroupTrips,
        50 AS SoloTrips,
        25 AS AddedGroups,
        100 AS AddedFriendships,
        50 AS AddedPosts,
        50 AS AddedReviews,
        60 AS CompletedTrips,
        40 AS PlannedTrips;

    SELECT N'User' AS Entity, UserNumber AS Seq, UserId AS Id FROM @InsertedUsers
    UNION ALL
    SELECT N'Trip', TripNumber, TripId FROM @InsertedTrips
    UNION ALL
    SELECT N'Group', GroupNumber, GroupId FROM @InsertedGroups
    ORDER BY Entity, Seq;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;

