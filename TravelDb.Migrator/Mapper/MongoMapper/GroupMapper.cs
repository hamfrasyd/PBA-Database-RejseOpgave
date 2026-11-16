using MongoDB.Bson;
using TravelDb.Migrator.Models;
using static TravelDb.Migrator.Mapper.MongoMapper.SqlMapping;

namespace TravelDb.Migrator.Mapper.MongoMapper;

/// <summary>Turns SQL groups into MongoDB documents.</summary>
public static class GroupMapper
{
    /// <summary>Creates one document for each group, including its members.</summary>
    /// <param name="snapshot">The copied SQL data.</param>
    /// <returns>The group documents.</returns>
    // Opretter alle grupper som dokumenter til MongoDB.
    public static List<BsonDocument> Map(SqlSnapshot snapshot)
    {
        var documents = new List<BsonDocument>();

        // Henter grupper og gruppemedlemskaber fra SQL.
        var groups = Rows(snapshot, "Group");
        var memberships = Rows(snapshot, "GroupMembership");

        // Gennemgår hver gruppe fra SQL.
        foreach (var group in groups)
        {
            string groupId = Id(group);

            // Finder de brugere, der er medlem af gruppen.
            var members = memberships
                .Where(m => Id(m, "Group_Id") == groupId)
                .Select(m => UserMapper.MapReference(snapshot, Id(m, "User_Id")));

            // Opretter et MongoDB-dokument med gruppens oplysninger og medlemmer.
            var document = new BsonDocument
            {
                ["_id"] = groupId,
                ["name"] = Value(group, "Name"),
                ["description"] = Value(group, "Description"),
                ["users"] = Array(members)
            };
            documents.Add(document);
        }
        return documents;
    }
}
