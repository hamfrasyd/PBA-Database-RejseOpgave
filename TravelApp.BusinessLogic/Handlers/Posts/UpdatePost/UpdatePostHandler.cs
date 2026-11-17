using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Posts;

namespace TravelApp.BusinessLogic.Handlers.Posts.UpdatePost
{
    /// <summary>Handles the update post operation.</summary>
    public class UpdatePostHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new UpdatePostHandler instance.</summary>
        public UpdatePostHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Updates a post</summary>
        public async Task UpdatePostAsync(UpdatePostRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Content))
            {
                throw new TravelAppValidationException("Invalid post data");
            }

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            Post? postInDb = await unitOfWork.Posts.GetPostByIdAsync(request.Id);

            if (postInDb is null)
            {
                throw new TravelAppNotFoundException($"Post {request.Id} was not found.");
            }

            postInDb.Id = request.Id;
            postInDb.Content = request.Content;

            await unitOfWork.Posts.UpdatePostAsync(postInDb);
            await unitOfWork.CommitAsync();
        }
    }
}
