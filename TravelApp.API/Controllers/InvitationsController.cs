using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.API.ExceptionFilters;
using TravelApp.BusinessLogic.Handlers.Invitations.CreateInvitation;
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

        #region POST - Create Invitation

        /// <summary>Invites an existing user to a trip.</summary>
        /// <param name="request">The trip and receiving user.</param>
        /// <param name="createInvitationHandler">The handler that creates the invitation.</param>
        /// <returns>The created invitation.</returns>
        // POST api/invitations
        [HttpPost]
        [ProducesResponseType(typeof(CreateInvitationResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreateInvitationResponse>> CreateInvitation([FromBody] CreateInvitationRequest request, [FromServices] CreateInvitationHandler createInvitationHandler)
        {
            CreateInvitationResponse response = await createInvitationHandler.CreateInvitationAsync(request);

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
    }
}
