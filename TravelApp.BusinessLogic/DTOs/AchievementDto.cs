using System;
using System.Collections.Generic;
using System.Text;

namespace TravelApp.BusinessLogic.DTOs
{
    public class AchievementDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
