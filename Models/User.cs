namespace TicketingSystem.Models
{
    public class User
    {
        public long UserID { get; set; }
        public string FullName { get; set; }
        public string AzureID{ get; set; }
        public eRole Role{ get; set; }
        public virtual List<Ticket> CreatedTickets { get; set; }
        public virtual List<Ticket> AssignedTickets { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Notification> Notifications { get; set; }
        public virtual List<Subscription> Subscriptions { get; set; }
        public enum eRole
        {
            None = 0,   
            Administator = 1,   
            Agent = 2,   
            Client = 3,   
        }
    }
}
