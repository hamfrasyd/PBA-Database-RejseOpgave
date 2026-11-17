using TravelApp.BusinessLogic.DTOs.Locations;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Locations.GetContinents
{
    /// <summary>Handles the get continents use case.</summary>
    public class GetContinentsHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new GetContinentsHandler instance.</summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        public GetContinentsHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Retrieves continents.</summary>
        /// <returns>The continents.</returns>
        public async Task<List<GetContinentsResponse>> GetContinentsAsync()
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            List<ContinentDTO> continentsFromDb = await unitOfWork.Locations.GetContinentsAsync();

            List<GetContinentsResponse> continentsResponse = new List<GetContinentsResponse>();

            foreach (ContinentDTO continent in continentsFromDb)
            {
                continentsResponse.Add(new GetContinentsResponse
                {
                    Id = continent.Id,
                    Name = continent.Name
                });
            }

            return continentsResponse;
        }
    }
}
