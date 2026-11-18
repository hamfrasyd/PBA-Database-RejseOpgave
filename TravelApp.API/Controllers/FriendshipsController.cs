using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.API.ExceptionFilters;
using TravelApp.BusinessLogic.Handlers.Friendships.AddFriend;

namespace TravelApp.API.Controllers
{
    /// <summary>Exposes HTTP endpoints for friendships.</summary>
    [ApiController]
    [Authorize]
    [Route("api/friendships")]
    public class FriendshipsController : ControllerBase
    {
        #region POST - Add Friend

        /// <summary>Adds an existing user as a friend.</summary>
        /// <param name="friendUserId">The identifier of the user to add.</param>
        /// <param name="addFriendHandler">The handler that adds the friendship.</param>
        /// <returns>The created friendship.</returns>
        // POST api/friendships/users/{friendUserId}
        [HttpPost("users/{friendUserId:int}")]
        [ProducesResponseType(typeof(AddFriendResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AddFriendResponse>> AddFriend([FromRoute] int friendUserId, [FromServices] AddFriendHandler addFriendHandler)
        {
            AddFriendResponse response = await addFriendHandler.AddFriendAsync(friendUserId);

            return Created($"/api/friendships/{response.Id}", response);
        }

        #endregion
    }
}
