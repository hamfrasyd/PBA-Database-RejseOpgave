using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TravelApp.BusinessLogic.Handlers.Groups.AddGroupMembers
{
    ///<summary>Request model for adding members to a travel group.</summary>
    public class AddGroupMembersRequest
    {
        ///<summary>Members to be added to a travel group</summary>
        [Required, MinLength(1)]
        public List<int> ToBeAddedMembers { get; set; } = new List<int>();
    }
}
