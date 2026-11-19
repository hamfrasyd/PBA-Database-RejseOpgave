namespace TravelApp.BusinessLogic.Enums
{
    /// <summary>Stores the invitation status text saved in the database and returned by the API.</summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class InvitationStatusNameAttribute : Attribute
    {
        /// <summary>Gets the invitation status name.</summary>
        public string Name { get; }

        /// <summary>Initializes the status name annotation.</summary>
        /// <param name="name">The status text.</param>
        public InvitationStatusNameAttribute(string name)
        {
            Name = name;
        }
    }
}
