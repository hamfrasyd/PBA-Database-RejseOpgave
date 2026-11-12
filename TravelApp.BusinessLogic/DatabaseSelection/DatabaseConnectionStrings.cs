namespace TravelApp.BusinessLogic.DatabaseSelection
{
    /// <summary>Central configuration for current and future database connections.</summary>
    public class DatabaseConnectionStrings
    {
        public required string Sql { get; init; } // Set at initialization, required.
        public string? Mongo { get; init; } // Not implemented yet, so nullable for now
        public string? Neo4j { get; init; } // Not implemented yet, so nullable for now
    }
}
