using Neo4j.Driver;
using System.Globalization;
using TravelDb.Migrator.Models;

public static class Neo4jRelationshipMapper
{
    public static async Task MigrateAsync(IAsyncSession session,SqlSnapshot snapshot)
    {
        var mappings = new[]
        {
            new Relation(
                "TripParticipant",
                "User", "User_Id",
                "Trip", "Trip_Id",
                "Participates_In"),

            new Relation(
                "GroupMembership",
                "User", "User_Id",
                "Group", "Group_Id",
                "Member_Of"),

            new Relation(
                "GroupTrip",
                "Group", "Group_Id",
                "Trip", "Trip_Id",
                "Organizes"),

            new Relation(
                "TripStop",
                "Trip", "Trip_Id",
                "City", "City_Id",
                "Has_Stops"),

            new Relation(
                "TripActivity",
                "Trip", "Trip_Id",
                "Activity", "Activity_Id",
                "Includes_Activity"),

            new Relation(
                "Friendship",
                "User", "UserA_Id",
                "User", "UserB_Id",
                "Friend_With"),

            new Relation(
                "Country",
                "Country", "Id",
                "Continent", "Continent_Id",
                "In_Continent"),

            new Relation(
                "City",
                "City", "Id",
                "Country", "Country_Id",
                "In_Country"),

            new Relation(
                "Activity",
                "Activity", "Id",
                "City", "City_Id",
                "Takes_Place_In"),

            new Relation(
                "User",
                "User", "Id",
                "City", "City_Id",
                "Lives_In"),

            new Relation(
                "Post",
                "Post", "Id",
                "User", "User_Id",
                "Wrote"),

            new Relation(
                "Review",
                "User", "User_Id",
                "Review", "Id",
                "Wrote"),

            new Relation(
                "Review",
                "Review", "Id",
                "Trip", "Trip_Id",
                "Reviews"),

            new Relation(
                "EarnedAchievement",
                "User", "User_Id",
                "Achievement", "Achievement_Id",
                "Earned"),

            new Relation(
                "Invite",
                "User", "Sender_Id",
                "Invite", "Id",
                "Sent"),

            new Relation(
                "Invite",
                "Invite", "Id",
                "User", "Receiver_Id",
                "Received_By"),

            new Relation(
                "Invite",
                "Invite", "Id",
                "Trip", "Trip_Id",
                "For_Trip")
        };

        foreach (var mapping in mappings)
        {
            string tableName = "dbo." + mapping.Table;

            if (!snapshot.Tables.TryGetValue(tableName, out var rows))
            {
                throw new InvalidOperationException($"SQL-tabellen mangler: {tableName}");
            }

            // Alle navne kommer fra vores faste mappings.
            string cypher =
                "UNWIND $rows AS row " +
                $"MATCH (a:`{mapping.FromLabel}` " +
                "{id: row.fromId}) " +
                $"MATCH (b:`{mapping.ToLabel}` " +
                "{id: row.toId}) " +
                $"MERGE (a)-[r:`{mapping.Type}`]->(b) " +
                "RETURN count(r) AS total";

            foreach (var batch in rows.Chunk(500))
            {
                var data = batch
                    .Where(row =>
                        row.TryGetValue(mapping.FromColumn, out var from)
                        && from != null
                        && row.TryGetValue(mapping.ToColumn, out var to)
                        && to != null).Select(row => new
                    {
                        fromId = Convert.ToInt64(row[mapping.FromColumn], CultureInfo.InvariantCulture),

                        toId = Convert.ToInt64(row[mapping.ToColumn], CultureInfo.InvariantCulture)
                    })
                    .ToList();

                if (data.Count == 0)
                    continue;

                var cursor = await session.RunAsync(cypher, new { rows = data });
                var record = await cursor.SingleAsync();

                long createdOrMatched = record["total"].As<long>();

                if (createdOrMatched != data.Count)
                {
                    throw new InvalidOperationException(
                        $"Relation {mapping.Type}: " +
                        $"Forventede {data.Count}, " +
                        $"fandt {createdOrMatched}.");
                }
                await cursor.ConsumeAsync();
            }
        }
    }

    private record Relation(
        string Table,
        string FromLabel,
        string FromColumn,
        string ToLabel,
        string ToColumn,
        string Type);
}