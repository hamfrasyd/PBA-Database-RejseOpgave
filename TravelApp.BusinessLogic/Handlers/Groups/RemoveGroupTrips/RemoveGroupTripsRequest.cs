using System;
using System.Collections.Generic;
using System.Text;

namespace TravelApp.BusinessLogic.Handlers.Groups.RemoveGroupTrips
{
    /// <summary>Contains the trips to remove from a group.</summary>
    public class RemoveGroupTripsRequest
    {
        /// <summary>Gets or sets the ids of the trips to remove from the group.</summary>
        public List<int> ToBeRemovedTrips { get; set; } = new();
    }
}
