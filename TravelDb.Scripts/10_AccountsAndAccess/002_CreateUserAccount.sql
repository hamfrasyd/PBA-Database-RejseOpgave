-- Opretter login-konti og knytter hver konto til én TravelDB-profil.
-- Opretter kun tabellen, hvis den mangler i databasen.

IF OBJECT_ID(N'dbo.UserAccount', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.UserAccount (
        TravelUserId INT NOT NULL,
        UserName NVARCHAR(128) NOT NULL,
        NormalizedUserName NVARCHAR(128) NOT NULL,
        PasswordHash NVARCHAR(512) NOT NULL,
        Role NVARCHAR(16) NOT NULL,

        CONSTRAINT PK_UserAccount PRIMARY KEY (TravelUserId),
        CONSTRAINT UQ_UserAccount_NormalizedUserName UNIQUE (NormalizedUserName),
        CONSTRAINT FK_UserAccount_User
            FOREIGN KEY (TravelUserId) REFERENCES dbo.[User](Id),
        CONSTRAINT CK_UserAccount_Role CHECK (Role IN (N'User', N'Admin'))
    );
END;