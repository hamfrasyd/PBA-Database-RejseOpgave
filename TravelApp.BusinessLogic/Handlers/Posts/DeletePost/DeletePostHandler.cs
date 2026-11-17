using System;
using System.Collections.Generic;
using System.Text;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Posts
{
    public class DeletePostHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new CreatePostHandler instance.</summary>
        public DeletePostHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public async Task DeletePostAsync(int postId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            bool wasDeleted = await unitOfWork.Posts.DeletePostAsync(postId);

            if (wasDeleted is false)
            {
                throw new TravelAppNotFoundException($"Activity {postId} was not found.");
            }

            await unitOfWork.CommitAsync();
        }
    }
}
