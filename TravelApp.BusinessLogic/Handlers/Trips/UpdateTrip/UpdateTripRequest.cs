using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TravelApp.BusinessLogic.Handlers.Trips.UpdateTrip
{
    /// <summary>Request to update a trip.</summary>
    public class UpdateTripRequest
    {
        //JsonIgnore is used to prevent the TripId from being included in the JSON payload when the request is sent.
        /// <summary>Gets or sets the trip identifier.</summary>
        [JsonIgnore]
        public int TripId { get; set; }

        /// <summary>Gets or sets the start date.</summary>
        public DateTime? StartDate { get; set; }
        /// <summary>Gets or sets the end date.</summary>
        public DateTime? EndDate { get; set; }

        /// <summary>Gets or sets a value indicating whether the trip is completed.</summary>
        public bool IsCompleted { get; set; }

        /// <summary>Gets or sets the planning notes.</summary>
        [StringLength(1024)]
        public string? PlanNotes { get; set; }

        /// <summary>Gets or sets the experience notes.</summary>
        [StringLength(1024)]
        public string? ExperienceNotes { get; set; }
    }
}
