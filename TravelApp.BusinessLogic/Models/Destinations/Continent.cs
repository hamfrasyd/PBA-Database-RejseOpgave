namespace TravelApp.BusinessLogic.Models
{
    public class Continent
    {
        // Fields
        private int _id;
        private string _name;
        private List<Country> _countries;

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

        public List<Country> Countries
        {
            get { return _countries; }
            set { _countries = value; }
        }

        // Constructor
        public Continent()
        {
            _name = string.Empty;
            _countries = new List<Country>();
        }
    }
}
