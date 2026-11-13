namespace TravelApp.BusinessLogic.Handlers.Achievements
{
    /// <summary>Contains the result of get earned achievements.</summary>
    public class GetEarnedAchievementsResponse
    {
        /// <summary>Gets or sets the achievement id.</summary>
        public int AchievementId { get; set; }
        /// <summary>Gets or sets the name.</summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>Gets or sets the description.</summary>
        public string Description { get; set; } = string.Empty;
        /// <summary>Gets or sets the earned at.</summary>
        public DateTime EarnedAt { get; set; }
    }
}
