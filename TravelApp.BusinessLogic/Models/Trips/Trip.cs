using TravelApp.BusinessLogic.Models.Invitations;
using TravelApp.BusinessLogic.Models.Reviews;

namespace TravelApp.BusinessLogic.Models.Trips
{
    /// <summary>Represents a trip.</summary>
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
        /// <summary>Gets or sets the identifier.</summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>Gets or sets the start date.</summary>
        public DateTime? StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        /// <summary>Gets or sets the end date.</summary>
        public DateTime? EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }

        /// <summary>Gets or sets a value indicating whether the trip is completed.</summary>
        public bool IsCompleted
        {
            get { return _isCompleted; }
            set { _isCompleted = value; }
        }

        /// <summary>Gets or sets the experience notes.</summary>
        public string? ExperienceNotes
        {
            get { return _experienceNotes; }
            set { _experienceNotes = value; }
        }

        /// <summary>Gets or sets the planning notes.</summary>
        public string? PlanNotes
        {
            get { return _planNotes; }
            set { _planNotes = value; }
        }

        /// <summary>Gets or sets the cities on the trip.</summary>
        public List<TripStop> TripStops
        {
            get { return _tripStops; }
            set { _tripStops = value; }
        }

        /// <summary>Gets or sets the activities on the trip.</summary>
        public List<TripActivity> TripActivities
        {
            get { return _tripActivities; }
            set { _tripActivities = value; }
        }

        /// <summary>Gets or sets the trip participants.</summary>
        public List<TripParticipant> TripParticipants
        {
            get { return _tripParticipants; }
            set { _tripParticipants = value; }
        }

        /// <summary>Gets or sets the reviews of the trip.</summary>
        public List<Review> Reviews
        {
            get { return _reviews; }
            set { _reviews = value; }
        }

        /// <summary>Gets or sets the invitations for the trip.</summary>
        public List<Invite> Invites
        {
            get { return _invites; }
            set { _invites = value; }
        }

        /// <summary>Initializes a new instance of the <see cref="Trip"/> class.</summary>
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
