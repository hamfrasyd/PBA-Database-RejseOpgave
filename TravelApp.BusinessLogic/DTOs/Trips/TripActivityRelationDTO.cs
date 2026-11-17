namespace TravelApp.BusinessLogic.DTOs.Trips
{
    /// <summary>Data Transfer Object representing a relation between a trip and an activity.</summary>
    public class TripActivityRelationDTO
    {
        /// <summary>Gets or sets the unique identifier of the trip activity relation.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the unique identifier of the trip associated with the activity.</summary>
        public int CityId { get; set; }
    }
}