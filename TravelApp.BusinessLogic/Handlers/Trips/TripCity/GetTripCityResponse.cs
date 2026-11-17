namespace TravelApp.BusinessLogic.Handlers.Trips.TripCity
{
    /// <summary>Response model for retrieving trip city details.</summary>
    public class GetTripCityResponse
    {
        /// <summary>Gets or sets the unique identifier of the trip city.</summary>
        public int Id { get; set; }
        /// <summary>Gets or sets the name of the city.</summary>
        public string City { get; set; } = string.Empty;
        /// <summary>Gets or sets the country of the city.</summary>
        public string Country { get; set; } = string.Empty;
        /// <summary>Gets or sets the continent of the city.</summary>
        public string Continent { get; set; } = string.Empty;
    }
}