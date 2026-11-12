namespace TravelApp.BusinessLogic.Models
{
    public class Trip
    {
        // Fields
        private int _id;
        private DateTime? _startDate;
        private DateTime? _endDate;
        private bool _isCompleted;
        private string? _experienceNotes;
        private string? _planNotes;
        private List<TripStop> _tripStops;
        private List<TripActivity> _tripActivities;
        private List<TripParticipant> _tripParticipants;
        private List<Review> _reviews;
        private List<Invite> _invites;

        // Properties
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public DateTime? StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        public DateTime? EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }

        public bool IsCompleted
        {
            get { return _isCompleted; }
            set { _isCompleted = value; }
        }

        public string? ExperienceNotes
        {
            get { return _experienceNotes; }
            set { _experienceNotes = value; }
        }

        public string? PlanNotes
        {
            get { return _planNotes; }
            set { _planNotes = value; }
        }

        public List<TripStop> TripStops
        {
            get { return _tripStops; }
            set { _tripStops = value; }
        }

        public List<TripActivity> TripActivities
        {
            get { return _tripActivities; }
            set { _tripActivities = value; }
        }

        public List<TripParticipant> TripParticipants
        {
            get { return _tripParticipants; }
            set { _tripParticipants = value; }
        }

        public List<Review> Reviews
        {
            get { return _reviews; }
            set { _reviews = value; }
        }

        public List<Invite> Invites
        {
            get { return _invites; }
            set { _invites = value; }
        }

        // Constructor
        public Trip()
        {
            _tripStops = new List<TripStop>();
            _tripActivities = new List<TripActivity>();
            _tripParticipants = new List<TripParticipant>();
            _reviews = new List<Review>();
            _invites = new List<Invite>();
        }
    }
}
