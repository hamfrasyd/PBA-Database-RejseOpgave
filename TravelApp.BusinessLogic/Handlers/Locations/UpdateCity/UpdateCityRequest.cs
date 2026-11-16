using System.ComponentModel.DataAnnotations;
using TravelApp.BusinessLogic.Models.Destinations;

namespace TravelApp.BusinessLogic.Handlers.Locations.UpdateCity
{
    /// <summary>Contains the information needed to update city.</summary>
    public class UpdateCityRequest
    {
        /// <summary>Gets or sets the name.</summary>
        [Required, StringLength(64)]
        public string Name { get; set; } = string.Empty;

        /// <summary>Gets or sets the Country for the city.</summary>
        public Country Country { get; set; } = new Country();
    }
}
