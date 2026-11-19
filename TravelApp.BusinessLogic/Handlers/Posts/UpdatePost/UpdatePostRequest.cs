namespace TravelApp.BusinessLogic.Handlers.Posts.UpdatePost
{
    ///<summary>Represents the request to update a post.</summary>
    public class UpdatePostRequest
    {
        /// <summary>Gets or sets the post id.</summary>
        public int Id { get; set; }
        /// <summary>Gets or sets the content.</summary>
        public string? Content { get; set; }
    }
}
