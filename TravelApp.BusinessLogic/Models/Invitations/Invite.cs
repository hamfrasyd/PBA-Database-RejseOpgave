
using TravelApp.BusinessLogic.Models.Trips;
using TravelApp.BusinessLogic.Models.Users;

namespace TravelApp.BusinessLogic.Models.Invitations
{
    /// <summary>Represents an invitation to a trip.</summary>
    public class Invite
    {
        // Fields
        private int _id;
        private DateTime _sendDate;
        private Trip? _trip;
        private TravelUser? _receiver;
        private TravelUser? _sender;
        private bool? _isAccepted;

        // Properties
        /// <summary>Gets or sets the identifier.</summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>Gets or sets the date the invitation was sent.</summary>
        public DateTime SendDate
        {
            get { return _sendDate; }
            set { _sendDate = value; }
        }

        /// <summary>Gets or sets the trip.</summary>
        public Trip? Trip
        {
            get { return _trip; }
            set { _trip = value; }
        }

        /// <summary>Gets or sets the user who receives the invitation.</summary>
        public TravelUser? Receiver
        {
            get { return _receiver; }
            set { _receiver = value; }
        }

        /// <summary>Gets or sets the user who sent the invitation.</summary>
        public TravelUser? Sender
        {
            get { return _sender; }
            set { _sender = value; }
        }

        /// <summary>
        /// Gets or sets the invitation response.
        /// Null = Pending, true = Accepted, false = Declined.
        /// </summary>
        public bool? IsAccepted
        {
            get { return _isAccepted; }
            set { _isAccepted = value; }
        }
    }
}
