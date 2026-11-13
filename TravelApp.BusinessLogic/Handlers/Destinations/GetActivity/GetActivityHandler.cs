using TravelApp.BusinessLogic.Models;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Destinations
{
    /// <summary>Handles the get activity use case.</summary>
    public class GetActivityHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        /// <summary>Initializes a new GetActivityHandler instance.</summary>
        public GetActivityHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Retrieves activity.</summary>
        public async Task<GetActivityResponse> GetActivityAsync(int id)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            Activity? activityFromDb = await unitOfWork.Destinations.GetActivityByIdAsync(id);

            if (activityFromDb is null)
            {
                throw new TravelAppNotFoundException($"Activity {id} was not found.");
            }

            if (activityFromDb.City is null)
            {
                throw new TravelAppValidationException($"Activity {id} must have a City.");
            }

            GetActivityResponse response = new GetActivityResponse
            { 
                Id = activityFromDb.Id,
                CityId = activityFromDb.City.Id,
                Name = activityFromDb.Name,
                Description = activityFromDb.Description
            };

            return response;
        }
    }
}
