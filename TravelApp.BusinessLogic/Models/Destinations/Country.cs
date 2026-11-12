namespace TravelApp.BusinessLogic.Models
{
    public class Country
    {
        // Fields
        private int _id;
        private string _name;
        private Continent? _continent;
        private List<City> _cities;

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

        public Continent? Continent
        {
            get { return _continent; }
            set { _continent = value; }
        }

        public List<City> Cities
        {
            get { return _cities; }
            set { _cities = value; }
        }

        // Constructor
        public Country()
        {
            _name = string.Empty;
            _cities = new List<City>();
        }
    }
}
