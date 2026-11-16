namespace TravelApp.BusinessLogic.DTOs.Trips
{
    /// <summary>Summarizes a city on a trip.</summary>
    public class TripCitySummaryDTO
    {
        /// <summary>Gets or sets the city identifier.</summary>
        public int Id { get; set; }
        /// <summary>Gets or sets the city name.</summary>
        public string Name { get; set; } = string.Empty;
    }
}
