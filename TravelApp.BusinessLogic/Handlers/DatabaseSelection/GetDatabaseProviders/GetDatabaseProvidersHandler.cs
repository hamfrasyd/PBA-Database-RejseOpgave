using TravelApp.BusinessLogic.DatabaseSelection;

namespace TravelApp.BusinessLogic.Handlers.DatabaseSelection.GetDatabaseProviders
{
    /// <summary>Provides a database-independent description of selectable persistence providers.</summary>
    public class GetDatabaseProvidersHandler
    {
        private readonly SelectedDatabaseProvider _databaseProviderSelection;

        public GetDatabaseProvidersHandler(SelectedDatabaseProvider databaseProviderSelection)
        {
            _databaseProviderSelection = databaseProviderSelection;
        }

        public GetDatabaseProvidersResponse GetDatabaseProviders()
        {
            GetDatabaseProvidersResponse currentProviders = new GetDatabaseProvidersResponse
            {
                Active = _databaseProviderSelection.Provider.ToString(),
                Choices =
                [
                    new DatabaseProviderOptionResponse { Name = DatabaseProviderTypes.Sql.ToString(), IsImplemented = true },
                    new DatabaseProviderOptionResponse { Name = DatabaseProviderTypes.Mongo.ToString(), IsImplemented = false },
                    new DatabaseProviderOptionResponse { Name = DatabaseProviderTypes.Graph.ToString(), IsImplemented = false }
                ]
            };

            return currentProviders;
        }
    }
}
