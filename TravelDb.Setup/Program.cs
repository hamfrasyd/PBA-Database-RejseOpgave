//bruges til at finde SQL-scripts, der er injiceret i programmet
using TravelDb.Setup.Services;

const string databaseName = "TravelDb";
const string password = "TravelDB!2026Local";

var masterConnectionString =
    $"Server=localhost,1433;" +
    $"Database=master;" +
    $"User Id=sa;" +
    $"Password={password};" +
    $"TrustServerCertificate=True;" +
    $"Encrypt=False;";

var travelDbConnectionString =
    $"Server=localhost,1433;" +
    $"Database={databaseName};" +
    $"User Id=sa;" +
    $"Password={password};" +
    $"TrustServerCertificate=True;" +
    $"Encrypt=False;";

DockerService dockerService = new DockerService();
DatabaseService databaseService = new DatabaseService();
MigrationService migrationService = new MigrationService();


try
{
    Console.WriteLine("TravelDb setup" + "\n");

    //1.Start Dcker - container
    Console.WriteLine("Starter SQL Server Docker-container...");
    await dockerService.StartAsync();
    Console.WriteLine("SQL Server Docker-container startet");

    // 2. Vent på SQL Server
    Console.WriteLine("Venter på SQL Server...");
    await databaseService.WaitUntilReadyAsync(masterConnectionString);
    Console.WriteLine("SQL Server er klar.");

    // 3. Opret databasen hvis den ikke findes 
    Console.WriteLine($"Kontrollerer database '{databaseName}'");
    await databaseService.CreateDatabaseIfMissingAsync(masterConnectionString, databaseName);
    Console.WriteLine($"Database '{databaseName}' er klar.");

    // 4. Kør DbUp migrations
    Console.WriteLine("Kører database-scripts...");
    migrationService.Run(travelDbConnectionString); //med docker : travelDbConnectionString
    Console.WriteLine("Databaseopdateringen blev gennemført!");
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Database setup gennemført." + "\n" + "Server: localhost,1433" + "\n" + $"Database: {databaseName}" + "\n" + "User: sa");
    Console.ResetColor();

    return 0;
}catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Der opstod en fejl:" + "\n" + ex.Message);
    Console.ResetColor();

    return -1;
}