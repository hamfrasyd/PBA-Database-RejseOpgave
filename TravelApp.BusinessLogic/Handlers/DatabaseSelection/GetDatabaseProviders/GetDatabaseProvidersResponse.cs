namespace TravelApp.BusinessLogic.Handlers.DatabaseSelection.GetDatabaseProviders
{
    /// <summary>Describes the database providers supported by this API.</summary>
    public class GetDatabaseProvidersResponse
    {
        public string Active { get; init; } = string.Empty;
        public string Header { get; init; } = string.Empty;
        public List<DatabaseProviderOptionResponse> Choices { get; init; } = [];
    }
}