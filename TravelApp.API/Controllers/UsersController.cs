using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.BusinessLogic.Handlers.Destinations;
using TravelApp.BusinessLogic.Handlers.Users;

namespace TravelApp.API.Controllers
{
    /// <summary>Exposes HTTP endpoints for users.</summary>
    [ApiController]
    [Authorize]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {



        /// <summary>Lists visible user profiles with optional search and sorting.</summary>
        // GET api/users
        [HttpGet]
        [ProducesResponseType(typeof(List<GetUserProfilesResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GetUserProfilesResponse>>> GetUsers([FromQuery] GetUserProfilesRequest request, [FromServices] GetUserProfilesHandler getUserProfilesHandler)
        {
            List<GetUserProfilesResponse> result = await getUserProfilesHandler.GetUserProfilesAsync(request);
            if (result.Count == 0)
            {
                return NoContent();
            }
            return Ok(result);
        }

        /// <summary>Retrieves a user profile.</summary>
        // GET api/users/{userId}
        [HttpGet("{userId:int}")]
        [ProducesResponseType(typeof(GetUserProfileResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Thrown from GetUserProfileHandler for missing resources.
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetUserProfileResponse>> GetUser([FromRoute] int userId, [FromServices] GetUserProfileHandler getUserProfileHandler)
        {
            GetUserProfileResponse result = await getUserProfileHandler.GetUserProfileAsync(userId);
            return Ok(result);
        }

        /// <summary>Lists unique visited countries from completed trips.</summary>
        // GET api/users/{userId}/visited-countries
        [HttpGet("{userId:int}/visited-countries")]
        [ProducesResponseType(typeof(List<GetVisitedCountriesResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Thrown from GetVisitedCountriesHandler for missing resources.
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GetVisitedCountriesResponse>>> GetVisitedCountries([FromRoute] int userId, [FromServices] GetVisitedCountriesHandler getVisitedCountriesHandler)
        {
            List<GetVisitedCountriesResponse> result = await getVisitedCountriesHandler.GetVisitedCountriesAsync(userId);
            if (result.Count == 0)
            {
                return NoContent();
            }
            return Ok(result);
        }

        /// <summary>Updates a user profile and returns its ID.</summary>
        // PUT api/users/{userId}
        [HttpPut("{userId:int}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // May be thrown from UpdateUserHandler.
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Thrown from UpdateUserHandler for missing resources.
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> UpdateUser([FromRoute] int userId, [FromBody] UpdateUserRequest request, [FromServices] UpdateUserHandler updateUserHandler)
        {
            request.UserId = userId;
            await updateUserHandler.UpdateUserAsync(request);
            return Ok(userId);
        }

        /// <summary>Deletes a user and returns its ID.</summary>
        // DELETE api/users/{userId}
        [HttpDelete("{userId:int}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Thrown from DeleteUserHandler for missing resources.
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> DeleteUser([FromRoute] int userId, [FromServices] DeleteUserHandler deleteUserHandler)
        {
            await deleteUserHandler.DeleteUserAsync(userId);
            return Ok(userId);
        }
    }
}
