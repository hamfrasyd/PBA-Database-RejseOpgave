using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Groups.DeleteGroup
{
    /// <summary>Handles the delete group operation.</summary>
    public class DeleteGroupHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new DeleteGroupHandler instance.</summary>
        public DeleteGroupHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Deletes a group.</summary>
        public async Task DeleteGroupAsync(int groupId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            await unitOfWork.Groups.DeleteGroupAsync(groupId);

            await unitOfWork.CommitAsync();
        }
    }
}