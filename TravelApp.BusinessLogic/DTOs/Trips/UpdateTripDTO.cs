namespace TravelApp.BusinessLogic.DTOs.Trips
{
    /// <summary>Data Transfer Object for updating trip details.</summary>
    public class UpdateTripDTO
    {
        /// <summary>Gets or sets the unique identifier of the trip.</summary>
        public DateTime? StartDate { get; set; }

        /// <summary>Gets or sets the unique identifier of the trip.</summary>
        public DateTime? EndDate { get; set; }

        /// <summary>Gets or sets the unique identifier of the trip.</summary>
        public bool? IsCompleted { get; set; }

        /// <summary>Gets or sets the unique identifier of the trip.</summary>
        public string? PlanNotes { get; set; }

        /// <summary>Gets or sets the unique identifier of the trip.</summary>
        public string? ExperienceNotes { get; set; }
    }
}