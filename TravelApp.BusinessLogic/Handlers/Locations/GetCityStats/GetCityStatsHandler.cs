using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Locations.GetDestinationStats
{
    /// <summary>Handles the get city stats use case.</summary>
    public class GetCityStatsHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new GetCityStatsHandler instance.</summary>
        public GetCityStatsHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Retrieves city stats.</summary>
        public async Task<GetCityStatsResponse> GetCityStatsAsync(int cityId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            GetCityStatsResponse? statistics = await unitOfWork.Locations.GetStatsAsync(cityId);

            if (statistics is null)
            {
                throw new TravelAppNotFoundException($"City {cityId} was not found.");
            }

            return statistics;
        }
    }
}
