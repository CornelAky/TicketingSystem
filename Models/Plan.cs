using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;     

namespace TicketingSystem.Models
{
    public class Plan
    {
        public long PlanId { get; set; }
        [Required(ErrorMessage = "PlanName is required")]
        [StringLength(100, ErrorMessage = "PlanName cannot be longer than 100 characters")]
        public string PlanName { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(1, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public double Price { get; set; }

        [Required(ErrorMessage = "MaxTicketsPerMonth is required")]
        [Range(1, int.MaxValue, ErrorMessage = "MaxTicketsPerMonth must be greater than 0")]
        public int MaxTicketsPerMonth { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
        public string Description { get; set; }
        [NotMapped]
        public virtual List<Subscription> Subscriptions { get; set; }
    }
}
