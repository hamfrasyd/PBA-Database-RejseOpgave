using System.ComponentModel.DataAnnotations;

namespace TravelApp.BusinessLogic.Handlers.Users
{
    /// <summary>Contains the information needed to update user.</summary>
    public class UpdateUserRequest
    {
        /// <summary>Gets or sets the user id.</summary>
        [System.Text.Json.Serialization.JsonIgnore]
        public int UserId { get; set; }

        /// <summary>Gets or sets the first name.</summary>
        [Required, StringLength(128)]
        public string FirstName { get; set; } = string.Empty;
        /// <summary>Gets or sets the last name.</summary>
        [Required, StringLength(128)]
        public string LastName { get; set; } = string.Empty;
        /// <summary>Gets or sets the city id.</summary>
        [Range(1, int.MaxValue)]
        public int CityId { get; set; }
        /// <summary>Gets or sets whether is private.</summary>
        public bool IsPrivate { get; set; }
    }
}
