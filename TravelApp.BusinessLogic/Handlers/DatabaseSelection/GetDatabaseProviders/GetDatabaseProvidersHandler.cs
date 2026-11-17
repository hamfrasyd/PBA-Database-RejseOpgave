using TravelApp.BusinessLogic.DatabaseSelection;

namespace TravelApp.BusinessLogic.Handlers.DatabaseSelection.GetDatabaseProviders
{
    /// <summary>Provides a database-independent description of selectable persistence providers.</summary>
    public class GetDatabaseProvidersHandler
    {
        private readonly SelectedDatabaseProvider _databaseProviderSelection;

        /// <summary>Initializes a new instance of the <see cref="GetDatabaseProvidersHandler"/> class.</summary>
        /// <param name="databaseProviderSelection">The currently selected database provider.</param>
        public GetDatabaseProvidersHandler(SelectedDatabaseProvider databaseProviderSelection)
        {
            _databaseProviderSelection = databaseProviderSelection;
        }

        /// <summary>Returns the active provider and the available choices.</summary>
        /// <returns>The active provider and the selectable choices.</returns>
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
