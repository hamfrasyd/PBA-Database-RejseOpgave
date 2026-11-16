namespace TravelApp.BusinessLogic.Models.Groups
{
    /// <summary>Represents a travel group.</summary>
    public class TravelGroup
    {
        // Fields
        private int _id;
        private string _name;
        private string? _description;
        private List<GroupMembership> _groupMemberships;
        private List<GroupTrip> _groupTrips;

        // Properties
        /// <summary>Gets or sets the identifier.</summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>Gets or sets the name.</summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>Gets or sets the description.</summary>
        public string? Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>Gets or sets the group memberships.</summary>
        public List<GroupMembership> GroupMemberships
        {
            get { return _groupMemberships; }
            set { _groupMemberships = value; }
        }

        /// <summary>Gets or sets the trips linked to the group.</summary>
        public List<GroupTrip> GroupTrips
        {
            get { return _groupTrips; }
            set { _groupTrips = value; }
        }

        /// <summary>Initializes a new instance of the <see cref="TravelGroup"/> class.</summary>
        public TravelGroup()
        {
            _name = string.Empty;
            _groupMemberships = new List<GroupMembership>();
            _groupTrips = new List<GroupTrip>();
        }
    }
}
