using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using TicketingSystem.Models;
using TicketingSystem.Services;

namespace TicketingSystem.Controllers
{
    public class TicketStatusController : Controller
    {
        private readonly ITicketStatusService _service;

        public TicketStatusController(ITicketStatusService service)
        {
            _service = service;
        }

        // GET: TicketStatus
        public async Task<IActionResult> Index()
        {
            var statuses = await _service.GetAllAsync();
            return View(statuses);
        }

        // GET: TicketStatus/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketStatus = await _service.GetByIdAsync(id.Value);
            if (ticketStatus == null)
            {
                return NotFound();
            }

            return View(ticketStatus);
        }

        // GET: TicketStatus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TicketStatus/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StatusId,StatusName")] TicketStatus ticketStatus)
        {
            ModelState.Remove("Tickets");
            if (ModelState.IsValid)
            {
                await _service.CreateAsync(ticketStatus);
                return RedirectToAction(nameof(Index));
            }
            return View(ticketStatus);
        }

        // GET: TicketStatus/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketStatus = await _service.GetByIdAsync(id.Value);
            if (ticketStatus == null)
            {
                return NotFound();
            }
            return View(ticketStatus);
        }

        // POST: TicketStatus/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("StatusId,StatusName")] TicketStatus ticketStatus)
        {
            ModelState.Remove("Tickets");
            if (id != ticketStatus.StatusId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.UpdateAsync(ticketStatus);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_service.TicketStatusExists(ticketStatus.StatusId))
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
            return View(ticketStatus);
        }

        // GET: TicketStatus/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketStatus = await _service.GetByIdAsync(id.Value);
            if (ticketStatus == null)
            {
                return NotFound();
            }

            return View(ticketStatus);
        }

        // POST: TicketStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
