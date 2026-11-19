using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TravelApp.BusinessLogic.Handlers.Groups.UpdateGroupMembers
{
    /// <summary>Contains the members to remove from a group.</summary>
    public class RemoveGroupMembersRequest
    {
        /// <summary>Gets or sets the ids of the users to remove from the group.</summary>
        [Required, MinLength(1)]
        public List<int> ToBeRemovedMembers { get; set; } = new();
    }
}
