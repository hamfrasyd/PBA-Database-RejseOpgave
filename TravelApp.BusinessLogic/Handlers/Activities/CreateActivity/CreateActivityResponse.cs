namespace TravelApp.BusinessLogic.Handlers.Activities.CreateActivity
{
    /// <summary>Contains the activity that was created.</summary>
    public class CreateActivityResponse
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