namespace TravelApp.BusinessLogic.Handlers.Posts
{
    ///<summary>Represents the response after creating a post.</summary>
    public class CreatePostResponse
    {
        /// <summary>Gets or sets the post id.</summary>
        public int Id { get; set; }
        /// <summary>Get pr sets the content of the post.</summary>
        public string Content { get; set; } = string.Empty;
        /// <summary>Get or sets the userid of the post.</summary>
        public int UserId { get; set; }
    }
}
