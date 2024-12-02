using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TicketingSystem.Models;
using TicketingSystem.Handler;

namespace TicketingSystem.Controllers
{
    [Authorize] // Toate metodele necesită autentificare, cu excepția celor marcate cu [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Pagina principală este accesibilă pentru toți utilizatorii
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        // Pagina de confidențialitate este accesibilă pentru toți utilizatorii
        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        //// Afișează informații despre utilizatorul autentificat
        //public IActionResult Profile()
        //{
        //    // Folosim CurrentUserService pentru a obține informații despre utilizatorul curent
        //    var currentUser = _currentUserService.GetCurrentUser();
        //    if (currentUser == null)
        //    {
        //        return RedirectToAction("Index");
        //    }

        //    return View(currentUser); // Trimite obiectul utilizatorului către View
        //}

        // Metoda de gestionare a erorilor
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]    
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
