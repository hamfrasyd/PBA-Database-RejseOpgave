using TravelApp.BusinessLogic.Models.Users;

namespace TravelApp.BusinessLogic.Models.Achievements
{
    /// <summary>Represents an achievement earned by a user.</summary>
    public class EarnedAchievement
    {
        // Fields
        private DateTime _earnedAt;
        private TravelUser? _user;
        private Achievement? _achievement;

        // Properties
        /// <summary>Gets or sets the time the achievement was earned.</summary>
        public DateTime EarnedAt
        {
            get { return _earnedAt; }
            set { _earnedAt = value; }
        }

        /// <summary>Gets or sets the user who earned the achievement.</summary>
        public TravelUser? User
        {
            get { return _user; }
            set { _user = value; }
        }

        /// <summary>Gets or sets the earned achievement definition.</summary>
        public Achievement? Achievement
        {
            get { return _achievement; }
            set { _achievement = value; }
        }

        /// <summary>Initializes a new instance of the <see cref="EarnedAchievement"/> class.</summary>
        public EarnedAchievement()
        {
        }
    }
}
