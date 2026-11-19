using TravelApp.BusinessLogic.DTOs.Posts;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;

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

        /// <summary>Updates a post.</summary>
        /// <param name="request">The updated post details.</param>
        /// <exception cref="TravelAppValidationException">Thrown when the content is empty.</exception>
        /// <exception cref="TravelAppNotFoundException">Thrown when the post does not exist.</exception>
        public async Task UpdatePostAsync(UpdatePostRequest request)
        {
            if (request.Content is not null && string.IsNullOrWhiteSpace(request.Content))
            {
                throw new TravelAppValidationException("Post content cannot be empty.");
            }

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            UpdatePostDTO postToUpdate = new UpdatePostDTO
            {
                Id = request.Id,
                Content = request.Content?.Trim()
            };

            await unitOfWork.Posts.UpdatePostAsync(postToUpdate);
            await unitOfWork.CommitAsync();
        }
    }
}
