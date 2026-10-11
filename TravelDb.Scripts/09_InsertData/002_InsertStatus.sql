/*
    Trin 1: Opret de statusværdier, proceduren og testen bruger.
    Kør mod TravelDb. Eksisterende matchende statusser genbruges.
*/
SET XACT_ABORT ON;
BEGIN TRY
    BEGIN TRANSACTION;
    IF EXISTS (SELECT StatusName FROM dbo.InvitationStatus GROUP BY StatusName HAVING COUNT(*)>1)
        THROW 51001,N'Der findes dubletter af statusnavne. Afklar dem først.',1;
    IF NOT EXISTS (SELECT 1 FROM sys.key_constraints
        WHERE parent_object_id=OBJECT_ID(N'dbo.InvitationStatus') AND name=N'UQ_InvitationStatus_StatusName')
    INSERT dbo.InvitationStatus(StatusName)
    SELECT d.Name FROM (VALUES(N'Pending'),(N'Accepted'),(N'Declined'),(N'Expired')) d(Name)
    WHERE NOT EXISTS (SELECT 1 FROM dbo.InvitationStatus s WHERE s.StatusName=d.Name);
    COMMIT;
END TRY
BEGIN CATCH
    IF XACT_STATE()<>0 ROLLBACK;
    THROW;
END CATCH;
SELECT Id,StatusName FROM dbo.InvitationStatus;
