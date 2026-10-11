-- 4. Åbne invitationer
CREATE OR ALTER VIEW dbo.vw_PendingInvitations
AS
SELECT
    i.Id                                    AS InviteId,
    i.Trip_Id                               AS TripId,
    t.StartDate                             AS TripStartDate,
    s.FirstName + N' ' + s.LastName         AS SenderName,
    r.FirstName + N' ' + r.LastName         AS ReceiverName,
    i.SendDate,
    DATEADD(DAY, 14, i.SendDate)            AS ExpiresAt,
    DATEDIFF(DAY, SYSUTCDATETIME(), DATEADD(DAY, 14, i.SendDate)) AS DaysUntilExpiry
FROM dbo.Invite i
JOIN dbo.InvitationStatus st ON st.Id = i.InvitationStatus_Id
JOIN dbo.Trip t              ON t.Id  = i.Trip_Id
JOIN dbo.[User] s            ON s.Id  = i.Sender_Id
JOIN dbo.[User] r            ON r.Id  = i.Receiver_Id
WHERE st.StatusName = N'Pending';
