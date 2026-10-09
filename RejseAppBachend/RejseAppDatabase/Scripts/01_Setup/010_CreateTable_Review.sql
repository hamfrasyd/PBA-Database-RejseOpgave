CREATE TABLE dbo.Review (
    Id INT IDENTITY(1,1),
    Trip_Id INT,
    User_Id INT,
    Content NVARCHAR(1024),
    Stars TINYINT
);