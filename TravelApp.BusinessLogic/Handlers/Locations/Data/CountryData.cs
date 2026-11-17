namespace TravelApp.BusinessLogic.Handlers.Locations.Data
{
    /// <summary>Contains the country fields exchanged with a repository.</summary>
    public class CountryData
    {
        /// <summary>Gets or sets the country identifier.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the country name.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Gets or sets the continent identifier.</summary>
        public int ContinentId { get; set; }
    }
}
