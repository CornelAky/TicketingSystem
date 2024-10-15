namespace TicketingSystem.Models
{
    public class Plan
    {
        public long PlanId { get; set; }
        public string PlanName { get; set; }
        public double Price { get; set; }
        public int MaxTicketsPerMonth { get; set; }
        public string Description { get; set; }
        public List<Subscription> Subscriptions { get; set; }
    }
}
