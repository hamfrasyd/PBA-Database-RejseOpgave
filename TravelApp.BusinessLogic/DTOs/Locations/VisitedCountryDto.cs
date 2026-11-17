namespace TravelApp.BusinessLogic.Handlers.Locations.Data
{
    /// <summary>Contains a country visited on a completed trip.</summary>
    public class VisitedCountryDto
    {
        /// <summary>Gets or sets the country identifier.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the continent identifier.</summary>
        public int ContinentId { get; set; }

        /// <summary>Gets or sets the country name.</summary>
        public string Name { get; set; } = string.Empty;
    }
}
