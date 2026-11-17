namespace TravelApp.BusinessLogic.Models.Users
{
    /// <summary>Represents one row from vw_UserProfile.</summary>
    public class UserProfileView
    {
        // Fields
        private int _userId;
        private string _fullName;
        private int _age;
        private string _homeCity;
        private string _homeCountry;
        private bool _isPrivate;
        private DateTime _memberSince;
        private int _friendCount;
        private int _completedTrips;
        private int _plannedTrips;
        private int _reviewCount;
        private int _achievementCount;

        /// <summary>Gets or sets the user identifier.</summary>
        public int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        /// <summary>Gets or sets the full name.</summary>
        public string FullName
        {
            get { return _fullName; }
            set { _fullName = value; }
        }

        /// <summary>Gets or sets the age.</summary>
        public int Age
        {
            get { return _age; }
            set { _age = value; }
        }

        /// <summary>Gets or sets the home city.</summary>
        public string HomeCity
        {
            get { return _homeCity; }
            set { _homeCity = value; }
        }

        /// <summary>Gets or sets the home country.</summary>
        public string HomeCountry
        {
            get { return _homeCountry; }
            set { _homeCountry = value; }
        }

        /// <summary>Gets or sets a value indicating whether the profile is private.</summary>
        public bool IsPrivate
        {
            get { return _isPrivate; }
            set { _isPrivate = value; }
        }

        /// <summary>Gets or sets the date the user became a member.</summary>
        public DateTime MemberSince
        {
            get { return _memberSince; }
            set { _memberSince = value; }
        }

        /// <summary>Gets or sets the friend count.</summary>
        public int FriendCount
        {
            get { return _friendCount; }
            set { _friendCount = value; }
        }

        /// <summary>Gets or sets the number of completed trips.</summary>
        public int CompletedTrips
        {
            get { return _completedTrips; }
            set { _completedTrips = value; }
        }

        /// <summary>Gets or sets the number of planned trips.</summary>
        public int PlannedTrips
        {
            get { return _plannedTrips; }
            set { _plannedTrips = value; }
        }

        /// <summary>Gets or sets the review count.</summary>
        public int ReviewCount
        {
            get { return _reviewCount; }
            set { _reviewCount = value; }
        }

        /// <summary>Gets or sets the achievement count.</summary>
        public int AchievementCount
        {
            get { return _achievementCount; }
            set { _achievementCount = value; }
        }

        /// <summary>Initializes a new instance of the <see cref="UserProfileView"/> class.</summary>
        public UserProfileView()
        {
            _fullName = string.Empty;
            _homeCity = string.Empty;
            _homeCountry = string.Empty;
        }
    }
}
