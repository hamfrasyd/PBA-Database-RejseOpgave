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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GetAchievementDefinitionsResponse>>> GetDefinitions([FromServices] GetAchievementDefinitionsHandler getAchievementDefinitionsHandler)
        {
            List<GetAchievementDefinitionsResponse> result = await getAchievementDefinitionsHandler.GetAchievementDefinitionsAsync();
            if (result.Count == 0)
            {
                return NoContent();
            }
            return Ok(result);
        }

        /// <summary>Lists achievements earned by a user.</summary>
        // GET api/achievements/users/{userId}
        [HttpGet("users/{userId:int}")]
        [Authorize]
        [ProducesResponseType(typeof(List<GetEarnedAchievementsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GetEarnedAchievementsResponse>>> GetEarned([FromRoute] int userId, [FromServices] GetEarnedAchievementsHandler getEarnedAchievementsHandler)
        {
            List<GetEarnedAchievementsResponse> result = await getEarnedAchievementsHandler.GetEarnedAchievementsAsync(userId);
            if (result.Count == 0)
            {
                return NoContent();
            }
            return Ok(result);
        }
    }
}
