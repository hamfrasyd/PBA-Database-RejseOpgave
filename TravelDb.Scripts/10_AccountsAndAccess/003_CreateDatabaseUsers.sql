-- Knytter de tre SQL Server-logins til brugere i TravelDb.

SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    IF USER_ID(N'TravelDbReadOnly') IS NULL
        CREATE USER [TravelDbReadOnly] FOR LOGIN [TravelDbReadOnly];

    IF USER_ID(N'TravelDbRestrictedReader') IS NULL
        CREATE USER [TravelDbRestrictedReader]
            FOR LOGIN [TravelDbRestrictedReader];

    IF USER_ID(N'TravelDbBackend') IS NULL
        CREATE USER [TravelDbBackend] FOR LOGIN [TravelDbBackend];

    -- Kontroller, at eksisterende brugere er knyttet til de forventede logins.
    IF EXISTS (
        SELECT 1
        FROM sys.database_principals AS DatabaseUser
        LEFT JOIN sys.server_principals AS ServerLogin
            ON ServerLogin.name = DatabaseUser.name
        WHERE DatabaseUser.name IN (
            N'TravelDbReadOnly',
            N'TravelDbRestrictedReader',
            N'TravelDbBackend'
        )
          AND (
              ServerLogin.sid IS NULL
              OR DatabaseUser.sid IS NULL
              OR DatabaseUser.sid <> ServerLogin.sid
          )
    )
        THROW 51105, N'En databasebruger er ikke knyttet til det forventede SQL-login.', 1;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;