namespace TravelApp.BusinessLogic.DTOs.Trips
{
    /// <summary>Data Transfer Object representing the relations of a trip.</summary>
    public class TripRelationsDTO
    {
        /// <summary>Gets or sets the unique identifier of the trip for Cities.</summary>
        public List<int> CityIds { get; set; } = [];

        /// <summary>Gets or sets the unique identifier of the trip for activities.</summary>
        public List<TripActivityRelationDTO> Activities { get; set; } = [];

        /// <summary>Gets or sets the unique identifier of the trip for participants.</summary>
        public List<int> ParticipantIds { get; set; } = [];
    }
}