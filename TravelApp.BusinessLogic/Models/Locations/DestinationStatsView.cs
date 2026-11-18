namespace TravelApp.BusinessLogic.Models.Locations
{
    /// <summary>Represents one row from vw_DestinationStats.</summary>
    public class DestinationStatsView
    {
        // Fields
        private int _cityId;
        private string _city;
        private string _country;
        private string _continent;
        private int _tripCount;
        private int _completedTripCount;
        private int _activityCount;
        private decimal? _avgStars;

        /// <summary>Gets or sets the city identifier.</summary>
        public int CityId
        {
            get { return _cityId; }
            set { _cityId = value; }
        }

        /// <summary>Gets or sets the city name.</summary>
        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        /// <summary>Gets or sets the country name.</summary>
        public string Country
        {
            get { return _country; }
            set { _country = value; }
        }

        /// <summary>Gets or sets the continent name.</summary>
        public string Continent
        {
            get { return _continent; }
            set { _continent = value; }
        }

        /// <summary>Gets or sets the trip count.</summary>
        public int TripCount
        {
            get { return _tripCount; }
            set { _tripCount = value; }
        }

        /// <summary>Gets or sets the completed trip count.</summary>
        public int CompletedTripCount
        {
            get { return _completedTripCount; }
            set { _completedTripCount = value; }
        }

        /// <summary>Gets or sets the activity count.</summary>
        public int ActivityCount
        {
            get { return _activityCount; }
            set { _activityCount = value; }
        }

        /// <summary>Gets or sets the average review stars.</summary>
        public decimal? AvgStars
        {
            get { return _avgStars; }
            set { _avgStars = value; }
        }

        /// <summary>Initializes a new instance of the <see cref="DestinationStatsView"/> class.</summary>
        public DestinationStatsView()
        {
            _city = string.Empty;
            _country = string.Empty;
            _continent = string.Empty;
        }
    }
}
