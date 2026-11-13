using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Handlers.Destinations;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models;

namespace TravelApp.BusinessLogic.Handlers.Posts.GetPost
{
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

            GetPostResponse? postFromDb = await unitOfWork.Posts.GetPostByIdAsync(id);

            if (postFromDb is null)
            {
                throw new TravelAppNotFoundException($"Post {id} was not found.");
            }

            if (postFromDb.UserId is null)
            {
                throw new InvalidOperationException($"Required User relationship was not loaded for Post {id}.");
            }

            return postFromDb;
        }


    }
}
