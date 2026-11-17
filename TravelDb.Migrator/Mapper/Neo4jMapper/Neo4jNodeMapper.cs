using Neo4j.Driver;
using System.Globalization;
using System.Text.RegularExpressions;
using TravelDb.Migrator.Models;

/// <summary>Creates one Neo4j node for each SQL row that has an id.</summary>
public static class Neo4jNodeMapper
{
    /// <summary>Creates nodes from the SQL tables.</summary>
    /// <param name="session">The open Neo4j session.</param>
    /// <param name="snapshot">The copied SQL data.</param>
    /// <returns>A task that finishes when the nodes are created.</returns>
    /// <exception cref="InvalidOperationException">Thrown when a table name cannot be used as a Neo4j label.</exception>
    public static async Task MigrateAsync(IAsyncSession session, SqlSnapshot snapshot)
    {
        foreach (var (tableName, rows) in snapshot.Tables)
        {
            if (rows.Count == 0)
                continue;

            // Relationstabeller uden Id springes over.
            if (!rows[0].ContainsKey("Id"))
                continue;

            string label = tableName.Split('.')[^1];

            if (!Regex.IsMatch(label, @"^[A-Za-z_][A-Za-z0-9_]*$"))
            {
                throw new InvalidOperationException($"Ugyldigt Neo4j-label: {label}");
            }

            // Almindelig string i stedet for interpoleret
            // raw string, så Cypher-klammerne virker.
            string cypher =
                "UNWIND $rows AS row " +
                $"MERGE (n:`{label}` {{id: row.id}}) " +
                "SET n += row.properties";

            foreach (var batch in rows.Chunk(500))
            {
                var data = batch.Select(row =>
                {
                    var properties =
                        new Dictionary<string, object>();

                    foreach (var (column, value) in row)
                    {
                        if (value != null)
                        {
                            properties[column] =
                                ConvertValue(value);
                        }
                    }

                    long id = Convert.ToInt64(
                        row["Id"],
                        CultureInfo.InvariantCulture);

                    properties["id"] = id;

                    return new
                    {
                        id,
                        properties
                    };
                }).ToList();

                var cursor = await session.RunAsync(
                    cypher,
                    new { rows = data });

                await cursor.ConsumeAsync();
            }
        }
    }

    /// <summary>Turns a SQL value into a value Neo4j can store.</summary>
    /// <param name="value">The SQL value.</param>
    /// <returns>A text, number, or other simple value Neo4j accepts.</returns>
    private static object ConvertValue(object value)
    {
        return value switch
        {
            DateTime date => date.ToString("O"),
            DateTimeOffset date => date.ToString("O"),

            decimal number => number.ToString(CultureInfo.InvariantCulture),

            Guid guid => guid.ToString(),

            byte[] bytes => Convert.ToBase64String(bytes),

            TimeSpan time => time.ToString(),

            _ => value
        };
    }
}