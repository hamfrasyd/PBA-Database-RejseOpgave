using MongoDB.Bson;
using TravelDb.Migrator.Models;

namespace TravelDb.Migrator.Mapper.MongoMapper;

public class MongoDocumentMapper
{
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
