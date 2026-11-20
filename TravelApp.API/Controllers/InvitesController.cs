
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.API.ExceptionFilters;
using TravelApp.BusinessLogic.Handlers.Invitations.AcceptInvitation;
using TravelApp.BusinessLogic.Handlers.Invitations.GetInvitations;
using TravelApp.BusinessLogic.Handlers.Invitations.GetInvitesById;
using TravelApp.BusinessLogic.Handlers.Invitations.SendInvitation;

namespace TravelApp.API.Controllers
{
    /// <summary>Exposes HTTP endpoints for invitations.</summary>
    [ApiController]
    [Authorize]
    [Route("api/invitations")]
    public class InvitationsController : ControllerBase
    {
        #region POST - Send Invitation

        /// <summary>Sends an invitation to a user for a trip.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="userId">The receiver's identifier.</param>
        /// <param name="sendInvitationHandler">The invitation handler.</param>
        /// <returns>The created invitation.</returns>
        // POST api/trips/{tripId}/invitations/users/{userId}
        [HttpPost("/api/trips/{tripId:int}/invitations/users/{userId:int}")]
        [ProducesResponseType(typeof(CreateInvitationHandler), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SendInvitationResponse>> SendInvitation(
            [FromRoute] int tripId,
            [FromRoute] int userId,
            [FromServices] CreateInvitationHandler createInvitationHandler)
        {
            SendInvitationResponse result = await createInvitationHandler.SendInvitationAsync(tripId, userId);

            return Created($"/api/invitations/{result.Id}", result);
        }

        #endregion

        #region GET - My Pending Invitations

        /// <summary>Gets pending invitations for the authenticated user.</summary>
        [HttpGet("mine")]
        [ProducesResponseType(typeof(List<GetMyPendingInvitationResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<GetMyPendingInvitationResponse>>>
            GetMyPendingInvitations([FromServices] GetPendingInvitationsHandler handler)
        {
            List<GetMyPendingInvitationResponse> invitations = await handler.GetInvitesByReciever();

            return Ok(invitations);
        }

        #endregion

        #region GET - Invitation By ID

        /// <summary>Gets an invitation by its identifier.</summary>
        [HttpGet("{invitationId:int}")]
        [ProducesResponseType(
            typeof(GetInvitationResponse),
            StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetInvitationResponse>>
            GetInvitationById(
                [FromRoute] int invitationId,
                [FromServices] GetInvitationByIdHandler handler)
        {
            GetInvitationResponse invitation =
                await handler.GetInviteById(invitationId);

            return Ok(invitation);
        }

        #endregion

        #region PUT - Accept Invitation

        /// <summary>Accepts an invitation to participate in a trip.</summary>
        /// <param name="invitationId">The invitation identifier.</param>
        /// <param name="handler">The handler for accepting invitations.</param>
        [HttpPut("{invitationId:int}/accept")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AcceptInvitation([FromRoute] int invitationId, [FromServices] AcceptInvitationHandler handler)
        {
            await handler.AcceptInvitationAsync(invitationId);
            return NoContent();
        }

        #endregion
    }
}
