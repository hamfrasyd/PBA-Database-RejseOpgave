namespace TravelApp.BusinessLogic.DTOs.Trips
{
    public class TripRelationsDTO
    {
        public List<int> CityIds { get; set; } = [];

        public List<TripActivityRelationDTO> Activities { get; set; } = [];

        public List<int> ParticipantIds { get; set; } = [];
    }
}