-- Indsætter de fem achievements, som tildelingsproceduren bruger.
-- Test: SELECT Id, Name, Description FROM dbo.Achievement;

SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    INSERT INTO dbo.Achievement (Name, Description)
    SELECT
        AchievementDefinition.Name,
        AchievementDefinition.Description
    FROM (VALUES
        (N'Første rejse', N'Gennemfør din første rejse.'),
        (N'Erfaren rejsende', N'Gennemfør fem forskellige rejser.'),
        (N'Byopdager', N'Besøg ti forskellige byer på gennemførte rejser.'),
        (N'Første anmeldelse', N'Skriv din første anmeldelse.'),
        (N'Social rejsende', N'Få fem forskellige venner.')
    ) AS AchievementDefinition (Name, Description)
    WHERE NOT EXISTS (
        SELECT 1
        FROM dbo.Achievement AS ExistingAchievement
        WHERE ExistingAchievement.Name = AchievementDefinition.Name
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
    Name,
    Description
FROM dbo.Achievement
WHERE Name IN (
    N'Første rejse',
    N'Erfaren rejsende',
    N'Byopdager',
    N'Første anmeldelse',
    N'Social rejsende'
);
