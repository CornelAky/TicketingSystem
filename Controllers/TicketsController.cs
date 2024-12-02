using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.DataAccess;
using TicketingSystem.Models;
using TicketingSystem.Models.ViewModel;

namespace TicketingSystem.Controllers
{

    public class TicketsController : Controller
    {
        private readonly AppContextDB _context;

        public TicketsController(AppContextDB context)
        {
            _context = context;
        }

        // GET: Tickets

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var appContextDB = _context.Tickets.Include(t => t.AssignedTo).Include(t => t.CreatorBy).Include(t => t.Status);
            return View(await appContextDB.ToListAsync());
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.AssignedTo)
                .Include(t => t.CreatorBy)
                .Include(t => t.Status)
                .FirstOrDefaultAsync(m => m.TicketId == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        [Authorize(Roles = "Admin,User")]
        public IActionResult Create()
        {
            // Populează dropdown-ul pentru Prioritate
            ViewData["Priority"] = new SelectList(Enum.GetValues(typeof(Ticket.ePriority)));
            return View();
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Priority")] Ticket ticket)
        {
            
            ModelState.Remove("Status");
            ModelState.Remove("Comments");
            ModelState.Remove("CreatorBy");
            ModelState.Remove("Documents");
            ModelState.Remove("AssignedTo");
            if (ModelState.IsValid)
            {
                // Setează valorile implicite pentru câmpurile care nu sunt introduse de utilizator
                ticket.CreatedById = GetCurrentUserId(); // Obține utilizatorul curent (Admin/User)
                ticket.AssignedToId = null; // Neatribuit inițial
                ticket.StatusId = _context.TicketStatuses.FirstOrDefault(s => s.StatusName == "New")?.StatusId ?? 1; // Default: "New"
                ticket.CreatedDate = DateTime.Now; // Data curentă
                ticket.ClosedDate = null; // ClosedDate rămâne null pentru început

                // Salvează în baza de date
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MyTickets));
            }

            // Repopulează dropdown-ul Prioritate în caz de eroare
            ViewData["Priority"] = new SelectList(Enum.GetValues(typeof(Ticket.ePriority)));
            return View(ticket);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Ticket ticket = await _context.Tickets
                .Include(t => t.AssignedTo)
                .Include(t => t.Status)
                .Include(t => t.CreatorBy)
                .FirstOrDefaultAsync(t => t.TicketId == id);

            if (ticket == null)
            {
                return NotFound();
            }

            SelectList assignedToList = new SelectList(_context.Users, "UserID", "FullName", ticket.AssignedToId);
            SelectList statusList = new SelectList(_context.TicketStatuses, "StatusId", "StatusName", ticket.StatusId);
            SelectList priorityList = new SelectList(Enum.GetValues(typeof(Ticket.ePriority)));

            ViewData["AssignedToId"] = assignedToList;
            ViewData["StatusId"] = statusList;
            ViewData["Priority"] = priorityList;

            return View(ticket);
        }



        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("TicketId,AssignedToId,StatusId,Priority,ClosedDate")] Ticket ticket)
        {
            try
            {
                Ticket existingTicket = await _context.Tickets
                    .Include(t => t.CreatorBy)
                    .FirstOrDefaultAsync(t => t.TicketId == ticket.TicketId);

                if (existingTicket == null)
                {
                    return NotFound();
                }

                // Actualizăm doar câmpurile editabile
                existingTicket.AssignedToId = ticket.AssignedToId;
                existingTicket.StatusId = ticket.StatusId;
                existingTicket.Priority = ticket.Priority;
                existingTicket.ClosedDate = ticket.ClosedDate;

                _context.Update(existingTicket);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }catch(Exception e)
            {
                // Refacem SelectList-urile pentru dropdown-uri
                ViewData["AssignedToId"] = new SelectList(_context.Users, "UserID", "FullName", ticket.AssignedToId);
                ViewData["StatusId"] = new SelectList(_context.TicketStatuses, "StatusId", "StatusName", ticket.StatusId);
                ViewData["Priority"] = new SelectList(Enum.GetValues(typeof(Ticket.ePriority)), ticket.Priority);

                return View(ticket);
            }
        }




        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.AssignedTo)
                .Include(t => t.CreatorBy)
                .Include(t => t.Status)
                .FirstOrDefaultAsync(m => m.TicketId == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "User")] // Restricționează accesul doar pentru utilizatori autentificați cu rolul User
        public async Task<IActionResult> MyTickets()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Obține ID-ul utilizatorului curent
            if (userId == null)
            {
                return Unauthorized();
            }

            // Filtrăm ticket-urile pentru utilizatorul curent
            List<Ticket> myTickets = _context.Tickets
                .Where(t => t.CreatorBy.UserID.ToString() == userId )
                .Include(t => t.AssignedTo)
                .Include(t => t.CreatorBy)
                .Include(t => t.Status).ToList();

            TicketViewModel model = new TicketViewModel
            {
                tickets = myTickets,
                ticketsAvailable = ticketsAvailable()
            };

            return View(model);
        }

        [Authorize(Roles = "Support,Admin")]
        public async Task<IActionResult> AssignedToMe()
        {
            // Obține ID-ul utilizatorului curent din claim-uri
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            // Obține ticketele asignate utilizatorului curent
            var tickets = await _context.Tickets
                .Include(t => t.AssignedTo)
                .Include(t => t.Status)
                .Where(t => t.AssignedToId.ToString() == userId)
                .ToListAsync();

            return View(tickets);
        }

        [Authorize(Roles = "User,Support,Admin")]
        public async Task<IActionResult> DetailsWithComments(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Ticket ticket = await _context.Tickets.Include(t => t.Comments).Include(t => t.CreatorBy).Include(t => t.AssignedTo).Include(t => t.Status).FirstOrDefaultAsync(m => m.TicketId == id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        [Authorize(Roles = "User,Support,Admin")]
        [HttpPost]
        public async Task<IActionResult> AddComment(long ticketId, string commentText)
        {
            if (string.IsNullOrWhiteSpace(commentText))
            {
                return RedirectToAction("DetailsWithComments", new { id = ticketId });
            }

            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket == null)
            {
                return NotFound();
            }

            Comment comment = new Comment
            {
                TicketId = ticketId,
                CommentText = commentText,
                CreatedDate = DateTime.UtcNow,
                UserId = long.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value)
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("DetailsWithComments", new { id = ticketId });
        }

        [Authorize(Roles = "Support,Admin")]
        [HttpPost]
        public async Task<IActionResult> ResolveTicket(long ticketId)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);

            if (ticket == null)
            {
                return NotFound();
            }

            // Caută StatusId pentru statusul "Resolved"
            TicketStatus resolvedStatus = await _context.TicketStatuses
                .FirstOrDefaultAsync(s => s.StatusName.Equals("Resolved"));

            // Setează StatusId la ID-ul corespunzător statusului "Resolved" dacă este găsit
            if (resolvedStatus != null)
            {
                ticket.StatusId = resolvedStatus.StatusId;
            }

            // Setează data de închidere
            ticket.ClosedDate = DateTime.UtcNow;

            _context.Update(ticket);
            await _context.SaveChangesAsync();

            return RedirectToAction("DetailsWithComments", new { id = ticketId });
        }


        private int ticketsAvailable()
        {

            //Daca are vreun abonament actic
            DateTime now = DateTime.Now;
            List<Subscription> subscriptionsAll = _context.Subscriptions.Include(x => x.Plan).ToList();
            List<Subscription> subscriptions = _context.Subscriptions.Include(x=>x.Plan).Where( x => x.UserId == GetCurrentUserId()  
                                                                           && x.StartDate<=now  
                                                                           && x.EndDate>= now 
                                                                           && x.IsActive).ToList();
            if (subscriptions.Count == 0)
                return 0;

            int credit = subscriptions.Sum(x => x.Plan.MaxTicketsPerMonth);

            if (credit <= 0)
                return 0;

            DateTime minDate = subscriptions.Min(x => x.StartDate);
            DateTime maxDate = subscriptions.Max(x => x.EndDate);

            int creditUsed = _context.Tickets
                .Where(t => t.CreatedById == GetCurrentUserId()
                            && minDate <= t.CreatedDate
                            && maxDate >= t.CreatedDate).Count();

            return (credit - creditUsed) < 0 ? 0 : credit - creditUsed; 
        }

        private bool TicketExists(long id)
        {
            return _context.Tickets.Any(e => e.TicketId == id);
        }
        // Metodă privată pentru a obține ID-ul utilizatorului curent
        private long GetCurrentUserId()
        {
            return long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        }
    }
}
