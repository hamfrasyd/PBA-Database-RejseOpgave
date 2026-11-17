namespace TravelApp.BusinessLogic.DTOs.Locations
{
    /// <summary>Contains the values required to persist a new city.</summary>
    public class CreateCityDTO
    {
        /// <summary>Gets or sets the city name.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Gets or sets the country identifier.</summary>
        public int CountryId { get; set; }
    }
}
