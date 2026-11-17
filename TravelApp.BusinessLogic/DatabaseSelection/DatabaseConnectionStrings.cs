namespace TravelApp.BusinessLogic.DatabaseSelection
{
    /// <summary>Central configuration for current and future database connections.</summary>
    public class DatabaseConnectionStrings
    {
        /// <summary>Gets the SQL statement.</summary>
        public required string Sql { get; init; } // Set at initialization, required.
        /// <summary>Gets the MongoDB connection string.</summary>
        public string? Mongo { get; init; } // Not implemented yet, so nullable for now
        /// <summary>Gets the Neo4j connection string.</summary>
        public string? Neo4j { get; init; } // Not implemented yet, so nullable for now
    }
}
