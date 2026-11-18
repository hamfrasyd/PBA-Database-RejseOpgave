using Microsoft.EntityFrameworkCore;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Invitations;
using TravelApp.DataAccess.SQLRepositories.EntityFramework;

namespace TravelApp.DataAccess.SQLRepositories
{
    /// <summary>Implements SQL Server database operations for trip invitations.</summary>
    public class SqlInviteRepository : IInviteRepository
    {
        private readonly TravelAppContext _context;

        /// <summary>Initializes the invitation repository.</summary>
        /// <param name="context">The shared database context.</param>
        public SqlInviteRepository(TravelAppContext context)
        {
            _context = context;
        }

        #region Create - Invitations

        /// <inheritdoc/>
        public async Task AddAsync(Invite invitation, int tripId, int senderId, int receiverId, int statusId)
        {
            await _context.InviteEntities.AddAsync(invitation);

            _context.Entry(invitation).Property("TripId").CurrentValue = tripId;
            _context.Entry(invitation).Property("SenderId").CurrentValue = senderId;
            _context.Entry(invitation).Property("ReceiverId").CurrentValue = receiverId;
            _context.Entry(invitation).Property("InvitationStatusId").CurrentValue = statusId;
        }

        #endregion

        #region Read - Invitations

        /// <inheritdoc/>
        public async Task<int?> GetStatusIdAsync(string statusName)
        {
            int? statusId = await _context.InvitationStatusEntities
                .AsNoTracking()
                .Where(status => status.StatusName == statusName)
                .Select(status => (int?)status.Id)
                .FirstOrDefaultAsync();

            return statusId;
        }

        /// <inheritdoc/>
        public async Task<bool> ActivePendingExistsAsync(int tripId, int receiverId, DateTime oldestActiveSendDateUtc)
        {
            bool invitationExists = await _context.InviteEntities.AsNoTracking().AnyAsync(invitation =>
                invitation.Trip!.Id == tripId &&
                invitation.Receiver!.Id == receiverId &&
                invitation.InvitationStatus!.StatusName == "Pending" &&
                invitation.SendDate > oldestActiveSendDateUtc);

            return invitationExists;
        }

        #endregion
    }
}
