namespace TravelApp.BusinessLogic.Models
{
    public class City
    {
        // Fields
        private int _id;
        private string _name;
        private Country _country;
        private List<Activity> _activities;
        
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

        public Country Country
        {
            get { return _country; }
            set { _country = value; }
        }

        public List<Activity> Activities
        {
            get { return _activities; }
            set { _activities = value; }
        }

        // Constructor
        public City()
        {
            _name = string.Empty;
            _activities = new List<Activity>();
        }
    }
}
