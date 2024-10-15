namespace TicketingSystem.Models
{
    public class Subscription
    {
        public long SubscriptionId { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public long PlanId { get; set; }
        public Plan Plan { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public List<Payment> Payments { get; set; }
    }
}
