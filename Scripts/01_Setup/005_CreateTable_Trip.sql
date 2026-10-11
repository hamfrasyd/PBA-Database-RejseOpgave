CREATE TABLE dbo.Trip (
    Id INT IDENTITY(1,1),
    StartDate DATETIME2,
    EndDate DATETIME2,
    IsCompleted BIT,
    ExperienceNotes NVARCHAR(1024),
    PlanNotes NVARCHAR(1024)
);