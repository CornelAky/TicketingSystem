using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.DataAccess;
using TicketingSystem.Models;

namespace TicketingSystem.Controllers
{
    public class SubscriptionsController : Controller
    {
        private readonly AppContextDB _context;

        public SubscriptionsController(AppContextDB context)
        {
            _context = context;
        }

        // GET: Subscriptions
        public async Task<IActionResult> Index()
        {
            var userRoles = User.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.Role).Select(c => c.Value).ToList();
            if (userRoles.Contains("Admin") || userRoles.Contains("Support"))
            {
                var appContextDB = _context.Subscriptions.Include(s => s.Plan).Include(s => s.User);
                return View(await appContextDB.ToListAsync());
                
            }
            else
            {
                var appContextDB = _context.Subscriptions.Include(s => s.Plan).Include(s => s.User).Where(x=>x.UserId==GetCurrentUserId());
                return View(await appContextDB.ToListAsync());
            }


        }

        // GET: Subscriptions/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscriptions
                .Include(s => s.Plan)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.SubscriptionId == id);
            if (subscription == null)
            {
                return NotFound();
            }

            return View(subscription);
        }

        // GET: Subscriptions/Create
        public IActionResult Create()
        {
            ViewData["PlanId"] = new SelectList(_context.Plans, "PlanId", "PlanName");
            var userRoles = User.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.Role).Select(c => c.Value).ToList();
            if (userRoles.Contains("Admin") || userRoles.Contains("Support"))
            {
                ViewData["UserId"] = new SelectList(_context.Users, "UserID", "FullName");
            }
            else
            {
                ViewData["UserId"] = new SelectList(_context.Users.Where(x => x.UserID == GetCurrentUserId()), "UserID", "FullName");
            }
            return View();
        }

        // POST: Subscriptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubscriptionId,UserId,PlanId,StartDate,EndDate,IsActive")] Subscription subscription)
        {
            ModelState.Remove("Payments");
            ModelState.Remove("Plan");
            ModelState.Remove("User");
            if (ModelState.IsValid)
            {
                _context.Add(subscription);
                await _context.SaveChangesAsync();
                //Adauga la facturi
                Plan plan = _context.Plans.Where(x => x.PlanId == subscription.PlanId).FirstOrDefault();
                if(plan!=null)
                {
                    Payment payment = new Payment 
                    { 
                        Amount=plan.Price,
                        SubscriptionId= subscription.SubscriptionId,
                        PaymentDate=DateTime.Now,
                    };
                    _context.Add(payment);
                    await _context.SaveChangesAsync();

                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["PlanId"] = new SelectList(_context.Plans, "PlanId", "PlanName", subscription.PlanId);
            var userRoles = User.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.Role).Select(c => c.Value).ToList();
            if (userRoles.Contains("Admin") || userRoles.Contains("Support"))
            {
                ViewData["UserId"] = new SelectList(_context.Users, "UserID", "FullName");
            }
            else
            {
                ViewData["UserId"] = new SelectList(_context.Users.Where(x => x.UserID == GetCurrentUserId()), "UserID", "FullName");
            }
            return View(subscription);
        }

        // GET: Subscriptions/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }
            ViewData["PlanId"] = new SelectList(_context.Plans, "PlanId", "PlanName", subscription.PlanId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserID", "FullName", subscription.UserId);
            return View(subscription);
        }

        // POST: Subscriptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("SubscriptionId,UserId,PlanId,StartDate,EndDate,IsActive")] Subscription subscription)
        {
            if (id != subscription.SubscriptionId)
            {
                return NotFound();
            }

            ModelState.Remove("Payments");
            ModelState.Remove("Plan");
            ModelState.Remove("User");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subscription);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubscriptionExists(subscription.SubscriptionId))
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
            ViewData["PlanId"] = new SelectList(_context.Plans, "PlanId", "PlanName", subscription.PlanId);
            ViewData["UserId"] = new SelectList(_context.Users.Where(x=>x.UserID== GetCurrentUserId()), "UserID", "FullName", subscription.UserId);
            return View(subscription);
        }

        // GET: Subscriptions/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscriptions
                .Include(s => s.Plan)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.SubscriptionId == id);
            if (subscription == null)
            {
                return NotFound();
            }

            return View(subscription);
        }

        // POST: Subscriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription != null)
            {
                _context.Subscriptions.Remove(subscription);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubscriptionExists(long id)
        {
            return _context.Subscriptions.Any(e => e.SubscriptionId == id);
        }

        private long GetCurrentUserId()
        {
            return long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        }
    }
}
