namespace TravelApp.BusinessLogic.DatabaseSelection
{
    /// <summary>Holds the database provider selected for the current scope.</summary>
    public class SelectedDatabaseProvider
    {
        /// <summary>Initializes a new instance of the <see cref="SelectedDatabaseProvider"/> class.</summary>
        public SelectedDatabaseProvider()
        {
        }

        /// <summary>Gets or sets the selected database provider.</summary>
        public DatabaseProviderTypes Provider { get; set; } = DatabaseProviderTypes.Sql;
    }
}
