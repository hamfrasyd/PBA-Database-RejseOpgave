-- Run manually in SSMS/sqlcmd, only against a local disposable TravelDb instance.
-- IMPORTANT: This file is intentionally outside TravelDb.Scripts/ so DbUp won't auto-run it.
-- Do not run this against production or shared databases.

USE [master];
GO

IF DB_ID(N'TravelDb') IS NULL
BEGIN
    THROW 50000, 'TravelDb does not exist. Run the original TravelDb.Setup first.', 1;
END;
GO

-- A restricted LOGIN is important: the unsafe endpoint can execute arbitrary injected SQL.
-- It must never be able to alter existing TravelDb application tables.
IF NOT EXISTS (SELECT 1 FROM sys.sql_logins WHERE name = N'SQLInjectionTestLabLogin')
BEGIN
    CREATE LOGIN [SQLInjectionTestLabLogin]
        WITH PASSWORD = 'SQLInjectionTest!2026LocalOnly', CHECK_POLICY = OFF;
END;
GO

USE [TravelDb];
GO

IF SCHEMA_ID(N'SQLInjectionTest') IS NULL
BEGIN
    EXEC(N'CREATE SCHEMA [SQLInjectionTest] AUTHORIZATION [dbo]');
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = N'SQLInjectionTestLabUser')
BEGIN
    CREATE USER [SQLInjectionTestLabUser] FOR LOGIN [SQLInjectionTestLabLogin];
END;
GO

-- This user can create/drop/read tables only in the lab schema, not in dbo.
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
GO

IF OBJECT_ID(N'SQLInjectionTest.Disposable', N'U') IS NULL
BEGIN
    CREATE TABLE [SQLInjectionTest].[Disposable]
    (
        Id INT NOT NULL PRIMARY KEY,
        Description NVARCHAR(200) NOT NULL
    );
END;
GO

DELETE FROM [SQLInjectionTest].[Users];
INSERT INTO [SQLInjectionTest].[Users] (Name)
VALUES (N'Alice'), (N'Bob'), (N'Charlie');

DELETE FROM [SQLInjectionTest].[Disposable];
INSERT INTO [SQLInjectionTest].[Disposable] (Id, Description)
VALUES (1, N'This table is intentionally safe to DROP in the lab.');
GO
