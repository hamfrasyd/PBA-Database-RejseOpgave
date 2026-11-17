namespace TravelApp.BusinessLogic.DTOs.Trips
{
    /// <summary>Data Transfer Object representing a trip activity.</summary>
    public class TripActivityDTO
    {
        /// <summary>Gets or sets the unique identifier of the trip activity.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the unique identifier of the trip associated with the activity.</summary>
        public int CityId { get; set; }

        /// <summary>Gets or sets the unique identifier of the activity.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Gets or sets the description of the activity.</summary>
        public string? Description { get; set; }
    }
}