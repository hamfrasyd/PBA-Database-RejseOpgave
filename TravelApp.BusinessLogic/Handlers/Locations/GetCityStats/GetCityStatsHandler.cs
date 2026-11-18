using TravelApp.BusinessLogic.DTOs.Locations;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Locations.GetCityStats
{
    /// <summary>Handles the get city stats use case.</summary>
    public class GetCityStatsHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new GetCityStatsHandler instance.</summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        public GetCityStatsHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Retrieves city stats.</summary>
        /// <param name="cityId">The city identifier.</param>
        /// <returns>The statistics for the city.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the city does not exist.</exception>
        public async Task<GetCityStatsResponse> GetCityStatsAsync(int cityId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            CityStatsDTO? statistics = await unitOfWork.Locations.GetCityStatsAsync(cityId);
            if (statistics is null)
            {
                throw new TravelAppNotFoundException($"City with ID {cityId} was not found.");
            }

            GetCityStatsResponse response = new GetCityStatsResponse
            {
                CityId = statistics.CityId,
                City = statistics.City,
                Country = statistics.Country,
                Continent = statistics.Continent,
                TripCount = statistics.TripCount,
                CompletedTripCount = statistics.CompletedTripCount,
                ActivityCount = statistics.ActivityCount,
                AvgStars = statistics.AvgStars
            };

            return response;
        }
    }
}
