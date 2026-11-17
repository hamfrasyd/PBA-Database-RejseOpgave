-- Giver backendrollen læseadgang til loginopslag og profilviewet.
-- Kontotabellen oprettes i 10_AccountsAndAccess/002_CreateUserAccount.sql.

SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    GRANT SELECT ON OBJECT::[dbo].[UserAccount] TO [traveldb_backend];
    GRANT SELECT ON OBJECT::[dbo].[vw_UserProfile] TO [traveldb_backend];

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;