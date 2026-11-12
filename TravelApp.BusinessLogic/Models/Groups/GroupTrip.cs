namespace TravelApp.BusinessLogic.Models
{
    public class GroupTrip
    {
        // Fields
        private Trip? _trip;
        private TravelGroup? _group;

        // Properties
        public Trip? Trip
        {
            get { return _trip; }
            set { _trip = value; }
        }

        public TravelGroup? Group
        {
            get { return _group; }
            set { _group = value; }
        }

        // Constructor
        public GroupTrip()
        {
        }
    }
}
