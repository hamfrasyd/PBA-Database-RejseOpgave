using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.Handlers.Accounts;
using TravelApp.BusinessLogic.Handlers.Users;

namespace TravelApp.API.Controllers
{
    /// <summary>Local demonstration sessions using users already stored in dbo.User.</summary>
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

        /// <summary>Selects an existing user for a local demonstration session.</summary>
        // POST api/auth/login
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Thrown from LoginHandler for an invalid ID.
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Thrown from LoginHandler when the user does not exist.
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request, [FromServices] LoginHandler loginHandler)
        {
            LoginResponse result = await loginHandler.LoginAsync(request);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, result.UserId.ToString()),
                new Claim(ClaimTypes.Name, result.FullName)
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Ok(result);
        }

        /// <summary>Signs out of the current demonstration session.</summary>
        // POST api/auth/logout
        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            string result = "Logged out.";
            return Ok(result);
        }

        /// <summary>Returns the profile of the currently selected user.</summary>
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
