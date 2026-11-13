namespace TravelApp.BusinessLogic.DatabaseSelection
{
    public class SelectedDatabaseProvider()
    {
        public DatabaseProviderTypes Provider { get; set; } = DatabaseProviderTypes.Sql;
    }
}
