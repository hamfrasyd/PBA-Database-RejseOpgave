using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Destinations;

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

        /// <summary>Updates activity.</summary>
        /// <param name="activityId">The activity identifier.</param>
        /// <param name="request">The updated activity details.</param>
        /// <exception cref="TravelAppNotFoundException">Thrown when the activity does not exist.</exception>
        /// <exception cref="TravelAppValidationException">Thrown when the name is missing or the city does not exist.</exception>
        public async Task UpdateActivityAsync(int activityId, UpdateActivityRequest request)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            Activity? activityromDbToUpdate = await unitOfWork.Activities.GetActivityByIdAsync(activityId);

            if (activityromDbToUpdate is null)
            {
                throw new TravelAppNotFoundException($"Activity with ID {activityId} was not found.");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new TravelAppValidationException("Activity name is required.");
            }

            City? cityFromDb = await unitOfWork.Locations.GetCityByIdAsync(request.CityId);

            if (cityFromDb is null)
            {
                throw new TravelAppValidationException("The parent destination does not exist.");
            }

            activityromDbToUpdate.Name = request.Name.Trim();
            activityromDbToUpdate.City = cityFromDb;
            activityromDbToUpdate.Description = request.Description;

            await unitOfWork.Activities.UpdateActivityAsync(activityId, request);

            await unitOfWork.CommitAsync();
        }
    }
}
