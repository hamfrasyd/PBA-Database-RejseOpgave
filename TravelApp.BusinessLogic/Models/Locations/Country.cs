namespace TravelApp.BusinessLogic.Models.Locations
{
    /// <summary>Represents a country on a continent.</summary>
    public class Country
    {
        // Fields
        private int _id;
        private string _name;
        private Continent? _continent;
        private List<City> _cities;

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

        /// <summary>Gets or sets the continent the country belongs to.</summary>
        public Continent? Continent
        {
            get { return _continent; }
            set { _continent = value; }
        }

        /// <summary>Gets or sets the cities in the country.</summary>
        public List<City> Cities
        {
            get { return _cities; }
            set { _cities = value; }
        }

        /// <summary>Initializes a new instance of the <see cref="Country"/> class.</summary>
        public Country()
        {
            _name = string.Empty;
            _cities = new List<City>();
        }
    }
}
