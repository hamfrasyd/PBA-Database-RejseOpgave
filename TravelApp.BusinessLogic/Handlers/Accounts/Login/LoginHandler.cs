using TravelApp.BusinessLogic.Auth;

namespace TravelApp.BusinessLogic.Handlers.Accounts
{
    /// <summary>Handles login for local accounts.</summary>
    public class LoginHandler
    {
        private readonly ILoginAccountVerifier _accountVerifier;
        private readonly IAccessTokenIssuer _tokenIssuer;

        /// <summary>Initializes a new LoginHandler instance.</summary>
        public LoginHandler(ILoginAccountVerifier accountVerifier, IAccessTokenIssuer tokenIssuer)
        {
            _accountVerifier = accountVerifier;
            _tokenIssuer = tokenIssuer;
        }

        /// <summary>Verifies the credentials and issues an access token.</summary>
        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            VerifiedLoginAccount? account = await _accountVerifier.VerifyAsync(request.UserName, request.Password);

            if (account is null)
            {
                return null;
            }

            return _tokenIssuer.Issue(account);
        }
    }
}