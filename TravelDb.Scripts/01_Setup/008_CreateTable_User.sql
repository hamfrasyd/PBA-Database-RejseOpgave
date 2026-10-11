CREATE TABLE dbo.[User] (
    Id INT IDENTITY(1,1),
    FirstName NVARCHAR(128),
    LastName NVARCHAR(128),
    Birthday DATE,
    IsPrivate BIT,
    CreatedAt DATETIME2,
    City_Id INT
);