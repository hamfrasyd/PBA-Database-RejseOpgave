using TravelApp.BusinessLogic.Models.Destinations;

namespace TravelApp.BusinessLogic.Models.Trips
{
    /// <summary>Links an activity to a trip.</summary>
    public class TripActivity
    {
        // Fields
        private Trip? _trip;
        private Activity? _activity;

        // Properties
        /// <summary>Gets or sets the trip.</summary>
        public Trip? Trip
        {
            get { return _trip; }
            set { _trip = value; }
        }

        /// <summary>Gets or sets the activity.</summary>
        public Activity? Activity
        {
            get { return _activity; }
            set { _activity = value; }
        }

        /// <summary>Initializes a new instance of the <see cref="TripActivity"/> class.</summary>
        public TripActivity()
        {
        }
    }
}
