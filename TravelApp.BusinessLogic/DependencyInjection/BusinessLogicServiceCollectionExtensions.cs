using Microsoft.Extensions.DependencyInjection;
using TravelApp.BusinessLogic.DatabaseSelection;
using TravelApp.BusinessLogic.Handlers.Achievements;
using TravelApp.BusinessLogic.Handlers.DatabaseSelection;
using TravelApp.BusinessLogic.Handlers.Destinations;
using TravelApp.BusinessLogic.Interfaces;


namespace TravelApp.BusinessLogic.DependencyInjection
{
    /// <summary>Registers application handlers grouped by feature.</summary>
    public static class BusinessLogicServiceCollectionExtensions
    {
        /// <summary>Adds business logic.</summary>
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            RegisterDatabaseSelection(services);
            RegisterAchievements(services);
            RegisterDestinations(services);
            return services;
        }

        private static void RegisterDatabaseSelection(IServiceCollection services)
        {
            services.AddScoped<SelectedDatabaseProvider>();
            services.AddScoped<GetDatabaseProvidersHandler>();
        }
        private static void RegisterAchievements(IServiceCollection services)
        {
            services.AddScoped<GetAchievementDefinitionsHandler>();
        }

        private static void RegisterDestinations(IServiceCollection services)
        {
            services.AddScoped<CreateActivityHandler>();
            services.AddScoped<CreateCityHandler>();
            services.AddScoped<DeleteActivityHandler>();
            services.AddScoped<DeleteCityHandler>();
            services.AddScoped<GetActivityHandler>();
            services.AddScoped<GetCitiesHandler>();
            services.AddScoped<GetCityActivitiesHandler>();
            services.AddScoped<GetCityHandler>();
            services.AddScoped<GetContinentsHandler>();
            services.AddScoped<GetCountriesHandler>();
            services.AddScoped<GetDestinationStatsHandler>();
            services.AddScoped<UpdateActivityHandler>();
            services.AddScoped<UpdateCityHandler>();

        }
    }
}
