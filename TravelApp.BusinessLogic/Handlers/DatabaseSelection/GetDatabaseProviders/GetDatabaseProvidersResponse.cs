namespace TravelApp.BusinessLogic.Handlers.DatabaseSelection.GetDatabaseProviders
{
    /// <summary>Describes the database providers supported by this API.</summary>
    public class GetDatabaseProvidersResponse
    {
        /// <summary>Gets the name of the active database provider.</summary>
        public string Active { get; init; } = string.Empty;
        /// <summary>Gets the header text.</summary>
        public string Header { get; init; } = string.Empty;
        /// <summary>Gets the selectable database providers.</summary>
        public List<DatabaseProviderOptionResponse> Choices { get; init; } = [];
    }
}