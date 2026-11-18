using TravelApp.BusinessLogic.Models.Invitations;

namespace TravelApp.BusinessLogic.Interfaces
{
    /// <summary>Defines database operations for trip invitations.</summary>
    public interface IInviteRepository
    {
        #region Create - Invitations

        /// <summary>Stages a trip invitation with its related identifiers.</summary>
        /// <param name="invitation">The invitation to add.</param>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="senderId">The sender's identifier.</param>
        /// <param name="receiverId">The receiver's identifier.</param>
        /// <param name="statusId">The invitation status identifier.</param>
        /// <returns>A task that completes when the invitation is staged.</returns>
        Task AddAsync(Invite invitation, int tripId, int senderId, int receiverId, int statusId);

        #endregion

        #region Read - Invitations

        /// <summary>Retrieves an invitation and its related users and status.</summary>
        /// <param name="invitationId">The invitation identifier.</param>
        /// <returns>The invitation, or <see langword="null"/> if it does not exist.</returns>
        Task<Invite?> GetByIdAsync(int invitationId);

        /// <summary>Retrieves the identifier of a named invitation status.</summary>
        /// <param name="statusName">The status name.</param>
        /// <returns>The status identifier, or <see langword="null"/> if it does not exist.</returns>
        Task<int?> GetStatusIdAsync(string statusName);

        /// <summary>Checks for an active pending invitation for a trip and receiver.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="receiverId">The receiver's identifier.</param>
        /// <param name="oldestActiveSendDateUtc">The earliest send date still considered active.</param>
        /// <returns><see langword="true"/> if an active pending invitation exists; otherwise, <see langword="false"/>.</returns>
        Task<bool> ActivePendingExistsAsync(int tripId, int receiverId, DateTime oldestActiveSendDateUtc);

        #endregion
    }
}
