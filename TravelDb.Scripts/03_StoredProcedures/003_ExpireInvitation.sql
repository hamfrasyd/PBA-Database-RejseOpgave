-- Ændrer invitationer fra Pending til Expired, når de er mindst 14 dage gamle.
-- Bruger UTC som standard og returnerer antallet af opdaterede invitationer via en outputparameter.

CREATE OR ALTER PROCEDURE dbo.ExpireInvitations
    @AsOfUtc datetime2 = NULL,
    @ExpiredCount int = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    SET @ExpiredCount = 0;

    IF @AsOfUtc IS NULL
        SET @AsOfUtc = SYSUTCDATETIME();

    DECLARE @PendingId int =
    (
        SELECT Id
        FROM dbo.InvitationStatus
        WHERE StatusName = N'Pending'
    );

    DECLARE @ExpiredId int =
    (
        SELECT Id
        FROM dbo.InvitationStatus
        WHERE StatusName = N'Expired'
    );

    IF @PendingId IS NULL OR @ExpiredId IS NULL
        THROW 51002, N'Pending eller Expired mangler.', 1;

    UPDATE dbo.Invite
    SET InvitationStatus_Id = @ExpiredId
    WHERE InvitationStatus_Id = @PendingId
    AND SendDate <= DATEADD(DAY, -14, @AsOfUtc);

    SET @ExpiredCount = @@ROWCOUNT;
END;