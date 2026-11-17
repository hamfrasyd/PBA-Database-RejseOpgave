using MongoDB.Bson;
using TravelDb.Migrator.Models;
using System.Globalization;

namespace TravelDb.Migrator.Mapper.MongoMapper;

//Denne klasse indeholder metoder til at mappe SQL-data til MongoDB-dokumenter. den bruges af de andre mapper-klasser 
internal static class SqlMapping
{
    // Henter alle rækker fra en bestemt SQL-tabel.
    public static List<Dictionary<string, object?>> Rows(SqlSnapshot snapshot, string table)
    {
        return snapshot.GetRows("dbo." + table);
    }

    // Henter et ID fra en SQL-række og laver det om til en string.
    public static string Id(Dictionary<string, object?> row, string column = "Id")
    {
        if (!row.TryGetValue(column, out var value) || value == null)
        {
            throw new InvalidOperationException($"Manglende SQL-værdi: {column}");
        }
        return Convert.ToString(value, CultureInfo.InvariantCulture)!;
    }

    // Finder en bestemt række i en SQL-tabel ud fra dens ID.
    public static Dictionary<string, object?> Find(SqlSnapshot snapshot, string table, string id)
    {
        return Rows(snapshot, table).FirstOrDefault(r => Id(r) == id) 
            ?? throw new InvalidOperationException($"Mangler {table} med Id={id}");
    }

    // Konverterer en SQL-værdi til en værdi, som MongoDB kan gemme.
    public static BsonValue Value(Dictionary<string, object?> row, string column)
    {
        // Kontrollerer at kolonnen findes i SQL-rækken.
        if (!row.TryGetValue(column, out var value))
            throw new InvalidOperationException($"Kolonnen {column} findes ikke.");

        // SQL NULL bliver til MongoDB null.
        if (value == null || value is DBNull)
            return BsonNull.Value;

        // Konverterer særlige SQL-datatyper til BSON.
        return value switch
        {
            // Dato gemmes i UTC (forudsætter at den er UTC).
            DateTime date => new BsonDateTime(DateTime.SpecifyKind(date, DateTimeKind.Utc)),

            // Dato med tidszone omregnes til UTC.
            DateTimeOffset date => new BsonDateTime(date.UtcDateTime),

            // Decimal gemmes som Decimal128.
            decimal number => new BsonDecimal128(number),

            // Guid gemmes som tekst.
            Guid guid => new BsonString(guid.ToString()),

            // Byte-array gemmes som binære data.
            byte[] bytes => new BsonBinaryData(bytes),

            // Andre datatyper konverteres automatisk.
            _ => BsonValue.Create(value)
        };
    }

    // Henter en valgfri kolonne eller returnerer null, hvis den mangler.
    public static BsonValue Optional(Dictionary<string, object?> row, string column)
    {
        return row.ContainsKey(column)
            ? Value(row, column) : BsonNull.Value;
    }

    // Omdanner en samling MongoDB-dokumenter til et BSON-array.
    public static BsonArray Array(IEnumerable<BsonDocument> documents)
    {
        return new BsonArray(documents);
    }
}