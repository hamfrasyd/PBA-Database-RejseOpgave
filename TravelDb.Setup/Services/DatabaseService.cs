using Microsoft.Data.SqlClient;

namespace TravelDb.Setup.Services;

//DatabaseService er ansvarlig for at håndtere database-relaterede operationer, såsom at vente på, at SQL Server er klar, og oprette databasen, hvis den mangler.
public class DatabaseService
{
    //WaitUntilReady tester forbindelsen til SQL Server ved at forsøge at åbne en forbindelse. 
    //Hvis forbindelsen ikke can åbnes, venter den i 2 sekunder og prøver igen, indtil timeout-perioden på 60 sekunder er nået.
    public async Task WaitUntilReadyAsync(string connectionString)
    {
        TimeSpan timeout = TimeSpan.FromSeconds(60);
        DateTime startTime = DateTime.UtcNow;

        while (DateTime.UtcNow - startTime < timeout)
        {
            try
            {
                // Prøver at åbne en forbindelse til SQL Server
                await using SqlConnection connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                return; // Hvis forbindelsen åbnes, returnerer vi og afslutter metoden
            }
            catch (SqlException)
            {
                // Hvis forbindelsen mislykkes, venter vi i 2 sekunder, før vi prøver igen
                await Task.Delay(2000);
            }
        }
        throw new TimeoutException("SQL Server blev ikke klar inden for 60 sekunder.");
    }

    //CreateDatabaseIfMissingAsync laver en ny database, hvis den ikke allerede findes. 
    //Den bruger master-databasen til at kontrollere, om databasen eksisterer, og opretter den, hvis den ikke gør.
    public async Task CreateDatabaseIfMissingAsync(string masterConnectionString, string databaseName)
    {
        // Opretter en forbindelse til master-databasen
        await using SqlConnection connection = new SqlConnection(masterConnectionString);
        await connection.OpenAsync();

        var sql = """
            IF DB_ID(@DatabaseName) IS NULL
            BEGIN
                DECLARE @Sql NVARCHAR(MAX);

                SET @Sql =
                    'CREATE DATABASE [' +
                    REPLACE(@DatabaseName, ']', ']]') +
                    ']';

                EXEC(@Sql);
            END
            """;

        await using SqlCommand command = new SqlCommand(sql, connection);

        //@DatabaseName parameteren bruges til at undgå SQL-injektion og sikre, at databasenavnet håndteres korrekt.
        // AddWithValue tilføjer parameteren til SQL-kommandoen med det angivne databasenavn.
        command.Parameters.AddWithValue("@DatabaseName", databaseName);
        await command.ExecuteNonQueryAsync();
    }

    /// <summary>
    /// BackupDatabaseAsync laver en fuld backup af databasen.
    /// Backup-filen bliver skrevet på SQL Server-siden, dvs. inde i Docker-containeren.
    /// </summary>
    public async Task BackupDatabaseAsync(string masterConnectionString, string databaseName, string backupFilePath)
    {
        // Opretter en forbindelse til master-databasen
        await using SqlConnection connection = new SqlConnection(masterConnectionString);

        // Udskriver STATS-beskeder, som viser progressen af backup-operationen
        connection.InfoMessage += (_, e) => Console.WriteLine(e.Message);
        await connection.OpenAsync();

        string safeName = databaseName.Replace("]", "]]");

        string backupDatabaseSql = $"""
        BACKUP DATABASE [{safeName}]
        TO DISK = @BackupPath
        WITH FORMAT, INIT, CHECKSUM, STATS = 10,
             NAME = @BackupName;
        """;

        await using SqlCommand command = new SqlCommand(backupDatabaseSql, connection);

        command.CommandTimeout = 1800; // Timeout på 30 min

        command.Parameters.AddWithValue("@BackupPath", backupFilePath);
        command.Parameters.AddWithValue("@BackupName", $"{databaseName}-full-{DateTime.Now:yyyyMMdd-HHmmss}");

        await command.ExecuteNonQueryAsync();
    }
}