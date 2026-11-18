using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.API.ExceptionFilters;
using TravelApp.BusinessLogic.Handlers.Invitations.CreateInvitation;
using TravelApp.BusinessLogic.Handlers.Invitations.GetInvitation;

namespace TravelApp.API.Controllers
{
    /// <summary>Exposes HTTP endpoints for trip invitations.</summary>
    [ApiController]
    [Authorize]
    [Route("api/invitations")]
    public class InvitationsController : ControllerBase
    {
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
    }
}
