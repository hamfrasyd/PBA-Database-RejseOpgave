using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TravelApp.BusinessLogic.Auth;
using TravelApp.DataAccess.SQLRepositories.EntityFramework;

namespace TravelApp.DataAccess.Auth
{
    public sealed class SqlLoginAccountVerifier(
        TravelAppContext context,
        IPasswordHasher<UserAccount> passwordHasher) : ILoginAccountVerifier
    {
        public async Task<VerifiedLoginAccount?> VerifyAsync(
            string userName,
            string password)
        {
            string normalizedUserName = userName.Trim().ToUpperInvariant();

            UserAccount? account = await context.Set<UserAccount>()
                .AsNoTracking()
                .SingleOrDefaultAsync(item =>
                    item.NormalizedUserName == normalizedUserName);

            if (account is null ||
                passwordHasher.VerifyHashedPassword(
                    account, account.PasswordHash, password)
                    == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return new VerifiedLoginAccount(
                account.TravelUserId.ToString(CultureInfo.InvariantCulture),
                account.TravelUserId,
                [account.Role]);
        }
    }
}