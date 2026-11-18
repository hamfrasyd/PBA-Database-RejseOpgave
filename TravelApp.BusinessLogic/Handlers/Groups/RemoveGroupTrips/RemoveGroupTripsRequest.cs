using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TravelApp.BusinessLogic.Handlers.Groups.RemoveGroupTrips
{
    /// <summary>Contains the trips to remove from a group.</summary>
    public class RemoveGroupTripsRequest
    {
        /// <summary>Gets or sets the ids of the trips to remove from the group.</summary>
        [Required, MinLength(1)]
        public List<int> ToBeRemovedTrips { get; set; } = new();
    }
}
