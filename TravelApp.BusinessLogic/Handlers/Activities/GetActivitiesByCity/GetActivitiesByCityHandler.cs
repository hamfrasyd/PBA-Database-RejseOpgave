using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Destinations;

namespace TravelApp.BusinessLogic.Handlers.Activities.GetActivitiesByCity
{
    /// <summary>Handles the get city activities use case.</summary>
    public class GetActivitiesByCityHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new GetCityActivitiesHandler instance.</summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        public GetActivitiesByCityHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Retrieves city activities.</summary>
        /// <param name="cityId">The city identifier.</param>
        /// <returns>The activities in the city.</returns>
        /// <exception cref="InvalidOperationException">Thrown when an activity's city was not loaded.</exception>
        public async Task<List<GetActivitiesByCityResponse>> GetActivitiesByCityIdAsync(int cityId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            List<Activity> activitiesFromDb = await unitOfWork.Activities.GetActivitiesAsync(cityId);

            List<GetActivitiesByCityResponse> cityActivities = new List<GetActivitiesByCityResponse>();

            foreach (Activity activity in activitiesFromDb)
            {
                cityActivities.Add(new GetActivitiesByCityResponse
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
