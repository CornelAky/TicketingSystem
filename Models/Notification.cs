using System.ComponentModel.DataAnnotations.Schema;

namespace TicketingSystem.Models
{
    public class Notification
    {
        public long NotificationId { get; set; }
        public long UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Status { get; set; } // Enum
        [NotMapped]
        public User User { get; set; }
    }
}
