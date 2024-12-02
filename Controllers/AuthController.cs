using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

using TicketingSystem.DataAccess;
using TicketingSystem.Models;

namespace TicketingSystem.Controllers;

[AllowAnonymous]
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
    public async Task<IActionResult> Login(string email, string password, bool rememberMe)
    {
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
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email)
        };

        // Adaugă rolurile utilizatorului
        if (!string.IsNullOrEmpty(user.Role))
        {
            var roles = user.Role.Split(';');
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Trim()));
            }
        }

        var claimsIdentity = new ClaimsIdentity(claims, "CustomCookieAuth");

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = rememberMe,
            ExpiresUtc = rememberMe ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddMinutes(30)
        };

        await HttpContext.SignInAsync("CustomCookieAuth", new ClaimsPrincipal(claimsIdentity), authProperties);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
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

        if (_context.Users.Any(u => u.Email == model.Email))
        {
            ModelState.AddModelError(string.Empty, "Email already exists");
            return View(model);
        }

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

        var user = new User
        {
            FullName = model.FullName,
            Email = model.Email,
            PasswordHash = hashedPassword,
            Role = "User" // Setare implicită de rol
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return RedirectToAction("Login", "Auth");
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult ManageRoles()
    {
        var users = _context.Users.ToList();

        // Trimite enum-ul eRoles la View pentru dropdown
        ViewBag.AvailableRoles = Enum.GetValues(typeof(User.eRoles)).Cast<User.eRoles>().Select(r => r.ToString()).ToList();

        return View(users);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SetRoles(long userId, List<string> selectedRoles)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserID == userId);

        if (user == null)
        {
            return NotFound();
        }

        // Combina rolurile selectate într-un șir separat prin `;`
        user.Role = string.Join(";", selectedRoles);
        _context.Update(user);
        await _context.SaveChangesAsync();

        return RedirectToAction("ManageRoles");
    }
    
}
