using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TravelApp.BusinessLogic.Auth;

namespace TravelApp.DataAccess.Auth
{
    /// <summary>Verifies user login credentials against accounts stored in SQL.</summary>
    /// <summary>Verifies user login credentials against accounts stored in SQL.</summary>
    public sealed class SqlLoginAccountVerifier : ILoginAccountVerifier
    {
        private readonly AccountsDbContext _context;
        private readonly IPasswordHasher<UserAccount> _passwordHasher;

        /// <summary>
        /// Initializes a new instance of the SqlLoginAccountVerifier class.
        /// </summary>
        /// <param name="context">The database context used to access user accounts.</param>
        /// <param name="passwordHasher">The password hasher used to verify user account passwords.</param>
        public SqlLoginAccountVerifier(AccountsDbContext context, IPasswordHasher<UserAccount> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        /// <summary>Verifies login credentials and returns the associated account details.</summary>
        public async Task<VerifiedLoginAccount?> VerifyAsync(string userName, string password)
        {
            string normalizedUserName = userName.Trim().ToUpperInvariant();

            UserAccount? account = await _context.UserAccounts.AsNoTracking().SingleOrDefaultAsync(item => item.NormalizedUserName == normalizedUserName && item.IsActive);

            if (account is null || _passwordHasher.VerifyHashedPassword(account, account.PasswordHash, password) == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return new VerifiedLoginAccount(account.TravelUserId.ToString(CultureInfo.InvariantCulture), account.TravelUserId, [account.Role]);
        }
    }
}