using Microsoft.EntityFrameworkCore;

namespace TravelApp.DataAccess.Auth
{
    /// <summary>Maps the local login accounts stored in SQL Server.</summary>
    public sealed class AccountsDbContext(DbContextOptions<AccountsDbContext> options) : DbContext(options)
    {
        /// <summary>Gets the local login accounts.</summary>
        public DbSet<UserAccount> UserAccounts => Set<UserAccount>();

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.ToTable("UserAccount", "dbo");
                entity.HasKey(account => account.TravelUserId);
                entity.Property(account => account.TravelUserId).ValueGeneratedNever();
                entity.Property(account => account.UserName).HasMaxLength(128);
                entity.Property(account => account.NormalizedUserName).HasMaxLength(128);
                entity.Property(account => account.PasswordHash).HasMaxLength(512);
                entity.Property(account => account.Role).HasMaxLength(16);
                entity.HasIndex(account => account.NormalizedUserName).IsUnique();

                // Login needs the profile ID from the account row.
                entity.Ignore(account => account.Profile);
            });
        }
    }
}