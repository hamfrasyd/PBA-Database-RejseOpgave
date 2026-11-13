using System.ComponentModel.DataAnnotations;

namespace TravelApp.BusinessLogic.Handlers.Destinations
{
    /// <summary>Contains the information needed to update activity.</summary>
    public class UpdateActivityRequest
    {
        /// <summary>Gets or sets the activity id.</summary>
        [System.Text.Json.Serialization.JsonIgnore]
        public int ActivityId { get; set; }

        /// <summary>Gets or sets the city id.</summary>
        [Range(1, int.MaxValue)]
        public int CityId { get; set; }

        /// <summary>Gets or sets the name.</summary>
        [Required, StringLength(128)]
        public string Name { get; set; } = string.Empty;

        /// <summary>Gets or sets the description.</summary>
        [StringLength(1024)]
        public string? Description { get; set; }
    }
}
