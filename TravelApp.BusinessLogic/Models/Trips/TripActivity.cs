namespace TravelApp.BusinessLogic.Models
{
    public class TripActivity
    {
        // Fields
        private Trip? _trip;
        private Activity? _activity;

        // Properties
        public Trip? Trip
        {
            get { return _trip; }
            set { _trip = value; }
        }

        public Activity? Activity
        {
            get { return _activity; }
            set { _activity = value; }
        }

        // Constructor
        public TripActivity()
        {
        }
    }
}
