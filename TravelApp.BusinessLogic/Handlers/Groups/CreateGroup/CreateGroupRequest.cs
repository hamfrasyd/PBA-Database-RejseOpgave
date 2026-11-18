using System;
using System.Collections.Generic;
using System.Text;

namespace TravelApp.BusinessLogic.Handlers.Groups.CreateGroup
{
    ///<summary>Request model for creating a new travel group.</summary>
    public class CreateGroupRequest
    {
        ///<summary>The name of the travel group to be created.</summary>
        public string? Name { get; set; }
        ///<summary>An optional description of the travel group to be created.</summary>
        public string? Description { get; set; }
    }
}
