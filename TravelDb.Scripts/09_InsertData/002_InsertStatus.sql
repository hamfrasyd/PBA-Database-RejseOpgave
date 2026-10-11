-- Indsætter de fire invitationsstatusser, som systemet bruger.
-- Test: SELECT Id, StatusName FROM dbo.InvitationStatus;

SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    INSERT INTO dbo.InvitationStatus (StatusName)
    SELECT StatusDefinition.StatusName
    FROM (VALUES
        (N'Pending'),
        (N'Accepted'),
        (N'Declined'),
        (N'Expired')
    ) AS StatusDefinition (StatusName)
    WHERE NOT EXISTS (
        SELECT 1
        FROM dbo.InvitationStatus AS ExistingStatus
        WHERE ExistingStatus.StatusName = StatusDefinition.StatusName
    );

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;

SELECT
    Id,
    StatusName
FROM dbo.InvitationStatus;
