using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Posts;

namespace TravelApp.BusinessLogic.Handlers.Posts.GetPost
{
    /// <summary>Handles the get post operation.</summary>
    public class GetPostHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        /// <summary>Initializes a new GetPostHandler instance.</summary>
        public GetPostHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Retrieves post.</summary>
        public async Task<GetPostResponse> GetPostAsync(int id)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            Post? postFromDb = await unitOfWork.Posts.GetPostByIdAsync(id);

            if (postFromDb is null)
            {
                throw new TravelAppNotFoundException($"Post {id} was not found.");
            }

            if (postFromDb.User is null)
            {
                throw new TravelAppNotFoundException($"User for post {id} was not found.");
            }

            GetPostResponse response = new GetPostResponse
            {
                Id = postFromDb.Id,
                UserId = postFromDb.User.Id,
                Content = postFromDb.Content
            };

            return response;
        }
    }
}
