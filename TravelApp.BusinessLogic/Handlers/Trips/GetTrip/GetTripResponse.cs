using TravelApp.BusinessLogic.Handlers.Trips.TripCity;

namespace TravelApp.BusinessLogic.Handlers.Trips.GetTrip
{
    /// <summary>Response containing the details of a trip.</summary>
    public class GetTripResponse
    {
        /// <summary>Gets or sets the trip identifier.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the start date.</summary>
        public DateTime? StartDate { get; set; }
        /// <summary>Gets or sets the end date.</summary>
        public DateTime? EndDate { get; set; }

        /// <summary>Gets or sets a value indicating whether the trip is completed.</summary>
        public bool IsCompleted { get; set; }

        /// <summary>Gets or sets the planning notes.</summary>
        public string? PlanNotes { get; set; }
        /// <summary>Gets or sets the experience notes.</summary>
        public string? ExperienceNotes { get; set; }

        /// <summary>Gets or sets the cities on the trip.</summary>
        public List<GetTripCityResponse> Cities { get; set; } = [];
    }
}
