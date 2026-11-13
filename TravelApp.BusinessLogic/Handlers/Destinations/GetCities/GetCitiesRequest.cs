namespace TravelApp.BusinessLogic.Handlers.Destinations
{
    /// <summary>Contains the information needed to get cities.</summary>
    public class GetCitiesRequest
    {
        /// <summary>Gets or sets the country id.</summary>
        public int? CountryId { get; set; }
        /// <summary>Gets or sets the search.</summary>
        public string? Search { get; set; }
        /// <summary>Gets or sets whether descending.</summary>
        public bool Descending { get; set; }
    }
}
