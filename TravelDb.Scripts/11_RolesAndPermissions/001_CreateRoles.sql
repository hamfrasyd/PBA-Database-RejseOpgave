-- Opretter databaseroller til begrænset læsning, almindelig læsning
-- og backendens databaseadgang.
-- Opretter kun roller, der mangler i databasen.

SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    -- Reader får læseadgang til destinationsstatistik.
    IF DATABASE_PRINCIPAL_ID(N'traveldb_reader') IS NULL
        CREATE ROLE [traveldb_reader] AUTHORIZATION [dbo];

    -- Read-only får ekstra læseadgang til de aftalte forretningsdata.
    IF DATABASE_PRINCIPAL_ID(N'traveldb_readonly') IS NULL
        CREATE ROLE [traveldb_readonly] AUTHORIZATION [dbo];

    -- Backend får de rettigheder, API'ets databasekald kræver.
    IF DATABASE_PRINCIPAL_ID(N'traveldb_backend') IS NULL
        CREATE ROLE [traveldb_backend] AUTHORIZATION [dbo];

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    -- Rul roller oprettet under denne kørsel tilbage, hvis et trin fejler.
    IF XACT_STATE() <> 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;