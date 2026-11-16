
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.API.ExceptionFilters;
using TravelApp.BusinessLogic.Handlers.Activities.CreateActivity;
using TravelApp.BusinessLogic.Handlers.Activities.DeleteActivity;
using TravelApp.BusinessLogic.Handlers.Activities.GetActivity;
using TravelApp.BusinessLogic.Handlers.Activities.UpdateActivity;

namespace TravelApp.API.Controllers
{
    /// <summary>Exposes HTTP endpoints for activities.</summary>
    [ApiController]
    [Route("api/activities")]
    public class ActivitiesController : ControllerBase
    {
        #region GET All - Activities

        ///// <summary>Lists activities, optionally filtered by city.</summary>
        //// GET api/activities
        //// GET api/activities?cityId=5
        //[HttpGet]
        //[AllowAnonymous]
        //[ProducesResponseType(typeof(List<GetCityActivitiesResponse>), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        //public async Task<ActionResult<List<GetCityActivitiesResponse>>> GetActivities([FromQuery] int? cityId, [FromServices] GetActivitiesHandler getActivitiesHandler)
        //{
        //    // TODO: Implement GetActivitiesHandler with optional cityId filtering.
        //    List<GetCityActivitiesResponse> result = await getActivitiesHandler.GetActivitiesAsync(cityId);

        //    return Ok(result);
        //}

        #endregion

        #region GET By ID - Activities

        /// <summary>Retrieves one activity.</summary>
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

        #region Create - Activities

        /// <summary>Creates an activity and returns its ID.</summary>
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
            int activityId = await createActivityHandler.CreateActivityAsync(request);

            return Created($"/api/activities/{activityId}", activityId);
        }

        #endregion

        #region Update - Activities

        /// <summary>Updates an activity and returns its ID.</summary>
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

        #region Delete - Activities

        /// <summary>Deletes an activity and returns its ID.</summary>
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
