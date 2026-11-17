using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models;
using TravelApp.BusinessLogic.Models.Posts;

namespace TravelApp.BusinessLogic.Handlers.Posts
{
    /// <summary>Handles the get posts operation.</summary>
    public class GetPostsHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        /// <summary>Initializes a new GetPostHandler instance.</summary>
        public GetPostsHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Retrieves posts for a specific user.</summary>
        public async Task<List<GetPostsResponse>> GetPostsByUserIdAsync(int userId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            List<Post> posts = await unitOfWork.Posts.GetPostsByUserIdAsync(userId);

            if (posts.Count == 0)
            {
                throw new TravelAppNotFoundException($"Post by user {userId} was not found.");
            }

            return posts
                .Select(post => new GetPostsResponse
                {
                    Id = post.Id,
                    UserId = post.User.Id,
                    Content = post.Content
                })
                .ToList();
        }
    }
}
