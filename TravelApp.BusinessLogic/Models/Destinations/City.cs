namespace TravelApp.BusinessLogic.Models.Destinations
{
    /// <summary>Represents a city in a country.</summary>
    public class City
    {
        // Fields
        private int _id;
        private string _name;
        private Country? _country;
        private List<Activity> _activities;
        
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

        /// <summary>Gets or sets the country the city belongs to.</summary>
        public Country? Country
        {
            get { return _country; }
            set { _country = value; }
        }

        /// <summary>Gets or sets the activities in the city.</summary>
        public List<Activity> Activities
        {
            get { return _activities; }
            set { _activities = value; }
        }

        /// <summary>Initializes a new instance of the <see cref="City"/> class.</summary>
        public City()
        {
            _name = string.Empty;
            _activities = new List<Activity>();
        }
    }
}
