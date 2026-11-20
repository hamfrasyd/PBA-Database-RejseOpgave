namespace TravelApp.BusinessLogic.DTOs.Friendships
{
    /// <summary>Contains a friendship and basic information about the other user.</summary>
    public class FriendDTO
    {
        /// <summary>Gets or sets the friendship identifier.</summary>
        public int FriendshipId { get; set; }

        /// <summary>Gets or sets the other user's identifier.</summary>
        public int UserId { get; set; }

        /// <summary>Gets or sets the other user's first name.</summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>Gets or sets the other user's last name.</summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>Gets or sets the date the friendship began.</summary>
        public DateTime StartDate { get; set; }
    }
}
