using TicketingSystem.Models;

namespace TicketingSystem.Services
{
    public interface IPlanService
    {
        Task<List<Plan>> GetAllAsync();
        Task<Plan> GetByIdAsync(long id);
        Task CreateAsync(Plan plan);
        Task UpdateAsync(Plan plan);
        Task DeleteAsync(long id);
        bool PlanExists(long id);
    }
}
