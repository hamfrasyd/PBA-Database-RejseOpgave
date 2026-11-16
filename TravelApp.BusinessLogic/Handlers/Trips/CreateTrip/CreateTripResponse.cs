using System.ComponentModel.DataAnnotations;
using TravelApp.BusinessLogic.Handlers.Trips.TripCity;

namespace TravelApp.BusinessLogic.Handlers.Trips.CreateTrip
{
    public class CreateTripResponse
    {
        public int Id { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [StringLength(1024)]
        public string? PlanNotes { get; set; }
        public bool IsCompleted { get; set; }

    }
}