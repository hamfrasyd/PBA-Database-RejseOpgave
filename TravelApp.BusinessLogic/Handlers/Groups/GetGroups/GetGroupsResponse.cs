namespace TravelApp.BusinessLogic.Handlers.Groups
{
    /// <summary>The response for get groups.</summary>
    public class GetGroupsResponse
    {
        /// <summary>The unique identifier of the travel group.</summary>
        public int Id { get; set; }

        /// <summary>The name of the travel group.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>An optional description of the travel group.</summary>
        public string? Description { get; set; }

        /// <summary>The users who are members of the group.</summary>
        public List<GetGroupsMember> Members { get; set; } = new();

        /// <summary>The trips that belong to the group.</summary>
        public List<GetGroupsTrip> Trips { get; set; } = new();
    }

    /// <summary>A member of a travel group in the get groups response.</summary>
    public class GetGroupsMember
    {
        /// <summary>The unique identifier of the user.</summary>
        public int UserId { get; set; }

        /// <summary>The display name of the user.</summary>
        public string Name { get; set; } = string.Empty;
    }

    /// <summary>A trip that belongs to a travel group in the get groups response.</summary>
    public class GetGroupsTrip
    {
        /// <summary>The unique identifier of the trip.</summary>
        public int TripId { get; set; }

        /// <summary>The name of the trip.</summary>
        public string Name { get; set; } = string.Empty;
    }
}