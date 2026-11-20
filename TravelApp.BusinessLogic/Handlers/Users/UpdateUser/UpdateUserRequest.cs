using System.ComponentModel.DataAnnotations;

namespace TravelApp.BusinessLogic.Handlers.Users.UpdateUser
{
    /// <summary>Contains the information needed to update user.</summary>
    public class UpdateUserRequest
    {
        /// <summary>Gets or sets the first name.</summary>
        [StringLength(128)]
        public string? FirstName { get; set; }
        /// <summary>Gets or sets the last name.</summary>
        [StringLength(128)]
        public string? LastName { get; set; }
        /// <summary>Gets or sets the city id.</summary>
        [Range(1, int.MaxValue)]
        public int CityId { get; set; }
        /// <summary>Gets or sets whether is private.</summary>
        public bool? IsPrivate { get; set; }
    }
}
