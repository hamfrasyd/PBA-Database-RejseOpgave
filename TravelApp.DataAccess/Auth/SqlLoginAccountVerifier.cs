using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TravelApp.BusinessLogic.Auth;

namespace TravelApp.DataAccess.Auth
{
    /// <summary>Verifies user login credentials against accounts stored in SQL.</summary>
    public sealed class SqlLoginAccountVerifier(AccountsDbContext context, IPasswordHasher<UserAccount> passwordHasher) : ILoginAccountVerifier
    {
        /// <summary>Verifies login credentials and returns the associated account details.</summary>
        public async Task<VerifiedLoginAccount?> VerifyAsync(string userName, string password)
        {
            string normalizedUserName = userName.Trim().ToUpperInvariant();

            UserAccount? account = await context.UserAccounts.AsNoTracking().SingleOrDefaultAsync(item => item.NormalizedUserName == normalizedUserName);

            if (account is null || passwordHasher.VerifyHashedPassword(account, account.PasswordHash, password) == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return new VerifiedLoginAccount(account.TravelUserId.ToString(CultureInfo.InvariantCulture), account.TravelUserId, [account.Role]);
        }
    }
}