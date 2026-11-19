-- Giver backendrollen adgang til login, profilopslag og registrering.
-- Kontotabellen oprettes i 10_AccountsAndAccess/002_CreateUserAccount.sql.

SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    -- Login og profilopslag.
    GRANT SELECT ON OBJECT::[dbo].[UserAccount] TO [traveldb_backend];
    GRANT SELECT ON OBJECT::[dbo].[vw_UserProfile] TO [traveldb_backend];

    -- Registrering slår hjembyen op sammen med dens land.
    GRANT SELECT ON OBJECT::[dbo].[City] TO [traveldb_backend];
    GRANT SELECT ON OBJECT::[dbo].[Country] TO [traveldb_backend];

    -- EF læser det genererede profil-id og knytter kontoen til profilen.
    GRANT SELECT ON OBJECT::[dbo].[User] ([Id]) TO [traveldb_backend];
    GRANT INSERT ON OBJECT::[dbo].[User] TO [traveldb_backend];
    GRANT INSERT ON OBJECT::[dbo].[UserAccount] TO [traveldb_backend];

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;