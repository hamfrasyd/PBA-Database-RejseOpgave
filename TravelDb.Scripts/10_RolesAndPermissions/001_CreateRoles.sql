-- Opretter databaseroller til læsning, direkte skrivning og procedurekald.
-- Opretter kun roller, der mangler i databasen.

SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    -- Reader får læseadgang til udvalgte views og tabeller.
    IF DATABASE_PRINCIPAL_ID(N'traveldb_reader') IS NULL
        CREATE ROLE [traveldb_reader] AUTHORIZATION [dbo];

    -- Writer oprettes uden rettigheder, indtil et endpoint kræver direkte skrivning.
    IF DATABASE_PRINCIPAL_ID(N'traveldb_writer') IS NULL
        CREATE ROLE [traveldb_writer] AUTHORIZATION [dbo];

    -- Executor får adgang til at kalde udvalgte stored procedures.
    IF DATABASE_PRINCIPAL_ID(N'traveldb_executor') IS NULL
        CREATE ROLE [traveldb_executor] AUTHORIZATION [dbo];

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    -- Rul roller oprettet under denne kørsel tilbage, hvis et trin fejler.
    IF XACT_STATE() <> 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;