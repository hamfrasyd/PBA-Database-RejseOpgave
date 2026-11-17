namespace TravelApp.BusinessLogic.Handlers.Locations.Data
{
    /// <summary>Contains the persisted fields of a city exchanged with a repository.</summary>
    public class CityDTO
    {
        /// <summary>Gets or sets the city identifier.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the city name.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Gets or sets the country identifier.</summary>
        public int CountryId { get; set; }
    }
}
