using TravelApp.BusinessLogic.Models.Users;

namespace TravelApp.BusinessLogic.Models
{
    public class GroupMembership
    {
        // Fields
        private User? _user;
        private TravelGroup? _group;

        // Properties
        public User? User
        {
            get { return _user; }
            set { _user = value; }
        }

        public TravelGroup? Group
        {
            get { return _group; }
            set { _group = value; }
        }

        // Constructor
        public GroupMembership()
        {
        }
    }
}
