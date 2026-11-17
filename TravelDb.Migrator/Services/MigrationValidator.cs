
using MongoDB.Bson;
using MongoDB.Driver;
using TravelDb.Migrator.Models;
using System.Globalization;

namespace TravelDb.Migrator.Services;

/// <summary>Checks that the MongoDB data matches the SQL data.</summary>
public class MigrationValidator
{
    /// <summary>Checks document ids and trip participants.</summary>
    /// <param name="snapshot">The copied SQL data.</param>
    /// <param name="connectionString">The MongoDB connection string.</param>
    /// <returns>A task that finishes when the checks pass.</returns>
    /// <exception cref="InvalidOperationException">Thrown when ids or trip participants do not match.</exception>
    // Starter valideringen af MongoDB.
    public async Task ValidateMongoAsync(SqlSnapshot snapshot, string connectionString)
    {
        // Opretter forbindelse til MongoDB.
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase("TravelDb");

        // Kontrollerer dokumenternes ID'er.
        await ValidateIdsAsync(snapshot, database);

        // Kontrollerer deltagere på alle rejser.
        await ValidateParticipantsAsync(snapshot, database);
    }

    /// <summary>Checks that each MongoDB collection has the same ids as its SQL table.</summary>
    /// <param name="snapshot">The copied SQL data.</param>
    /// <param name="database">The MongoDB database to check.</param>
    /// <returns>A task that finishes when the ids match.</returns>
    /// <exception cref="InvalidOperationException">Thrown when a collection does not have the same ids as SQL.</exception>
    // Kontrollerer at MongoDB har de samme ID'er som SQL.
    private static async Task ValidateIdsAsync(SqlSnapshot snapshot, IMongoDatabase database)
    {
        // Sammenkobler MongoDB-collections med SQL-tabeller.
        var tables = new Dictionary<string, string>
        {
            ["trips"] = "dbo.Trip",
            ["users"] = "dbo.User",
            ["cities"] = "dbo.City",
            ["activities"] = "dbo.Activity",
            ["groups"] = "dbo.Group",
            ["achievements"] = "dbo.Achievement"
        };

        foreach (var (collectionName, tableName) in tables)
        {
            // Finder alle forventede ID'er fra SQL.
            var expectedIds = snapshot.GetRows(tableName)
                .Select(row => Convert.ToString(row["Id"], CultureInfo.InvariantCulture)!)
                .ToHashSet();

            // Henter ID'erne fra MongoDB.
            var collection = database.GetCollection<BsonDocument>(collectionName);

            var documents = await collection
                .Find(FilterDefinition<BsonDocument>.Empty)
                .ToListAsync();

            var actualIds = documents
                .Select(d => d["_id"].AsString)
                .ToHashSet();

            // Stopper hvis MongoDB og SQL ikke har de samme ID'er.
            if (!expectedIds.SetEquals(actualIds))
            {
                throw new InvalidOperationException($"ID-validering fejlede: {collectionName}");
            }
        }
    }

    /// <summary>Checks that each trip has the same participants in MongoDB as in SQL.</summary>
    /// <param name="snapshot">The copied SQL data.</param>
    /// <param name="database">The MongoDB database to check.</param>
    /// <returns>A task that finishes when the participants match.</returns>
    /// <exception cref="InvalidOperationException">Thrown when a trip's participants do not match.</exception>
    // Kontrollerer at hver rejse har de rigtige deltagere.
    private static async Task ValidateParticipantsAsync(SqlSnapshot snapshot, IMongoDatabase database)
    {
        // Henter alle rejsedeltagere fra SQL.
        var sqlParticipants = snapshot.GetRows("dbo.TripParticipant");

        // Henter alle rejser fra MongoDB.
        var trips = await database
            .GetCollection<BsonDocument>("trips")
            .Find(FilterDefinition<BsonDocument>.Empty)
            .ToListAsync();

        foreach (var trip in trips)
        {
            var tripId = trip["_id"].AsString;

            // Finder de deltagere, som hører til rejsen i SQL.
            var expected = sqlParticipants
                .Where(row => Convert.ToString(row["Trip_Id"], CultureInfo.InvariantCulture) == tripId)
                .Select(row => Convert.ToString(row["User_Id"], CultureInfo.InvariantCulture)!)
                .ToHashSet();

            // Finder de deltagere, som ligger i MongoDB.
            var actual = trip["participants"]
                .AsBsonArray
                .Select(p => p.AsBsonDocument["userId"].AsString)
                .ToHashSet();

            // Stopper hvis deltagerne ikke stemmer overens.
            if (!expected.SetEquals(actual))
            {
                throw new InvalidOperationException($"Deltagere stemmer ikke for Trip {tripId}");
            }
        }
    }
}
