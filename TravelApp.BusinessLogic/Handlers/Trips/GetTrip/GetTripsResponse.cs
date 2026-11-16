
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

        public List<GetTripsCityResponse> Cities { get; set; } = [];

        public int ActivityCount { get; set; }
        public int ParticipantCount { get; set; }
    }
}
