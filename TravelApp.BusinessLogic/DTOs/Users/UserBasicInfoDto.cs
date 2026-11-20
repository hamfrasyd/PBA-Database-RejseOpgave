namespace TravelApp.BusinessLogic.DTOs.Users
{
    /// <summary>Contains the basic user fields written by an update: identifier, first name, last name, home city, and privacy.</summary>
    public class UserBasicInfoDto
    {
        /// <summary>Gets or sets the user identifier.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the first name.</summary>
        public string? FirstName { get; set; }

        /// <summary>Gets or sets the last name.</summary>
        public string? LastName { get; set; }

        /// <summary>Gets or sets the home city identifier.</summary>
        public int CityId { get; set; }

        /// <summary>Gets or sets a value indicating whether the profile is private.</summary>
        public bool? IsPrivate { get; set; }
    }
}
