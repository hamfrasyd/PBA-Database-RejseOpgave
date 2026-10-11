/*
    Opretter en gruppe og tilføjer brugere som medlemmer.

    @MemberIds sendes som JSON:
    [1, 4, 8, 12]

    Hele operationen køres i én transaktion.
    Hvis noget fejler, bliver gruppen ikke oprettet.
*/

CREATE OR ALTER PROCEDURE dbo.CreateGroupWithMembers
    @Name NVARCHAR(255),
    @Description NVARCHAR(MAX),
    @MemberIds NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Gruppen skal have et navn
        IF @Name IS NULL OR LTRIM(RTRIM(@Name)) = N''
        BEGIN
            THROW 51010,
                N'Gruppen skal have et navn.',
                1;
        END;

        -- Kontrollér at MemberIds er gyldig JSON
        IF ISJSON(@MemberIds) <> 1
        BEGIN
            THROW 51011,
                N'MemberIds skal være et gyldigt JSON-array.',
                1;
        END;

        -- Gem medlemmerne midlertidigt
        DECLARE @Members TABLE (UserId INT PRIMARY KEY);

        INSERT INTO @Members (UserId)
        SELECT DISTINCT TRY_CAST([value] AS INT)
        FROM OPENJSON(@MemberIds)
        WHERE TRY_CAST([value] AS INT) IS NOT NULL;

        -- Der skal være mindst ét medlem
        IF NOT EXISTS (SELECT 1 FROM @Members)
        BEGIN
            THROW 51012,
                N'Gruppen skal have mindst ét medlem.',
                1;
        END;

        -- Kontrollér at alle brugere findes
        IF EXISTS (
            SELECT 1
            FROM @Members m
            WHERE NOT EXISTS (
                SELECT 1
                FROM dbo.[User] u
                WHERE u.Id = m.UserId
            )
        )
        BEGIN
            THROW 51013,
                N'En eller flere brugere findes ikke.',
                1;
        END;

        -- Opret gruppen
        INSERT INTO dbo.[Group] (Name, Description)
        VALUES (@Name, @Description);

        DECLARE @GroupId INT;

        SET @GroupId = CONVERT(INT, SCOPE_IDENTITY());

        -- Tilføj medlemmerne
        INSERT INTO dbo.GroupMembership (User_Id, Group_Id)
        SELECT UserId, @GroupId
        FROM @Members;

        COMMIT TRANSACTION;

        -- Returnér den nye gruppes ID
        SELECT @GroupId AS GroupId;
    END TRY

    BEGIN CATCH

        IF XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        THROW;

    END CATCH;
END;


-- Test
--EXEC dbo.CreateGroupWithMembers
--    @Name = N'Rejseholdet',
--    @Description = N'Vi planlægger en fælles tur til Japan.',
--    @MemberIds = N'[1, 2, 3, 4]';