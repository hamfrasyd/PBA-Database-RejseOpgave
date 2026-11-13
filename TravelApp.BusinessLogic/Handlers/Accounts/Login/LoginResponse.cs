namespace TravelApp.BusinessLogic.Handlers.Accounts
{
    /// <summary>Contains the result of login.</summary>
    public class LoginResponse
    {
        /// <summary>Gets or sets the user id.</summary>
        public int UserId { get; set; }
        /// <summary>Gets or sets the full name.</summary>
        public string FullName { get; set; } = string.Empty;
    }
}
