-- Opretter en audittabel for ændringer på anmeldelser.
-- Anmeldelsens id gemmes uden fremmednøgle, så auditposten
-- bevares, når anmeldelsen slettes.

IF OBJECT_ID(N'dbo.ReviewAudit', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ReviewAudit (
        Id BIGINT IDENTITY(1, 1) PRIMARY KEY,
        ReviewId INT NOT NULL,
        ActionName VARCHAR(6) NOT NULL,
        ChangedAtUtc DATETIME2(7) NOT NULL,
        LoginName SYSNAME NOT NULL,
        OldStars TINYINT NULL,
        NewStars TINYINT NULL,

        CONSTRAINT CK_ReviewAudit_ActionName
            CHECK (ActionName IN ('INSERT', 'UPDATE', 'DELETE'))
    );
END;