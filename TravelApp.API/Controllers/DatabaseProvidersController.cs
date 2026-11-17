using Microsoft.AspNetCore.Mvc;
using TravelApp.API.ExceptionFilters;
using TravelApp.BusinessLogic.Handlers.DatabaseSelection.GetDatabaseProviders;

namespace TravelApp.API.Controllers
{
    /// <summary>Exposes the available database providers.</summary>
    [ApiController]
    [Route("api/database-providers")]
    public class DatabaseProvidersController : ControllerBase
    {
        #region GET - Database Providers
        /// <summary>
        /// Returns the available database provider choices.
        /// </summary>
        /// <param name="getDatabaseProvidersHandler">The handler that lists the database providers.</param>
        /// <returns>The active provider and the selectable choices.</returns>
        // GET api/database-providers
        [HttpGet]
        [ProducesResponseType(typeof(GetDatabaseProvidersResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public ActionResult<GetDatabaseProvidersResponse> Get([FromServices] GetDatabaseProvidersHandler getDatabaseProvidersHandler)
        {
            GetDatabaseProvidersResponse result = getDatabaseProvidersHandler.GetDatabaseProviders();
            return Ok(result);
        }
        #endregion
    }
}
