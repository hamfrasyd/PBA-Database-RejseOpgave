using TravelApp.BusinessLogic.DTOs.Accounts;
using TravelApp.BusinessLogic.Models.Users;

namespace TravelApp.BusinessLogic.Interfaces
{
    /// <summary>Defines operations for local login accounts.</summary>
    public interface IUserAccountRepository
    {
        /// <summary>Stages a login account for the next commit.</summary>
        Task AddAsync(TravelUser profile, CreateUserAccountDto account);
    }
}