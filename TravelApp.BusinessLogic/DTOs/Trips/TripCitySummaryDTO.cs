namespace TravelApp.BusinessLogic.DTOs.Trips
{
    /// <summary>Data Transfer Object representing a summary of a trip city.</summary>
    public class TripCitySummaryDTO
    {
        /// <summary>Gets or sets the unique identifier of the trip city.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the name of the trip city.</summary>
        public string Name { get; set; } = string.Empty;
    }
}