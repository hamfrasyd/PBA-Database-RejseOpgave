namespace TravelApp.BusinessLogic.Handlers.Locations.GetCountries
{
    /// <summary>Contains the result of get countries.</summary>
    public class GetCountriesResponse
    {
        /// <summary>Gets or sets the entity ID.</summary>
        public int Id { get; set; }
        /// <summary>Gets or sets the continent id.</summary>
        public int ContinentId { get; set; }
        /// <summary>Gets or sets the name.</summary>
        public string Name { get; set; } = string.Empty;
    }
}
