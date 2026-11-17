using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TravelApp.BusinessLogic.Handlers.Posts
{
    public class CreatePostRequest
    {
        /// <summary>Gets or sets the content.</summary>
        [Required, StringLength(2048)]
        public string Content { get; set; } = string.Empty;

        ///<summary>Gets or sets the user id.</summary>
        [Required]
        public int UserId { get; set; }
    }
}