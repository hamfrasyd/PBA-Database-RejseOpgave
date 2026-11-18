namespace TravelApp.BusinessLogic.Handlers.Groups.CreateGroup
{
    ///<summary>Response model for the creation of a new travel group.</summary>
    public class CreateGroupResponse
    {
        ///<summary>The unique identifier of the newly created travel group.</summary>
        public int Id { get; set; }
        ///<summary>The name of the newly created travel group.</summary>
        public string Name { get; set; }
        ///<summary>An optional description of the newly created travel group.</summary>
        public string? Description { get; set; }
    }
}
