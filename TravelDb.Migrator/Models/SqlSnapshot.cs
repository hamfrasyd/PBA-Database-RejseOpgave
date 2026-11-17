
namespace TravelDb.Migrator.Models;

/// <summary>A copy of the SQL tables and their rows.</summary>
public class SqlSnapshot
{
    /// <summary>Gets the tables, using the schema and table name as the key.</summary>
    // Gemmer SQL-tabellerne med deres rækker og kolonneværdier. Tabellenavnet bruges som nøgle.
    public Dictionary<string, List<Dictionary<string, object?>>> Tables
    { get; } = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>Gets the total number of rows in all tables.</summary>
    // Beregner det samlede antal rækker fra alle tabeller.
    public int TotalRows => Tables.Values.Sum(rows => rows.Count);

    /// <summary>Gets the rows from one table.</summary>
    /// <param name="tableName">The schema and table name, for example dbo.Trip.</param>
    /// <returns>The rows in that table.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the table is not in the snapshot.</exception>
    // Henter alle rækker fra en bestemt SQL-tabel.
    public List<Dictionary<string, object?>> GetRows(string tableName)
    {
        return Tables[tableName];
    }
}
