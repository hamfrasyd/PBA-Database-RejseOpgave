namespace TravelApp.BusinessLogic.Models
{
    public class TripStop
    {
        // Fields
        private Trip? _trip;
        private City? _city;

        // Properties
        public Trip? Trip
        {
            get { return _trip; }
            set { _trip = value; }
        }

        public City? City
        {
            get { return _city; }
            set { _city = value; }
        }

        // Constructor
        public TripStop()
        {
        }
    }
}
