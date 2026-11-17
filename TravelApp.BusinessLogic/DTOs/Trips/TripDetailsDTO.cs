namespace TravelApp.BusinessLogic.DTOs.Trips
{
    public class TripDetailsDTO
    {
        public int Id { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool IsCompleted { get; set; }

        public string? PlanNotes { get; set; }
        public string? ExperienceNotes { get; set; }

        public List<TripDestinationDTO> Cities { get; set; } = [];
    }
}