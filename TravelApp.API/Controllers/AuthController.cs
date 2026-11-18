using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TravelApp.API.DatabaseSelection;
using TravelApp.API.ExceptionFilters;
using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Handlers.Accounts.Login;
using TravelApp.BusinessLogic.Handlers.Accounts.Register;
using TravelApp.BusinessLogic.Handlers.Users.GetUserProfile;

namespace TravelApp.API.Controllers
{
    /// <summary>Authenticates local accounts and exposes the current user's profile.</summary>
    [ApiController]
    [Route("api/auth")]
    [UseSqlDatabase]
    public class AuthController : ControllerBase
    {
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new AuthController instance.</summary>
        /// <param name="currentUser">The current logged-in user.</param>
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

        /// <summary>Creates a profile and a local login account.</summary>
        // POST api/auth/register
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request, [FromServices] RegisterHandler registerHandler)
        {
            int userId = await registerHandler.RegisterAsync(request);

            return Created($"/api/users/{userId}", new { UserId = userId });
        }

        /// <summary>Returns the profile of the currently authenticated user.</summary>
        // GET api/auth/me
        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(typeof(GetUserProfileResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)] // Thrown from GetUserProfileHandler.
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetUserProfileResponse>> Me([FromServices] GetUserProfileHandler getUserProfileHandler)
        {
            GetUserProfileResponse result = await getUserProfileHandler.GetUserProfileAsync(_currentUser.UserId);

            return Ok(result);
        }

    }
}