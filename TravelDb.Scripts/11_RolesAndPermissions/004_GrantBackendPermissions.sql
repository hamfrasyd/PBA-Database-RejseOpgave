-- Giver backendrollen adgang til de objekter og handlinger,
-- som backendens SQL-repositories anvender.
-- Destinationsstatistik tilgås gennem traveldb_reader.

SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    -- Login og registrering af appkonti.
    GRANT SELECT, INSERT
        ON OBJECT::[dbo].[UserAccount]
        TO [traveldb_backend];

    -- Profilopslag og profilsøgning.
    GRANT SELECT
        ON OBJECT::[dbo].[vw_UserProfile]
        TO [traveldb_backend];

    -- Læsning af kontinenter og lande.
    GRANT SELECT
        ON OBJECT::[dbo].[Continent]
        TO [traveldb_backend];

    GRANT SELECT
        ON OBJECT::[dbo].[Country]
        TO [traveldb_backend];

    -- Byopslag og oprettelse af byer.
    GRANT SELECT, INSERT
        ON OBJECT::[dbo].[City]
        TO [traveldb_backend];

    -- Læsning, oprettelse og sletning af aktiviteter.
    GRANT SELECT, INSERT, DELETE
        ON OBJECT::[dbo].[Activity]
        TO [traveldb_backend];

    -- Ændring af aktivitetens oplysninger.
    GRANT UPDATE ([Name], [Description], [City_Id])
        ON OBJECT::[dbo].[Activity]
        TO [traveldb_backend];

    -- Registrering, brugeropslag og sletning af profiler.
    GRANT SELECT, INSERT, DELETE
        ON OBJECT::[dbo].[User]
        TO [traveldb_backend];

    -- Fjern en eventuel tidligere UPDATE-tildeling på hele tabellen.
    REVOKE UPDATE
        ON OBJECT::[dbo].[User]
        FROM [traveldb_backend];

    -- Ændring af de profilfelter, backenden understøtter.
    GRANT UPDATE ([FirstName], [LastName], [IsPrivate], [City_Id])
        ON OBJECT::[dbo].[User]
        TO [traveldb_backend];

    -- Oprettelse, læsning og sletning af rejser.
    GRANT SELECT, INSERT, DELETE
        ON OBJECT::[dbo].[Trip]
        TO [traveldb_backend];

    -- Ændring af rejsens oplysninger.
    GRANT UPDATE (
        [StartDate],
        [EndDate],
        [IsCompleted],
        [PlanNotes],
        [ExperienceNotes]
    )
        ON OBJECT::[dbo].[Trip]
        TO [traveldb_backend];

    -- Læsning, tilføjelse og fjernelse af rejsedeltagere.
    GRANT SELECT, INSERT, DELETE
        ON OBJECT::[dbo].[TripParticipant]
        TO [traveldb_backend];

    -- Læsning, tilføjelse og fjernelse af destinationer på rejser.
    GRANT SELECT, INSERT, DELETE
        ON OBJECT::[dbo].[TripStop]
        TO [traveldb_backend];

    -- Læsning, tilføjelse og fjernelse af aktiviteter på rejser.
    GRANT SELECT, INSERT, DELETE
        ON OBJECT::[dbo].[TripActivity]
        TO [traveldb_backend];

    -- Oprettelse, læsning og sletning af anmeldelser.
    GRANT SELECT, INSERT, DELETE
        ON OBJECT::[dbo].[Review]
        TO [traveldb_backend];

    -- Ændring af anmeldelsens tekst og stjerner.
    GRANT UPDATE ([Content], [Stars])
        ON OBJECT::[dbo].[Review]
        TO [traveldb_backend];

    -- Oprettelse, læsning og sletning af opslag.
    GRANT SELECT, INSERT, DELETE
        ON OBJECT::[dbo].[Post]
        TO [traveldb_backend];

    -- Fjern en eventuel tidligere UPDATE-tildeling på hele tabellen.
    REVOKE UPDATE
        ON OBJECT::[dbo].[Post]
        FROM [traveldb_backend];

    -- Ændring af opslagets tekst.
    GRANT UPDATE ([Content])
        ON OBJECT::[dbo].[Post]
        TO [traveldb_backend];

    -- Læsning, oprettelse og sletning af venskaber.
    GRANT SELECT, INSERT, DELETE
        ON OBJECT::[dbo].[Friendship]
        TO [traveldb_backend];

    -- Læsning, oprettelse og sletning af grupper.
    GRANT SELECT, INSERT, DELETE
        ON OBJECT::[dbo].[Group]
        TO [traveldb_backend];

    -- Ændring af gruppens navn og beskrivelse.
    GRANT UPDATE ([Name], [Description])
        ON OBJECT::[dbo].[Group]
        TO [traveldb_backend];

    -- Læsning, tilføjelse og fjernelse af gruppemedlemmer.
    GRANT SELECT, INSERT, DELETE
        ON OBJECT::[dbo].[GroupMembership]
        TO [traveldb_backend];

    -- Læsning, tilføjelse og fjernelse af grupperejser.
    GRANT SELECT, INSERT, DELETE
        ON OBJECT::[dbo].[GroupTrip]
        TO [traveldb_backend];

    -- Læsning af achievement-definitioner og opnåede achievements.
    GRANT SELECT
        ON OBJECT::[dbo].[Achievement]
        TO [traveldb_backend];

    GRANT SELECT
        ON OBJECT::[dbo].[EarnedAchievement]
        TO [traveldb_backend];

    -- Afsendelse, læsning og sletning af invitationer.
    GRANT SELECT, INSERT, DELETE
        ON OBJECT::[dbo].[Invite]
        TO [traveldb_backend];

    -- Accept og afvisning af invitationer.
    GRANT UPDATE
        ON OBJECT::[dbo].[Invite]
        TO [traveldb_backend];

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    -- Rul rettighedstildelingerne tilbage, hvis et trin fejler.
    IF XACT_STATE() <> 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;