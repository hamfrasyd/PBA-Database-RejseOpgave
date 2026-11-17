namespace TravelApp.BusinessLogic.DTOs.Activities
{
    /// <summary>Contains the values required to persist a new activity.</summary>
    public class CreateActivityDto
    {
        /// <summary>Gets or sets the city identifier.</summary>
        public int CityId { get; set; }

        /// <summary>Gets or sets the name.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Gets or sets the description.</summary>
        public string? Description { get; set; }
    }
}