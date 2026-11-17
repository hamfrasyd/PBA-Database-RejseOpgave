using MongoDB.Bson;
using TravelDb.Migrator.Models;
using static TravelDb.Migrator.Mapper.MongoMapper.SqlMapping;

namespace TravelDb.Migrator.Mapper.MongoMapper;

public static class ActivityMapper
{
    //MapEmbedded Bruges når en aktivitet ligger inde i en trip.
    public static BsonDocument MapEmbedded(SqlSnapshot snapshot, string activityId)
    {
        var activity = Find(snapshot, "Activity", activityId);

        return new BsonDocument
        {
            ["activityId"] = activityId,
            ["name"] = Value(activity, "Name"),
            ["description"] = Value(activity, "Description"),
            ["location"] = new BsonDocument
            {
                ["city"] = CityMapper.MapEmbedded(snapshot, Id(activity, "City_Id"))
            }
        };
    }

    // Aktiviteter til deres egen collection
    public static List<BsonDocument> Map(SqlSnapshot snapshot)
    {
        var documents = new List<BsonDocument>();

        foreach (var activity in Rows(snapshot, "Activity"))
        {
            var embedded = MapEmbedded(snapshot, Id(activity));

            embedded["_id"] = embedded["activityId"];
            embedded.Remove("activityId");

            documents.Add(embedded);
        }

        return documents;
    }
}
