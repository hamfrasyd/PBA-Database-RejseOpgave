-- Opretter SQL Server-logins til den lokale TravelDB-demo.
-- Eksisterende logins beholder deres adgangskoder.

IF SUSER_ID(N'TravelDbReadOnly') IS NULL
BEGIN
    CREATE LOGIN [TravelDbReadOnly]
        WITH PASSWORD = 'Lokal!ReadOnly',
             CHECK_POLICY = ON;
END;

IF SUSER_ID(N'TravelDbRestrictedReader') IS NULL
BEGIN
    CREATE LOGIN [TravelDbRestrictedReader]
        WITH PASSWORD = 'Lokal!ResReader',
             CHECK_POLICY = ON;
END;

IF SUSER_ID(N'TravelDbBackend') IS NULL
BEGIN
    CREATE LOGIN [TravelDbBackend]
        WITH PASSWORD = 'Lokal!Backend',
             CHECK_POLICY = ON;
END;