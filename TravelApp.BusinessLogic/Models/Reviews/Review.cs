using TravelApp.BusinessLogic.Models.Trips;
using TravelApp.BusinessLogic.Models.Users;

namespace TravelApp.BusinessLogic.Models.Reviews
{
    /// <summary>Represents a user's review of a trip.</summary>
    public class Review
    {
        // Fields
        private int _id;
        private string _content;
        private byte _stars;
        private Trip? _trip;
        private TravelUser? _user;

        // Properties
        /// <summary>Gets or sets the identifier.</summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>Gets or sets the review text.</summary>
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        /// <summary>Gets or sets the star rating.</summary>
        public byte Stars
        {
            get { return _stars; }
            set { _stars = value; }
        }

        /// <summary>Gets or sets the reviewed trip.</summary>
        public Trip? Trip
        {
            get { return _trip; }
            set { _trip = value; }
        }

        /// <summary>Gets or sets the user who wrote the review.</summary>
        public TravelUser? User
        {
            get { return _user; }
            set { _user = value; }
        }

        /// <summary>Initializes a new instance of the <see cref="Review"/> class.</summary>
        public Review()
        {
            _content = string.Empty;
        }
    }
}
