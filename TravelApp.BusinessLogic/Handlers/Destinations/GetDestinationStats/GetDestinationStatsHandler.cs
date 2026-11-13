using TravelApp.BusinessLogic.Models;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Destinations
{
    /// <summary>Handles the get destination stats use case.</summary>
    public class GetDestinationStatsHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new GetDestinationStatsHandler instance.</summary>
        public GetDestinationStatsHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Retrieves destination stats.</summary>
        public async Task<GetDestinationStatsResponse> GetDestinationStatsAsync(int cityId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            GetDestinationStatsResponse? statistics = await unitOfWork.Destinations.GetStatsAsync(cityId);

            if (statistics is null)
            {
                throw new TravelAppNotFoundException($"City {cityId} was not found.");
            }

            return statistics;
        }
    }
}
