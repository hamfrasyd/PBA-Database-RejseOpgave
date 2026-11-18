namespace TravelApp.BusinessLogic.DTOs.Accounts
{
    /// <summary>Contains the credentials needed to create a local login account.</summary>
    public sealed class CreateUserAccountDto
    {
        /// <summary>Gets or sets the user name for the new account.</summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>Gets or sets the password to hash before storage.</summary>
        public string Password { get; set; } = string.Empty;
    }
}