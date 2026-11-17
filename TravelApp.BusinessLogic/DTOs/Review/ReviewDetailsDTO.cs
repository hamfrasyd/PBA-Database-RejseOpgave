using System;
using System.Collections.Generic;
using System.Text;
using TravelApp.BusinessLogic.DTOs.Trips;
using TravelApp.BusinessLogic.Models.Trips;
using TravelApp.BusinessLogic.Models.Users;

namespace TravelApp.BusinessLogic.DTOs.Review
{
    public class ReviewDetailsDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public byte Stars { get; set; }
        public int TripId { get; set; }
        public ReviewerFullNameDTO UserName { get; set; }
    }
}
