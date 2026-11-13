namespace TravelApp.BusinessLogic.Handlers.Destinations
{
    /// <summary>Contains the result of get visited countries.</summary>
    public class GetVisitedCountriesResponse
    {
        /// <summary>Gets or sets the entity ID.</summary>
        public int Id { get; set; }
        /// <summary>Gets or sets the continent id.</summary>
        public int ContinentId { get; set; }
        /// <summary>Gets or sets the name.</summary>
        public string Name { get; set; } = string.Empty;
    }
}
