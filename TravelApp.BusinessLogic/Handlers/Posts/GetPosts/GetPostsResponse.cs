namespace TravelApp.BusinessLogic.Handlers.Posts.GetPosts
{
    /// <summary>The response for get posts</summary>
    public class GetPostsResponse
    {
        /// <summary>The Id of the post</summary>
        public int Id { get; set; }
        /// <summary>The id of the user that wrote the post</summary>
        public int UserId { get; set; }
        /// <summary>The content of the post</summary>
        public string Content { get; set; } = string.Empty;

    }
}
