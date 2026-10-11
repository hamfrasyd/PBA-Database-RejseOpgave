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

-- Definerer en liste af bruger-ID'er, som kan sendes til achievement-proceduren.
IF TYPE_ID(N'dbo.AchievementUserIds') IS NULL
BEGIN
    EXEC(N'
        CREATE TYPE dbo.AchievementUserIds AS TABLE (
            User_Id INT NOT NULL PRIMARY KEY
        );
    ');
END;

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

-- Opretter en anmeldelse af en gennemført rejse.
-- Kontrollerer at brugeren og rejsen findes, at brugeren deltager, og at bedømmelsen er mellem 1 og 6.

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

-- Opretter en gruppe og tilføjer medlemmer fra en JSON-liste.
-- Validerer gruppen og brugerne samt sikrer, at hele oprettelsen gennemføres i én transaktion.

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

-- Ændrer invitationer fra Pending til Expired, når de er mindst 14 dage gamle.
-- Bruger UTC som standard og returnerer antallet af opdaterede invitationer via en outputparameter.

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

-- Kontrollerer achievements for de angivne brugere baseret på rejser, byer, anmeldelser og venskaber.
-- Tildeler kun achievements, som brugerne endnu ikke har modtaget.

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

    DECLARE @StartedOwnTransaction BIT =
        CASE WHEN @@TRANCOUNT = 0 THEN 1 ELSE 0 END;

    DECLARE @EligibleAchievements TABLE (
        UserId INT NOT NULL,
        AchievementName NVARCHAR(128) NOT NULL,
        PRIMARY KEY (UserId, AchievementName)
    );

    BEGIN TRY
        IF @StartedOwnTransaction = 1
            BEGIN TRANSACTION;

        -- 1. Første rejse og Erfaren rejsende
        ;WITH CompletedTripCounts AS (
            SELECT
                Participant.User_Id AS UserId,
                COUNT(*) AS CompletedTrips
            FROM dbo.TripParticipant AS Participant
            JOIN dbo.Trip AS Trip
                ON Trip.Id = Participant.Trip_Id
            JOIN @Users AS CandidateUser
                ON CandidateUser.User_Id = Participant.User_Id
            WHERE Trip.IsCompleted = 1
            GROUP BY Participant.User_Id
        )
        INSERT INTO @EligibleAchievements (UserId, AchievementName)
        SELECT
            UserId,
            N'Første rejse'
        FROM CompletedTripCounts
        WHERE CompletedTrips >= 1

        UNION ALL

        SELECT
            UserId,
            N'Erfaren rejsende'
        FROM CompletedTripCounts
        WHERE CompletedTrips >= 5;


        -- 2. Byopdager: 10 forskellige byer på gennemførte rejser
        INSERT INTO @EligibleAchievements (UserId, AchievementName)
        SELECT
            Participant.User_Id,
            N'Byopdager'
        FROM dbo.TripParticipant AS Participant
        JOIN @Users AS CandidateUser
            ON CandidateUser.User_Id = Participant.User_Id
        JOIN dbo.Trip AS Trip
            ON Trip.Id = Participant.Trip_Id
        JOIN dbo.TripStop AS TripStop
            ON TripStop.Trip_Id = Trip.Id
        WHERE Trip.IsCompleted = 1
        GROUP BY Participant.User_Id
        HAVING COUNT(DISTINCT TripStop.City_Id) >= 10;


        -- 3. Første anmeldelse
        INSERT INTO @EligibleAchievements (UserId, AchievementName)
        SELECT
            CandidateUser.User_Id,
            N'Første anmeldelse'
        FROM @Users AS CandidateUser
        WHERE EXISTS (
            SELECT 1
            FROM dbo.Review AS Review
            WHERE Review.User_Id = CandidateUser.User_Id
        );


        -- 4. Social rejsende: 5 forskellige venner
        ;WITH UserFriendships AS (
            SELECT
                Friendship.UserA_Id AS UserId,
                Friendship.UserB_Id AS FriendId
            FROM dbo.Friendship AS Friendship
            JOIN @Users AS CandidateUser
                ON CandidateUser.User_Id = Friendship.UserA_Id

            UNION

            SELECT
                Friendship.UserB_Id AS UserId,
                Friendship.UserA_Id AS FriendId
            FROM dbo.Friendship AS Friendship
            JOIN @Users AS CandidateUser
                ON CandidateUser.User_Id = Friendship.UserB_Id
        )
        INSERT INTO @EligibleAchievements (UserId, AchievementName)
        SELECT
            UserId,
            N'Social rejsende'
        FROM UserFriendships
        GROUP BY UserId
        HAVING COUNT(*) >= 5;


        -- 5. Tildel kun achievements, som brugeren mangler
        INSERT INTO dbo.EarnedAchievement (
            User_Id,
            Achievement_Id,
            EarnedAt
        )
        SELECT
            Eligible.UserId,
            Achievement.Id,
            SYSUTCDATETIME()
        FROM @EligibleAchievements AS Eligible
        JOIN dbo.Achievement AS Achievement
            ON Achievement.Name = Eligible.AchievementName
        WHERE NOT EXISTS (
            SELECT 1
            FROM dbo.EarnedAchievement AS Existing
                WITH (UPDLOCK, HOLDLOCK)
            WHERE Existing.User_Id = Eligible.UserId
              AND Existing.Achievement_Id = Achievement.Id
        );

        IF @StartedOwnTransaction = 1
            COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH
        IF @StartedOwnTransaction = 1
           AND XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH;
END;
GO


-- 04_StoredFunctions

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

-- Aktiveres ved oprettelse eller opdatering af en rejse.
-- Kontrollerer achievements for deltagere på gennemførte rejser.

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

-- Aktiveres når deltagere tilføjes eller opdateres på en rejse.
-- Kontrollerer om de berørte deltagere har opnået nye achievements.

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

-- Aktiveres når destinationer tilføjes eller opdateres på en rejse.
-- Kontrollerer achievements for alle deltagere på de berørte rejser.

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

-- Aktiveres når en anmeldelse oprettes eller opdateres.
-- Kontrollerer om anmeldelsens forfatter har opnået nye achievements.

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

-- Aktiveres når et venskab oprettes eller opdateres.
-- Kontrollerer achievements for begge brugere i venskabet.

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


-- 06_Views

-- Viser hver rejse med status, gruppe, deltagere, byer og anmeldelser.
-- Test: SELECT * FROM dbo.vw_TripOverview;

CREATE OR ALTER VIEW dbo.vw_TripOverview
AS
SELECT
    Trip.Id AS TripId,
    Trip.StartDate,
    Trip.EndDate,
    DATEDIFF(DAY, Trip.StartDate, Trip.EndDate) + 1 AS DurationDays,

    CASE
        WHEN Trip.IsCompleted = 1 THEN N'Gennemført'
        ELSE N'Planlagt'
    END AS TripStatus,

    -- Vælger den første gruppe alfabetisk, hvis flere grupper deler rejsen.
    (
        SELECT TOP (1) TripGroup.Name
        FROM dbo.GroupTrip AS GroupTrip
        JOIN dbo.[Group] AS TripGroup
            ON TripGroup.Id = GroupTrip.Group_Id
        WHERE GroupTrip.Trip_Id = Trip.Id
        ORDER BY TripGroup.Name
    ) AS GroupName,

    ISNULL((
        SELECT COUNT(*)
        FROM dbo.TripParticipant AS Participant
        WHERE Participant.Trip_Id = Trip.Id
    ), 0) AS ParticipantCount,

    CitySummary.Cities,
    ISNULL(CitySummary.CityCount, 0) AS CityCount,
    ISNULL(ReviewSummary.ReviewCount, 0) AS ReviewCount,
    ReviewSummary.AvgStars

FROM dbo.Trip AS Trip

OUTER APPLY (
    SELECT
        COUNT(*) AS CityCount,
        STRING_AGG(
            CAST(City.Name AS NVARCHAR(MAX)),
            N', '
        ) WITHIN GROUP (ORDER BY City.Name) AS Cities
    FROM dbo.TripStop AS TripStop
    JOIN dbo.City AS City
        ON City.Id = TripStop.City_Id
    WHERE TripStop.Trip_Id = Trip.Id
) AS CitySummary

OUTER APPLY (
    SELECT
        COUNT(*) AS ReviewCount,
        CAST(
            AVG(CAST(Review.Stars AS DECIMAL(4, 2)))
            AS DECIMAL(3, 1)
        ) AS AvgStars
    FROM dbo.Review AS Review
    WHERE Review.Trip_Id = Trip.Id
) AS ReviewSummary;

-- Viser brugerprofiler med bopæl, alder, rejser og aktivitet.
-- Test: SELECT * FROM dbo.vw_UserProfile;

CREATE OR ALTER VIEW dbo.vw_UserProfile
AS
SELECT
    ProfileUser.Id AS UserId,
    ProfileUser.FirstName + N' ' + ProfileUser.LastName AS FullName,

    AgeCalculation.CalendarYears -
        CASE
            WHEN DATEADD(
                YEAR,
                AgeCalculation.CalendarYears,
                ProfileUser.Birthday
            ) > AgeCalculation.Today THEN 1
            ELSE 0
        END AS Age,

    HomeCity.Name AS HomeCity,
    HomeCountry.Name AS HomeCountry,
    ProfileUser.IsPrivate,
    ProfileUser.CreatedAt AS MemberSince,

    (
        SELECT COUNT(*)
        FROM dbo.Friendship AS Friendship
        WHERE Friendship.UserA_Id = ProfileUser.Id
           OR Friendship.UserB_Id = ProfileUser.Id
    ) AS FriendCount,

    TripSummary.CompletedTrips,
    TripSummary.PlannedTrips,

    (
        SELECT COUNT(*)
        FROM dbo.Review AS Review
        WHERE Review.User_Id = ProfileUser.Id
    ) AS ReviewCount,

    (
        SELECT COUNT(*)
        FROM dbo.EarnedAchievement AS Achievement
        WHERE Achievement.User_Id = ProfileUser.Id
    ) AS AchievementCount

FROM dbo.[User] AS ProfileUser

JOIN dbo.City AS HomeCity
    ON HomeCity.Id = ProfileUser.City_Id

JOIN dbo.Country AS HomeCountry
    ON HomeCountry.Id = HomeCity.Country_Id

CROSS APPLY (
    SELECT
        CAST(GETDATE() AS DATE) AS Today,
        DATEDIFF(
            YEAR,
            ProfileUser.Birthday,
            CAST(GETDATE() AS DATE)
        ) AS CalendarYears
) AS AgeCalculation

OUTER APPLY (
    SELECT
        COUNT(CASE WHEN Trip.IsCompleted = 1 THEN 1 END) AS CompletedTrips,
        COUNT(CASE WHEN Trip.IsCompleted = 0 THEN 1 END) AS PlannedTrips
    FROM dbo.TripParticipant AS Participant
    JOIN dbo.Trip AS Trip
        ON Trip.Id = Participant.Trip_Id
    WHERE Participant.User_Id = ProfileUser.Id
) AS TripSummary;

-- Viser statistik for hver by med rejser, aktiviteter og anmeldelser.
-- Test: SELECT * FROM dbo.vw_DestinationStats;

CREATE OR ALTER VIEW dbo.vw_DestinationStats
AS
SELECT
    DestinationCity.Id AS CityId,
    DestinationCity.Name AS City,
    Country.Name AS Country,
    Continent.Name AS Continent,

    ISNULL(TripSummary.TripCount, 0) AS TripCount,
    ISNULL(TripSummary.CompletedTripCount, 0) AS CompletedTripCount,

    (
        SELECT COUNT(*)
        FROM dbo.Activity AS Activity
        WHERE Activity.City_Id = DestinationCity.Id
    ) AS ActivityCount,

    (
        SELECT
            CAST(
                AVG(CAST(Review.Stars AS DECIMAL(4, 2)))
                AS DECIMAL(3, 1)
            )
        FROM dbo.Review AS Review
        WHERE Review.Trip_Id IN (
            SELECT TripStop.Trip_Id
            FROM dbo.TripStop AS TripStop
            WHERE TripStop.City_Id = DestinationCity.Id
        )
    ) AS AvgStars

FROM dbo.City AS DestinationCity

JOIN dbo.Country AS Country
    ON Country.Id = DestinationCity.Country_Id

JOIN dbo.Continent AS Continent
    ON Continent.Id = Country.Continent_Id

OUTER APPLY (
    SELECT
        COUNT(*) AS TripCount,
        COUNT(CASE WHEN Trip.IsCompleted = 1 THEN 1 END) AS CompletedTripCount
    FROM dbo.TripStop AS TripStop
    JOIN dbo.Trip AS Trip
        ON Trip.Id = TripStop.Trip_Id
    WHERE TripStop.City_Id = DestinationCity.Id
) AS TripSummary;

-- Viser afventende invitationer med afsender, modtager og udløbsdato.
-- Test: SELECT * FROM dbo.vw_PendingInvitations;

CREATE OR ALTER VIEW dbo.vw_PendingInvitations
AS
SELECT
    Invitation.Id AS InviteId,
    Invitation.Trip_Id AS TripId,
    Trip.StartDate AS TripStartDate,

    Sender.FirstName + N' ' + Sender.LastName AS SenderName,
    Receiver.FirstName + N' ' + Receiver.LastName AS ReceiverName,

    Invitation.SendDate,
    DATEADD(DAY, 14, Invitation.SendDate) AS ExpiresAt,

    DATEDIFF(
        DAY,
        SYSUTCDATETIME(),
        DATEADD(DAY, 14, Invitation.SendDate)
    ) AS DaysUntilExpiry

FROM dbo.Invite AS Invitation

JOIN dbo.InvitationStatus AS InvitationStatus
    ON InvitationStatus.Id = Invitation.InvitationStatus_Id

JOIN dbo.Trip AS Trip
    ON Trip.Id = Invitation.Trip_Id

JOIN dbo.[User] AS Sender
    ON Sender.Id = Invitation.Sender_Id

JOIN dbo.[User] AS Receiver
    ON Receiver.Id = Invitation.Receiver_Id

WHERE InvitationStatus.StatusName = N'Pending';


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

-- Opretter et dagligt job, der udløber invitationer efter 14 dage.
-- Test: EXEC msdb.dbo.sp_start_job @job_name = N'TravelDb - Expire invitations';

IF EXISTS (
    SELECT 1
    FROM msdb.dbo.sysjobs
    WHERE name = N'TravelDb - Expire invitations'
)
BEGIN
    PRINT N'Jobbet findes allerede.';
    RETURN;
END;

DECLARE @ExpirationJobId UNIQUEIDENTIFIER;
DECLARE @DailyScheduleId INT;

EXEC msdb.dbo.sp_add_job
    @job_name = N'TravelDb - Expire invitations',
    @enabled = 1,
    @description = N'Markerer Pending-invitationer som Expired efter 14 dage.',
    @job_id = @ExpirationJobId OUTPUT;

EXEC msdb.dbo.sp_add_jobstep
    @job_id = @ExpirationJobId,
    @step_name = N'Udløb gamle invitationer',
    @subsystem = N'TSQL',
    @database_name = N'TravelDb',
    @command = N'
        DECLARE @ExpiredInvitationCount INT;

        EXEC dbo.ExpireInvitations
            @ExpiredCount = @ExpiredInvitationCount OUTPUT;

        PRINT CONCAT(
            N''Antal udløbne invitationer: '',
            @ExpiredInvitationCount
        );
    ';

-- Jobbet kører kl. 02:00 efter SQL Serverens lokale tid.
EXEC msdb.dbo.sp_add_schedule
    @schedule_name = N'TravelDb - Invitations daily 02:00',
    @enabled = 1,
    @freq_type = 4,
    @freq_interval = 1,
    @active_start_time = 020000,
    @schedule_id = @DailyScheduleId OUTPUT;

EXEC msdb.dbo.sp_attach_schedule
    @job_id = @ExpirationJobId,
    @schedule_id = @DailyScheduleId;

EXEC msdb.dbo.sp_add_jobserver
    @job_id = @ExpirationJobId;


-- 09_InsertData

-- Indsætter de fem achievements, som tildelingsproceduren bruger.
-- Test: SELECT Id, Name, Description FROM dbo.Achievement;

SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    INSERT INTO dbo.Achievement (Name, Description)
    SELECT
        AchievementDefinition.Name,
        AchievementDefinition.Description
    FROM (VALUES
        (N'Første rejse', N'Gennemfør din første rejse.'),
        (N'Erfaren rejsende', N'Gennemfør fem forskellige rejser.'),
        (N'Byopdager', N'Besøg ti forskellige byer på gennemførte rejser.'),
        (N'Første anmeldelse', N'Skriv din første anmeldelse.'),
        (N'Social rejsende', N'Få fem forskellige venner.')
    ) AS AchievementDefinition (Name, Description)
    WHERE NOT EXISTS (
        SELECT 1
        FROM dbo.Achievement AS ExistingAchievement
        WHERE ExistingAchievement.Name = AchievementDefinition.Name
    );

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;

SELECT
    Id,
    Name,
    Description
FROM dbo.Achievement
WHERE Name IN (
    N'Første rejse',
    N'Erfaren rejsende',
    N'Byopdager',
    N'Første anmeldelse',
    N'Social rejsende'
);

-- Indsætter de fire invitationsstatusser, som systemet bruger.
-- Test: SELECT Id, StatusName FROM dbo.InvitationStatus;

SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    INSERT INTO dbo.InvitationStatus (StatusName)
    SELECT StatusDefinition.StatusName
    FROM (VALUES
        (N'Pending'),
        (N'Accepted'),
        (N'Declined'),
        (N'Expired')
    ) AS StatusDefinition (StatusName)
    WHERE NOT EXISTS (
        SELECT 1
        FROM dbo.InvitationStatus AS ExistingStatus
        WHERE ExistingStatus.StatusName = StatusDefinition.StatusName
    );

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;

SELECT
    Id,
    StatusName
FROM dbo.InvitationStatus;

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