namespace TravelApp.BusinessLogic.Auth
{
    /// <summary>Contains the account information needed to issue an access token.</summary>
    public sealed record VerifiedLoginAccount(string AccountId, int TravelUserId, IReadOnlyList<string> Roles);
}