namespace TravelApp.BusinessLogic.Handlers.Achievements
{
    /// <summary>Contains the result of get achievement definitions.</summary>
    public class GetAchievementDefinitionsResponse
    {
        /// <summary>Gets or sets the entity ID.</summary>
        public int Id { get; set; }
        /// <summary>Gets or sets the name.</summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>Gets or sets the description.</summary>
        public string Description { get; set; } = string.Empty;
    }
}
