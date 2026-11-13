
using MongoDB.Bson;
using TravelDb.Migrator.Models;
using static TravelDb.Migrator.Mapper.MongoMapper.SqlMapping;

namespace TravelDb.Migrator.Mapper.MongoMapper;

public static class CityMapper
{
    // Opretter byoplysninger, der kan bruges inde i andre dokumenter.
    public static BsonDocument MapEmbedded(SqlSnapshot snapshot, string cityId)
    {
        // Finder byen og dens tilhørende land og kontinent fra SQL.
        var city = Find(snapshot, "City", cityId);
        var country = Find(snapshot, "Country", Id(city, "Country_Id"));
        var continent = Find(snapshot, "Continent", Id(country, "Continent_Id"));

        // Bygger et dokument med by, land og kontinent.
        return new BsonDocument
        {
            ["cityId"] = cityId,
            ["name"] = Value(city, "Name"),
            ["country"] = new BsonDocument
            {
                ["countryId"] = Id(country),
                ["name"] = Value(country, "Name"),
                ["continent"] = new BsonDocument
                {
                    ["continentId"] = Id(continent),
                    ["name"] = Value(continent, "Name")
                }
            }
        };
    }

    // Opretter alle byer til deres egen cities-collection.
    public static List<BsonDocument> Map(SqlSnapshot snapshot)
    {
        var documents = new List<BsonDocument>();
        var activities = Rows(snapshot, "Activity");

        // Gennemgår alle byer fra SQL.
        foreach (var city in Rows(snapshot, "City"))
        {
            var cityId = Id(city);

            // Genbruger MapEmbedded til at hente by, land og kontinent.
            var document = MapEmbedded(snapshot, cityId);

            // Ændrer ID-felterne til MongoDB-strukturen for cities.
            document["_id"] = document["cityId"];
            document.Remove("cityId");

            var country = document["country"].AsBsonDocument;
            country["_id"] = country["countryId"];
            country.Remove("countryId");

            var continent = country["continent"].AsBsonDocument;
            continent["_id"] = continent["continentId"];
            continent.Remove("continentId");

            // Finder de aktiviteter fra SQL, som hører til byen.
            var cityActivities = activities
                .Where(a => Id(a, "City_Id") == cityId)
                .Select(a => new BsonDocument
                {
                    ["activityId"] = Id(a)
                });

            // Tilføjer aktiviteternes ID'er som et array.
            document["activities"] = Array(cityActivities);

            documents.Add(document);
        }

        return documents;
    }
}
