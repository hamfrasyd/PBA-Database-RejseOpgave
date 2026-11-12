SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    GRANT EXECUTE ON OBJECT::[dbo].[AddReview]
        TO [traveldb_executor];

    GRANT EXECUTE ON OBJECT::[dbo].[CreateGroupWithMembers]
        TO [traveldb_executor];

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;