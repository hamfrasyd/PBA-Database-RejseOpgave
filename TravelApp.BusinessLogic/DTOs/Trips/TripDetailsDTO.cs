namespace TravelApp.BusinessLogic.DTOs.Trips
{
    /// <summary>Data Transfer Object representing detailed information about a trip.</summary>
    public class TripDetailsDTO
    {
        /// <summary>Gets or sets the unique identifier of the trip.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the name of the trip.</summary>
        public DateTime? StartDate { get; set; }

        /// <summary>Gets or sets the name of the trip.</summary>
        public DateTime? EndDate { get; set; }

        /// <summary>Gets or sets a value indicating whether the trip is completed.</summary>
        public bool IsCompleted { get; set; }

        /// <summary>Gets or sets the plan notes for the trip.</summary>
        public string? PlanNotes { get; set; }

        /// <summary>Gets or sets the experience notes for the trip.</summary>
        public string? ExperienceNotes { get; set; }

        /// <summary>Gets or sets the list of cities associated with the trip.</summary>
        public List<TripDestinationDTO> Cities { get; set; } = [];
    }
}