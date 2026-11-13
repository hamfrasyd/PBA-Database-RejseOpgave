namespace TravelApp.BusinessLogic.Models
{
    public class Invite
    {
        // Fields
        private int _id;
        private DateTime _sendDate;
        private Trip? _trip;
        private TravelUser? _receiver;
        private TravelUser? _sender;
        private InvitationStatus? _invitationStatus;

        // Properties
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public DateTime SendDate
        {
            get { return _sendDate; }
            set { _sendDate = value; }
        }

        public Trip? Trip
        {
            get { return _trip; }
            set { _trip = value; }
        }

        public TravelUser? Receiver
        {
            get { return _receiver; }
            set { _receiver = value; }
        }

        public TravelUser? Sender
        {
            get { return _sender; }
            set { _sender = value; }
        }

        public InvitationStatus? InvitationStatus
        {
            get { return _invitationStatus; }
            set { _invitationStatus = value; }
        }

        // Constructor
        public Invite()
        {
        }
    }
}
