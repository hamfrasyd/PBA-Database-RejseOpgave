namespace TravelApp.BusinessLogic.DTOs.Users
{
    /// <summary>Contains the search, sort, and privacy options used when listing user profiles.</summary>
    public class UserProfileSearchDTO
    {
        /// <summary>Gets or sets the name text to search for. Whitespace means no name search.</summary>
        public string? Search { get; set; }

        /// <summary>Gets or sets whether matching profiles are ordered by user identifier descending.</summary>
        public bool Descending { get; set; }

        /// <summary>Gets or sets the identifier of the user who is searching.</summary>
        public int CurrentUserId { get; set; }

        /// <summary>Gets or sets whether private profiles are included in the search.</summary>
        public bool IncludePrivateProfiles { get; set; }
    }
}
