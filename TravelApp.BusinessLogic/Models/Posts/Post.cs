using TravelApp.BusinessLogic.Models.Users;

namespace TravelApp.BusinessLogic.Models.Posts
{
    /// <summary>Represents a post written by a user.</summary>
    public class Post
    {
        // Fields
        private int _id;
        private string _content;
        private TravelUser? _user;

        // Properties
        /// <summary>Gets or sets the identifier.</summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>Gets or sets the post text.</summary>
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        /// <summary>Gets or sets the user who wrote the post.</summary>
        public TravelUser? User
        {
            get { return _user; }
            set { _user = value; }
        }

        /// <summary>Initializes a new instance of the <see cref="Post"/> class.</summary>
        public Post()
        {
            _content = string.Empty;
        }
    }
}
