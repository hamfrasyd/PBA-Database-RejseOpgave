namespace TravelApp.BusinessLogic.Models
{
    public class TravelGroup
    {
        // Fields
        private int _id;
        private string _name;
        private string? _description;
        private List<GroupMembership> _groupMemberships;
        private List<GroupTrip> _groupTrips;

        // Properties
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string? Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public List<GroupMembership> GroupMemberships
        {
            get { return _groupMemberships; }
            set { _groupMemberships = value; }
        }

        public List<GroupTrip> GroupTrips
        {
            get { return _groupTrips; }
            set { _groupTrips = value; }
        }

        // Constructor
        public TravelGroup()
        {
            _name = string.Empty;
            _groupMemberships = new List<GroupMembership>();
            _groupTrips = new List<GroupTrip>();
        }
    }
}
