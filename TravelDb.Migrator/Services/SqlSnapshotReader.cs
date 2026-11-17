
using Microsoft.Data.SqlClient;
using TravelDb.Migrator.Models;

namespace TravelDb.Migrator.Services;

public class SqlSnapshotReader
{
    // Henter alle SQL-tabeller og deres data til et SqlSnapshot.
    public async Task<SqlSnapshot> ReadAsync(string connectionString)
    {
        var snapshot = new SqlSnapshot();

        // Opretter forbindelse til SQL Server.
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        // Finder alle brugertabeller i databasen.
        var tables = await GetTableNamesAsync(connection);

        // Gennemgår hver tabel og henter dens data.
        foreach (var table in tables)
        {
            // Beskytter tabel- og schemanavne i SQL-forespørgslen.
            var builder = new SqlCommandBuilder();
            var tableName = $"{builder.QuoteIdentifier(table.Schema)}." + $"{builder.QuoteIdentifier(table.Name)}";

            await using var command = new SqlCommand($"SELECT * FROM {tableName}", connection);
            await using var reader = await command.ExecuteReaderAsync();

            var rows = new List<Dictionary<string, object?>>();

            // Gennemgår alle rækker i tabellen.
            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);

                // Henter kolonnenavne og værdier fra den aktuelle række.
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var columnName = reader.GetName(i);
                    var value = await reader.IsDBNullAsync(i)
                        ? null : reader.GetValue(i);

                    row[columnName] = value;
                }
                rows.Add(row);
            }

            // Gemmer tabellens rækker i snapshot.
            var key = $"{table.Schema}.{table.Name}";
            snapshot.Tables[key] = rows;
        }
        return snapshot;
    }

    // Finder alle brugertabeller undtagen DbUp's migrationsjournal.
    private static async Task<List<(string Schema, string Name)>> GetTableNamesAsync(SqlConnection connection)
    {
        const string sql = """
            SELECT
                s.name AS SchemaName,
                t.name AS TableName
            FROM sys.tables AS t
            INNER JOIN sys.schemas AS s
                ON t.schema_id = s.schema_id
            WHERE t.is_ms_shipped = 0
              AND NOT (
                  s.name = 'dbo'
                  AND t.name = 'SchemaVersions'
              )
            ORDER BY
                s.name,
                t.name;
            """;

        var tables = new List<(string Schema, string Name)>();

        await using var command = new SqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync();

        // Gemmer schema og navn for hver fundet tabel.
        while (await reader.ReadAsync())
        {
            tables.Add((reader.GetString(0), reader.GetString(1)
            ));
        }
        return tables;
    }
}
