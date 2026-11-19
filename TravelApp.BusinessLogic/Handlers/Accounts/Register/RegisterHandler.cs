using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Accounts;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Locations;
using TravelApp.BusinessLogic.Models.Users;

namespace TravelApp.BusinessLogic.Handlers.Accounts.Register
{
    /// <summary>Validates registration and creates a local user account.</summary>
    public sealed class RegisterHandler(
        IUnitOfWorkFactory unitOfWorkFactory,
        IUserAccountRepository userAccountRepository)
    {
        /// <summary>Registers a user and returns the new profile ID.</summary>
        public async Task<int> RegisterAsync(RegisterRequest request)
        {
            if (!PasswordValidator.IsValid(request.Password))
            {
                throw new TravelAppValidationException("Adgangskoden skal være på 15–64 tegn.");
            }

            DateTime now = DateTime.UtcNow;
            DateOnly today = DateOnly.FromDateTime(now);
            DateOnly birthday = request.Birthday ?? throw new TravelAppValidationException("Angiv en fødselsdato.");

            if (birthday > today)
            {
                throw new TravelAppValidationException("Fødselsdatoen må ikke ligge i fremtiden.");
            }

            await using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

            City? city = await unitOfWork.Locations.GetCityByIdAsync(request.CityId);
            if (city is null)
            {
                throw new TravelAppValidationException($"Byen med id {request.CityId} findes ikke.");
            }

            var profile = new TravelUser
            {
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                Birthday = birthday,
                IsPrivate = request.IsPrivate,
                CreatedAt = now
            };

            return await userAccountRepository.CreateAsync(profile, request.CityId, new CreateUserAccountDto
            {
                UserName = request.UserName.Trim(),
                Password = request.Password
            });
        }
    }
}