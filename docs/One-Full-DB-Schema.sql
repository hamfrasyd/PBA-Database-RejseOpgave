-- 00_CreateDb

-- Opret databasen, hvis den ikke allerede findes
USE [master];
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.databases
    WHERE name = N'TravelDb'
)
BEGIN
    PRINT 'TravelDb findes ikke. Opretter databasen...';
    CREATE DATABASE [TravelDb];
END
ELSE
BEGIN
    PRINT 'TravelDb findes allerede. Fortsætter...';
END;
GO

USE [TravelDb];
GO


-- 01_Setup

CREATE TABLE dbo.Continent (
Id INT IDENTITY(1,1),
Name NVARCHAR(64)
);

CREATE TABLE dbo.Country (
    Id INT IDENTITY(1,1),
    Continent_Id INT,
    Name NVARCHAR(64)
);

CREATE TABLE dbo.City (
    Id INT IDENTITY(1,1),
    Country_Id INT,
    Name NVARCHAR(64)
);

CREATE TABLE dbo.Activity (
    Id INT IDENTITY(1,1),
    City_Id INT,
    Name NVARCHAR(128),
    Description NVARCHAR(1024)
);

CREATE TABLE dbo.Trip (
    Id INT IDENTITY(1,1),
    StartDate DATETIME2,
    EndDate DATETIME2,
    IsCompleted BIT,
    ExperienceNotes NVARCHAR(1024),
    PlanNotes NVARCHAR(1024)
);

CREATE TABLE dbo.TripStop (
    City_Id INT,
    Trip_Id INT
);

CREATE TABLE dbo.TripActivity (
    Trip_Id INT,
    Activity_Id INT
);

CREATE TABLE dbo.[User] (
    Id INT IDENTITY(1,1),
    FirstName NVARCHAR(128),
    LastName NVARCHAR(128),
    Birthday DATE,
    IsPrivate BIT,
    CreatedAt DATETIME2,
    City_Id INT
);

CREATE TABLE dbo.TripParticipant (
    User_Id INT,
    Trip_Id INT
);

CREATE TABLE dbo.Review (
    Id INT IDENTITY(1,1),
    Trip_Id INT,
    User_Id INT,
    Content NVARCHAR(1024),
    Stars TINYINT
);

CREATE TABLE dbo.Post (
    Id INT IDENTITY(1,1),
    User_Id INT,
    Content NVARCHAR(1024)
);

CREATE TABLE dbo.Friendship (
    Id INT IDENTITY(1,1),
    UserA_Id INT,
    UserB_Id INT,
    StartDate DATETIME2
);

CREATE TABLE dbo.Achievement (
    Id INT IDENTITY(1,1),
    Name NVARCHAR(128),
    Description NVARCHAR(1024)
);

CREATE TABLE dbo.EarnedAchievement (
    User_Id INT,
    Achievement_Id INT,
    EarnedAt DATETIME2
);

CREATE TABLE dbo.InvitationStatus (
    Id INT IDENTITY(1,1),
    StatusName NVARCHAR(64)
);

CREATE TABLE dbo.Invite (
    Id INT IDENTITY(1,1),
    Trip_Id INT,
    Receiver_Id INT,
    Sender_Id INT,
    InvitationStatus_Id INT,
    SendDate DATETIME2
);

CREATE TABLE dbo.[Group] (
Id INT IDENTITY(1,1),
Name NVARCHAR(64),
Description NVARCHAR(64)
);

CREATE TABLE dbo.GroupMembership (
    User_Id INT,
    Group_Id INT
);

CREATE TABLE dbo.GroupTrip (
    Trip_Id INT,
    Group_Id INT
);


-- 02_Constrains

-- Not Null and Null
-- Trin A: Sæt felter der skal være NOT NULL
-- OBS. IDENTITY Id-kolonner er allerede NOT NULL i SQL Server.

ALTER TABLE dbo.Continent ALTER COLUMN Name NVARCHAR(64) NOT NULL;

ALTER TABLE dbo.Country ALTER COLUMN Continent_Id INT NOT NULL;
ALTER TABLE dbo.Country ALTER COLUMN Name NVARCHAR(64) NOT NULL;

ALTER TABLE dbo.City ALTER COLUMN Country_Id INT NOT NULL;
ALTER TABLE dbo.City ALTER COLUMN Name NVARCHAR(64) NOT NULL;

ALTER TABLE dbo.Activity ALTER COLUMN City_Id INT NOT NULL;
ALTER TABLE dbo.Activity ALTER COLUMN Name NVARCHAR(128) NOT NULL;

ALTER TABLE dbo.Trip ALTER COLUMN IsCompleted BIT NOT NULL;

ALTER TABLE dbo.TripStop ALTER COLUMN City_Id INT NOT NULL;
ALTER TABLE dbo.TripStop ALTER COLUMN Trip_Id INT NOT NULL;

ALTER TABLE dbo.TripActivity ALTER COLUMN Trip_Id INT NOT NULL;
ALTER TABLE dbo.TripActivity ALTER COLUMN Activity_Id INT NOT NULL;

ALTER TABLE dbo.[User] ALTER COLUMN FirstName NVARCHAR(128) NOT NULL;
ALTER TABLE dbo.[User] ALTER COLUMN LastName NVARCHAR(128) NOT NULL;
ALTER TABLE dbo.[User] ALTER COLUMN Birthday DATE NOT NULL;
ALTER TABLE dbo.[User] ALTER COLUMN IsPrivate BIT NOT NULL;
ALTER TABLE dbo.[User] ALTER COLUMN CreatedAt DATETIME2 NOT NULL;
ALTER TABLE dbo.[User] ALTER COLUMN City_Id INT NOT NULL;

ALTER TABLE dbo.TripParticipant ALTER COLUMN User_Id INT NOT NULL;
ALTER TABLE dbo.TripParticipant ALTER COLUMN Trip_Id INT NOT NULL;

ALTER TABLE dbo.Review ALTER COLUMN Trip_Id INT NOT NULL;
ALTER TABLE dbo.Review ALTER COLUMN User_Id INT NOT NULL;
ALTER TABLE dbo.Review ALTER COLUMN Content NVARCHAR(1024) NOT NULL;
ALTER TABLE dbo.Review ALTER COLUMN Stars TINYINT NOT NULL;

ALTER TABLE dbo.Post ALTER COLUMN User_Id INT NOT NULL;
ALTER TABLE dbo.Post ALTER COLUMN Content NVARCHAR(1024) NOT NULL;

ALTER TABLE dbo.Friendship ALTER COLUMN UserA_Id INT NOT NULL;
ALTER TABLE dbo.Friendship ALTER COLUMN UserB_Id INT NOT NULL;
ALTER TABLE dbo.Friendship ALTER COLUMN StartDate DATETIME2 NOT NULL;

ALTER TABLE dbo.Achievement ALTER COLUMN Name NVARCHAR(128) NOT NULL;
ALTER TABLE dbo.Achievement ALTER COLUMN Description NVARCHAR(1024) NOT NULL;

ALTER TABLE dbo.EarnedAchievement ALTER COLUMN User_Id INT NOT NULL;
ALTER TABLE dbo.EarnedAchievement ALTER COLUMN Achievement_Id INT NOT NULL;
ALTER TABLE dbo.EarnedAchievement ALTER COLUMN EarnedAt DATETIME2 NOT NULL;

ALTER TABLE dbo.InvitationStatus ALTER COLUMN StatusName NVARCHAR(64) NOT NULL;

ALTER TABLE dbo.Invite ALTER COLUMN Trip_Id INT NOT NULL;
ALTER TABLE dbo.Invite ALTER COLUMN Receiver_Id INT NOT NULL;
ALTER TABLE dbo.Invite ALTER COLUMN Sender_Id INT NOT NULL;
ALTER TABLE dbo.Invite ALTER COLUMN InvitationStatus_Id INT NOT NULL;
ALTER TABLE dbo.Invite ALTER COLUMN SendDate DATETIME2 NOT NULL;

ALTER TABLE dbo.[Group] ALTER COLUMN Name NVARCHAR(64) NOT NULL;

ALTER TABLE dbo.GroupMembership ALTER COLUMN User_Id INT NOT NULL;
ALTER TABLE dbo.GroupMembership ALTER COLUMN Group_Id INT NOT NULL;

ALTER TABLE dbo.GroupTrip ALTER COLUMN Trip_Id INT NOT NULL;
ALTER TABLE dbo.GroupTrip ALTER COLUMN Group_Id INT NOT NULL;


-- Trin B: Sæt felter der må være NULL
ALTER TABLE dbo.Activity ALTER COLUMN Description NVARCHAR(1024) NULL;

ALTER TABLE dbo.Trip ALTER COLUMN StartDate DATETIME2 NULL;
ALTER TABLE dbo.Trip ALTER COLUMN EndDate DATETIME2 NULL;
ALTER TABLE dbo.Trip ALTER COLUMN ExperienceNotes NVARCHAR(1024) NULL;
ALTER TABLE dbo.Trip ALTER COLUMN PlanNotes NVARCHAR(1024) NULL;

ALTER TABLE dbo.[Group] ALTER COLUMN Description NVARCHAR(64) NULL;

-- Computed
-- Et venskab har ingen retning. UserA_Id og UserB_Id gemmer de valgte brugere,
-- mens UserLow_Id og UserHigh_Id sorterer deres Id'er,
-- så det samme venskab ikke kan gemmes igen i omvendt rækkefølge.

ALTER TABLE dbo.Friendship
ADD UserLow_Id AS
(
    CASE
        WHEN UserA_Id < UserB_Id THEN UserA_Id
        ELSE UserB_Id
    END
) PERSISTED;

ALTER TABLE dbo.Friendship
ADD UserHigh_Id AS
(
    CASE
        WHEN UserA_Id > UserB_Id THEN UserA_Id
        ELSE UserB_Id
    END
) PERSISTED;

-- Primary Keys
ALTER TABLE dbo.Continent
ADD CONSTRAINT PK_Continent PRIMARY KEY (Id);

ALTER TABLE dbo.Country
ADD CONSTRAINT PK_Country PRIMARY KEY (Id);

ALTER TABLE dbo.City
ADD CONSTRAINT PK_City PRIMARY KEY (Id);

ALTER TABLE dbo.Activity
ADD CONSTRAINT PK_Activity PRIMARY KEY (Id);

ALTER TABLE dbo.Trip
ADD CONSTRAINT PK_Trip PRIMARY KEY (Id);

ALTER TABLE dbo.TripStop
ADD CONSTRAINT PK_TripStop PRIMARY KEY (City_Id, Trip_Id);

ALTER TABLE dbo.TripActivity
ADD CONSTRAINT PK_TripActivity PRIMARY KEY (Trip_Id, Activity_Id);

ALTER TABLE dbo.[User]
ADD CONSTRAINT PK_User PRIMARY KEY (Id);

ALTER TABLE dbo.TripParticipant
ADD CONSTRAINT PK_TripParticipant PRIMARY KEY (User_Id, Trip_Id);

ALTER TABLE dbo.Review
ADD CONSTRAINT PK_Review PRIMARY KEY (Id);

ALTER TABLE dbo.Post
ADD CONSTRAINT PK_Post PRIMARY KEY (Id);

ALTER TABLE dbo.Friendship
ADD CONSTRAINT PK_Friendship PRIMARY KEY (Id);

ALTER TABLE dbo.Achievement
ADD CONSTRAINT PK_Achievement PRIMARY KEY (Id);

ALTER TABLE dbo.EarnedAchievement
ADD CONSTRAINT PK_EarnedAchievement PRIMARY KEY (User_Id, Achievement_Id);

ALTER TABLE dbo.InvitationStatus
ADD CONSTRAINT PK_InvitationStatus PRIMARY KEY (Id);

ALTER TABLE dbo.Invite
ADD CONSTRAINT PK_Invite PRIMARY KEY (Id);

ALTER TABLE dbo.[Group]
ADD CONSTRAINT PK_Group PRIMARY KEY (Id);

ALTER TABLE dbo.GroupMembership
ADD CONSTRAINT PK_GroupMembership PRIMARY KEY (User_Id, Group_Id);

ALTER TABLE dbo.GroupTrip
ADD CONSTRAINT PK_GroupTrip PRIMARY KEY (Trip_Id, Group_Id);

-- Foregin Keys
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

-- Unique Constraints
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

-- Check Constraints
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


-- 03_StoredProcedures

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

-- Test ved at køre følgende:
-- EXEC dbo.AddReview @TripId = 1, @UserId = 1, @Content = N'En rigtig god rejseeeeeeeee.', @Stars = 5;


/*
    Opretter en gruppe og tilføjer brugere som medlemmer.

    @MemberIds sendes som JSON:
    [1, 4, 8, 12]

    Hele operationen køres i én transaktion.
    Hvis noget fejler, bliver gruppen ikke oprettet.
*/

CREATE OR ALTER PROCEDURE dbo.CreateGroupWithMembers
    @Name NVARCHAR(255),
    @Description NVARCHAR(MAX),
    @MemberIds NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Gruppen skal have et navn
        IF @Name IS NULL OR LTRIM(RTRIM(@Name)) = N''
        BEGIN
            THROW 51010,
                N'Gruppen skal have et navn.',
                1;
        END;

        -- Kontrollér at MemberIds er gyldig JSON
        IF ISJSON(@MemberIds) <> 1
        BEGIN
            THROW 51011,
                N'MemberIds skal være et gyldigt JSON-array.',
                1;
        END;

        -- Gem medlemmerne midlertidigt
        DECLARE @Members TABLE (UserId INT PRIMARY KEY);

        INSERT INTO @Members (UserId)
        SELECT DISTINCT TRY_CAST([value] AS INT)
        FROM OPENJSON(@MemberIds)
        WHERE TRY_CAST([value] AS INT) IS NOT NULL;

        -- Der skal være mindst ét medlem
        IF NOT EXISTS (SELECT 1 FROM @Members)
        BEGIN
            THROW 51012,
                N'Gruppen skal have mindst ét medlem.',
                1;
        END;

        -- Kontrollér at alle brugere findes
        IF EXISTS (
            SELECT 1
            FROM @Members m
            WHERE NOT EXISTS (
                SELECT 1
                FROM dbo.[User] u
                WHERE u.Id = m.UserId
            )
        )
        BEGIN
            THROW 51013,
                N'En eller flere brugere findes ikke.',
                1;
        END;

        -- Opret gruppen
        INSERT INTO dbo.[Group] (Name, Description)
        VALUES (@Name, @Description);

        DECLARE @GroupId INT;

        SET @GroupId = CONVERT(INT, SCOPE_IDENTITY());

        -- Tilføj medlemmerne
        INSERT INTO dbo.GroupMembership (User_Id, Group_Id)
        SELECT UserId, @GroupId
        FROM @Members;

        COMMIT TRANSACTION;

        -- Returnér den nye gruppes ID
        SELECT @GroupId AS GroupId;
    END TRY

    BEGIN CATCH

        IF XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        THROW;

    END CATCH;
END;


-- Test ved at køre følgende:
-- EXEC dbo.CreateGroupWithMembers @Name = N'Rejseholdet', @Description = N'Vi planlægger en fælles tur til Japan.', @MemberIds = N'[1, 2, 3, 4]';

CREATE OR ALTER PROCEDURE dbo.ExpireInvitations
    @AsOfUtc datetime2 = NULL,
    @ExpiredCount int = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    SET @ExpiredCount = 0;

    IF @AsOfUtc IS NULL
        SET @AsOfUtc = SYSUTCDATETIME();

    DECLARE @PendingId int =
    (
        SELECT Id
        FROM dbo.InvitationStatus
        WHERE StatusName = N'Pending'
    );

    DECLARE @ExpiredId int =
    (
        SELECT Id
        FROM dbo.InvitationStatus
        WHERE StatusName = N'Expired'
    );

    IF @PendingId IS NULL OR @ExpiredId IS NULL
        THROW 51002, N'Pending eller Expired mangler.', 1;

    UPDATE dbo.Invite
    SET InvitationStatus_Id = @ExpiredId
    WHERE InvitationStatus_Id = @PendingId
    AND SendDate <= DATEADD(DAY, -14, @AsOfUtc);

    SET @ExpiredCount = @@ROWCOUNT;
END;
GO

IF TYPE_ID(N'dbo.AchievementUserIds') IS NULL
BEGIN
    EXEC(N'
        CREATE TYPE dbo.AchievementUserIds
        AS TABLE
        (
            User_Id INT NOT NULL PRIMARY KEY
        );
    ');
END;
GO

CREATE OR ALTER PROCEDURE dbo.AwardAchievements
    @Users dbo.AchievementUserIds READONLY
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NOT EXISTS (SELECT 1 FROM @Users)
        RETURN;

    IF (
        SELECT COUNT(*)
        FROM dbo.Achievement
        WHERE Name IN (
            N'Første rejse',
            N'Erfaren rejsende',
            N'Byopdager',
            N'Første anmeldelse',
            N'Social rejsende'
        )
    ) <> 5
    BEGIN
        THROW 50102,
            N'En achievement-definition mangler eller er omdøbt.',
            1;
    END;

    DECLARE @OwnTransaction BIT =
        CASE WHEN @@TRANCOUNT = 0 THEN 1 ELSE 0 END;

    BEGIN TRY

        IF @OwnTransaction = 1
            BEGIN TRANSACTION;

        ;WITH Completed AS
        (
            SELECT
                p.User_Id,
                p.Trip_Id
            FROM dbo.TripParticipant p
            JOIN @Users u
                ON u.User_Id = p.User_Id
            JOIN dbo.Trip t
                ON t.Id = p.Trip_Id
            WHERE t.IsCompleted = 1
        ),
        TripCounts AS
        (
            SELECT
                User_Id,
                COUNT(*) AS Total
            FROM Completed
            GROUP BY User_Id
        ),
        CityCounts AS
        (
            SELECT
                c.User_Id,
                COUNT(DISTINCT s.City_Id) AS Total
            FROM Completed c
            JOIN dbo.TripStop s
                ON s.Trip_Id = c.Trip_Id
            GROUP BY c.User_Id
        ),
        Reviewers AS
        (
            SELECT DISTINCT r.User_Id
            FROM dbo.Review r
            JOIN @Users u
                ON u.User_Id = r.User_Id
        ),
        FriendPairs AS
        (
            SELECT
                f.UserA_Id AS User_Id,
                f.UserB_Id AS Friend_Id
            FROM dbo.Friendship f
            JOIN @Users u
                ON u.User_Id = f.UserA_Id

            UNION

            SELECT
                f.UserB_Id,
                f.UserA_Id
            FROM dbo.Friendship f
            JOIN @Users u
                ON u.User_Id = f.UserB_Id
        ),
        FriendCounts AS
        (
            SELECT
                User_Id,
                COUNT(*) AS Total
            FROM FriendPairs
            GROUP BY User_Id
        ),
        Eligible AS
        (
            SELECT User_Id, N'Første rejse' AS Name
            FROM TripCounts
            WHERE Total >= 1

            UNION ALL

            SELECT User_Id, N'Erfaren rejsende'
            FROM TripCounts
            WHERE Total >= 5

            UNION ALL

            SELECT User_Id, N'Byopdager'
            FROM CityCounts
            WHERE Total >= 10

            UNION ALL

            SELECT User_Id, N'Første anmeldelse'
            FROM Reviewers

            UNION ALL

            SELECT User_Id, N'Social rejsende'
            FROM FriendCounts
            WHERE Total >= 5
        )

        INSERT dbo.EarnedAchievement
        (
            User_Id,
            Achievement_Id,
            EarnedAt
        )
        SELECT
            e.User_Id,
            a.Id,
            SYSUTCDATETIME()
        FROM Eligible e
        JOIN dbo.Achievement a
            ON a.Name = e.Name
        WHERE NOT EXISTS
        (
            SELECT 1
            FROM dbo.EarnedAchievement existing
                WITH (UPDLOCK, HOLDLOCK)
            WHERE existing.User_Id = e.User_Id
            AND existing.Achievement_Id = a.Id
        );

        IF @OwnTransaction = 1
            COMMIT;

    END TRY
    BEGIN CATCH

        IF @OwnTransaction = 1
        AND XACT_STATE() <> 0
            ROLLBACK;

        THROW;

    END CATCH;
END;
GO


- 04_StoredFunctions

-- Returns the full name for a given user
CREATE FUNCTION dbo.GetUsersFullName(@UserId INT)
RETURNS NVARCHAR(255)
AS
BEGIN
    DECLARE @FullName NVARCHAR(255);

    SELECT @FullName = FirstName + ' ' + LastName
    FROM dbo.[User]
    WHERE Id = @UserId;

    RETURN @FullName;
END;

-- Returns the total number of completed trips for a given user
CREATE FUNCTION dbo.GetAmountOfTripsByUser(@UserId INT)
RETURNS INT
AS
BEGIN
    DECLARE @TripCount INT;

    SELECT @TripCount = COUNT(*)
    FROM dbo.TripParticipant tp
    INNER JOIN dbo.Trip t ON t.Id = tp.Trip_Id
    WHERE tp.User_Id = @UserId
    AND t.IsCompleted = 1;

    RETURN @TripCount;
END;

-- Returns the number of days a trip lasts (start and end day both included).
-- Returns 0 if the trip doesn't exist or has no start/end date.
CREATE FUNCTION dbo.GetDurationOfTrip (@TripId INT)
RETURNS INT
AS
BEGIN
    DECLARE @StartDate DATETIME2;
    DECLARE @EndDate DATETIME2;

    SELECT @StartDate = StartDate,
           @EndDate = EndDate
    FROM dbo.Trip
    WHERE Id = @TripId;

    IF @StartDate IS NULL OR @EndDate IS NULL
        RETURN 0;

    RETURN DATEDIFF(DAY, @StartDate, @EndDate) + 1;
END;


-- 05_Triggers

CREATE OR ALTER TRIGGER dbo.trg_Trip_Achievements
ON dbo.Trip
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Users dbo.AchievementUserIds;

    INSERT @Users (User_Id)
    SELECT DISTINCT p.User_Id
    FROM inserted i
    JOIN dbo.TripParticipant p
        ON p.Trip_Id = i.Id
    WHERE i.IsCompleted = 1;

    IF EXISTS (SELECT 1 FROM @Users)
        EXEC dbo.AwardAchievements @Users = @Users;
END;

CREATE OR ALTER TRIGGER dbo.trg_TripParticipant_Achievements
ON dbo.TripParticipant
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Users dbo.AchievementUserIds;

    INSERT @Users (User_Id)
    SELECT DISTINCT User_Id
    FROM inserted;

    IF EXISTS (SELECT 1 FROM @Users)
        EXEC dbo.AwardAchievements @Users = @Users;
END;

CREATE OR ALTER TRIGGER dbo.trg_TripStop_Achievements
ON dbo.TripStop
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Users dbo.AchievementUserIds;

    INSERT @Users (User_Id)
    SELECT DISTINCT p.User_Id
    FROM inserted i
    JOIN dbo.TripParticipant p
        ON p.Trip_Id = i.Trip_Id;

    IF EXISTS (SELECT 1 FROM @Users)
        EXEC dbo.AwardAchievements @Users = @Users;
END;

CREATE OR ALTER TRIGGER dbo.trg_Review_Achievements
ON dbo.Review
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Users dbo.AchievementUserIds;

    INSERT @Users (User_Id)
    SELECT DISTINCT User_Id
    FROM inserted;

    IF EXISTS (SELECT 1 FROM @Users)
        EXEC dbo.AwardAchievements @Users = @Users;
END;

CREATE OR ALTER TRIGGER dbo.trg_Friendship_Achievements
ON dbo.Friendship
AFTER INSERT, UPDATE
AS
BEGIN
SET NOCOUNT ON;

DECLARE @Users dbo.AchievementUserIds;

INSERT @Users (User_Id)
SELECT UserA_Id
FROM inserted

UNION

SELECT UserB_Id
FROM inserted;

IF EXISTS (SELECT 1 FROM @Users)
EXEC dbo.AwardAchievements @Users = @Users;
END;

/*
Se alle triggerne i databasen:
SELECT
name,
parent_class_desc,
create_date,
modify_date
FROM sys.triggers
ORDER BY name;
*/



-- 06_Views

-- 1. Rejseoversigt
CREATE OR ALTER VIEW dbo.vw_TripOverview
AS
SELECT
    t.Id                                    AS TripId,
    t.StartDate,
    t.EndDate,
    DATEDIFF(DAY, t.StartDate, t.EndDate) + 1 AS DurationDays,
    CASE WHEN t.IsCompleted = 1 THEN N'Gennemført' ELSE N'Planlagt' END AS TripStatus,
    g.GroupName,
    ISNULL(p.ParticipantCount, 0)           AS ParticipantCount,
    s.Cities,
    ISNULL(s.CityCount, 0)                  AS CityCount,
    ISNULL(r.ReviewCount, 0)                AS ReviewCount,
    r.AvgStars
FROM dbo.Trip t
OUTER APPLY (
    SELECT TOP (1) gr.Name AS GroupName
    FROM dbo.GroupTrip gt
    JOIN dbo.[Group] gr ON gr.Id = gt.Group_Id
    WHERE gt.Trip_Id = t.Id
    ORDER BY gr.Name
) g
OUTER APPLY (
    SELECT COUNT(*) AS ParticipantCount
    FROM dbo.TripParticipant tp
    WHERE tp.Trip_Id = t.Id
) p
OUTER APPLY (
    SELECT
        COUNT(*) AS CityCount,
        STRING_AGG(CAST(c.Name AS NVARCHAR(MAX)), N', ') WITHIN GROUP (ORDER BY c.Name) AS Cities
    FROM dbo.TripStop ts
    JOIN dbo.City c ON c.Id = ts.City_Id
    WHERE ts.Trip_Id = t.Id
) s
OUTER APPLY (
    SELECT
        COUNT(*) AS ReviewCount,
        CAST(AVG(CAST(rv.Stars AS DECIMAL(4,2))) AS DECIMAL(3,1)) AS AvgStars
    FROM dbo.Review rv
    WHERE rv.Trip_Id = t.Id
) r;

-- 2. Brugerprofil
CREATE OR ALTER VIEW dbo.vw_UserProfile
AS
SELECT
    u.Id                                    AS UserId,
    u.FirstName + N' ' + u.LastName         AS FullName,
    DATEDIFF(YEAR, u.Birthday, CAST(GETDATE() AS DATE))
      - CASE WHEN DATEADD(YEAR, DATEDIFF(YEAR, u.Birthday, CAST(GETDATE() AS DATE)), u.Birthday)
                  > CAST(GETDATE() AS DATE) THEN 1 ELSE 0 END AS Age,
    c.Name                                  AS HomeCity,
    co.Name                                 AS HomeCountry,
    u.IsPrivate,
    u.CreatedAt                             AS MemberSince,
    (SELECT COUNT(*) FROM dbo.Friendship f
      WHERE f.UserA_Id = u.Id OR f.UserB_Id = u.Id)            AS FriendCount,
    (SELECT COUNT(*) FROM dbo.TripParticipant tp
      JOIN dbo.Trip t ON t.Id = tp.Trip_Id
      WHERE tp.User_Id = u.Id AND t.IsCompleted = 1)           AS CompletedTrips,
    (SELECT COUNT(*) FROM dbo.TripParticipant tp
      JOIN dbo.Trip t ON t.Id = tp.Trip_Id
      WHERE tp.User_Id = u.Id AND t.IsCompleted = 0)           AS PlannedTrips,
    (SELECT COUNT(*) FROM dbo.Review rv WHERE rv.User_Id = u.Id) AS ReviewCount,
    (SELECT COUNT(*) FROM dbo.EarnedAchievement ea
      WHERE ea.User_Id = u.Id)                                 AS AchievementCount
FROM dbo.[User] u
JOIN dbo.City c     ON c.Id  = u.City_Id
JOIN dbo.Country co ON co.Id = c.Country_Id;

-- 3. Destinationsstatistik
CREATE OR ALTER VIEW dbo.vw_DestinationStats
AS
SELECT
    c.Id                                    AS CityId,
    c.Name                                  AS City,
    co.Name                                 AS Country,
    ct.Name                                 AS Continent,
    ISNULL(v.TripCount, 0)                  AS TripCount,
    ISNULL(v.CompletedTripCount, 0)         AS CompletedTripCount,
    (SELECT COUNT(*) FROM dbo.Activity a WHERE a.City_Id = c.Id) AS ActivityCount,
    rv.AvgStars
FROM dbo.City c
JOIN dbo.Country co   ON co.Id = c.Country_Id
JOIN dbo.Continent ct ON ct.Id = co.Continent_Id
OUTER APPLY (
    SELECT
        COUNT(DISTINCT ts.Trip_Id) AS TripCount,
        COUNT(DISTINCT CASE WHEN t.IsCompleted = 1 THEN ts.Trip_Id END) AS CompletedTripCount
    FROM dbo.TripStop ts
    JOIN dbo.Trip t ON t.Id = ts.Trip_Id
    WHERE ts.City_Id = c.Id
) v
OUTER APPLY (
    SELECT CAST(AVG(CAST(r.Stars AS DECIMAL(4,2))) AS DECIMAL(3,1)) AS AvgStars
    FROM dbo.Review r
    WHERE r.Trip_Id IN (SELECT ts.Trip_Id FROM dbo.TripStop ts WHERE ts.City_Id = c.Id)
) rv;

-- 4. Åbne invitationer
CREATE OR ALTER VIEW dbo.vw_PendingInvitations
AS
SELECT
    i.Id                                    AS InviteId,
    i.Trip_Id                               AS TripId,
    t.StartDate                             AS TripStartDate,
    s.FirstName + N' ' + s.LastName         AS SenderName,
    r.FirstName + N' ' + r.LastName         AS ReceiverName,
    i.SendDate,
    DATEADD(DAY, 14, i.SendDate)            AS ExpiresAt,
    DATEDIFF(DAY, SYSUTCDATETIME(), DATEADD(DAY, 14, i.SendDate)) AS DaysUntilExpiry
FROM dbo.Invite i
JOIN dbo.InvitationStatus st ON st.Id = i.InvitationStatus_Id
JOIN dbo.Trip t              ON t.Id  = i.Trip_Id
JOIN dbo.[User] s            ON s.Id  = i.Sender_Id
JOIN dbo.[User] r            ON r.Id  = i.Receiver_Id
WHERE st.StatusName = N'Pending';


-- 07_Indexes
-- Find brugere på en bestemt rejse
CREATE NONCLUSTERED INDEX IX_TripParticipant_TripId
ON dbo.TripParticipant (Trip_Id, User_Id);

-- Find byerne på en bestemt rejse
CREATE NONCLUSTERED INDEX IX_TripStop_TripId
ON dbo.TripStop (Trip_Id, City_Id);

-- Find anmeldelser og stjerner for en rejse
CREATE NONCLUSTERED INDEX IX_Review_TripId
ON dbo.Review (Trip_Id)
INCLUDE (Stars);

-- Find invitationer ud fra status og dato
CREATE NONCLUSTERED INDEX IX_Invite_Status_SendDate
ON dbo.Invite (InvitationStatus_Id, SendDate);


-- 08_Events

IF EXISTS (
SELECT 1
FROM msdb.dbo.sysjobs
WHERE name = N'TravelDb - Expire invitations'
)
BEGIN
PRINT N'Jobbet findes allerede.';
RETURN;
END;

DECLARE @JobId uniqueidentifier;
DECLARE @ScheduleId int;

EXEC msdb.dbo.sp_add_job
@job_name = N'TravelDb - Expire invitations',
@enabled = 1,
@description = N'Markerer Pending-invitationer som Expired efter 14 dage.',
@job_id = @JobId OUTPUT;

EXEC msdb.dbo.sp_add_jobstep
@job_id = @JobId,
@step_name = N'Udløb gamle invitationer',
@subsystem = N'TSQL',
@database_name = N'TravelDb',
@command = N'
DECLARE @Changed int;

EXEC dbo.ExpireInvitations
@ExpiredCount = @Changed OUTPUT;

PRINT CONCAT(
N''Antal udløbne invitationer: '',
@Changed
);';

EXEC msdb.dbo.sp_add_schedule
@schedule_name = N'TravelDb - Invitations daily 02:00',
@enabled = 1,
@freq_type = 4,
@freq_interval = 1,
@active_start_time = 020000,
@schedule_id = @ScheduleId OUTPUT;

EXEC msdb.dbo.sp_attach_schedule
@job_id = @JobId,
@schedule_id = @ScheduleId;

EXEC msdb.dbo.sp_add_jobserver
@job_id = @JobId;

-- 09_InsertData

/*
    Opbygning:
    1. Kontrollér eksisterende navne
    2. Gør achievement-navne unikke
    3. Opret de fem achievements

    Noter:
    - Kør hele filen i en konsol for TravelDb.
    - Name bruges af tildelingsproceduren. Navnene skal derfor være unikke.
    - Hvis et navn ændres, skal proceduren og testene også opdateres.
    - Id er stadig primærnøglen og bruges af EarnedAchievement.
    - Filen kan genkøres. Eksisterende matchende achievements beholder deres Id.
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    -- ------------------------------------------------------------
    -- Trin 1: Kontrollér navne.
    -- Afbryd ved dubletter, så eksisterende tildelinger ikke sammenlægges vilkårligt.
    IF EXISTS (
        SELECT Name FROM dbo.Achievement GROUP BY Name HAVING COUNT(*)>1
    )
        THROW 50100,N'Der findes achievement-navne flere gange. Afklar dubletterne først.',1;

    IF EXISTS (SELECT 1 FROM dbo.Achievement WHERE Name=N'First completed trip')
    AND EXISTS (SELECT 1 FROM dbo.Achievement WHERE Name=N'Første rejse')
        THROW 50101,N'Både First completed trip og Første rejse findes. Afklar hvilken række der skal bruges.',1;

    UPDATE dbo.Achievement
    SET Name=N'Første rejse'
    WHERE Name=N'First completed trip';
    -- ------------------------------------------------------------
    -- Trin 2: Indsæt kun de navne, som endnu ikke findes.
    INSERT dbo.Achievement(Name,Description)
    SELECT d.Name,d.Description
    FROM (VALUES
        (N'Første rejse',N'Gennemfør din første rejse.'),
        (N'Erfaren rejsende',N'Gennemfør fem forskellige rejser.'),
        (N'Byopdager',N'Besøg ti forskellige byer på gennemførte rejser.'),
        (N'Første anmeldelse',N'Skriv din første anmeldelse.'),
        (N'Social rejsende',N'Få fem forskellige venner.')
    ) AS d(Name,Description)
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.Achievement a WHERE a.Name=d.Name
    );

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE()<>0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
GO
SELECT Id,Name,Description
FROM dbo.Achievement
WHERE Name IN (N'Første rejse',N'Erfaren rejsende',N'Byopdager',N'Første anmeldelse',N'Social rejsende');


/*
    Trin 1: Opret de statusværdier, proceduren og testen bruger.
    Kør mod TravelDb. Eksisterende matchende statusser genbruges.
*/
SET XACT_ABORT ON;
BEGIN TRY
    BEGIN TRANSACTION;
    IF EXISTS (SELECT StatusName FROM dbo.InvitationStatus GROUP BY StatusName HAVING COUNT(*)>1)
        THROW 51001,N'Der findes dubletter af statusnavne. Afklar dem først.',1;
    IF NOT EXISTS (SELECT 1 FROM sys.key_constraints
        WHERE parent_object_id=OBJECT_ID(N'dbo.InvitationStatus') AND name=N'UQ_InvitationStatus_StatusName')
    INSERT dbo.InvitationStatus(StatusName)
    SELECT d.Name FROM (VALUES(N'Pending'),(N'Accepted'),(N'Declined'),(N'Expired')) d(Name)
    WHERE NOT EXISTS (SELECT 1 FROM dbo.InvitationStatus s WHERE s.StatusName=d.Name);
    COMMIT;
END TRY
BEGIN CATCH
    IF XACT_STATE()<>0 ROLLBACK;
    THROW;
END CATCH;
SELECT Id,StatusName FROM dbo.InvitationStatus;


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

