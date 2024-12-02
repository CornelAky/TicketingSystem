using TicketingSystem.Models;

namespace TicketingSystem.Services
{
    public interface ITicketStatusService
    {
        Task<List<TicketStatus>> GetAllAsync();
        Task<TicketStatus> GetByIdAsync(long id);
        Task CreateAsync(TicketStatus ticketStatus);
        Task UpdateAsync(TicketStatus ticketStatus);
        Task DeleteAsync(long id);
        bool TicketStatusExists(long id);
    }
}