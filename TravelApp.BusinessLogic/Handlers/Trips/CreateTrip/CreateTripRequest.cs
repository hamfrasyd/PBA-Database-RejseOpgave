using System.ComponentModel.DataAnnotations;

namespace TravelApp.BusinessLogic.Handlers.Trips
{
    /// <summary>Request model for creating a new trip.</summary>
    public class CreateTripRequest
    {
        /// <summary>Gets or sets the name of the trip.</summary>
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [StringLength(1024)]
        public string? PlanNotes { get; set; }
    }
}