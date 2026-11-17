namespace TravelApp.BusinessLogic.Models.Destinations
{
    /// <summary>Represents an activity available in a city.</summary>
    public class Activity
    {
        // Fields
        private int _id;
        private string _name;
        private string? _description;
        private City? _city;

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

        /// <summary>Gets or sets the city the activity belongs to.</summary>
        public City? City
        {
            get { return _city; }
            set { _city = value; }
        }

        /// <summary>Initializes a new instance of the <see cref="Activity"/> class.</summary>
        public Activity()
        {
            _name = string.Empty;
        }
    }
}
