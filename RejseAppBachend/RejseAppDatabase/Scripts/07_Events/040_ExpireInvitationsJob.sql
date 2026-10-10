IF EXISTS (
SELECT 1
FROM msdb.dbo.sysjobs
WHERE name = N'TravelDb - Expire invitations'
)
BEGIN
PRINT N'Jobbet findes allerede.';
RETURN;
END;

DECLARE @JobId uniqueidentifier;
DECLARE @ScheduleId int;

EXEC msdb.dbo.sp_add_job
@job_name = N'TravelDb - Expire invitations',
@enabled = 1,
@description = N'Markerer Pending-invitationer som Expired efter 14 dage.',
@job_id = @JobId OUTPUT;

EXEC msdb.dbo.sp_add_jobstep
@job_id = @JobId,
@step_name = N'Udløb gamle invitationer',
@subsystem = N'TSQL',
@database_name = N'TravelDb',
@command = N'
DECLARE @Changed int;

EXEC dbo.ExpireInvitations
@ExpiredCount = @Changed OUTPUT;

PRINT CONCAT(
N''Antal udløbne invitationer: '',
@Changed
);';

EXEC msdb.dbo.sp_add_schedule
@schedule_name = N'TravelDb - Invitations daily 02:00',
@enabled = 1,
@freq_type = 4,
@freq_interval = 1,
@active_start_time = 020000,
@schedule_id = @ScheduleId OUTPUT;

EXEC msdb.dbo.sp_attach_schedule
@job_id = @JobId,
@schedule_id = @ScheduleId;

EXEC msdb.dbo.sp_add_jobserver
@job_id = @JobId;