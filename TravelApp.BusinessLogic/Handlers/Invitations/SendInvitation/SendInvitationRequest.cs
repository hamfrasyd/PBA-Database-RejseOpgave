using System.ComponentModel.DataAnnotations;

namespace TravelApp.BusinessLogic.Handlers.Invitations.SendInvitation
{
    /// <summary>Contains the information needed to invite a user to a trip.</summary>
    public class SendInvitationRequest
    {
        /// <summary>Gets or sets the trip identifier.</summary>
        [Range(1, int.MaxValue)]
        public int TripId { get; set; }

        /// <summary>Gets or sets the receiver's user identifier.</summary>
        [Range(1, int.MaxValue)]
        public int ReceiverId { get; set; }
    }
}
