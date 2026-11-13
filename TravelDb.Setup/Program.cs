//bruges til at finde SQL-scripts, der er injiceret i programmet
using TravelDb.Setup.Services;

const string databaseName = "TravelDb";
const string password = "TravelDB!2026Local";
const string MongoPassword = "TravelDB!2026Local";
const string Neo4jPassword = "TravelDB!2026Local";

//sqlServer
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

//MongoDB
var mongoConnectionString =
    $"mongodb://admin:{Uri.EscapeDataString(MongoPassword)}" +
    "@127.0.0.1:27018/?authSource=admin&directConnection=true";

//Neo4j
var neo4jUri = "bolt://localhost:7687";

DockerService dockerService = new DockerService();
DatabaseService databaseService = new DatabaseService();
MigrationService migrationService = new MigrationService();

MongoDBService mongoService = new();
Neo4jDBService neo4jService = new();

try
{
    Console.WriteLine("TravelDb setup" + "\n");

    // 1. Start Dcker - container
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

    // 5. Opret backup af databasen hvis programmet startes med argumentet "backup".
    Console.WriteLine("Starter backup af databasen...");
    if (args.Contains("backup"))
    {
        const string containerName = "travel-mssql";
        const string backupFolder = "/var/opt/mssql/backup";

        string fileName = $"{databaseName}-{DateTime.Now:yyyyMMdd-HHmmss}.bak";
        string containerPath = $"{backupFolder}/{fileName}";

        Console.WriteLine("Opretter backup...");
        await dockerService.CreateFolderIfMissingAsync(containerName, backupFolder);
        await databaseService.BackupDatabaseAsync(masterConnectionString, databaseName, containerPath);
        Console.WriteLine($"Backup gemt i containeren: {containerPath}");
    }
    else
    {
        Console.WriteLine("Manglede argumentet 'backup', backup blev ikke oprettet.");
    }

    // 6. Opsætning færdig
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Database setup gennemført." + "\n" + "Server: localhost,1433" + "\n" + $"Database: {databaseName}" + "\n" + "User: sa");
    Console.ResetColor();

    // 5. Klargør MongoDB
    Console.WriteLine("Klargør MongoDB...");
    await mongoService.InitializeAsync(mongoConnectionString, databaseName);
    Console.WriteLine("MongoDB er klar.");

    // 6. Klargør Neo4j
    Console.WriteLine("Klargør Neo4j...");
    await neo4jService.InitializeAsync(neo4jUri, Neo4jPassword);
    Console.WriteLine("Neo4j er klar.");

    // Alle databaser er klar
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Alle tre databaser er klar!");
    Console.ResetColor();

return 0;
}catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Der opstod en fejl:" + "\n" + ex.Message);
    Console.ResetColor();

    return -1;
}