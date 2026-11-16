namespace TravelApp.BusinessLogic.Handlers.Trips.TripParticipants
{
    /// <summary>Describes a participant on a trip.</summary>
    public class GetTripParticipantResponse
    {
        /// <summary>Gets or sets the user identifier.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the participant's full name.</summary>
        public string FullName { get; set; } = string.Empty;
    }
}
