namespace TravelApp.BusinessLogic.DTOs
{
    /// <summary>Data Transfer Object representing an achievement.</summary>
    public class AchievementDto
    {
        /// <summary>Gets or sets the unique identifier of the achievement.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the name of the achievement.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Gets or sets the description of the achievement.</summary>
        public string Description { get; set; } = string.Empty;
    }
}
