using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models;

namespace TravelApp.BusinessLogic.Handlers.Posts
{
    public class CreatePostHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new CreatePostHandler instance.</summary>
        public CreatePostHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Creates post.</summary>
        public async Task<CreatePostResponse> CreatePostAsync(CreatePostRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Content) || request.UserId <= 0)
            {
                throw new TravelAppValidationException("Invalid post data.");
            }

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            TravelUser user = await unitOfWork.Users.GetByIdAsync(request.UserId);

            Post addedPost = await unitOfWork.Posts.AddPostAsync(new Post
            {
                Content = request.Content.Trim(),
                User = user
            });

            await unitOfWork.CommitAsync();

            return new CreatePostResponse { Id = addedPost.Id, Content = addedPost.Content, UserId = addedPost.User.Id };
        }
    }
}
