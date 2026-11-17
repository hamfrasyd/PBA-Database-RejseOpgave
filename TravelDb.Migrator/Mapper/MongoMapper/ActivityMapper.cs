using MongoDB.Bson;
using TravelDb.Migrator.Models;
using static TravelDb.Migrator.Mapper.MongoMapper.SqlMapping;

namespace TravelDb.Migrator.Mapper.MongoMapper;

/// <summary>Turns SQL activities into MongoDB documents.</summary>
public static class ActivityMapper
{
    /// <summary>Creates a short activity document that can sit inside another document.</summary>
    /// <param name="snapshot">The copied SQL data.</param>
    /// <param name="activityId">The activity id.</param>
    /// <returns>The activity with its name, description, and city.</returns>
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

    /// <summary>Creates one document for each activity.</summary>
    /// <param name="snapshot">The copied SQL data.</param>
    /// <returns>The activity documents.</returns>
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
