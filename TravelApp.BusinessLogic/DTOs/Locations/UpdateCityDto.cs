namespace TravelApp.BusinessLogic.DTOs.Locations
{
    /// <summary>Contains the values required to persist a city update.</summary>
    public class UpdateCityDTO
    {
        /// <summary>Gets or sets the city identifier.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the city name.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Gets or sets the country identifier.</summary>
        public int CountryId { get; set; }
    }
}
