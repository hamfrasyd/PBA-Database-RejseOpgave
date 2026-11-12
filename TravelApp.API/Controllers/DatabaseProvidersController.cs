using Microsoft.AspNetCore.Mvc;
using TravelApp.BusinessLogic.Handlers.DatabaseSelection;

namespace TravelApp.API.Controllers
{
    [ApiController]
    [Route("api/database-providers")]
    public class DatabaseProvidersController : ControllerBase
    {
        private readonly GetDatabaseProvidersHandler _getDatabaseProvidersHandler;

        public DatabaseProvidersController(GetDatabaseProvidersHandler getDatabaseProvidersHandler)
        {
            _getDatabaseProvidersHandler = getDatabaseProvidersHandler;
        }

        /// <summary>
        /// Returns the available database provider choices.
        /// </summary>
        // GET api/database-providers
        [HttpGet]
        [ProducesResponseType(typeof(GetDatabaseProvidersResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<GetDatabaseProvidersResponse> Get()
        {
            GetDatabaseProvidersResponse result = _getDatabaseProvidersHandler.GetDatabaseProviders();
            return Ok(result);
        }
    }
}
