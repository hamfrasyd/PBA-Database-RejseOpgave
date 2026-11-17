
using Neo4j.Driver;
using TravelDb.Migrator.Models;

namespace TravelDb.Migrator.Services;

/// <summary>Copies the SQL data into Neo4j.</summary>
public class Neo4jMigrationService
{
    private readonly string _uri;
    private readonly string _password;

    /// <summary>Stores the Neo4j address and password.</summary>
    /// <param name="uri">The Neo4j address, for example bolt://localhost:7687.</param>
    /// <param name="password">The password for the neo4j user.</param>
    public Neo4jMigrationService(string uri, string password)
    {
        _uri = uri;
        _password = password;
    }

    /// <summary>Creates Neo4j nodes and the links between them.</summary>
    /// <param name="snapshot">The copied SQL data.</param>
    /// <returns>A task that finishes when the graph has been written.</returns>
    // Migrerer SQL-data til Neo4j.
    public async Task MigrateAsync(SqlSnapshot snapshot)
    {
        // Opretter forbindelse til Neo4j.
        await using var driver = GraphDatabase.Driver(_uri, AuthTokens.Basic("neo4j", _password));

        // Kontrollerer at Neo4j er tilgængelig.
        await driver.VerifyConnectivityAsync();

        // Opretter en session til databasen.
        await using var session = driver.AsyncSession(config => config.WithDatabase("neo4j"));

        // Opretter først nodes fra SQL-tabellerne.
        await Neo4jNodeMapper.MigrateAsync(session, snapshot);

        // Opretter derefter relationships mellem nodes.
        await Neo4jRelationshipMapper.MigrateAsync(session, snapshot);
    }
}
