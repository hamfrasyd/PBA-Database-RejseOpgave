namespace TravelApp.BusinessLogic.DTOs.Trips
{
    /// <summary>Contains the cities, activities, and participants linked to a trip.</summary>
    public class TripRelationsDTO
    {
        /// <summary>Gets or sets the city identifiers.</summary>
        public List<int> CityIds { get; set; } = [];

        /// <summary>Gets or sets the linked activities.</summary>
        public List<TripActivityRelationDTO> Activities { get; set; } = [];

        /// <summary>Gets or sets the participant identifiers.</summary>
        public List<int> ParticipantIds { get; set; } = [];
    }
}
