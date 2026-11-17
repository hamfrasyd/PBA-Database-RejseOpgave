using MongoDB.Bson;
using MongoDB.Driver;

namespace TravelDb.Setup.Services;

//Kontrollerer MongoDB-forbindelsen og opretter ravelDb-collections, hvis de ikke findes.
/// <summary>Checks MongoDB and creates the collections that are missing.</summary>
public class MongoDBService
{
    /// <summary>Checks the MongoDB connection and creates the collections that are missing.</summary>
    /// <param name="connectionString">The MongoDB connection string.</param>
    /// <param name="databaseName">The database to prepare.</param>
    public async Task InitializeAsync(string connectionString, string databaseName)
    {
        //Opret MongoDB-client og Vælg databasen TravelDb
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);

        //Kontroller forbindelsen
        await database.RunCommandAsync<BsonDocument>(new BsonDocument("ping", 1));

        //Collections der skal oprettes, hvis de ikke findes
        string[] collections =
        {
            "trips",
            "users",
            "cities",
            "activities",
            "groups",
            "achievements"
        };

        // Find eksisterende collections
        using var cursor = await database.ListCollectionNamesAsync();
        var existing = await cursor.ToListAsync();

        // Opret dem der mangler
        foreach (var name in collections)
        {
            if (!existing.Contains(name))
            {
                await database.CreateCollectionAsync(name);
            }
        }
    }
}
