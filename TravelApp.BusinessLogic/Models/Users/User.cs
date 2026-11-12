namespace TravelApp.BusinessLogic.Models
{
    public class User
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
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public DateOnly Birthday
        {
            get { return _birthday; }
            set { _birthday = value; }
        }

        public bool IsPrivate
        {
            get { return _isPrivate; }
            set { _isPrivate = value; }
        }

        public DateTime CreatedAt
        {
            get { return _createdAt; }
            set { _createdAt = value; }
        }

        public City? City
        {
            get { return _city; }
            set { _city = value; }
        }

        public List<TripParticipant> TripParticipants
        {
            get { return _tripParticipants; }
            set { _tripParticipants = value; }
        }

        public List<Review> Reviews
        {
            get { return _reviews; }
            set { _reviews = value; }
        }

        public List<Post> Posts
        {
            get { return _posts; }
            set { _posts = value; }
        }

        public List<EarnedAchievement> EarnedAchievements
        {
            get { return _earnedAchievements; }
            set { _earnedAchievements = value; }
        }

        public List<Invite> ReceivedInvites
        {
            get { return _receivedInvites; }
            set { _receivedInvites = value; }
        }

        public List<Invite> SentInvites
        {
            get { return _sentInvites; }
            set { _sentInvites = value; }
        }

        public List<GroupMembership> GroupMemberships
        {
            get { return _groupMemberships; }
            set { _groupMemberships = value; }
        }

        // Constructor
        public User()
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
