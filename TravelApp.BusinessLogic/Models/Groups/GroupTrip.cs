using TravelApp.BusinessLogic.Models.Trips;

namespace TravelApp.BusinessLogic.Models.Groups
{
    /// <summary>Links a trip to a travel group.</summary>
    public class GroupTrip
    {
        // Fields
        private Trip? _trip;
        private TravelGroup? _group;

        // Properties
        /// <summary>Gets or sets the trip.</summary>
        public Trip? Trip
        {
            get { return _trip; }
            set { _trip = value; }
        }

        /// <summary>Gets or sets the travel group.</summary>
        public TravelGroup? Group
        {
            get { return _group; }
            set { _group = value; }
        }

        /// <summary>Initializes a new instance of the <see cref="GroupTrip"/> class.</summary>
        public GroupTrip()
        {
        }
    }
}
