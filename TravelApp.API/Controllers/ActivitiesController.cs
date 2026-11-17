
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

        /// <summary>Creates an activity and returns the persisted activity.</summary>
        /// <param name="request">The activity to create.</param>
        /// <param name="createActivityHandler">The handler that creates the activity.</param>
        /// <returns>The created activity.</returns>
        /// <exception cref="TravelAppValidationException">Thrown when the name is missing or the city does not exist.</exception>
        // POST api/activities
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(CreateActivityResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreateActivityResponse>> CreateActivity([FromBody] CreateActivityRequest request, [FromServices] CreateActivityHandler createActivityHandler)
        {
            CreateActivityResponse createdActivity = await createActivityHandler.CreateActivityAsync(request);

            return Created($"/api/activities/{createdActivity.Id}", createdActivity);
        }

        #endregion

        #region GET - All Activities By City

        /// <summary>Lists activities for a specific city.</summary>
        /// <param name="cityId">The city identifier.</param>
        /// <param name="getActivitiesByCityHandler">The handler that loads the city's activities.</param>
        /// <returns>The activities in the city.</returns>
        // GET api/activities/cities/{cityId}
        [HttpGet("cities/{cityId:int}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<GetActivitiesByCityResponse>), StatusCodes.Status200OK)]
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

        /// <summary>Updates an activity and returns the persisted activity.</summary>
        /// <param name="activityId">The activity identifier.</param>
        /// <param name="request">The updated activity details.</param>
        /// <param name="updateActivityHandler">The handler that updates the activity.</param>
        /// <returns>The updated activity.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the activity does not exist.</exception>
        /// <exception cref="TravelAppValidationException">Thrown when the name is missing or the city does not exist.</exception>
        // PUT api/activities/{activityId}
        [HttpPut("{activityId:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(UpdateActivityResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UpdateActivityResponse>> UpdateActivity([FromRoute] int activityId, [FromBody] UpdateActivityRequest request, [FromServices] UpdateActivityHandler updateActivityHandler)
        {
            UpdateActivityResponse updatedActivity = await updateActivityHandler.UpdateActivityAsync(activityId, request);

            return Ok(updatedActivity);
        }

        #endregion

        #region DELETE - Activity

        /// <summary>Deletes an activity.</summary>
        /// <param name="activityId">The activity identifier.</param>
        /// <param name="deleteActivityHandler">The handler that deletes the activity.</param>
        /// <returns>An empty response when the activity is deleted.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the activity does not exist.</exception>
        // DELETE api/activities/{activityId}
        [HttpDelete("{activityId:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteActivity([FromRoute] int activityId, [FromServices] DeleteActivityHandler deleteActivityHandler)
        {
            await deleteActivityHandler.DeleteActivityAsync(activityId);

            return NoContent();
        }

        #endregion
    }
}