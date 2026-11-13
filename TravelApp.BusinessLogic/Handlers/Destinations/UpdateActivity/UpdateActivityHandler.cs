using TravelApp.BusinessLogic.Models;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Destinations
{
    /// <summary>Handles the update activity use case.</summary>
    public class UpdateActivityHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new UpdateActivityHandler instance.</summary>
        public UpdateActivityHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Updates activity.</summary>
        public async Task UpdateActivityAsync(UpdateActivityRequest request)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            Activity? activityromDbToUpdate = await unitOfWork.Destinations.GetActivityByIdAsync(request.ActivityId);

            if (activityromDbToUpdate is null)
            {
                throw new TravelAppNotFoundException($"Activity {request.ActivityId} was not found.");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new TravelAppValidationException("Activity name is required.");
            }

            City? cityFromDb = await unitOfWork.Destinations.GetCityByIdAsync(request.CityId);

            if (cityFromDb is null)
            {
                throw new TravelAppValidationException("The parent destination does not exist.");
            }

            activityromDbToUpdate.Name = request.Name.Trim();
            activityromDbToUpdate.City = cityFromDb;
            activityromDbToUpdate.Description = request.Description;

            await unitOfWork.Destinations.UpdateActivityAsync(activityromDbToUpdate);

            await unitOfWork.CommitAsync();
        }
    }
}
