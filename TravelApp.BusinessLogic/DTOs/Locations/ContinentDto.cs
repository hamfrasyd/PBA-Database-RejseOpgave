namespace TravelApp.BusinessLogic.DTOs.Locations
{
    /// <summary>Contains the continent fields exchanged with a repository.</summary>
    public class ContinentDTO
    {
        /// <summary>Gets or sets the continent identifier.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the continent name.</summary>
        public string Name { get; set; } = string.Empty;
    }
}
