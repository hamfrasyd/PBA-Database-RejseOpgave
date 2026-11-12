namespace TravelApp.BusinessLogic.Models
{
    public class Achievement
    {
        // Fields
        private int _id;
        private string _name;
        private string _description;


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

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        // Constructor
        public Achievement()
        {
            _name = string.Empty;
            _description = string.Empty;
        }
    }
}
