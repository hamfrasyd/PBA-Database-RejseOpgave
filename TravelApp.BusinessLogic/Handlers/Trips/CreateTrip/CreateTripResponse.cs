using System.ComponentModel.DataAnnotations;

namespace TravelApp.BusinessLogic.Handlers.Trips.CreateTrip
{
    /// <summary>Response returned after a trip is created.</summary>
    public class CreateTripResponse
    {
        /// <summary>Gets or sets the trip identifier.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the start date.</summary>
        public DateTime? StartDate { get; set; }
        /// <summary>Gets or sets the end date.</summary>
        public DateTime? EndDate { get; set; }
        /// <summary>Gets or sets the planning notes.</summary>
        [StringLength(1024)]
        public string? PlanNotes { get; set; }
        /// <summary>Gets or sets a value indicating whether the trip is completed.</summary>
        public bool IsCompleted { get; set; }

    }
}
