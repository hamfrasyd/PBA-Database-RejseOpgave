
using MongoDB.Bson;
using MongoDB.Driver;
using TravelDb.Migrator.Models;
using System.Globalization;

namespace TravelDb.Migrator.Services;

public class MigrationValidator
{
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
