using System;
using System.Collections.Generic;
using System.Text;

namespace TravelApp.BusinessLogic.Handlers.Posts
{
    public class UpdatePostRequest
    {
        public int Id { get; set; }
        public string Content { get; set; }
    }
}
