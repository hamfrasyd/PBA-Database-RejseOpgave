namespace TravelApp.BusinessLogic.Handlers.Locations.UpdateCity
{
    /// <summary>Contains the city that was updated.</summary>
    public class UpdateCityResponse
    {
        /// <summary>Gets or sets the city identifier.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the country identifier.</summary>
        public int CountryId { get; set; }

        /// <summary>Gets or sets the city name.</summary>
        public string Name { get; set; } = string.Empty;
    }
}
