
using MongoDB.Bson;
using MongoDB.Driver;

namespace TravelDb.Migrator.Services;

/// <summary>Saves the mapped documents in MongoDB.</summary>
public class MongoMigrationService
{
    private readonly IMongoDatabase _database;
    private const int BatchSize = 500;

    /// <summary>Connects to the TravelDb database.</summary>
    /// <param name="connectionString">The MongoDB connection string.</param>
    public MongoMigrationService(string connectionString)
    {
        // Opretter forbindelse til MongoDB-databasen.
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase("TravelDb");
    }

    /// <summary>Saves every collection in MongoDB.</summary>
    /// <param name="collections">The documents grouped by collection name.</param>
    /// <returns>A task that finishes when every collection has been saved and checked.</returns>
    /// <exception cref="InvalidOperationException">Thrown when a document has no id, ids are repeated, or a document is missing after the save.</exception>
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

    /// <summary>Checks that every document has its own id.</summary>
    /// <param name="documents">The documents to check.</param>
    /// <param name="collectionName">The collection name, used in the error message.</param>
    /// <exception cref="InvalidOperationException">Thrown when an id is missing or used more than once.</exception>
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

    /// <summary>Saves the documents in groups of 500.</summary>
    /// <param name="collection">The MongoDB collection to write to.</param>
    /// <param name="documents">The documents to save.</param>
    /// <returns>A task that finishes when the documents are saved.</returns>
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

    /// <summary>Checks that every saved document can be found again.</summary>
    /// <param name="collection">The MongoDB collection to check.</param>
    /// <param name="documents">The documents that should be in the collection.</param>
    /// <param name="collectionName">The collection name, used in the error message.</param>
    /// <returns>A task that finishes when the check is done.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the number of saved documents does not match.</exception>
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
