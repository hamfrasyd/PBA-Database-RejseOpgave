using MongoDB.Bson;
using TravelDb.Migrator.Models;
using System.Globalization;

namespace TravelDb.Migrator.Mapper.MongoMapper;

/// <summary>Helper methods that read SQL rows and turn values into MongoDB values.</summary>
//Denne klasse indeholder metoder til at mappe SQL-data til MongoDB-dokumenter. den bruges af de andre mapper-klasser 
internal static class SqlMapping
{
    /// <summary>Gets the rows from one SQL table.</summary>
    /// <param name="snapshot">The copied SQL data.</param>
    /// <param name="table">The table name without the schema, for example Trip.</param>
    /// <returns>The rows in that table.</returns>
    // Henter alle rækker fra en bestemt SQL-tabel.
    public static List<Dictionary<string, object?>> Rows(SqlSnapshot snapshot, string table)
    {
        return snapshot.GetRows("dbo." + table);
    }

    /// <summary>Reads an id from a SQL row and returns it as text.</summary>
    /// <param name="row">The SQL row.</param>
    /// <param name="column">The column that holds the id. Defaults to Id.</param>
    /// <returns>The id as text.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the column is missing or empty.</exception>
    // Henter et ID fra en SQL-række og laver det om til en string.
    public static string Id(Dictionary<string, object?> row, string column = "Id")
    {
        if (!row.TryGetValue(column, out var value) || value == null)
        {
            throw new InvalidOperationException($"Manglende SQL-værdi: {column}");
        }
        return Convert.ToString(value, CultureInfo.InvariantCulture)!;
    }

    /// <summary>Finds one row in a table by its id.</summary>
    /// <param name="snapshot">The copied SQL data.</param>
    /// <param name="table">The table name without the schema.</param>
    /// <param name="id">The id to find.</param>
    /// <returns>The matching row.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no row has that id.</exception>
    // Finder en bestemt række i en SQL-tabel ud fra dens ID.
    public static Dictionary<string, object?> Find(SqlSnapshot snapshot, string table, string id)
    {
        return Rows(snapshot, table).FirstOrDefault(r => Id(r) == id) 
            ?? throw new InvalidOperationException($"Mangler {table} med Id={id}");
    }

    /// <summary>Turns a SQL value into a value MongoDB can store.</summary>
    /// <param name="row">The SQL row.</param>
    /// <param name="column">The column to read.</param>
    /// <returns>The MongoDB value. SQL null becomes MongoDB null.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the column does not exist.</exception>
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

    /// <summary>Reads a column, or returns null when the column is missing.</summary>
    /// <param name="row">The SQL row.</param>
    /// <param name="column">The column to read.</param>
    /// <returns>The MongoDB value, or null when the column is missing.</returns>
    // Henter en valgfri kolonne eller returnerer null, hvis den mangler.
    public static BsonValue Optional(Dictionary<string, object?> row, string column)
    {
        return row.ContainsKey(column)
            ? Value(row, column) : BsonNull.Value;
    }

    /// <summary>Puts MongoDB documents into one array.</summary>
    /// <param name="documents">The documents to include.</param>
    /// <returns>The documents as a MongoDB array.</returns>
    // Omdanner en samling MongoDB-dokumenter til et BSON-array.
    public static BsonArray Array(IEnumerable<BsonDocument> documents)
    {
        return new BsonArray(documents);
    }
}