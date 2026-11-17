using TravelApp.BusinessLogic.Models.Achievements;
using TravelApp.BusinessLogic.Models.Groups;
using TravelApp.BusinessLogic.Models.Invitations;
using TravelApp.BusinessLogic.Models.Locations;
using TravelApp.BusinessLogic.Models.Posts;
using TravelApp.BusinessLogic.Models.Reviews;
using TravelApp.BusinessLogic.Models.Trips;

namespace TravelApp.BusinessLogic.Models.Users
{
    /// <summary>Represents a user of the travel application.</summary>
    public class TravelUser
    {
        // Fields
        private int _id;
        private string _firstName;
        private string _lastName;
        private DateOnly _birthday;
        private bool _isPrivate;
        private DateTime _createdAt;
        private City? _city;
        private List<TripParticipant> _tripParticipants;
        private List<Review> _reviews;
        private List<Post> _posts;
        private List<EarnedAchievement> _earnedAchievements;
        private List<Invite> _receivedInvites;
        private List<Invite> _sentInvites;
        private List<GroupMembership> _groupMemberships;

        // Properties
        /// <summary>Gets or sets the identifier.</summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>Gets or sets the first name.</summary>
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        /// <summary>Gets or sets the last name.</summary>
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        /// <summary>Gets or sets the birthday.</summary>
        public DateOnly Birthday
        {
            get { return _birthday; }
            set { _birthday = value; }
        }

        /// <summary>Gets or sets a value indicating whether the profile is private.</summary>
        public bool IsPrivate
        {
            get { return _isPrivate; }
            set { _isPrivate = value; }
        }

        /// <summary>Gets or sets the creation time.</summary>
        public DateTime CreatedAt
        {
            get { return _createdAt; }
            set { _createdAt = value; }
        }

        /// <summary>Gets or sets the home city.</summary>
        public City? City
        {
            get { return _city; }
            set { _city = value; }
        }

        /// <summary>Gets or sets the trips the user participates in.</summary>
        public List<TripParticipant> TripParticipants
        {
            get { return _tripParticipants; }
            set { _tripParticipants = value; }
        }

        /// <summary>Gets or sets the reviews written by the user.</summary>
        public List<Review> Reviews
        {
            get { return _reviews; }
            set { _reviews = value; }
        }

        /// <summary>Gets or sets the posts written by the user.</summary>
        public List<Post> Posts
        {
            get { return _posts; }
            set { _posts = value; }
        }

        /// <summary>Gets or sets the achievements earned by the user.</summary>
        public List<EarnedAchievement> EarnedAchievements
        {
            get { return _earnedAchievements; }
            set { _earnedAchievements = value; }
        }

        /// <summary>Gets or sets the invitations received by the user.</summary>
        public List<Invite> ReceivedInvites
        {
            get { return _receivedInvites; }
            set { _receivedInvites = value; }
        }

        /// <summary>Gets or sets the invitations sent by the user.</summary>
        public List<Invite> SentInvites
        {
            get { return _sentInvites; }
            set { _sentInvites = value; }
        }

        /// <summary>Gets or sets the groups the user belongs to.</summary>
        public List<GroupMembership> GroupMemberships
        {
            get { return _groupMemberships; }
            set { _groupMemberships = value; }
        }

        /// <summary>Initializes a new instance of the <see cref="TravelUser"/> class.</summary>
        public TravelUser()
        {
            _firstName = string.Empty;
            _lastName = string.Empty;
            _tripParticipants = new List<TripParticipant>();
            _reviews = new List<Review>();
            _posts = new List<Post>();
            _earnedAchievements = new List<EarnedAchievement>();
            _receivedInvites = new List<Invite>();
            _sentInvites = new List<Invite>();
            _groupMemberships = new List<GroupMembership>();
        }
    }
}
