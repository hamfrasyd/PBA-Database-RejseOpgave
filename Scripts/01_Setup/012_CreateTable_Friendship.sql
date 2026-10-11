CREATE TABLE dbo.Friendship (
    Id INT IDENTITY(1,1),
    UserA_Id INT,
    UserB_Id INT,
    StartDate DATETIME2
);