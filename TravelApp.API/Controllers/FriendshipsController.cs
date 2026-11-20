using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.API.ExceptionFilters;
using TravelApp.BusinessLogic.Handlers.Friendships.AddFriend;
using TravelApp.BusinessLogic.Handlers.Friendships.GetFriendship;
using TravelApp.BusinessLogic.Handlers.Friendships.GetFriends;
using TravelApp.BusinessLogic.Handlers.Friendships.RemoveFriend;

namespace TravelApp.API.Controllers
{
    /// <summary>Exposes HTTP endpoints for friendships.</summary>
    [ApiController]
    [Authorize]
    [Route("api/friendships")]
    public class FriendshipsController : ControllerBase
    {
        #region GET - Friends By User

        /// <summary>Lists a user's friends when their profile is visible.</summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="getFriendsHandler">The handler that retrieves the friends.</param>
        /// <returns>The user's friends.</returns>
        // GET api/friendships/users/{userId}
        [HttpGet("users/{userId:int}")]
        [ProducesResponseType(typeof(List<GetFriendsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GetFriendsResponse>>> GetFriends([FromRoute] int userId, [FromServices] GetFriendsHandler getFriendsHandler)
        {
            List<GetFriendsResponse> response = await getFriendsHandler.GetFriendsAsync(userId);

            return Ok(response);
        }

        #endregion

        #region GET - Friendship By ID

        /// <summary>Retrieves a friendship for one of its members.</summary>
        /// <param name="friendshipId">The friendship identifier.</param>
        /// <param name="getFriendshipHandler">The handler that retrieves the friendship.</param>
        /// <returns>The friendship details.</returns>
        // GET api/friendships/{friendshipId}
        [HttpGet("{friendshipId:int}")]
        [ProducesResponseType(typeof(GetFriendshipResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetFriendshipResponse>> GetFriendship([FromRoute] int friendshipId, [FromServices] GetFriendshipHandler getFriendshipHandler)
        {
            GetFriendshipResponse response = await getFriendshipHandler.GetFriendshipAsync(friendshipId);

            return Ok(response);
        }

        #endregion

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

        #region DELETE - Remove Friend

        /// <summary>Removes a friendship for one of its members.</summary>
        /// <param name="friendshipId">The friendship identifier.</param>
        /// <param name="removeFriendHandler">The handler that removes the friendship.</param>
        /// <returns>An empty response when the friendship is removed.</returns>
        // DELETE api/friendships/{friendshipId}
        [HttpDelete("{friendshipId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveFriend([FromRoute] int friendshipId, [FromServices] RemoveFriendHandler removeFriendHandler)
        {
            await removeFriendHandler.RemoveFriendAsync(friendshipId);

            return NoContent();
        }

        #endregion
    }
}
