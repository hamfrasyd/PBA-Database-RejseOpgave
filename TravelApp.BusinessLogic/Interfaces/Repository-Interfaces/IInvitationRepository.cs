
using TravelApp.BusinessLogic.Models.Invitations;

namespace TravelApp.BusinessLogic.Interfaces
{
    /// <summary>Defines the repository for managing invitations.</summary>
    public interface IInvitationRepository
    {
        #region Create
        /// <summary>Adds a new invitation to the repository.</summary>
        Task SendInviteAsync(Invite invite, int tripId, int senderId, int receiverId);
        #endregion

        #region Read by id
        /// <summary>Retrieves an invitation by its identifier.</summary>
        Task<Invite?> GetInviteByIdAsync(int invitationId);
        #endregion

        #region Read by receiver
        /// <summary>Retrieves pending invitations for a specific receiver.</summary>
        Task<List<Invite>> GetInvitesByReceiverAsync(int receiverId, DateTime todayUtc);
        #endregion

        #region Update
        /// <summary>Updates an existing invitation in the repository.</summary>
        Task UpdateInviteAsync(Invite invite);
        #endregion
        
        #region Delete
        /// <summary>Deletes an existing invitation from the repository.</summary>
        Task DeleteInviteAsync(Invite invite);
        #endregion

        #region DeleteExpired
        /// <summary>Deletes expired pending invitations from the repository.</summary>
        Task<int> DeleteExpiredPendingInvitesAsync(DateTime todayUtc);
        #endregion

        #region Checks
        /// <summary>Checks if a user is a participant in a specific trip.</summary>
        Task<bool> IsParticipantAsync(int tripId, int userId);

        /// <summary>Checks if a pending invitation exists for a specific trip and receiver.</summary>
        Task<bool> PendingInviteExistsForTripAsync(int tripId, int receiverId);
        #endregion
    }
}
