namespace TravelApp.BusinessLogic.Auth
{
    /// <summary>Verifies the credentials of a local login account.</summary>
    public interface ILoginAccountVerifier
    {
        /// <summary>Looks up the account and verifies its password.</summary>
        Task<VerifiedLoginAccount?> VerifyAsync(string userName, string password);
    }
}