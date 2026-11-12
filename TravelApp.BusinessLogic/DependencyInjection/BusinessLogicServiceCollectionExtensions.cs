using Microsoft.Extensions.DependencyInjection;
using TravelApp.BusinessLogic.DatabaseSelection;
using TravelApp.BusinessLogic.Handlers.DatabaseSelection;
using TravelApp.BusinessLogic.Interfaces;


namespace TravelApp.BusinessLogic.DependencyInjection
{
    /// <summary>Registers application handlers grouped by feature.</summary>
    public static class BusinessLogicServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            RegisterDatabaseSelection(services);

            return services;
        }

        private static void RegisterDatabaseSelection(IServiceCollection services)
        {
            services.AddScoped<SelectedDatabaseProvider>();
            services.AddScoped<GetDatabaseProvidersHandler>();
        }
    }
}
