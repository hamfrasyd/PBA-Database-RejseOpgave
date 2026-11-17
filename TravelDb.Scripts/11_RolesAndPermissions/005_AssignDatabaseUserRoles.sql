-- Knytter de tre databasebrugere til deres roller i TravelDb.

SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    -- Alle tre brugere får adgang til destinationsstatistik.
    IF IS_ROLEMEMBER(N'traveldb_reader', N'TravelDbRestrictedReader') = 0
        ALTER ROLE [traveldb_reader] ADD MEMBER [TravelDbRestrictedReader];

    IF IS_ROLEMEMBER(N'traveldb_reader', N'TravelDbReadOnly') = 0
        ALTER ROLE [traveldb_reader] ADD MEMBER [TravelDbReadOnly];

    IF IS_ROLEMEMBER(N'traveldb_reader', N'TravelDbBackend') = 0
        ALTER ROLE [traveldb_reader] ADD MEMBER [TravelDbBackend];

    -- Den almindelige læser får adgang til de aftalte forretningsdata.
    IF IS_ROLEMEMBER(N'traveldb_readonly', N'TravelDbReadOnly') = 0
        ALTER ROLE [traveldb_readonly] ADD MEMBER [TravelDbReadOnly];

    -- Backenden får adgang til loginopslag og profilviewet.
    IF IS_ROLEMEMBER(N'traveldb_backend', N'TravelDbBackend') = 0
        ALTER ROLE [traveldb_backend] ADD MEMBER [TravelDbBackend];

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;