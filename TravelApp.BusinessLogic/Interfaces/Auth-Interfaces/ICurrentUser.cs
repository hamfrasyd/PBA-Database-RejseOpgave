namespace TravelApp.BusinessLogic.Auth
{
    /// <summary>Defines the icurrent user contract.</summary>
    public interface ICurrentUser
    {
        /// <summary>Gets the user id.</summary>
        public int UserId { get; }
        /// <summary>Gets whether is admin.</summary>
        public bool IsAdmin { get; }
    }
}
