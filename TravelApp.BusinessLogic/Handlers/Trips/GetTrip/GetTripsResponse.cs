using TravelApp.BusinessLogic.Handlers.Destinations;

namespace TravelApp.BusinessLogic.Handlers.Trips.GetTrip
{
    public class GetTripsResponse
    {
        public int Id { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool IsCompleted { get; set; }

        public string? PlanNotes { get; set; }
        public string? ExperienceNotes { get; set; }

        public List<int> CityIds { get; set; } = [];
        public List<int> ActivityIds { get; set; } = [];
        public List<int> ParticipantIds { get; set; } = [];
    }
}