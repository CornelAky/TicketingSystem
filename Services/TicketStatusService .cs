using Microsoft.EntityFrameworkCore;
using TicketingSystem.DataAccess;
using TicketingSystem.Models;

namespace TicketingSystem.Services
{
    public class TicketStatusService : ITicketStatusService
    {
        private readonly AppContextDB _context;

        public TicketStatusService(AppContextDB context)
        {
            _context = context;
        }

        public async Task<List<TicketStatus>> GetAllAsync()
        {
            return await _context.TicketStatuses.ToListAsync();
        }

        public async Task<TicketStatus> GetByIdAsync(long id)
        {
            return await _context.TicketStatuses.FirstOrDefaultAsync(ts => ts.StatusId == id);
        }

        public async Task CreateAsync(TicketStatus ticketStatus)
        {
            _context.TicketStatuses.Add(ticketStatus);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TicketStatus ticketStatus)
        {
            _context.TicketStatuses.Update(ticketStatus);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var ticketStatus = await _context.TicketStatuses.FindAsync(id);
            if (ticketStatus != null)
            {
                _context.TicketStatuses.Remove(ticketStatus);
                await _context.SaveChangesAsync();
            }
        }

        public bool TicketStatusExists(long id)
        {
            return _context.TicketStatuses.Any(ts => ts.StatusId == id);
        }
    }
}
