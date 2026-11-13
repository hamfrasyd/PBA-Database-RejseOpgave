using System;
using System.Collections.Generic;
using System.Text;

namespace TravelApp.BusinessLogic.Handlers.Posts.GetPosts
{
    /// <summary>Contains the information needed to get posts.</summary>
    public class GetPostsRequest
    {
        /// <summary>Gets or sets the post id.</summary>
        public int? PostId { get; set; }
        /// <summary>Gets or sets whether descending.</summary>
        public bool Descending { get; set; }
    }
}
