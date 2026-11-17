namespace TravelApp.BusinessLogic.Models.Invitations
{
    /// <summary>Represents the status of an invitation.</summary>
    public class InvitationStatus
    {
        // Fields
        private int _id;
        private string _statusName;

        /// <summary>Gets or sets the identifier.</summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        // Properties
        /// <summary>Gets or sets the status name.</summary>
        public string StatusName
        {
            get { return _statusName; }
            set { _statusName = value; }
        }

        /// <summary>Initializes a new instance of the <see cref="InvitationStatus"/> class.</summary>
        public InvitationStatus()
        {
            _statusName = string.Empty;
        }
    }
}
