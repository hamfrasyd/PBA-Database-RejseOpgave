namespace TravelApp.BusinessLogic.Handlers.Activities.UpdateActivity
{
    /// <summary>Contains the activity that was updated.</summary>
    public class UpdateActivityResponse
    {
        /// <summary>Gets or sets the activity identifier.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the city identifier.</summary>
        public int CityId { get; set; }

        /// <summary>Gets or sets the name.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Gets or sets the description.</summary>
        public string? Description { get; set; }
    }
}