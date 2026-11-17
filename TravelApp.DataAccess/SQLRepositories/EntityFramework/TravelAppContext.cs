using Microsoft.EntityFrameworkCore;
using TravelApp.DataAccess.Audit;
using TravelApp.DataAccess.Auth;
using TravelApp.BusinessLogic.Models.Posts;
using TravelApp.BusinessLogic.Models.Groups;
using TravelApp.BusinessLogic.Models.Reviews;
using TravelApp.BusinessLogic.Models.Friendships;
using TravelApp.BusinessLogic.Models.Achievements;
using TravelApp.BusinessLogic.Models.Trips;
using TravelApp.BusinessLogic.Models.Invitations;
using TravelApp.BusinessLogic.Models.Users;
using TravelApp.BusinessLogic.Models.Activities;
using TravelApp.BusinessLogic.Models.Locations;

namespace TravelApp.DataAccess.SQLRepositories.EntityFramework
{
    /// <summary>Maps the SQL-first TravelDb schema. EF migrations do not own this database.</summary>
    public class TravelAppContext : DbContext
    {
        /// <summary>Initializes a new TravelAppContext instance.</summary>
        /// <param name="options">The Entity Framework options for this context.</param>
        public TravelAppContext(DbContextOptions<TravelAppContext> options) : base(options)
        {
        }

        /// <summary>Gets the continent entities.</summary>
        public DbSet<Continent> ContinentEntities => Set<Continent>();
        /// <summary>Gets the country entities.</summary>
        public DbSet<Country> CountryEntities => Set<Country>();
        /// <summary>Gets the city entities.</summary>
        public DbSet<City> CityEntities => Set<City>();
        /// <summary>Gets the activity entities.</summary>
        public DbSet<Activity> ActivityEntities => Set<Activity>();
        /// <summary>Gets the trip entities.</summary>
        public DbSet<Trip> TripEntities => Set<Trip>();
        /// <summary>Gets the travel user entities.</summary>
        public DbSet<TravelUser> TravelUserEntities => Set<TravelUser>();
        /// <summary>Gets the review entities.</summary>
        public DbSet<Review> ReviewEntities => Set<Review>();
        /// <summary>Gets the post entities.</summary>
        public DbSet<Post> PostEntities => Set<Post>();
        /// <summary>Gets the friendship entities.</summary>
        public DbSet<Friendship> FriendshipEntities => Set<Friendship>();
        /// <summary>Gets the achievement entities.</summary>
        public DbSet<Achievement> AchievementEntities => Set<Achievement>();
        /// <summary>Gets the invitation status entities.</summary>
        public DbSet<InvitationStatus> InvitationStatusEntities => Set<InvitationStatus>();
        /// <summary>Gets the invite entities.</summary>
        public DbSet<Invite> InviteEntities => Set<Invite>();
        /// <summary>Gets the travel group entities.</summary>
        public DbSet<TravelGroup> TravelGroupEntities => Set<TravelGroup>();
        /// <summary>Gets the trip participant entities.</summary>
        public DbSet<TripParticipant> TripParticipantEntities => Set<TripParticipant>();
        /// <summary>Gets the trip stop entities.</summary>
        public DbSet<TripStop> TripStopEntities => Set<TripStop>();
        /// <summary>Gets the trip activity entities.</summary>
        public DbSet<TripActivity> TripActivityEntities => Set<TripActivity>();
        /// <summary>Gets the earned achievement entities.</summary>
        public DbSet<EarnedAchievement> EarnedAchievementEntities => Set<EarnedAchievement>();
        /// <summary>Gets the group membership entities.</summary>
        public DbSet<GroupMembership> GroupMembershipEntities => Set<GroupMembership>();
        /// <summary>Gets the group trip entities.</summary>
        public DbSet<GroupTrip> GroupTripEntities => Set<GroupTrip>();
        /// <summary>Gets the review audit entities.</summary>
        public DbSet<ReviewAudit> ReviewAuditEntities => Set<ReviewAudit>();
        /// <summary>Gets the user profile views.</summary>
        public DbSet<UserProfileView> UserProfileViews => Set<UserProfileView>();
        /// <summary>Gets the destination statistics views.</summary>
        public DbSet<DestinationStatsView> DestinationStatsViews => Set<DestinationStatsView>();
        /// <summary>Gets the trip overview views.</summary>
        public DbSet<TripOverviewView> TripOverviewViews => Set<TripOverviewView>();
        /// <summary>Gets the pending invitation views.</summary>
        public DbSet<PendingInvitationView> PendingInvitationViews => Set<PendingInvitationView>();

        /// <summary>Configures the SQL schema mappings.</summary>
        /// <param name="modelBuilder">The model builder used to configure the entities.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Continent>(entity =>
            {
                entity.ToTable("Continent", "dbo");
                entity.HasKey(item => item.Id).HasName("PK_Continent");
                entity.Property(item => item.Name).HasMaxLength(64);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Country", "dbo");
                entity.HasKey(item => item.Id).HasName("PK_Country");
                entity.Property<int>("ContinentId").HasColumnName("Continent_Id").IsRequired();
                entity.Property(item => item.Name).HasMaxLength(64);
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("City", "dbo");
                entity.HasKey(item => item.Id).HasName("PK_City");
                entity.Property<int>("CountryId").HasColumnName("Country_Id").IsRequired();
                entity.Property(item => item.Name).HasMaxLength(64);
            });

            modelBuilder.Entity<Activity>(entity =>
            {
                entity.ToTable("Activity", "dbo");
                entity.HasKey(item => item.Id).HasName("PK_Activity");
                entity.Property<int>("CityId").HasColumnName("City_Id").IsRequired();
                entity.Property(item => item.Name).HasMaxLength(128);
                entity.Property(item => item.Description).HasMaxLength(1024);
            });

            modelBuilder.Entity<Trip>(entity =>
            {
                entity.ToTable("Trip", "dbo", table =>
                {
                    // EF Core 8.0+ supports the SQL Server OUTPUT clause for INSERT/UPDATE/DELETE operations. Bruges til tabeller med triggere
                    table.UseSqlOutputClause(false);
                    table.HasTrigger("trg_Trip_Achievements");
                    table.HasCheckConstraint(
                        "CK_Trip_DateRange",
                        "[StartDate] IS NULL OR [EndDate] IS NULL OR [EndDate] >= [StartDate]");
                });
                entity.HasKey(item => item.Id).HasName("PK_Trip");
                entity.Property(item => item.ExperienceNotes).HasMaxLength(1024);
                entity.Property(item => item.PlanNotes).HasMaxLength(1024);
            });

            modelBuilder.Entity<TravelUser>(entity =>
            {
                entity.ToTable("User", "dbo");
                entity.HasKey(item => item.Id).HasName("PK_User");
                entity.Property(item => item.FirstName).HasMaxLength(128);
                entity.Property(item => item.LastName).HasMaxLength(128);
                entity.Property<int>("CityId").HasColumnName("City_Id").IsRequired();
                entity.Property(item => item.Birthday).HasColumnType("date");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("Review", "dbo", table =>
                {
                    table.UseSqlOutputClause(false);
                    table.HasTrigger("trg_Review_Achievements");
                    table.HasTrigger("trg_Review_Audit");
                    table.HasCheckConstraint("CK_Review_Stars", "[Stars] BETWEEN 1 AND 6");
                });
                entity.HasKey(item => item.Id).HasName("PK_Review");
                entity.Property<int>("TripId").HasColumnName("Trip_Id").IsRequired();
                entity.Property<int>("UserId").HasColumnName("User_Id").IsRequired();
                entity.Property(item => item.Content).HasMaxLength(1024);
                entity.Property(item => item.Stars).HasColumnType("tinyint");
                entity.HasIndex("TripId")
                    .IsClustered(false)
                    .HasDatabaseName("IX_Review_TripId")
                    .IncludeProperties(review => review.Stars);
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("Post", "dbo");
                entity.HasKey(item => item.Id).HasName("PK_Post");
                entity.Property<int>("UserId").HasColumnName("User_Id").IsRequired();
                entity.Property(item => item.Content).HasMaxLength(1024);
            });

            modelBuilder.Entity<Friendship>(entity =>
            {
                entity.ToTable("Friendship", "dbo", table =>
                {
                    table.UseSqlOutputClause(false);
                    table.HasTrigger("trg_Friendship_Achievements");
                    table.HasCheckConstraint("CK_Friendship_DifferentUsers", "[UserA_Id] <> [UserB_Id]");
                });
                entity.HasKey(item => item.Id).HasName("PK_Friendship");
                entity.Property<int>("UserAId").HasColumnName("UserA_Id").IsRequired();
                entity.Property<int>("UserBId").HasColumnName("UserB_Id").IsRequired();
                entity.Property<int>("UserLowId")
                    .HasColumnName("UserLow_Id")
                    .HasComputedColumnSql("CASE WHEN [UserA_Id] < [UserB_Id] THEN [UserA_Id] ELSE [UserB_Id] END", stored: true);
                entity.Property<int>("UserHighId")
                    .HasColumnName("UserHigh_Id")
                    .HasComputedColumnSql("CASE WHEN [UserA_Id] > [UserB_Id] THEN [UserA_Id] ELSE [UserB_Id] END", stored: true);
                entity.HasIndex("UserLowId", "UserHighId")
                    .IsUnique()
                    .HasDatabaseName("UQ_Friendship_NormalizedUsers");
            });

            modelBuilder.Entity<Achievement>(entity =>
            {
                entity.ToTable("Achievement", "dbo");
                entity.HasKey(item => item.Id).HasName("PK_Achievement");
                entity.Property(item => item.Name).HasMaxLength(128);
                entity.Property(item => item.Description).HasMaxLength(1024);
                entity.HasIndex(item => item.Name)
                    .IsUnique()
                    .HasDatabaseName("UQ_Achievement_Name");
            });

            modelBuilder.Entity<InvitationStatus>(entity =>
            {
                entity.ToTable("InvitationStatus", "dbo");
                entity.HasKey(item => item.Id).HasName("PK_InvitationStatus");
                entity.Property(item => item.StatusName).HasMaxLength(64);
                entity.HasIndex(item => item.StatusName)
                    .IsUnique()
                    .HasDatabaseName("UQ_InvitationStatus_StatusName");
            });

            modelBuilder.Entity<Invite>(entity =>
            {
                entity.ToTable("Invite", "dbo", table =>
                {
                    table.HasCheckConstraint("CK_Invite_DifferentUsers", "[Sender_Id] <> [Receiver_Id]");
                });
                entity.HasKey(item => item.Id).HasName("PK_Invite");
                entity.Property<int>("TripId").HasColumnName("Trip_Id").IsRequired();
                entity.Property<int>("ReceiverId").HasColumnName("Receiver_Id").IsRequired();
                entity.Property<int>("SenderId").HasColumnName("Sender_Id").IsRequired();
                entity.Property<int>("InvitationStatusId").HasColumnName("InvitationStatus_Id").IsRequired();
                entity.HasIndex("InvitationStatusId", nameof(Invite.SendDate))
                    .IsClustered(false)
                    .HasDatabaseName("IX_Invite_Status_SendDate");
            });

            modelBuilder.Entity<TravelGroup>(entity =>
            {
                entity.ToTable("Group", "dbo");
                entity.HasKey(item => item.Id).HasName("PK_Group");
                entity.Property(item => item.Name).HasMaxLength(64);
                entity.Property(item => item.Description).HasMaxLength(64);
            });

            modelBuilder.Entity<TripParticipant>(entity =>
            {
                entity.ToTable("TripParticipant", "dbo", table =>
                {
                    table.UseSqlOutputClause(false);
                    table.HasTrigger("trg_TripParticipant_Achievements");
                });
                entity.HasKey("UserId", "TripId").HasName("PK_TripParticipant");
                entity.Property<int>("UserId").HasColumnName("User_Id").IsRequired();
                entity.Property<int>("TripId").HasColumnName("Trip_Id").IsRequired();
                entity.HasIndex("TripId", "UserId")
                    .IsClustered(false)
                    .HasDatabaseName("IX_TripParticipant_TripId");
                entity.HasOne(item => item.Trip)
                    .WithMany(trip => trip.TripParticipants)
                    .HasForeignKey("TripId")
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_TripParticipant_Trip_TripId");
            });

            modelBuilder.Entity<TripStop>(entity =>
            {
                entity.ToTable("TripStop", "dbo", table =>
                {
                    table.UseSqlOutputClause(false);
                    table.HasTrigger("trg_TripStop_Achievements");
                });
                entity.HasKey("CityId", "TripId").HasName("PK_TripStop");
                entity.Property<int>("CityId").HasColumnName("City_Id").IsRequired();
                entity.Property<int>("TripId").HasColumnName("Trip_Id").IsRequired();
                entity.HasIndex("TripId", "CityId")
                    .IsClustered(false)
                    .HasDatabaseName("IX_TripStop_TripId");
                entity.HasOne(item => item.Trip)
                    .WithMany(trip => trip.TripStops)
                    .HasForeignKey("TripId")
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_TripStop_Trip_TripId");
            });

            modelBuilder.Entity<TripActivity>(entity =>
            {
                entity.ToTable("TripActivity", "dbo");
                entity.HasKey("TripId", "ActivityId").HasName("PK_TripActivity");
                entity.Property<int>("TripId").HasColumnName("Trip_Id").IsRequired();
                entity.Property<int>("ActivityId").HasColumnName("Activity_Id").IsRequired();
                entity.HasOne(item => item.Trip)
                    .WithMany(trip => trip.TripActivities)
                    .HasForeignKey("TripId")
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_TripActivity_Trip_TripId");
            });

            modelBuilder.Entity<EarnedAchievement>(entity =>
            {
                entity.ToTable("EarnedAchievement", "dbo");
                entity.HasKey("UserId", "AchievementId").HasName("PK_EarnedAchievement");
                entity.Property<int>("UserId").HasColumnName("User_Id").IsRequired();
                entity.Property<int>("AchievementId").HasColumnName("Achievement_Id").IsRequired();
            });

            modelBuilder.Entity<GroupMembership>(entity =>
            {
                entity.ToTable("GroupMembership", "dbo");
                entity.HasKey("UserId", "GroupId").HasName("PK_GroupMembership");
                entity.Property<int>("UserId").HasColumnName("User_Id").IsRequired();
                entity.Property<int>("GroupId").HasColumnName("Group_Id").IsRequired();
            });

            modelBuilder.Entity<GroupTrip>(entity =>
            {
                entity.ToTable("GroupTrip", "dbo");
                entity.HasKey("TripId", "GroupId").HasName("PK_GroupTrip");
                entity.Property<int>("TripId").HasColumnName("Trip_Id").IsRequired();
                entity.Property<int>("GroupId").HasColumnName("Group_Id").IsRequired();
            });


            // Explicit relationship mappings mirror the foreign keys in the SQL-first schema.
            modelBuilder.Entity<Country>()
                .HasOne(item => item.Continent)
                .WithMany(principal => principal.Countries)
                .HasForeignKey("ContinentId")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Country_Continent_ContinentId");

            modelBuilder.Entity<City>()
                .HasOne(item => item.Country)
                .WithMany(principal => principal.Cities)
                .HasForeignKey("CountryId")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_City_Country_CountryId");

            modelBuilder.Entity<Activity>()
                .HasOne(item => item.City)
                .WithMany(principal => principal.Activities)
                .HasForeignKey("CityId")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Activity_City_CityId");

            modelBuilder.Entity<TravelUser>()
                .HasOne(item => item.City)
                .WithMany()
                .HasForeignKey("CityId")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_User_City_CityId");

            modelBuilder.Entity<TripStop>()
                .HasOne(item => item.City)
                .WithMany()
                .HasForeignKey("CityId")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_TripStop_City_CityId");

            modelBuilder.Entity<TripActivity>()
                .HasOne(item => item.Activity)
                .WithMany()
                .HasForeignKey("ActivityId")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_TripActivity_Activity_ActivityId");

            modelBuilder.Entity<TripParticipant>()
                .HasOne(item => item.User)
                .WithMany(principal => principal.TripParticipants)
                .HasForeignKey("UserId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TripParticipant_User_UserId");

            modelBuilder.Entity<Review>()
                .HasOne(item => item.Trip)
                .WithMany(principal => principal.Reviews)
                .HasForeignKey("TripId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Review_Trip_TripId");

            modelBuilder.Entity<Review>()
                .HasOne(item => item.User)
                .WithMany(principal => principal.Reviews)
                .HasForeignKey("UserId")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Review_User_UserId");

            modelBuilder.Entity<Post>()
                .HasOne(item => item.User)
                .WithMany(principal => principal.Posts)
                .HasForeignKey("UserId")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Post_User_UserId");

            modelBuilder.Entity<Friendship>()
                .HasOne(item => item.UserA)
                .WithMany()
                .HasForeignKey("UserAId")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Friendship_User_UserAId");

            modelBuilder.Entity<Friendship>()
                .HasOne(item => item.UserB)
                .WithMany()
                .HasForeignKey("UserBId")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Friendship_User_UserBId");

            modelBuilder.Entity<EarnedAchievement>()
                .HasOne(item => item.User)
                .WithMany(principal => principal.EarnedAchievements)
                .HasForeignKey("UserId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_EarnedAchievement_User_UserId");

            modelBuilder.Entity<EarnedAchievement>()
                .HasOne(item => item.Achievement)
                .WithMany()
                .HasForeignKey("AchievementId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_EarnedAchievement_Achievement_AchievementId");

            modelBuilder.Entity<Invite>()
                .HasOne(item => item.Trip)
                .WithMany(principal => principal.Invites)
                .HasForeignKey("TripId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Invite_Trip_TripId");

            modelBuilder.Entity<Invite>()
                .HasOne(item => item.Receiver)
                .WithMany(principal => principal.ReceivedInvites)
                .HasForeignKey("ReceiverId")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Invite_User_ReceiverId");

            modelBuilder.Entity<Invite>()
                .HasOne(item => item.Sender)
                .WithMany(principal => principal.SentInvites)
                .HasForeignKey("SenderId")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Invite_User_SenderId");

            modelBuilder.Entity<Invite>()
                .HasOne(item => item.InvitationStatus)
                .WithMany()
                .HasForeignKey("InvitationStatusId")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Invite_InvitationStatus_InvitationStatusId");

            modelBuilder.Entity<GroupMembership>()
                .HasOne(item => item.User)
                .WithMany(principal => principal.GroupMemberships)
                .HasForeignKey("UserId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_GroupMembership_User_UserId");

            modelBuilder.Entity<GroupMembership>()
                .HasOne(item => item.Group)
                .WithMany(principal => principal.GroupMemberships)
                .HasForeignKey("GroupId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_GroupMembership_Group_GroupId");

            modelBuilder.Entity<GroupTrip>()
                .HasOne(item => item.Trip)
                .WithMany()
                .HasForeignKey("TripId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_GroupTrip_Trip_TripId");

            modelBuilder.Entity<GroupTrip>()
                .HasOne(item => item.Group)
                .WithMany(principal => principal.GroupTrips)
                .HasForeignKey("GroupId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_GroupTrip_Group_GroupId");

            modelBuilder.Entity<UserProfileView>(entity => entity.HasNoKey().ToView("vw_UserProfile", "dbo"));

            modelBuilder.Entity<DestinationStatsView>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("vw_DestinationStats", "dbo");
                entity.Property(item => item.AvgStars).HasPrecision(3, 1);
            });

            modelBuilder.Entity<TripOverviewView>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("vw_TripOverview", "dbo");
                entity.Property(item => item.TripStatus).HasMaxLength(10);
                entity.Property(item => item.GroupName).HasMaxLength(64);
                entity.Property(item => item.Cities).HasColumnType("nvarchar(max)");
                entity.Property(item => item.AvgStars).HasPrecision(3, 1);
            });

            modelBuilder.Entity<PendingInvitationView>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("vw_PendingInvitations", "dbo");
                entity.Property(item => item.SenderName).HasMaxLength(257);
                entity.Property(item => item.ReceiverName).HasMaxLength(257);
            });

            modelBuilder.Entity<ReviewAudit>(entity =>
            {
                entity.ToTable("ReviewAudit", "dbo", table =>
                {
                    table.HasCheckConstraint(
                        "CK_ReviewAudit_ActionName",
                        "[ActionName] IN ('INSERT', 'UPDATE', 'DELETE')");
                });
                entity.HasKey(item => item.Id);
                entity.Property(item => item.ActionName).HasMaxLength(6).IsUnicode(false);
                entity.Property(item => item.ChangedAtUtc).HasColumnType("datetime2(7)");
                entity.Property(item => item.LoginName).HasColumnType("sysname");
                entity.Property(item => item.OldStars).HasColumnType("tinyint");
                entity.Property(item => item.NewStars).HasColumnType("tinyint");
            });

            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.ToTable("UserAccount", "dbo", table =>
                {
                    table.HasCheckConstraint("CK_UserAccount_Role", "[Role] IN (N'User', N'Admin')");
                });
                entity.HasKey(account => account.TravelUserId).HasName("PK_UserAccount");
                entity.Property(account => account.TravelUserId).ValueGeneratedNever();
                entity.Property(account => account.UserName).HasMaxLength(128);
                entity.Property(account => account.NormalizedUserName).HasMaxLength(128);
                entity.Property(account => account.PasswordHash).HasMaxLength(512);
                entity.Property(account => account.Role).HasMaxLength(16);
                entity.HasIndex(account => account.NormalizedUserName)
                    .IsUnique()
                    .HasDatabaseName("UQ_UserAccount_NormalizedUserName");
                entity.HasOne<TravelUser>()
                    .WithOne()
                    .HasForeignKey<UserAccount>(account => account.TravelUserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("FK_UserAccount_User");
            });
        }
    }
}
