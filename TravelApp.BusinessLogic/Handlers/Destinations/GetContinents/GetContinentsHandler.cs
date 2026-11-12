using TravelApp.BusinessLogic.Models;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Destinations
{
    public class GetContinentsHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public GetContinentsHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public async Task<List<GetContinentsResponse>> GetContinentsAsync()
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            List<Continent> continentsFromDb = await unitOfWork.Destinations.GetContinentsAsync();

            List<GetContinentsResponse> continentsResponse = new List<GetContinentsResponse>();

            foreach (Continent continent in continentsFromDb)
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
