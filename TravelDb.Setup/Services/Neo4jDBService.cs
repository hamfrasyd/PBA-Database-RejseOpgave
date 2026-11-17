
using Neo4j.Driver;

namespace TravelDb.Setup.Services;

/// <summary>Checks the Neo4j connection and creates unique id rules.</summary>
public class Neo4jDBService
{
    /// <summary>Checks the Neo4j connection and creates a unique id rule for each node type.</summary>
    /// <param name="uri">The Neo4j address, for example bolt://localhost:7687.</param>
    /// <param name="password">The password for the neo4j user.</param>
    public async Task InitializeAsync(string uri, string password)
    {
        //Opret forbindelse til Neo4j
        await using var driver = GraphDatabase.Driver(uri, AuthTokens.Basic("neo4j", password));

        //Kontroller forbindelsen
        await driver.VerifyConnectivityAsync();

        //Opret constraints
        string[] queries =
        {
            """
            CREATE CONSTRAINT user_id IF NOT EXISTS
            FOR (u:User)
            REQUIRE u.id IS UNIQUE
            """,

            """
            CREATE CONSTRAINT trip_id IF NOT EXISTS
            FOR (t:Trip)
            REQUIRE t.id IS UNIQUE
            """,

            """
            CREATE CONSTRAINT city_id IF NOT EXISTS
            FOR (c:City)
            REQUIRE c.id IS UNIQUE
            """,

            """
            CREATE CONSTRAINT group_id IF NOT EXISTS
            FOR (g:Group)
            REQUIRE g.id IS UNIQUE
            """
        };

        await using var session = driver.AsyncSession();

        foreach (var query in queries)
        {
            var result = await session.RunAsync(query);
            await result.ConsumeAsync();
        }
    }
}
