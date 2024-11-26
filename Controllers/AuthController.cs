using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using System;
using System.Security.Claims;

using TicketingSystem.DataAccess;
using TicketingSystem.Models;

namespace TicketingSystem.Controllers;
public class AuthController : Controller
{
    private readonly AppContextDB _context;

    public AuthController(AppContextDB context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        // Găsește utilizatorul în baza de date
        var user = _context.Users.FirstOrDefault(u => u.Email == email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            ModelState.AddModelError(string.Empty, "Invalid username or password");
            return View();
        }

        // Creează lista de claims
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
            new Claim(ClaimTypes.Name, user.FullName.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        // Creează identitatea utilizatorului
        var claimsIdentity = new ClaimsIdentity(claims, "CustomCookieAuth");

        // Creează ticket-ul de autentificare
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true, // Păstrează cookie-ul între sesiuni
        };

        await HttpContext.SignInAsync("CustomCookieAuth", new ClaimsPrincipal(claimsIdentity), authProperties);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("CustomCookieAuth");
        return RedirectToAction("Login", "Auth");
    }


    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(Models.ViewModel.RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Verifică dacă utilizatorul sau email-ul există deja
        if (_context.Users.Any(u => u.FullName == model.FullName || u.Email == model.Email))
        {
            ModelState.AddModelError(string.Empty, "Full name or email already exists");
            return View(model);
        }

        // Hash-uiește parola utilizatorului
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

        // Creează un nou utilizator
        var user = new User
        {
            FullName = model.FullName,
            Email = model.Email,
            PasswordHash = hashedPassword,
            Role = "User" // Setează rolul implicit
        };

        // Salvează utilizatorul în baza de date
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Redirecționează utilizatorul către pagina de autentificare
        return RedirectToAction("Login", "Auth");
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}
