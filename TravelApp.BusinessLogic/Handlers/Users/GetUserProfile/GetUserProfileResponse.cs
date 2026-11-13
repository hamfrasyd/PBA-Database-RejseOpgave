namespace TravelApp.BusinessLogic.Handlers.Users
{
    /// <summary>Contains the result of get user profile.</summary>
    public class GetUserProfileResponse
    {
        /// <summary>Gets or sets the user id.</summary>
        public int UserId { get; set; }
        /// <summary>Gets or sets the full name.</summary>
        public string FullName { get; set; } = string.Empty;
        /// <summary>Gets or sets the age.</summary>
        public int Age { get; set; }
        /// <summary>Gets or sets the home city.</summary>
        public string HomeCity { get; set; } = string.Empty;
        /// <summary>Gets or sets the home country.</summary>
        public string HomeCountry { get; set; } = string.Empty;
        /// <summary>Gets or sets whether is private.</summary>
        public bool IsPrivate { get; set; }
        /// <summary>Gets or sets the member since.</summary>
        public DateTime MemberSince { get; set; }
        /// <summary>Gets or sets the friend count.</summary>
        public int FriendCount { get; set; }
        /// <summary>Gets or sets the completed trips.</summary>
        public int CompletedTrips { get; set; }
        /// <summary>Gets or sets the planned trips.</summary>
        public int PlannedTrips { get; set; }
        /// <summary>Gets or sets the review count.</summary>
        public int ReviewCount { get; set; }
        /// <summary>Gets or sets the achievement count.</summary>
        public int AchievementCount { get; set; }
    }
}
