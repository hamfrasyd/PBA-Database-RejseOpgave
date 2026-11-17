using TravelApp.BusinessLogic.DTOs.Activities;
using TravelApp.BusinessLogic.DTOs.Locations;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Activities.UpdateActivity
{
    /// <summary>Handles the update activity use case.</summary>
    public class UpdateActivityHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new UpdateActivityHandler instance.</summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        public UpdateActivityHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Updates an activity and returns the persisted result.</summary>
        /// <param name="activityId">The activity identifier.</param>
        /// <param name="request">The updated activity details.</param>
        /// <returns>The updated activity.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the activity does not exist.</exception>
        /// <exception cref="TravelAppValidationException">Thrown when the name is missing or the city does not exist.</exception>
        public async Task<UpdateActivityResponse> UpdateActivityAsync(int activityId, UpdateActivityRequest request)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();
            
            bool cityExists = await unitOfWork.Locations.CityExistsAsync(request.CityId);
            if (cityExists is false)
            {
                throw new TravelAppValidationException($"City with ID {request.CityId} does not exist.");
            }

            ActivityDTO activityToUpdate = new ActivityDTO
            {
                Id = activityId,
                CityId = request.CityId,
                Name = request.Name.Trim(),
                Description = request.Description
            };

            bool activityWasFound = await unitOfWork.Activities.UpdateActivityAsync(activityToUpdate);
            if (activityWasFound is false)
            {
                throw new TravelAppNotFoundException($"Activity with ID {activityId} was not found.");
            }

            await unitOfWork.CommitAsync();

            UpdateActivityResponse response = new UpdateActivityResponse
            {
                Id = activityToUpdate.Id,
                CityId = activityToUpdate.CityId,
                Name = activityToUpdate.Name,
                Description = activityToUpdate.Description
            };

            return response;
        }
    }
}