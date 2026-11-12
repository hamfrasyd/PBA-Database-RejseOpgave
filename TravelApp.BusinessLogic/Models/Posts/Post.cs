namespace TravelApp.BusinessLogic.Models
{
    public class Post
    {
        // Fields
        private int _id;
        private string _content;
        private User? _user;

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

        public User? User
        {
            get { return _user; }
            set { _user = value; }
        }

        // Constructor
        public Post()
        {
            _content = string.Empty;
        }
    }
}
