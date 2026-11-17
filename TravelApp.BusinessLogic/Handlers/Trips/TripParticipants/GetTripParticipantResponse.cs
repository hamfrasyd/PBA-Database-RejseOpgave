namespace TravelApp.BusinessLogic.Handlers.Trips.TripParticipants
{
    /// <summary>Response model for retrieving trip participant details.</summary>
    public class GetTripParticipantResponse
    {
        /// <summary>Gets or sets the unique identifier of the trip participant.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the first name of the trip participant.</summary>
        public string FullName { get; set; } = string.Empty;
    }
}