using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Posts.DeletePost
{
    ///<summary>Handles the delete post operation.</summary>
    public class DeletePostHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new CreatePostHandler instance.</summary>
        public DeletePostHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        ///<summary>Deletes a post.</summary>
        public async Task DeletePostAsync(int postId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            await unitOfWork.Posts.DeletePostAsync(postId);

            await unitOfWork.CommitAsync();
        }
    }
}
