-- Giver read-only-rollen læseadgang til de aftalte forretningstabeller.

SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    GRANT SELECT ON OBJECT::[dbo].[Continent] TO [traveldb_readonly];
    GRANT SELECT ON OBJECT::[dbo].[Country] TO [traveldb_readonly];
    GRANT SELECT ON OBJECT::[dbo].[City] TO [traveldb_readonly];
    GRANT SELECT ON OBJECT::[dbo].[Activity] TO [traveldb_readonly];
    GRANT SELECT ON OBJECT::[dbo].[User] TO [traveldb_readonly];

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;