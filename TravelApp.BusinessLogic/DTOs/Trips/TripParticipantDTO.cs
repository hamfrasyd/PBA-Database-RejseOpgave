namespace TravelApp.BusinessLogic.DTOs.Trips
{
    /// <summary>Data Transfer Object representing a trip participant.</summary>
    public class TripParticipantDTO
    {
        /// <summary>Gets or sets the unique identifier of the trip participant.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the first name of the trip participant.</summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>Gets or sets the last name of the trip participant.</summary>
        public string LastName { get; set; } = string.Empty;
    }
}