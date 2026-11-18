using System.ComponentModel.DataAnnotations;

namespace TravelApp.BusinessLogic.Handlers.Posts
{
    /// <summary>Contains the data required to create a new post.</summary>
    public class CreatePostRequest
    {
        /// <summary>Gets or sets the content of the post.</summary>
        [Required, StringLength(2048)]
        public string Content { get; set; } = string.Empty;

        /// <summary>Gets or sets the id of the user creating the post.</summary>
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }
    }
}