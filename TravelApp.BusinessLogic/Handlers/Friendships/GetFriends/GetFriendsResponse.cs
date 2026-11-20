namespace TravelApp.BusinessLogic.Handlers.Friendships.GetFriends
{
    /// <summary>Contains information about one of a user's friends.</summary>
    public class GetFriendsResponse
    {
        /// <summary>Gets or sets the friendship identifier.</summary>
        public int FriendshipId { get; set; }

        /// <summary>Gets or sets the friend's user identifier.</summary>
        public int UserId { get; set; }

        /// <summary>Gets or sets the friend's first name.</summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>Gets or sets the friend's last name.</summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>Gets or sets when the friendship started.</summary>
        public DateTime StartDate { get; set; }
    }
}
