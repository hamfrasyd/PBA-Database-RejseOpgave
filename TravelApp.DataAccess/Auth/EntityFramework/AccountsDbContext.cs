using Microsoft.EntityFrameworkCore;
using TravelApp.BusinessLogic.Models.Users;

namespace TravelApp.DataAccess.Auth
{
    /// <summary>Maps local login accounts and the SQL profiles created during registration.</summary>
    public sealed class AccountsDbContext : DbContext
    {
        /// <summary> Initializes a new instance of the <see cref="AccountsDbContext"/> class. </summary>
        public AccountsDbContext(DbContextOptions<AccountsDbContext> options) : base(options)
        {
        }
    
        /// <summary>Gets the local login accounts.</summary>
        public DbSet<UserAccount> UserAccounts => Set<UserAccount>();

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TravelUser>(entity =>
            {
                entity.ToTable("User", "dbo");
                entity.HasKey(profile => profile.Id);
                entity.Property(profile => profile.Id).ValueGeneratedOnAdd();
                entity.Property(profile => profile.FirstName).HasMaxLength(128);
                entity.Property(profile => profile.LastName).HasMaxLength(128);
                entity.Property(profile => profile.Birthday).HasColumnType("date");
                entity.Property<int>("CityId").HasColumnName("City_Id");

                entity.Ignore(profile => profile.City);
                entity.Ignore(profile => profile.TripParticipants);
                entity.Ignore(profile => profile.Reviews);
                entity.Ignore(profile => profile.Posts);
                entity.Ignore(profile => profile.EarnedAchievements);
                entity.Ignore(profile => profile.ReceivedInvites);
                entity.Ignore(profile => profile.SentInvites);
                entity.Ignore(profile => profile.GroupMemberships);
            });

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

                entity.HasOne(account => account.Profile).WithOne().HasForeignKey<UserAccount>(account => account.TravelUserId).OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}