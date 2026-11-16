namespace TravelApp.BusinessLogic.Handlers.Trips.TripCity
{
    /// <summary>Describes a city included in a trip response.</summary>
    public class GetTripCityResponse
    {
        /// <summary>Gets or sets the city identifier.</summary>
        public int Id { get; set; }
        /// <summary>Gets or sets the city name.</summary>
        public string City { get; set; } = string.Empty;
        /// <summary>Gets or sets the country name.</summary>
        public string Country { get; set; } = string.Empty;
        /// <summary>Gets or sets the continent name.</summary>
        public string Continent { get; set; } = string.Empty;
    }
}
