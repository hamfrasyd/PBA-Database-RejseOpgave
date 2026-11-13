namespace TravelApp.BusinessLogic.Handlers.Users
{
    /// <summary>Contains the information needed to get user profiles.</summary>
    public class GetUserProfilesRequest
    {
        /// <summary>Gets or sets the search.</summary>
        public string? Search { get; set; }
        /// <summary>Gets or sets whether descending.</summary>
        public bool Descending { get; set; }
    }
}
