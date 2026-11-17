using TravelApp.BusinessLogic.DTOs.Activities;
using TravelApp.BusinessLogic.Interfaces;

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
        public async Task<List<GetActivitiesByCityResponse>> GetActivitiesByCityIdAsync(int cityId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            List<ActivityDto> activitiesFromDb = await unitOfWork.Activities.GetActivitiesAsync(cityId);

            List<GetActivitiesByCityResponse> cityActivities = new List<GetActivitiesByCityResponse>();

            foreach (ActivityDto activity in activitiesFromDb)
            {
                cityActivities.Add(new GetActivitiesByCityResponse
                {
                    Id = activity.Id,
                    CityId = activity.CityId,
                    Name = activity.Name,
                    Description = activity.Description
                });
            }

            return cityActivities;
        }
    }
}