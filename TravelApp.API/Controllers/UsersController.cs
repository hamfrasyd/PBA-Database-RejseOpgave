using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.API.ExceptionFilters;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Handlers.Locations.GetVisitedCountries;
using TravelApp.BusinessLogic.Handlers.Users.DeleteUser;
using TravelApp.BusinessLogic.Handlers.Users.GetUserProfile;
using TravelApp.BusinessLogic.Handlers.Users.GetAllUserProfiles;
using TravelApp.BusinessLogic.Handlers.Users.UpdateUser;

namespace TravelApp.API.Controllers
{
    /// <summary>Exposes HTTP endpoints for users.</summary>
    [ApiController]
    [Authorize]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        #region GET - All Users

        /// <summary>Lists visible user profiles with optional search and sorting.</summary>
        /// <param name="request">The filter and sort options.</param>
        /// <param name="getAllUserProfilesHandler">The handler that loads the profiles.</param>
        /// <returns>The matching profiles. An empty list is returned when none match.</returns>
        // GET api/users
        [HttpGet]
        [ProducesResponseType(typeof(List<UserProfileResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<UserProfileResponse>>> GetUsers([FromQuery] GetAllUserProfilesRequest request, [FromServices] GetAllUserProfilesHandler getAllUserProfilesHandler)
        {
            List<UserProfileResponse> result = await getAllUserProfilesHandler.GetAllUserProfilesAsync(request);

            return Ok(result);
        }

        #endregion

        #region GET - User By ID

        /// <summary>Retrieves a user profile.</summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="getUserProfileHandler">The handler that loads the profile.</param>
        /// <returns>The user profile.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the user does not exist.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the profile is private and the current user is neither the owner nor an administrator.</exception>
        // GET api/users/{userId}
        [HttpGet("{userId:int}")]
        [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserProfileResponse>> GetUser([FromRoute] int userId, [FromServices] GetUserProfileHandler getUserProfileHandler)
        {
            UserProfileResponse result = await getUserProfileHandler.GetUserProfileAsync(userId);

            return Ok(result);
        }

        #endregion

        #region GET - Visited Countries

        /// <summary>Lists unique visited countries from completed trips.</summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="getVisitedCountriesHandler">The handler that loads the visited countries.</param>
        /// <returns>The visited countries, or no content when the user has none.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the user does not exist.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the profile is private and the current user is neither the owner nor an administrator.</exception>
        // GET api/users/{userId}/visited-countries
        [HttpGet("{userId:int}/visited-countries")]
        [ProducesResponseType(typeof(List<GetVisitedCountriesResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GetVisitedCountriesResponse>>> GetVisitedCountries([FromRoute] int userId, [FromServices] GetVisitedCountriesHandler getVisitedCountriesHandler)
        {
            List<GetVisitedCountriesResponse> result = await getVisitedCountriesHandler.GetVisitedCountriesAsync(userId);

            if (result.Count == 0)
            {
                return NoContent();
            }

            return Ok(result);
        }

        #endregion

        #region PUT - Update User

        /// <summary>Updates a user profile and returns the persisted user.</summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="request">The updated user details.</param>
        /// <param name="updateUserHandler">The handler that updates the user.</param>
        /// <returns>The updated user.</returns>
        /// <exception cref="TravelAppForbiddenException">Thrown when the current user cannot update the user.</exception>
        /// <exception cref="TravelAppNotFoundException">Thrown when the user does not exist.</exception>
        /// <exception cref="TravelAppValidationException">Thrown when the home city does not exist.</exception>
        // PUT api/users/{userId}
        [HttpPut("{userId:int}")]
        [ProducesResponseType(typeof(UpdateUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UpdateUserResponse>> UpdateUser([FromRoute] int userId, [FromBody] UpdateUserRequest request, [FromServices] UpdateUserHandler updateUserHandler)
        {
            UpdateUserResponse result = await updateUserHandler.UpdateUserAsync(userId, request);

            return Ok(result);
        }

        #endregion

        #region DELETE - User

        /// <summary>Deletes a user.</summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="deleteUserHandler">The handler that deletes the user.</param>
        /// <returns>An empty response when the user is deleted.</returns>
        /// <exception cref="TravelAppForbiddenException">Thrown when the current user cannot delete the user.</exception>
        /// <exception cref="TravelAppNotFoundException">Thrown when the user does not exist.</exception>
        // DELETE api/users/{userId}
        [HttpDelete("{userId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUser([FromRoute] int userId, [FromServices] DeleteUserHandler deleteUserHandler)
        {
            await deleteUserHandler.DeleteUserAsync(userId);

            return NoContent();
        }

        #endregion
    }
}