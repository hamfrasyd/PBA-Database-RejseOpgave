-- Viser afventende invitationer med afsender, modtager og udløbsdato.
-- Test: SELECT * FROM dbo.vw_PendingInvitations;

CREATE OR ALTER VIEW dbo.vw_PendingInvitations
AS
SELECT
    Invitation.Id AS InviteId,
    Invitation.Trip_Id AS TripId,
    Trip.StartDate AS TripStartDate,

    Sender.FirstName + N' ' + Sender.LastName AS SenderName,
    Receiver.FirstName + N' ' + Receiver.LastName AS ReceiverName,

    Invitation.SendDate,
    DATEADD(DAY, 14, Invitation.SendDate) AS ExpiresAt,

    DATEDIFF(
        DAY,
        SYSUTCDATETIME(),
        DATEADD(DAY, 14, Invitation.SendDate)
    ) AS DaysUntilExpiry

FROM dbo.Invite AS Invitation

JOIN dbo.InvitationStatus AS InvitationStatus
    ON InvitationStatus.Id = Invitation.InvitationStatus_Id

JOIN dbo.Trip AS Trip
    ON Trip.Id = Invitation.Trip_Id

JOIN dbo.[User] AS Sender
    ON Sender.Id = Invitation.Sender_Id

JOIN dbo.[User] AS Receiver
    ON Receiver.Id = Invitation.Receiver_Id

WHERE InvitationStatus.StatusName = N'Pending';
