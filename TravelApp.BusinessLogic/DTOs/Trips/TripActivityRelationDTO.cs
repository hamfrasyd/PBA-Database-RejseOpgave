namespace TravelApp.BusinessLogic.DTOs.Trips
{
    /// <summary>Identifies an activity linked to a trip and the city it belongs to.</summary>
    public class TripActivityRelationDTO
    {
        /// <summary>Gets or sets the activity identifier.</summary>
        public int Id { get; set; }
        /// <summary>Gets or sets the city identifier.</summary>
        public int CityId { get; set; }
    }
}
