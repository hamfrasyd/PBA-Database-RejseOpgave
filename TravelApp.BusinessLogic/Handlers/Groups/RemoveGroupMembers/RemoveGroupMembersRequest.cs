using System;
using System.Collections.Generic;
using System.Text;

namespace TravelApp.BusinessLogic.Handlers.Groups.UpdateGroupMembers
{
    /// <summary>Contains the members to remove from a group.</summary>
    public class RemoveGroupMembersRequest
    {
        /// <summary>Gets or sets the ids of the users to remove from the group.</summary>
        public List<int> ToBeRemovedMembers { get; set; } = new();
    }
}
