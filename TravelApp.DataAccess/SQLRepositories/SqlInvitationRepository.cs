using Microsoft.EntityFrameworkCore;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Invitations;
using TravelApp.DataAccess.SQLRepositories.EntityFramework;

namespace TravelApp.DataAccess.SQLRepositories
{
    /// <summary>Implements the repository for managing invitations using SQL.</summary>
    public class SqlInvitationRepository : IInvitationRepository
    {
        private readonly TravelAppContext _context;

        /// <summary>Initializes a new SqlInvitationRepository instance.</summary>
        public SqlInvitationRepository(TravelAppContext context)
        {
            _context = context;
        }

        #region Create
        /// <summary>Adds a new invitation to the repository.</summary>
        public async Task SendInviteAsync(Invite invite, int tripId, int senderId, int receiverId)
        {
            await _context.InviteEntities.AddAsync(invite);

            _context.Entry(invite).Property("TripId").CurrentValue = tripId;
            _context.Entry(invite).Property("SenderId").CurrentValue = senderId;
            _context.Entry(invite).Property("ReceiverId").CurrentValue = receiverId;
        }
        #endregion


        /// <summary>Checks if a user is a participant in a specific trip.</summary>
        public async Task<bool> IsParticipantAsync(int tripId, int userId)
        {
            return await _context.TripParticipantEntities
                .AsNoTracking()
                .AnyAsync(participant => participant.Trip!.Id == tripId && participant.User!.Id == userId);
        }

        #region Read by id
        /// <summary>Retrieves an invitation by its identifier.</summary>
        public async Task<Invite?> GetInviteByIdAsync(int invitationId)
        {
            DateTime tomorrowUtc = DateTime.UtcNow.Date.AddDays(1);

            return await _context.InviteEntities
                .Include(invite => invite.Trip).Include(invite => invite.Sender).Include(invite => invite.Receiver)
                .FirstOrDefaultAsync(invite => invite.Id == invitationId &&
                    (
                        invite.IsAccepted != null || invite.Trip!.StartDate == null || invite.Trip.StartDate >= tomorrowUtc
                    ));
        }
        #endregion

        #region Read by receiver
        /// <summary>Retrieves pending invitations for a specific receiver.</summary>
        public async Task<List<Invite>> GetPendingInvitesByReceiverAsync(int receiverId, DateTime todayUtc)
        {
            return await _context.InviteEntities
                .AsNoTracking()
                .Include(invite => invite.Trip).Include(invite => invite.Sender)
                .Where(invite =>
                    EF.Property<int>(invite, "ReceiverId") == receiverId && invite.IsAccepted == null &&
                    (invite.Trip!.StartDate == null || invite.Trip.StartDate >= todayUtc.AddDays(1)))
                .OrderBy(invite => invite.Trip!.StartDate)
                .ToListAsync();
        }
        #endregion


        /// <summary>Checks if a pending invitation exists for a specific trip and receiver.</summary>
        public async Task<bool> PendingInviteExistsForTripAsync(int tripId, int receiverId)
        {
            return await _context.InviteEntities
                .AsNoTracking()
                .AnyAsync(invite =>  EF.Property<int>(invite, "TripId") == tripId &&
                    EF.Property<int>(invite, "ReceiverId") == receiverId && invite.IsAccepted == null);
        }

        /// <summary>Updates an existing invitation in the repository.</summary>
        public Task UpdateInviteAsync(Invite invite)
        {
            _context.InviteEntities.Update(invite);
            return Task.CompletedTask;
        }

        /// <summary>Deletes an existing invitation from the repository.</summary>
        public Task DeleteInviteAsync(Invite invite)
        {
            _context.InviteEntities.Remove(invite);
            return Task.CompletedTask;
        }

        /// <summary>Deletes expired pending invitations from the repository.</summary>
        //Denne går uden om unit of work, da den ikke skal bruge transaction. Den skal bare slette alle udløbne invitationer.
        public async Task<int> DeleteExpiredPendingInvitesAsync(
            DateTime todayUtc)
        {
            return await _context.InviteEntities
                .Where(invite =>
                    invite.IsAccepted == null &&
                    invite.Trip!.StartDate != null &&
                    invite.Trip.StartDate < todayUtc.AddDays(1))
                .ExecuteDeleteAsync();
        }
    }
}
