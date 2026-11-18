namespace TravelApp.BusinessLogic.Handlers.Invitations.RespondInvitation
{
    /// <summary>Contains the new status of an invitation.</summary>
    public class RespondInvitationResponse
    {
        /// <summary>Gets or sets the invitation identifier.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the invitation status.</summary>
        public string Status { get; set; } = string.Empty;
    }
}
