using MongoDB.Bson;
using TravelDb.Migrator.Models;
using static TravelDb.Migrator.Mapper.MongoMapper.SqlMapping;

namespace TravelDb.Migrator.Mapper.MongoMapper;

public static class AchievementMapper
{
    // Mapper achievements til MongoDB-dokumenter.
    public static List<BsonDocument> Map(SqlSnapshot snapshot)
    {
        var documents = new List<BsonDocument>();

        foreach (var achievement in Rows(snapshot, "Achievement"))
        {
            documents.Add(new BsonDocument
            {
                ["_id"] = Id(achievement),
                ["name"] = Value(achievement, "Name"),
                ["description"] = Value(achievement, "Description")
            });
        }
        return documents;
    }
}

