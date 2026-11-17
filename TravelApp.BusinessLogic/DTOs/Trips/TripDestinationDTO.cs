namespace TravelApp.BusinessLogic.DTOs.Trips
{
    /// <summary>Data Transfer Object representing a trip destination.</summary>
    public class TripDestinationDTO
    {
        /// <summary>Gets or sets the unique identifier of the trip destination.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the name of the trip destination city.</summary>
        public string City { get; set; } = string.Empty;

        /// <summary>Gets or sets the country of the trip destination city.</summary>
        public string Country { get; set; } = string.Empty;

        /// <summary>Gets or sets the continent of the trip destination city.</summary>
        public string Continent { get; set; } = string.Empty;
    }
}