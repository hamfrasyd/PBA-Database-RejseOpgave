namespace TravelApp.BusinessLogic.Handlers.Trips.GetTrip
{
    /// <summary>Response model for retrieving trip city details for all trips.</summary>
    public class GetTripsCityResponse
    {
        /// <summary>Gets or sets the unique identifier of the trip city.</summary>
        public int Id { get; set; }
        /// <summary>Gets or sets the name of the city.</summary>
        public string Name { get; set; } = string.Empty;
    }
}