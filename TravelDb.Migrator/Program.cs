
using TravelDb.Migrator.Mapper.MongoMapper;
using TravelDb.Migrator.Services;

const string databaseName = "TravelDb";
const string password = "TravelDB!2026Local";

// SQL Server
var sqlConnectionString =
    $"Server=localhost,1433;" +
    $"Database={databaseName};" +
    $"User Id=sa;" +
    $"Password={password};" +
    $"TrustServerCertificate=True;" +
    $"Encrypt=False;";

// MongoDB
var mongoConnectionString =
    $"mongodb://admin:{Uri.EscapeDataString(password)}" +
    "@127.0.0.1:27018/?authSource=admin&directConnection=true";

// Neo4j
var neo4jUri = "bolt://localhost:7687";

try
{
    Console.WriteLine("TravelDb Migrator\n");

    //Hent alle data fra SQL Server.
    Console.WriteLine("Henter data fra SQL Server...");
    var reader = new SqlSnapshotReader();
    var snapshot = await reader.ReadAsync(sqlConnectionString);

    Console.WriteLine($"SQL: {snapshot.TotalRows} rækker hentet.");

    //Konverter SQL-data til MongoDB-dokumenter.
    Console.WriteLine("Konverterer data til MongoDB...");

    var mongoMapper = new MongoDocumentMapper();
    var collections = mongoMapper.Map(snapshot);

    //Gem dokumenterne i MongoDB.
    Console.WriteLine("Migrerer til MongoDB...");

    var mongoMigrator = new MongoMigrationService(mongoConnectionString);
    await mongoMigrator.MigrateAsync(collections);

    Console.WriteLine("MongoDB-migrering gennemført.");

    //Kontroller MongoDB-migreringen.
    Console.WriteLine("Validerer MongoDB...");

    var validator = new MigrationValidator();
    await validator.ValidateMongoAsync(snapshot, mongoConnectionString);

    Console.WriteLine("MongoDB-validering gennemført.");

    //Migrer SQL-data til Neo4j.
    Console.WriteLine("Migrerer til Neo4j...");

    var neo4jMigrator = new Neo4jMigrationService(neo4jUri, password);
    await neo4jMigrator.MigrateAsync(snapshot);

    Console.WriteLine("Neo4j-migrering gennemført.");

    //Alle migreringer er gennemført.
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("\nDatamigreringen er gennemført!");
    Console.ResetColor();

    return 0;
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Migreringen fejlede:\n{ex}");
    Console.ResetColor();

    return -1;
}
