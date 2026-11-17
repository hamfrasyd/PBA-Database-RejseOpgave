using System;
using System.Collections.Generic;
using System.Text;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models;

namespace TravelApp.BusinessLogic.Handlers.Posts
{
    public class GetPostsHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        /// <summary>Initializes a new GetPostHandler instance.</summary>
        public GetPostsHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

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
