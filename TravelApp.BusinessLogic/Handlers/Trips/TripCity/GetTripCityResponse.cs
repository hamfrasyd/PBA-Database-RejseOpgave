namespace TravelApp.BusinessLogic.Handlers.Trips.TripCity
{
    public class GetTripCityResponse
    {
        public int Id { get; set; }
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Continent { get; set; } = string.Empty;
    }
}