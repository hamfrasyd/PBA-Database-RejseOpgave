using Microsoft.Extensions.DependencyInjection;
using TravelApp.BusinessLogic.DatabaseSelection;
using TravelApp.BusinessLogic.Handlers.Trips.GetTrip;
using TravelApp.BusinessLogic.Handlers.Accounts.Login;
using TravelApp.BusinessLogic.Handlers.Achievements.GetAchievementDefinitions;
using TravelApp.BusinessLogic.Handlers.Achievements.GetEarnedAchievements;
using TravelApp.BusinessLogic.Handlers.Activities.CreateActivity;
using TravelApp.BusinessLogic.Handlers.Activities.DeleteActivity;
using TravelApp.BusinessLogic.Handlers.Activities.GetActivitiesByCity;
using TravelApp.BusinessLogic.Handlers.Activities.GetActivity;
using TravelApp.BusinessLogic.Handlers.Activities.UpdateActivity;
using TravelApp.BusinessLogic.Handlers.DatabaseSelection.GetDatabaseProviders;
using TravelApp.BusinessLogic.Handlers.Locations.CreateCity;
using TravelApp.BusinessLogic.Handlers.Locations.GetCities;
using TravelApp.BusinessLogic.Handlers.Locations.GetCity;
using TravelApp.BusinessLogic.Handlers.Locations.GetContinents;
using TravelApp.BusinessLogic.Handlers.Locations.GetCountries;
using TravelApp.BusinessLogic.Handlers.Locations.GetCityStats;
using TravelApp.BusinessLogic.Handlers.Locations.GetVisitedCountries;
using TravelApp.BusinessLogic.Handlers.Users.DeleteUser;
using TravelApp.BusinessLogic.Handlers.Users.GetUserProfile;
using TravelApp.BusinessLogic.Handlers.Users.GetAllUserProfiles;
using TravelApp.BusinessLogic.Handlers.Users.UpdateUser;
using TravelApp.BusinessLogic.Handlers.Trips.TripCity;
using TravelApp.BusinessLogic.Handlers.Trips.TripParticipants;
using TravelApp.BusinessLogic.Handlers.Trips.CreateTrip;
using TravelApp.BusinessLogic.Handlers.Trips.DeleteTrip;
using TravelApp.BusinessLogic.Handlers.Trips.UpdateTrip;
using TravelApp.BusinessLogic.Handlers.Trips.TripActivities;
using TravelApp.BusinessLogic.Handlers.Posts.UpdatePost;
using TravelApp.BusinessLogic.Handlers.Posts.CreatePost;
using TravelApp.BusinessLogic.Handlers.Posts.GetPosts;
using TravelApp.BusinessLogic.Handlers.Posts.GetPost;
using TravelApp.BusinessLogic.Handlers.Posts.DeletePost;

namespace TravelApp.BusinessLogic.DependencyInjection
{
    /// <summary>Registers application handlers grouped by feature.</summary>
    public static class BusinessLogicServiceCollectionExtensions
    {
        /// <summary>Adds business logic services to the service collection.</summary>
        /// <param name="services">The service collection to register handlers in.</param>
        /// <returns>The same service collection, so registration can be chained in Program.cs.</returns>
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            RegisterDatabaseSelection(services);
            RegisterAccounts(services);
            RegisterAchievements(services);
            RegisterActivities(services);
            RegisterLocations(services);
            RegisterUsers(services);
            RegisterTrips(services);
            RegisterPosts(services);

            return services;
        }

        private static void RegisterDatabaseSelection(IServiceCollection services)
        {
            services.AddScoped<SelectedDatabaseProvider>();
            services.AddScoped<GetDatabaseProvidersHandler>();
        }

        private static void RegisterAccounts(IServiceCollection services)
        {
            services.AddScoped<LoginHandler>();
        }

        private static void RegisterAchievements(IServiceCollection services)
        {
            services.AddScoped<GetAchievementDefinitionsHandler>();
            services.AddScoped<GetEarnedAchievementsHandler>();
        }
        private static void RegisterActivities(IServiceCollection services)
        {
            services.AddScoped<CreateActivityHandler>();
            services.AddScoped<DeleteActivityHandler>();
            services.AddScoped<GetActivityHandler>();
            services.AddScoped<GetAllActivitiesByCityHandler>();
            services.AddScoped<UpdateActivityHandler>();
        }

        private static void RegisterLocations(IServiceCollection services)
        {
            services.AddScoped<CreateCityHandler>();
            services.AddScoped<GetCitiesHandler>();
            services.AddScoped<GetCityHandler>();
            services.AddScoped<GetContinentsHandler>();
            services.AddScoped<GetCountriesHandler>();
            services.AddScoped<GetCityStatsHandler>();
            services.AddScoped<GetVisitedCountriesHandler>();
        }
        private static void RegisterUsers(IServiceCollection services)
        {
            services.AddScoped<DeleteUserHandler>();
            services.AddScoped<GetUserProfileHandler>();
            services.AddScoped<GetAllUserProfilesHandler>();
            services.AddScoped<UpdateUserHandler>();
        }

        private static void RegisterTrips(IServiceCollection services)
        {
            services.AddScoped<CreateTripHandler>();
            services.AddScoped<GetTripHandler>();
            services.AddScoped<GetTripsHandler>();
            services.AddScoped<UpdateTripHandler>();
            services.AddScoped<DeleteTripHandler>();
            services.AddScoped<TripActivitiesHandler>();
            services.AddScoped<TripCityHandler>();
            services.AddScoped<TripParticipantsHandler>();
        }

        private static void RegisterPosts(IServiceCollection services)
        {
            services.AddScoped<GetPostsHandler>();
            services.AddScoped<GetPostHandler>();
            services.AddScoped<CreatePostHandler>();
            services.AddScoped<DeletePostHandler>();
            services.AddScoped<UpdatePostHandler>();
        }
    }
}
