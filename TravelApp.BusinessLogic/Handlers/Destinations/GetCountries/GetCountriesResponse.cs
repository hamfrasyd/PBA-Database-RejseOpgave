namespace TravelApp.BusinessLogic.Handlers.Destinations
{
    public class GetCountriesResponse
    {
        public int Id { get; set; }
        public int ContinentId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
