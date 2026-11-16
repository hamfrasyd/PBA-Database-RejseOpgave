namespace TravelApp.BusinessLogic.DTOs.Achievements
{
    /// <summary>Represents an achievement definition.</summary>
    public class AchievementDto
    {
        /// <summary>Gets or sets the identifier.</summary>
        public int Id { get; set; }
        /// <summary>Gets or sets the name.</summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>Gets or sets the description.</summary>
        public string Description { get; set; } = string.Empty;
    }
}
