-- Opretter to appkonti til den lokale TravelDB-demo.
-- Hashene bruger samme format som .NETs PasswordHasher.

SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    -- Kontrollér, at kontiene knyttes til de forventede testprofiler.
    IF NOT EXISTS (SELECT 1 FROM dbo.[User] WHERE Id = 1 AND FirstName = N'Anna')
        OR NOT EXISTS (SELECT 1 FROM dbo.[User] WHERE Id = 2 AND FirstName = N'Oliver')
        THROW 51120, N'Demokontiene kræver Anna med id 1 og Oliver med id 2.', 1;

    -- Opret Anna som almindelig bruger, hvis profilen endnu ikke har en konto.
    IF NOT EXISTS (SELECT 1 FROM dbo.UserAccount WHERE TravelUserId = 1)
    BEGIN
        INSERT INTO dbo.UserAccount
            (TravelUserId, UserName, NormalizedUserName, PasswordHash, Role)
        VALUES (
            1,
            N'anna.test',
            N'ANNA.TEST',
            N'AQAAAAIAAYagAAAAENekIzggAUbAKqLrEqKV4nF6PH7tRT0VZFZ2vJHh3UW2CbotG2reTzmCP6EuqmX1+w==',
            N'User'
        );
    END;

    -- Opret Oliver som app-admin, hvis profilen endnu ikke har en konto.
    IF NOT EXISTS (SELECT 1 FROM dbo.UserAccount WHERE TravelUserId = 2)
    BEGIN
        INSERT INTO dbo.UserAccount
            (TravelUserId, UserName, NormalizedUserName, PasswordHash, Role)
        VALUES (
            2,
            N'oliver.test',
            N'OLIVER.TEST',
            N'AQAAAAIAAYagAAAAEKEGH5emS9mq/92psze05c3F2p2kCjCI4ej18SYpEDWKWia54fYIukmekaUSoFEQ1Q==',
            N'Admin'
        );
    END;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;