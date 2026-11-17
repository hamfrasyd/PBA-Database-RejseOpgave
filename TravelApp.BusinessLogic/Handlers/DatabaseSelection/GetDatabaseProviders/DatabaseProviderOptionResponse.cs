namespace TravelApp.BusinessLogic.Handlers.DatabaseSelection.GetDatabaseProviders
{
    /// <summary>Describes one selectable database provider.</summary>
    public class DatabaseProviderOptionResponse
    {
        /// <summary>Gets the provider name.</summary>
        public string Name { get; init; } = string.Empty;
        /// <summary>Gets a value indicating whether the provider is implemented.</summary>
        public bool IsImplemented { get; init; }
    }
}
