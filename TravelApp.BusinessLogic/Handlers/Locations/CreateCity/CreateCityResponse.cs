namespace TravelApp.BusinessLogic.Handlers.Locations.CreateCity
{
    /// <summary>Contains the city that was created.</summary>
    public class CreateCityResponse
    {
        /// <summary>Gets or sets the city identifier.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the city name.</summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>Gets or sets the country identifier.</summary>
        public int CountryId { get; set; }
    }
}
