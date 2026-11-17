using MongoDB.Bson;
using TravelDb.Migrator.Models;

namespace TravelDb.Migrator.Mapper.MongoMapper;

/// <summary>Builds all MongoDB collections from the copied SQL data.</summary>
public class MongoDocumentMapper
{
    /// <summary>Turns the SQL data into documents for each MongoDB collection.</summary>
    /// <param name="snapshot">The copied SQL data.</param>
    /// <returns>The documents grouped by collection name.</returns>
    // Samler dokumenter fra alle seks mappers til MongoDB.
    public Dictionary<string, List<BsonDocument>> Map(SqlSnapshot snapshot)
    {
        return new Dictionary<string, List<BsonDocument>>
        {
            ["trips"] = TripMapper.Map(snapshot),
            ["users"] = UserMapper.Map(snapshot),
            ["cities"] = CityMapper.Map(snapshot),
            ["activities"] = ActivityMapper.Map(snapshot),
            ["groups"] = GroupMapper.Map(snapshot),
            ["achievements"] = AchievementMapper.Map(snapshot)
        };
    }
}
