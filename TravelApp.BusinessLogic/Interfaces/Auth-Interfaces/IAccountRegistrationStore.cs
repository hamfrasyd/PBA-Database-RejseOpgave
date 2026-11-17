using TravelApp.BusinessLogic.Models.Users;

namespace TravelApp.BusinessLogic.Auth
{
    /// <summary>Stores a new profile and its login account together.</summary>
    public interface IAccountRegistrationStore
    {
        /// <summary>Creates the profile and account and returns the profile ID.</summary>
        Task<int> CreateAsync(TravelUser profile, int cityId, string userName, string password);
    }
}