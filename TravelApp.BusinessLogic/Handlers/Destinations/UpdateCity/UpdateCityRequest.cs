using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TravelApp.BusinessLogic.Models;

namespace TravelApp.BusinessLogic.Handlers.Destinations
{
    /// <summary>Contains the information needed to update city.</summary>
    public class UpdateCityRequest
    {
        /// <summary>Gets or sets the city id.</summary>
        [JsonIgnore]
        public int CityId { get; set; }

        /// <summary>Gets or sets the name.</summary>
        [Required, StringLength(64)]
        public string Name { get; set; } = string.Empty;

        /// <summary>Gets or sets the Country for the city.</summary>
        public Country Country { get; set; } = new Country();
    }
}
