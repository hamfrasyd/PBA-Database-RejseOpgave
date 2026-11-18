using System.ComponentModel.DataAnnotations;

namespace TravelApp.BusinessLogic.Handlers.Invitations.RespondInvitation
{
    /// <summary>Contains the decision to accept or decline an invitation.</summary>
    public class RespondInvitationRequest
    {
        /// <summary>Gets or sets the decision, either Accepted or Declined.</summary>
        [Required]
        public string Decision { get; set; } = string.Empty;
    }
}
