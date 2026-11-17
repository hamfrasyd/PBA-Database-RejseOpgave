using System;
using System.Collections.Generic;
using System.Text;
using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models;

namespace TravelApp.BusinessLogic.Handlers.Posts
{
    public class UpdatePostHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new CreatePostHandler instance.</summary>
        public UpdatePostHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

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
