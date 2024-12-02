using Microsoft.EntityFrameworkCore;

using TicketingSystem.DataAccess;
using TicketingSystem.Models;

namespace TicketingSystem.Services
{
    public class PlanService : IPlanService
    {
        private readonly AppContextDB _context;

        public PlanService(AppContextDB context)
        {
            _context = context;
        }

        public async Task<List<Plan>> GetAllAsync()
        {
            return await _context.Plans.ToListAsync();
        }

        public async Task<Plan> GetByIdAsync(long id)
        {
            return await _context.Plans.FirstOrDefaultAsync(p => p.PlanId == id);
        }

        public async Task CreateAsync(Plan plan)
        {
            _context.Plans.Add(plan);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Plan plan)
        {
            _context.Plans.Update(plan);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var plan = await _context.Plans.FindAsync(id);
            if (plan != null)
            {
                _context.Plans.Remove(plan);
                await _context.SaveChangesAsync();
            }
        }

        public bool PlanExists(long id)
        {
            return _context.Plans.Any(p => p.PlanId == id);
        }
    }
}
