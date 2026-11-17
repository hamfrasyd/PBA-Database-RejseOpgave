using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Destinations;
using TravelApp.BusinessLogic.Models.Users;

namespace TravelApp.BusinessLogic.Handlers.Accounts.Register
{
    /// <summary>Validates registration and creates a local user account.</summary>
    public sealed class RegisterHandler(IUnitOfWorkFactory unitOfWorkFactory)
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
            if (request.Birthday is not { } birthday || birthday > today)
            {
                throw new TravelAppValidationException("Angiv en gyldig fødselsdato, der ikke ligger i fremtiden.");
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
                CreatedAt = now,
                City = city
            };

            await unitOfWork.Users.AddWithAccountAsync(profile, request.UserName.Trim(), request.Password);
            await unitOfWork.CommitAsync();

            return profile.Id;
        }
    }
}