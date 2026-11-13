using System.ComponentModel.DataAnnotations;

namespace TravelApp.BusinessLogic.Handlers.Accounts
{
    /// <summary>Selects an existing user for a local demonstration session.</summary>
    public class LoginRequest
    {
        /// <summary>Gets or sets the user id.</summary>
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }
    }
}
