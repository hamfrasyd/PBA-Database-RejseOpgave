namespace TravelApp.BusinessLogic.Handlers.Friendships.GetFriendship
{
    /// <summary>Contains the details of a friendship.</summary>
    public class GetFriendshipResponse
    {
        /// <summary>Gets or sets the friendship identifier.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the first user's identifier.</summary>
        public int FirstUserId { get; set; }

        /// <summary>Gets or sets the second user's identifier.</summary>
        public int SecondUserId { get; set; }

        /// <summary>Gets or sets the date the friendship began.</summary>
        public DateTime StartDate { get; set; }
    }
}
