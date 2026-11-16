
namespace TravelApp.BusinessLogic.DTOs.Trips
{
    public class TripSummaryDTO
    {
        public int Id { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool IsCompleted { get; set; }

        public string? PlanNotes { get; set; }
        public string? ExperienceNotes { get; set; }

        public List<TripCitySummaryDTO> Cities { get; set; } = [];

        public int ActivityCount { get; set; }
        public int ParticipantCount { get; set; }
    }
}
