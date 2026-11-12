namespace TravelApp.BusinessLogic.Models
{
    public class InvitationStatus
    {
        // Fields
        private int _id;
        private string _statusName;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        // Properties
        public string StatusName
        {
            get { return _statusName; }
            set { _statusName = value; }
        }

        // Constructor
        public InvitationStatus()
        {
            _statusName = string.Empty;
        }
    }
}
