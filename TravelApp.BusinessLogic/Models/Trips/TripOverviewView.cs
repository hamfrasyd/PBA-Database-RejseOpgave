namespace TravelApp.BusinessLogic.Models.Trips
{
    /// <summary>Represents one row from vw_TripOverview.</summary>
    public class TripOverviewView
    {
        // Fields
        private int _tripId;
        private DateTime? _startDate;
        private DateTime? _endDate;
        private int? _durationDays;
        private string _tripStatus;
        private string? _groupName;
        private int _participantCount;
        private string? _cities;
        private int _cityCount;
        private int _reviewCount;
        private decimal? _avgStars;

        /// <summary>Gets or sets the trip identifier.</summary>
        public int TripId
        {
            get { return _tripId; }
            set { _tripId = value; }
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

        /// <summary>Gets or sets the trip length in days.</summary>
        public int? DurationDays
        {
            get { return _durationDays; }
            set { _durationDays = value; }
        }

        /// <summary>Gets or sets the trip status text.</summary>
        public string TripStatus
        {
            get { return _tripStatus; }
            set { _tripStatus = value; }
        }

        /// <summary>Gets or sets the first group name linked to the trip.</summary>
        public string? GroupName
        {
            get { return _groupName; }
            set { _groupName = value; }
        }

        /// <summary>Gets or sets the participant count.</summary>
        public int ParticipantCount
        {
            get { return _participantCount; }
            set { _participantCount = value; }
        }

        /// <summary>Gets or sets the city names on the trip.</summary>
        public string? Cities
        {
            get { return _cities; }
            set { _cities = value; }
        }

        /// <summary>Gets or sets the city count.</summary>
        public int CityCount
        {
            get { return _cityCount; }
            set { _cityCount = value; }
        }

        /// <summary>Gets or sets the review count.</summary>
        public int ReviewCount
        {
            get { return _reviewCount; }
            set { _reviewCount = value; }
        }

        /// <summary>Gets or sets the average review stars.</summary>
        public decimal? AvgStars
        {
            get { return _avgStars; }
            set { _avgStars = value; }
        }

        /// <summary>Initializes a new instance of the <see cref="TripOverviewView"/> class.</summary>
        public TripOverviewView()
        {
            _tripStatus = string.Empty;
        }
    }
}
