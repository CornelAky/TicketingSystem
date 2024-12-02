using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using TicketingSystem.Models;
using TicketingSystem.Services;

namespace TicketingSystem.Controllers
{
    public class PlansController : Controller
    {
        private readonly IPlanService _planService;

        public PlansController(IPlanService planService)
        {
            _planService = planService;
        }

        // GET: Plans
        public async Task<IActionResult> Index()
        {
            var plans = await _planService.GetAllAsync();
            return View(plans);
        }

        // GET: Plans/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plan = await _planService.GetByIdAsync(id.Value);
            if (plan == null)
            {
                return NotFound();
            }

            return View(plan);
        }

        // GET: Plans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Plans/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlanId,PlanName,Price,MaxTicketsPerMonth,Description")] Plan plan)
        {
            ModelState.Remove("Subscriptions");
            if (ModelState.IsValid)
            {
                await _planService.CreateAsync(plan);
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        // GET: Plans/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plan = await _planService.GetByIdAsync(id.Value);
            if (plan == null)
            {
                return NotFound();
            }

            return View(plan);
        }

        // POST: Plans/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("PlanId,PlanName,Price,MaxTicketsPerMonth,Description")] Plan plan)
        {
            if (id != plan.PlanId)
            {
                return NotFound();
            }
            ModelState.Remove("Subscriptions");

            if (ModelState.IsValid)
            {
                try
                {
                    await _planService.UpdateAsync(plan);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_planService.PlanExists(plan.PlanId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        // GET: Plans/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plan = await _planService.GetByIdAsync(id.Value);
            if (plan == null)
            {
                return NotFound();
            }

            return View(plan);
        }

        // POST: Plans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            await _planService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
