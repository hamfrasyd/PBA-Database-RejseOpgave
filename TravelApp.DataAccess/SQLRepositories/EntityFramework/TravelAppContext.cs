using Microsoft.EntityFrameworkCore;
using TravelApp.BusinessLogic.Models;
using TravelApp.BusinessLogic.Handlers.Destinations;
using TravelApp.BusinessLogic.Handlers.Users;

namespace TravelApp.DataAccess.SQLRepositories.EntityFramework
{
    /// <summary>Maps the SQL-first TravelDb schema. EF migrations do not own this database.</summary>
    public class TravelAppContext : DbContext
    {
        /// <summary>Initializes a new TravelAppContext instance.</summary>
        public TravelAppContext(DbContextOptions<TravelAppContext> options) : base(options)
        {
        }

        public DbSet<Continent> ContinentEntities => Set<Continent>();
        public DbSet<Country> CountryEntities => Set<Country>();
        public DbSet<City> CityEntities => Set<City>();
        public DbSet<Activity> ActivityEntities => Set<Activity>();
        public DbSet<Trip> TripEntities => Set<Trip>();
        public DbSet<TravelUser> TravelUserEntities => Set<TravelUser>();
        public DbSet<Review> ReviewEntities => Set<Review>();
        public DbSet<Post> PostEntities => Set<Post>();
        public DbSet<Friendship> FriendshipEntities => Set<Friendship>();
        public DbSet<Achievement> AchievementEntities => Set<Achievement>();
        public DbSet<InvitationStatus> InvitationStatusEntities => Set<InvitationStatus>();
        public DbSet<Invite> InviteEntities => Set<Invite>();
        public DbSet<TravelGroup> TravelGroupEntities => Set<TravelGroup>();
        public DbSet<TripParticipant> TripParticipantEntities => Set<TripParticipant>();
        public DbSet<TripStop> TripStopEntities => Set<TripStop>();
        public DbSet<TripActivity> TripActivityEntities => Set<TripActivity>();
        public DbSet<EarnedAchievement> EarnedAchievementEntities => Set<EarnedAchievement>();
        public DbSet<GroupMembership> GroupMembershipEntities => Set<GroupMembership>();
        public DbSet<GroupTrip> GroupTripEntities => Set<GroupTrip>();
        public DbSet<GetUserProfileResponse> UserProfiles => Set<GetUserProfileResponse>();
        public DbSet<GetDestinationStatsResponse> DestinationStatistics => Set<GetDestinationStatsResponse>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Continent>(entity =>
            {
                entity.ToTable("Continent", "dbo");
                entity.HasKey(item => item.Id);
                entity.Property(item => item.Name).HasMaxLength(64);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Country", "dbo");
                entity.HasKey(item => item.Id);
                entity.Property<int>("ContinentId").HasColumnName("Continent_Id").IsRequired();
                entity.Property(item => item.Name).HasMaxLength(64);
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("City", "dbo");
                entity.HasKey(item => item.Id);
                entity.Property<int>("CountryId").HasColumnName("Country_Id").IsRequired();
                entity.Property(item => item.Name).HasMaxLength(64);
            });

            modelBuilder.Entity<Activity>(entity =>
            {
                entity.ToTable("Activity", "dbo");
                entity.HasKey(item => item.Id);
                entity.Property<int>("CityId").HasColumnName("City_Id").IsRequired();
                entity.Property(item => item.Name).HasMaxLength(128);
                entity.Property(item => item.Description).HasMaxLength(1024);
            });

            modelBuilder.Entity<Trip>(entity =>
            {
                entity.ToTable("Trip", "dbo");
                entity.HasKey(item => item.Id);
                entity.Property(item => item.ExperienceNotes).HasMaxLength(1024);
                entity.Property(item => item.PlanNotes).HasMaxLength(1024);
            });

            modelBuilder.Entity<TravelUser>(entity =>
            {
                entity.ToTable("User", "dbo");
                entity.HasKey(item => item.Id);
                entity.Property(item => item.FirstName).HasMaxLength(128);
                entity.Property(item => item.LastName).HasMaxLength(128);
                entity.Property<int>("CityId").HasColumnName("City_Id").IsRequired();
                entity.Property(item => item.Birthday).HasColumnType("date");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("Review", "dbo");
                entity.HasKey(item => item.Id);
                entity.Property<int>("TripId").HasColumnName("Trip_Id").IsRequired();
                entity.Property<int>("UserId").HasColumnName("User_Id").IsRequired();
                entity.Property(item => item.Content).HasMaxLength(1024);
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("Post", "dbo");
                entity.HasKey(item => item.Id);
                entity.Property<int>("UserId").HasColumnName("User_Id").IsRequired();
                entity.Property(item => item.Content).HasMaxLength(1024);
            });

            modelBuilder.Entity<Friendship>(entity =>
            {
                entity.ToTable("Friendship", "dbo");
                entity.HasKey(item => item.Id);
                entity.Property<int>("UserAId").HasColumnName("UserA_Id").IsRequired();
                entity.Property<int>("UserBId").HasColumnName("UserB_Id").IsRequired();
                entity.Property<int>("UserLowId")
                    .HasColumnName("UserLow_Id")
                    .HasComputedColumnSql("CASE WHEN [UserA_Id] < [UserB_Id] THEN [UserA_Id] ELSE [UserB_Id] END", stored: true);
                entity.Property<int>("UserHighId")
                    .HasColumnName("UserHigh_Id")
                    .HasComputedColumnSql("CASE WHEN [UserA_Id] > [UserB_Id] THEN [UserA_Id] ELSE [UserB_Id] END", stored: true);
            });

            modelBuilder.Entity<Achievement>(entity =>
            {
                entity.ToTable("Achievement", "dbo");
                entity.HasKey(item => item.Id);
                entity.Property(item => item.Name).HasMaxLength(128);
                entity.Property(item => item.Description).HasMaxLength(1024);
            });

            modelBuilder.Entity<InvitationStatus>(entity =>
            {
                entity.ToTable("InvitationStatus", "dbo");
                entity.HasKey(item => item.Id);
                entity.Property(item => item.StatusName).HasMaxLength(64);
            });

            modelBuilder.Entity<Invite>(entity =>
            {
                entity.ToTable("Invite", "dbo");
                entity.HasKey(item => item.Id);
                entity.Property<int>("TripId").HasColumnName("Trip_Id").IsRequired();
                entity.Property<int>("ReceiverId").HasColumnName("Receiver_Id").IsRequired();
                entity.Property<int>("SenderId").HasColumnName("Sender_Id").IsRequired();
                entity.Property<int>("InvitationStatusId").HasColumnName("InvitationStatus_Id").IsRequired();
            });

            modelBuilder.Entity<TravelGroup>(entity =>
            {
                entity.ToTable("Group", "dbo");
                entity.HasKey(item => item.Id);
                entity.Property(item => item.Name).HasMaxLength(64);
                entity.Property(item => item.Description).HasMaxLength(64);
            });

            modelBuilder.Entity<TripParticipant>(entity =>
            {
                entity.ToTable("TripParticipant", "dbo");
                entity.HasKey("UserId", "TripId");
                entity.Property<int>("UserId").HasColumnName("User_Id").IsRequired();
                entity.Property<int>("TripId").HasColumnName("Trip_Id").IsRequired();
                entity.HasOne(item => item.Trip)
                    .WithMany(trip => trip.TripParticipants)
                    .HasForeignKey("TripId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TripStop>(entity =>
            {
                entity.ToTable("TripStop", "dbo");
                entity.HasKey("CityId", "TripId");
                entity.Property<int>("CityId").HasColumnName("City_Id").IsRequired();
                entity.Property<int>("TripId").HasColumnName("Trip_Id").IsRequired();
                entity.HasOne(item => item.Trip)
                    .WithMany(trip => trip.TripStops)
                    .HasForeignKey("TripId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TripActivity>(entity =>
            {
                entity.ToTable("TripActivity", "dbo");
                entity.HasKey("TripId", "ActivityId");
                entity.Property<int>("TripId").HasColumnName("Trip_Id").IsRequired();
                entity.Property<int>("ActivityId").HasColumnName("Activity_Id").IsRequired();
                entity.HasOne(item => item.Trip)
                    .WithMany(trip => trip.TripActivities)
                    .HasForeignKey("TripId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<EarnedAchievement>(entity =>
            {
                entity.ToTable("EarnedAchievement", "dbo");
                entity.HasKey("UserId", "AchievementId");
                entity.Property<int>("UserId").HasColumnName("User_Id").IsRequired();
                entity.Property<int>("AchievementId").HasColumnName("Achievement_Id").IsRequired();
            });

            modelBuilder.Entity<GroupMembership>(entity =>
            {
                entity.ToTable("GroupMembership", "dbo");
                entity.HasKey("UserId", "GroupId");
                entity.Property<int>("UserId").HasColumnName("User_Id").IsRequired();
                entity.Property<int>("GroupId").HasColumnName("Group_Id").IsRequired();
            });

            modelBuilder.Entity<GroupTrip>(entity =>
            {
                entity.ToTable("GroupTrip", "dbo");
                entity.HasKey("TripId", "GroupId");
                entity.Property<int>("TripId").HasColumnName("Trip_Id").IsRequired();
                entity.Property<int>("GroupId").HasColumnName("Group_Id").IsRequired();
            });


            // Explicit relationship mappings mirror the foreign keys in the SQL-first schema.
            modelBuilder.Entity<Country>()
                .HasOne(item => item.Continent)
                .WithMany(principal => principal.Countries)
                .HasForeignKey("ContinentId")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<City>()
                .HasOne(item => item.Country)
                .WithMany(principal => principal.Cities)
                .HasForeignKey("CountryId")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Activity>()
                .HasOne(item => item.City)
                .WithMany(principal => principal.Activities)
                .HasForeignKey("CityId")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TravelUser>()
                .HasOne(item => item.City)
                .WithMany()
                .HasForeignKey("CityId")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TripStop>()
                .HasOne(item => item.City)
                .WithMany()
                .HasForeignKey("CityId")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TripActivity>()
                .HasOne(item => item.Activity)
                .WithMany()
                .HasForeignKey("ActivityId")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TripParticipant>()
                .HasOne(item => item.User)
                .WithMany(principal => principal.TripParticipants)
                .HasForeignKey("UserId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(item => item.Trip)
                .WithMany(principal => principal.Reviews)
                .HasForeignKey("TripId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(item => item.User)
                .WithMany(principal => principal.Reviews)
                .HasForeignKey("UserId")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Post>()
                .HasOne(item => item.User)
                .WithMany(principal => principal.Posts)
                .HasForeignKey("UserId")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Friendship>()
                .HasOne(item => item.UserA)
                .WithMany()
                .HasForeignKey("UserAId")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Friendship>()
                .HasOne(item => item.UserB)
                .WithMany()
                .HasForeignKey("UserBId")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<EarnedAchievement>()
                .HasOne(item => item.User)
                .WithMany(principal => principal.EarnedAchievements)
                .HasForeignKey("UserId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EarnedAchievement>()
                .HasOne(item => item.Achievement)
                .WithMany()
                .HasForeignKey("AchievementId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Invite>()
                .HasOne(item => item.Trip)
                .WithMany(principal => principal.Invites)
                .HasForeignKey("TripId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Invite>()
                .HasOne(item => item.Receiver)
                .WithMany(principal => principal.ReceivedInvites)
                .HasForeignKey("ReceiverId")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Invite>()
                .HasOne(item => item.Sender)
                .WithMany(principal => principal.SentInvites)
                .HasForeignKey("SenderId")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Invite>()
                .HasOne(item => item.InvitationStatus)
                .WithMany()
                .HasForeignKey("InvitationStatusId")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<GroupMembership>()
                .HasOne(item => item.User)
                .WithMany(principal => principal.GroupMemberships)
                .HasForeignKey("UserId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GroupMembership>()
                .HasOne(item => item.Group)
                .WithMany(principal => principal.GroupMemberships)
                .HasForeignKey("GroupId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GroupTrip>()
                .HasOne(item => item.Trip)
                .WithMany()
                .HasForeignKey("TripId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GroupTrip>()
                .HasOne(item => item.Group)
                .WithMany(principal => principal.GroupTrips)
                .HasForeignKey("GroupId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GetUserProfileResponse>(entity => entity.HasNoKey().ToView("vw_UserProfile", "dbo"));
            modelBuilder.Entity<GetDestinationStatsResponse>(entity => entity.HasNoKey().ToView("vw_DestinationStats", "dbo"));
        }
    }
}
