using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Destinations;

namespace TravelApp.BusinessLogic.Handlers.Activities.GetActivity
{
    /// <summary>Handles the get activity use case.</summary>
    public class GetActivityHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        /// <summary>Initializes a new GetActivityHandler instance.</summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        public GetActivityHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Retrieves activity.</summary>
        /// <param name="id">The activity identifier.</param>
        /// <returns>The activity.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the activity does not exist.</exception>
        /// <exception cref="TravelAppValidationException">Thrown when the activity has no city.</exception>
        public async Task<GetActivityResponse> GetActivityAsync(int id)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            Activity? activityFromDb = await unitOfWork.Activities.GetActivityByIdAsync(id);

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
