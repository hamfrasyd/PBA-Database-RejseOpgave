namespace TravelApp.BusinessLogic.DTOs.Trips
{
    public class TripDestinationDTO
    {
        public int Id { get; set; }

        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Continent { get; set; } = string.Empty;
    }
}