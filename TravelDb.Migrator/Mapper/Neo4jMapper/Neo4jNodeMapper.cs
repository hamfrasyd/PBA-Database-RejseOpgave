using Neo4j.Driver;
using System.Globalization;
using System.Text.RegularExpressions;
using TravelDb.Migrator.Models;

public static class Neo4jNodeMapper
{
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