using TravelApp.BusinessLogic.Models.Users;

namespace TravelApp.BusinessLogic.Models.Trips
{
    /// <summary>Links a user to a trip.</summary>
    public class TripParticipant
    {
        // Fields
        private Trip? _trip;
        private TravelUser? _user;

        // Properties
        /// <summary>Gets or sets the trip.</summary>
        public Trip? Trip
        {
            get { return _trip; }
            set { _trip = value; }
        }

        /// <summary>Gets or sets the participating user.</summary>
        public TravelUser? User
        {
            get { return _user; }
            set { _user = value; }
        }

        /// <summary>Initializes a new instance of the <see cref="TripParticipant"/> class.</summary>
        public TripParticipant()
        {
        }
    }
}
