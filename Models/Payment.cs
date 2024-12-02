using System.ComponentModel.DataAnnotations.Schema;

namespace TicketingSystem.Models
{
    public class Payment
    {
        public long PaymentId { get; set; }
        public long SubscriptionId { get; set; }
        public virtual Subscription Subscription { get; set; }
        public double Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        [NotMapped]
        public string PlanName { get; set; }
        [NotMapped]
        public string UserName { get; set; }
    }
}
