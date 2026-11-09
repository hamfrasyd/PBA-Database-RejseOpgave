-- SQL Injection demo setup. Execute only on your own local SQL Server.
-- Uses an isolated schema and a restricted login in the existing TravelDb database.
-- IMPORTANT: Never use sa or an application login in the API connection string.

USE [master];
GO

IF DB_ID(N'TravelDb') IS NULL
BEGIN
    THROW 50000, N'TravelDb is missing. Run TravelDb.Setup before this script.', 1;
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.sql_logins WHERE name = N'SQLInjectionTestLabLogin')
BEGIN
    CREATE LOGIN [SQLInjectionTestLabLogin]
        WITH PASSWORD = 'SQLInjectionTest!2026LocalOnly',
             CHECK_POLICY = OFF,
             CHECK_EXPIRATION = OFF;
END;
GO

USE [TravelDb];
GO

IF SCHEMA_ID(N'SQLInjectionTest') IS NULL
BEGIN
    EXEC(N'CREATE SCHEMA [SQLInjectionTest] AUTHORIZATION [dbo]');
END;
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.database_principals
    WHERE name = N'SQLInjectionTestLabUser'
)
BEGIN
    CREATE USER [SQLInjectionTestLabUser] FOR LOGIN [SQLInjectionTestLabLogin];
END;
GO

-- The account may create/drop/read only lab objects in SQLInjectionTest schema.
GRANT CONNECT TO [SQLInjectionTestLabUser];
GRANT CREATE TABLE TO [SQLInjectionTestLabUser];
GRANT CONTROL ON SCHEMA::[SQLInjectionTest] TO [SQLInjectionTestLabUser];
GO

IF OBJECT_ID(N'SQLInjectionTest.Users', N'U') IS NULL
BEGIN
    CREATE TABLE [SQLInjectionTest].[Users]
    (
        Id INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
        Name NVARCHAR(128) NOT NULL
    );
END;

IF OBJECT_ID(N'SQLInjectionTest.Disposable', N'U') IS NULL
BEGIN
    CREATE TABLE [SQLInjectionTest].[Disposable]
    (
        Id INT NOT NULL PRIMARY KEY,
        Description NVARCHAR(200) NOT NULL
    );
END;

DELETE FROM [SQLInjectionTest].[Users];
INSERT INTO [SQLInjectionTest].[Users] (Name)
VALUES (N'Alice'), (N'Bob'), (N'Charlie');

DELETE FROM [SQLInjectionTest].[Disposable];
INSERT INTO [SQLInjectionTest].[Disposable] (Id, Description)
VALUES (1, N'This lab table may be dropped and recreated.');
GO

SELECT Id, Name FROM [SQLInjectionTest].[Users];
SELECT OBJECT_ID(N'SQLInjectionTest.Disposable', N'U') AS DisposableTableObjectId;
GO
