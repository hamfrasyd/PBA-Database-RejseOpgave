namespace TravelApp.BusinessLogic.Handlers.Destinations
{
    /// <summary>Contains the result of get activity.</summary>
    public class GetActivityResponse
    {
        /// <summary>Gets or sets the entity ID.</summary>
        public int Id { get; set; }
        /// <summary>Gets or sets the city id.</summary>
        public int CityId { get; set; }
        /// <summary>Gets or sets the name.</summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>Gets or sets the description.</summary>
        public string? Description { get; set; }
    }
}
