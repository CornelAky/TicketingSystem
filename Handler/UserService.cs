using TicketingSystem.DataAccess;
using TicketingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace TicketingSystem.Handler
{
    public class UserService
    {
        private readonly AppContextDB _context;

        public UserService(AppContextDB context)
        {
            _context = context;
        }

        public async Task RegisterOrUpdateUserAsync(string azureId, string name)
        {
            // Verificăm dacă utilizatorul există deja în baza de date
            var user = await _context.Users.FirstOrDefaultAsync(u => u.AzureID == azureId);

            if (user == null)
            {
                // Dacă utilizatorul nu există, îl creăm
                user = new User
                {
                    AzureID = azureId,
                    FullName = name,
                    //DateCreated = DateTime.UtcNow // Poți adăuga și alte câmpuri
                };
                _context.Users.Add(user);
            }
            else
            {
                // Dacă utilizatorul există, facem update
                user.FullName = name;
                //user.DateUpdated = DateTime.UtcNow; // Poți adăuga alte câmpuri pentru tracking
                _context.Users.Update(user);
            }

            // Salvăm modificările în baza de date
            await _context.SaveChangesAsync();
        }
    }

}
