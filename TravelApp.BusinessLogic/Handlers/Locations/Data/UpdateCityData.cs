namespace TravelApp.BusinessLogic.Handlers.Locations.Data
{
    /// <summary>Contains the values required to persist a city update.</summary>
    public class UpdateCityData
    {
        /// <summary>Gets or sets the city identifier.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the city name.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Gets or sets the country identifier.</summary>
        public int CountryId { get; set; }
    }
}
