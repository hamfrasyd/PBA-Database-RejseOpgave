using System;
using System.Collections.Generic;
using System.Text;

namespace TravelApp.BusinessLogic.Handlers.Groups.AddGroupMembers
{
    ///<summary>Request model for adding members to a travel group.</summary>
    public class AddGroupMembersRequest
    {
        ///<summary>Members to be added to a travel group</summary>
        public List<int>? ToBeAddedMembers { get; set; }
    }
}
