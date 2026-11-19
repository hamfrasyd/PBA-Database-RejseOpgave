using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.API.ExceptionFilters;
using TravelApp.BusinessLogic.Handlers.Invitations.SendInvitation;
using TravelApp.BusinessLogic.Handlers.Invitations.DeleteInvitation;
using TravelApp.BusinessLogic.Handlers.Invitations.GetInvitation;
using TravelApp.BusinessLogic.Handlers.Invitations.GetInvitations;
using TravelApp.BusinessLogic.Handlers.Invitations.RespondInvitation;

namespace TravelApp.API.Controllers
{
    /// <summary>Exposes HTTP endpoints for trip invitations.</summary>
    [ApiController]
    [Authorize]
    [Route("api/invitations")]
    public class InvitationsController : ControllerBase
    {
        #region GET - Received Invitations

        /// <summary>Lists invitations received by the current user.</summary>
        /// <param name="getInvitationsHandler">The handler that retrieves the invitations.</param>
        /// <returns>The received invitations.</returns>
        // GET api/invitations/received
        [HttpGet("received")]
        [ProducesResponseType(typeof(List<GetInvitationsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GetInvitationsResponse>>> GetReceivedInvitations([FromServices] GetInvitationsHandler getInvitationsHandler)
        {
            List<GetInvitationsResponse> response = await getInvitationsHandler.GetInvitationsAsync(false);

            return Ok(response);
        }

        #endregion

        #region GET - Sent Invitations

        /// <summary>Lists invitations sent by the current user.</summary>
        /// <param name="getInvitationsHandler">The handler that retrieves the invitations.</param>
        /// <returns>The sent invitations.</returns>
        // GET api/invitations/sent
        [HttpGet("sent")]
        [ProducesResponseType(typeof(List<GetInvitationsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GetInvitationsResponse>>> GetSentInvitations([FromServices] GetInvitationsHandler getInvitationsHandler)
        {
            List<GetInvitationsResponse> response = await getInvitationsHandler.GetInvitationsAsync(true);

            return Ok(response);
        }

        #endregion

        #region GET - Invitation By ID

        /// <summary>Retrieves an invitation for its sender or receiver.</summary>
        /// <param name="invitationId">The invitation identifier.</param>
        /// <param name="getInvitationHandler">The handler that retrieves the invitation.</param>
        /// <returns>The invitation details.</returns>
        // GET api/invitations/{invitationId}
        [HttpGet("{invitationId:int}")]
        [ProducesResponseType(typeof(GetInvitationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetInvitationResponse>> GetInvitation([FromRoute] int invitationId, [FromServices] GetInvitationHandler getInvitationHandler)
        {
            GetInvitationResponse response = await getInvitationHandler.GetInvitationAsync(invitationId);

            return Ok(response);
        }

        #endregion

        #region POST - Send Invitation

        /// <summary>Invites an existing user to a trip.</summary>
        /// <param name="request">The trip and receiving user.</param>
        /// <param name="sendInvitationHandler">The handler that sends the invitation.</param>
        /// <returns>The sent invitation.</returns>
        // POST api/invitations
        [HttpPost]
        [ProducesResponseType(typeof(SendInvitationResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SendInvitationResponse>> SendInvitation([FromBody] SendInvitationRequest request, [FromServices] SendInvitationHandler sendInvitationHandler)
        {
            SendInvitationResponse response = await sendInvitationHandler.SendInvitationAsync(request);

            return Created($"/api/invitations/{response.Id}", response);
        }

        #endregion

        #region PUT - Respond To Invitation

        /// <summary>Accepts or declines a pending invitation.</summary>
        /// <param name="invitationId">The invitation identifier.</param>
        /// <param name="request">The invitation decision.</param>
        /// <param name="respondInvitationHandler">The handler that responds to the invitation.</param>
        /// <returns>The updated invitation status.</returns>
        // PUT api/invitations/{invitationId}/response
        [HttpPut("{invitationId:int}/response")]
        [ProducesResponseType(typeof(RespondInvitationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RespondInvitationResponse>> RespondInvitation([FromRoute] int invitationId, [FromBody] RespondInvitationRequest request, [FromServices] RespondInvitationHandler respondInvitationHandler)
        {
            RespondInvitationResponse response = await respondInvitationHandler.RespondInvitationAsync(invitationId, request);

            return Ok(response);
        }

        #endregion

        #region DELETE - Cancel Invitation

        /// <summary>Cancels an invitation for its sender or an administrator.</summary>
        /// <param name="invitationId">The invitation identifier.</param>
        /// <param name="deleteInvitationHandler">The handler that cancels the invitation.</param>
        /// <returns>An empty response when the invitation is deleted.</returns>
        // DELETE api/invitations/{invitationId}
        [HttpDelete("{invitationId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteInvitation([FromRoute] int invitationId, [FromServices] DeleteInvitationHandler deleteInvitationHandler)
        {
            await deleteInvitationHandler.DeleteInvitationAsync(invitationId);

            return NoContent();
        }

        #endregion
    }
}
