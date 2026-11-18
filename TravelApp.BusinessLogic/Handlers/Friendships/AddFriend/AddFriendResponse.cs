namespace TravelApp.BusinessLogic.Handlers.Friendships.AddFriend
{
    /// <summary>Contains a newly created friendship.</summary>
    public class AddFriendResponse
    {
        /// <summary>Gets or sets the friendship identifier.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the current user's identifier.</summary>
        public int UserId { get; set; }

        /// <summary>Gets or sets the friend's identifier.</summary>
        public int FriendUserId { get; set; }

        /// <summary>Gets or sets when the friendship started.</summary>
        public DateTime StartDate { get; set; }
    }
}
