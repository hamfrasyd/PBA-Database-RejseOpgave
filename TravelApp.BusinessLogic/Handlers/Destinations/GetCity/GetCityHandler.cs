using TravelApp.BusinessLogic.Models;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Destinations
{
    /// <summary>Handles the get city use case.</summary>
    public class GetCityHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        /// <summary>Initializes a new GetCityHandler instance.</summary>
        public GetCityHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Retrieves city.</summary>
        public async Task<GetCityResponse> GetCityAsync(int id)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            City? cityFromDb = await unitOfWork.Destinations.GetCityByIdAsync(id);

            if (cityFromDb is null)
            {
                throw new TravelAppNotFoundException($"City {id} was not found.");
            }

            if (cityFromDb.Country is null)
            {
                throw new InvalidOperationException($"Required Country relationship was not loaded for City {id}.");
            }

            GetCityResponse response = new GetCityResponse
            {
                Id = cityFromDb.Id,
                CountryId = cityFromDb.Country.Id,
                Name = cityFromDb.Name
            };

            return response;
        }
    }
}
