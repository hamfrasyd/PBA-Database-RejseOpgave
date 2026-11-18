using System;
using System.Collections.Generic;
using System.Text;

namespace TravelApp.BusinessLogic.Handlers.Groups.AddGroupTrips
{
    ///<summary>Request model for adding trips to a travel group.</summary>
    public class AddGroupTripsRequest
    {
        ///<summary>¨Trips to be added to a travel group</summary>
        public List<int> ToBeAddedTrips { get; set; } = new List<int>();
    }
}
