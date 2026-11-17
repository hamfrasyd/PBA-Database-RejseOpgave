namespace TravelApp.BusinessLogic.Handlers.Locations.Data
{
    /// <summary>Contains destination statistics for a city read from a repository.</summary>
    public class CityStatsData
    {
        /// <summary>Gets or sets the city identifier.</summary>
        public int CityId { get; set; }

        /// <summary>Gets or sets the city name.</summary>
        public string City { get; set; } = string.Empty;

        /// <summary>Gets or sets the country name.</summary>
        public string Country { get; set; } = string.Empty;

        /// <summary>Gets or sets the continent name.</summary>
        public string Continent { get; set; } = string.Empty;

        /// <summary>Gets or sets the trip count.</summary>
        public int TripCount { get; set; }

        /// <summary>Gets or sets the completed trip count.</summary>
        public int CompletedTripCount { get; set; }

        /// <summary>Gets or sets the activity count.</summary>
        public int ActivityCount { get; set; }

        /// <summary>Gets or sets the average review stars.</summary>
        public decimal? AvgStars { get; set; }
    }
}
