using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.DataAccess;
using TicketingSystem.Models;

namespace TicketingSystem.Controllers
{
    public class PlansController : Controller
    {
        private readonly AppContextDB _context;

        public PlansController(AppContextDB context)
        {
            _context = context;
        }

        // GET: Plans
        public async Task<IActionResult> Index()
        {
            return View(await _context.Plans.ToListAsync());
        }

        // GET: Plans/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plan = await _context.Plans
                .FirstOrDefaultAsync(m => m.PlanId == id);
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlanId,PlanName,Price,MaxTicketsPerMonth,Description")] Plan plan)
        {
            ModelState.Remove("Subscriptions");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(plan);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    // Verificăm dacă eroarea este legată de unicitatea lui PlanName
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("IX_Plan_PlanName"))
                    {
                        ModelState.AddModelError("PlanName", "The plan name must be unique. Please choose a different name.");
                    }
                    else
                    {
                        // Poți trata alte excepții de la baza de date aici
                        ModelState.AddModelError(string.Empty, "An error occurred while saving the plan. Please try again.");
                    }
                }
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

            var plan = await _context.Plans.FindAsync(id);
            if (plan == null)
            {
                return NotFound();
            }
            return View(plan);
        }

        // POST: Plans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    _context.Update(plan);
                    await _context.SaveChangesAsync();
                }
                //catch (DbUpdateException ex)
                //{
                //    // Verificăm dacă eroarea este legată de unicitatea lui PlanName
                //    if (ex.InnerException != null && ex.InnerException.Message.Contains("IX_Plan_PlanName"))
                //    {
                //        ModelState.AddModelError("PlanName", "The plan name must be unique. Please choose a different name.");
                //    }
                //    else
                //    {
                //        // Poți trata alte excepții de la baza de date aici
                //        ModelState.AddModelError(string.Empty, "An error occurred while saving the plan. Please try again.");
                //    }
                //}
                catch (DbUpdateConcurrencyException)
                {

                    if (!PlanExists(plan.PlanId))
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

            var plan = await _context.Plans
                .FirstOrDefaultAsync(m => m.PlanId == id);
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
            var plan = await _context.Plans.FindAsync(id);
            if (plan != null)
            {
                _context.Plans.Remove(plan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlanExists(long id)
        {
            return _context.Plans.Any(e => e.PlanId == id);
        }
    }
}
