/*
    Opbygning:
    1. Forbered kørslen og geografiske referencedata
    2. Opret 100 brugere
    3. Opret 25 grupper og gruppemedlemskaber
    4. Opret 100 rejser og tilknyt deltagere
    5. Tilføj byer og aktiviteter til rejserne
    6. Opret 100 venskaber
    7. Opret 50 opslag og 50 anmeldelser
    8. Kontrollér testdata og afslut transaktionen

    Noter:
    - Forbind direkte til TravelDb, før scriptet køres.
    - Hver kørsel tilføjer et nyt datasæt. Eksisterende data bevares.
    - Kontinenter, lande og byer genbruges, når navn og overordnet område matcher.
    - De faktiske IDENTITY-værdier gemmes og bruges til relationerne.
    - Grupper og aktiviteter får en fælles kørselskode i navnet, fx [B1197DE0].
    - Der oprettes 50 grupperejser og 50 individuelle rejser.
    - 60 rejser er gennemførte, og 40 er planlagte.
    - Anmeldelser bruger skalaen 1-6 fra databasens CHECK-constraint.
    - Ved fejl rulles transaktionen tilbage.
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @Now datetime2 = SYSUTCDATETIME();
    DECLARE @Run nvarchar(8) = LEFT(REPLACE(CONVERT(nvarchar(36), NEWID()), '-', ''), 8);
    DECLARE @N table (N int PRIMARY KEY);
    ;WITH Numbers AS (
        SELECT 1 AS N
        UNION ALL SELECT N + 1 FROM Numbers WHERE N < 100
    )
    INSERT @N SELECT N FROM Numbers OPTION (MAXRECURSION 100);

    -- ------------------------------------------------------------
    -- Trin 1: Forbered geografiske referencedata
    -- Genbrug eksisterende kontinenter, lande og byer ud fra navn og overordnet område.
    DECLARE @Geo table (
        Seq int PRIMARY KEY, ContinentName nvarchar(64),
        CountryName nvarchar(64), CityName nvarchar(64),
        ContinentId int NULL, CountryId int NULL, CityId int NULL
    );
    INSERT @Geo (Seq, ContinentName, CountryName, CityName) VALUES
    (1,N'Europe',N'Denmark',N'Copenhagen'),
    (2,N'Europe',N'Denmark',N'Aarhus'),
    (3,N'Europe',N'Sweden',N'Stockholm'),
    (4,N'Europe',N'Norway',N'Oslo'),
    (5,N'Europe',N'Germany',N'Berlin'),
    (6,N'Europe',N'France',N'Paris'),
    (7,N'Europe',N'Italy',N'Rome'),
    (8,N'Europe',N'Spain',N'Barcelona'),
    (9,N'Europe',N'Portugal',N'Lisbon'),
    (10,N'Europe',N'Netherlands',N'Amsterdam'),
    (11,N'Europe',N'United Kingdom',N'London'),
    (12,N'Europe',N'Czechia',N'Prague'),
    (13,N'Asia',N'Japan',N'Tokyo'),
    (14,N'Asia',N'Thailand',N'Bangkok'),
    (15,N'Asia',N'South Korea',N'Seoul'),
    (16,N'North America',N'United States',N'New York'),
    (17,N'North America',N'Canada',N'Vancouver'),
    (18,N'South America',N'Argentina',N'Buenos Aires'),
    (19,N'Africa',N'Morocco',N'Marrakesh'),
    (20,N'Oceania',N'Australia',N'Sydney');

    INSERT dbo.Continent (Name)
    SELECT DISTINCT g.ContinentName FROM @Geo g
    WHERE NOT EXISTS (SELECT 1 FROM dbo.Continent c WHERE c.Name = g.ContinentName);
    UPDATE g SET ContinentId = (
        SELECT MIN(c.Id) FROM dbo.Continent c WHERE c.Name = g.ContinentName
    ) FROM @Geo g;

    INSERT dbo.Country (Continent_Id, Name)
    SELECT DISTINCT g.ContinentId, g.CountryName FROM @Geo g
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.Country c
        WHERE c.Name = g.CountryName AND c.Continent_Id = g.ContinentId
    );
    UPDATE g SET CountryId = (
        SELECT MIN(c.Id) FROM dbo.Country c
        WHERE c.Name = g.CountryName AND c.Continent_Id = g.ContinentId
    ) FROM @Geo g;

    INSERT dbo.City (Country_Id, Name)
    SELECT g.CountryId, g.CityName FROM @Geo g
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.City c WHERE c.Name = g.CityName AND c.Country_Id = g.CountryId
    );
    UPDATE g SET CityId = (
        SELECT MIN(c.Id) FROM dbo.City c
        WHERE c.Name = g.CityName AND c.Country_Id = g.CountryId
    ) FROM @Geo g;

    -- ------------------------------------------------------------
    -- Trin 2: Opret 100 brugere
    -- Navnene sammensættes fra nedenstående lister.
    DECLARE @First table (N int PRIMARY KEY, Name nvarchar(128));
    DECLARE @Last table (N int PRIMARY KEY, Name nvarchar(128));
    INSERT @First VALUES
    (1,N'Anna'),(2,N'Oliver'),(3,N'Freja'),(4,N'William'),(5,N'Emma'),
    (6,N'Noah'),(7,N'Alma'),(8,N'Oscar'),(9,N'Clara'),(10,N'Lucas'),
    (11,N'Sofia'),(12,N'Emil'),(13,N'Ida'),(14,N'Victor'),(15,N'Laura'),
    (16,N'Elias'),(17,N'Maja'),(18,N'Felix'),(19,N'Asta'),(20,N'Adam');
    INSERT @Last VALUES
    (1,N'Jensen'),(2,N'Nielsen'),(3,N'Hansen'),(4,N'Andersen'),(5,N'Pedersen'),
    (6,N'Christensen'),(7,N'Larsen'),(8,N'Sørensen'),(9,N'Rasmussen'),(10,N'Jørgensen'),
    (11,N'Madsen'),(12,N'Kristensen'),(13,N'Olsen'),(14,N'Thomsen'),(15,N'Christiansen'),
    (16,N'Poulsen'),(17,N'Johansen'),(18,N'Møller'),(19,N'Mortensen'),(20,N'Knudsen');

    -- Gem hvert genereret ID med det samme. ID-værdierne behøver ikke begynde ved 1.
    DECLARE @Users table (Seq int PRIMARY KEY, Id int UNIQUE);
    DECLARE @Trips table (Seq int PRIMARY KEY, Id int UNIQUE, StartDate datetime2, EndDate datetime2);
    DECLARE @Groups table (Seq int PRIMARY KEY, Id int UNIQUE);
    DECLARE @i int = 1, @Id int, @CityId int;
    WHILE @i <= 100
    BEGIN
        SELECT @CityId = CityId FROM @Geo WHERE Seq = ((@i - 1) % 20) + 1;
        INSERT dbo.[User] (FirstName,LastName,Birthday,IsPrivate,CreatedAt,City_Id)
        SELECT f.Name,l.Name,DATEADD(DAY, -(@i * 73), CONVERT(date,'2001-09-15')),
               CASE WHEN @i % 4 = 0 THEN 1 ELSE 0 END,
               DATEADD(DAY,-(180 + @i),@Now),@CityId
        FROM @First f CROSS JOIN @Last l
        WHERE f.N = ((@i - 1) % 20) + 1
          AND l.N = (((@i - 1) / 20 + (@i - 1) % 20) % 20) + 1;
        SET @Id = CONVERT(int,SCOPE_IDENTITY());
        INSERT @Users VALUES (@i,@Id);
        SET @i += 1;
    END;

    -- ------------------------------------------------------------
    -- Trin 3.A: Opret 25 grupper
    SET @i = 1;
    WHILE @i <= 25
    BEGIN
        INSERT dbo.[Group] (Name,Description)
        VALUES (
            CONCAT(CASE @i % 5 WHEN 0 THEN N'Weekend explorers'
                WHEN 1 THEN N'University friends' WHEN 2 THEN N'Food and culture'
                WHEN 3 THEN N'Hiking friends' ELSE N'City break crew' END,
                N' ',@i,N' [',@Run,N']'),
            N'Friends planning shared journeys and local experiences.'
        );
        SET @Id = CONVERT(int,SCOPE_IDENTITY());
        INSERT @Groups VALUES (@i,@Id);
        SET @i += 1;
    END;

    -- ------------------------------------------------------------
    -- Trin 3.B: Tilknyt gruppemedlemmer
    -- Hver gruppe får fire medlemmer. Alle 100 brugere tilhører én gruppe.
    INSERT dbo.GroupMembership (User_Id,Group_Id)
    SELECT u.Id,g.Id FROM @Users u
    JOIN @Groups g ON g.Seq = ((u.Seq - 1) / 4) + 1;

    -- ------------------------------------------------------------
    -- Trin 4.A: Opret 100 rejser
    DECLARE @Start datetime2, @End datetime2;
    SET @i = 1;
    WHILE @i <= 100
    BEGIN
        -- De første 60 rejser ligger i fortiden. De resterende 40 ligger i fremtiden.
        SET @Start = DATEADD(DAY,
            CASE WHEN @i <= 60 THEN -210 + @i * 3 ELSE (@i - 60) * 3 END,
            CONVERT(datetime2,CONVERT(date,@Now)));
        SET @End = DATEADD(DAY,2 + (@i % 8),@Start);
        -- Rejserne markeres først som gennemførte, når deltagere og byer er tilføjet.
        -- Dermed kan eventuelle triggers bruge disse relationer ved opdateringen.
        INSERT dbo.Trip (StartDate,EndDate,IsCompleted,ExperienceNotes,PlanNotes)
        VALUES (@Start,@End,0,
            CASE WHEN @i <= 60 THEN
                N'Enjoyed the local food and neighbourhood walks. Book popular sights in advance.'
                ELSE NULL END,
            CASE @i % 4 WHEN 0 THEN N'Travel by train, visit the old town and a local museum.'
                WHEN 1 THEN N'Book central accommodation and join a guided food tour.'
                WHEN 2 THEN N'Allow one free day for exploring and a nearby day trip.'
                ELSE N'Compare flights, arrange airport transport and reserve dinner.' END);
        SET @Id = CONVERT(int,SCOPE_IDENTITY());
        INSERT @Trips VALUES (@i,@Id,@Start,@End);
        SET @i += 1;
    END;

    -- ------------------------------------------------------------
    -- Trin 4.B: Tilknyt grupper og deltagere
    -- Rejse nr. 1-50: To rejser pr. gruppe med alle fire medlemmer som deltagere.
    -- Numrene er interne løbenumre i testdata og ikke databasens ID-værdier.
    INSERT dbo.GroupTrip (Trip_Id,Group_Id)
    SELECT t.Id,g.Id FROM @Trips t
    JOIN @Groups g ON g.Seq = ((t.Seq - 1) % 25) + 1
    WHERE t.Seq <= 50;
    INSERT dbo.TripParticipant (User_Id,Trip_Id)
    SELECT m.User_Id,t.Id FROM @Trips t
    JOIN @Groups g ON g.Seq = ((t.Seq - 1) % 25) + 1
    JOIN dbo.GroupMembership m ON m.Group_Id = g.Id
    WHERE t.Seq <= 50;
    -- Rejse nr. 51-100 er individuelle rejser uden tilknytning til en gruppe.
    INSERT dbo.TripParticipant (User_Id,Trip_Id)
    SELECT u.Id,t.Id FROM @Trips t JOIN @Users u ON u.Seq = t.Seq - 50
    WHERE t.Seq > 50;

    -- ------------------------------------------------------------
    -- Trin 5.A: Tilknyt byer til rejserne
    -- Hver rejse får én by. Hver femte rejse får desuden en anden by.
    INSERT dbo.TripStop (City_Id,Trip_Id)
    SELECT g.CityId,t.Id FROM @Trips t
    JOIN @Geo g ON g.Seq = ((t.Seq - 1) % 20) + 1;
    INSERT dbo.TripStop (City_Id,Trip_Id)
    SELECT g.CityId,t.Id FROM @Trips t
    JOIN @Geo g ON g.Seq = (t.Seq % 20) + 1
    WHERE t.Seq % 5 = 0;

    -- ------------------------------------------------------------
    -- Trin 5.B: Opret og tilknyt aktiviteter
    -- Opret to lokale aktiviteter pr. by med kørselskoden i navnet.
    DECLARE @Activities table (Id int PRIMARY KEY,CityId int);
    INSERT dbo.Activity (City_Id,Name,Description)
    OUTPUT inserted.Id,inserted.City_Id INTO @Activities
    SELECT g.CityId,
           CONCAT(g.CityName,N' ',v.Label,N' [',@Run,N']'),v.Description
    FROM @Geo g CROSS JOIN (VALUES
        (N'walking tour',N'A guided walk through historic streets and local landmarks.'),
        (N'food tasting',N'A small-group introduction to regional food at local venues.')
    ) v(Label,Description);
    INSERT dbo.TripActivity (Trip_Id,Activity_Id)
    SELECT stop.Trip_Id,a.Id FROM dbo.TripStop stop
    JOIN @Trips t ON t.Id = stop.Trip_Id
    JOIN @Activities a ON a.CityId = stop.City_Id;

    -- ------------------------------------------------------------
    -- Trin 5.C: Markér de 60 tidligere rejser som gennemførte
    UPDATE trip SET IsCompleted = 1 FROM dbo.Trip trip
    JOIN @Trips t ON t.Id = trip.Id WHERE t.Seq <= 60;

    -- ------------------------------------------------------------
    -- Trin 6: Opret 100 venskaber
    -- Brugerne forbindes i en ring: 1 med 2, 2 med 3 osv. og til sidst 100 med 1.
    -- Hvert brugerpar oprettes kun én gang.
    INSERT dbo.Friendship (UserA_Id,UserB_Id,StartDate)
    SELECT a.Id,b.Id,DATEADD(DAY,-(100 + a.Seq),@Now)
    FROM @Users a JOIN @Users b ON b.Seq = (a.Seq % 100) + 1;

    -- ------------------------------------------------------------
    -- Trin 7.A: Opret 50 opslag
    DECLARE @PostIds table (Id int PRIMARY KEY);
    INSERT dbo.Post (User_Id,Content)
    OUTPUT inserted.Id INTO @PostIds
    SELECT u.Id,CONCAT(
        CASE u.Seq % 5 WHEN 0 THEN N'Looking for recommendations for '
            WHEN 1 THEN N'Just returned from a great trip to '
            WHEN 2 THEN N'My favourite travel memory this year is from '
            WHEN 3 THEN N'Planning a weekend in ' ELSE N'Sharing a few highlights from ' END,
        geo.CityName,N'. ',
        CASE u.Seq % 3 WHEN 0 THEN N'The local food is definitely worth exploring.'
            WHEN 1 THEN N'I recommend leaving time for walking around without a fixed plan.'
            ELSE N'Next time I would book a longer stay and use public transport.' END)
    FROM @Users u JOIN @Geo geo ON geo.Seq = ((u.Seq - 1) % 20) + 1
    WHERE u.Seq <= 50;

    -- ------------------------------------------------------------
    -- Trin 7.B: Opret 50 anmeldelser
    -- Hver anmeldelse gælder en gennemført rejse og skrives af en deltager på rejsen.
    DECLARE @ReviewIds table (Id int PRIMARY KEY);
    INSERT dbo.Review (Trip_Id,User_Id,Content,Stars)
    OUTPUT inserted.Id INTO @ReviewIds
    SELECT t.Id,p.User_Id,
        CASE t.Seq % 5 WHEN 0 THEN N'Excellent trip. The walking tour and local food were the highlights.'
            WHEN 1 THEN N'Good destination, but accommodation was more expensive than expected.'
            WHEN 2 THEN N'Well organised and easy to get around. I would happily return.'
            WHEN 3 THEN N'Enjoyable trip with a few transport delays. Allow more time between stops.'
            ELSE N'A memorable experience. Booking the activities in advance worked well.' END,
        CONVERT(tinyint,2 + (t.Seq % 5))
    FROM @Trips t CROSS APPLY (
        SELECT MIN(tp.User_Id) AS User_Id FROM dbo.TripParticipant tp WHERE tp.Trip_Id = t.Id
    ) p WHERE t.Seq <= 50;

    -- ------------------------------------------------------------
    -- Trin 8.A: Kontrollér antal og relationer
    -- Afbryd kørslen og rul tilbage, hvis kontrollen finder en fejl.
    IF (SELECT COUNT(*) FROM @Users) <> 100 OR (SELECT COUNT(*) FROM @Trips) <> 100
       OR (SELECT COUNT(*) FROM @Groups) <> 25 OR (SELECT COUNT(*) FROM @PostIds) <> 50
       OR (SELECT COUNT(*) FROM @ReviewIds) <> 50
        THROW 50001,'Fixture counts do not match the requested counts.',1;
    IF (SELECT COUNT(*) FROM dbo.Friendship f
        JOIN @Users a ON a.Id=f.UserA_Id JOIN @Users b ON b.Id=f.UserB_Id) <> 100
        THROW 50002,'Expected 100 friendships between fixture users.',1;
    IF EXISTS (SELECT 1 FROM @Trips t WHERE
        NOT EXISTS (SELECT 1 FROM dbo.TripStop s WHERE s.Trip_Id=t.Id)
        OR NOT EXISTS (SELECT 1 FROM dbo.TripParticipant p WHERE p.Trip_Id=t.Id))
        THROW 50003,'A fixture trip is missing a stop or participant.',1;
    IF EXISTS (SELECT 1 FROM @ReviewIds r JOIN dbo.Review rv ON rv.Id=r.Id
        WHERE NOT EXISTS (SELECT 1 FROM dbo.TripParticipant p
                          WHERE p.Trip_Id=rv.Trip_Id AND p.User_Id=rv.User_Id))
        THROW 50004,'A reviewer is not a trip participant.',1;

    -- ------------------------------------------------------------
    -- Trin 8.B: Gem ændringerne og vis resultatet
    COMMIT TRANSACTION;
    SELECT @Run AS FixtureRun,100 AS AddedUsers,100 AS AddedTrips,
        50 AS GroupTrips,50 AS SoloTrips,25 AS AddedGroups,
        100 AS AddedFriendships,50 AS AddedPosts,50 AS AddedReviews,
        60 AS CompletedTrips,40 AS PlannedTrips;
    -- Gem dette resultat, hvis de indsatte brugere, rejser og grupper skal findes senere.
    SELECT N'User' AS Entity,Seq,Id FROM @Users
    UNION ALL SELECT N'Trip',Seq,Id FROM @Trips
    UNION ALL SELECT N'Group',Seq,Id FROM @Groups
    ORDER BY Entity,Seq;
END TRY
BEGIN CATCH
    -- Rul en aktiv transaktion tilbage, og send fejlen videre til SQL-klienten.
    IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;

