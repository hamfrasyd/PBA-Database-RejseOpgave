using TravelApp.BusinessLogic.DTOs.Accounts;
using TravelApp.BusinessLogic.Models.Users;

namespace TravelApp.BusinessLogic.Interfaces
{
    /// <summary>Defines operations for local login accounts.</summary>
    public interface IUserAccountRepository
    {
        /// <summary>Creates a SQL user profile and its login account atomically.</summary>
        Task<int> CreateAsync(TravelUser profile, int cityId, CreateUserAccountDto account);
    }
}