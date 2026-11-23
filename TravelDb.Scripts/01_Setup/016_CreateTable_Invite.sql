CREATE TABLE dbo.Invite (
    Id INT IDENTITY(1,1),
    Trip_Id INT,
    Receiver_Id INT,
    Sender_Id INT,
    IsAccepted BIT NULL,
    SendDate DATETIME2
);