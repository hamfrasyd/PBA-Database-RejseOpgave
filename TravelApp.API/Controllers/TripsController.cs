
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.API.ExceptionFilters;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Handlers.Activities.GetActivity;
using TravelApp.BusinessLogic.Handlers.Trips;
using TravelApp.BusinessLogic.Handlers.Trips.CreateTrip;
using TravelApp.BusinessLogic.Handlers.Trips.GetTrip;
using TravelApp.BusinessLogic.Handlers.Trips.TripParticipants;

namespace TravelApp.API.Controllers
{
    /// <summary>Exposes HTTP endpoints for trips and their related resources.</summary>
    [ApiController]
    [Authorize]
    [Route("api/trips")]
    public class TripsController : ControllerBase
    {
        #region POST - Create-new Trip

        /// <summary>Creates a trip for the current user.</summary>
        /// <param name="request">The trip details to create.</param>
        /// <param name="createTripHandler">The handler that creates the trip.</param>
        /// <returns>The created trip.</returns>
        /// <exception cref="TravelAppValidationException">Thrown when the start date is after the end date.</exception>
        /// <exception cref="TravelAppNotFoundException">Thrown when the current user does not exist.</exception>
        // POST api/trips
        [HttpPost]
        [ProducesResponseType(typeof(CreateTripResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreateTripResponse>> CreateTrip([FromBody] CreateTripRequest request, [FromServices] CreateTripHandler createTripHandler)
        {
            CreateTripResponse result = await createTripHandler.CreateTripAsync(request);

            return Created($"/api/trips/{result.Id}", result);
        }

        #endregion

        #region POST - Add-to-existing Cities, Activities & Participants

        /// <summary>Adds an existing city to a trip.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="cityId">The city identifier.</param>
        /// <param name="manageTripRelationsHandler">The handler that changes trip relations.</param>
        /// <returns>An empty response when the city is added.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the trip or city does not exist.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the current user cannot modify the trip.</exception>
        /// <exception cref="TravelAppConflictException">Thrown when the city is already part of the trip.</exception>
        // POST api/trips/{tripId}/cities/{cityId}
        [HttpPost("{tripId:int}/cities/{cityId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddCity([FromRoute] int tripId, [FromRoute] int cityId, [FromServices] ManageTripRelationsHandler manageTripRelationsHandler)
        {
            await manageTripRelationsHandler.AddCityAsync(tripId, cityId);

            return NoContent();
        }

        /// <summary>Adds an existing activity to a trip.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="activityId">The activity identifier.</param>
        /// <param name="manageTripRelationsHandler">The handler that changes trip relations.</param>
        /// <returns>An empty response when the activity is added.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the trip or activity does not exist.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the current user cannot modify the trip.</exception>
        /// <exception cref="TravelAppValidationException">Thrown when the activity's city is not on the trip.</exception>
        /// <exception cref="TravelAppConflictException">Thrown when the activity is already part of the trip.</exception>
        // POST api/trips/{tripId}/activities/{activityId}
        [HttpPost("{tripId:int}/activities/{activityId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddActivity([FromRoute] int tripId, [FromRoute] int activityId, [FromServices] ManageTripRelationsHandler manageTripRelationsHandler)
        {
            await manageTripRelationsHandler.AddActivityAsync(tripId, activityId);

            return NoContent();
        }

        /// <summary>Adds an existing user as a trip participant.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="manageTripRelationsHandler">The handler that changes trip relations.</param>
        /// <returns>An empty response when the participant is added.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the trip or user does not exist.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the current user cannot modify the trip.</exception>
        /// <exception cref="TravelAppConflictException">Thrown when the user is already participating.</exception>
        // POST api/trips/{tripId}/participants/{userId}
        [HttpPost("{tripId:int}/participants/{userId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddParticipant([FromRoute] int tripId, [FromRoute] int userId, [FromServices] ManageTripRelationsHandler manageTripRelationsHandler)
        {
            await manageTripRelationsHandler.AddParticipantAsync(tripId, userId);

            return NoContent();
        }

        #endregion

        #region GET - All Trips

        /// <summary>Lists trips belonging to the current user.</summary>
        /// <param name="countryId">The country identifier to filter by, or <see langword="null"/> for all countries.</param>
        /// <param name="continentId">The continent identifier to filter by, or <see langword="null"/> for all continents.</param>
        /// <param name="handler">The handler that loads the trips.</param>
        /// <returns>The matching trip summaries.</returns>
        // GET api/trips
        [HttpGet]
        [ProducesResponseType(typeof(List<GetTripsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GetTripsResponse>>> GetTrips([FromQuery] int? countryId, [FromQuery] int? continentId, [FromServices] GetTripsHandler handler)
        {
            List<GetTripsResponse> result = await handler.GetTripsAsync(countryId, continentId);

            return Ok(result);
        }

        #endregion

        #region GET - Trip By ID

        /// <summary>Retrieves a trip with its cities, countries and continents.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="getTripHandler">The handler that loads the trip.</param>
        /// <returns>The trip details.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the trip does not exist.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the current user cannot view the trip.</exception>
        // GET api/trips/{tripId}
        [HttpGet("{tripId:int}")]
        [ProducesResponseType(typeof(GetTripResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetTripResponse>> GetTrip([FromRoute] int tripId, [FromServices] GetTripHandler getTripHandler)
        {
            GetTripResponse result = await getTripHandler.GetTripAsync(tripId);

            return Ok(result);
        }

        #endregion

        #region GET - Trip Activities & Trip Participants

        /// <summary>Lists activities associated with a trip.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="getTripActivitiesHandler">The handler that loads the trip activities.</param>
        /// <returns>The activities linked to the trip.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the trip does not exist.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the current user cannot view the trip.</exception>
        // GET api/trips/{tripId}/activities
        [HttpGet("{tripId:int}/activities")]
        [ProducesResponseType(typeof(List<GetActivityResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GetActivityResponse>>> GetTripActivities([FromRoute] int tripId, [FromServices] GetTripActivitiesHandler getTripActivitiesHandler)
        {
            List<GetActivityResponse> result = await getTripActivitiesHandler.GetTripActivitiesAsync(tripId);

            return Ok(result);
        }

        /// <summary>Lists users participating in a trip.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="getTripParticipantsHandler">The handler that loads the participants.</param>
        /// <returns>The participants of the trip.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the trip does not exist.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the current user cannot view the trip.</exception>
        // GET api/trips/{tripId}/participants
        [HttpGet("{tripId:int}/participants")]
        [ProducesResponseType(typeof(List<GetTripParticipantResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GetTripParticipantResponse>>> GetTripParticipants([FromRoute] int tripId, [FromServices] GetTripParticipantsHandler getTripParticipantsHandler)
        {
            List<GetTripParticipantResponse> result = await getTripParticipantsHandler.GetTripParticipantsAsync(tripId);

            return Ok(result);
        }

        #endregion

        #region PUT - Update Trip

        /// <summary>Updates a trip's dates, status and notes.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="request">The updated trip details.</param>
        /// <param name="updateTripHandler">The handler that updates the trip.</param>
        /// <returns>An empty response when the trip is updated.</returns>
        /// <exception cref="TravelAppValidationException">Thrown when the start date is after the end date.</exception>
        /// <exception cref="TravelAppNotFoundException">Thrown when the trip does not exist.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the current user cannot update the trip.</exception>
        // PUT api/trips/{tripId}
        [HttpPut("{tripId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTrip([FromRoute] int tripId, [FromBody] UpdateTripRequest request, [FromServices] UpdateTripHandler updateTripHandler)
        {
            request.TripId = tripId;

            await updateTripHandler.UpdateTripAsync(request);

            return NoContent();
        }

        #endregion

        #region DELETE - Trip

        /// <summary>Deletes a trip and its related records.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="deleteTripHandler">The handler that deletes the trip.</param>
        /// <returns>An empty response when the trip is deleted.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the trip does not exist.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the current user cannot delete the trip.</exception>
        // DELETE api/trips/{tripId}
        [HttpDelete("{tripId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteTrip([FromRoute] int tripId, [FromServices] DeleteTripHandler deleteTripHandler)
        {
            await deleteTripHandler.DeleteTripAsync(tripId);

            return NoContent();
        }

        #endregion

        #region DELETE - Remove-from-trip Cities, Activities & Participants

        /// <summary>Removes a city from a trip.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="cityId">The city identifier.</param>
        /// <param name="manageTripRelationsHandler">The handler that changes trip relations.</param>
        /// <returns>An empty response when the city is removed.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the trip does not exist or the city is not on the trip.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the current user cannot modify the trip.</exception>
        /// <exception cref="TravelAppConflictException">Thrown when the city still has activities on the trip.</exception>
        // DELETE api/trips/{tripId}/cities/{cityId}
        [HttpDelete("{tripId:int}/cities/{cityId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveCity([FromRoute] int tripId, [FromRoute] int cityId, [FromServices] ManageTripRelationsHandler manageTripRelationsHandler)
        {
            await manageTripRelationsHandler.RemoveCityAsync(tripId, cityId);

            return NoContent();
        }

        /// <summary>Removes an activity from a trip.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="activityId">The activity identifier.</param>
        /// <param name="manageTripRelationsHandler">The handler that changes trip relations.</param>
        /// <returns>An empty response when the activity is removed.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the trip does not exist or the activity is not on the trip.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the current user cannot modify the trip.</exception>
        // DELETE api/trips/{tripId}/activities/{activityId}
        [HttpDelete("{tripId:int}/activities/{activityId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveActivity([FromRoute] int tripId, [FromRoute] int activityId, [FromServices] ManageTripRelationsHandler manageTripRelationsHandler)
        {
            await manageTripRelationsHandler.RemoveActivityAsync(tripId, activityId);

            return NoContent();
        }

        /// <summary>Removes a participant from a trip.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="manageTripRelationsHandler">The handler that changes trip relations.</param>
        /// <returns>An empty response when the participant is removed.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the trip does not exist or the user is not participating.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the current user cannot modify the trip.</exception>
        /// <exception cref="TravelAppConflictException">Thrown when the user is the last participant.</exception>
        // DELETE api/trips/{tripId}/participants/{userId}
        [HttpDelete("{tripId:int}/participants/{userId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveParticipant([FromRoute] int tripId, [FromRoute] int userId, [FromServices] ManageTripRelationsHandler manageTripRelationsHandler)
        {
            await manageTripRelationsHandler.RemoveParticipantAsync(tripId, userId);

            return NoContent();
        }

        #endregion
    }
}
