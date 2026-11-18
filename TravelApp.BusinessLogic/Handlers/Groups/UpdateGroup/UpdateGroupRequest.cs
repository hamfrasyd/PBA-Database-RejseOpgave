namespace TravelApp.BusinessLogic.Handlers.Groups.UpdateGroup
{
    /// <summary>Contains the data required to update a group.</summary>
    public class UpdateGroupRequest
    {
        /// <summary>Gets or sets the unique identifier of the group.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the new name of the group.</summary>
        public string? Name { get; set; }

        /// <summary>Gets or sets the new description of the group.</summary>
        public string? Description { get; set; }
    }
}
