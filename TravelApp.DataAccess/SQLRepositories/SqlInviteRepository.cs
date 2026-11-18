using Microsoft.EntityFrameworkCore;
using TravelApp.BusinessLogic.DTOs.Invitations;
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
        public async Task<Invite?> GetByIdAsync(int invitationId)
        {
            Invite? invitationFromDb = await _context.InviteEntities
                .Include(invitation => invitation.Trip)
                .Include(invitation => invitation.Sender)
                .Include(invitation => invitation.Receiver)
                .Include(invitation => invitation.InvitationStatus)
                .FirstOrDefaultAsync(invitation => invitation.Id == invitationId);

            return invitationFromDb;
        }

        /// <inheritdoc/>
        public async Task<List<InvitationDTO>> GetForUserAsync(int userId, bool sentByUser)
        {
            IQueryable<Invite> invitationsQuery = _context.InviteEntities.AsNoTracking();

            if (sentByUser is true)
            {
                invitationsQuery = invitationsQuery.Where(invitation => invitation.Sender!.Id == userId);
            }
            else
            {
                invitationsQuery = invitationsQuery.Where(invitation => invitation.Receiver!.Id == userId);
            }

            IQueryable<InvitationDTO> invitationDetailsQuery = invitationsQuery.Select(invitation => new InvitationDTO
            {
                Id = invitation.Id,
                TripId = invitation.Trip!.Id,
                SenderId = invitation.Sender!.Id,
                SenderName = invitation.Sender.FirstName + " " + invitation.Sender.LastName,
                ReceiverId = invitation.Receiver!.Id,
                ReceiverName = invitation.Receiver.FirstName + " " + invitation.Receiver.LastName,
                Status = invitation.InvitationStatus!.StatusName,
                SendDate = invitation.SendDate
            });

            List<InvitationDTO> invitationsFromDb = await invitationDetailsQuery
                .OrderByDescending(invitation => invitation.SendDate)
                .ThenByDescending(invitation => invitation.Id)
                .ToListAsync();

            return invitationsFromDb;
        }

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

        #region Update - Invitations

        /// <inheritdoc/>
        public async Task<bool> UpdateStatusAsync(int invitationId, int statusId)
        {
            // FindAsync reuses the invitation already tracked by GetByIdAsync.
            Invite? invitationFromDb = await _context.InviteEntities.FindAsync(invitationId);
            if (invitationFromDb is null)
            {
                return false;
            }

            _context.Entry(invitationFromDb).Property("InvitationStatusId").CurrentValue = statusId;

            return true;
        }

        #endregion

        #region Delete - Invitations

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(int invitationId)
        {
            // FindAsync uses the tracked invitation from GetByIdAsync without another query.
            Invite? invitationFromDb = await _context.InviteEntities.FindAsync(invitationId);
            if (invitationFromDb is null)
            {
                return false;
            }

            _context.InviteEntities.Remove(invitationFromDb);

            return true;
        }

        #endregion
    }
}
