
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.API.ExceptionFilters;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Handlers.Activities.CreateActivity;
using TravelApp.BusinessLogic.Handlers.Activities.DeleteActivity;
using TravelApp.BusinessLogic.Handlers.Activities.GetActivitiesByCity;
using TravelApp.BusinessLogic.Handlers.Activities.GetActivity;
using TravelApp.BusinessLogic.Handlers.Activities.UpdateActivity;

namespace TravelApp.API.Controllers
{
    /// <summary>Exposes HTTP endpoints for activities.</summary>
    [ApiController]
    [Route("api/activities")]
    public class ActivitiesController : ControllerBase
    {
        #region POST - Create-new Activity for a city

        /// <summary>Creates an activity and returns its ID.</summary>
        /// <param name="request">The activity to create.</param>
        /// <param name="createActivityHandler">The handler that creates the activity.</param>
        /// <returns>The identifier of the created activity.</returns>
        /// <exception cref="TravelAppValidationException">Thrown when the name is missing or the city does not exist.</exception>
        // POST api/activities
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> CreateActivity([FromBody] CreateActivityRequest request, [FromServices] CreateActivityHandler createActivityHandler)
        {
            int activityId = await createActivityHandler.CreateNewActivityAsync(request);

            return Created($"/api/activities/{activityId}", activityId);
        }

        #endregion

        #region GET - All Activities By City

        /// <summary>Lists activities for a specific city.</summary>
        /// <param name="cityId">The city identifier.</param>
        /// <param name="getActivitiesByCityHandler">The handler that loads the city's activities.</param>
        /// <returns>The activities in the city.</returns>
        /// <exception cref="InvalidOperationException">Thrown when an activity's city was not loaded.</exception>
        // GET api/activities/cities/{cityId}
        [HttpGet("cities/{cityId:int}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<GetActivitiesByCityResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GetActivitiesByCityResponse>>> GetActivitiesByCity([FromRoute] int cityId, [FromServices] GetActivitiesByCityHandler getActivitiesByCityHandler)
        {
            List<GetActivitiesByCityResponse> result = await getActivitiesByCityHandler.GetActivitiesByCityIdAsync(cityId);

            return Ok(result);
        }

        #endregion

        #region GET - Activity By ID

        /// <summary>Retrieves one activity.</summary>
        /// <param name="activityId">The activity identifier.</param>
        /// <param name="getActivityHandler">The handler that loads the activity.</param>
        /// <returns>The activity.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the activity does not exist.</exception>
        /// <exception cref="TravelAppValidationException">Thrown when the activity has no city.</exception>
        // GET api/activities/{activityId}
        [HttpGet("{activityId:int}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetActivityResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetActivityResponse>> GetActivity([FromRoute] int activityId, [FromServices] GetActivityHandler getActivityHandler)
        {
            GetActivityResponse result = await getActivityHandler.GetActivityAsync(activityId);

            return Ok(result);
        }

        #endregion

        #region PUT - Update Activity

        /// <summary>Updates an activity and returns its ID.</summary>
        /// <param name="activityId">The activity identifier.</param>
        /// <param name="request">The updated activity details.</param>
        /// <param name="updateActivityHandler">The handler that updates the activity.</param>
        /// <returns>The identifier of the updated activity.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the activity does not exist.</exception>
        /// <exception cref="TravelAppValidationException">Thrown when the name is missing or the city does not exist.</exception>
        // PUT api/activities/{activityId}
        [HttpPut("{activityId:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> UpdateActivity([FromRoute] int activityId, [FromBody] UpdateActivityRequest request, [FromServices] UpdateActivityHandler updateActivityHandler)
        {
            await updateActivityHandler.UpdateActivityAsync(activityId, request);

            return Ok(activityId);
        }

        #endregion

        #region DELETE - Activity

        /// <summary>Deletes an activity and returns its ID.</summary>
        /// <param name="activityId">The activity identifier.</param>
        /// <param name="deleteActivityHandler">The handler that deletes the activity.</param>
        /// <returns>The identifier of the deleted activity.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the activity does not exist.</exception>
        // DELETE api/activities/{activityId}
        [HttpDelete("{activityId:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> DeleteActivity([FromRoute] int activityId, [FromServices] DeleteActivityHandler deleteActivityHandler)
        {
            await deleteActivityHandler.DeleteActivityAsync(activityId);

            return Ok(activityId);
        }

        #endregion
    }
}
