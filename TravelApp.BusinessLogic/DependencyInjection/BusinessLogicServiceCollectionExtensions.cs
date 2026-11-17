using Microsoft.Extensions.DependencyInjection;
using TravelApp.BusinessLogic.DatabaseSelection;
using TravelApp.BusinessLogic.Handlers.Trips;
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
using TravelApp.BusinessLogic.Handlers.Locations.DeleteCity;
using TravelApp.BusinessLogic.Handlers.Locations.GetCities;
using TravelApp.BusinessLogic.Handlers.Locations.GetCity;
using TravelApp.BusinessLogic.Handlers.Locations.GetContinents;
using TravelApp.BusinessLogic.Handlers.Locations.GetCountries;
using TravelApp.BusinessLogic.Handlers.Locations.GetDestinationStats;
using TravelApp.BusinessLogic.Handlers.Locations.GetVisitedCountries;
using TravelApp.BusinessLogic.Handlers.Locations.UpdateCity;
using TravelApp.BusinessLogic.Handlers.Users.DeleteUser;
using TravelApp.BusinessLogic.Handlers.Users.GetUserProfile;
using TravelApp.BusinessLogic.Handlers.Users.GetUserProfiles;
using TravelApp.BusinessLogic.Handlers.Users.UpdateUser;


namespace TravelApp.BusinessLogic.DependencyInjection
{
    /// <summary>Registers application handlers grouped by feature.</summary>
    public static class BusinessLogicServiceCollectionExtensions
    {
        /// <summary>Adds business logic.</summary>
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
            services.AddScoped<GetActivitiesByCityHandler>();
            services.AddScoped<UpdateActivityHandler>();
        }

        private static void RegisterLocations(IServiceCollection services)
        {
            services.AddScoped<CreateCityHandler>();
            services.AddScoped<DeleteCityHandler>();
            services.AddScoped<GetCitiesHandler>();
            services.AddScoped<GetCityHandler>();
            services.AddScoped<GetContinentsHandler>();
            services.AddScoped<GetCountriesHandler>();
            services.AddScoped<GetCityStatsHandler>();
            services.AddScoped<GetVisitedCountriesHandler>();
            services.AddScoped<UpdateCityHandler>();
        }
        private static void RegisterUsers(IServiceCollection services)
        {
            services.AddScoped<DeleteUserHandler>();
            services.AddScoped<GetUserProfileHandler>();
            services.AddScoped<GetUserProfilesHandler>();
            services.AddScoped<UpdateUserHandler>();
        }

        private static void RegisterTrips(IServiceCollection services)
        {
            services.AddScoped<CreateTripHandler>();
            services.AddScoped<GetTripHandler>();
            services.AddScoped<GetTripsHandler>();
            services.AddScoped<UpdateTripHandler>();
            services.AddScoped<DeleteTripHandler>();
            services.AddScoped<ManageTripRelationsHandler>();

            services.AddScoped<GetTripActivitiesHandler>();
            services.AddScoped<GetTripParticipantsHandler>();
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
