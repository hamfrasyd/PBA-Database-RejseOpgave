
namespace TravelApp.BusinessLogic.DTOs.Trips
{
    /// <summary>Data Transfer Object representing a summary of a trip.</summary>
    public class TripSummaryDTO
    {
        /// <summary>Gets or sets the unique identifier of the trip.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the name of the trip.</summary>
        public DateTime? StartDate { get; set; }

        /// <summary>Gets or sets the name of the trip.</summary>
        public DateTime? EndDate { get; set; }

        /// <summary>Gets or sets the name of the trip.</summary>
        public bool IsCompleted { get; set; }

        /// <summary>Gets or sets the name of the trip.</summary>
        public string? PlanNotes { get; set; }

        /// <summary>Gets or sets the name of the trip.</summary>
        public string? ExperienceNotes { get; set; }

        /// <summary>Gets or sets the list of cities associated with the trip.</summary>
        public List<TripCitySummaryDTO> Cities { get; set; } = [];

        /// <summary>Gets or sets the list of activities associated with the trip.</summary>
        public int ActivityCount { get; set; }

        /// <summary>Gets or sets the list of participants associated with the trip.</summary>
        public int ParticipantCount { get; set; }
    }
}
