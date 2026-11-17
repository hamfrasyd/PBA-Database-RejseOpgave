namespace TravelApp.BusinessLogic.Handlers.Locations.Data
{
    /// <summary>Contains the repository filter and sort options for cities.</summary>
    public class GetCitiesFilterDTO
    {
        /// <summary>Gets or sets the country identifier to filter by.</summary>
        public int? CountryId { get; set; }

        /// <summary>Gets or sets the name search text.</summary>
        public string? Search { get; set; }

        /// <summary>Gets or sets whether cities are ordered by name descending.</summary>
        public bool Descending { get; set; }
    }
}
