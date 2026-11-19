using System;
using System.Collections.Generic;
using System.Text;

namespace TravelApp.BusinessLogic.DTOs.Groups
{
    ///<summary>Update model for travel group</summary>
    public class UpdateTravelGroupDTO
    {
        /// <summary>The unique identifier of the travel group.</summary>
        public int Id { get; set; }

        /// <summary>The title of the travel group.</summary>
        public string? Name { get; set; }

        /// <summary>An optional description of the travel group.</summary>
        public string? Description { get; set; }
    }
}
