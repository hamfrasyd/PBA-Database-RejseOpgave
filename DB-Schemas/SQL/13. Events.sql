/*
    Opret proceduren, som det planlagte job skal kalde.
    Regel: Pending-invitationer udløber efter mindst 14 døgn.
    SendDate skal gemmes i UTC. Andre statusser ændres ikke.
    Gentagen kørsel ændrer ikke invitationer, der allerede er udløbet.
*/
CREATE OR ALTER PROCEDURE dbo.ExpireInvitations
    @AsOfUtc datetime2 = NULL,
    @ExpiredCount int = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    SET @ExpiredCount=0;
    IF @AsOfUtc IS NULL SET @AsOfUtc=SYSUTCDATETIME();
    DECLARE @PendingId int=(SELECT Id FROM dbo.InvitationStatus WHERE StatusName=N'Pending');
    DECLARE @ExpiredId int=(SELECT Id FROM dbo.InvitationStatus WHERE StatusName=N'Expired');
    IF @PendingId IS NULL OR @ExpiredId IS NULL
        THROW 51002,N'Pending eller Expired mangler. Kør fil 01.',1;

    UPDATE dbo.Invite
    SET InvitationStatus_Id=@ExpiredId
    WHERE InvitationStatus_Id=@PendingId
      AND SendDate<=DATEADD(DAY,-14,@AsOfUtc);
    SET @ExpiredCount=@@ROWCOUNT;
END;
GO

-- ------------------------------------------------------------
/*
    Denne opsætning er kun til lokal udvikling og skal ændres hvsi den skal implementeres i Azure
    Opretter en daglig opgave i SQL Server Agent.
    Opgaven udløber gamle invitationer kl. 02:00 efter serverens klokkeslæt.
    SQL Server Agent skal være startet.
*/
USE msdb;
GO
SET XACT_ABORT ON;

IF CONVERT(nvarchar(128), SERVERPROPERTY('Edition')) LIKE N'%Express%'
    THROW 51007, N'SQL Server Express understøtter ikke SQL Server Agent.', 1;

IF OBJECT_ID(N'TravelDb.dbo.ExpireInvitations', N'P') IS NULL
    THROW 51008, N'Opret TravelDb.dbo.ExpireInvitations først.', 1;

IF EXISTS (SELECT 1 FROM dbo.sysjobs WHERE name = N'TravelDb - Expire invitations')
BEGIN
    PRINT N'Jobbet findes allerede. Det er ikke ændret.';
    RETURN;
END;

BEGIN TRY
    BEGIN TRANSACTION;
    DECLARE @JobId uniqueidentifier, @ScheduleId int, @Result int;
    DECLARE @StartDate int = CONVERT(int, CONVERT(char(8), GETDATE(), 112));

    -- ------------------------------------------------------------
    -- Trin 1: Opret jobbet
    EXEC @Result = dbo.sp_add_job
        @job_name = N'TravelDb - Expire invitations',
        @enabled = 1,
        @description = N'Markerer Pending-invitationer som Expired efter 14 dage.',
        @job_id = @JobId OUTPUT;
    IF @Result <> 0 THROW 51009, N'Jobbet kunne ikke oprettes.', 1;

    -- Trin 2: Tilføj SQL-koden, som jobbet skal udføre
    EXEC @Result = dbo.sp_add_jobstep
        @job_id = @JobId,
        @step_name = N'Udløb gamle invitationer',
        @subsystem = N'TSQL',
        @database_name = N'TravelDb',
        @command = N'DECLARE @Changed int;
EXEC dbo.ExpireInvitations @ExpiredCount = @Changed OUTPUT;
DECLARE @Message nvarchar(200) = CONCAT(N''Antal udløbne invitationer: '', @Changed);
PRINT @Message;',
        @on_success_action = 1,
        @on_fail_action = 2,
        @retry_attempts = 3,
        @retry_interval = 5,
        @flags = 4;
    IF @Result <> 0 THROW 51010, N'Jobtrinnet kunne ikke oprettes.', 1;

    -- Trin 3: Kør én gang dagligt kl. 02:00 efter serverens klokkeslæt
    EXEC @Result = dbo.sp_add_schedule
        @schedule_name = N'TravelDb - Invitations daily 02:00',
        @enabled = 1,
        @freq_type = 4,
        @freq_interval = 1,
        @active_start_date = @StartDate,
        @active_start_time = 020000,
        @schedule_id = @ScheduleId OUTPUT;
    IF @Result <> 0 THROW 51011, N'Tidsplanen kunne ikke oprettes.', 1;

    EXEC @Result = dbo.sp_attach_schedule
        @job_id = @JobId, @schedule_id = @ScheduleId;
    IF @Result <> 0 THROW 51012, N'Tidsplanen kunne ikke tilknyttes.', 1;

    -- Trin 4: Tilknyt jobbet til denne SQL Server
    EXEC @Result = dbo.sp_add_jobserver @job_id = @JobId;
    IF @Result <> 0 THROW 51013, N'Jobbet kunne ikke tilknyttes serveren.', 1;
    COMMIT TRANSACTION;
    PRINT N'Jobbet er oprettet og aktiveret. SQL Server Agent skal være startet.';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
GO

-- ------------------------------------------------------------
/* Viser jobbets tidsplan og de seneste kørsler. */
USE msdb;
GO

-- Trin 1: Kontrollér, at jobbet og tidsplanen er aktiveret
SELECT j.name AS Jobnavn, j.enabled AS JobAktiveret,
       s.name AS Tidsplan, s.enabled AS TidsplanAktiveret,
       s.freq_type AS Frekvenstype, s.freq_interval AS Interval,
       s.active_start_time AS Starttid
FROM dbo.sysjobs j
LEFT JOIN dbo.sysjobschedules js ON js.job_id = j.job_id
LEFT JOIN dbo.sysschedules s ON s.schedule_id = js.schedule_id
WHERE j.name = N'TravelDb - Expire invitations';

-- Trin 2: Se de seneste resultater. Ingen rækker betyder, at der endnu ikke er historik.
SELECT TOP (20) h.step_id, h.step_name, h.run_date, h.run_time,
       CASE h.run_status
           WHEN 0 THEN N'Fejlet'
           WHEN 1 THEN N'Gennemført'
           WHEN 2 THEN N'Forsøges igen'
           WHEN 3 THEN N'Annulleret'
           ELSE N'Kører'
       END AS Resultat,
       h.message
FROM dbo.sysjobhistory h
JOIN dbo.sysjobs j ON j.job_id = h.job_id
WHERE j.name = N'TravelDb - Expire invitations'
ORDER BY h.instance_id DESC;