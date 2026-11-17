namespace TravelApp.BusinessLogic.Handlers.Accounts.Login
{
    /// <summary>Contains the result of login.</summary>
    public class LoginResponse
    {
        /// <summary>Gets or sets the JWT access token.</summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>Gets or sets the token expiration time in UTC.</summary>
        public DateTimeOffset ExpiresAtUtc { get; set; }
    }
}