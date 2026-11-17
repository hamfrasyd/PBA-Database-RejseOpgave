namespace TravelApp.BusinessLogic.Handlers.Trips.GetTrip
{
    /// <summary>Summarizes a city in a trip list response.</summary>
    public class GetTripsCityResponse
    {
        /// <summary>Gets or sets the city identifier.</summary>
        public int Id { get; set; }
        /// <summary>Gets or sets the city name.</summary>
        public string Name { get; set; } = string.Empty;
    }
}
