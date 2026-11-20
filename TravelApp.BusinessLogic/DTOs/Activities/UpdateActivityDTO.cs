namespace TravelApp.BusinessLogic.DTOs.Activities
{
    /// <summary>Contains activity fields exchanged with a repository.</summary>
    public class UpdateActivityDTO
    {
        /// <summary>Gets or sets the activity identifier.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the city identifier.</summary>
        public int CityId { get; set; }

        /// <summary>Gets or sets the name.</summary>
        public string? Name { get; set; }

        /// <summary>Gets or sets the description.</summary>
        public string? Description { get; set; }
    }
}