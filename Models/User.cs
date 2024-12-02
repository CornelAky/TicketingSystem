namespace TicketingSystem.Models
{
    public class User
    {
        public long UserID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } 
        public string Role { get; set; } 
        public virtual List<Ticket> CreatedTickets { get; set; }
        public virtual List<Ticket> AssignedTickets { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Notification> Notifications { get; set; }
        public virtual List<Subscription> Subscriptions { get; set; }

        public enum eRoles
        {
            Admin,
            User,
            Manager,
            Support
        }
    }
}
