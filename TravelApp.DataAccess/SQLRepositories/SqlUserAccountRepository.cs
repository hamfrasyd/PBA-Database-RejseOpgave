using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TravelApp.BusinessLogic.DTOs.Accounts;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Users;
using TravelApp.DataAccess.Auth;
using TravelApp.DataAccess.SQLRepositories.EntityFramework;

namespace TravelApp.DataAccess.SQLRepositories
{
    /// <summary>Stores and retrieves local login accounts in TravelDB.</summary>
    public sealed class SqlUserAccountRepository(TravelAppContext context, IPasswordHasher<UserAccount> passwordHasher) : IUserAccountRepository
    {
        /// <inheritdoc />
        public async Task AddAsync(TravelUser profile, CreateUserAccountDto account)
        {
            string userName = account.UserName.Trim();

            var entity = new UserAccount
            {
                Profile = profile,
                UserName = userName,
                NormalizedUserName = NormalizeUserName(userName),
                Role = "User"
            };

            entity.PasswordHash = passwordHasher.HashPassword(entity, account.Password);
            await context.Set<UserAccount>().AddAsync(entity);
        }

        /// <summary>Finds an account by its user name for login verification.</summary>
        internal Task<UserAccount?> FindByUserNameAsync(string userName)
        {
            string normalizedUserName = NormalizeUserName(userName);

            return context.Set<UserAccount>().AsNoTracking().SingleOrDefaultAsync(account => account.NormalizedUserName == normalizedUserName);
        }

        private static string NormalizeUserName(string userName)
        {
            return userName.Trim().ToUpperInvariant();
        }
    }
}