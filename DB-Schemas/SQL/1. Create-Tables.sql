/*
    Opbygning:
    0. Opret database og tabeller
    2. Sæt NULL / NOT NULL
    3. Tilføj computed columns
    4. Tilføj primary keys
    5. Tilføj foreign keys og cascade delete
    6. Tilføj UNIQUE-constraints
    7. Tilføj CHECK-constraints

    Noter:
    - IDENTITY skal sættes, når kolonnen oprettes.
    - IDENTITY-kolonner er automatisk NOT NULL i SQL Server.
    - [User] og [Group] bruger [..] fordi USER og GROUP er SQL Server-nøgleord.
    - Cascade delete bruges kun, der hvor det giver mening.
*/

-- ------------------------------------------------------------
-- Trin 1.A: Opret databasen, hvis den ikke allerede findes

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

-- Lad uspecificerede almindelige kolonner tillade NULL-værdier ved den indledende oprettelse.
-- NULL / NOT NULL sættes i trin 2.
SET ANSI_NULL_DFLT_ON ON;
GO

-- Trin 1.B: Opret tabeller
-- IDENTITY sættes her, fordi det er en egenskab på kolonnen og ikke en constraint, der kan tilføjes senere.

CREATE TABLE dbo.Continent (
    Id INT IDENTITY(1,1),
    Name NVARCHAR(64)
);
GO

CREATE TABLE dbo.Country (
    Id INT IDENTITY(1,1),
    Continent_Id INT,
    Name NVARCHAR(64)
);
GO

CREATE TABLE dbo.City (
    Id INT IDENTITY(1,1),
    Country_Id INT,
    Name NVARCHAR(64)
);
GO

CREATE TABLE dbo.Activity (
    Id INT IDENTITY(1,1),
    City_Id INT,
    Name NVARCHAR(128),
    Description NVARCHAR(1024)
);
GO

CREATE TABLE dbo.Trip (
    Id INT IDENTITY(1,1),
    StartDate DATETIME2,
    EndDate DATETIME2,
    IsCompleted BIT,
    ExperienceNotes NVARCHAR(1024),
    PlanNotes NVARCHAR(1024)
);
GO

CREATE TABLE dbo.TripStop (
    City_Id INT,
    Trip_Id INT
);
GO

CREATE TABLE dbo.TripActivity (
    Trip_Id INT,
    Activity_Id INT
);
GO

CREATE TABLE dbo.[User] (
    Id INT IDENTITY(1,1),
    FirstName NVARCHAR(128),
    LastName NVARCHAR(128),
    Birthday DATE,
    IsPrivate BIT,
    CreatedAt DATETIME2,
    City_Id INT
);
GO

CREATE TABLE dbo.TripParticipant (
    User_Id INT,
    Trip_Id INT
);
GO

CREATE TABLE dbo.Review (
    Id INT IDENTITY(1,1),
    Trip_Id INT,
    User_Id INT,
    Content NVARCHAR(1024),
    Stars TINYINT
);
GO

CREATE TABLE dbo.Post (
    Id INT IDENTITY(1,1),
    User_Id INT,
    Content NVARCHAR(1024)
);
GO

CREATE TABLE dbo.Friendship (
    Id INT IDENTITY(1,1),
    UserA_Id INT,
    UserB_Id INT,
    StartDate DATETIME2
);
GO

CREATE TABLE dbo.Achievement (
    Id INT IDENTITY(1,1),
    Name NVARCHAR(128),
    Description NVARCHAR(1024)
);
GO

CREATE TABLE dbo.EarnedAchievement (
    User_Id INT,
    Achievement_Id INT,
    EarnedAt DATETIME2
);
GO

CREATE TABLE dbo.InvitationStatus (
    Id INT IDENTITY(1,1),
    StatusName NVARCHAR(64)
);
GO

CREATE TABLE dbo.Invite (
    Id INT IDENTITY(1,1),
    Trip_Id INT,
    Receiver_Id INT,
    Sender_Id INT,
    InvitationStatus_Id INT,
    SendDate DATETIME2
);
GO

CREATE TABLE dbo.[Group] (
    Id INT IDENTITY(1,1),
    Name NVARCHAR(64),
    Description NVARCHAR(64)
);
GO

CREATE TABLE dbo.GroupMembership (
    User_Id INT,
    Group_Id INT
);
GO

CREATE TABLE dbo.GroupTrip (
    Trip_Id INT,
    Group_Id INT
);
GO