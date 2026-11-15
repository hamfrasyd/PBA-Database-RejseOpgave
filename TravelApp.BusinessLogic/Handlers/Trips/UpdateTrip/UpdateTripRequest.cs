using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TravelApp.BusinessLogic.Handlers.Trips
{
    public class UpdateTripRequest
    {
        //JsonIgnore is used to prevent the TripId from being included in the JSON payload when the request is sent.
        [JsonIgnore]
        public int TripId { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool IsCompleted { get; set; }

        [StringLength(1024)]
        public string? PlanNotes { get; set; }

        [StringLength(1024)]
        public string? ExperienceNotes { get; set; }
    }
}