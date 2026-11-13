namespace TravelApp.BusinessLogic.Models
{
    public class Review
    {
        // Fields
        private int _id;
        private string _content;
        private byte _stars;
        private Trip? _trip;
        private TravelUser? _user;

        // Properties
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        public byte Stars
        {
            get { return _stars; }
            set { _stars = value; }
        }

        public Trip? Trip
        {
            get { return _trip; }
            set { _trip = value; }
        }

        public TravelUser? User
        {
            get { return _user; }
            set { _user = value; }
        }

        // Constructor
        public Review()
        {
            _content = string.Empty;
        }
    }
}
