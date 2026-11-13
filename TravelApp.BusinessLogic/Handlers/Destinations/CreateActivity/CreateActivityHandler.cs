using TravelApp.BusinessLogic.Models;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Destinations
{
    /// <summary>Handles the create activity use case.</summary>
    public class CreateActivityHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new CreateActivityHandler instance.</summary>
        public CreateActivityHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Creates activity.</summary>
        public async Task<int> CreateActivityAsync(CreateActivityRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new TravelAppValidationException("Activity name is required.");
            }

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            City? cityFromDb = await unitOfWork.Destinations.GetCityByIdAsync(request.CityId);

            if (cityFromDb is null)
            {
                throw new TravelAppValidationException("The parent destination does not exist.");
            }

            Activity createdActivity = new Activity
            {
                Name = request.Name.Trim(),
                City = cityFromDb,
                Description = request.Description
            };

            await unitOfWork.Destinations.AddActivityAsync(createdActivity);

            await unitOfWork.CommitAsync();

            return createdActivity.Id;
        }
    }
}
