namespace TravelApp.BusinessLogic.Models
{
    public class TripParticipant
    {
        // Fields
        private Trip? _trip;
        private User? _user;

        // Properties
        public Trip? Trip
        {
            get { return _trip; }
            set { _trip = value; }
        }

        public User? User
        {
            get { return _user; }
            set { _user = value; }
        }

        // Constructor
        public TripParticipant()
        {
        }
    }
}
