using System.ComponentModel.DataAnnotations;

namespace TravelApp.BusinessLogic.Handlers.Trips
{
    public class CreateTripRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [StringLength(1024)]
        public string? PlanNotes { get; set; }
    }
}