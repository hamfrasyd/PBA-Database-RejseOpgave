using System.ComponentModel.DataAnnotations;

namespace TravelApp.BusinessLogic.Handlers.Accounts.Register
{
    /// <summary>Contains the profile and credentials for a new account.</summary>
    public sealed class RegisterRequest
    {
        /// <summary>Gets or sets the first name.</summary>
        [Required, StringLength(128)]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>Gets or sets the last name.</summary>
        [Required, StringLength(128)]
        public string LastName { get; set; } = string.Empty;

        /// <summary>Gets or sets the date of birth.</summary>
        [Required]
        public DateOnly? Birthday { get; set; } 

        /// <summary>Gets or sets the ID of the user's home city.</summary>
        [Range(1, int.MaxValue)]
        public int CityId { get; set; }

        /// <summary>Gets or sets whether the profile is private.</summary>
        public bool IsPrivate { get; set; } = true;

        /// <summary>Gets or sets the user name used to log in.</summary>
        [Required, StringLength(128)]
        public string UserName { get; set; } = string.Empty;

        /// <summary>Gets or sets the proposed password.</summary>
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}