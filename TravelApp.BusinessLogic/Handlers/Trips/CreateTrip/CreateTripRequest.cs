using System.ComponentModel.DataAnnotations;

namespace TravelApp.BusinessLogic.Handlers.Trips
{
    /// <summary>Request to create a trip.</summary>
    public class CreateTripRequest
    {
        /// <summary>Gets or sets the start date.</summary>
        public DateTime? StartDate { get; set; }
        /// <summary>Gets or sets the end date.</summary>
        public DateTime? EndDate { get; set; }

        /// <summary>Gets or sets the planning notes.</summary>
        [StringLength(1024)]
        public string? PlanNotes { get; set; }
    }
}
