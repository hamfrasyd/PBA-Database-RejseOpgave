-- Definerer en liste af bruger-ID'er, som kan sendes til achievement-proceduren.

IF TYPE_ID(N'dbo.AchievementUserIds') IS NULL
BEGIN
    EXEC(N'
        CREATE TYPE dbo.AchievementUserIds AS TABLE (
            User_Id INT NOT NULL PRIMARY KEY
        );
    ');
END;