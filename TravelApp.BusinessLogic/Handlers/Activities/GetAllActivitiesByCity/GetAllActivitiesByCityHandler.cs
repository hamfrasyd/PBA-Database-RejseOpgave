using TravelApp.BusinessLogic.DTOs.Activities;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Activities.GetActivitiesByCity
{
    /// <summary>Handles the get city activities use case.</summary>
    public class GetAllActivitiesByCityHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new GetAllActivitiesByCityHandler instance.</summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        public GetAllActivitiesByCityHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Retrieves city activities.</summary>
        /// <param name="cityId">The city identifier.</param>
        /// <returns>The activities in the city.</returns>
        public async Task<List<GetAllActivitiesByCityResponse>> GetActivitiesByCityIdAsync(int cityId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            List<ActivityDTO> activitiesFromDb = await unitOfWork.Activities.GetActivitiesAsync(cityId);

            List<GetAllActivitiesByCityResponse> cityActivities = activitiesFromDb.Select(activity => new GetAllActivitiesByCityResponse
            {
                Id = activity.Id,
                CityId = activity.CityId,
                Name = activity.Name,
                Description = activity.Description
            }).ToList();

            return cityActivities;
        }
    }
}