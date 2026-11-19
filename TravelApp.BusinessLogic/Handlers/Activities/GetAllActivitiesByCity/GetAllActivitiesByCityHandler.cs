using TravelApp.BusinessLogic.DTOs.Activities;
using TravelApp.BusinessLogic.DTOs.Pagination;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Activities.GetAllActivitiesByCity
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
        /// <param name="pageNumber"="1">The page number for pagination, starting from 1.</param>
        /// <returns>The activities in the city.</returns>
        public async Task<PagedResultDTO<GetAllActivitiesByCityResponse>> GetActivitiesByCityIdAsync(int cityId, int pageNumber = 1)
        {
            if (pageNumber < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than or equal to 1.");
            }

            const int pageSize = 10; // Fixed page size for simplicity; can be made configurable if needed.

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            PagedResultDTO<ActivityDTO> activitiesFromDb = await unitOfWork.Activities.GetActivitiesAsync(cityId, pageNumber, pageSize);

            return new PagedResultDTO<GetAllActivitiesByCityResponse>
            {
                Items = activitiesFromDb.Items.Select(activity => new GetAllActivitiesByCityResponse
                {
                    Id = activity.Id,
                    CityId = activity.CityId,
                    Name = activity.Name,
                    Description = activity.Description
                   
                }).ToList(),

                TotalCount = activitiesFromDb.TotalCount,
                PageNumber = activitiesFromDb.PageNumber,
                PageSize = activitiesFromDb.PageSize
            };
        }

    }
}
