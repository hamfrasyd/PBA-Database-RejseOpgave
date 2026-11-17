using TravelApp.BusinessLogic.Handlers.Accounts;

namespace TravelApp.BusinessLogic.Auth
{
    /// <summary>Issues access tokens for verified accounts.</summary>
    public interface IAccessTokenIssuer
    {
        /// <summary>Creates the login response for a verified account.</summary>
        LoginResponse Issue(VerifiedLoginAccount account);
    }
}