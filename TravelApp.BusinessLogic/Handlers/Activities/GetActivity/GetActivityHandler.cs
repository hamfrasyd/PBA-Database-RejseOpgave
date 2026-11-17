using TravelApp.BusinessLogic.DTOs.Activities;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;

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
        public async Task<GetActivityResponse> GetActivityAsync(int id)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            ActivityDto? activityFromDb = await unitOfWork.Activities.GetActivityByIdAsync(id);
            if (activityFromDb is null)
            {
                throw new TravelAppNotFoundException($"Activity {id} was not found.");
            }

            GetActivityResponse response = new GetActivityResponse
            {
                Id = activityFromDb.Id,
                CityId = activityFromDb.CityId,
                Name = activityFromDb.Name,
                Description = activityFromDb.Description
            };

            return response;
        }
    }
}