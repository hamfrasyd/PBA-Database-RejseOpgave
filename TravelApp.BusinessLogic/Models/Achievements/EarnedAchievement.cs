namespace TravelApp.BusinessLogic.Models
{
    public class EarnedAchievement
    {
        // Fields
        private DateTime _earnedAt;
        private User? _user;
        private Achievement? _achievement;

        // Properties
        public DateTime EarnedAt
        {
            get { return _earnedAt; }
            set { _earnedAt = value; }
        }

        public User? User
        {
            get { return _user; }
            set { _user = value; }
        }

        public Achievement? Achievement
        {
            get { return _achievement; }
            set { _achievement = value; }
        }

        // Constructor
        public EarnedAchievement()
        {
        }
    }
}
