using TravelApp.BusinessLogic.Handlers.Trips.TripCity;

namespace TravelApp.BusinessLogic.Handlers.Trips.GetTrip
{
    public class GetTripResponse
    {
        public int Id { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool IsCompleted { get; set; }

        public string? PlanNotes { get; set; }
        public string? ExperienceNotes { get; set; }

        public List<GetTripCityResponse> Cities { get; set; } = [];
    }
}