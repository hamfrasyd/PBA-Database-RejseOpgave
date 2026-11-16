namespace TravelApp.BusinessLogic.Models.Achievements
{
    /// <summary>Represents an achievement a user can earn.</summary>
    public class Achievement
    {
        // Fields
        private int _id;
        private string _name;
        private string _description;


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
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>Initializes a new instance of the <see cref="Achievement"/> class.</summary>
        public Achievement()
        {
            _name = string.Empty;
            _description = string.Empty;
        }
    }
}
