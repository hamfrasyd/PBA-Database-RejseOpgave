namespace TravelApp.BusinessLogic.DTOs.Trips
{
    /// <summary>Describes a destination city on a trip.</summary>
    public class TripDestinationDTO
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
