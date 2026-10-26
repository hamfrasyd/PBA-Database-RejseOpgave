-- Opretter et dagligt job, der udløber invitationer efter 14 dage.
-- Test: EXEC msdb.dbo.sp_start_job @job_name = N'TravelDb - Expire invitations';

IF EXISTS (
    SELECT 1
    FROM msdb.dbo.sysjobs
    WHERE name = N'TravelDb - Expire invitations'
)
BEGIN
    PRINT N'Jobbet findes allerede.';
    RETURN;
END;

DECLARE @ExpirationJobId UNIQUEIDENTIFIER;
DECLARE @DailyScheduleId INT;

EXEC msdb.dbo.sp_add_job
    @job_name = N'TravelDb - Expire invitations',
    @enabled = 1,
    @description = N'Markerer Pending-invitationer som Expired efter 14 dage.',
    @job_id = @ExpirationJobId OUTPUT;

EXEC msdb.dbo.sp_add_jobstep
    @job_id = @ExpirationJobId,
    @step_name = N'Udløb gamle invitationer',
    @subsystem = N'TSQL',
    @database_name = N'TravelDb',
    @command = N'
        DECLARE @ExpiredInvitationCount INT;

        EXEC dbo.ExpireInvitations
            @ExpiredCount = @ExpiredInvitationCount OUTPUT;

        PRINT CONCAT(
            N''Antal udløbne invitationer: '',
            @ExpiredInvitationCount
        );
    ';

-- Jobbet kører kl. 02:00 efter SQL Serverens lokale tid.
EXEC msdb.dbo.sp_add_schedule
    @schedule_name = N'TravelDb - Invitations daily 02:00',
    @enabled = 1,
    @freq_type = 4,
    @freq_interval = 1,
    @active_start_time = 020000,
    @schedule_id = @DailyScheduleId OUTPUT;

EXEC msdb.dbo.sp_attach_schedule
    @job_id = @ExpirationJobId,
    @schedule_id = @DailyScheduleId;

EXEC msdb.dbo.sp_add_jobserver
    @job_id = @ExpirationJobId;
