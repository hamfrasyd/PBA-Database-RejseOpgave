namespace TravelApp.BusinessLogic.Handlers.Destinations
{
    /// <summary>Contains the result of get continents.</summary>
    public class GetContinentsResponse
    {
        /// <summary>Gets or sets the entity ID.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the name.</summary>
        public string Name { get; set; } = string.Empty;
    }
}
