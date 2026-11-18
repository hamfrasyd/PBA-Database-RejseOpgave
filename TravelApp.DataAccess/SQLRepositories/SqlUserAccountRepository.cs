using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TravelApp.BusinessLogic.DTOs.Accounts;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Users;
using TravelApp.DataAccess.Auth;

namespace TravelApp.DataAccess.SQLRepositories
{
    /// <summary>Creates local login accounts and their profiles in SQL Server.</summary>
    public sealed class SqlUserAccountRepository : IUserAccountRepository
    {
        private readonly AccountsDbContext _context;
        private readonly IPasswordHasher<UserAccount> _passwordHasher;

        /// <summary> Initializes a new instance of the <see cref="SqlUserAccountRepository"/> class. </summary>
        public SqlUserAccountRepository(AccountsDbContext context,IPasswordHasher<UserAccount> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }
   
        /// <inheritdoc />
        public async Task<int> CreateAsync(TravelUser profile, int cityId, CreateUserAccountDto account)
        {
            string userName = account.UserName.Trim();

            var entity = new UserAccount
            {
                Profile = profile,
                UserName = userName,
                NormalizedUserName = userName.ToUpperInvariant(),
                Role = "User"
            };

            entity.PasswordHash = _passwordHasher.HashPassword(entity, account.Password);

            _context.Add(profile).Property<int>("CityId").CurrentValue = cityId;
            _context.UserAccounts.Add(entity);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException exception) when (IsDuplicateUserName(exception))
            {
                throw new TravelAppConflictException("Brugernavnet er allerede optaget.");
            }

            return profile.Id;
        }

        private static bool IsDuplicateUserName(DbUpdateException exception)
        {
            if (exception.InnerException is not SqlException sqlException)
            {
                return false;
            }

            // SQL Server uses these error numbers for duplicate unique values.
            bool isDuplicateValue = sqlException.Number == 2601 || sqlException.Number == 2627;

            // Only translate conflicts involving the unique user name.
            bool concernsUserName = sqlException.Message.Contains("UQ_UserAccount_NormalizedUserName", StringComparison.Ordinal);

            return isDuplicateValue && concernsUserName;
        }
    }
}