using TravelApp.BusinessLogic.Models.Users;

namespace TravelApp.BusinessLogic.Models.Groups
{
    /// <summary>Links a user to a travel group.</summary>
    public class GroupMembership
    {
        // Fields
        private TravelUser? _user;
        private TravelGroup? _group;

        // Properties
        /// <summary>Gets or sets the member.</summary>
        public TravelUser? User
        {
            get { return _user; }
            set { _user = value; }
        }

        /// <summary>Gets or sets the travel group.</summary>
        public TravelGroup? Group
        {
            get { return _group; }
            set { _group = value; }
        }

        /// <summary>Initializes a new instance of the <see cref="GroupMembership"/> class.</summary>
        public GroupMembership()
        {
        }
    }
}
