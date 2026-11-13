namespace TravelApp.BusinessLogic.Handlers.Destinations
{
    /// <summary>Contains the result of get cities.</summary>
    public class GetCitiesResponse
    {
        /// <summary>Gets or sets the entity ID.</summary>
        public int Id { get; set; }
        /// <summary>Gets or sets the country id.</summary>
        public int CountryId { get; set; }
        /// <summary>Gets or sets the name.</summary>
        public string Name { get; set; } = string.Empty;
    }
}
