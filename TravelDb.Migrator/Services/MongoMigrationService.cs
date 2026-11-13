
using MongoDB.Bson;
using MongoDB.Driver;

namespace TravelDb.Migrator.Services;

public class MongoMigrationService
{
    private readonly IMongoDatabase _database;
    private const int BatchSize = 500;

    public MongoMigrationService(string connectionString)
    {
        // Opretter forbindelse til MongoDB-databasen.
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase("TravelDb");
    }

    // Migrerer alle collections til MongoDB.
    public async Task MigrateAsync(Dictionary<string, List<BsonDocument>> collections)
    {
        foreach (var (collectionName, documents) in collections)
        {
            // Finder den collection, som dokumenterne skal gemmes i.
            var collection = _database.GetCollection<BsonDocument>(collectionName);

            // Kontrollerer at dokumenterne har gyldige og unikke ID'er.
            ValidateIds(documents, collectionName);

            // Indsætter nye dokumenter eller opdaterer eksisterende.
            await SaveDocumentsAsync(collection, documents);

            // Kontrollerer at alle forventede dokumenter findes.
            await ValidateMigrationAsync(collection, documents, collectionName);
        }
    }

    // Kontrollerer at alle dokumenter har et unikt _id.
    private static void ValidateIds(List<BsonDocument> documents, string collectionName)
    {
        if (documents.Any(d => !d.Contains("_id")))
        {
            throw new InvalidOperationException($"Et dokument i {collectionName} mangler _id.");
        }

        var ids = documents.Select(d => d["_id"]).ToList();

        if (ids.Distinct().Count() != ids.Count)
        {
            throw new InvalidOperationException($"Dublerede ID'er i {collectionName}.");
        }
    }

    // Gemmer dokumenterne i batches på højst 500 ad gangen.
    private static async Task SaveDocumentsAsync(IMongoCollection<BsonDocument> collection, List<BsonDocument> documents)
    {
        foreach (var batch in documents.Chunk(BatchSize))
        {
            var operations = new List<WriteModel<BsonDocument>>();

            foreach (var document in batch)
            {
                // Finder et eksisterende dokument med samme _id.
                var filter = Builders<BsonDocument>.Filter.Eq("_id", document["_id"]);

                // Opdaterer dokumentet eller opretter det, hvis det mangler.
                operations.Add(new ReplaceOneModel<BsonDocument>(filter, document)
                {
                    IsUpsert = true
                });
            }
            await collection.BulkWriteAsync(operations);
        }
    }

    // Kontrollerer at alle dokumenter findes efter migreringen.
    private static async Task ValidateMigrationAsync(
        IMongoCollection<BsonDocument> collection,
        List<BsonDocument> documents,
        string collectionName)
    {
        var ids = documents.Select(d => d["_id"]).ToList();
        long migratedCount = 0;

        // Tæller dokumenterne i mindre grupper.
        foreach (var batch in ids.Chunk(BatchSize))
        {
            var filter = Builders<BsonDocument>.Filter.In("_id", batch);

            migratedCount += await collection.CountDocumentsAsync(filter);
        }

        // Stopper migreringen, hvis der mangler dokumenter.
        if (migratedCount != documents.Count)
        {
            throw new InvalidOperationException(
                $"Validering fejlede for {collectionName}. " +
                $"Forventede {documents.Count}, fandt {migratedCount}.");
        }
    }
}
