/*
    Opbygning:
    1. Kontrollér eksisterende navne
    2. Gør achievement-navne unikke
    3. Opret de fem achievements

    Noter:
    - Kør hele filen i en konsol for TravelDb.
    - Name bruges af tildelingsproceduren. Navnene skal derfor være unikke.
    - Hvis et navn ændres, skal proceduren og testene også opdateres.
    - Id er stadig primærnøglen og bruges af EarnedAchievement.
    - Filen kan genkøres. Eksisterende matchende achievements beholder deres Id.
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    -- ------------------------------------------------------------
    -- Trin 1: Kontrollér navne.
    -- Afbryd ved dubletter, så eksisterende tildelinger ikke sammenlægges vilkårligt.
    IF EXISTS (
        SELECT Name FROM dbo.Achievement GROUP BY Name HAVING COUNT(*)>1
    )
        THROW 50100,N'Der findes achievement-navne flere gange. Afklar dubletterne først.',1;

    IF EXISTS (SELECT 1 FROM dbo.Achievement WHERE Name=N'First completed trip')
       AND EXISTS (SELECT 1 FROM dbo.Achievement WHERE Name=N'Første rejse')
        THROW 50101,N'Både First completed trip og Første rejse findes. Afklar hvilken række der skal bruges.',1;

    UPDATE dbo.Achievement
    SET Name=N'Første rejse'
    WHERE Name=N'First completed trip';

    -- ------------------------------------------------------------
    -- Trin 2: Tilføj UNIQUE på Name, hvis denne constraint ikke allerede findes.
    IF NOT EXISTS (
        SELECT 1 FROM sys.key_constraints
        WHERE parent_object_id=OBJECT_ID(N'dbo.Achievement')
          AND name=N'UQ_Achievement_Name'
    )

    -- ------------------------------------------------------------
    -- Trin 3: Indsæt kun de navne, som endnu ikke findes.
    INSERT dbo.Achievement(Name,Description)
    SELECT d.Name,d.Description
    FROM (VALUES
        (N'Første rejse',N'Gennemfør din første rejse.'),
        (N'Erfaren rejsende',N'Gennemfør fem forskellige rejser.'),
        (N'Byopdager',N'Besøg ti forskellige byer på gennemførte rejser.'),
        (N'Første anmeldelse',N'Skriv din første anmeldelse.'),
        (N'Social rejsende',N'Få fem forskellige venner.')
    ) AS d(Name,Description)
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.Achievement a WHERE a.Name=d.Name
    );

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE()<>0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
GO
SELECT Id,Name,Description
FROM dbo.Achievement
WHERE Name IN (N'Første rejse',N'Erfaren rejsende',N'Byopdager',N'Første anmeldelse',N'Social rejsende');
