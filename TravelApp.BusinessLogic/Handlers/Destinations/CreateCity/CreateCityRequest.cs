using System.ComponentModel.DataAnnotations;

namespace TravelApp.BusinessLogic.Handlers.Destinations
{
    /// <summary>Contains the information needed to create city.</summary>
    public class CreateCityRequest
    {
        /// <summary>Gets or sets the country id.</summary>
        [Range(1, int.MaxValue)]
        public int CountryId { get; set; }

        /// <summary>Gets or sets the name.</summary>
        [Required, StringLength(64)]
        public string Name { get; set; } = string.Empty;
    }
}
