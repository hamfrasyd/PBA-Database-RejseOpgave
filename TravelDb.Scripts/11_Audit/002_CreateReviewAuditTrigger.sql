-- Logger oprettelse, ændring og sletning af anmeldelser.
-- Gemmer én auditpost pr. berørt anmeldelse.

CREATE OR ALTER TRIGGER dbo.trg_Review_Audit
ON dbo.Review
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Brug samme tidspunkt og login for alle rækker i handlingen.
    DECLARE @ChangedAtUtc DATETIME2(7) = SYSUTCDATETIME();
    DECLARE @LoginName SYSNAME = ORIGINAL_LOGIN();

    -- Sammenhold anmeldelsens værdier før og efter handlingen.
    INSERT INTO dbo.ReviewAudit (
        ReviewId,
        ActionName,
        ChangedAtUtc,
        LoginName,
        OldStars,
        NewStars
    )
    SELECT
        COALESCE(NewReview.Id, OldReview.Id),
        CASE
            WHEN OldReview.Id IS NULL THEN 'INSERT'
            WHEN NewReview.Id IS NULL THEN 'DELETE'
            ELSE 'UPDATE'
        END,
        @ChangedAtUtc,
        @LoginName,
        OldReview.Stars,
        NewReview.Stars
    FROM inserted AS NewReview
    FULL OUTER JOIN deleted AS OldReview
        ON OldReview.Id = NewReview.Id;
END;