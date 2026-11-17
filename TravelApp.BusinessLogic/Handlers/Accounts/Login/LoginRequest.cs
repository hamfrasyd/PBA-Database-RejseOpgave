using System.ComponentModel.DataAnnotations;

namespace TravelApp.BusinessLogic.Handlers.Accounts.Login
{
    /// <summary>Contains the credentials for a local account.</summary>
    public class LoginRequest
    {
        /// <summary>Gets or sets the user name.</summary>
        [Required]
        [StringLength(128)]
        public string UserName { get; set; } = string.Empty;

        /// <summary>Gets or sets the password.</summary>
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}