using MongoDB.Bson;
using TravelDb.Migrator.Models;
using static TravelDb.Migrator.Mapper.MongoMapper.SqlMapping;

namespace TravelDb.Migrator.Mapper.MongoMapper;

public static class TripMapper
{
    // Opretter alle rejser som dokumenter til MongoDB.
    public static List<BsonDocument> Map(SqlSnapshot snapshot)
    {
        var documents = new List<BsonDocument>();

        // Henter rejser og tilhørende oplysninger fra SQL.
        var trips = Rows(snapshot, "Trip");
        var stops = Rows(snapshot, "TripStop");
        var participants = Rows(snapshot, "TripParticipant");
        var groupTrips = Rows(snapshot, "GroupTrip");
        var tripActivities = Rows(snapshot, "TripActivity");
        var reviews = Rows(snapshot, "Review");

        // Gennemgår hver rejse fra SQL.
        foreach (var trip in trips)
        {
            string tripId = Id(trip);

            // Finder rejsens destinationer og tilføjer byoplysninger.
            var destinations = stops
                .Where(s => Id(s, "Trip_Id") == tripId)
                .Select(s => new BsonDocument
                {
                    ["city"] = CityMapper.MapEmbedded(snapshot, Id(s, "City_Id"))
                });

            // Finder de brugere, der deltager i rejsen.
            var users = participants
                .Where(p => Id(p, "Trip_Id") == tripId)
                .Select(p => new BsonDocument
                {
                    ["userId"] = Id(p, "User_Id")
                });

            // Finder de grupper, der er tilknyttet rejsen.
            var groups = groupTrips
                .Where(g => Id(g, "Trip_Id") == tripId)
                .Select(g => new BsonDocument
                {
                    ["groupId"] = Id(g, "Group_Id")
                });

            // Finder rejsens aktiviteter og opretter indlejrede dokumenter.
            var activities = tripActivities
                .Where(a => Id(a, "Trip_Id") == tripId)
                .Select(a => ActivityMapper.MapEmbedded(snapshot, Id(a, "Activity_Id")));

            // Finder rejsens anmeldelser med oplysninger om brugeren.
            var tripReviews = reviews
                .Where(r => Id(r, "Trip_Id") == tripId)
                .Select(r => new BsonDocument
                {
                    ["reviewId"] = Id(r),
                    ["rating"] = Value(r, "Stars"),
                    ["content"] = Value(r, "Content"),
                    ["user"] = UserMapper.MapReference(snapshot, Id(r, "User_Id"))
                });

            // Samler alle oplysninger i ét MongoDB-dokument.
            var document = new BsonDocument
            {
                ["_id"] = tripId,
                ["startDate"] = Value(trip, "StartDate"),
                ["endDate"] = Value(trip, "EndDate"),
                ["isCompleted"] = Value(trip, "IsCompleted"),
                ["experienceNotes"] = Value(trip, "ExperienceNotes"),
                ["planNotes"] = Value(trip, "PlanNotes"),
                ["destinations"] = Array(destinations),
                ["participants"] = Array(users),
                ["groupParticipants"] = Array(groups),
                ["reviews"] = Array(tripReviews),
                ["activities"] = Array(activities)
            };
            documents.Add(document);
        }
        return documents;
    }
}
