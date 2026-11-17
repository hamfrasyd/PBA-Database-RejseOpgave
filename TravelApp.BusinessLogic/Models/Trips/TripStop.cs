using TravelApp.BusinessLogic.Models.Destinations;

namespace TravelApp.BusinessLogic.Models.Trips
{
    /// <summary>Links a city stop to a trip.</summary>
    public class TripStop
    {
        // Fields
        private Trip? _trip;
        private City? _city;

        // Properties
        /// <summary>Gets or sets the trip.</summary>
        public Trip? Trip
        {
            get { return _trip; }
            set { _trip = value; }
        }

        /// <summary>Gets or sets the city.</summary>
        public City? City
        {
            get { return _city; }
            set { _city = value; }
        }

        /// <summary>Initializes a new instance of the <see cref="TripStop"/> class.</summary>
        public TripStop()
        {
        }
    }
}
