using TravelApp.BusinessLogic.Models.Users;

namespace TravelApp.BusinessLogic.Models
{
    public class Friendship
    {
        // Fields
        private int _id;
        private DateTime _startDate;
        private User? _userA;
        private User? _userB;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        public User? UserA
        {
            get { return _userA; }
            set { _userA = value; }
        }

        public User? UserB
        {
            get { return _userB; }
            set { _userB = value; }
        }

        // Constructor
        public Friendship()
        {
        }
    }
}
