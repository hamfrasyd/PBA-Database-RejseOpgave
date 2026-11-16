using TravelApp.BusinessLogic.Models.Users;

namespace TravelApp.BusinessLogic.Models.Friendships
{
    /// <summary>Represents a friendship between two users.</summary>
    public class Friendship
    {
        // Fields
        private int _id;
        private DateTime _startDate;
        private TravelUser? _userA;
        private TravelUser? _userB;

        /// <summary>Gets or sets the identifier.</summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>Gets or sets the date the friendship started.</summary>
        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        /// <summary>Gets or sets the first user in the friendship.</summary>
        public TravelUser? UserA
        {
            get { return _userA; }
            set { _userA = value; }
        }

        /// <summary>Gets or sets the second user in the friendship.</summary>
        public TravelUser? UserB
        {
            get { return _userB; }
            set { _userB = value; }
        }

        /// <summary>Initializes a new instance of the <see cref="Friendship"/> class.</summary>
        public Friendship()
        {
        }
    }
}
