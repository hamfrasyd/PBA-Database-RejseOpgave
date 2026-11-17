using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Activities.DeleteActivity
{
    /// <summary>Handles the delete activity use case.</summary>
    public class DeleteActivityHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new DeleteActivityHandler instance.</summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        public DeleteActivityHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Deletes activity.</summary>
        /// <param name="resourceId">The activity identifier.</param>
        /// <exception cref="TravelAppNotFoundException">Thrown when the activity does not exist.</exception>
        public async Task DeleteActivityAsync(int resourceId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            bool wasDeleted = await unitOfWork.Activities.DeleteActivityAsync(resourceId);

            if (wasDeleted is false)
            {
                throw new TravelAppNotFoundException($"Activity {resourceId} was not found.");
            }

            await unitOfWork.CommitAsync();
        }
    }
}
