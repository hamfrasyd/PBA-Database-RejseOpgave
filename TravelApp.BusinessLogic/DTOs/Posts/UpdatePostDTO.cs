namespace TravelApp.BusinessLogic.DTOs.Posts
{
    /// <summary>Contains the post fields that can be updated.</summary>
    public class UpdatePostDTO
    {
        /// <summary>Gets or sets the post identifier.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the post content. Null leaves the stored content unchanged.</summary>
        public string? Content { get; set; }
    }
}
