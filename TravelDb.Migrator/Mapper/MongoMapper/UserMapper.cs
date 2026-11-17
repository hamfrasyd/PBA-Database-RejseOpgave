
using MongoDB.Bson;
using TravelDb.Migrator.Models;
using static TravelDb.Migrator.Mapper.MongoMapper.SqlMapping;

namespace TravelDb.Migrator.Mapper.MongoMapper;

public static class UserMapper
{
    // Opretter korte brugeroplysninger til andre dokumenter.
    public static BsonDocument MapReference(SqlSnapshot snapshot, string userId)
    {
        var user = Find(snapshot, "User", userId);

        return new BsonDocument
        {
            ["userId"] = userId,
            ["firstName"] = Value(user, "FirstName"),
            ["lastName"] = Value(user, "LastName")
        };
    }

    // Opretter alle brugere som dokumenter til MongoDB.
    public static List<BsonDocument> Map(SqlSnapshot snapshot)
    {
        var documents = new List<BsonDocument>();

        // Henter brugere og deres tilhørende oplysninger fra SQL.
        var users = Rows(snapshot, "User");
        var friendships = Rows(snapshot, "Friendship");
        var posts = Rows(snapshot, "Post");
        var earned = Rows(snapshot, "EarnedAchievement");
        var invites = Rows(snapshot, "Invite");

        // Gennemgår hver bruger fra SQL.
        foreach (var user in users)
        {
            var userId = Id(user);

            // Finder venskaber, hvor brugeren er en af de to parter.
            var friends = friendships
                .Where(f => Id(f, "UserA_Id") == userId || Id(f, "UserB_Id") == userId)
                .Select(f =>
                {
                    // Finder ID'et på den anden bruger i venskabet.
                    var friendId = Id(f, "UserA_Id") == userId
                        ? Id(f, "UserB_Id") : Id(f, "UserA_Id");

                    return new BsonDocument
                    {
                        ["_id"] = Id(f),
                        ["startDate"] = Value(f, "StartDate"),
                        ["friend"] = MapReference(snapshot, friendId)
                    };
                });

            // Finder alle opslag, som brugeren har oprettet.
            var userPosts = posts
                .Where(p => Id(p, "User_Id") == userId)
                .Select(p => new BsonDocument
                {
                    ["_id"] = Id(p),
                    ["content"] = Value(p, "Content")
                });

            // Finder brugerens achievements og hvornår de blev opnået.
            var earnedItems = earned
                .Where(e => Id(e, "User_Id") == userId)
                .Select(e =>
                {
                    var achievementId = Id(e, "Achievement_Id");
                    var achievement = Find(snapshot, "Achievement", achievementId);

                    return new BsonDocument
                    {
                        ["achievementId"] = achievementId,
                        ["name"] = Value(achievement, "Name"),
                        ["description"] = Value(achievement, "Description"),
                        ["earnedAt"] = Value(e, "EarnedAt")
                    };
                })
                .ToList();

            // Finder invitationer, som brugeren har sendt.
            var sentInvites = invites
                .Where(i => Id(i, "Sender_Id") == userId)
                .Select(i => MapInvite(snapshot, i, received: false));

            // Finder invitationer, som brugeren har modtaget.
            var receivedInvites = invites
                .Where(i => Id(i, "Receiver_Id") == userId)
                .Select(i => MapInvite(snapshot, i, received: true));

            // Finder brugerens by, hvis der er registreret en.
            BsonValue city = user["City_Id"] == null
                ? BsonNull.Value : CityMapper.MapEmbedded(snapshot, Id(user, "City_Id"));

            // Samler brugerens oplysninger i ét MongoDB-dokument.
            documents.Add(new BsonDocument
            {
                ["_id"] = userId,
                ["firstName"] = Value(user, "FirstName"),
                ["lastName"] = Value(user, "LastName"),
                ["birthday"] = Value(user, "Birthday"),
                ["isPrivate"] = Value(user, "IsPrivate"),
                ["createdAt"] = Value(user, "CreatedAt"),
                ["city"] = city,
                ["friendships"] = Array(friends),
                ["posts"] = Array(userPosts),

                // Liste med ID'erne på brugerens achievements.
                ["achievements"] = Array(earnedItems.Select(e => new BsonDocument
                {
                    ["achievementId"] = e["achievementId"].DeepClone()
                })),

                // Achievements med navn, beskrivelse og dato.
                ["earnedAchievements"] = Array(earnedItems.Select(e => (BsonDocument)e.DeepClone())),

                ["invitesSent"] = Array(sentInvites),
                ["invitesReceived"] = Array(receivedInvites)
            });
        }
        return documents;
    }

    // Opretter en invitation med status, rejse og brugeroplysninger.
    private static BsonDocument MapInvite(SqlSnapshot snapshot, Dictionary<string, object?> invite, bool received)
    {
        // Finder invitationens rejse og status fra SQL.
        var tripId = Id(invite, "Trip_Id");
        var trip = Find(snapshot, "Trip", tripId);
        var status = Find(snapshot, "InvitationStatus", Id(invite, "InvitationStatus_Id"));

        // Opretter korte oplysninger om rejsen.
        var tripDocument = new BsonDocument
        {
            ["tripId"] = tripId,
            ["startDate"] = Value(trip, "StartDate"),
            ["endDate"] = Value(trip, "EndDate")
        };

        // Opretter invitationens fælles oplysninger.
        var document = new BsonDocument
        {
            ["_id"] = Id(invite),
            ["status"] = Value(status, "StatusName"),
            ["sendDate"] = Value(invite, "SendDate"),
            ["invitationStatusId"] = Id(invite, "InvitationStatus_Id"),
            ["trip"] = tripDocument
        };

        if (received)
        {
            // Modtagne invitationer viser afsenderen.
            document["sender"] = MapReference(snapshot, Id(invite, "Sender_Id"));

            // Finder alle destinationer, der hører til rejsen.
            var destinations = Rows(snapshot, "TripStop")
                .Where(s => Id(s, "Trip_Id") == tripId)
                .Select(s => new BsonDocument
                {
                    ["city"] = CityMapper.MapEmbedded(snapshot, Id(s, "City_Id"))
                });

            tripDocument["destinations"] = Array(destinations);
        }
        else
        {
            // Sendte invitationer viser modtageren.
            document["recipient"] = MapReference(snapshot, Id(invite, "Receiver_Id"));
        }
        return document;
    }
}
