
namespace TravelDb.Migrator.Models;

public class SqlSnapshot
{
    // Gemmer SQL-tabellerne med deres rækker og kolonneværdier. Tabellenavnet bruges som nøgle.
    public Dictionary<string, List<Dictionary<string, object?>>> Tables
    { get; } = new(StringComparer.OrdinalIgnoreCase);

    // Beregner det samlede antal rækker fra alle tabeller.
    public int TotalRows => Tables.Values.Sum(rows => rows.Count);

    // Henter alle rækker fra en bestemt SQL-tabel.
    public List<Dictionary<string, object?>> GetRows(string tableName)
    {
        return Tables[tableName];
    }
}
