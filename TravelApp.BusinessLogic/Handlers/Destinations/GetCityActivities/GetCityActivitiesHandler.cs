using TravelApp.BusinessLogic.Models;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Destinations
{
    /// <summary>Handles the get city activities use case.</summary>
    public class GetCityActivitiesHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new GetCityActivitiesHandler instance.</summary>
        public GetCityActivitiesHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Retrieves city activities.</summary>
        public async Task<List<GetCityActivitiesResponse>> GetCityActivitiesAsync(int cityId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            List<Activity> activitiesFromDb = await unitOfWork.Destinations.GetActivitiesForCityAsync(cityId);

            List<GetCityActivitiesResponse> cityActivities = new List<GetCityActivitiesResponse>();

            foreach (Activity activity in activitiesFromDb)
            {
                cityActivities.Add(new GetCityActivitiesResponse
                {
                    Id = activity.Id,
                    CityId = activity.City?.Id ?? throw new InvalidOperationException("Required City relationship was not loaded."),
                    Name = activity.Name,
                    Description = activity.Description
                });
            }

            return cityActivities;
        }
    }
}
