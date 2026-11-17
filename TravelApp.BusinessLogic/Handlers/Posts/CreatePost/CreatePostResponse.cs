using System;
using System.Collections.Generic;
using System.Text;

namespace TravelApp.BusinessLogic.Handlers.Posts
{
    public class CreatePostResponse
    {
        /// <summary>Gets or sets the post id.</summary>
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public int UserId { get; set; }
    }
}
