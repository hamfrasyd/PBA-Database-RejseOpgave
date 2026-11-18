using System;
using System.Collections.Generic;
using System.Text;

namespace TravelApp.BusinessLogic.DTOs.Groups
{
    /// <summary>Read model for a travel group, including its members and trips.</summary>
    public class TravelGroupDTO
    {
        /// <summary>The unique identifier of the travel group.</summary>
        public int Id { get; set; }

        /// <summary>The title of the travel group.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>An optional description of the travel group.</summary>
        public string? Description { get; set; }

        /// <summary>The users who are members of the group.</summary>
        public List<GroupMemberDto> Members { get; set; } = new();

        /// <summary>The trips that belong to the group.</summary>
        public List<GroupTripDto> Trips { get; set; } = new();
    }

    /// <summary>A member of a travel group.</summary>
    public class GroupMemberDto
    {
        /// <summary>The unique identifier of the user.</summary>
        public int UserId { get; set; }

        /// <summary>The display name of the user.</summary>
        public string Name { get; set; } = string.Empty;
    }

    /// <summary>A trip that belongs to a travel group.</summary>
    public class GroupTripDto
    {
        /// <summary>The unique identifier of the trip.</summary>
        public int TripId { get; set; }

        /// <summary>The name of the trip.</summary>
        public string Name { get; set; } = string.Empty;
    }
}
