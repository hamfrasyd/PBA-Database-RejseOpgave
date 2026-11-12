namespace TravelApp.BusinessLogic.Models
{
    public class Activity
    {
        // Fields
        private int _id;
        private string _name;
        private string? _description;
        private City? _city;

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

        public City? City
        {
            get { return _city; }
            set { _city = value; }
        }

        // Constructor
        public Activity()
        {
            _name = string.Empty;
        }
    }
}
