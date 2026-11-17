namespace TravelApp.BusinessLogic.Models.Invitations
{
    /// <summary>Represents one row from vw_PendingInvitations.</summary>
    public class PendingInvitationView
    {
        // Fields
        private int _inviteId;
        private int _tripId;
        private DateTime? _tripStartDate;
        private string _senderName;
        private string _receiverName;
        private DateTime _sendDate;
        private DateTime _expiresAt;
        private int _daysUntilExpiry;

        /// <summary>Gets or sets the invitation identifier.</summary>
        public int InviteId
        {
            get { return _inviteId; }
            set { _inviteId = value; }
        }

        /// <summary>Gets or sets the trip identifier.</summary>
        public int TripId
        {
            get { return _tripId; }
            set { _tripId = value; }
        }

        /// <summary>Gets or sets the trip start date.</summary>
        public DateTime? TripStartDate
        {
            get { return _tripStartDate; }
            set { _tripStartDate = value; }
        }

        /// <summary>Gets or sets the sender's full name.</summary>
        public string SenderName
        {
            get { return _senderName; }
            set { _senderName = value; }
        }

        /// <summary>Gets or sets the receiver's full name.</summary>
        public string ReceiverName
        {
            get { return _receiverName; }
            set { _receiverName = value; }
        }

        /// <summary>Gets or sets the date the invitation was sent.</summary>
        public DateTime SendDate
        {
            get { return _sendDate; }
            set { _sendDate = value; }
        }

        /// <summary>Gets or sets the date the invitation expires.</summary>
        public DateTime ExpiresAt
        {
            get { return _expiresAt; }
            set { _expiresAt = value; }
        }

        /// <summary>Gets or sets the number of days until the invitation expires.</summary>
        public int DaysUntilExpiry
        {
            get { return _daysUntilExpiry; }
            set { _daysUntilExpiry = value; }
        }

        /// <summary>Initializes a new instance of the <see cref="PendingInvitationView"/> class.</summary>
        public PendingInvitationView()
        {
            _senderName = string.Empty;
            _receiverName = string.Empty;
        }
    }
}
