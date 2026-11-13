using TravelApp.BusinessLogic.Models;

namespace TravelApp.BusinessLogic.Handlers.Destinations
{
    /// <summary>Contains the result of get destination stats.</summary>
    public class GetDestinationStatsResponse
    {
        /// <summary>Gets or sets the city id.</summary>
        public int CityId { get; set; }
        /// <summary>Gets or sets the city.</summary>
        public string City { get; set; } = string.Empty;
        /// <summary>Gets or sets the country.</summary>
        public string Country { get; set; } = string.Empty;
        /// <summary>Gets or sets the continent.</summary>
        public string Continent { get; set; } = string.Empty;
        /// <summary>Gets or sets the trip count.</summary>
        public int TripCount { get; set; }
        /// <summary>Gets or sets the completed trip count.</summary>
        public int CompletedTripCount { get; set; }
        /// <summary>Gets or sets the activity count.</summary>
        public int ActivityCount { get; set; }
        /// <summary>Gets or sets the avg stars.</summary>
        public decimal? AvgStars { get; set; }
    }
}
