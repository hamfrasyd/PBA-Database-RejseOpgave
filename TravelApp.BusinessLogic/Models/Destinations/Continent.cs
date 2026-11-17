namespace TravelApp.BusinessLogic.Models.Destinations
{
    /// <summary>Represents a continent.</summary>
    public class Continent
    {
        // Fields
        private int _id;
        private string _name;
        private List<Country> _countries;

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

        /// <summary>Gets or sets the countries on the continent.</summary>
        public List<Country> Countries
        {
            get { return _countries; }
            set { _countries = value; }
        }

        /// <summary>Initializes a new instance of the <see cref="Continent"/> class.</summary>
        public Continent()
        {
            _name = string.Empty;
            _countries = new List<Country>();
        }
    }
}
