using TravelApp.BusinessLogic.Models;

namespace TravelApp.BusinessLogic.Interfaces
{
    public interface ITripRepository
    {
        
        Task<Trip?> GetByIdAsync(int tripId);

        Task<List<Trip>> GetByUserAsync(int userId);

        Task AddAsync(Trip trip);

        void Delete(Trip trip);

        Task<bool> CityExistsAsync(int cityId);

        Task<TripStop?> GetStopAsync(int tripId, int cityId);
        Task<TripActivity?> GetTripActivityAsync(
            int tripId, int activityId);

        Task<TripParticipant?> GetParticipantAsync(
            int tripId, int userId);

        void AddStop(TripStop stop);
        void AddTripActivity(TripActivity activity);
        void AddParticipant(TripParticipant participant);

        void RemoveStop(TripStop stop);
        void RemoveTripActivity(TripActivity activity);
        void RemoveParticipant(TripParticipant participant);
    }
}