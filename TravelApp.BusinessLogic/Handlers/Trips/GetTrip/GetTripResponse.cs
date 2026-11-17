using TravelApp.BusinessLogic.Handlers.Trips.TripCity;

namespace TravelApp.BusinessLogic.Handlers.Trips.GetTrip
{
    /// <summary>Response model for retrieving trip details.</summary>
    public class GetTripResponse
    {
        /// <summary>Gets or sets the unique identifier of the trip.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the start date of the trip.</summary>
        public DateTime? StartDate { get; set; }

        /// <summary>Gets or sets the end date of the trip.</summary>
        public DateTime? EndDate { get; set; }

        /// <summary>Gets or sets a value indicating whether the trip is completed.</summary>
        public bool IsCompleted { get; set; }

        /// <summary>Gets or sets the plan notes for the trip.</summary>
        public string? PlanNotes { get; set; }

        /// <summary>Gets or sets the experience notes for the trip.</summary>
        public string? ExperienceNotes { get; set; }

        /// <summary>Gets or sets the list of cities associated with the trip.</summary>
        public List<GetTripCityResponse> Cities { get; set; } = [];
    }
}