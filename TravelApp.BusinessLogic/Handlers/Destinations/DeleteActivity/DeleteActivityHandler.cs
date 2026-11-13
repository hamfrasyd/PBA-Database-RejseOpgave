using TravelApp.BusinessLogic.Models;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Destinations
{
    /// <summary>Handles the delete activity use case.</summary>
    public class DeleteActivityHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new DeleteActivityHandler instance.</summary>
        public DeleteActivityHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Deletes activity.</summary>
        public async Task DeleteActivityAsync(int resourceId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            bool wasDeleted = await unitOfWork.Destinations.DeleteActivityAsync(resourceId);

            if (wasDeleted is false)
            {
                throw new TravelAppNotFoundException($"Activity {resourceId} was not found.");
            }

            await unitOfWork.CommitAsync();
        }
    }
}
