namespace TravelApp.BusinessLogic.DTOs.Users
{
    /// <summary>Contains the basic user fields an update may change: name, home city, and privacy.</summary>
    public class UpdateUserBasicInfoDto
    {
        /// <summary>Gets or sets the user identifier.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the first name.</summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>Gets or sets the last name.</summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>Gets or sets the home city identifier.</summary>
        public int CityId { get; set; }

        /// <summary>Gets or sets a value indicating whether the profile is private.</summary>
        public bool IsPrivate { get; set; }
    }
}
