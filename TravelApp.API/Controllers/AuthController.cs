using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.Handlers.Accounts;
using TravelApp.BusinessLogic.Handlers.Users;

namespace TravelApp.API.Controllers
{
    /// <summary>Authenticates local accounts and exposes the current user's profile.</summary>
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new AuthController instance.</summary>
        public AuthController(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        /// <summary>Verifies local credentials and returns a JWT access token.</summary>
        // POST api/auth/login
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request, [FromServices] LoginHandler loginHandler)
        {
            LoginResponse? response = await loginHandler.LoginAsync(request);

            if (response is null)
            {
                return Unauthorized(new { Message = "Ugyldigt brugernavn eller adgangskode." });
            }

            return Ok(response);
        }

        /// <summary>Returns the profile of the currently authenticated user.</summary>
        // GET api/auth/me
        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(typeof(GetUserProfileResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Thrown from GetUserProfileHandler.
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetUserProfileResponse>> Me([FromServices] GetUserProfileHandler getUserProfileHandler)
        {
            GetUserProfileResponse result = await getUserProfileHandler.GetUserProfileAsync(_currentUser.UserId);

            return Ok(result);
        }
    }
}