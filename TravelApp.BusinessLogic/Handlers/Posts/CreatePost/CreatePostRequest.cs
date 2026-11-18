using System.ComponentModel.DataAnnotations;

namespace TravelApp.BusinessLogic.Handlers.Posts.CreatePost
{
    /// <summary>Contains the data required to create a new post.</summary>
    public class CreatePostRequest
    {
        /// <summary>Gets or sets the content.</summary>
        [Required, StringLength(2048)]
        ///<summary>Gets or sets the content of the post.</summary>
        public string Content { get; set; } = string.Empty;
        ///<summary>Gets or sets the user id.</summary>
        [Required]
        public int UserId { get; set; }
    }
}