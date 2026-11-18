using TravelApp.BusinessLogic.Models.Users;

namespace TravelApp.DataAccess.Auth
{
    /// <summary>Represents a local login account linked to a TravelDB profile.</summary>
    public sealed class UserAccount
    {
        /// <summary>Gets or sets the ID of the associated TravelDB profile.</summary>
        public int TravelUserId { get; set; }

        /// <summary>Gets or sets the profile associated with the account.</summary>
        public TravelUser? Profile { get; set; }

        /// <summary>Gets or sets the user name used to sign in.</summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>Gets or sets the normalized user name used for account lookups.</summary>
        public string NormalizedUserName { get; set; } = string.Empty;

        /// <summary>Gets or sets the stored password hash used to verify credentials.</summary>
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>Gets or sets the application role assigned to the account.</summary>
        public string Role { get; set; } = string.Empty;

        /// <summary>Gets or sets whether the account can sign in.</summary>
        public bool IsActive { get; set; } = true;
    }
}