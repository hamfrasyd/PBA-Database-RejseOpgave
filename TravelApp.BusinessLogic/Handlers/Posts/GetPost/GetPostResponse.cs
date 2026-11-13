using System;
using System.Collections.Generic;
using System.Text;

namespace TravelApp.BusinessLogic.Handlers.Posts
{
    /// <summary>The response for get post</summary>
    public class GetPostResponse
    {
        /// <summary>The Id of the post</summary>
        public int Id { get; set; }
        /// <summary>The id of the user that wrote the post</summary>
        public int? UserId { get; set; }
        /// <summary>The content of the post</summary>
        public string? Content { get; set; } = string.Empty;
    }
}
