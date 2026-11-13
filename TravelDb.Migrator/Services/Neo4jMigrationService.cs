
using Neo4j.Driver;
using TravelDb.Migrator.Models;

namespace TravelDb.Migrator.Services;

public class Neo4jMigrationService
{
    private readonly string _uri;
    private readonly string _password;

    public Neo4jMigrationService(string uri, string password)
    {
        _uri = uri;
        _password = password;
    }

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
