
using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Trips;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Trips
{
    /// <summary>Provides shared access control for trips.</summary>
    public static class TripAccessHelper
    {
        /// <summary>Ensures that the current user can view the trip.</summary>
        public static async Task EnsureCanViewAsync(IUnitOfWork unitOfWork, int tripId, ICurrentUser currentUser)
        {
            await EnsureAccessAsync(unitOfWork, tripId, currentUser, "view");
        }

        /// <summary>Ensures that the current user can modify the trip.</summary>
        public static async Task EnsureCanModifyAsync(IUnitOfWork unitOfWork, int tripId, ICurrentUser currentUser)
        {
            await EnsureAccessAsync(unitOfWork, tripId, currentUser, "modify");
        }

        private static async Task EnsureAccessAsync(IUnitOfWork unitOfWork, int tripId, ICurrentUser currentUser, string action)
        {
            TripAccessInfoDTO? access = await unitOfWork.Trips.GetAccessInfoAsync(tripId, currentUser.UserId);

            if (access is null)
            {
                throw new TravelAppNotFoundException($"Trip {tripId} was not found.");
            }

            if (!access.IsParticipant && !currentUser.IsAdmin)
            {
                throw new TravelAppForbiddenException($"You cannot {action} this trip.");
            }
        }
    }
}
