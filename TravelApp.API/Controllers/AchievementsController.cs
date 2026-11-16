using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.BusinessLogic.Exceptions.Filters;
using TravelApp.BusinessLogic.Handlers.Achievements;

namespace TravelApp.API.Controllers
{
    /// <summary>Exposes HTTP endpoints for achievements.</summary>
    [ApiController]
    [Route("api/achievements")]
    public class AchievementsController : ControllerBase
    {



        /// <summary>Lists available achievements.</summary>
        // GET api/achievements
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<GetAchievementDefinitionsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GetAchievementDefinitionsResponse>>> GetDefinitions([FromServices] GetAchievementDefinitionsHandler getAchievementDefinitionsHandler)
        {
            List<GetAchievementDefinitionsResponse> result = await getAchievementDefinitionsHandler.GetAchievementDefinitionsAsync();

            return Ok(result);
        }

        /// <summary>Lists achievements earned by a user.</summary>
        // GET api/achievements/users/{userId}
        [HttpGet("users/{userId:int}")]
        [Authorize]
        [ProducesResponseType(typeof(List<GetEarnedAchievementsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GetEarnedAchievementsResponse>>> GetEarned([FromRoute] int userId, [FromServices] GetEarnedAchievementsHandler getEarnedAchievementsHandler)
        {
            List<GetEarnedAchievementsResponse> result = await getEarnedAchievementsHandler.GetEarnedAchievementsAsync(userId);

            return Ok(result);
        }
    }
}
